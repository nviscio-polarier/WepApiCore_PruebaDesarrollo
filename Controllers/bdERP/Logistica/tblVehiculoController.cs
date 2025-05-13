using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;


namespace WebApiCore.Controllers;

public class tblVehiculoController : ODataController
{
    private readonly bdERP db;
    public tblVehiculoController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get()
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        tblUsuario objUsuario = db.tblUsuario.Where(x => x.idUsuario == idUsuario && !x.isEliminado).FirstOrDefault();
        if (objUsuario == null)
            return BadRequest();

        List<int> idsLavanderia = db.tblLavanderia.Where(x => x.idUsuario.Select(y => y.idUsuario).Contains(idUsuario)).Select(x => x.idLavanderia).ToList();
        return Ok(db.tblVehiculo.Where(vehiculo => vehiculo.idLavanderia.Any(vnl => idsLavanderia.Contains((int)vnl.idLavanderia)) && !vehiculo.eliminado));
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] byte key)
    {
        return Ok(await db.tblVehiculo.FindAsync(key));
    }
}
