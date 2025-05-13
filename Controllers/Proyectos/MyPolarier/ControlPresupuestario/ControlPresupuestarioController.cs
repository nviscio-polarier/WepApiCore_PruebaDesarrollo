using WebApiCore.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Newtonsoft.Json;
using WebApiCore.Class.externos.SAP;
using WebApiCore.Class.externos.SAP.Context;
using WebApiCore.Class.externos.SAP.Controllers;
using Microsoft.AspNetCore.OData.Formatter;
using WebApiCore.Context;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using WebApiCore.Hubs;
using WebApiCore.Enums.General;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Net;

namespace WebApiCore.Controllers.Proyectos.MyPolarier.ControlPresupuestario
{
    public class ControlPresupuestarioController : ODataController
    {
        private readonly SAPWrap sap = new();
        private readonly bdERP db;

        public enum VistaControlPresupuestario : byte
        {
            Mensual = 1,
            Acumulado = 2
        }

        public enum TipoCentro : byte
        {
            CentroCoste = 1,
            ElementoPEP = 2
        }

        public ControlPresupuestarioController(bdERP context, IHubContext<NotificacionesHub> hubContext)
        {
            db = context;

        }

        private bool userHasAccess(string codigo)
        {
            var idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
            var objUsuario = db.tblUsuario.FirstOrDefault(x => x.idUsuario.Equals(idUsuario));
            if (objUsuario == null)
            {
                return false;
            }
            var permisoConcedido = !db.tblUsuario.Where(x => x.idUsuario == idUsuario && x.idPermiso.Any(y => y.codigo == codigo)).IsNullOrEmpty(); //Revisa si el usuario tiene el permiso asociado a su cuenta (tblPermisoNUsuario)
            return permisoConcedido || objUsuario.idCargo == 1; // Desarrollador
        }

