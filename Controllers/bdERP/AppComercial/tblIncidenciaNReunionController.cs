using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblIncidenciaNReunionController : ODataController
{
    private readonly bdERP db;

    public tblIncidenciaNReunionController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<bool> Delete([FromODataUri] int key)
    {
        var entity = await db.tblIncidenciaNReunion.FindAsync(key);
        if (entity == null)
            return false;

        db.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }

}
