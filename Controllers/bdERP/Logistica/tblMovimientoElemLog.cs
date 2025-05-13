//using Microsoft.AspNetCore.JsonPatch;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.OData.Formatter;
//using Microsoft.AspNetCore.OData.Query;
//using Microsoft.AspNetCore.OData.Routing.Controllers;
//using WebApiCore.Context;
//using WebApiCore.Security;

//namespace WebApiCore.Controllers;

//public class tblMovimientoElemLogController : ODataController
//{
//    private readonly bdERP db;

//    public tblMovimientoElemLogController(bdERP context)
//    {
//        db = context;
//    }

//    [EnableQuery]
//    [HttpGet]
//    [Authorize]
//    public async Task<ActionResult> Get()
//    {
//        return Ok(db.tblMovimientoElemLog);
//    }

//    [EnableQuery]
//    [HttpPost]
//    [Authorize]
//    public async Task<ActionResult> Post([FromBody] tblMovimientoElemLog movimientoElemLog)
//    {
//        try
//        {
//            movimientoElemLog.fecha = DateTimeOffset.UtcNow;

//            db.tblMovimientoElemLog.Add(movimientoElemLog);
//            await db.SaveChangesAsync();

//            return Created(movimientoElemLog);
//        }
//        catch (Exception ex)
//        {
//            return BadRequest();
//        }
//    }

//    [EnableQuery]
//    [HttpPatch]
//    [Authorize]
//    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblMovimientoElemLog> movElemLog)
//    {
//        var entity = db.tblMovimientoElemLog.FirstOrDefault(x => x.idMovimientoElemLog.Equals(key));
//        if (entity == null)
//            return BadRequest();

//        var operation = movElemLog.Operations.FirstOrDefault(x => x.path.Equals("/tblCantidadNMovimientoElemLog"));
//        if (operation != null)
//        {
//            db.tblCantidadNMovimientoElemLog.RemoveRange(db.tblCantidadNMovimientoElemLog.Where(x => x.idMovimientoElemLog.Equals(key)));
//        }


//        movElemLog.ApplyTo(entity);

//        await db.SaveChangesAsync();

//        return Ok(entity);
//    }
//}
