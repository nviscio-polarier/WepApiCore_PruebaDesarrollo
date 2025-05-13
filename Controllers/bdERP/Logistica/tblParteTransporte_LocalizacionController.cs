using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblParteTransporte_LocalizacionController : ODataController
{
    private readonly bdERP db;

    public tblParteTransporte_LocalizacionController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get(int? idParteTransporte)
    {
        if (idParteTransporte != null)
        {
            return Ok(db.tblParteTransporte_Localizacion.Where(x => x.idParteTransporte.Equals(idParteTransporte)));
        }
        else
        {
            return Ok(db.tblParteTransporte_Localizacion);
        }
    }
}
