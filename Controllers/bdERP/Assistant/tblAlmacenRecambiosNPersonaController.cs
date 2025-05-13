using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblAlmacenRecambiosNPersonaController : ODataController
{
    private readonly bdERP db;

    public tblAlmacenRecambiosNPersonaController(bdERP context)
    {
        db = context;
    }

    [HttpGet]
    [EnableQuery]
    [Authorize]
    public ActionResult Get([FromODataUri] int? idPersona = null)
    {
        return Ok(db.tblAlmacenRecambiosNPersona.Where(arnp => idPersona == null || arnp.idPersona == idPersona));
    }
}
