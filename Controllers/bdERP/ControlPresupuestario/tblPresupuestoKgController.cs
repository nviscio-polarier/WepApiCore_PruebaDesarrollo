using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblPresupuestoKgController : ODataController
{
    private readonly bdERP db;

    public tblPresupuestoKgController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblPresupuestoKg presupuestoKg)
    {
        var idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        var objUsuario = db.tblUsuario.FirstOrDefault(x => x.idUsuario.Equals(idUsuario));
        if (objUsuario == null)
        {
            return BadRequest();
        }
        if (presupuestoKg == null) return BadRequest();

        db.tblPresupuestoKg.Add(presupuestoKg);
        await db.SaveChangesAsync();

        return Created(presupuestoKg);
    }
}