        [EnableQuery]
        [HttpGet("odata/MyPolarier/ControlPresupuestario")]
        [Authorize]
        public async Task<ActionResult> ControlPresupuestario([FromODataUri] short idEmpresaPolarier, [FromODataUri] DateTime? fecha, [FromODataUri] VistaControlPresupuestario vista, [FromODataUri] string idsCentroCosteJSON, [FromODataUri] string idsElementoPEPJSON)
        {

            if (fecha == null) return BadRequest("Fecha no establecida");
            var idsCentroCoste = JsonConvert.DeserializeObject<List<int?>>(idsCentroCosteJSON);
            var idsElementoPEP = JsonConvert.DeserializeObject<List<int?>>(idsElementoPEPJSON);
            if (idsCentroCoste.Count == 0 && idsElementoPEP.Count == 0) return BadRequest("Centro de coste / Elemento PEP no establecido");

            var objEmpresa = db.tblEmpresasPolarier.Find(idEmpresaPolarier);
            if (objEmpresa == null) return BadRequest("Empresa no encontrada");

            if (vista == VistaControlPresupuestario.Acumulado)
            {
                List<DateTime> fechasCierre = new List<DateTime>();
                DateTime fechaInicio = new DateTime(fecha.Value.Year, 1, 1); // Enero del año de la fecha
                DateTime fechaFin = new DateTime(fecha.Value.Year, fecha.Value.Month, 1); // Mes de la fecha

                for (DateTime fechaActual = fechaInicio; fechaActual <= fechaFin; fechaActual = fechaActual.AddMonths(1))
                    fechasCierre.Add(fechaActual);

                var todosCerrados = 
                    db.tblCierrePresupuestario
                    .Where(x => 
                        x.idEmpresaPolarier == idEmpresaPolarier && 
                        x.fecha >= fechaInicio && x.fecha <= fechaFin &&
                        (
                            (x.idAdmCentroCoste != null && idsCentroCoste.Contains(x.idAdmCentroCoste)) ||
                            (x.idAdmElementoPEP != null && idsElementoPEP.Contains(x.idAdmElementoPEP))
                        )
                    )
                    .ToList()
                    .GroupBy(x => new { x.idAdmCentroCoste, x.idAdmElementoPEP })
                    .All(x => fechasCierre.All(fechaCierre => x.Select(x => x.fecha).Contains(fechaCierre)));

                if (!todosCerrados) return Ok(JsonConvert.DeserializeObject("[]"));
            }


            var tblCierrePresupuestario = db.tblCierrePresupuestario
                .Include(x => x.tblPartidaNCierrePresupuestario)
                .Include(x => x.tblPlanificacionNCierrePresupuestario)
                .Include(x => x.idAdmElementoPEPNavigation)
                .Where(cp =>
                cp.idEmpresaPolarier == idEmpresaPolarier &&
                cp.fecha.Year == fecha.Value.Year && (vista == VistaControlPresupuestario.Mensual ?
                    cp.fecha.Month == fecha.Value.Month :
                    cp.fecha.Month <= fecha.Value.Month) &&
                (
                    (
                        cp.idAdmCentroCoste != null &&
                        idsCentroCoste.Contains(cp.idAdmCentroCoste)
                    ) ||
                    (
                        cp.idAdmElementoPEP != null &&
                        (
                            (cp.idAdmElementoPEP != null && idsElementoPEP.Contains(cp.idAdmElementoPEPNavigation.idAdmElementoPEPPadre)) ||
                            idsElementoPEP.Contains(cp.idAdmElementoPEP)
                        )
                    )
                )
            )
            .ToList()
            .SelectMany(x => x.tblPartidaNCierrePresupuestario.Select(pc => new
                {
                    x.idAdmCentroCoste,
                    x.idAdmElementoPEP,
                    pc.idAdmCuentaContable,
                    idProyecto = JsonConvert.SerializeObject(new List<int> { x.idAdmCentroCoste ?? -1, x.idAdmElementoPEPNavigation?.idAdmElementoPEPPadre ?? x.idAdmElementoPEP ?? -1 }),
                    Partida = new ModeloPartida
                    {
                        idCentroElem = JsonConvert.SerializeObject(new List<int> { x.idAdmCentroCoste ?? -1, x.idAdmElementoPEP ?? -1 }),
                        idAdmCuentaContable = pc.idAdmCuentaContable,
                        asientoDocumento = pc.asientoDocumento,
                        fechaDocumento = pc.fechaDocumento,
                        comentarioDocumento = pc.comentarioDocumento,
                        valor = pc.valor,
                        idMoneda = pc.idMoneda
                    },
                    Planificacion = (ModeloPlanificacion)null
                })
                .Concat(x.tblPlanificacionNCierrePresupuestario.Select(pc => new
                {
                    x.idAdmCentroCoste,
                    x.idAdmElementoPEP,
                    pc.idAdmCuentaContable,
                    idProyecto = JsonConvert.SerializeObject(new List<int> { x.idAdmCentroCoste ?? -1, x.idAdmElementoPEPNavigation?.idAdmElementoPEPPadre ?? x.idAdmElementoPEP ?? -1 }),
                    Partida = (ModeloPartida)null,
                    Planificacion = new ModeloPlanificacion
                    {
                        idCentroElem = JsonConvert.SerializeObject(new List<int> { x.idAdmCentroCoste ?? -1, x.idAdmElementoPEP ?? -1 }),
                        idAdmCuentaContable = pc.idAdmCuentaContable,
                        valor = pc.valor,
                        idMoneda = pc.idMoneda
                    }
                }))
            )
            .GroupBy(p => new { p.idAdmCentroCoste, p.idAdmElementoPEP, p.idAdmCuentaContable, p.idProyecto })
            .Select(g => new ModeloCuentaContable
            {
                idAdmCentroCoste = g.Key.idAdmCentroCoste,
                idAdmElementoPEP = g.Key.idAdmElementoPEP,
                idAdmCuentaContable = g.Key.idAdmCuentaContable,
                idProyecto = g.Key.idProyecto,
                tblPartida = g.Where(p => p.Partida != null).Select(p => p.Partida).ToList(),
                tblPlanificacion = g.Where(p => p.Planificacion != null).Select(p => p.Planificacion).ToList()
            })
            .ToList();


            return Ok(tblCierrePresupuestario);
        }

