using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;

namespace WebApiCore.Controllers;

[Microsoft.AspNetCore.Authorization.AllowAnonymous]
public class tblAreaLavanderiaController : ODataController
{
    private readonly bdERP db;

    public tblAreaLavanderiaController(bdERP context)
    {
        db = context;
    }


    [EnableQuery]
    [HttpGet]
    public async Task<ActionResult> Get([FromODataUri] int? idLavanderia) //SMART AREA
    {
        return Ok(db.tblAreaLavanderia.Where(x => idLavanderia == null || x.tblAreaLavanderiaNLavanderia.Count(l => l.idLavanderia.Equals(idLavanderia)) > 0));
    }
}