using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;

namespace WebApiCore.Controllers;

public class tblLogErrorController : ODataController
{
    private readonly bdERP db;

    public tblLogErrorController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpPost("odata/tblLogError")]
    //[Authorize]
    public async Task<ActionResult> post([FromODataUri] string text)
    {
        try
        {
            db.tblLogError.Add(new tblLogError()
            {
                denominacion = text,
                error = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm")
            });

            db.SaveChanges();

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex);

        }
    }
}
