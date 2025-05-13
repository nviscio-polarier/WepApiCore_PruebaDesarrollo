using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Class.Proyectos.MyPolarier.RRHH;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblTipoDocumentoController : ODataController
{

    private readonly bdERP db;
    public tblTipoDocumentoController(bdERP context)
    {
        db = context;
    }

    [EnableQuery(MaxExpansionDepth = 3)]
    [HttpGet]
    [Authorize]

    public async Task<ActionResult> Get()
    {
        return Ok(db.tblTipoDocumento);
    }
}