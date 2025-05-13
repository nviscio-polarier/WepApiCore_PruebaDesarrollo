using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblCentroTrabajoController : ODataController
{
    private readonly bdERP db;

    public tblCentroTrabajoController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] bool todas = false)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        return Ok(db.tblCentroTrabajo.Where(x => (x.idUsuario.Select(y => y.idUsuario).Contains(idUsuario)) || todas));
    }

}
