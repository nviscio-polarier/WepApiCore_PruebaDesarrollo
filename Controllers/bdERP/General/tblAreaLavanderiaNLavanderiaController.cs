using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Class.bdERP.General;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

[Authorize]
public class tblAreaLavanderiaNLavanderiaController : ODataController
{
    private readonly bdERP db;

    public tblAreaLavanderiaNLavanderiaController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    public async Task<ActionResult> Get()
    {
        return Ok(db.tblAreaLavanderiaNLavanderia);
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] tblAreaLavanderiaNLavanderiaAgrupado areas)
    {
        try
        {
            db.tblAreaLavanderiaNLavanderia.AddRange(areas.lista);
            foreach (var area in areas.lista)
            {
                db.tblPosicionNAreaLavanderiaNLavanderia.Add(new tblPosicionNAreaLavanderiaNLavanderia()
                {
                    denominacion = "",
                    idAreaLavanderia = area.idAreaLavanderia,
                    idLavanderia = area.idLavanderia,
                    numPos = 1
                });
            }

            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok(areas.lista.ToList());
    }
}
