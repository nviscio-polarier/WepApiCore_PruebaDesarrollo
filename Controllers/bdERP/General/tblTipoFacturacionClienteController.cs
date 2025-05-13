using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblTipoFacturacionClienteController : ODataController
{
    private readonly bdERP db;

    public tblTipoFacturacionClienteController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<IQueryable<tblTipoFacturacionCliente>> Get()
    {
        return db.tblTipoFacturacionCliente;
    }
}