        [EnableQuery]
        [HttpGet("odata/MyPolarier/ControlPresupuestario/SAP")]
        [Authorize]
        public async Task<ActionResult> ControlPresupuestarioSAP([FromODataUri] short idEmpresaPolarier, [FromODataUri] DateTime? fecha, [FromODataUri] VistaControlPresupuestario vista, [FromODataUri] string idsCentroCosteJSON, [FromODataUri] string idsElementoPEPJSON)
        {

            if (fecha == null) return BadRequest("Fecha no establecida");
            var idsCentroCoste = JsonConvert.DeserializeObject<List<int?>>(idsCentroCosteJSON);
            var idsElementoPEP = JsonConvert.DeserializeObject<List<int?>>(idsElementoPEPJSON);
            if (idsCentroCoste.Count == 0 && idsElementoPEP.Count == 0) return BadRequest("Centro de coste / Elemento PEP no establecido");

            var objEmpresa = db.tblEmpresasPolarier.Find(idEmpresaPolarier);
            if (objEmpresa == null) return BadRequest("Empresa no encontrada");

            try
            {
                var datosSAP = await getDatosSAPAsync(objEmpresa, fecha, vista, idsCentroCoste, idsElementoPEP);
                return Ok(datosSAP);
            }
            catch (Exception ex)
            {
                if (ex.Message == "timeoutSAP")
                    return StatusCode((int)HttpStatusCode.GatewayTimeout, ex.Message);
                else throw;
            }
        }

