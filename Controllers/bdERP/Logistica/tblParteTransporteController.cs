using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblParteTransporteController : ODataController
{
    private readonly bdERP db;

    public tblParteTransporteController(bdERP context)
    {
        db = context;
    }

    [EnableQuery(MaxExpansionDepth = 0)]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get(int? idParteTransporte)
    {
        if (idParteTransporte != null)
        {
            return Ok(db.tblParteTransporte.Where(x => x.idParteTransporte.Equals(idParteTransporte)));
        }
        else
        {
            return Ok(db.tblParteTransporte);
        }
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblParteTransporte parteTransporte)
    {
        try
        {
            int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
            parteTransporte.idUsuarioResponsable = idUsuario;
            parteTransporte.idEstado = 1;

            var ruta = db.tblRutaExpedicion.Where(x => x.idRutaExpedicion.Equals(parteTransporte.idRutaExpedicion))
                  .Select(x => new
                  {
                      denoRutaExpedicion = x.denominacion,
                      paradas = x.tblParadaNRutaExpedicion
                  }).FirstOrDefault();

            //Ruta de expedición
            parteTransporte.denoRutaExpedicion = ruta.denoRutaExpedicion;
            parteTransporte.tblParadaNParteTransporte = ruta.paradas.Select(x => new tblParadaNParteTransporte()
            {
                idLavanderia = x.idLavanderia,
                idEntidad = x.idEntidad,
                orden = x.orden,
                observaciones = x.observaciones,
                isCarga = x.isCarga
            }).ToList();

            //Transportistas
            ICollection<tblPersona> transportistas = parteTransporte.idPersonaTransportista;
            parteTransporte.idPersonaTransportista = new List<tblPersona>();

            await db.SaveChangesAsync();

            foreach (tblPersona transportista in transportistas)
            {
                parteTransporte.idPersonaTransportista.Add(db.tblPersona.Where(e => e.idPersona == transportista.idPersona).FirstOrDefault());
            }

            db.tblParteTransporte.Add(parteTransporte);

            await db.SaveChangesAsync();


            return Ok(parteTransporte.idParteTransporte);
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblParteTransporte> parte)
    {
        var entity = db.tblParteTransporte.FirstOrDefault(x => x.idParteTransporte.Equals(key));
        if (entity == null)
            return BadRequest();

        parte.ApplyTo(entity);

        await db.SaveChangesAsync();

        return Ok(entity);
    }

    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<bool> Delete([FromODataUri] int key)
    {
        var entity = await db.tblParteTransporte.FindAsync(key);
        if (entity == null)
            return false;

        db.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }
}
