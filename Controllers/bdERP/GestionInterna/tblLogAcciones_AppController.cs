using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;


namespace WebApiCore.Controllers;

public class tblLogAcciones_AppController : ODataController
{
    private readonly bdERP db;

    public tblLogAcciones_AppController(bdERP context)
    {
        db = context;
    }

    [HttpGet]
    [EnableQuery]
    [Authorize]
    public async Task<ActionResult> Get()
    {
        return Ok(db.tblLogAcciones_App);
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] logAccion log)
    {
        try
        {
            DateTimeOffset fechaActual = DateTime.UtcNow;

            var dataLog = new tblLogAcciones_App()
            {
                idUsuario = log.idUsuario,
                fecha = fechaActual,
                app = log.app,
                versionApp = log.versionApp,
                isAndroid = log.isAndroid
            };

            db.tblLogAcciones_App.Add(dataLog);
            await db.SaveChangesAsync();

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }
}

public class logAccion
{
    public int idUsuario { get; set; }
    public string app { get; set; }
    public string versionApp { get; set; }
    public bool isAndroid { get; set; }

}