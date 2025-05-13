using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Controllers.Proyectos.SAP;
using WebApiCore.Security;

namespace WebApiCore.Controllers.Proyectos.MyPolarier.Administracion;
public class ArticulosController : ODataController
{
    private readonly bdERP db;
    public ArticulosController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpPost("odata/MyPolarier/Administracion/Articulos/UpdateTablasSAP")]
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
