using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblTipoConsumoLenceriaController : ODataController
{
    private readonly bdERP db;

    public tblTipoConsumoLenceriaController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<IQueryable<tblTipoConsumoLenceria>> Get()
    {
        return db.tblTipoConsumoLenceria;
    }
}
