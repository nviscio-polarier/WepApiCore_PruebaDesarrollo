using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using WebApiCore.Class;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblEnvioController : ODataController
{
    private readonly bdERP db;

    public tblEnvioController(bdERP context)
    {
        db = context;
    }

    [EnableQuery(MaxAnyAllExpressionDepth = 2, MaxNodeCount = 3000)]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get()
    {
        return Ok(db.tblEnvio);
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblEnvio envio, [FromODataUri] bool notificar)
    {
        //if (!ModelState.IsValid)
        //{
        //    return BadRequest(ModelState);
        //}
        List<tblTipoDocumento_Envio> docs = new List<tblTipoDocumento_Envio>();
        foreach (tblTipoDocumento_Envio doc in envio.idTipoDocumento_Envio)
        {
            docs.Add(db.tblTipoDocumento_Envio.Find(doc.idTipoDocumento_Envio));
        }
        envio.idTipoDocumento_Envio = docs;

        try
        {
            db.tblEnvio.Add(envio);
            await db.SaveChangesAsync();

            #region NOTIFICACIÓN
            if (envio.idCorreo.Count > 0 && notificar)
            {
                string correos = String.Join(",", envio.idCorreo.Select(x => x.denominacion));
                await Notificar(envio.idEnvio, correos, true);
            }
            #endregion
            return Created(envio);
        }
        catch (Exception er)
        {
            return BadRequest(er.Message);
        }
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromODataUri] bool notificar, [FromBody] JsonPatchDocument<tblEnvio> envio)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var entity = await db.tblEnvio.Include(x => x.idCorreo).Include(x => x.tblPackingList).FirstOrDefaultAsync(x => x.idEnvio == key);

        if (entity == null)
        {
            return NotFound();
        }

        envio.ApplyTo(entity);
        try
        {
            await db.SaveChangesAsync();

            if (entity.idCorreo.Count > 0 && notificar)
            {
                string correos = String.Join(",", entity.idCorreo.Select(x => x.denominacion));
                #region NOTIFICACIÓN
                await Notificar(entity.idEnvio, correos, false);
                #endregion
            }
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!tblEnvioExists(key))
            {
                return NotFound();
            }
            throw;
        }
        return Updated(entity);
    }

    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<ActionResult> Delete([FromODataUri] int key)
    {
        var entity = await db.tblEnvio
            .Include(x => x.idCorreo)
            .Include(x => x.tblPackingList)
            .Include(x => x.tblEnvio_Documento)
            .FirstOrDefaultAsync(x => x.idEnvio == key);

        if (entity == null)
        {
            return NotFound();
        }

        entity.idCorreo.Clear();
        entity.tblEnvio_Documento.Clear();
        entity.tblPackingList.Clear();

        try
        {
            await db.SaveChangesAsync();
            db.tblEnvio.Remove(entity);
            await db.SaveChangesAsync();

        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
        return NoContent();
    }

    private bool tblEnvioExists(int id)
    {
        return db.tblEnvio.Count(e => e.idEnvio == id) > 0;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Notificar([FromODataUri] int idEnvio, [FromODataUri] string correos, [FromODataUri] bool isCreacion)
    {
        if (Utils.isProduccion())
        {
            dynamic envio = db.tblEnvio.Where(x => x.idEnvio == idEnvio)
                .Select(x => new
                {
                    proyecto = x.idProyectoNavigation != null ? x.idProyectoNavigation.denominacion : "",
                    destinatario = x.idDestinatarioNavigation != null ? x.idDestinatarioNavigation.denominacion : "",
                    embarcador = x.idEmbarcadorNavigation != null ? x.idEmbarcadorNavigation.denominacion : "",
                    tipoContenedor = x.idTipoContenedorNavigation != null ? x.idTipoContenedorNavigation.denominacion : "",
                    incotermProveedor = x.idIncotermProvNavigation != null ? x.idIncotermProvNavigation.denominacion : "",
                    x.numCont,
                    puertoCarga = x.idPuertoCargaNavigation != null ? x.idPuertoCargaNavigation.denominacion : "",
                    puertoDestino = x.idPuertoDestinoNavigation != null ? x.idPuertoDestinoNavigation.denominacion : "",
                    incotermCliente = x.idIncotermClienteNavigation != null ? x.idIncotermClienteNavigation.denominacion : "",
                    x.blNumero,
                    fechaEstimacionCarga = x.fechaEstimacionCarga != null ? x.fechaEstimacionCarga.Value.ToString("dd/MM/yyyy") : "",
                    etd = x.etd != null ? x.etd.Value.ToString("dd/MM/yyyy") : "",
                    etaDestino = x.etaDestino != null ? x.etaDestino.Value.ToString("dd/MM/yyyy") : "",
                    despacho = x.despacho != null ? x.despacho.Value.ToString("dd/MM/yyyy") : "",
                    entrega = x.entrega != null ? x.entrega.Value.ToString("dd/MM/yyyy") : ""
                }).FirstOrDefault();
            string textoMail = (isCreacion ? "Se ha creado un nuevo" : "Se ha modificado un") + @" envío con los siguientes datos: <br/> <br/> 
                            <b>Proyecto:</b> " + envio.proyecto + @" <br/>
                            <b>Destinatario:</b> " + envio.destinatario + @" <br/>
                            <b>Embarcador:</b> " + envio.embarcador + @" <br/> <br/>

                            <b>Tipo contenedor:</b> " + envio.tipoContenedor + @" <br/>
                            <b>Incoterm proveedor:</b> " + envio.incotermProveedor + @" <br/>
                            <b>Número contenedor:</b> " + envio.numCont + @" <br/>
                            <b>Puerto carga:</b> " + envio.puertoCarga + @" <br/>
                            <b>Puerto destino:</b> " + envio.puertoDestino + @" <br/>
                            <b>Incoterm cliente:</b> " + envio.incotermCliente + @" <br/>
                            <b>BL número:</b> " + envio.blNumero + @" <br/> <br/>
                            <b>Fecha estimación carga:</b> " + envio.fechaEstimacionCarga + @" <br/>
                            <b>ETD:</b> " + envio.etd + @" <br/>
                            <b>ETA destino:</b> " + envio.etaDestino + @" <br/>
                            <b>Despacho:</b> " + envio.despacho + @" <br/>
                            <b>Entrega:</b> " + envio.entrega + @" <br/>";

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

            foreach (string correo in correos.Split(','))
            {
                mail.To.Add(correo);
            }

            mail.Subject = "Control de envíos - " + (isCreacion ? "Nuevo envío!" : "Envío modificado");
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
            catch { }
        }
        return BadRequest();
    }

}
