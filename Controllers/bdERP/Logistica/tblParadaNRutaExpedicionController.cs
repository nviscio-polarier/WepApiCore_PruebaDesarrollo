using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblParadaNRutaExpedicionController : ODataController
{
    private readonly bdERP db;

    public tblParadaNRutaExpedicionController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get()
    {
        return Ok(db.tblParadaNRutaExpedicion);
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblParadaNRutaExpedicion paradaRuta)
    {
        try
        {
            paradaRuta.activo = true;

            #region Compatibilidad versión anterior (ELIMINAR)
            if (paradaRuta.idEntidad != null)
            {
                var entidadNRuta = new tblEntidadNRutaExpedicion
                {
                    idEntidad = (int)paradaRuta.idEntidad,
                    idRutaExpedicion = paradaRuta.idRutaExpedicion,
                    orden = (byte)(paradaRuta.orden - 1) //Quitando lavandería
                };
                db.tblEntidadNRutaExpedicion.Add(entidadNRuta);
            }
            #endregion

            #region Modificar orden última lavandería
            var entity = db.tblParadaNRutaExpedicion.Where(x => x.idRutaExpedicion.Equals(paradaRuta.idRutaExpedicion)).FirstOrDefault();
            if (entity != null)
            {
                int idRutaExpedicion = entity.idRutaExpedicion;
                var tblParadaNRutaExpedicion = db.tblParadaNRutaExpedicion.Where(x => x.idRutaExpedicion == idRutaExpedicion && x.idLavanderia != null && x.orden != 1 && x.isCarga == false).FirstOrDefault();
                if (tblParadaNRutaExpedicion != null)
                {
                    tblParadaNRutaExpedicion.orden = (byte)(tblParadaNRutaExpedicion.orden + 1);
                }
            }
            #endregion

            db.tblParadaNRutaExpedicion.Add(paradaRuta);

            await db.SaveChangesAsync();
            return Ok(Created(paradaRuta).Entity.idParadaNRutaExpedicion);
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblParadaNRutaExpedicion> paradaRuta)
    {
        var entity = db.tblParadaNRutaExpedicion.FirstOrDefault(x => x.idParadaNRutaExpedicion.Equals(key));
        if (entity == null)
            return BadRequest();

        paradaRuta.ApplyTo(entity);


        if (entity.idEntidad != null)
        {
            int idEntidad = (int)entity.idEntidad;
            int idRutaExpedicion = entity.idRutaExpedicion;
            short orden = entity.orden;

            var tblEntidadNRutaExpedicion = db.tblEntidadNRutaExpedicion.Where(x => x.idEntidad == idEntidad && x.idRutaExpedicion == idRutaExpedicion).FirstOrDefault();
            if (tblEntidadNRutaExpedicion != null)
            {
                tblEntidadNRutaExpedicion.orden = (byte)(orden - 1);
            }
        }

        await db.SaveChangesAsync();

        return Ok(entity);
    }

    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<bool> Delete([FromODataUri] int key)
    {
        var entity = await db.tblParadaNRutaExpedicion.FindAsync(key);
        if (entity == null)
            return false;

        #region Compatibilidad versión anterior (ELIMINAR)
        if (entity.idEntidad != null)
        {
            int idEntidad = (int)entity.idEntidad;
            int idRutaExpedicion = entity.idRutaExpedicion;

            var tblEntidadNRutaExpedicion = db.tblEntidadNRutaExpedicion.Where(x => x.idEntidad == idEntidad && x.idRutaExpedicion == idRutaExpedicion).FirstOrDefault();
            if (tblEntidadNRutaExpedicion != null)
                db.Remove(tblEntidadNRutaExpedicion);
        }
        #endregion

        #region Modificar orden última lavandería
        if (entity.idRutaExpedicion != null)
        {
            int idRutaExpedicion = entity.idRutaExpedicion;
            var tblParadaNRutaExpedicion = db.tblParadaNRutaExpedicion.Where(x => x.idRutaExpedicion == idRutaExpedicion && x.idLavanderia != null && x.orden != 1 && x.isCarga == false).FirstOrDefault();
            if (tblParadaNRutaExpedicion != null)
            {
                tblParadaNRutaExpedicion.orden = (byte)(tblParadaNRutaExpedicion.orden - 1);
            }
        }
        #endregion

        db.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }
}
