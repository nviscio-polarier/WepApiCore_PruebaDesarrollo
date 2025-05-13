using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using AuthorizeAttribute = WebApiCore.Security.AuthorizeAttribute;

namespace WebApiCore.Controllers.Proyectos.MyPolarier.Gestoria;
public class SolicitudAltaGestoriaController : ODataController
{
    private readonly bdERP db;
    public SolicitudAltaGestoriaController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet("odata/MyPolarier/Gestoria/SolicitudGestoria")]
    [Authorize]
    public async Task<ActionResult> SolicitudGestoria()
    {
        var result = (from sg in db.tblSolicitudAlta
                      join ll in db.tblLlamamiento on sg.idSolicitudAlta equals ll.idLlamamiento into llGroup
                      from ll in llGroup.DefaultIfEmpty()
                      join p in db.tblPersona on ll.idPersona equals p.idPersona into pGroup
                      from p in pGroup.DefaultIfEmpty()
                      join tc in db.tblTipoContrato on ll.idTipoContrato equals tc.idTipoContrato into tcGroup
                      from tc in tcGroup.DefaultIfEmpty()
                      join ct in db.tblCentroTrabajo on ll.idCentroTrabajo equals ct.idCentroTrabajo into ctGroup
                      from ct in ctGroup.DefaultIfEmpty()
                      join ci in db.tblCategoriaInterna on ll.idCategoriaInterna equals ci.idCategoriaInterna into ciGroup
                      from ci in ciGroup.DefaultIfEmpty()
                      join cc in db.tblCategoriaConvenio on ci.idCategoriaConvenio equals cc.idCategoriaConvenio into ccGroup
                      from cc in ccGroup.DefaultIfEmpty()
                      where p.eliminado == false
                      select new
                      {
                          centroTrabajo = ct.denominacion,
                          tipoContrato = tc.denominacion,
                          categoriaConvenio = cc.denominacion,
                          categoriaInterna = ci.denominacion,
                          p.apellidos,
                          p.nombre,
                          p.codigoGestoria,
                          sg.idEstadoSolicitudAlta,
                          ll.fechaIni,
                          sg.fecha_reg
                      }).Distinct().OrderBy(x => x.nombre).ToList();

        return Ok(result);
    }

    //[EnableQuery]
    //[HttpPost("odata/MyPolarier/Gestoria/insertSolicitud")]
    //[Authorize]
    //public async Task<ActionResult> insertSolicitud([FromBody] List<SolicitudGestoria> solicitud)
    //{

    //    foreach (var item in solicitud)
    //    {
    //        var tipoContrato = db.tblPersonaNTipoContrato.FirstOrDefault(x => x.idPersona == item.idLlamamiento && x.fechaBajaContrato == null);

    //        var insert = new tblSolicitudGestoria
    //        {
    //            fecha_alta = tipoContrato?.fechaAltaContrato != null ? tipoContrato.fechaAltaContrato : new DateTimeOffset(),
    //            fecha_reg = new DateTimeOffset(),
    //            idEstadoSolicitudGestoria = 1,
    //            idPersona = (int)item.idLlamamiento,
    //        };
    //    }


    //    return Ok();
    //}

    //[EnableQuery]
    //[HttpPost("odata/MyPolarier/Gestoria/updateSolicitud")]
    //[Authorize]
    //public async Task<ActionResult> updateSolicitud([FromBody] SolicitudGestoria solicitud)
    //{

    //    var idSolicitudGestoria = solicitud.idSolicitudGestoria;

    //    if (idSolicitudGestoria == null)
    //    {
    //        return BadRequest();
    //    }

    //    var tblSolicitudGestoria = db.tblSolicitudAltaGestoria;
    //    var tblPersona = db.tblPersona;


    //    if (solicitud.codigoGestoria != null)
    //    {
    //        tblPersona.FirstOrDefault(x => x.idPersona == solicitud.idLlamamiento).codigoGestoria = solicitud.codigoGestoria;
    //    }

    //    if (solicitud.idEstadoSolicitudGestoria != null)
    //    {
    //        tblSolicitudGestoria.FirstOrDefault(x => x.idSolicitudGestoria == solicitud.idSolicitudGestoria).idEstadoSolicitudAltaGestoria = (byte)solicitud.idSolicitudGestoria;
    //    }

    //    await db.SaveChangesAsync();
    //    return Ok();
    //}

    //Datos filtros pantalla solicitud gestoria

    [EnableQuery]
    [HttpGet("odata/MyPolarier/Gestoria/empresaPolarier_solicitudGestoria")]
    public async Task<ActionResult> empresaPolarier_solicitudGestoria()
    {
        var idsPersonaSolicitud = db.tblSolicitudAlta.Select(x => x.idSolicitudAltaNavigation.idPersona).ToList();
        var tblPersona_filterSolicitud = db.tblPersona.Where(x => idsPersonaSolicitud.Contains(x.idPersona));
        var idsEmpresaPolarier = tblPersona_filterSolicitud.Select(x => x.idEmpresaPolarier).ToList();
        var tblEmpresaPolarier_filterSolicitud = db.tblEmpresasPolarier.Where(x => idsEmpresaPolarier.Contains(x.idEmpresaPolarier)).Select(x => new { x.denominacion, x.idEmpresaPolarier });

        return Ok(tblEmpresaPolarier_filterSolicitud);
    }

    [EnableQuery]
    [HttpGet("odata/MyPolarier/Gestoria/categoriasConvenio_solicitudGestoria")]
    [Authorize]
    public async Task<ActionResult> categoriasConvenio_solicitudGestoria()
    {

        var idsLlamamientoSolicitud = db.tblSolicitudAlta.Select(x => x.idSolicitudAlta).ToList();
        var tblCategoriaInterna_filterSolicitud = db.tblCategoriaInterna.Where(x => x.tblLlamamiento.Any(x => idsLlamamientoSolicitud.Contains(x.idLlamamiento)));

        var idsCategoriaConvenio = tblCategoriaInterna_filterSolicitud.Select(x => x.idCategoriaConvenio).ToList();
        var tblCategoriaConvenio_filterSolicitud = db.tblCategoriaConvenio.Where(x => idsCategoriaConvenio.Contains(x.idCategoriaConvenio))
            .Select(x => new { x.denominacion, x.idCategoriaConvenio });

        return Ok(tblCategoriaConvenio_filterSolicitud);
    }

    [EnableQuery]
    [HttpGet("odata/MyPolarier/Gestoria/categoriaInterna_solicitudGestoria")]
    [Authorize]
    public async Task<ActionResult> categoriaInterna_solicitudGestoria()
    {
        var idsLlamamientoSolicitud = db.tblSolicitudAlta.Select(x => x.idSolicitudAlta).ToList();
        var tblCategoriaInterna_filterSolicitud = db.tblCategoriaInterna.Where(x => 
                x.tblLlamamiento.Any(x => idsLlamamientoSolicitud.Contains(x.idLlamamiento))
            ).Select(x => new { x.denominacion, x.idCategoriaInterna });

        return Ok(tblCategoriaInterna_filterSolicitud);
    }

    [EnableQuery]
    [HttpGet("odata/MyPolarier/Gestoria/tipoContrato_solicitudGestoria")]
    [Authorize]
    public async Task<ActionResult> tipoContrato_solicitudGestoria()
    {
        var idsLlamamientoSolicitud = db.tblSolicitudAlta.Select(x => x.idSolicitudAlta).ToList();
        var tblTipoContrato_filterSolicitud = db.tblTipoContrato.Where(x => 
            x.tblLlamamiento.Any(x => idsLlamamientoSolicitud.Contains(x.idLlamamiento))
        ).Select(x => new { x.denominacion, x.idTipoContrato });

        return Ok(tblTipoContrato_filterSolicitud);
    }
}

public class SolicitudGestoria
{
    public int? idSolicitudGestoria { get; set; }
    public int? idLlamamiento { get; set; }
    public string? codigoGestoria { get; set; }
    public DateTimeOffset? fecha_alta { get; set; }
    public DateTimeOffset? fecha_reg { get; set; }
    public int idEstadoSolicitudGestoria { get; set; }
    public bool? isUrgente { get; set; }
    public bool? isNuevoAlta { get; set; }
    public bool? isLlamamiento { get; set; }
    public List<tblDocumentoNSolicitudAlta> tblDocumentoNSolicitudAlta { get; set; }

}

