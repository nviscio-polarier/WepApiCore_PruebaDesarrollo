using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Newtonsoft.Json;
using System.Linq.Dynamic.Core;
using System.Text;
using WebApiCore.Class;
using WebApiCore.Class.bdERP.Administracion;
using WebApiCore.Class.ftp;
using WebApiCore.Context;
using WebApiCore.Enums.Assistant;
using WebApiCore.Enums.General;
using WebApiCore.Enums.RRHH;
using WebApiCore.Hubs;
using WebApiCore.Security;
using static WebApiCore.Controllers.Proyectos.MyPolarier.RRHH.GestionNominasController;

namespace WebApiCore.Controllers.Proyectos.MyPolarier.RRHH
{
    public class AsientosNominasController : ODataController
    {
        private readonly bdERP db;
        private readonly tblNominaController nc;

        public AsientosNominasController(bdERP context, IHubContext<NotificacionesHub> _hubContext)
        {
            db = context;
            nc = new(db, _hubContext);
        }

        #region España

        [HttpGet("odata/MyPolarier/Contabilidad/AsientosNominas/GetHistorialNominas")]
        [Authorize]
        public ActionResult GetHistorialNominas([FromODataUri] short idEmpresaPolarier, [FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta)
        {
            return Ok(GetDatosNominas(idEmpresaPolarier, fechaDesde, fechaHasta));
        }

        [HttpPost("odata/MyPolarier/Contabilidad/AsientosNominas/GenerarAsientosNominasSAP")]
        [Authorize]
        public async Task<ActionResult> GenerarAsientosNominasSAP([FromODataUri] short idEmpresaPolarier, [FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta, [FromODataUri] int? idElementoPEP, [FromODataUri] int? idCentroCoste, [FromODataUri] DateTime fechaAsiento)
        {
            var idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

            if (idEmpresaPolarier == 0)
                return BadRequest("No se ha específicado empresa");

            if (idElementoPEP == null && idCentroCoste == null)
                return BadRequest("No se ha específicado elemento PEP ni centro de coste");

            var objElementoPEP = db.tblAdmElementoPEP
                .Include(x => x.idAdmCentroBeneficioNavigation)
                .Include(x => x.idEmpresaPolarierNavigation)
                    .ThenInclude(x => x.idMonedaNavigation)
                .FirstOrDefault(x => x.idAdmElementoPEP == idElementoPEP);
            var objCentroCoste = db.tblAdmCentroCoste
                .Include(x => x.idEmpresaPolarierNavigation)
                    .ThenInclude(x => x.idMonedaNavigation)
                .FirstOrDefault(x => x.idAdmCentroCoste == idCentroCoste);

            var datosNominas = GetDatosNominas(idEmpresaPolarier, fechaDesde, fechaHasta)
                .Where(x => x.idElementoPEP == idElementoPEP && x.idCentroCoste == idCentroCoste)
                .ToList();

            GenerarHistoricoAsientoNomina(ref datosNominas, idUsuario, fechaAsiento);

            if (datosNominas.All(dn => dn.idEstadoHistoricoAsientoNomina >= (byte)idsEstadoHistoricoAsientoNomina.Contabilizado && !dn.isRetenida))
            {
                var idsNominasContabilizadas = datosNominas
                    .Where(nc => nc.idElementoPEP == idElementoPEP && nc.idCentroCoste == idCentroCoste).Select(nc => nc.idNomina);

                var datosNominasGrouped = datosNominas.GroupBy(x => new { Key = objElementoPEP != null ? x.idTipoTrabajo : x.idCentroCoste, x.idCuentaContable_Salario, x.idCuentaContable_SSEmpresa });

                List<EntradaAsientoContable> entradas = new();

                var empresa = objElementoPEP?.idEmpresaPolarierNavigation ?? objCentroCoste?.idEmpresaPolarierNavigation;

                string sociedad = empresa?.companyCode_SAP ?? "";
                string moneda = empresa?.idMonedaNavigation.codigo ?? "";
                string textoCabecera = String.Concat(("NOM." + objCentroCoste?.denominacion).ToUpper().Trim().Take(25));
                string claseDocumento = "SA";
                string cuentaContable_Salario = "";
                string cuentaContable_SSEmpresa = "";
                string centroCoste = objCentroCoste != null ? objCentroCoste.codigo : "";
                string centroBeneficio = objElementoPEP != null ? objElementoPEP.idAdmCentroBeneficioNavigation.codigo : "";
                string pep = objElementoPEP != null ? objElementoPEP.codigo : "";

                void AddEntradaAsientoContable(string referencia, decimal? importe, string cuentaContable)
                {
                    entradas.Add(new EntradaAsientoContable
                    {
                        IDApunte = 1,
                        Sociedad = sociedad,
                        ClaseDocumento = "SA",
                        Referencia1 = referencia,
                        TextoCabecera = textoCabecera,
                        FechaFactura = fechaAsiento.ToString("dd.MM.yyyy"),
                        FechaContable = fechaAsiento.ToString("dd.MM.yyyy"),
                        FeDeclImpt = fechaAsiento.ToString("dd.MM.yyyy"), // Todo: Revisar fecha
                        FeCumpImpt = fechaAsiento.ToString("dd.MM.yyyy"), // Todo: Revisar fecha
                        CuentaContable = cuentaContable,
                        ImporteTransaccion = importe ?? 0,
                        MonedaTransaccion = moneda,
                        ImporteSociedad = importe ?? 0,
                        MonedaSociedad = moneda,
                        Centrocoste = centroCoste,
                        Centrobeneficio = centroBeneficio,
                        Pep = pep
                    });
                }

                var tblAdmCuentaContable = db.tblAdmCuentaContable
                    .Where(x => datosNominasGrouped.Select(x => x.Key.idCuentaContable_Salario).Contains(x.idAdmCuentaContable) ||
                                datosNominasGrouped.Select(x => x.Key.idCuentaContable_SSEmpresa).Contains(x.idAdmCuentaContable)
                    ).ToList();

                var tblTipoTrabajo = db.tblTipoTrabajo
                    .Where(x => datosNominasGrouped.Select(x => x.Key.Key).Contains(x.idTipoTrabajo))
                    .ToList();

                foreach (var item in datosNominasGrouped)
                {

                    cuentaContable_Salario = tblAdmCuentaContable.FirstOrDefault(x => x.idAdmCuentaContable == item.Key.idCuentaContable_Salario)?.codigo ?? "";
                    cuentaContable_SSEmpresa = tblAdmCuentaContable.FirstOrDefault(x => x.idAdmCuentaContable == item.Key.idCuentaContable_SSEmpresa)?.codigo ?? "";

                    var conceptos = new List<(string concepto, decimal? suma, string? cuentaContable)>
                {
                    {("SUELDOS Y SALARIOS", item.Sum(x => x.salarioBruto - x.conceptoNEspecie - (x.absentismo ?? 0)), (string?)cuentaContable_Salario)},
                    {("SEGURO CONVENIO COLECTIVO", item.Sum(x => x.seguroConvenio), (string ?) cuentaContable_Salario)},
                    {("SEGURO CONVENIO COLECTIVO", -item.Sum(x => x.seguroConvenio), (string ?) "46000001")},
                    {("IRPF EMPLEADOS", -item.Sum(x => x.tributacionIRPF), (string ?) "47510001")},
                    {("SEG. SOCIAL EMPLEADOS", -item.Sum(x => x.segSocialTrabajador), (string ?) "47600001")},
                    {("REMUNERACIONES PTES. PAGO", -item.Sum(x => x.liquidoPercibir), (string ?) "46500001")},
                    {("EMBARGO", -item.Sum(x => x.embargo), (string ?)"46500001")},
                    {("ANTICIPO DE REMUNERACIONES", -item.Sum(x => x.anticipo), (string ?) "46000002")},
                    {("SEG. SOCIAL EMPRESA", item.Sum(x => x.segSocialEmpresa), (string ?) cuentaContable_SSEmpresa)},
                    {("SEG. SOCIAL EMPRESA", -item.Sum(x => x.segSocialEmpresa), (string ?) "47600002")},
                    {("DESC. PREAVISO", -item.Sum(x => x.descuentoPreaviso + x.plusAsistenciaMesAnterior), (string ?) "75800002")},
                    {("OTROS GASTOS PERSONAL", item.Sum(x => x.tributacionEspeciesEmpresa), (string ?) "64900002")},
                    {("HªPª ACR IRPF EMPRESA", -item.Sum(x => x.tributacionEspeciesEmpresa), (string ?) "47510027")},
                };

                    // 42 = ES04.001.1 = LUIS PRADILLA
                    // 53 = ES04.021.1 = DAVID MAY
                    if (idCentroCoste == 42 || idCentroCoste == 53)
                    {
                        conceptos.AddRange(new List<(string concepto, decimal? suma, string? cuentaContable)>
                    {
                        {("SALARIO EN ESPECIE", item.Sum(x => x.salarioEspecie), (string ?) "64000001")},
                        {("SALARIO EN ESPECIE", -item.Sum(x => x.salarioEspecie), (string ?) "75500001")},
                    });
                    }

                    if (objElementoPEP != null)
                    {
                        var tipoTrabajo = tblTipoTrabajo.FirstOrDefault(x => x.idTipoTrabajo == item.Key.Key);

                        textoCabecera = String.Concat(("NOM." + tipoTrabajo.denominacion).ToUpper().Trim().Take(25));
                    }

                    foreach (var concepto in conceptos)
                        AddEntradaAsientoContable(
                            concepto.concepto,
                            concepto.suma,
                            concepto.cuentaContable
                            );

                }

                var csv = EntradaAsientoContable.GenerateCSV(entradas);
                var objStream = new MemoryStream(Encoding.GetEncoding("ISO-8859-1").GetBytes(csv));
                var nombreArchivo = "asientoNominas_" + (objElementoPEP != null ? $"PEP_{idElementoPEP}" : $"CC_{idCentroCoste}") + "_" + fechaAsiento.ToString("yyyy-MM") + ".csv";

                var ftp = new FTPConnection();
                var ftpResponse = await ftp.UploadFile(nombreArchivo, objStream.ToArray());
                var status = ftpResponse.StatusCode == System.Net.FtpStatusCode.ClosingData;

                if (status)
                {
                    await db.SaveChangesAsync();
                }

                return Ok(status);
            }

            await db.SaveChangesAsync();

            return Ok(true);
        }
        [HttpPost("odata/MyPolarier/Contabilidad/AsientosNominas/GenerarHistoricoAsientoNomina")]
        [Authorize]
        public async Task<ActionResult> GenerarHistoricoAsientoNomina([FromODataUri] int idNomina, [FromODataUri] DateTime fechaAsiento, [FromODataUri] byte? idEstadoHistoricoAsientoNominaDestino = null, [FromODataUri] bool filtrarFiniquitos = true)
        {
            var idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

            var datosNominas = GetDatosNominas(idNomina).ToList();

            GenerarHistoricoAsientoNomina(ref datosNominas, idUsuario, fechaAsiento, idEstadoHistoricoAsientoNominaDestino, filtrarFiniquitos);

            await db.SaveChangesAsync();

            return Ok(true);
        }

        [HttpPost("odata/MyPolarier/Contabilidad/AsientosNominas/GenerarAnticipo")]
        [Authorize]
        public async Task<ActionResult> GenerarAnticipo([FromODataUri] int idNomina, [FromODataUri] DateTime fechaAsiento)
        {
            var idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

            var nomina = await db.tblNomina.SingleOrDefaultAsync(x => x.idNomina == idNomina);

            if (nomina == null)
            {
                return NotFound();
            }

            var datosNominas = GetDatosNominas(idNomina).ToList();

            var historicoActual = datosNominas.FirstOrDefault();

            var ultimoHistoricoAsientoNomina = await db.tblHistoricoAsientoNomina
                .Where(han => han.idNomina == idNomina)
                .GroupBy(han => han.idNomina)
                .Select(g => g.OrderByDescending(han => han.fecha).FirstOrDefault())
                .FirstOrDefaultAsync();

            var anteriorLiquidoPercibir = ultimoHistoricoAsientoNomina?.liquidoPercibir ?? 0;

            if (historicoActual == null || anteriorLiquidoPercibir <= historicoActual.liquidoPercibir)
            {
                return BadRequest();
            }

            DateTime fechaDesdeMesSiguiente = new DateTime(nomina.fechaDesde.Year, nomina.fechaDesde.Month, 1).AddMonths(1);
            DateTime fechaHastaMesSiguiente = fechaDesdeMesSiguiente.AddMonths(1).AddDays(-1);

            var nominaMesSiguiente = db.tblNomina
                .Include(n => n.tblConceptoNominaNNomina)
                .FirstOrDefault(n => n.idPersona == nomina.idPersona && n.fechaDesde >= fechaDesdeMesSiguiente && n.fechaHasta <= fechaHastaMesSiguiente);

            var importe = anteriorLiquidoPercibir - historicoActual.liquidoPercibir;

            if (nominaMesSiguiente == null)
            {
                nominaMesSiguiente ??= nc.GetNewNomina(nomina.idPersona, fechaDesdeMesSiguiente, fechaHastaMesSiguiente, idUsuario);

                if (nominaMesSiguiente == null)
                {
                    return BadRequest();
                }

                nominaMesSiguiente.tblConceptoNominaNNomina.Add(GetAnticipo(nominaMesSiguiente));

                db.tblNomina.Add(nominaMesSiguiente);
            }
            else
            {
                var anticipoMesSiguiente = nominaMesSiguiente.tblConceptoNominaNNomina.FirstOrDefault(cnnn => cnnn.idConceptoNomina == (short)idsConceptoNomina.Anticipo);

                if (anticipoMesSiguiente == null)
                {
                    nominaMesSiguiente.tblConceptoNominaNNomina.Add(GetAnticipo(nominaMesSiguiente));
                }
                else
                {
                    anticipoMesSiguiente.fecha_validacion = DateTimeOffset.UtcNow;
                    anticipoMesSiguiente.idUsuario_validacion = idUsuario;
                    anticipoMesSiguiente.observaciones = $"{anticipoMesSiguiente.observaciones}\n- Generado automáticamente desde Asientos de nóminas por importe de {importe}";
                    anticipoMesSiguiente.precioUnitario += importe;
                    anticipoMesSiguiente.importe += importe;
                }
            }

            GenerarHistoricoAsientoNomina(ref datosNominas, idUsuario, fechaAsiento, (byte)idsEstadoHistoricoAsientoNomina.Anticipo, false);

            await db.SaveChangesAsync();

            return Ok(true);

            tblConceptoNominaNNomina GetAnticipo(tblNomina nomina)
            {
                return new()
                {
                    idConceptoNomina = (short)idsConceptoNomina.Anticipo,
                    cantidad = 1,
                    precioUnitario = importe,
                    importe = importe,
                    fecha_validacion = DateTimeOffset.UtcNow,
                    fecha = fechaDesdeMesSiguiente,
                    idUsuario_validacion = idUsuario,
                    observaciones = $"- Generado automáticamente desde Asientos de nóminas por importe de {importe}"
                };
            }
        }

        public void GenerarHistoricoAsientoNomina(List<int> idsNomina, DateTime fechaAsiento, int idUsuario, byte? idEstadoHistoricoAsientoNominaDestino = null, bool filtrarFiniquitos = true)
        {
            var datosNominas = GetDatosNominas(idsNomina).ToList();

            GenerarHistoricoAsientoNomina(ref datosNominas, idUsuario, fechaAsiento, idEstadoHistoricoAsientoNominaDestino, filtrarFiniquitos);
        }

        private IEnumerable<HistoricoNominaContabilidad> GetDatosNominas(int idNomina)
        {
            var tblNomina = db.tblNomina
                .Include(n => n.tblConceptoNominaNNomina)
                .Include(n => n.tblConceptoNominaNNomina_Gestoria)
                .Where(n => n.idNomina == idNomina)
                .ToList();

            return GetDatosNominas(tblNomina);
        }

        private IEnumerable<HistoricoNominaContabilidad> GetDatosNominas(List<int> idsNomina)
        {
            var tblNomina = db.tblNomina
                .Include(n => n.tblConceptoNominaNNomina)
                .Include(n => n.tblConceptoNominaNNomina_Gestoria)
                .Where(n => idsNomina.Contains(n.idNomina))
                .ToList();

            return GetDatosNominas(tblNomina);
        }

        private IEnumerable<HistoricoNominaContabilidad> GetDatosNominas(short idEmpresaPolarier, DateTime fechaDesde, DateTime fechaHasta)
        {
            var tblNomina = db.tblNomina
                .Include(n => n.tblConceptoNominaNNomina)
                .Include(n => n.tblConceptoNominaNNomina_Gestoria)
                .Where(n =>
                    n.idEmpresaPolarier == idEmpresaPolarier
                    && n.fechaDesde >= fechaDesde
                    && n.fechaHasta <= fechaHasta
                )
                .ToList();

            return GetDatosNominas(tblNomina);
        }

        private IEnumerable<HistoricoNominaContabilidad> GetDatosNominas(List<tblNomina> tblNomina)
        {
            short idConceptoNomina_CotFormacionProfesional = 17;
            short idConceptoNomina_TributacionIRPF = 18;
            short idConceptoNomina_ConceptoNEspecie = 35;
            short idConceptoNomina_CotDesempleo = 20;
            short idConceptoNomina_CotRestoHorasExtra = 32;
            short idConceptoNomina_PagaExtra1 = 25;
            short idConceptoNomina_PagaExtra2 = 26;
            short idConceptoNomina_SeguroConvenio = 11;
            short idConceptoNomina_CotMeqEqTrabajador = 40;

            List<short> idsConceptoNomina_CotCC = new() { 21, 53 };

            var pagasMensuales = tblNomina.Where(n => n.fechaBaja == null || n.idMotivoBaja == (byte)idsMotivoBaja.ConversionContrato);

            var isCentroPagado = pagasMensuales
                .GroupBy(x => new { x.idAdmCentroCoste, x.idAdmElementoPEP })
                .Select(g => new
                {
                    g.Key.idAdmCentroCoste,
                    g.Key.idAdmElementoPEP,
                    isPagado = g.Any(x => x.idEstadoHistoricoAsientoNomina != null)
                });

            var ultimoHistoricoAsientoNomina = db.tblHistoricoAsientoNomina
                .Where(han => pagasMensuales.Select(n => n.idNomina).Contains(han.idNomina))
                .GroupBy(han => han.idNomina)
                .Select(g => g.OrderByDescending(han => han.fecha).FirstOrDefault())
                .ToList();

            var results =
                tblNomina
                .Select(n => new HistoricoNominaContabilidad
                {
                    idCentroElem = JsonConvert.SerializeObject(new List<int> { n.idAdmCentroCoste ?? -1, n.idAdmElementoPEP ?? -1 }),
                    idPersona = n.idPersona,
                    nombreCompleto = n.nombreCompleto,

                    idTipoTrabajo = n.idTipoTrabajo,
                    idElementoPEP = n.idAdmElementoPEP,
                    idCentroCoste = n.idAdmCentroCoste,
                    idCuentaContable_Salario = n.idAdmCuentaContable_Salario,
                    idCuentaContable_SSEmpresa = n.idAdmCuentaContable_SSEmpresa,

                    idNomina = n.idNomina,
                    idTipoNomina = n.idTipoNomina,
                    idEstadoNomina = n.idEstadoNomina,
                    idMotivoBaja = n.idMotivoBaja,
                    isRetenida = n.isRetenida,
                    idEstadoHistoricoAsientoNomina = n.idEstadoHistoricoAsientoNomina,
                    fechaDesde = n.fechaDesde,
                    fechaHasta = n.fechaHasta,
                    fechaBaja = n.fechaBaja,
                    fechaCobro = n.fechaHasta,
                    tipoPaga = n.tipoPaga,
                    salarioBruto = n.salarioBruto ?? 0,
                    pagaExtra1 = n.tblConceptoNominaNNomina.FirstOrDefault(x => x.idConceptoNomina == idConceptoNomina_PagaExtra1) == null ? 0 : n.tblConceptoNominaNNomina.FirstOrDefault(x => x.idConceptoNomina == idConceptoNomina_PagaExtra1).importe,
                    pagaExtra2 = n.tblConceptoNominaNNomina.FirstOrDefault(x => x.idConceptoNomina == idConceptoNomina_PagaExtra2) == null ? 0 : n.tblConceptoNominaNNomina.FirstOrDefault(x => x.idConceptoNomina == idConceptoNomina_PagaExtra2).importe,
                    tributacionIRPF = (n.tblConceptoNominaNNomina.FirstOrDefault(x => x.idConceptoNomina == idConceptoNomina_TributacionIRPF) == null ? 0 : n.tblConceptoNominaNNomina.FirstOrDefault(x => x.idConceptoNomina == idConceptoNomina_TributacionIRPF).importe)
                        + (n.tblConceptoNominaNNomina.FirstOrDefault(x => x.idConceptoNomina == (short)idsConceptoNomina.TributacionIRPFIndemnizacion) == null ? 0 : n.tblConceptoNominaNNomina.FirstOrDefault(x => x.idConceptoNomina == (short)idsConceptoNomina.TributacionIRPFIndemnizacion).importe),
                    segSocialTrabajador =
                        (n.tblConceptoNominaNNomina.FirstOrDefault(x => x.idConceptoNomina == idConceptoNomina_CotFormacionProfesional) == null ? 0 : n.tblConceptoNominaNNomina.FirstOrDefault(x => x.idConceptoNomina == idConceptoNomina_CotFormacionProfesional).importe)
                        + (n.tblConceptoNominaNNomina.FirstOrDefault(x => x.idConceptoNomina == idConceptoNomina_CotDesempleo) == null ? 0 : n.tblConceptoNominaNNomina.FirstOrDefault(x => x.idConceptoNomina == idConceptoNomina_CotDesempleo).importe)
                        + (n.tblConceptoNominaNNomina.FirstOrDefault(x => idsConceptoNomina_CotCC.Contains(x.idConceptoNomina)) == null ? 0 : n.tblConceptoNominaNNomina.FirstOrDefault(x => idsConceptoNomina_CotCC.Contains(x.idConceptoNomina)).importe)
                        + (n.tblConceptoNominaNNomina.FirstOrDefault(x => x.idConceptoNomina == idConceptoNomina_CotRestoHorasExtra) == null ? 0 : n.tblConceptoNominaNNomina.FirstOrDefault(x => x.idConceptoNomina == idConceptoNomina_CotRestoHorasExtra).importe)
                        + (n.tblConceptoNominaNNomina.FirstOrDefault(x => x.idConceptoNomina == idConceptoNomina_CotMeqEqTrabajador) == null ? 0 : n.tblConceptoNominaNNomina.FirstOrDefault(x => x.idConceptoNomina == idConceptoNomina_CotMeqEqTrabajador).importe)
                        + (n.tblConceptoNominaNNomina.FirstOrDefault(x => x.idConceptoNomina == (short)idsConceptoNomina.CotizacionAdicionalSolidaridad) == null ? 0 : n.tblConceptoNominaNNomina.FirstOrDefault(x => x.idConceptoNomina == (short)idsConceptoNomina.CotizacionAdicionalSolidaridad).importe),
                    descuentosSalariales = (n.absentismo ?? 0) + (n.anticipo ?? 0),
                    descuentoPreaviso = n.descuentoPreaviso ?? 0,
                    liquidoPercibir = n.liquidoPercibir ?? 0,
                    anticipo = n.anticipo ?? 0,
                    embargo = n.embargo ?? 0,
                    absentismo = n.absentismo ?? 0,
                    indemnizacion = n.indemnizacion ?? 0,
                    conceptoNEspecie = (n.tblConceptoNominaNNomina.FirstOrDefault(x => x.idConceptoNomina == idConceptoNomina_ConceptoNEspecie) == null ? 0 : n.tblConceptoNominaNNomina.FirstOrDefault(x => x.idConceptoNomina == idConceptoNomina_ConceptoNEspecie).importe)
                                        + (n.tblConceptoNominaNNomina.FirstOrDefault(x => x.idConceptoNomina == idConceptoNomina_SeguroConvenio) == null ? 0 : n.tblConceptoNominaNNomina.FirstOrDefault(x => x.idConceptoNomina == idConceptoNomina_SeguroConvenio).importe),
                    tributacionEspeciesEmpresa = n.tributacionEspeciesEmpresa ?? 0,
                    segSocialEmpresa = n.segSocialEmpresa ?? 0,
                    totalTC1 = n.totalTC1 ?? 0,
                    baseCC = n.baseCC ?? 0,
                    costeEmpresa = n.costeEmpresa ?? 0,
                    seguroConvenio = n.tblConceptoNominaNNomina.FirstOrDefault(x => x.idConceptoNomina == idConceptoNomina_SeguroConvenio) == null ? 0 : n.tblConceptoNominaNNomina.FirstOrDefault(x => x.idConceptoNomina == idConceptoNomina_SeguroConvenio).importe,
                    salarioEspecie = n.tblConceptoNominaNNomina.FirstOrDefault(x => x.idConceptoNomina == idConceptoNomina_ConceptoNEspecie) == null ? 0 : n.tblConceptoNominaNNomina.FirstOrDefault(x => x.idConceptoNomina == idConceptoNomina_ConceptoNEspecie).importe,
                    plusAsistenciaMesAnterior = n.tblConceptoNominaNNomina.FirstOrDefault(x => x.idConceptoNomina == (short)idsConceptoNomina.PlusAsistenciaMesAnterior)?.importe ?? 0,
                    hasConceptosGestoria = n.tblConceptoNominaNNomina_Gestoria.Any(x => x.importe > 0),
                    isPendienteAnticipo = !n.isRetenida && ultimoHistoricoAsientoNomina.Any(uhan => uhan != null && uhan.idNomina == n.idNomina && uhan.liquidoPercibir > n.liquidoPercibir),
                    isPendientePagar = !n.isRetenida
                        && (
                            (
                                isCentroPagado.Any(cp => cp.idAdmCentroCoste == n.idAdmCentroCoste && cp.idAdmElementoPEP == n.idAdmElementoPEP && cp.isPagado)
                                && n.idEstadoHistoricoAsientoNomina == null
                            )
                            || ultimoHistoricoAsientoNomina.Any(uhan => uhan != null && uhan.idNomina == n.idNomina && uhan.liquidoPercibir < n.liquidoPercibir)
                        ),
                });

            return results;
        }

        private void GenerarHistoricoAsientoNomina(ref List<HistoricoNominaContabilidad> datosNominas, int idUsuario, DateTime fechaAsiento, byte? idEstadoHistoricoAsientoNominaDestino = null, bool filtrarFiniquitos = true)
        {
            var hayPagosPendientes = datosNominas.Any(dn => dn.idEstadoHistoricoAsientoNomina == null);
            var hayRetenciones = datosNominas.Any(dn => dn.isRetenida);

            var fecha = DateTime.Now;

            var datosNominasFiltered = datosNominas.Where(dn => !dn.isRetenida && (!hayPagosPendientes || !filtrarFiniquitos || dn.fechaBaja == null || dn.idMotivoBaja == (byte)idsMotivoBaja.ConversionContrato));

            var idsNomina = datosNominasFiltered.Select(dn => dn.idNomina).ToList();

            var ultimoHistoricoAsientoNomina = db.tblHistoricoAsientoNomina
                .Where(han => idsNomina.Contains(han.idNomina))
                .GroupBy(han => han.idNomina)
                .Select(g => g.OrderByDescending(han => han.fecha).FirstOrDefault());

            var asientoNominaCentroSel = datosNominasFiltered
                .Select(dn => new tblHistoricoAsientoNomina
                {
                    idNomina = dn.idNomina,
                    idAdmElementoPEP = dn.idElementoPEP,
                    idAdmCentroCoste = dn.idCentroCoste,
                    idUsuario = idUsuario,
                    idEstadoHistoricoAsientoNomina = idEstadoHistoricoAsientoNominaDestino ?? GetEstadoHistoricoAsientoNomina(dn.idEstadoHistoricoAsientoNomina),
                    fecha = fecha,
                    fechaContabilizado = fechaAsiento,
                    fechaDesde = dn.fechaDesde,
                    fechaHasta = dn.fechaHasta,
                    salarioBruto = dn.salarioBruto ?? 0,
                    tributacionIRPF = dn.tributacionIRPF ?? 0,
                    segSocialTrabajador = dn.segSocialTrabajador ?? 0,
                    liquidoPercibir = dn.liquidoPercibir ?? 0,
                    embargo = dn.embargo ?? 0,
                    conceptoNEspecie = dn.conceptoNEspecie ?? 0,
                    segSocialEmpresa = dn.segSocialEmpresa ?? 0,
                    descuentosSalariales = dn.descuentosSalariales ?? 0,
                    plusAsistenciaMesAnterior = dn.plusAsistenciaMesAnterior ?? 0,
                    descuentoPreaviso = dn.descuentoPreaviso ?? 0
                });

            var asientoNominaDiferentes = (
                from ancs in asientoNominaCentroSel
                join uhan in ultimoHistoricoAsientoNomina on ancs.idNomina equals uhan.idNomina into uhanGroup
                from uhan in uhanGroup.DefaultIfEmpty()
                where
                    uhan != null
                    && (
                        ancs.salarioBruto != uhan.salarioBruto
                        || ancs.tributacionIRPF != uhan.tributacionIRPF
                        || ancs.segSocialTrabajador != uhan.segSocialTrabajador
                        || ancs.liquidoPercibir != uhan.liquidoPercibir
                        || ancs.embargo != uhan.embargo
                        || ancs.conceptoNEspecie != uhan.conceptoNEspecie
                        || ancs.segSocialEmpresa != uhan.segSocialEmpresa
                        || ancs.descuentosSalariales != uhan.descuentosSalariales
                        || ancs.plusAsistenciaMesAnterior != uhan.plusAsistenciaMesAnterior
                        || ancs.descuentoPreaviso != uhan.descuentoPreaviso
                    )
                select ancs
            );

            var isContabilizado = !asientoNominaDiferentes.Any() && asientoNominaCentroSel.All(ancs => ancs.idEstadoHistoricoAsientoNomina == (byte)idsEstadoHistoricoAsientoNomina.Contabilizado);

            if (!isContabilizado)
            {
                asientoNominaCentroSel = datosNominasFiltered
                .Select(dn => new tblHistoricoAsientoNomina
                {
                    idNomina = dn.idNomina,
                    idAdmElementoPEP = dn.idElementoPEP,
                    idAdmCentroCoste = dn.idCentroCoste,
                    idUsuario = idUsuario,
                    idEstadoHistoricoAsientoNomina = idEstadoHistoricoAsientoNominaDestino ?? (byte)idsEstadoHistoricoAsientoNomina.Pagado,
                    fecha = fecha,
                    fechaContabilizado = fechaAsiento,
                    fechaDesde = dn.fechaDesde,
                    fechaHasta = dn.fechaHasta,
                    salarioBruto = dn.salarioBruto ?? 0,
                    tributacionIRPF = dn.tributacionIRPF ?? 0,
                    segSocialTrabajador = dn.segSocialTrabajador ?? 0,
                    liquidoPercibir = dn.liquidoPercibir ?? 0,
                    embargo = dn.embargo ?? 0,
                    conceptoNEspecie = dn.conceptoNEspecie ?? 0,
                    segSocialEmpresa = dn.segSocialEmpresa ?? 0,
                    descuentosSalariales = dn.descuentosSalariales ?? 0,
                    plusAsistenciaMesAnterior = dn.plusAsistenciaMesAnterior ?? 0,
                    descuentoPreaviso = dn.descuentoPreaviso ?? 0
                });
            }

            var asientoNominaCentroSelFinal = (
                from ancs in asientoNominaCentroSel
                join uhan in ultimoHistoricoAsientoNomina on ancs.idNomina equals uhan.idNomina into uhanGroup
                from uhan in uhanGroup.DefaultIfEmpty()
                where
                    uhan == null
                    || ancs.idEstadoHistoricoAsientoNomina != uhan.idEstadoHistoricoAsientoNomina
                    || ancs.salarioBruto != uhan.salarioBruto
                    || ancs.tributacionIRPF != uhan.tributacionIRPF
                    || ancs.segSocialTrabajador != uhan.segSocialTrabajador
                    || ancs.liquidoPercibir != uhan.liquidoPercibir
                    || ancs.embargo != uhan.embargo
                    || ancs.conceptoNEspecie != uhan.conceptoNEspecie
                    || ancs.segSocialEmpresa != uhan.segSocialEmpresa
                    || ancs.descuentosSalariales != uhan.descuentosSalariales
                    || ancs.plusAsistenciaMesAnterior != uhan.plusAsistenciaMesAnterior
                    || ancs.descuentoPreaviso != uhan.descuentoPreaviso
                select ancs
            );

            if (isContabilizado && asientoNominaCentroSelFinal.Any())
            {
                asientoNominaCentroSelFinal = asientoNominaCentroSel;
            }

            db.tblHistoricoAsientoNomina.AddRange(asientoNominaCentroSelFinal);

            var tblNomina = db.tblNomina
                .Where(x => asientoNominaCentroSelFinal.Select(x => x.idNomina).Contains(x.idNomina))
                .ToList();

            foreach (var n in tblNomina)
            {
                var idEstadoHistoricoAsientoNomina = asientoNominaCentroSelFinal.FirstOrDefault(ancs => ancs.idNomina == n.idNomina)?.idEstadoHistoricoAsientoNomina;

                var datoNomina = datosNominas.FirstOrDefault(dn => dn.idNomina == n.idNomina);

                if (datoNomina != null)
                {
                    datoNomina.idEstadoHistoricoAsientoNomina = idEstadoHistoricoAsientoNomina;
                }

                n.idEstadoHistoricoAsientoNomina = idEstadoHistoricoAsientoNomina;
            }

            byte? GetEstadoHistoricoAsientoNomina(byte? idEstadoHistoricoAsientoNomina)
            {
                if (idEstadoHistoricoAsientoNomina == null)
                    return (byte)idsEstadoHistoricoAsientoNomina.Pagado;

                var idsEstadoHistoricoAsientoNominaCustom = new List<byte?>
                {
                    (byte)idsEstadoHistoricoAsientoNomina.Pagado,
                    (byte)idsEstadoHistoricoAsientoNomina.Anticipo
                };

                if (!hayPagosPendientes && !hayRetenciones && idsEstadoHistoricoAsientoNominaCustom.Contains(idEstadoHistoricoAsientoNomina))
                    return (byte)idsEstadoHistoricoAsientoNomina.Contabilizado;

                return idEstadoHistoricoAsientoNomina;
            }
        }

        #endregion

        #region Dominicana

        [HttpGet("odata/MyPolarier/Contabilidad/AsientosNominas/GetHistorialNominas_RD")]
        [Authorize]
        public async Task<ActionResult> GetHistorialNominas_RD([FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta, [FromODataUri] TipoNominaRD fortnight)
        {
            return Ok(GetDatosNominas_RD(fechaDesde, fechaHasta, fortnight).Select(x => new
            {
                x.idNomina_RD,
                x.nombreCompleto,
                x.inicioPeriodo,
                x.finPeriodo,
                x.sueldo,
                x.diasPropina,
                x.diasFeriados,
                x.horasNocturnas,
                x.horasExtras35,
                x.primaVacacional,
                x.gratificacion,
                x.otrosIngresos,
                x.salarioRetroactivo,
                x.incentivos,
                x.subsidPorEnferm,
                x.ayudaPorNacimiento,
                x.subsidioPorMaternidad,
                x.ayudaPorMuerte,
                x.reembolsoOtrosDescuentos,
                x.saldoAFavorISR,
                x.reembolsoISR,
                x.totalIngresos,
                x.impSobreLaRenta,
                x.seguroMedicoPrivado,
                x.descuentoDeLicenciaMedica,
                x.sindicato,
                x.anticipoNomina,
                x.descAFP,
                x.descSFS,
                x.dependAdicionalesSFS,
                x.otrosDescuentos,
                x.ahorroCoop,
                x.prestamoCoop,
                x.ordenDeCompra,
                x.infotep,
                x.descuentosOtros,
                x.totalDescuentos,
                x.neto,
                x.contabilizado,
                x.idPersona,
                centro = x.idAdmCentroCosteNavigation != null ? $"{x.idAdmCentroCosteNavigation.codigo} - {x.idAdmCentroCosteNavigation.denominacion}"
                        : (x.idAdmElementoPEPNavigation != null ? $"{x.idAdmElementoPEPNavigation.codigo} - {x.idAdmElementoPEPNavigation.denominacion}" : "Sin centro"),
                tipoCentro = x.idAdmCentroCoste != null ? "CentroCoste" : (x.idAdmElementoPEP != null ? "ElementoPEP" : "Sin centro"),
                idCentro = x.idAdmCentroCoste != null ? x.idAdmCentroCoste : (x.idAdmElementoPEP != null ? x.idAdmElementoPEP : 0),
            })); ;
        }

        [HttpPost("odata/MyPolarier/Contabilidad/AsientosNominas/GenerarAsientosNominasSAP_RD")]
        [Authorize]
        public async Task<ActionResult> GenerarAsientosNominasSAP_RD([FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta, [FromODataUri] TipoNominaRD fortnight, [FromODataUri] int? idCentroCoste, [FromODataUri] int? idElementoPEP, [FromODataUri] DateTime fechaAsiento)
        {
            var idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

            var elementoPEP = db.tblAdmElementoPEP
                .Include(x => x.idEmpresaPolarierNavigation)
                    .ThenInclude(x => x.idMonedaNavigation)
                .Include(x => x.idAdmCentroBeneficioNavigation)
                .Where(x => x.idAdmElementoPEP == idElementoPEP)
                .SingleOrDefault();
            var centroCoste = db.tblAdmCentroCoste
                .Include(x => x.idEmpresaPolarierNavigation)
                    .ThenInclude(x => x.idMonedaNavigation)
                .Where(x => x.idAdmCentroCoste == idCentroCoste)
                .SingleOrDefault();

            if (elementoPEP == null && centroCoste == null)
                return BadRequest("No se ha específicado elemento PEP ni centro de coste");

            List<tblNomina_RD> datosNominas;
            List<IGrouping<int?, tblNomina_RD>> datosNominasAgrupados;
            IQueryable<dynamic> tiposTrabajoSeleccionados;
            string nombreCentro = "";
            string textoCabecera;

            datosNominas = GetDatosNominas_RD(fechaDesde, fechaHasta, fortnight)
                .Where(x => x.idAdmElementoPEP == idElementoPEP && x.idAdmCentroCoste == idCentroCoste).ToList();
            datosNominasAgrupados = datosNominas
                .GroupBy(x => x.idAdmCuentaContable).ToList();

            nombreCentro = centroCoste?.denominacion ?? elementoPEP.denominacion;

            GenerarHistoricoAsientoNomina_RD(datosNominas, idUsuario, fechaDesde);

            List<EntradaAsientoContable> entradas = new();
            var empresa = elementoPEP?.idEmpresaPolarierNavigation ?? centroCoste?.idEmpresaPolarierNavigation;
            string sociedad = empresa.companyCode_SAP;
            string moneda = empresa.idMonedaNavigation.codigo;
            textoCabecera = String.Concat(("NOM." + datosNominasAgrupados.Select(x => x.Select(x => x.finPeriodo).FirstOrDefault()?.ToString("dd/MM/yyyy")).FirstOrDefault()).Trim().Take(25));

            void AddEntradaAsientoContable(string referencia, decimal? importe, string cuentaContable)
            {
                entradas.Add(new EntradaAsientoContable
                {
                    IDApunte = 1,
                    Sociedad = sociedad,
                    ClaseDocumento = "SA",
                    Referencia1 = referencia,
                    TextoCabecera = textoCabecera,
                    FechaFactura = fechaAsiento.ToString("dd.MM.yyyy"),
                    FechaContable = fechaAsiento.ToString("dd.MM.yyyy"),
                    FeDeclImpt = fechaAsiento.ToString("dd.MM.yyyy"), // Todo: Revisar fecha
                    FeCumpImpt = fechaAsiento.ToString("dd.MM.yyyy"), // Todo: Revisar fecha
                    CuentaContable = cuentaContable,
                    ImporteTransaccion = importe ?? 0,
                    MonedaTransaccion = moneda,
                    ImporteSociedad = importe ?? 0,
                    MonedaSociedad = moneda,
                    Centrocoste = centroCoste != null ? centroCoste.codigo : "",
                    Centrobeneficio = elementoPEP != null ? elementoPEP.idAdmCentroBeneficioNavigation.codigo : "",
                    Pep = elementoPEP != null ? elementoPEP.codigo : "",
                });
            }

            var tblAdmCuentaContable = db.tblAdmCuentaContable
                .Where(x => datosNominasAgrupados.Select(x => x.Key).Contains(x.idAdmCuentaContable))
                .ToList();

            foreach (var item in datosNominasAgrupados)
            {

                var cuentaContable = tblAdmCuentaContable.FirstOrDefault(x => x.idAdmCuentaContable == item.Key);

                var conceptos = new Dictionary<string, (decimal? suma, string propiedad, string? cuentaContable)>
                {
                    {"Sueldo", (item.Sum(x => x.sueldo), "sueldo", (string?)null)},
                    {"Propina", (item.Sum(x => x.diasPropina),"diasPropina", (string?)null)},
                    {"Dias feriados", (item.Sum(x => x.diasFeriados),"diasFeriados", (string?)null)},
                    {"Horas nocturnas", (item.Sum(x => x.horasNocturnas), "horasNocturnas", (string?)null)},
                    {"Horas extra 35%", (item.Sum(x => x.horasExtras35), "horasExtras35", (string?)null)},
                    {"Prima vacacional", (item.Sum(x => x.primaVacacional), "primaVacacional", (string?)null)},
                    {"Gratificación", (item.Sum(x => x.gratificacion), "gratificacion", (string?)null)},
                    {"Otros ingresos", (item.Sum(x => x.otrosIngresos), "otrosIngresos", (string?)null)},
                    {"Salario retroactivo", (item.Sum(x => x.salarioRetroactivo), "salarioRetroactivo", (string?)null)},
                    {"Incentivos", (item.Sum(x => x.incentivos), "incentivos", (string?)null)},
                    {"Subsidio por enfermedad", (item.Sum(x => x.subsidPorEnferm), "subsidPorEnferm", (string?)null)},
                    {"Ayuda por nacimiento", (item.Sum(x => x.ayudaPorNacimiento), "ayudaPorNacimiento", (string?)null)},
                    {"Subsidio por maternidad", (item.Sum(x => x.subsidioPorMaternidad), "subsidioPorMaternidad", (string?)null)},
                    {"Ayuda por muerte", (item.Sum(x => x.ayudaPorMuerte), "ayudaPorMuerte", (string?)null)},
                    {"Reembolso otros descuentos", (-item.Sum(x => x.reembolsoOtrosDescuentos), "reembolsoOtrosDescuentos", (string?)"46500001")},
                    {"Saldo a favor de ISR", (-item.Sum(x => x.saldoAFavorISR), "saldoAFavorISR", (string?)"47510007")},
                    {"Reembolso ISR", (-item.Sum(x => x.reembolsoISR), "reembolsoISR", (string?)"47510007")},
                    {"Impuesto sobre la renta", (-item.Sum(x => x.impSobreLaRenta), "impSobreLaRenta", (string?)"47510007")},
                    {"SM privado", (-item.Sum(x => x.seguroMedicoPrivado), "seguroMedicoPrivado", (string?)"46000005")},
                    {"Dto licencia médica", (-item.Sum(x => x.descuentoDeLicenciaMedica), "descuentoDeLicenciaMedica", (string?)null)},
                    {"Sindicato", (-item.Sum(x => x.sindicato), "sindicato", (string?)"46000008")},
                    {"Anticipo nómina", (item.Sum(x => x.anticipoNomina), "anticipoNomina", (string?)"46000002")},
                    {"Dto AFP", (-item.Sum(x => x.descAFP), "descAFP", (string?)"47600004")},
                    {"Dto SFS", (-item.Sum(x => x.descSFS), "descSFS", (string?)"47600003")},
                    {"Depend adic SFS", (-item.Sum(x => x.dependAdicionalesSFS), "dependAdicionalesSFS", (string?)"47600003")},
                    {"Descuento días", (-item.Sum(x => x.descuentosOtros), "descuentosOtros", (string?)"77800002")},
                    {"Ahorro coop", (-item.Sum(x => x.ahorroCoop), "ahorroCoop", (string?)"46000007")},
                    {"Préstamo coop", (-item.Sum(x => x.prestamoCoop), "prestamoCoop", (string?)"46000007")},
                    {"Orden de compra", (-item.Sum(x => x.ordenDeCompra), "ordenDeCompra", (string?)"46000006")},
                    {"Infotep", (-item.Sum(x => x.infotep), "infotep", (string?)"47600006")},
                    {"Otros descuentos", (-item.Sum(x => x.otrosDescuentos), "otrosDescuentos", (string?)"77800002")},
                    {"Neto", (-item.Sum(x => x.neto), "neto",(string?)"46500001")}
                };

                foreach (var concepto in conceptos)
                {
                    AddEntradaAsientoContable(
                        concepto.Key,
                        concepto.Value.suma,
                        concepto.Value.cuentaContable ?? cuentaContable?.codigo
                        );
                }
            }

            var csv = EntradaAsientoContable.GenerateCSV(entradas);
            var objStream = new MemoryStream(Encoding.GetEncoding("ISO-8859-1").GetBytes(csv));

            string fortnightString = fortnight.ToString();
            var nombreArchivo = "asientoNominas_" + nombreCentro + "_" + fechaAsiento.ToString("yyyy-MM") + "_" + fortnightString + ".csv";

            var ftp = new FTPConnection();
            var ftpResponse = await ftp.UploadFile(nombreArchivo, objStream.ToArray());

            List<tblNomina_RD> nominasContabilizadas = GetDatosNominas_RD(fechaDesde, fechaHasta, fortnight)
                .Where(x => x.idAdmElementoPEP == idElementoPEP && x.idAdmCentroCoste == idCentroCoste).ToList();
            foreach (tblNomina_RD nomina in nominasContabilizadas)
            {
                nomina.contabilizado = true;
            }
            db.SaveChanges();

            return Ok(ftpResponse.StatusCode == System.Net.FtpStatusCode.ClosingData);
        }

        [HttpPost("odata/MyPolarier/Contabilidad/AsientosNominas/GuardarHistoricoAsientoNomina_RD")]
        [Authorize]
        public async Task<ActionResult> GuardarHistoricoAsientoNomina_RD([FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta, [FromODataUri] TipoNominaRD fortnight, [FromODataUri] int? idCentroCoste, [FromODataUri] int? idElementoPEP)
        {
            var idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

            var datosNominas = GetDatosNominas_RD(fechaDesde, fechaHasta, fortnight)
                .Where(x => x.idAdmElementoPEP == idElementoPEP && x.idAdmCentroCoste == idCentroCoste)
                .ToList();

            GenerarHistoricoAsientoNomina_RD(datosNominas, idUsuario);

            await db.SaveChangesAsync();

            return Ok(true);
        }

        private IQueryable<tblNomina_RD> GetDatosNominas_RD(DateTime fechaDesde, DateTime fechaHasta, TipoNominaRD fortnight)
        {
            Date inicio;
            Date fin;
            if (fortnight == TipoNominaRD.NominaQ1)
            {
                inicio = new Date(fechaDesde.Year, fechaDesde.Month, fechaDesde.Day);
                fin = new Date(fechaHasta.Year, fechaHasta.Month, 15);
            }
            else if (fortnight == TipoNominaRD.NominaQ2)
            {
                inicio = new Date(fechaDesde.Year, fechaDesde.Month, 16);
                fin = new Date(fechaHasta.Year, fechaHasta.Month, fechaHasta.Day);
            }
            else
            {
                inicio = new Date(fechaDesde.Year, 1, 1);
                fin = new Date(fechaHasta.Year, 12, 31);
            }
            var nominas = db.tblNomina_RD
                .Where(x => x.inicioPeriodo == inicio && x.finPeriodo == fin);
            return nominas;
        }

        private void GenerarHistoricoAsientoNomina_RD(List<tblNomina_RD> datosNominas, int idUsuario, DateTime? fechaAsiento = null)
        {
            var fecha = DateTimeOffset.Now;

            var asientoNominaCentroSel = datosNominas
                .Select(dn => new tblHistoricoAsientoNomina_RD
                {
                    idNomina_RD = dn.idNomina_RD,
                    idAdmElementoPEP = dn.idAdmElementoPEP,
                    idAdmCentroCoste = dn.idAdmCentroCoste,
                    idUsuario = idUsuario,
                    fecha = fecha,
                    fechaContabilizado = fechaAsiento,
                    fechaDesde = dn.inicioPeriodo.Value,
                    fechaHasta = dn.finPeriodo.Value,
                    sueldo = dn.sueldo ?? 0,
                    diasPropina = dn.diasPropina ?? 0,
                    diasFeriados = dn.diasFeriados ?? 0,
                    horasNocturnas = dn.horasNocturnas ?? 0,
                    horasExtras35 = dn.horasExtras35 ?? 0,
                    primaVacacional = dn.primaVacacional ?? 0,
                    gratificacion = dn.gratificacion ?? 0,
                    otrosIngresos = dn.otrosIngresos ?? 0,
                    salarioRetroactivo = dn.salarioRetroactivo ?? 0,
                    incentivos = dn.incentivos ?? 0,
                    subsidPorEnferm = dn.subsidPorEnferm ?? 0,
                    subsidioPorMaternidad = dn.subsidioPorMaternidad ?? 0,
                    ayudaPorNacimiento = dn.ayudaPorNacimiento ?? 0,
                    ayudaPorMuerte = dn.ayudaPorMuerte ?? 0,
                    reembolsoOtrosDescuentos = dn.reembolsoOtrosDescuentos ?? 0,
                    saldoAFavorISR = dn.saldoAFavorISR ?? 0,
                    reembolsoISR = dn.reembolsoISR ?? 0,
                    totalIngresos = dn.totalIngresos ?? 0,
                    impSobreLaRenta = dn.impSobreLaRenta ?? 0,
                    seguroMedicoPrivado = dn.seguroMedicoPrivado ?? 0,
                    descuentoDeLicenciaMedica = dn.descuentoDeLicenciaMedica ?? 0,
                    sindicato = dn.sindicato ?? 0,
                    anticipoNomina = dn.anticipoNomina ?? 0,
                    descAFP = dn.descAFP ?? 0,
                    descSFS = dn.descSFS ?? 0,
                    dependAdicionalesSFS = dn.dependAdicionalesSFS ?? 0,
                    otrosDescuentos = dn.otrosDescuentos ?? 0,
                    ahorroCoop = dn.ahorroCoop ?? 0,
                    prestamoCoop = dn.prestamoCoop ?? 0,
                    ordenDeCompra = dn.ordenDeCompra ?? 0,
                    infotep = dn.infotep ?? 0,
                    descuentosOtros = dn.descuentosOtros ?? 0,
                    totalDescuentos = dn.totalDescuentos ?? 0,
                    neto = dn.neto ?? 0,
                });

            db.tblHistoricoAsientoNomina_RD.AddRange(asientoNominaCentroSel);
        }

        #endregion

        #region México

        [HttpGet("odata/MyPolarier/Contabilidad/AsientosNominas/GetHistorialNominas_MX")]
        [Authorize]
        public async Task<ActionResult> GetHistorialNominas_MX([FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta, [FromODataUri] TipoNominaMX tipoNomina, [FromODataUri] idsEmpresaPolarier idEmpresaPolarier)
        {
            return Ok(GetDatosNominas_MX(fechaDesde, fechaHasta, tipoNomina, idEmpresaPolarier).Select(x => new
            {
                x.idNomina_MX,
                x.idTipoNomina_MX,
                x.nombreCompleto,
                x.inicioPeriodo,
                x.finPeriodo,
                x.sueldo,
                x.horasExtras,
                x.primaVacacional,
                x.primaDominical,
                x.bono,
                x.descansoTrabajado,
                x.aguinaldo,
                x.valesDespensa,
                x.fondoAhorro,
                x.otraPercepcion,
                x.gastosSindicales,
                x.PTU,
                x.infonavitEmpleado,
                x.fonacotEmpleado,
                x.IMSSEmpleado,
                x.SAREmpleado,
                x.ISREmpleado,
                x.subsidioEmpleo,
                x.devolucionPrestamo,
                x.otrasDeducciones,
                x.descAlimentos,
                x.totalDeducciones,
                x.totalSP,
                x.totalSV,
                x.percepcionNeta,
                x.IMSSPatronal,
                x.infonavitPatronal,
                x.SARPatronal,
                x.impuestoEstatalSobreNominas,
                x.contabilizado,
                x.idPersona,
                centro = x.idAdmCentroCosteNavigation != null ? $"{x.idAdmCentroCosteNavigation.codigo} - {x.idAdmCentroCosteNavigation.denominacion}"
                        : (x.idAdmElementoPEPNavigation != null ? $"{x.idAdmElementoPEPNavigation.codigo} - {x.idAdmElementoPEPNavigation.denominacion}" : "Sin centro"),
                tipoCentro = x.idAdmCentroCoste != null ? "CentroCoste" : (x.idAdmElementoPEP != null ? "ElementoPEP" : "Sin centro"),
                idCentro = x.idAdmCentroCoste != null ? x.idAdmCentroCoste : (x.idAdmElementoPEP != null ? x.idAdmElementoPEP : 0),
                x.idTipoTrabajo
            }));
        }

        [HttpPost("odata/MyPolarier/Contabilidad/AsientosNominas/GenerarAsientosNominasSAP_MX")]
        [Authorize]
        public async Task<ActionResult> GenerarAsientosNominasSAP_MX([FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta, [FromODataUri] TipoNominaMX tipoNomina, [FromODataUri] idsEmpresaPolarier idEmpresaPolarier, [FromODataUri] int? idCentroCoste, [FromODataUri] int? idElementoPEP, [FromODataUri] DateTime fechaAsiento)
        {
            var idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

            var elementoPEP = db.tblAdmElementoPEP
                .Include(x => x.idEmpresaPolarierNavigation)
                    .ThenInclude(x => x.idMonedaNavigation)
                .Include(x => x.idAdmCentroBeneficioNavigation)
                .Where(x => x.idAdmElementoPEP == idElementoPEP)
                .SingleOrDefault();
            var centroCoste = db.tblAdmCentroCoste
                .Include(x => x.idEmpresaPolarierNavigation)
                    .ThenInclude(x => x.idMonedaNavigation)
                .Where(x => x.idAdmCentroCoste == idCentroCoste)
                .SingleOrDefault();

            if (elementoPEP == null && centroCoste == null)
                return BadRequest("No se ha específicado elemento PEP ni centro de coste");

            List<tblNomina_MX> datosNominas;
            List<IGrouping<(byte? idTipoTrabajo, int? idCentroTrabajo), tblNomina_MX>> datosNominasAgrupados;
            IQueryable<dynamic> tiposTrabajoSeleccionados;
            string nombreCentro = "";
            string codigoCentro = "";
            string textoCabecera;

            datosNominas = GetDatosNominas_MX(fechaDesde, fechaHasta, tipoNomina, idEmpresaPolarier)
                .Include(x => x.idPersonaNavigation)
                .Where(x => x.idAdmElementoPEP == idElementoPEP && x.idAdmCentroCoste == idCentroCoste).ToList();
            datosNominasAgrupados = datosNominas
                .GroupBy(x => (x.idPersonaNavigation?.idTipoTrabajo, x.idPersonaNavigation?.idCentroTrabajo)).ToList();

            nombreCentro = centroCoste?.denominacion ?? elementoPEP.denominacion;
            codigoCentro = centroCoste?.codigo ?? elementoPEP.codigo;

            GenerarHistoricoAsientoNomina_MX(datosNominas, idUsuario, fechaDesde);

            List<EntradaAsientoContable> entradas = new();
            var empresa = elementoPEP?.idEmpresaPolarierNavigation ?? centroCoste?.idEmpresaPolarierNavigation;
            string sociedad = empresa.companyCode_SAP;
            string moneda = empresa.idMonedaNavigation.codigo;
            textoCabecera = String.Concat(("NOM." + datosNominasAgrupados.Select(x => x.Select(x => x.finPeriodo).FirstOrDefault()?.ToString("dd/MM/yyyy")).FirstOrDefault()).Trim().Take(25));

            void AddEntradaAsientoContable(string referencia, decimal? importe, string cuentaContable)
            {
                entradas.Add(new EntradaAsientoContable
                {
                    IDApunte = 1,
                    Sociedad = sociedad,
                    ClaseDocumento = "SA",
                    Referencia1 = referencia,
                    TextoCabecera = textoCabecera,
                    FechaFactura = fechaAsiento.ToString("dd.MM.yyyy"),
                    FechaContable = fechaAsiento.ToString("dd.MM.yyyy"),
                    FeDeclImpt = fechaAsiento.ToString("dd.MM.yyyy"), // Todo: Revisar fecha
                    FeCumpImpt = fechaAsiento.ToString("dd.MM.yyyy"), // Todo: Revisar fecha
                    CuentaContable = cuentaContable,
                    ImporteTransaccion = importe ?? 0,
                    MonedaTransaccion = moneda,
                    ImporteSociedad = importe ?? 0,
                    MonedaSociedad = moneda,
                    Centrocoste = centroCoste != null ? centroCoste.codigo : "",
                    Centrobeneficio = elementoPEP != null ? elementoPEP.idAdmCentroBeneficioNavigation.codigo : "",
                    Pep = elementoPEP != null ? elementoPEP.codigo : "",
                });
            }

            var tblCuentaContableNTipoTrabajo = db.tblCuentaContableNTipoTrabajo
                .Include(x => x.idAdmCuentaContable_Sueldo_MXNavigation)
                .Include(x => x.idAdmCuentaContable_IMSS_MXNavigation)
                .Include(x => x.idAdmCuentaContable_INFONAVIT_MXNavigation)
                .Include(x => x.idAdmCuentaContable_SAR_MXNavigation)
                .Include(x => x.idAdmCuentaContable_ImpEstatalNominas_MXNavigation)
                .ToList();
            var tblCuentaContableNCentroTrabajo = db.tblCuentaContableNCentroTrabajo
                .Include(x => x.idAdmCuentaContable_Sueldo_MXNavigation)
                .Include(x => x.idAdmCuentaContable_IMSS_MXNavigation)
                .Include(x => x.idAdmCuentaContable_INFONAVIT_MXNavigation)
                .Include(x => x.idAdmCuentaContable_SAR_MXNavigation)
                .Include(x => x.idAdmCuentaContable_ImpEstatalNominas_MXNavigation)
                .ToList();

            foreach (var item in datosNominasAgrupados)
            {

                tblAdmCuentaContable cuentaContable_sueldo = item.Key.idTipoTrabajo != null ?
                    tblCuentaContableNTipoTrabajo.FirstOrDefault(x => x.idTipoTrabajo == (byte)item.Key.idTipoTrabajo).idAdmCuentaContable_Sueldo_MXNavigation
                    : tblCuentaContableNCentroTrabajo.FirstOrDefault(x => x.idCentroTrabajo == (int)item.Key.idCentroTrabajo).idAdmCuentaContable_Sueldo_MXNavigation;
                tblAdmCuentaContable cuentaContable_IMSS = item.Key.idTipoTrabajo != null ?
                    tblCuentaContableNTipoTrabajo.FirstOrDefault(x => x.idTipoTrabajo == (byte)item.Key.idTipoTrabajo).idAdmCuentaContable_IMSS_MXNavigation
                    : tblCuentaContableNCentroTrabajo.FirstOrDefault(x => x.idCentroTrabajo == (int)item.Key.idCentroTrabajo).idAdmCuentaContable_IMSS_MXNavigation;
                tblAdmCuentaContable cuentaContable_INFONAVIT = item.Key.idTipoTrabajo != null ?
                    tblCuentaContableNTipoTrabajo.FirstOrDefault(x => x.idTipoTrabajo == (byte)item.Key.idTipoTrabajo).idAdmCuentaContable_INFONAVIT_MXNavigation
                    : tblCuentaContableNCentroTrabajo.FirstOrDefault(x => x.idCentroTrabajo == (int)item.Key.idCentroTrabajo).idAdmCuentaContable_INFONAVIT_MXNavigation;
                tblAdmCuentaContable cuentaContable_SAR = item.Key.idTipoTrabajo != null ?
                    tblCuentaContableNTipoTrabajo.FirstOrDefault(x => x.idTipoTrabajo == (byte)item.Key.idTipoTrabajo).idAdmCuentaContable_SAR_MXNavigation
                    : tblCuentaContableNCentroTrabajo.FirstOrDefault(x => x.idCentroTrabajo == (int)item.Key.idCentroTrabajo).idAdmCuentaContable_SAR_MXNavigation;
                tblAdmCuentaContable cuentaContable_ImpEstatalNominas = item.Key.idTipoTrabajo != null ?
                    tblCuentaContableNTipoTrabajo.FirstOrDefault(x => x.idTipoTrabajo == (byte)item.Key.idTipoTrabajo).idAdmCuentaContable_ImpEstatalNominas_MXNavigation
                    : tblCuentaContableNCentroTrabajo.FirstOrDefault(x => x.idCentroTrabajo == (int)item.Key.idCentroTrabajo).idAdmCuentaContable_ImpEstatalNominas_MXNavigation;

                var conceptos = new List<(string concepto, decimal? suma, string propiedad, string? cuentaContable)>
                {
                    {("Sueldos y salarios",              item.Sum(x => x.sueldo), "sueldo", cuentaContable_sueldo.codigo)},
                    {("Horas extras",                    item.Sum(x => x.horasExtras), "horasExtras", cuentaContable_sueldo.codigo)},
                    {("Prima vacacional",                item.Sum(x => x.primaVacacional), "primaVacacional", cuentaContable_sueldo.codigo)},
                    {("Prima dominical",                 item.Sum(x => x.primaDominical), "primaDominical", cuentaContable_sueldo.codigo)},
                    {("Bono",                            item.Sum(x => x.bono), "bono", cuentaContable_sueldo.codigo)},
                    {("Descanso trabajado",              item.Sum(x => x.descansoTrabajado), "descansoTrabajado", cuentaContable_sueldo.codigo)},
                    {("Aguinaldo",                       item.Sum(x => x.aguinaldo), "aguinaldo", cuentaContable_sueldo.codigo)},
                    {("Vales de despensa",               item.Sum(x => x.valesDespensa), "valesDespensa", cuentaContable_sueldo.codigo)},
                    {("Vales de despensa",               -item.Sum(x => x.valesDespensa), "valesDespensa", "46000010")},
                    {("Fondo de ahorro",                 item.Sum(x => x.fondoAhorro), "fondoAhorro", cuentaContable_sueldo.codigo)},
                    {("Otra percepción",                 item.Sum(x => x.otraPercepcion), "otraPercepcion", cuentaContable_sueldo.codigo)},
                    {("Gastos sindicales",               item.Sum(x => x.gastosSindicales), "gastosSindicales", "64900005")},
                    {("PTU",                             item.Sum(x => x.PTU), "PTU", cuentaContable_sueldo.codigo)},
                    {("Infonavit empleado",              -item.Sum(x => x.infonavitEmpleado), "infonavitEmpleado", "47600009")},
                    {("Fonacot empleado",                -item.Sum(x => x.fonacotEmpleado), "fonacotEmpleado", "47600010")},
                    {("IMSS empleado",                   -item.Sum(x => x.IMSSEmpleado), "IMSSEmpleado", "47600007")},
                    {("SAR empleado",                    -item.Sum(x => x.SAREmpleado), "SAREmpleado", "47600008")},
                    {("ISR empleado",                    -item.Sum(x => x.ISREmpleado), "ISREmpleado", "47510007")},
                    {("Subsidio al empleo",              -item.Sum(x => x.subsidioEmpleo), "subsidioEmpleo", "47000004")},
                    {("Devolucion prestamo",             -item.Sum(x => x.devolucionPrestamo), "devolucionPrestamo", "46000002")},
                    {("Otras deducciones",               -item.Sum(x => x.otrasDeducciones), "otrasDeducciones", "46000009")},
                    {("Desc Alimentos",                  -item.Sum(x => x.descAlimentos), "descAlimentos", "62300016")},
                    {("Percepción neta",                 -item.Sum(x => x.totalSP), "percepcionNeta", "46500001")},
                    {("IMSS patronal",                   item.Sum(x => x.IMSSPatronal), "IMSSPatronal", cuentaContable_IMSS.codigo)},
                    {("IMSS patronal",                   -item.Sum(x => x.IMSSPatronal), "IMSSPatronal", "47600007")},
                    {("Infonavit patronal",              item.Sum(x => x.infonavitPatronal), "infonavitPatronal", cuentaContable_INFONAVIT.codigo)},
                    {("Infonavit patronal",              -item.Sum(x => x.infonavitPatronal), "infonavitPatronal", "47600009")},
                    {("SAR patronal",                    item.Sum(x => x.SARPatronal), "SARPatronal", cuentaContable_SAR.codigo)},
                    {("SAR patronal",                    -item.Sum(x => x.SARPatronal), "SARPatronal", "47600008")},
                    {("Impuesto estatal sobre nóminas",  item.Sum(x => x.impuestoEstatalSobreNominas), "impuestoEstatalSobreNominas", cuentaContable_ImpEstatalNominas.codigo)},
                    {("Impuesto estatal sobre nóminas",  -item.Sum(x => x.impuestoEstatalSobreNominas), "impuestoEstatalSobreNominas", "47600011")},
                };

                foreach (var concepto in conceptos)
                {
                    AddEntradaAsientoContable(
                        concepto.concepto,
                        concepto.suma,
                        concepto.cuentaContable
                        );
                }
            }

            var csv = EntradaAsientoContable.GenerateCSV(entradas);
            var objStream = new MemoryStream(Encoding.GetEncoding("ISO-8859-1").GetBytes(csv));

            string fortnightString = tipoNomina.ToString();
            var nombreArchivo = "asientoNominas_" + nombreCentro + "_" + codigoCentro + "_" + fechaAsiento.ToString("yyyy-MM") + "_" + fortnightString + ".csv";

            var ftp = new FTPConnection();
            var ftpResponse = await ftp.UploadFile(nombreArchivo, objStream.ToArray());

            List<tblNomina_MX> nominasContabilizadas = GetDatosNominas_MX(fechaDesde, fechaHasta, tipoNomina, idEmpresaPolarier)
                .Where(x => x.idAdmElementoPEP == idElementoPEP && x.idAdmCentroCoste == idCentroCoste).ToList();
            foreach (tblNomina_MX nomina in nominasContabilizadas)
            {
                nomina.contabilizado = true;
            }
            db.SaveChanges();

            return Ok(ftpResponse.StatusCode == System.Net.FtpStatusCode.ClosingData);
        }

        private IQueryable<tblNomina_MX> GetDatosNominas_MX(DateTime fechaDesde, DateTime fechaHasta, TipoNominaMX tipoNomina, idsEmpresaPolarier idEmpresaPolarier)
        {
            Date inicio;
            Date fin;
            if (TipoPagaMXUtils.isPagaQ1(tipoNomina))
            {
                inicio = new Date(fechaDesde.Year, fechaDesde.Month, fechaDesde.Day);
                fin = new Date(fechaHasta.Year, fechaHasta.Month, 15);
            }
            else if (TipoPagaMXUtils.isPagaQ2(tipoNomina))
            {
                inicio = new Date(fechaDesde.Year, fechaDesde.Month, 16);
                fin = new Date(fechaHasta.Year, fechaHasta.Month, fechaHasta.Day);
            }
            else
            {
                inicio = new Date(fechaDesde.Year, fechaDesde.Month, 1);
                fin = new Date(fechaHasta.Year, fechaHasta.Month, DateTime.DaysInMonth(fechaHasta.Year, fechaHasta.Month));
            }
            var nominas = db.tblNomina_MX
                .Where(x =>
                    x.inicioPeriodo == inicio &&
                    x.finPeriodo == fin &&
                    x.idTipoNomina_MX == (short)tipoNomina &&
                    (
                        (x.idAdmElementoPEPNavigation != null && x.idAdmElementoPEPNavigation.idEmpresaPolarier == (short)idEmpresaPolarier) ||
                        (x.idAdmCentroCosteNavigation != null && x.idAdmCentroCosteNavigation.idEmpresaPolarier == (short)idEmpresaPolarier)
                    )
                );

            return nominas;
        }

        [HttpPost("odata/MyPolarier/Contabilidad/AsientosNominas/GuardarHistoricoAsientoNomina_MX")]
        [Authorize]
        public async Task<ActionResult> GuardarHistoricoAsientoNomina_MX([FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta, [FromODataUri] TipoNominaMX tipoNomina, [FromODataUri] idsEmpresaPolarier idEmpresaPolarier, [FromODataUri] int? idCentroCoste, [FromODataUri] int? idElementoPEP)
        {
            var idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

            var datosNominas = GetDatosNominas_MX(fechaDesde, fechaHasta, tipoNomina, idEmpresaPolarier)
                .Where(x => x.idAdmElementoPEP == idElementoPEP && x.idAdmCentroCoste == idCentroCoste)
                .ToList();

            GenerarHistoricoAsientoNomina_MX(datosNominas, idUsuario);

            await db.SaveChangesAsync();

            return Ok(true);
        }

        private void GenerarHistoricoAsientoNomina_MX(List<tblNomina_MX> datosNominas, int idUsuario, DateTime? fechaAsiento = null)
        {
            var fecha = DateTimeOffset.Now;

            var asientoNominaCentroSel = datosNominas
                .Select(dn => new tblHistoricoAsientoNomina_MX
                {
                    idNomina_MX = dn.idNomina_MX,
                    idTipoNomina_MX = dn.idTipoNomina_MX,
                    idAdmElementoPEP = dn.idAdmElementoPEP,
                    idAdmCentroCoste = dn.idAdmCentroCoste,
                    idUsuario = idUsuario,
                    fecha = fecha,
                    fechaContabilizado = fechaAsiento,
                    fechaDesde = dn.inicioPeriodo.Value,
                    fechaHasta = dn.finPeriodo.Value,
                    sueldo = dn.sueldo,
                    horasExtras = dn.horasExtras,
                    primaVacacional = dn.primaVacacional,
                    primaDominical = dn.primaDominical,
                    bono = dn.bono,
                    descansoTrabajado = dn.descansoTrabajado,
                    aguinaldo = dn.aguinaldo,
                    valesDespensa = dn.valesDespensa,
                    fondoAhorro = dn.fondoAhorro,
                    otraPercepcion = dn.otraPercepcion,
                    gastosSindicales = dn.gastosSindicales,
                    PTU = dn.PTU,
                    infonavitEmpleado = dn.infonavitEmpleado,
                    fonacotEmpleado = dn.fonacotEmpleado,
                    IMSSEmpleado = dn.IMSSEmpleado,
                    SAREmpleado = dn.SAREmpleado,
                    ISREmpleado = dn.ISREmpleado,
                    subsidioEmpleo = dn.subsidioEmpleo,
                    devolucionPrestamo = dn.devolucionPrestamo,
                    otrasDeducciones = dn.otrasDeducciones,
                    descAlimentos = dn.descAlimentos,
                    totalDeducciones = dn.totalDeducciones,
                    totalSP = dn.totalSP,
                    totalSV = dn.totalSV,
                    percepcionNeta = dn.percepcionNeta,
                    IMSSPatronal = dn.IMSSPatronal,
                    infonavitPatronal = dn.infonavitPatronal,
                    SARPatronal = dn.SARPatronal,
                    impuestoEstatalSobreNominas = dn.impuestoEstatalSobreNominas,
                });

            db.tblHistoricoAsientoNomina_MX.AddRange(asientoNominaCentroSel);
        }

        #endregion
    }

    public class CentroWrapp
    {
        public int idCentro { get; set; }
        public string denominacion { get; set; }
    }

    public class HistoricoNominaContabilidad : HistoricoNomina
    {
        public int? idTipoNomina { get; set; }
        public string idCentroElem { get; set; }
        public int idPersona { get; set; }
        public int? idElementoPEP { get; set; }
        public int? idCentroCoste { get; set; }
        public int? idCuentaContable_Salario { get; set; }
        public int? idCuentaContable_SSEmpresa { get; set; }
        public int? idTipoTrabajo { get; set; }
        public int? idCategoriaInterna { get; set; }
        public bool isRetenida { get; set; }
        public byte? idEstadoHistoricoAsientoNomina { get; set; }
        public string nombreCompleto { get; set; }
        public decimal? seguroConvenio { get; set; }
        public decimal? salarioEspecie { get; set; }
        public decimal? plusAsistenciaMesAnterior { get; set; }
        public decimal? descuentosSalariales { get; set; }
        public decimal? descuentoPreaviso { get; set; }
        public bool hasConceptosGestoria { get; set; }
        public short idEstadoNomina { get; set; }
        public byte? idMotivoBaja { get; set; }
        public DateTime fechaDesde { get; set; }
        public DateTime fechaHasta { get; set; }
        public DateTime? fechaBaja { get; set; }
        public bool isPendienteAnticipo { get; set; }
        public bool isPendientePagar { get; set; }
    }
}