        private async Task<List<ModeloCuentaContable>> getDatosSAPAsync(tblEmpresasPolarier objEmpresa, DateTime? fecha, VistaControlPresupuestario vista, List<int?> idsCentroCoste, List<int?> idsElementoPEP)
        {
            List<PartidaContableJSON> partidasSAP = new();
            List<PlanificacionJSON> planificacionSAP = new();

            List<CentroSel> tblCentroSel = new();
            tblCentroSel.AddRange(db.tblAdmCentroCoste
                .Where(x => idsCentroCoste.Contains(x.idAdmCentroCoste))
                .Select(x => new CentroSel
                {
                    tipoCentro = TipoCentro.CentroCoste,
                    codigo = x.codigo
                }).ToList());
            tblCentroSel.AddRange(db.tblAdmElementoPEP
                .Where(x => idsElementoPEP.Contains(x.idAdmElementoPEP))
                .Select(x => new CentroSel
                {
                    tipoCentro = TipoCentro.ElementoPEP,
                    codigo = x.codigo
                }).ToList());

            var tasks = new List<Task<LlamadasSAP>>();
            foreach (var item in tblCentroSel)
            {
                for (int i = vista == VistaControlPresupuestario.Mensual ? fecha.Value.Month : 1; i <= fecha.Value.Month; i++)
                {
                    DateTime mes = new(fecha.Value.Year, i, 1);
                    tasks.Add(Task.Run(async () =>
                    {
                        try
                        {
                            return await GetPartidasContablesSAP(objEmpresa, item, mes);
                        }
                        catch
                        {
                            throw new Exception("timeoutSAP");
                        }
                    }));
                }
            }

            var results = await Task.WhenAll(tasks);
            foreach (var SAP in results)
            {
                partidasSAP.AddRange(SAP.partidasSAP);
                planificacionSAP.AddRange(SAP.planificacionSAP);
            }

            var tblMoneda = db.tblMoneda.ToDictionary(m => m.codigo, m => m.idMoneda);
            var tblAdmCuentaContable = db.tblAdmCuentaContable.Where(x =>
                    partidasSAP.Select(y => y.codigoCuentaContable).Distinct().Contains(x.codigo) ||
                    planificacionSAP.Select(y => y.codigoCuentaContable).Distinct().Contains(x.codigo)
                ).ToDictionary(cc => cc.codigo, cc => cc);

            var dic_tblAdmCentroCoste = db.tblAdmCentroCoste
                .Where(x =>
                    x.isEliminado == false &&
                    (
                        partidasSAP
                            .Select(y => y.codigoCentroCoste).Distinct()
                            .ToList().Contains(x.codigo) ||
                        planificacionSAP
                            .Select(y => y.codigoCentroCoste).Distinct()
                            .ToList().Contains(x.codigo)
                        )
                ).ToDictionary(x => x.codigo ?? "", x => x);

            var dic_tblAdmElementoPEP = db.tblAdmElementoPEP
                .Where(x =>
                    x.isEliminado == false &&
                    (
                        partidasSAP
                            .Select(y => y.codigoElementoPEP).Distinct()
                            .ToList().Contains(x.codigo) ||
                        partidasSAP
                            .Select(y => y.codigoProyecto).Distinct()
                            .ToList().Contains(x.codigo) ||
                        planificacionSAP
                            .Select(y => y.codigoElementoPEP).Distinct()
                            .ToList().Contains(x.codigo) ||
                        planificacionSAP
                            .Select(y => y.codigoProyecto).Distinct()
                            .ToList().Contains(x.codigo)
                    )
                ).ToDictionary(x => x.codigo ?? "", x => x);

            var modelosPartida = partidasSAP
            .GroupBy(x => new
            {
                x.codigoCentroCoste,
                x.codigoElementoPEP,
                x.codigoCuentaContable,
                x.codigoProyecto
            }).Select(x =>
            {
                int? idAdmCentroCoste = dic_tblAdmCentroCoste.TryGetValue(x.Key.codigoCentroCoste, out var cc) ? cc.idAdmCentroCoste : null;
                int? idAdmElementoPEP = dic_tblAdmElementoPEP.TryGetValue(x.Key.codigoElementoPEP, out var pep) ? pep.idAdmElementoPEP : null;
                int idAdmCuentaContable = tblAdmCuentaContable.TryGetValue(x.Key.codigoCuentaContable, out var cuentaContable) ? cuentaContable.idAdmCuentaContable : 0;
                int? idProyecto = dic_tblAdmElementoPEP.TryGetValue(x.Key.codigoProyecto, out var proyecto) ? proyecto.idAdmElementoPEP : null;

                return new ModeloCuentaContable()
                {
                    idAdmCentroCoste = idAdmCentroCoste,
                    idAdmElementoPEP = idAdmElementoPEP,
                    idAdmCuentaContable = idAdmCuentaContable,
                    idProyecto = JsonConvert.SerializeObject(new List<int> { idAdmCentroCoste ?? -1, idProyecto ?? idAdmElementoPEP ?? -1}),
                    tblPartida = x.Select(y => new ModeloPartida
                    {
                        idCentroElem = JsonConvert.SerializeObject(new List<int> { idAdmCentroCoste ?? -1, idAdmElementoPEP ?? -1}),
                        idAdmCuentaContable = idAdmCuentaContable,
                        asientoDocumento = y.asientoDocumento,
                        fechaDocumento = y.fechaDocumento,
                        comentarioDocumento = y.comentarioDocumento,
                        valor = (y.valor * -1) ?? 0,
                        idMoneda = tblMoneda.TryGetValue(y.codigoMoneda, out var idMoneda) ? idMoneda : (byte)idsMoneda.Euro
                        
                    }).ToList(),
                    tblPlanificacion = new()
                };
            }).ToList();

            var modelosPlanificacion = planificacionSAP
            .Where(x =>
                (!dic_tblAdmCentroCoste.TryGetValue(x.codigoCentroCoste, out var cc) || cc.idEmpresaPolarier == objEmpresa.idEmpresaPolarier) &&
                (!dic_tblAdmElementoPEP.TryGetValue(x.codigoElementoPEP, out var pep) || pep.idEmpresaPolarier == objEmpresa.idEmpresaPolarier)
            )
            .GroupBy(x => new
            {
                x.codigoCentroCoste,
                x.codigoElementoPEP,
                x.codigoCuentaContable,
                x.codigoProyecto
            }).Select(x =>
            {
                int? idAdmCentroCoste = dic_tblAdmCentroCoste.TryGetValue(x.Key.codigoCentroCoste, out var cc) ? cc.idAdmCentroCoste : null;
                int? idAdmElementoPEP = dic_tblAdmElementoPEP.TryGetValue(x.Key.codigoElementoPEP, out var pep) ? pep.idAdmElementoPEP : null;
                int idAdmCuentaContable = tblAdmCuentaContable.TryGetValue(x.Key.codigoCuentaContable, out var cuentaContable) ? cuentaContable.idAdmCuentaContable : 0;
                int? idProyecto = dic_tblAdmElementoPEP.TryGetValue(x.Key.codigoProyecto, out var proyecto) ? proyecto.idAdmElementoPEP : null;

                return new ModeloCuentaContable()
                {
                    idAdmCentroCoste = idAdmCentroCoste,
                    idAdmElementoPEP = idAdmElementoPEP,
                    idAdmCuentaContable = idAdmCuentaContable,
                    idProyecto = JsonConvert.SerializeObject(new List<int> { idAdmCentroCoste ?? -1, idProyecto ?? idAdmElementoPEP ?? -1}),
                    tblPartida = new(),
                    tblPlanificacion = x.Select(y => new ModeloPlanificacion
                    {
                        idCentroElem = JsonConvert.SerializeObject(new List<int> { idAdmCentroCoste ?? -1, idAdmElementoPEP ?? -1}),
                        idAdmCuentaContable = idAdmCuentaContable,
                        valor = (y.valor * -1) ?? 0,
                        idMoneda = tblMoneda.TryGetValue(y.codigoMoneda, out var idMoneda) ? idMoneda : (byte)idsMoneda.Euro
                        
                    }).ToList(),
                };
            }).ToList();

            var modelosCuenta = modelosPartida.Concat(modelosPlanificacion)
                .GroupBy(x => new { x.idAdmCentroCoste, x.idAdmElementoPEP, x.idAdmCuentaContable, x.idProyecto })
                .Select(x => new ModeloCuentaContable
                {
                    idAdmCentroCoste = x.Key.idAdmCentroCoste,
                    idAdmElementoPEP = x.Key.idAdmElementoPEP,
                    idAdmCuentaContable = x.Key.idAdmCuentaContable,
                    idProyecto = x.Key.idProyecto,
                    tblPartida = x.SelectMany(y => y.tblPartida).ToList(),
                    tblPlanificacion = x.SelectMany(y => y.tblPlanificacion).ToList()
                })
                //TODO: ELIMINAR FILTRO DE CUENTAS CON CENTRO COSTE Y ELEMENTO PEP Y HACER DESARROLLO DE CONTROLAR ERROR EN MYPOLARIER
                .Where(x => x.idAdmCentroCoste == null || x.idAdmElementoPEP == null)
                .ToList();

            return modelosCuenta;
        }

