using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblLavanderiaController : ODataController
{
    private readonly bdERP db;

    public tblLavanderiaController(bdERP context)
    {
        db = context;
    }

    [EnableQuery(MaxAnyAllExpressionDepth = 5)]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] bool todas = false)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        tblUsuario objUsuario = db.tblUsuario.Where(x => x.idUsuario == idUsuario && !x.isEliminado).FirstOrDefault();
        if (objUsuario == null)
            return BadRequest();

        return Ok(db.tblLavanderia.Where(x => (!todas && x.idUsuario.Select(y => y.idUsuario).Contains(idUsuario)) || todas));
    }
}
