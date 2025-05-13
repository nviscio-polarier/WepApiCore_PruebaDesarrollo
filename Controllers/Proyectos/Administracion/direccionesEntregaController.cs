using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.ComponentModel.DataAnnotations;
using WebApiCore.Context;
using WebApiCore.Security;
using static WebApiCore.Controllers.Proyectos.Administracion.direccionesEntregaController;

namespace WebApiCore.Controllers.Proyectos.Administracion;

public class direccionesEntregaController : ODataController
{
    private readonly bdERP db;
    public direccionesEntregaController(bdERP context)
    {
        db = context;
    }

    public class DireccionEntrega
    {
        public DireccionEntrega(string direccionEntrega = "")
        {
            this.direccionEntrega = direccionEntrega;
        }

        [Key]
        public string direccionEntrega { get; set; }
    }

    [EnableQuery]
    [HttpGet]
    [Route("odata/Administracion/GetDireccionesEntrega")]
    [Authorize]
    public async Task<ActionResult> GetDireccionesEntrega([FromODataUri] int idEmpresaPolarier)
    {
        var direccionesLavanderias = db.tblLavanderia
            .Where(x => x.idEmpresaPolarier == idEmpresaPolarier && !string.IsNullOrWhiteSpace(x.direccion))
            .Select(x => new DireccionEntrega(x.direccion)).ToList();
        var direccionesCentros = db.tblCentroTrabajo
            .Where(x => !string.IsNullOrWhiteSpace(x.direccion))
            .Select(x => new DireccionEntrega(x.direccion)).ToList();
        var direccionesEntrega = direccionesCentros.Concat(direccionesLavanderias);
        return Ok(direccionesEntrega);
    }
}
