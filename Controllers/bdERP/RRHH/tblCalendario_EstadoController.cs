using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblCalendario_EstadoController : ODataController
{
    private readonly bdERP db;

    public tblCalendario_EstadoController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<IQueryable<tblCalendario_Estado>> Get()
    {
        return db.tblCalendario_Estado;
    }
}
