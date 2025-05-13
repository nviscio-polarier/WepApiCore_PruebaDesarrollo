using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApiCore.Class;
using WebApiCore.Context;
using WebApiCore.Hubs;
using WebApiCore.Security;

namespace WebApiCore.Controllers.Proyectos.MyPolarier.Dashboard.SmartView
{
    public class SmartViewController : ODataController
    {
        private readonly bdERP db;
        private readonly IHubContext<NotificacionesHub> _hubContext;
        public SmartViewController(bdERP context, IHubContext<NotificacionesHub> hubContext)
        {
            db = context;
            _hubContext = hubContext;
        }
        [EnableQuery]
        [Authorize]
        [HttpPost("odata/Dashboard/SmartView/tblPersonaFotos")]
        public async Task<ActionResult> tblPersonaFotos([FromBody] List<int> ids)
        {
            return Ok(
                db.tblPersona
                    .Where(x => ids.Contains(x.idPersona))
                    .Include(x => x.idFotoPerfilNavigation)
                    .Select(x => new { x.idPersona, x.idFotoPerfilNavigation.documento })
                );
        }

        [HttpGet("odata/Dashboard/SmartView/spSelectPersonasKgHora")]
        [Authorize]
        public async Task<ActionResult> spSelectPersonasKgHora([FromODataUri] int idLavanderia)
        {
            var connection = db.Database.GetDbConnection();
            var results = await connection.QueryAsync("EXEC [MyRealData].[EF_SmartView_spSelectPersonasKgHora] @idLavanderia", new { idLavanderia });
            return Ok(results.ToList());
        }

        [HttpGet("odata/Dashboard/SmartView/spSelectPersonas")]
        [Authorize]
        public async Task<ActionResult> spSelectPersonas([FromODataUri] int idLavanderia)
        {
            var connection = db.Database.GetDbConnection();
            var results = await connection.QueryAsync("EXEC [MyRealData].[EF_SmartView_spSelectPersonas] @idLavanderia", new { idLavanderia });
            return Ok(results.ToList());
        }

        [HttpGet("odata/Dashboard/SmartView/DatosProduccion")]
        [Authorize]
        public async Task<ActionResult> GetDatosProduccion([FromODataUri] int idLavanderia, [FromODataUri] int minsEsperaInicio = 1, [FromODataUri] int minsProduccionActual = 5, [FromODataUri] int minsCooldownTiempoPerdido = 3)
        {

            #region Aplicar offset lavanderia a fecha actual

            var tblLavanderia = db.tblLavanderia
           .Where(x => x.idLavanderia.Equals(idLavanderia))
           .Select(x => new
           {
               x.idLavanderia,
               x.horarioVerano,
               x.idZonaHorariaNavigation
           }).FirstOrDefault();

            int gmt = (tblLavanderia.horarioVerano == true ? 1 : 0) + Convert.ToInt32(tblLavanderia.idZonaHorariaNavigation.GMT);

            DateTimeOffset offset = (DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(gmt)));
            #endregion

            var data = db.tblClienteNMaquina.Where(x => x.fechaFin == null && x.idMaquinaNavigation.idLavanderia == idLavanderia).Select(cnm => new
            {
                cnm.idMaquina,
                cnm.fechaIni,
                prendasCliente = cnm.fechaIni < offset.AddMinutes(-minsEsperaInicio) ?
                            db.tblPrendaNMaquina.Where(x => x.idMaquina == cnm.idMaquina && x.fecha > cnm.fechaIni).Count()
                            : 0,
                prendasActuales = cnm.fechaIni < offset.AddMinutes(-minsEsperaInicio) ?
                            db.tblPrendaNMaquina.Where(x => x.idMaquina == cnm.idMaquina && x.fecha > cnm.fechaIni && x.fecha > offset.AddMinutes(-minsProduccionActual)).Count()
                            : 0,
                tblPrendaNMaquina = db.tblPrendaNMaquina
                                        .Where(pnm => pnm.idMaquina == cnm.idMaquina && pnm.fecha >= cnm.fechaIni)
                                        .ToList()
            }).ToList();