        private async Task<LlamadasSAP> GetPartidasContablesSAP(tblEmpresasPolarier objEmpresa, CentroSel centroSel, DateTime fecha)
        {
            List<PartidaContableJSON> partidasSAP;
            List<PlanificacionJSON> planificacionSAP;

            var partidasOdata = $"$filter=" +
                $"CompanyCode eq '{objEmpresa.companyCode_SAP}' and " +
                $"(startswith(GLAccount, '6') or startswith(GLAccount, '7')) and " +
                $"not(startswith(GLAccount, '6300')) and " +
                $"FiscalYear eq '{fecha.Year}' and " +
                $"FiscalPeriod eq '0{fecha.Month}' and " +
                GetCentroOdataFilter(centroSel, "ProjectExternalID") +
                $"&$select=ID,CompanyCode,CostCenter,WBSElementExternalID,ProjectExternalID,GLAccount," +
                $"AmountInCompanyCodeCurrency,CompanyCodeCurrency,AccountingDocument,PostingDate,DocumentItemText";
            var partidasContablesResponse = sap.partidasContablesController.Get(partidasOdata);

            var planificacionOdata = $"$filter=" +
                $"CompanyCode eq '{objEmpresa.companyCode_SAP}' and " +
                $"(startswith(GLAccount, '6') or startswith(GLAccount, '7')) and " +
                $"not(startswith(GLAccount, '6300')) and " +
                $"FiscalYear eq '{fecha.Year}' and " +
                $"FiscalPeriod eq '0{fecha.Month}' and " +
                GetCentroOdataFilter(centroSel, "Project") +
                $"&$select=CompanyCode,CostCenter,WBSElement,Project,GLAccount,AmountInCompanyCodeCurrency,CompanyCodeCurrency";
            var planificacionResponse = sap.planificacionController.Get(planificacionOdata);

            Task.WaitAll(partidasContablesResponse, planificacionResponse);

            SAPUtils.TryDeserializeResponse<PartidasContables.Model>(partidasContablesResponse.Result, out var partidasContables);
            if (partidasContables != null && partidasContables.entry != null)
            {
                string partidasJsonString = JsonConvert.SerializeObject(partidasContables.entry.Select(x => x.content.properties).ToArray());
                partidasSAP = JsonConvert.DeserializeObject<List<PartidaContableJSON>>(partidasJsonString)
                    .Where(x => !string.IsNullOrEmpty(x.codigoSociedad)).ToList();
            }
            else
            {
                partidasSAP = new List<PartidaContableJSON>();
            }

            SAPUtils.TryDeserializeResponse<Planificacion.Model>(planificacionResponse.Result, out var planificacion);
            if (planificacion != null && planificacion.entry != null)
            {
                string planificacionJsonString = JsonConvert.SerializeObject(planificacion.entry.Select(x => x.content.properties).ToArray());
                planificacionSAP = JsonConvert.DeserializeObject<List<PlanificacionJSON>>(planificacionJsonString)
                                        .Where(x => !string.IsNullOrEmpty(x.codigoSociedad)).ToList();
            }
            else
            {
                planificacionSAP = new List<PlanificacionJSON>();
            }

            return new LlamadasSAP { 
                partidasSAP = partidasSAP, 
                planificacionSAP = planificacionSAP
            };
        }

