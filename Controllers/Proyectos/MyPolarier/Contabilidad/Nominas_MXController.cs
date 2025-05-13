using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using System.Linq.Dynamic.Core;
using WebApiCore.Context;
using WebApiCore.Enums.RRHH;
using WebApiCore.Security;

namespace WebApiCore.Controllers.Proyectos.MyPolarier.RRHH
{
    public class Nominas_MXController : ODataController
    {
        private readonly bdERP db;

        public Nominas_MXController(bdERP context)
        {
            db = context;
        }

        [HttpPatch("odata/MyPolarier/Contabilidad/Nominas_MX/PatchIdPersona")]
        [Authorize]
        public async Task<ActionResult> PatchIdPersona([FromODataUri] int? idPersona, [FromODataUri] int id_MX)
        {
            var tblPersona = db.tblPersona.Where(x => x.id_MX == id_MX).ToList();

            foreach (var persona in tblPersona)
            {
                persona.id_MX = null;
            }

            if (idPersona != null)
            {
                var persona = db.tblPersona.Find(idPersona);
                persona.id_MX = id_MX;
            }

            await db.SaveChangesAsync();

            return Ok(true);
        }


        [HttpPost("odata/MyPolarier/Contabilidad/Nominas_MX/ImportarNominas({fecha},{tipoNomina})")]
        [Authorize]
        public async Task<ActionResult> ImportarNominas(DateTime fecha, TipoNominaMX tipoNomina)
        {

            if (Request.Form.Files.Count == 0)
                return BadRequest("No se ha adjuntado ningún archivo");
            else if (Request.Form.Files.Count > 1)
                return BadRequest("Solo se puede adjuntar un archivo");

            var file = Request.Form.Files.First();

            if (file.Length == 0)
                return BadRequest("El archivo está vacío");
            else if (file.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                return BadRequest("El archivo no es un archivo de Excel");

            var nominas = new List<tblNomina_MX>();
            var personasNoEncontradas = new List<dynamic>();

            Date inicioPeriodo = new();
            Date finPeriodo = new();
            if (TipoPagaMXUtils.isPagaQ1(tipoNomina))
            {
                inicioPeriodo = new Date(fecha.Year, fecha.Month, 1);
                finPeriodo = new Date(fecha.Year, fecha.Month, 15);
            }
            else if (TipoPagaMXUtils.isPagaQ2(tipoNomina))
            {
                inicioPeriodo = new Date(fecha.Year, fecha.Month, 16);
                finPeriodo = new Date(fecha.Year, fecha.Month, DateTime.DaysInMonth(fecha.Year, fecha.Month));
            }
            else
            {
                inicioPeriodo = new Date(fecha.Year, fecha.Month, 1);
                finPeriodo = new Date(fecha.Year, fecha.Month, DateTime.DaysInMonth(fecha.Year, fecha.Month));
            }

            try
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    using var workbook = new XLWorkbook(stream);
                    var worksheet = workbook.Worksheet("Informe"); // Obtén la primera hoja de trabajo

                    var tblPersonas_MX = db.tblPersona
                        .Include(x => x.idTipoTrabajoNavigation)
                            .ThenInclude(x => x.tblCuentaContableNTipoTrabajo)
                        .Include(x => x.idCentroTrabajoNavigation)
                            .ThenInclude(x => x.tblCuentaContableNCentroTrabajo)
                        .Where(x => x.id_MX != null)
                        .ToDictionary(x => (int)x.id_MX, x => x);

                    // Busca la fila de resumen para filtrar el apartado duplicado de Estructura
                    var summaryRowNumber = worksheet
                        .RowsUsed().Skip(2)
                        .FirstOrDefault(x => x.Cell(1).Value.ToString() == "GRAN TOTAL")?
                        .RowNumber()
                        ?? worksheet.RowCount();

                    var rowsNomina = worksheet
                        .RowsUsed().Skip(2)
                        .Where(x => int.TryParse(x.Cell(1).Value.ToString(), out int id_MX) && x.RowNumber() < summaryRowNumber)
                        .ToList();

                    personasNoEncontradas = rowsNomina
                        .Where(x => !tblPersonas_MX.ContainsKey(int.Parse(x.Cell(1).Value.ToString())))
                        .Select(x => new
                        {
                            id_MX = int.Parse(x.Cell(1).Value.ToString()),
                            nombreCompleto = x.Cell(2).Value.ToString()
                        }).ToDynamicList();

                    if (personasNoEncontradas.Count > 0)
                    {
                        return Ok(personasNoEncontradas);
                    }

                    nominas = rowsNomina
                        .Where(x => tblPersonas_MX.ContainsKey(int.Parse(x.Cell(1).Value.ToString())))
                        .Select(x =>
                        {
                            var pers = tblPersonas_MX[int.Parse(x.Cell(1).Value.ToString())];
                            return new tblNomina_MX
                            {
                                idPersona = pers.idPersona,
                                idTipoNomina_MX = (short)tipoNomina,
                                inicioPeriodo = inicioPeriodo,
                                finPeriodo = finPeriodo,
                                nombreCompleto = x.Cell(2).Value.ToString(),
                                idAdmCentroCoste = pers.idAdmCentroCoste,
                                idAdmElementoPEP = pers.idAdmElementoPEP,
                                idAdmCuentaContable_Sueldo_MX = pers.idLavanderia != null ? 
                                    pers.idTipoTrabajoNavigation.tblCuentaContableNTipoTrabajo.idAdmCuentaContable_Sueldo_MX 
                                    : pers.idCentroTrabajoNavigation.tblCuentaContableNCentroTrabajo.idAdmCuentaContable_Sueldo_MX,
                                idAdmCuentaContable_IMSS_MX = pers.idLavanderia != null ? 
                                    pers.idTipoTrabajoNavigation.tblCuentaContableNTipoTrabajo.idAdmCuentaContable_IMSS_MX 
                                    : pers.idCentroTrabajoNavigation.tblCuentaContableNCentroTrabajo.idAdmCuentaContable_IMSS_MX,
                                idAdmCuentaContable_INFONAVIT_MX= pers.idLavanderia != null ? 
                                    pers.idTipoTrabajoNavigation.tblCuentaContableNTipoTrabajo.idAdmCuentaContable_INFONAVIT_MX 
                                    : pers.idCentroTrabajoNavigation.tblCuentaContableNCentroTrabajo.idAdmCuentaContable_INFONAVIT_MX,
                                idAdmCuentaContable_SAR_MX = pers.idLavanderia != null ? 
                                    pers.idTipoTrabajoNavigation.tblCuentaContableNTipoTrabajo.idAdmCuentaContable_SAR_MX 
                                    : pers.idCentroTrabajoNavigation.tblCuentaContableNCentroTrabajo.idAdmCuentaContable_SAR_MX,
                                idAdmCuentaContable_ImpEstatalNominas_MX = pers.idLavanderia != null ? 
                                    pers.idTipoTrabajoNavigation.tblCuentaContableNTipoTrabajo.idAdmCuentaContable_ImpEstatalNominas_MX 
                                    : pers.idCentroTrabajoNavigation.tblCuentaContableNCentroTrabajo.idAdmCuentaContable_ImpEstatalNominas_MX,
                                contabilizado = false,
                                idTipoTrabajo = pers.idTipoTrabajo,
                                sueldo = ToDecimal(x.Cell(3).Value),
                                horasExtras = ToDecimal(x.Cell(4).Value),
                                primaVacacional = ToDecimal(x.Cell(5).Value),
                                primaDominical = ToDecimal(x.Cell(6).Value),
                                bono = ToDecimal(x.Cell(7).Value),
                                descansoTrabajado = ToDecimal(x.Cell(8).Value),
                                aguinaldo = ToDecimal(x.Cell(9).Value),
                                valesDespensa = ToDecimal(x.Cell(10).Value),
                                fondoAhorro = ToDecimal(x.Cell(11).Value),
                                otraPercepcion = ToDecimal(x.Cell(12).Value),
                                gastosSindicales = ToDecimal(x.Cell(13).Value),
                                PTU = ToDecimal(x.Cell(14).Value),
                                infonavitEmpleado = ToDecimal(x.Cell(15).Value),
                                fonacotEmpleado = ToDecimal(x.Cell(16).Value),
                                IMSSEmpleado = ToDecimal(x.Cell(17).Value),
                                SAREmpleado = ToDecimal(x.Cell(18).Value),
                                ISREmpleado = ToDecimal(x.Cell(19).Value),
                                subsidioEmpleo = ToDecimal(x.Cell(20).Value),
                                devolucionPrestamo = ToDecimal(x.Cell(21).Value),
                                otrasDeducciones = ToDecimal(x.Cell(22).Value),
                                descAlimentos = ToDecimal(x.Cell(23).Value),
                                totalDeducciones = ToDecimal(x.Cell(24).Value),
                                totalSP = ToDecimal(x.Cell(25).Value),
                                totalSV = ToDecimal(x.Cell(26).Value),
                                percepcionNeta = ToDecimal(x.Cell(27).Value),
                                IMSSPatronal = ToDecimal(x.Cell(28).Value),
                                infonavitPatronal = ToDecimal(x.Cell(29).Value),
                                SARPatronal = ToDecimal(x.Cell(30).Value),
                                impuestoEstatalSobreNominas = ToDecimal(x.Cell(31).Value),
                            };
                        }).ToList();
                }
            } catch (Exception e)
            {
                return BadRequest("Ha ocurrido un error mientras se procesaba el archivo");
            }

            if (nominas.Count > 0)
            {
                var idsPersona = nominas.Select(x => x.idPersona).ToList();
                var nominasExistentes = db.tblNomina_MX
                    .Include(x => x.tblHistoricoAsientoNomina_MX)
                    .Where(x =>
                        idsPersona.Contains(x.idPersona) &&
                        x.inicioPeriodo == inicioPeriodo &&
                        x.finPeriodo == finPeriodo &&
                        x.idTipoNomina_MX == (short)tipoNomina
                    ).ToList();

                db.tblHistoricoAsientoNomina_MX.RemoveRange(nominasExistentes.SelectMany(x => x.tblHistoricoAsientoNomina_MX));
                db.tblNomina_MX.RemoveRange(nominasExistentes);

                db.tblNomina_MX.AddRange(nominas);
                await db.SaveChangesAsync();
            }
            return Ok(personasNoEncontradas);
        }

        private static decimal ToDecimal(XLCellValue celValue)
        {
            return decimal.TryParse(
                celValue.ToString().Replace(',', '.'), 
                System.Globalization.NumberStyles.Currency, 
                System.Globalization.CultureInfo.InvariantCulture, 
                out decimal value
            ) ? value : 0;
        }
    }
}