            var result = new { datosProduccion = data.Select(cnm =>
            {
                TimeSpan tiempoPerdido = TimeSpan.Zero;

                if (cnm.tblPrendaNMaquina.Any())
                {
                    // Si hay prendas, calcular tiempo perdido
                    var tiempoHastaPrimeraPrenda = cnm.tblPrendaNMaquina.Select(pnm => pnm.fecha).Min().Subtract(cnm.fechaIni);
                    var tiempoDesdeUltimaPrenda = offset.Subtract(cnm.tblPrendaNMaquina.Select(pnm => pnm.fecha).Max());
                    
                    // Desde inicio hasta primera prenda
                    tiempoPerdido += tiempoHastaPrimeraPrenda.TotalMinutes >= minsCooldownTiempoPerdido ? tiempoHastaPrimeraPrenda : TimeSpan.Zero;

                    // Entre prendas
                    tiempoPerdido += cnm.tblPrendaNMaquina
                        .OrderBy(pnm => pnm.fecha)
                        .Zip(
                            cnm.tblPrendaNMaquina.OrderBy(pnm => pnm.fecha).Skip(1),
                            (prendaPrev, prendaActual) => prendaActual.fecha - prendaPrev.fecha)
                        .Where(tiempo => tiempo.TotalMinutes >= minsCooldownTiempoPerdido)
                        .Aggregate(TimeSpan.Zero, (tiempoTotal, tiempoActual) => tiempoTotal + tiempoActual);

                    // Desde ultima prenda hasta ahora
                    tiempoPerdido += tiempoDesdeUltimaPrenda.TotalMinutes >= minsCooldownTiempoPerdido ? tiempoDesdeUltimaPrenda : TimeSpan.Zero;
                }
                else
                {
                    // Si no hay prendas, calcular tiempo perdido desde inicio hasta ahora
                    tiempoPerdido = offset.Subtract(cnm.fechaIni);
                }

                return new
                {
                    cnm.idMaquina,
                    cnm.prendasCliente,
                    cnm.prendasActuales,
                    tiempoPerdido = tiempoPerdido.TotalSeconds / 3600
                };
            }
            
            ), fechaLavanderia = offset };
            return Ok(result);
        }

        //CONTROL DE RENDIMIENTO
        [HttpGet("odata/Dashboard/SmartView/spSelectReportSmartHub")]
        public async Task<ActionResult> spSelectReportSmartHub([FromODataUri] int idLavanderia, [FromODataUri] DateTime fechaIni, [FromODataUri] DateTime fechaFin, [FromODataUri] string group, [FromODataUri] int? idMaquina, [FromODataUri] int? idCompañia, [FromODataUri] int? idEntidad, [FromODataUri] int? idGrupoPlantillaPrenda_generica, [FromODataUri] int? idFamilia, [FromODataUri] int? idTipoPrenda)
        {
            if (idLavanderia == -1)
            {
                return Ok(new List<dynamic> { });
            }

            #region Aplicar offset lavanderia a fecha actual

            var tblLavanderia = db.tblLavanderia
           .Where(x => x.idLavanderia.Equals(idLavanderia))
           .Select(x => new
           {
               x.idLavanderia,
               x.horarioVerano,
               x.idZonaHorariaNavigation
           }).FirstOrDefault();

            int gmt = (tblLavanderia.horarioVerano == true ? 1 : 0) + Convert.ToInt32(tblLavanderia.idZonaHorariaNavigation.GMT);


            DateTimeOffset fechaIniOffset = new (fechaIni, TimeSpan.FromHours(gmt));
            DateTimeOffset fechaFinOffset = new (fechaFin, TimeSpan.FromHours(gmt));
            fechaFinOffset = fechaFinOffset.AddDays(1).AddSeconds(-1);
            #endregion


            var connection = db.Database.GetDbConnection();
            var results = await connection.QueryAsync("EXEC [MyRealData].[EF_spSelectReportSmartHub] @idLavanderia, @fechaIni, @fechaFin, @group, @idMaquina, @idCompañia, @idEntidad, @idGrupoPlantillaPrenda_generica, @idFamilia, @idTipoPrenda",
                new
                {
                    idLavanderia = idLavanderia,
                    fechaIni = fechaIniOffset,
                    fechaFin = fechaFinOffset,
                    group = group,
                    idMaquina = idMaquina,
                    idCompañia = idCompañia,
                    idEntidad = idEntidad,
                    idGrupoPlantillaPrenda_generica = idGrupoPlantillaPrenda_generica,
                    idFamilia = idFamilia,
                    idTipoPrenda = idTipoPrenda
                });
            return Ok(results.ToList());
        }