        private static string GetCentroOdataFilter(CentroSel centroSel, string campo)
        {
            if (centroSel.tipoCentro == TipoCentro.CentroCoste)
            {
                return $"CostCenter eq '{centroSel.codigo}'";
            }
            else
            {
                return $"{campo} eq '{centroSel.codigo}'";

            }
        }

        [HttpPost("odata/MyPolarier/ControlPresupuestario/CerrarCentros")]
        [Authorize]
        public async Task<ActionResult> CerrarCentros([FromODataUri] short idEmpresaPolarier, [FromODataUri] DateTime fecha, [FromODataUri] byte idMoneda, [FromBody] CerrarCentrosPayload payload)
        {
            var idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
            var objUsuario = db.tblUsuario.FirstOrDefault(x => x.idUsuario.Equals(idUsuario));
            if (objUsuario == null)
            {
                return BadRequest();
            }
            if (!objUsuario.isDepartamentoControl && !userHasAccess("escrituraControlPresupuestario"))
            {
                return BadRequest();
            }
            var idsPEP = payload.datosCierre
                .Select(x => x.idAdmElementoPEP)
                .Concat(payload.idsElementoPEP.Select(x => (int?)x))
                .Distinct()
                .ToList();
            var idsCC = payload.datosCierre
                .Select(x => x.idAdmCentroCoste)
                .Concat(payload.idsCentroCoste.Select(x => (int?)x))
                .Distinct()
                .ToList();

            var tblDb = db.tblCierrePresupuestario
                .Where(pc =>
                    pc.idEmpresaPolarier == idEmpresaPolarier &&
                    pc.fecha == fecha &&
                    (
                        (pc.idAdmCentroCoste != null && idsCC.Contains(pc.idAdmCentroCoste)) ||
                        (pc.idAdmElementoPEP != null && idsPEP.Contains(pc.idAdmElementoPEP))
                    )
            );

            db.tblPartidaNCierrePresupuestario.RemoveRange(tblDb.SelectMany(pc => pc.tblPartidaNCierrePresupuestario));
            db.tblPlanificacionNCierrePresupuestario.RemoveRange(tblDb.SelectMany(pc => pc.tblPlanificacionNCierrePresupuestario));
            db.tblCierrePresupuestario.RemoveRange(tblDb);

            var tblCierrePresupuestario = payload.datosCierre.GroupBy(x => new { x.idAdmCentroCoste, x.idAdmElementoPEP })
                .Select(x =>
                {
                    return new tblCierrePresupuestario
                    {
                        idEmpresaPolarier = idEmpresaPolarier,
                        idAdmCentroCoste = x.Key.idAdmCentroCoste,
                        idAdmElementoPEP = x.Key.idAdmElementoPEP,
                        fecha = fecha,
                        tblPartidaNCierrePresupuestario = x.SelectMany(cc =>
                            cc.tblPartida.Select(x => new tblPartidaNCierrePresupuestario
                            {
                                idAdmCuentaContable = x.idAdmCuentaContable,
                                asientoDocumento = x.asientoDocumento,
                                comentarioDocumento = x.comentarioDocumento,
                                fechaDocumento = x.fechaDocumento,
                                valor = x.valor,
                                idMoneda = x.idMoneda,
                            }).ToList()
                        ).ToList(),
                        tblPlanificacionNCierrePresupuestario = x.SelectMany(cc =>
                            cc.tblPlanificacion.Select(x => new tblPlanificacionNCierrePresupuestario
                            {
                                idAdmCuentaContable = x.idAdmCuentaContable,
                                valor = x.valor,
                                idMoneda = x.idMoneda,
                            }).ToList()
                        ).ToList(),
                    };
                })
                .ToList();

            tblCierrePresupuestario.AddRange(
                payload.idsElementoPEP
                .Where(pep => !tblCierrePresupuestario.Any(cp => pep == cp.idAdmElementoPEP))
                .Select(idAdmElementoPEP => new tblCierrePresupuestario
                {
                    idEmpresaPolarier = idEmpresaPolarier,
                    idAdmElementoPEP = idAdmElementoPEP,
                    fecha = fecha,
                })
            );

            tblCierrePresupuestario.AddRange(
                payload.idsCentroCoste
                .Where(pep => !tblCierrePresupuestario.Any(cp => pep == cp.idAdmCentroCoste))
                .Select(idAdmCentroCoste => new tblCierrePresupuestario
                {
                    idEmpresaPolarier = idEmpresaPolarier,
                    idAdmCentroCoste = idAdmCentroCoste,
                    fecha = fecha,
                })
            );

            db.tblCierrePresupuestario.AddRange(tblCierrePresupuestario);

            await db.SaveChangesAsync();

            return Ok(true);
        }

