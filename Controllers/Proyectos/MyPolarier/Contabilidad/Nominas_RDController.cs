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
    public class Nominas_RDController : ODataController
    {
        private readonly bdERP db;

        public Nominas_RDController(bdERP context)
        {
            db = context;
        }


        [HttpPost("odata/MyPolarier/Contabilidad/Nominas_RD/ImportarRegalias({fecha})")]
        [Authorize]
        public async Task<ActionResult> ImportarRegalias(DateTime fecha)
        {

            if (Request.Form.Files.Count == 0)
                return BadRequest("No se ha adjuntado ningún archivo");

            foreach (var file in Request.Form.Files)
            {
                if (file.Length == 0)
                    return BadRequest("Uno de los archivos está vacío");
                else if (file.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    return BadRequest("Uno de los archivos no es un archivo de Excel");
            }

            var nominas = new List<tblNomina_RD>();
            var personasNoEncontradas = new List<dynamic>();

            Date inicioPeriodo = new Date(fecha.Year, 1, 1);
            Date finPeriodo = new Date(fecha.Year, 12, 31);

            foreach (var file in Request.Form.Files)
            {
                try
                {
                    using var stream = new MemoryStream();
                    await file.CopyToAsync(stream);
                    using var workbook = new XLWorkbook(stream);
                    var worksheet = workbook.Worksheet("Nómina"); // Obtén la primera hoja de trabajo

                    var tblPersonas_RD = db.tblPersona
                        .Include(x => x.idTipoTrabajoNavigation)
                            .ThenInclude(x => x.tblCuentaContableNTipoTrabajo)
                        .Include(x => x.idCentroTrabajoNavigation)
                            .ThenInclude(x => x.tblCuentaContableNCentroTrabajo)
                        .Where(x => x.id_VIPS != null)
                        .ToDictionary(x => (int)x.id_VIPS, x => x);

                    // Busca la fila de resumen para filtrar el apartado duplicado de Estructura

                    var rowsNomina = worksheet
                        .RowsUsed()
                        .Where(x => int.TryParse(x.Cell(1).Value.ToString(), out int id_VIPS))
                        .ToList();

                    personasNoEncontradas = rowsNomina
                        .Where(x => !tblPersonas_RD.ContainsKey(int.Parse(x.Cell(1).Value.ToString())))
                        .Select(x => new
                        {
                            id_VIPS = int.Parse(x.Cell(1).Value.ToString()),
                            nombreCompleto = x.Cell(1).Value.ToString()
                        }).ToDynamicList();

                    if (personasNoEncontradas.Count > 0)
                    {
                        return Ok(personasNoEncontradas);
                    }

                    nominas.AddRange(rowsNomina
                        .Where(x => tblPersonas_RD.ContainsKey(int.Parse(x.Cell(1).Value.ToString())))
                        .Select(x =>
                        {
                            var pers = tblPersonas_RD[int.Parse(x.Cell(1).Value.ToString())];
                            return new tblNomina_RD
                            {
                                inicioPeriodo = inicioPeriodo,
                                finPeriodo = finPeriodo,
                                idPersona = pers.idPersona,
                                nombreCompleto = (pers.nombre + " " + pers.apellidos).ToUpper(),
                                idAdmCentroCoste = pers.idAdmCentroCoste,
                                idAdmElementoPEP = pers.idAdmElementoPEP,
                                idAdmCuentaContable = pers.idAdmCuentaContable_Salario,
                                idTipoNomina_RD = (short)TipoNominaRD.Regalia,
                                gratificacion = ToDecimal(x.Cell(21).Value), // Total ingresos
                                prestamoCoop = ToDecimal(x.Cell(36).Value), // Total descuentos 
                                neto = ToDecimal(x.Cell(37).Value), // Neto
                            };
                        }).ToList());
                } catch (Exception e)
                {
                    return BadRequest("Ha ocurrido un error mientras se procesaba el archivo");
                }
            }
            

            if (nominas.Count > 0)
            {
                var idsPersona = nominas.Select(x => x.idPersona).ToList();
                var nominasExistentes = db.tblNomina_RD
                    .Include(x => x.tblHistoricoAsientoNomina_RD)
                    .Where(x =>
                        idsPersona.Contains(x.idPersona) &&
                        x.inicioPeriodo == inicioPeriodo &&
                        x.finPeriodo == finPeriodo &&
                        x.idTipoNomina_RD == (short)TipoNominaRD.Regalia
                    ).ToList();

                db.tblHistoricoAsientoNomina_RD.RemoveRange(nominasExistentes.SelectMany(x => x.tblHistoricoAsientoNomina_RD));
                db.tblNomina_RD.RemoveRange(nominasExistentes);

                db.tblNomina_RD.AddRange(nominas);
                await db.SaveChangesAsync();
            }
            return Ok($"Nóminas insertadas/actualizadas: {nominas.Count}");
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
