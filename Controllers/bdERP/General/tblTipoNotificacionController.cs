using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblTipoNotificacionController : ODataController
{
    private readonly bdERP db;

    public tblTipoNotificacionController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<IQueryable<tblTipoNotificacion>> Get()
    {
        return db.tblTipoNotificacion;
    }
}