        [EnableQuery]
        [HttpGet("odata/MyPolarier/ControlPresupuestario/AjustesPresupuestarios")]
        [Authorize]
        public async Task<ActionResult> AjustesPresupuestarios([FromODataUri] DateTime fecha, [FromODataUri] VistaControlPresupuestario vista, [FromODataUri] string idsCentroCosteJSON, [FromODataUri] string idsElementoPEPJSON)
        {
            var idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
            var objUsuario = db.tblUsuario.FirstOrDefault(x => x.idUsuario.Equals(idUsuario));
            if (objUsuario == null)
            {
                return null; //BadRequest();
            }
            var idsCentroCoste = JsonConvert.DeserializeObject<List<int?>>(idsCentroCosteJSON);
            var idsCentroCosteVisibles = db.tblAdmCentroCoste.Where(x => x.idUsuario.Any(y => y.idUsuario == idUsuario) && idsCentroCoste.Contains((int?)x.idAdmCentroCoste)).Select(x => (int?)x.idAdmCentroCoste);
            var idsElementoPEP = JsonConvert.DeserializeObject<List<int?>>(idsElementoPEPJSON);
            var idsElementoPEPVisibles = db.tblAdmElementoPEP.Where(x => x.idUsuario.Any(y => y.idUsuario == idUsuario) && idsElementoPEP.Contains((int?)x.idAdmElementoPEP)).Select(x => (int?)x.idAdmElementoPEP);
            if (!objUsuario.isDepartamentoControl && !userHasAccess("lecturaTotalControlPresupuestario"))
            {
                idsCentroCoste = idsCentroCoste.Where(x => x.HasValue && idsCentroCosteVisibles.Contains(x.Value)).ToList();
                idsElementoPEP = idsElementoPEP.Where(x => x.HasValue && idsElementoPEPVisibles.Contains(x.Value)).ToList();
            }

            if (idsCentroCoste.Count == 0 && idsElementoPEP.Count == 0) return BadRequest("Centro de coste / Elemento PEP no establecido");

            switch (vista)
            {
                case VistaControlPresupuestario.Mensual:
                    return Ok(db.tblAjustePresupuestario.Where(x =>
                        (
                            idsCentroCoste.Contains(x.idAdmCentroCoste) ||
                            idsElementoPEP.Contains(x.idAdmElementoPEP) ||
                            (x.idAdmElementoPEPNavigation != null && idsElementoPEP.Contains(x.idAdmElementoPEPNavigation.idAdmElementoPEPPadre))
                        ) &&
                        x.tblMesNAjustePresupuestario.OrderBy(x => x.fecha).FirstOrDefault().fecha.Year == fecha.Year &&
                        x.tblMesNAjustePresupuestario.OrderBy(x => x.fecha).FirstOrDefault().fecha.Month == fecha.Month
                    ).Select(x => new
                    {
                        x.idAjustePresupuestario,
                        x.idAdmCentroCoste,
                        x.idAdmElementoPEP,
                        idProyecto = JsonConvert.SerializeObject(new List<int> { x.idAdmCentroCoste ?? -1, x.idAdmElementoPEPNavigation.idAdmElementoPEPPadre ?? x.idAdmElementoPEP ?? -1 }),
                        x.idAdmCuentaContable,
                        x.idMoneda,
                        x.fecha,
                        x.valor,
                        x.observaciones,
                    }));
                case VistaControlPresupuestario.Acumulado:
                    return Ok(db.tblAjustePresupuestario.Where(x =>
                        (
                            idsCentroCoste.Contains(x.idAdmCentroCoste) ||
                            idsElementoPEP.Contains(x.idAdmElementoPEP) ||
                            (x.idAdmElementoPEPNavigation != null && idsElementoPEP.Contains(x.idAdmElementoPEPNavigation.idAdmElementoPEPPadre))
                        ) &&
                        x.tblMesNAjustePresupuestario.Any(x => x.fecha.Year == fecha.Year && x.fecha.Month == fecha.Month)
                    ).Select(x => new
                    {
                        x.idAjustePresupuestario,
                        x.idAdmCentroCoste,
                        x.idAdmElementoPEP,
                        idProyecto = JsonConvert.SerializeObject(new List<int> { x.idAdmCentroCoste ?? -1, x.idAdmElementoPEPNavigation.idAdmElementoPEPPadre ?? x.idAdmElementoPEP ?? -1 }),
                        x.idAdmCuentaContable,
                        x.idMoneda,
                        x.fecha,
                        x.valor,
                        x.observaciones,
                    }));
                default:
                    return BadRequest();
            }
        }

