using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Data;
using WebApiCore.Context;

namespace WebApiCore.Controllers;

public class AppUpdaterController : ODataController
{
    private readonly bdGestionAplicaciones db;

    public AppUpdaterController(bdGestionAplicaciones context)
    {
        db = context;
    }

    [HttpPost("odata/AppUpdater/CheckVersion")]
    //[Authorize]
    public async Task<ActionResult> CheckVersion([FromODataUri] int idAplicacionesNPantalla)
    {
        try
        {
            var aplicacionesNPantallasToUpdate = from ap in db.tblAplicacionesNPantallas
                                                 where ap.IdAplicacionesNPantalla == idAplicacionesNPantalla
                                                 select ap;

            if (aplicacionesNPantallasToUpdate.Count() > 0)
            {
                aplicacionesNPantallasToUpdate.First().FechaUltimaConexion = DateTime.Now;
                db.SaveChanges();
            }

            var aplicacionesNPantallas = (from ap in aplicacionesNPantallasToUpdate
                                          join a in db.tblAplicaciones on ap.IdAplicacion equals a.IdAplicacion
                                          select new
                                          {
                                              ap.IdAplicacionesNPantalla,
                                              ap.Version,
                                              ap.Actualizar,
                                              a.UltimaVersion,
                                              a.Url,
                                              a.Password,
                                              a.FechaImplementacion,
                                              a.TablaConfig
                                          }).FirstOrDefault();

            var nombreTablaConfig = aplicacionesNPantallas.TablaConfig;

            dynamic config = "";
            if (nombreTablaConfig == "tblConfigPTMYPR")
            {
                config = db.tblConfigPTMYPR
                                        .Where(c => c.IdAplicacionesNPantalla == idAplicacionesNPantalla)
                                        .Select(c => new
                                        {
                                            c.IdConfig,
                                            c.IdLavanderia,
                                            c.MaquinaConfig,
                                            c.Idioma,
                                            c.IdiomaNumerico,
                                            c.PassConfig,
                                            c.DiasAlmacenamiento,
                                            c.MinutosSincronizar,
                                            c.MantenerPrendaInsert,
                                            c.MantenerConfigSesion,
                                            c.CambioDia,
                                            c.PassCambioDia,
                                            c.DiaCambiado,
                                            c.DiasMin,
                                            c.DiasMax
                                        })
                                        .FirstOrDefault();
            }
            else if (nombreTablaConfig == "tblConfigPTMYOF")
            {
                config = db.tblConfigPTMYOF
                                        .Where(c => c.IdAplicacionesNPantalla == idAplicacionesNPantalla)
                                         .Select(c => new
                                         {
                                             c.idConfig,
                                             c.idCompañia,
                                             c.idEntidad,
                                             c.Idioma,
                                             c.TiempoActualizar,
                                             c.TiempoCierreAutomatico,
                                             c.TipoEntregaPendiente,
                                             c.ValidarPersona,
                                             c.Peligro,
                                             c.Alerta,
                                             c.Imprimir,
                                             c.NCopiasValidar,
                                             c.PrendasImpresion,
                                             c.TituloAlbaran,
                                             c.OrdenacionGrids,
                                             c.idLavanderia
                                         })
                                       .FirstOrDefault();
            }
            else if (nombreTablaConfig == "tblConfigMYUNIF")
            {
                config = db.tblConfigMYUNIF
                                        .Where(c => c.idAplicacionesNPantalla == idAplicacionesNPantalla)
                                         .Select(c => new
                                         {
                                             c.idConfig,
                                             c.bdCU,
                                             c.Idioma,
                                             c.PassConfig,
                                             c.PassAcceso,
                                             c.TiempoReenvio,
                                             c.DiasAlmacenamiento,
                                             c.HoraServidor,
                                             c.PrendasExtra,
                                             c.PrendasGenericas,
                                             c.PrendasAutomaticas,
                                             c.BarcodeLenght,
                                             c.idLavanderia
                                         })
                                        .FirstOrDefault();
            }
            else if (nombreTablaConfig == "tblConfigTabletMyOffice")
            {
                config = db.tblConfigTabletMyOffice
                                                .Where(c => c.IdAplicacionesNPantalla == idAplicacionesNPantalla)
                                                .Select(c => new
                                                {
                                                    c.idConfig,
                                                    c.IdCompañia,
                                                    c.IdEntidad,
                                                    c.TipoConexion,
                                                    c.TipoAplicacion,
                                                    c.CambioEntidad,
                                                    c.CambioAlmacenes,
                                                    c.TipoAlmacen,
                                                    c.CodigoRevisor,
                                                    c.RevisionesRepartosIncompletos,
                                                    c.RevisionesStockIgualReposicion,
                                                    c.RepartosEntregaIgualPendiente
                                                })
                                                .FirstOrDefault();
            }
            else if (nombreTablaConfig == "tblConfigTabletMyInventory")
            {
                config = db.tblConfigTabletMyInventory
                                                    .Where(c => c.idAplicacionesNPantalla == idAplicacionesNPantalla)
                                                    .Select(c => new
                                                    {
                                                        c.idConfig,
                                                        c.idLavanderia,
                                                        c.TipoConexion,
                                                        c.aplicarStock
                                                    })
                                                   .FirstOrDefault();
            }
            else if (nombreTablaConfig == "tblConfigTabletMyAudit")
            {
                config = db.tblConfigTabletMyAudit.Where(c => c.idAplicacionesNPantalla == idAplicacionesNPantalla)
                                                    .Select(c => new
                                                    {
                                                        c.idConfig,
                                                        c.TipoConexion
                                                    })
                                                   .FirstOrDefault();
            }
            else if (nombreTablaConfig == "tblConfigTabletMyQuality")
            {
                config = db.tblConfigTabletMyQuality.Where(c => c.idAplicacionesNPantalla == idAplicacionesNPantalla)
                                                    .Select(c => new
                                                    {
                                                        c.idConfig,
                                                        c.idLavanderia,
                                                        c.TipoConexion
                                                    })
                                                   .FirstOrDefault();
            }

            var updaterData = db.tblAplicaciones
                                        .Where(a => a.Codigo == "UPDATER")
                                        .Select(a => new
                                        {
                                            a.UltimaVersion,
                                            a.Url
                                        })
                                        .FirstOrDefault();

            var result = new
            {
                aplicacionesNPantallas,
                config,
                updaterData
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }
}
