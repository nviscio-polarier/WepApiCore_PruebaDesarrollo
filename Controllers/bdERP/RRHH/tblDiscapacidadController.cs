using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblDiscapacidadController : ODataController
{
    private readonly bdERP db;

    public tblDiscapacidadController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<IQueryable<tblDiscapacidad>> Get()
    {
        return db.tblDiscapacidad;
    }
}
