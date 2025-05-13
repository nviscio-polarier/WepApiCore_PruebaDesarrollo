using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblFamiliaController : ODataController
{
    private readonly bdERP db;

    public tblFamiliaController(bdERP context)
    {
        db = context;
    }

    [Route("tblFamilia")]
    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<IQueryable<tblFamilia>> Get()
    {
        return db.tblFamilia;
    }
}
