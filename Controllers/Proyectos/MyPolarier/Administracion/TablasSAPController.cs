using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Controllers.Proyectos.SAP;
using WebApiCore.Security;

namespace WebApiCore.Controllers.Proyectos.MyPolarier.Administracion;

public class TablasSAPController : ODataController
{
    private readonly bdERP db;
    public TablasSAPController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpPost("odata/MyPolarier/Administracion/TablasSAP/UpdateTablasSAP")]
    [Authorize]
    public async Task<ActionResult> UpdateTablasSAP()
    {
        SAPInfoDumpController dumper = new(db);

        try
        {
            await dumper.UpdateAll();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return Ok(true);
    }
}
