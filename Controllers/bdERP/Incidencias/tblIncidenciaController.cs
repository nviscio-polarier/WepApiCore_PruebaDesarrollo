using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;
using System.Net.Mail;
using WebApiCore.Class;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblIncidenciaController : ODataController
{
    private readonly bdERP db;

    public tblIncidenciaController(bdERP context)
    {
        db = context;
    }

    [EnableQuery(MaxExpansionDepth = 0)]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get()
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        List<int> idsEntidad = Utils.selectEntidadesVisibles(db, idUsuario, null);

        return Ok(db.tblIncidencia.Where(x =>
            (x.idLavanderia != null && x.idLavanderiaNavigation.idEntidad.Where(e => idsEntidad.Contains(e.idEntidad)).Count() > 0) ||
            (x.idEntidad != null && idsEntidad.Contains((int)x.idEntidad)) ||
            (x.idCompañia != null && x.idCompañiaNavigation.tblEntidad.Where(e => idsEntidad.Contains(e.idEntidad)).Count() > 0)
        ));
    }

    [EnableQuery]
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] tblIncidencia incidencia)
    {
        try
        {
            int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
            incidencia.fechaRegistro = DateTime.Now; //UTC

            var resultParameter = new SqlParameter
            {
                ParameterName = "@result",
                SqlDbType = SqlDbType.NVarChar,
                Direction = ParameterDirection.Output,
                Size = 8
            };

            db.Database.ExecuteSqlRaw(string.Format("SET @result = (SELECT Incidencias.EF_funCodigoIncidencia({0}));", incidencia.idLavanderia), resultParameter).ToString();
            incidencia.codigo = (string)resultParameter.Value;
            incidencia.denominacion = "";
            incidencia.estadoMaquinaInicial = incidencia.estadoMaquina;
            incidencia.idUsuarioCrea = idUsuario;

            db.tblIncidencia.Add(incidencia);
            await db.SaveChangesAsync();

            #region NOTIFICACIÓN
            tblTipoSubIncidencia tipoSub = db.tblTipoSubIncidencia.Find(incidencia.idSubTipoIncidencia);

            List<string> listCorreos = db.tblCorreosNLav
                .Where(x => x.idLavanderia == incidencia.idLavanderia &&
                        x.idTipoIncidencia.Where(y => y.idTipoIncidencia == tipoSub.idTipoIncidencia).Count() > 0)
                .Select(x => x.denominacion)
                .ToList();

            if (listCorreos.Count > 0)
            {
                string correos = String.Join(",", listCorreos);
                await Notificar(incidencia.idIncidencia, correos, true, idUsuario);
            }
            #endregion

            return Created(incidencia);
        }
        catch (Exception ex)
        {
            return BadRequest("Error de BDD");
        }
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblIncidencia> incidencia)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        tblUsuario objUsuario = db.tblUsuario.Where(x => x.idUsuario == idUsuario && !x.isEliminado).FirstOrDefault();
        if (objUsuario == null)
            return BadRequest();

        var entity = db.tblIncidencia.Where(x => x.idIncidencia.Equals(key)).FirstOrDefault();

        incidencia.ApplyTo(entity);

        await db.SaveChangesAsync();

        if (entity.estado) //CERRADO
        {
            var tipoSub = db.tblTipoSubIncidencia.FirstOrDefault(tsi => tsi.idSubTipoIncidencia == entity.idSubTipoIncidencia);

            var listCorreos = db.tblCorreosNLav
                .Where(cnl =>
                    tipoSub != null
                    && cnl.idLavanderia == entity.idLavanderia
                    && cnl.idTipoIncidencia.Any(ti => ti.idTipoIncidencia == tipoSub.idTipoIncidencia)
                )
                .Select(x => x.denominacion)
                .ToList();

            if (listCorreos.Count > 0)
            {
                string correos = String.Join(",", listCorreos);
                await Notificar(entity.idIncidencia, correos, false, idUsuario);
            }
        }

        List<tblIncidencia> result = new List<tblIncidencia>();
        result.Add(entity);
        return Ok(result);
    }

    [EnableQuery]
    [HttpGet]
    public async Task<ActionResult> Notificar([FromODataUri] int idIncidencia, [FromODataUri] string correos, [FromODataUri] bool isCreacion, [FromODataUri] int idUsuario)
    {
        tblIncidencia incidencia = await db.tblIncidencia.FindAsync(idIncidencia);
        tblTipoSubIncidencia subTipo = await db.tblTipoSubIncidencia.FindAsync(incidencia.idSubTipoIncidencia);
        tblTipoIncidencia tipoInc = await db.tblTipoIncidencia.FindAsync(subTipo.idTipoIncidencia);
        tblLavanderia lav = await db.tblLavanderia.FindAsync(incidencia.idLavanderia);
        tblUsuario usuario = await db.tblUsuario.FindAsync(incidencia.idUsuarioCrea);

        string textoMail = "";
        if (isCreacion)
        {
            textoMail = "Se ha creado una nueva " + @" incidencia con los siguientes datos: <br/> <br/> ";
        }
        else if (!isCreacion)
        {
            tblUsuario user = await db.tblUsuario.FindAsync(idUsuario);
            textoMail = "Incidencia cerrada por <b>" + user.nombre + @"</b> con los siguientes datos: <br/> <br/> ";
        }

        textoMail += "<b>Lavandería:</b> " + lav.denominacion + @" <br/>
                                <b>Código de incidencia:</b> " + incidencia.codigo + @" <br/>
                                <b>Fecha de la incidencia:</b> " + incidencia.fechaIncidencia.ToString("dd/MM/yyyy") + @" <br/>
                                <b>Creada por:</b> " + (usuario != null ? usuario.nombre : "NAN") + @" <br/>
                                <b>Tipo de incidencia:</b> " + tipoInc.denominacion + @" <br/>
                                <b>Sub-Tipo de incidencia:</b> " + subTipo.denominacion + @" <br/>";

        if (subTipo.idTipoIncidencia == 3) //SAT
        {
            tblMaquina maq = await db.tblMaquina.FindAsync(incidencia.idMaquina);
            textoMail += "<b>Máquina afectada:</b> " + maq.denominacion + @" <br/>";
        }

        textoMail += "<b>Descripción:</b> " + incidencia.descripcionIncidencia + @" <br/>";

        if (subTipo.idTipoIncidencia == 2) //TRANSPORTE
        {
            tblVehiculo veh = await db.tblVehiculo.FindAsync(incidencia.idVehiculo);
            textoMail += "<b>Vehículo afectada:</b> " + veh.matricula + " - " + veh.denominacion + @" <br/>";
        }

        if (subTipo.idTipoIncidencia == 3) //SAT
        {
            string resolucion = db.tblIncidenciaNParte.Where(x => x.idIncidencia == idIncidencia).OrderByDescending(y => y.fecha).Select(z => z.idParteNavigation.resolucion).FirstOrDefault();
            textoMail += "<b>Resolución:</b> " + resolucion + @" <br/>";

        }

        if (subTipo.idTipoIncidencia == 4) //RECURSOS HUMANOS
        {
            tblPersona pers = await db.tblPersona.FindAsync(incidencia.idUsuarioAfecta);
            textoMail += "<b>Persona afectada:</b> " + pers.nombre + " " + pers.apellidos + @" <br/>";
        }

        if (subTipo.idTipoIncidencia == 5) //CLIENTE
        {
            tblEntidad ent = await db.tblEntidad.FindAsync(incidencia.idEntidad);
            textoMail += "<b>Cliente afectado:</b> " + ent.denominacion + @" <br/>";
        }

        // Generamos el paquete SMTP con la configuracion del servidor y las credenciales de acceso
        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        SmtpClient smtpClient = new SmtpClient();
        smtpClient.Host = "outlook.office365.com";
        smtpClient.Port = 587;
        smtpClient.EnableSsl = true;
        smtpClient.Credentials = new System.Net.NetworkCredential("mypolarier@polarier.com", "Vog45080");

        // Generamos el mail a enviar
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress("mypolarier@polarier.com", "No Reply MyPolarier");

        if (Utils.isProduccion())
        {
            foreach (string correo in correos.Split(','))
            {
                mail.To.Add(correo);
            }
        }
        else
        {
            mail.To.Add("nalfonso@polarier.com");
        }

        mail.Subject = "Gestor de Incidencias - " + lav.denominacion + " - " + (isCreacion ? "Nueva incidencia!" : "Incidencia cerrada");
        mail.SubjectEncoding = System.Text.Encoding.UTF8;

        mail.Body = textoMail;
        mail.BodyEncoding = System.Text.Encoding.UTF8;
        mail.IsBodyHtml = true;
        mail.Priority = MailPriority.High;

        //Enviamos el mail
        try
        {
            smtpClient.Send(mail);
            return Ok();
        }
        catch
        {
            return BadRequest();
        }
        return BadRequest();
    }
}