using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblTipoReunionController : ODataController
{
    private readonly bdERP db;

    public tblTipoReunionController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<IQueryable<tblTipoReunion>> Get()
    {
        return db.tblTipoReunion;
    }

}
