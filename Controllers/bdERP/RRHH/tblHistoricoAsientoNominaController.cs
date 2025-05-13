using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblHistoricoAsientoNominaController : ODataController
{
    private readonly bdERP db;

    public tblHistoricoAsientoNominaController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public ActionResult Get([FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta)
    {
        var tblHistoricoAsientoNomina = db.tblHistoricoAsientoNomina
            .Where(han => han.fechaDesde >= fechaDesde && han.fechaHasta <= fechaHasta);

        return Ok(tblHistoricoAsientoNomina);
    }
}
