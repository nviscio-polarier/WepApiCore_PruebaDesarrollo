using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData.Edm;
using System.Globalization;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblTasaCambioPresupuestoController : ODataController
{
    private readonly bdERP db;

    public tblTasaCambioPresupuestoController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public ActionResult Get()
    {
        return Ok(db.tblTasaCambioPresupuesto);
    }
}
