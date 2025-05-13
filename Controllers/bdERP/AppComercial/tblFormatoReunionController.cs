using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblFormatoReunionController : ODataController
{
    private readonly bdERP db;

    public tblFormatoReunionController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<IQueryable<tblFormatoReunion>> Get()
    {
        return db.tblFormatoReunion;
    }

}