        private class CentroSel
        {
            public TipoCentro tipoCentro { get; set; }
            public string codigo { get; set; }
        }

        public class CerrarCentrosPayload { 
            public List<ModeloCuentaContable> datosCierre { get; set; }
            public List<int> idsCentroCoste { get; set; }
            public List<int> idsElementoPEP { get; set; }
        }

        private class LlamadasSAP
        {
            public List<PartidaContableJSON> partidasSAP { get; set; }
            public List<PlanificacionJSON> planificacionSAP { get; set; }
        }

        private class PlanificacionJSON
        {
            [JsonProperty("CostCenter")]
            public string? codigoCentroCoste { get; set; }
            [JsonProperty("WBSElement")]
            public string? codigoElementoPEP { get; set; }
            [JsonProperty("Project")]
            public string? codigoProyecto { get; set; }
            [JsonProperty("GLAccount")]
            public string? codigoCuentaContable { get; set; }
            [JsonProperty("CompanyCode")]
            public string? codigoSociedad { get; set; }
            [JsonProperty("AmountInCompanyCodeCurrency")]
            public decimal? valor { get; set; }
            [JsonProperty("CompanyCodeCurrency")]
            public string? codigoMoneda { get; set; }
        }

        private class PartidaContableJSON
        {
            [JsonProperty("ID")]
            public string? idSAP { get; set; }
            [JsonProperty("CostCenter")]
            public string? codigoCentroCoste { get; set; }
            [JsonProperty("WBSElementExternalID")]
            public string? codigoElementoPEP { get; set; }
            [JsonProperty("ProjectExternalID")]
            public string? codigoProyecto { get; set; }
            [JsonProperty("GLAccount")]
            public string? codigoCuentaContable { get; set; }
            [JsonProperty("CompanyCode")]
            public string? codigoSociedad { get; set; }
            [JsonProperty("AmountInCompanyCodeCurrency")]
            public decimal? valor { get; set; }
            [JsonProperty("CompanyCodeCurrency")]
            public string? codigoMoneda { get; set; }
            [JsonProperty("AccountingDocument")]
            public string? asientoDocumento { get; set; }
            [JsonProperty("PostingDate")]
            public DateTime? fechaDocumento { get; set; }
            [JsonProperty("DocumentItemText")]
            public string? comentarioDocumento { get; set; }
        }

        public class ModeloCuentaContable
        {
            public int? idAdmCentroCoste { get; set; }
            public int? idAdmElementoPEP { get; set; }
            public int idAdmCuentaContable { get; set; }
            public string idProyecto { get; set; }
            public List<ModeloPartida> tblPartida { get; set; }
            public List<ModeloPlanificacion> tblPlanificacion { get; set; }
        }

        public class ModeloPartida
        {
            public string idCentroElem { get; set; }
            public int idAdmCuentaContable { get; set; }
            public string asientoDocumento { get; set; }
            public DateTime? fechaDocumento { get; set; }
            public string comentarioDocumento { get; set; }
            public decimal valor { get; set; }
            public byte idMoneda { get; set; }
        }

        public class ModeloPlanificacion
        {
            public string idCentroElem { get; set; }
            public int idAdmCuentaContable { get; set; }
            public decimal valor { get; set; }
            public byte idMoneda { get; set; }
        }
    }
}
