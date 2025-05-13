using iText.Kernel.Pdf;
using iText.Layout;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.SignalR;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Data;
using WebApiCore.Class;
using WebApiCore.Class.bdERP.RRHH.tblDocumento;
using WebApiCore.Class.bdERP.tblDocumento;
using WebApiCore.Class.notificaciones;
using WebApiCore.Context;
using WebApiCore.Hubs;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblDocumentoController : ODataController
{
    private readonly bdERP db;
    private readonly IHubContext<NotificacionesHub> _hubContext;
    public tblDocumentoController(bdERP context, IHubContext<NotificacionesHub> hubContext)
    {
        db = context;
        _hubContext = hubContext;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get(bool todos = false)
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

        IQueryable<tblDocumento> documentos = db.tblDocumento.Select(x => new tblDocumento
        {
            idDocumento = x.idDocumento,
            fecha = x.fecha,
            fechaModificacion = x.fechaModificacion,
            denominacion = x.denominacion,
            documento = null,
            extension = x.extension,
            idCarpetaDocumentos = x.idCarpetaDocumentos,
            idPersona = x.idPersona,
            nuevo = x.nuevo,
            requerido = x.requerido,
            firmado = x.firmado,
            notificacion = x.notificacion,
            idPersonaNavigation = x.idPersonaNavigation,
            subCarpeta = x.subCarpeta,
            isVisible = x.isVisible
        });

        if (todos && objUsuario.enableDatosRRHH) //MyPolarier
        {
            var idsPersona = db.tblPersona.Where(x =>
            ((x.idLavanderia != null && objUsuario.idLavanderia.Select(y => y.idLavanderia).Contains((int)x.idLavanderia)) || x.idCentroTrabajo != null)
            && (x.idCentroTrabajo == null || (x.idCentroTrabajo != null && objUsuario.idCentroTrabajo.Select(y => y.idCentroTrabajo).Contains((int)(x.idCentroTrabajo))))
            && ((x.activo == true && todos == false) || (todos == true))
            && x.eliminado == false).Select(z => z.idPersona);

            return Ok(documentos.Where(x => idsPersona.Contains((int)x.idPersona)));
        }
        else if (!todos) // AppPolarier
        {
            return Ok(documentos.Where(x => (x.idPersona == null || x.idPersona.Equals(objUsuario.idPersona)) && x.isVisible == true ));
        }

        return BadRequest();
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

        tblDocumento objDoc = await db.tblDocumento.FindAsync(key);

        if (objDoc == null)
            return BadRequest();

        if (objDoc.idPersona.Equals(objUsuario.idPersona) && objDoc.nuevo.Equals(true))
        {
            objDoc.nuevo = false;
            objDoc.notificacion = false;
            await db.SaveChangesAsync();

            var entityUsuario = db.tblUsuario.Where(x => x.idPersona.Equals(objDoc.idPersona) && !x.isEliminado).FirstOrDefault();
            if (entityUsuario != null)
            {
                NotificacionesHub notificaciones = new NotificacionesHub(_hubContext, db);
                notificaciones.SendToUser(entityUsuario.idUsuario.ToString(), "notificaciones_RRHH", "tblDocumento");
            }
        }
        return Ok(objDoc);
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblDocumento> documento)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        tblUsuario objUsuario = db.tblUsuario.Where(x => x.idUsuario == idUsuario && !x.isEliminado).FirstOrDefault();
        if (objUsuario == null)
            return BadRequest();

        var entity = db.tblDocumento.FirstOrDefault(x => x.idDocumento.Equals(key) &&
        ((x.idPersona == null || x.idPersona.Equals(objUsuario.idPersona)) || objUsuario.enableDatosRRHH));

        if (entity == null)
            return BadRequest();

        documento.ApplyTo(entity);
        if (entity.requerido == true && entity.documento != null)
        {
            entity.requerido = false;
        }

        entity.notificacion = entity.isVisible;

        await db.SaveChangesAsync();

        _hubContext.Clients.Group("RRHH").SendAsync("RRHH/notificaciones", "tblDocumento");

        tblUsuario entityUsuario = db.tblUsuario.FirstOrDefault(x => x.idPersona == entity.idPersona);
        if (entityUsuario != null)
        {
            NotificacionesHub notificaciones = new NotificacionesHub(_hubContext, db);
            notificaciones.SendToUser(entityUsuario.idUsuario.ToString(), "notificaciones_RRHH", "tblDocumento");

            if (entity.notificacion == true)
            {
                string type = entity.requerido == true ? "tblDocumento_requerido" : entity.firmado == true ? "tblDocumento_firmado" : "tblDocumento_nuevo";
                await PushNotification(type, entityUsuario.idUsuario, entityUsuario.notificationToken, entity.denominacion);
            }
        }

        return Ok(true);
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblDocumento documento)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        documento.nuevo = documento.requerido == null && documento.firmado == null;
        documento.notificacion = documento.isVisible;
        documento.fecha = DateTime.Now;
        documento.fechaModificacion = DateTime.Now;
        documento.isLeido = documento.firmado == null ? null : false;

        db.tblDocumento.Add(documento);
        await db.SaveChangesAsync();

        _hubContext.Clients.Group("RRHH").SendAsync("RRHH/notificaciones", "tblDocumento");

        tblUsuario entityUsuario = db.tblUsuario.FirstOrDefault(x => x.idPersona == documento.idPersona);
        if (entityUsuario != null)
        {
            NotificacionesHub notificaciones = new NotificacionesHub(_hubContext, db);
            notificaciones.SendToUser(entityUsuario.idUsuario.ToString(), "notificaciones_RRHH", "tblDocumento");

            if (documento.notificacion == true)
            {
                string type = documento.requerido == true ? "tblDocumento_requerido" : documento.firmado == false ? "tblDocumento_firmado" : "tblDocumento_nuevo";
                await PushNotification(type, entityUsuario.idUsuario, entityUsuario.notificationToken, documento.denominacion);
            }
        }

        return Created(documento);
    }

    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<bool> Delete([FromODataUri] int key)
    {
        var entity = db.tblDocumento.Where(x => x.idDocumento.Equals(key)).FirstOrDefault();
        if (entity == null)
            return false;

        bool hasRelation_peticionCambios = db.tblPersona_PeticionCambioDatos.Where(
                x => x.idFotoDocumentoIdentidad_A.Equals(key) ||
                x.idFotoDocumentoIdentidad_B.Equals(key) ||
                x.idFotoNAF.Equals(key) ||
                x.idFotoIBAN.Equals(key)
            ).Count() > 0;

        bool hasRelation_persona = db.tblPersona.Where(
            x => x.idFotoDocumentoIdentidad_A.Equals(key) ||
            x.idFotoDocumentoIdentidad_B.Equals(key) ||
            x.idFotoNAF.Equals(key) ||
            x.idFotoIBAN.Equals(key)
        ).Count() > 0;

        if (!hasRelation_peticionCambios && !hasRelation_persona)
        {
            db.RemoveRange(db.tblDocumentoNSolicitudAlta.Where(x => x.idDocumento == entity.idDocumento));
            db.Remove(entity);
            await db.SaveChangesAsync();

            _hubContext.Clients.Group("RRHH").SendAsync("RRHH/notificaciones", "tblDocumento");

            tblUsuario entityUsuario = db.tblUsuario.FirstOrDefault(x => x.idPersona == entity.idPersona);
            if (entityUsuario != null)
            {
                NotificacionesHub notificaciones = new NotificacionesHub(_hubContext, db);
                notificaciones.SendToUser(entityUsuario.idUsuario.ToString(), "notificaciones_RRHH", "tblDocumento");
            }

            return true;
        }
        else
        {
            return true;
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Sign([FromBody] SignDocumento objSign)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        tblUsuario objUsuario = db.tblUsuario.Where(x => x.idUsuario == idUsuario && !x.isEliminado).FirstOrDefault();
        if (objUsuario == null)
            return BadRequest();

        tblDocumento objDoc = db.tblDocumento.FirstOrDefault(x => x.idDocumento.Equals(objSign.idDocumento) && x.idPersona.Equals(objUsuario.idPersona));
        if (objDoc == null)
            return BadRequest();

        int xPos = 50;
        int yPos = 90;
        int width = 100;
        int height = 50;
        int padding = 10;

        byte[] doc = objDoc.documento;
        MemoryStream docStream = new MemoryStream(doc);
        byte[] sign = Convert.FromBase64String(objSign.signBase64);
        MemoryStream signStream = new MemoryStream(sign);

        if (objDoc.extension.Equals("application/pdf"))
        {
            MemoryStream streamOutput = new MemoryStream();
            PdfDocument pdfDocument = new PdfDocument(new PdfReader(docStream), new PdfWriter(streamOutput));

            Document document = new Document(pdfDocument);
            iText.IO.Image.ImageData imageData = iText.IO.Image.ImageDataFactory.Create(sign);

            iText.Layout.Element.Image image = new iText.Layout.Element.Image(imageData);
            image.ScaleAbsolute(width, height);
            image.SetFixedPosition(pdfDocument.GetNumberOfPages(), xPos + padding, yPos + padding);

            document.Add(image);
            document.Close();

            objDoc.documento = streamOutput.ToArray();
        }
        else
        {
            IImageFormat format_doc;
            MemoryStream streamOutput = new MemoryStream();

            using (Image<Rgba32> imageSign = Image.Load<Rgba32>(sign))
            using (Image<Rgba32> imageDoc = Image.Load<Rgba32>(doc, out format_doc))
            {
                imageSign.Mutate(x => x.Resize(width, height));

                imageDoc.Mutate(x => x
                .DrawImage(imageSign, new SixLabors.ImageSharp.Point(0, imageDoc.Height - imageSign.Height), 1f)
                );

                imageDoc.Save(streamOutput, format_doc);

                objDoc.documento = streamOutput.ToArray();
            }
        }

        objDoc.firmado = true;
        objDoc.isLeido = true;
        objDoc.fechaFirma = DateTime.Now;

        await db.SaveChangesAsync();

        _hubContext.Clients.Group("RRHH").SendAsync("RRHH/notificaciones", "tblDocumento");

        tblUsuario entityUsuario = db.tblUsuario.FirstOrDefault(x => x.idPersona == objDoc.idPersona);
        if (entityUsuario != null)
        {
            NotificacionesHub notificaciones = new NotificacionesHub(_hubContext, db);
            notificaciones.SendToUser(entityUsuario.idUsuario.ToString(), "notificaciones_RRHH", "tblDocumento");
        }

        return Ok(objDoc.documento);
    }


    private tblDocumento generateNewDocumento(int idPersona, tblDocumento documento)
    {
        return new tblDocumento
        {
            denominacion = documento.denominacion,
            documento = documento.documento,
            extension = documento.extension,
            fecha = DateTimeOffset.UtcNow,
            fechaModificacion = DateTimeOffset.UtcNow,
            idCarpetaDocumentos = documento.idCarpetaDocumentos,
            idPersona = idPersona,
            notificacion = true,
            firmado = documento.firmado,
            nuevo = true,
            isLeido = documento.isLeido,
            requerido = documento.requerido,
            isVisible = documento.isVisible
        };
    }

    [HttpPost]
    [RequestSizeLimit(1_000_000_000)]
    [Authorize]
    public async Task<ActionResult> PostMasivo([FromBody] PostMasivoDocumentos postMasivo)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        foreach (int idPersona in postMasivo.idsPersona)
        {
            db.tblDocumento.Add(generateNewDocumento(idPersona, postMasivo.documento));
        }

        await db.SaveChangesAsync();


        #region NOTIFICACIONES
        if (Utils.isProduccion())
        {
            var usuarios = db.tblUsuario
            .Where(x => postMasivo.idsPersona.Contains((int)x.idPersona))
            .Select(x => new
            {
                idUsuario = x.idUsuario,
                notificationToken = x.notificationToken
            }).ToList();

            List<string> idsUsuario = usuarios.Select(x => x.idUsuario.ToString()).ToList();

            NotificacionesHub notificaciones = new NotificacionesHub(_hubContext, db);
            notificaciones.SendToUsers(idsUsuario, "notificaciones_RRHH", "tblDocumento");

            string type = "tblDocumento_firmado";
            foreach (var user in usuarios)
            {
                if (user.notificationToken != null && user.notificationToken != "")
                {
                    PushNotification(type, user.idUsuario, user.notificationToken, postMasivo.documento.denominacion);
                }
            }
        }

        #endregion

        return Ok(true);
    }

    [HttpPost]
    [RequestSizeLimit(1_000_000_000)]
    [Authorize]
    public async Task<ActionResult> PostMasivoMultDoc([FromBody] PostMasivoMultDocBody postMasivo)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        foreach (int idPersona in postMasivo.idsPersona)
        {
            foreach (var documento in postMasivo.documentos)
            {
                db.tblDocumento.Add(generateNewDocumento(idPersona, documento));
            }
        }

        await db.SaveChangesAsync();


        #region NOTIFICACIONES
        if (Utils.isProduccion())
        {
            var usuarios = db.tblUsuario
                .Where(x => postMasivo.idsPersona.Contains((int)x.idPersona))
                .Select(x => new
                {
                    idUsuario = x.idUsuario,
                    notificationToken = x.notificationToken
                }).ToList();

            List<string> idsUsuario = usuarios.Select(x => x.idUsuario.ToString()).ToList();

            NotificacionesHub notificaciones = new NotificacionesHub(_hubContext, db);
            notificaciones.SendToUsers(idsUsuario, "notificaciones_RRHH", "tblDocumento");

            string type = "tblDocumento_firmado";
            foreach (var user in usuarios)
            {
                if (user.notificationToken != null && user.notificationToken != "")
                {
                    foreach (var documento in postMasivo.documentos)
                    {
                        await PushNotification(type, user.idUsuario, user.notificationToken, documento.denominacion);
                    }
                }
            }
        }

        #endregion

        return Ok(true);
    }

    [HttpPost]
    [RequestSizeLimit(1_000_000_000)]
    [Authorize]
    public async Task<ActionResult> fn_importacionMasivaDocumentos([FromBody] tblDocumentosAgrupados tblDocumento)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        string idUsuario = this.HttpContext.Items["idUsuario"].ToString();
        NotificacionesHub notificaciones = new NotificacionesHub(_hubContext, db);
        notificaciones.SendToUser(idUsuario, "initProgressBar", "");

        int totalDocs = tblDocumento.documentos.Count();
        int numDocumento = 1;

        List<int> idsPersona = tblDocumento.documentos.Select(x => (int)x.idPersona).ToList();
        List<tblPersona> personas = db.tblPersona.Where(x => idsPersona.Contains(x.idPersona)).ToList();

        foreach (tblDocumentosAgrupados.tblDocumento_ doc in tblDocumento.documentos)
        {
            db.tblDocumento.Add(
               new tblDocumento
               {
                   denominacion = doc.denominacion,
                   documento = doc.documento,
                   extension = doc.extension,
                   fecha = doc.fecha,
                   fechaModificacion = doc.fechaModificacion,
                   idCarpetaDocumentos = doc.idCarpetaDocumentos,
                   idPersona = doc.idPersona,
                   notificacion = doc.notificacion,
                   firmado = doc.firmado,
                   nuevo = doc.nuevo,
                   requerido = doc.requerido
               });

            tblPersona persona = personas.Where(x => x.idPersona == doc.idPersona).FirstOrDefault();
            if (persona != null)
            {
                persona.isCodigoGestoriaValidado = true;
            }

            var progress = (numDocumento * 1.0) / totalDocs;

            await db.SaveChangesAsync();

            if (doc.notificationToken != null && doc.notificationToken != "")
            {
                await PushNotification("tblDocumento_firmado", (int)doc.idUsuario, doc.notificationToken.ToString(), doc.denominacion);
            }

            notificaciones.SendToUser(idUsuario, "updateProgressBar", (progress * 100).ToString());
            numDocumento++;
        }

        #region NOTIFICACIONES
        _hubContext.Clients.Group("RRHH").SendAsync("RRHH/notificaciones", "tblDocumento");

        List<string?> idsUsuario = tblDocumento.documentos.Select(x => x.idUsuario != null ? x.idUsuario.ToString() : null).ToList();
        notificaciones.SendToUsers(idsUsuario, "notificaciones_RRHH", "tblDocumento");
        #endregion

        await _hubContext.Clients.All.SendAsync("updateProgressBar", 100);
        return Ok(true);
    }

    public async Task<bool> PushNotification(string type, int idUsuario, string notificationToken, string denoDocumento)
    {
        string title = "";
        string body = denoDocumento ?? "Haz click para acceder";

        if (type == "tblDocumento_requerido" || type == "tblDocumento_firmado")
        {
            title = db.tblTipoNotificacion.Where(x => x.clave == type).Select(x => x.idTraduccionDescripcionNavigation.es).FirstOrDefault();
        }
        else if (type == "tblDocumento_nuevo")
        {
            title = "Documento nuevo";
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
