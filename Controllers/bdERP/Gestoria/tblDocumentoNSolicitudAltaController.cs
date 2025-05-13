using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Class.Proyectos.MyPolarier.RRHH;
using WebApiCore.Context;
using WebApiCore.Controllers.Proyectos.MyPolarier.Gestoria;
using WebApiCore.Enums.RRHH;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblDocumentoNSolicitudAltaController : ODataController
{
    private readonly bdERP db;
    public tblDocumentoNSolicitudAltaController(bdERP context)
    {
        db = context;
    }

    [EnableQuery(MaxExpansionDepth = 3)]
    [HttpGet]
    [Authorize]

    public async Task<ActionResult> Get()
    {
        return Ok(db.tblDocumentoNSolicitudAlta);
    }

}