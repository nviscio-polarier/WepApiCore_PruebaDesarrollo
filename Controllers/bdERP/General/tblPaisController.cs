using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblPaisController : ODataController
{
    private readonly bdERP db;

    public tblPaisController(bdERP context)
    {
        db = context;
    }

    [EnableQuery(MaxAnyAllExpressionDepth = 2)]
    [HttpGet]
    [Authorize]
    public async Task<IQueryable<tblPais>> Get()
    {
        return db.tblPais;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] int key)
    {
        return Ok(db.tblPais.Where(x => x.idPais == key));
    }
}
