using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Class.bdERP.MyRealData;
using WebApiCore.Context;

namespace WebApiCore.Controllers;

public class tblLogConexionesController : ODataController
{
    private readonly bdERP db;
    public tblLogConexionesController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult> PostMasivo([FromBody] tblLogConexionesAgrupado conexiones)
    {
        try
        {
            int? idEnergyHub = conexiones.tblLogConexiones.FirstOrDefault().idEnergyHub;
            tblEnergyHub tblEnergyHub = await db.tblEnergyHub.FindAsync(idEnergyHub);

            #region Aplicar offset lavanderia a fecha actual
            int idLavanderia = tblEnergyHub.idLavanderia;

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

            List<tblLogConexiones> logConexiones = new List<tblLogConexiones>();
            foreach (var log in conexiones.tblLogConexiones)
            {
                logConexiones.Add(new tblLogConexiones
                {
                    tipoConexion = log.tipoConexion,
                    codigoRespuesta = log.codigoRespuesta,
                    idEnergyHub = log.idEnergyHub
                });
            }

            foreach (tblLogConexiones data in logConexiones)
            {
                data.fecha = offset;
            }
            db.tblLogConexiones.AddRange(logConexiones);

            await db.SaveChangesAsync();

            return Ok(true);
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }
}