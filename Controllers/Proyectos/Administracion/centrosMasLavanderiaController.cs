using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.ComponentModel.DataAnnotations;
using WebApiCore.Context;
using WebApiCore.Security;
namespace WebApiCore.Controllers.Proyectos.Administracion;

public class centrosMasLavanderiaController : ODataController
{
    private readonly bdERP db;
    public centrosMasLavanderiaController(bdERP context)
    {
        db = context;
    }

    public class CentroMasLavanderia
    {
        public CentroMasLavanderia(string idCentro, string denominacion, string direccion = "")
        {
            this.idCentro = idCentro;
            this.denominacion = denominacion;
            this.direccion = direccion;
        }

        [Key]
        public string idCentro { get; set; }
        public string denominacion { get; set; }
        public string direccion { get; set; }

    }

    [EnableQuery]
    [HttpGet]
    [Route("odata/Administracion/GetCentroMasLavanderia")]
    [Authorize]
    public async Task<ActionResult> GetCentroMasLavanderia()
    {
        var centros = db.tblCentroTrabajo.Select(x => new CentroMasLavanderia("0_" + x.idCentroTrabajo, x.denominacion, x.direccion)).ToList();
        var lavanderias = db.tblLavanderia.Select(x => new CentroMasLavanderia("1_"+ x.idLavanderia, x.denominacion, x.direccion)).ToList();
        var centrosMasLavanderia = centros.Concat(lavanderias);
        return Ok(centrosMasLavanderia);
    }
}
