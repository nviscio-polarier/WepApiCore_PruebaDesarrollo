using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApiCore.Class;
using WebApiCore.Class.notificaciones;
using WebApiCore.Context;
using WebApiCore.Hubs;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblPersona_PeticionCambioDatosController : ODataController
{
    private readonly bdERP db;

    private readonly IHubContext<NotificacionesHub> _hubContext;
    public tblPersona_PeticionCambioDatosController(bdERP context, IHubContext<NotificacionesHub> hubContext)
    {
        db = context;
        _hubContext = hubContext;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] bool todos = false)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        var objUsuario = db.tblUsuario
         .Where(x => x.idUsuario.Equals(idUsuario) && !x.isEliminado)
         .Select(x => new
         {
             x.idUsuario,
             x.enableDatosRRHH,
             x.idCentroTrabajo,
             x.idPersona,
             x.idLavanderia
         }).FirstOrDefault();

        if (objUsuario == null)
            return BadRequest();

        if (todos)
        {
            if (!objUsuario.enableDatosRRHH) //No tienes permisos para acceder a los datos de todos los usuarios
            {
                return BadRequest();
            }

            var idsPersona = db.tblPersona.Where(x =>
                ((x.idLavanderia != null && objUsuario.idLavanderia.Select(y => y.idLavanderia).Contains((int)x.idLavanderia)) || x.idCentroTrabajo != null)
                && (x.idCentroTrabajo == null || (x.idCentroTrabajo != null && objUsuario.idCentroTrabajo.Select(y => y.idCentroTrabajo).Contains((int)(x.idCentroTrabajo))))
                && ((x.activo == true && todos == false) || (todos == true))
                && x.eliminado == false).Select(z => z.idPersona);

            return Ok(db.tblPersona_PeticionCambioDatos.Where(x => idsPersona.Contains(x.idPersona)));

        }
        else
        {
            return Ok(db.tblPersona_PeticionCambioDatos.Where(x => x.idPersona == objUsuario.idPersona));
        }
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] int key)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        tblUsuario objUsuario = db.tblUsuario.Where(x => x.idUsuario == idUsuario && !x.isEliminado).FirstOrDefault();
        if (objUsuario == null)
            return BadRequest();

        var objPeticion = db.tblPersona_PeticionCambioDatos.Where(x => x.idPeticionCambioDatos.Equals(key));

        if (objPeticion == null)
            return BadRequest();

        return Ok(objPeticion);
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblPersona_PeticionCambioDatos> peticionCambioDatos)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        tblUsuario objUsuario = db.tblUsuario.Where(x => x.idUsuario == idUsuario && !x.isEliminado).FirstOrDefault();
        if (objUsuario == null)
            return BadRequest();

        var entity = db.tblPersona_PeticionCambioDatos.Include(x => x.idLicenciaConducir).Where(x => x.idPeticionCambioDatos.Equals(key)).FirstOrDefault();

        if (entity == null)
        {
            entity = db.tblPersona_PeticionCambioDatos.Add(new tblPersona_PeticionCambioDatos()
            {
                idPersona = (int)objUsuario.idPersona,
                idPeticionCambioDatos_estado = 1,
                fechaPeticion = DateTime.Now,
                notificacion = false,
            }).Entity;
        }

        bool cambioEstadoMyPolarier = peticionCambioDatos.Operations.FirstOrDefault(x => x.path.Contains("idPeticionCambioDatos_estado")) != null;

        var operation_idLicenciaConducir = peticionCambioDatos.Operations.FirstOrDefault(x => x.path.Equals("/idLicenciaConducir"));
        if (operation_idLicenciaConducir != null)
        {
            if (entity != null)
            {
                entity.idLicenciaConducir.Clear();
            }

            List<byte> idsLicenciaConducir = JsonConvert.DeserializeObject<List<tblLicenciaConducir>>(operation_idLicenciaConducir.value.ToString()).Select(x => x.idLicenciaConducir).ToList();

            entity.idLicenciaConducir = db.tblLicenciaConducir.Where(x => idsLicenciaConducir.Contains(x.idLicenciaConducir)).ToList();

            peticionCambioDatos.Operations.Remove(operation_idLicenciaConducir);

        }

        //Al acepatar o denegar desde MyPolarier
        if (cambioEstadoMyPolarier)
        {
            entity.fechaRespuesta = DateTime.Now;
            entity.notificacion = true;
        }

        //Al modificar una petición por el mismo usuario afectado
        if (objUsuario.idPersona.Equals(entity.idPersona))
        {
            entity.fechaPeticion = DateTime.Now;
        }

        peticionCambioDatos.ApplyTo(entity);
        await db.SaveChangesAsync();

        _hubContext.Clients.Group("RRHH").SendAsync("RRHH/notificaciones", "tblPersona_PeticionCambioDatos");

        var entityUsuario = db.tblUsuario.Where(x => x.idPersona.Equals(entity.idPersona) && !x.isEliminado).FirstOrDefault();
        if (entityUsuario != null)
        {
            NotificacionesHub notificaciones = new NotificacionesHub(_hubContext, db);
            notificaciones.SendToUser(entityUsuario.idUsuario.ToString(), "notificaciones_RRHH", "tblPersona_PeticionCambioDatos");

            if (cambioEstadoMyPolarier)
            {
                string type = entity.idPeticionCambioDatos_estado == 2 ? "tblPersona_PeticionCambioDatos_validado" : "tblPersona_PeticionCambioDatos_denegado";
                await PushNotification(type, entityUsuario.idUsuario, entityUsuario.notificationToken);
            }
        }

        List<tblPersona_PeticionCambioDatos> result = new List<tblPersona_PeticionCambioDatos>();
        result.Add(entity);
        return Ok(result);
    }

    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<ActionResult> Delete([FromODataUri] int key)
    {
        var entity = db.tblPersona_PeticionCambioDatos.Where(x => x.idPeticionCambioDatos.Equals(key) && x.idPeticionCambioDatos_estado.Equals(1)).FirstOrDefault();
        if (entity == null)
            return BadRequest();

        var entityUsuario = db.tblUsuario.Where(x => x.idPersona.Equals(entity.idPersona) && !x.isEliminado).FirstOrDefault();

        db.Remove(entity);
        await db.SaveChangesAsync();

        _hubContext.Clients.Group("RRHH").SendAsync("RRHH/notificaciones", "tblPersona_PeticionCambioDatos");

        if (entityUsuario != null) //Aviso SignalR
        {
            NotificacionesHub notificaciones = new NotificacionesHub(_hubContext, db);
            notificaciones.SendToUser(entityUsuario.idUsuario.ToString(), "notificaciones_RRHH", "tblPersona_PeticionCambioDatos");
        }

        return Ok();
    }


    public async Task<bool> PushNotification(string type, int idUsuario, string notificationToken)
    {
        string title = "";
        string body = "Haz click para acceder";

        if (type == "tblPersona_PeticionCambioDatos_validado" || type == "tblPersona_PeticionCambioDatos_denegado")
        {
            title = db.tblTipoNotificacion.Where(x => x.clave == type).Select(x => x.idTraduccionDenominacionNavigation.es).FirstOrDefault();
        }

        var request = new NotificacionRequest()
        {
            idUsuario = idUsuario,
            notificacionToken = notificationToken,
            title = title,
            body = body
        };

        return await new Utils.Notificaciones(db).SendMessageAsync(request);
    }
}
