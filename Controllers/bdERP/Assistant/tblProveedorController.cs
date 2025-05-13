using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblProveedorController : ODataController
{
    private readonly bdERP db;

    public tblProveedorController(bdERP context)
    {
        db = context;
    }

    [HttpGet]
    [EnableQuery]
    [Authorize]
    public ActionResult Get([FromODataUri] int? idProveedor = null)
    {
        return Ok(db.tblProveedor.Where(p => (idProveedor == null || p.idProveedor == idProveedor) && p.activo == true && !p.eliminado));
    }

    [HttpGet]
    [EnableQuery]
    [Authorize]
    public ActionResult Get([FromODataUri] int key)
    {
        return Ok(db.tblProveedor.Where(p => p.idProveedor == key && p.activo == true && !p.eliminado));
    }
}