        [HttpGet("odata/Dashboard/SmartView/spSelectReportSmartHub_Totales")]
        public async Task<ActionResult> spSelectReportSmartHub([FromODataUri] int idLavanderia, [FromODataUri] DateTime fechaIni, [FromODataUri] DateTime fechaFin)
        {

            #region Aplicar offset lavanderia a fecha actual

            var tblLavanderia = db.tblLavanderia
           .Where(x => x.idLavanderia.Equals(idLavanderia))
           .Select(x => new
           {
               x.idLavanderia,
               x.horarioVerano,
               x.idZonaHorariaNavigation
           }).FirstOrDefault();

            int gmt = (tblLavanderia.horarioVerano == true ? 1 : 0) + Convert.ToInt32(tblLavanderia.idZonaHorariaNavigation.GMT);


            DateTimeOffset fechaIniOffset = new(fechaIni, TimeSpan.FromHours(gmt));
            DateTimeOffset fechaFinOffset = new(fechaFin, TimeSpan.FromHours(gmt));
            fechaFinOffset = fechaFinOffset.AddDays(1).AddSeconds(-1);
            #endregion

            var connection = db.Database.GetDbConnection();
            var results = await connection.QueryAsync("EXEC [MyRealData].[EF_spSelectReportSmartHub_Totales] @idLavanderia, @fechaIni, @fechaFin",
                new
                {
                    idLavanderia = idLavanderia,
                    fechaIni = fechaIniOffset,
                    fechaFin = fechaFinOffset
                });
            return Ok(results.ToList());
        }

        [HttpGet("odata/Dashboard/SmartView/spSelectReportSmartHub_Analisis")]
        public async Task<ActionResult> spSelectReportSmartHub_analisis([FromODataUri] string idsFamilia, [FromODataUri] int idLavanderia, [FromODataUri] DateTime fechaIni, [FromODataUri] DateTime fechaFin)
        {

            #region Aplicar offset lavanderia a fecha actual

            var tblLavanderia = db.tblLavanderia
           .Where(x => x.idLavanderia.Equals(idLavanderia))
           .Select(x => new
           {
               x.idLavanderia,
               x.horarioVerano,
               x.idZonaHorariaNavigation
           }).FirstOrDefault();

            int gmt = (tblLavanderia.horarioVerano == true ? 1 : 0) + Convert.ToInt32(tblLavanderia.idZonaHorariaNavigation.GMT);


            DateTimeOffset fechaIniOffset = new(fechaIni, TimeSpan.FromHours(gmt));
            DateTimeOffset fechaFinOffset = new(fechaFin, TimeSpan.FromHours(gmt));
            fechaFinOffset = fechaFinOffset.AddDays(1).AddSeconds(-1);
            #endregion

            var connection = db.Database.GetDbConnection();
            var results = await connection.QueryAsync("EXEC [MyRealData].[EF_spSelectReportSmartHub_Analisis] @idsFamilia, @idLavanderia, @fechaIni, @fechaFin",
                new
                {
                    idsFamilia = idsFamilia,
                    idLavanderia = idLavanderia,
                    fechaIni = fechaIniOffset,
                    fechaFin = fechaFinOffset
                });
            return Ok(results.ToList());
        }

        //CONTROL DE RENDIMIENTO
        [HttpGet("odata/Dashboard/SmartView/spSelectPersonaNAreaSmartHub")]
        public async Task<ActionResult> spSelectPersonaNAreaSmartHub([FromODataUri] int idLavanderia, [FromODataUri] DateTime fechaIni, [FromODataUri] DateTime fechaFin, [FromODataUri] string group, [FromODataUri] int? idPersona)
        {
            var connection = db.Database.GetDbConnection();
            var results = await connection.QueryAsync("EXEC [MyRealData].[EF_spSelectPersonaNAreaSmartHub] @idLavanderia, @fechaIni, @fechaFin, @group, @idPersona",
                new
                {
                    idLavanderia = idLavanderia,
                    fechaIni = fechaIni,
                    fechaFin = fechaFin,
                    group = group,
                    idPersona = idPersona
                });
            return Ok(results.ToList());
        }
    }
}
