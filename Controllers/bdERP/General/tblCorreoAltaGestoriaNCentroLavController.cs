using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblCorreoAltaGestoriaNCentroLavController : ODataController
{
    private readonly bdERP db;

    public tblCorreoAltaGestoriaNCentroLavController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<IQueryable<tblCorreoAltaGestoriaNCentroLav>> Get()
    {
        return db.tblCorreoAltaGestoriaNCentroLav;
    }
}
