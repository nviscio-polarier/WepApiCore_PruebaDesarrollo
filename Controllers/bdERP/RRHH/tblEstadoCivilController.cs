using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblEstadoCivilController : ODataController
{
    private readonly bdERP db;

    public tblEstadoCivilController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<IQueryable<tblEstadoCivil>> Get()
    {
        return db.tblEstadoCivil;
    }
}
