using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Class.Proyectos.MyPolarier.Administracion;
using WebApiCore.Context;
using WebApiCore.Security;

// Controller para Personal ANTIGÜO

namespace WebApiCore.Controllers.Proyectos.MyPolarier.RRHH;
public class PersonalController : ODataController
{
    private readonly bdERP db;

    public PersonalController(bdERP context)
    {
        db = context;
    }

    [HttpPost("odata/MyPolarier/RRHH/Personal/UpdatePersonasVIPS")]
    [Authorize]
    public async Task<ActionResult> UpdatePersonasVIPS()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var VIPS = new VIPSService(db, configuration);
        try
        {
            await VIPS.UpdatePersonasVIPS();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok(true);
    }
}