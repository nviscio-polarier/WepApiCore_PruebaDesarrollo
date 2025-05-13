using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Class;
using WebApiCore.Context;
using WebApiCore.Security;
namespace WebApiCore.Controllers;

public class tblGestionRetiroCustomController : ODataController
{
    private readonly bdERP db;
    public tblGestionRetiroCustomController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet("odata/MyQuality/GestionRetiro/GetRetiros")]
    [Authorize]
    public async Task<ActionResult> GetRetiros([FromODataUri] int idLavanderia, [FromODataUri] int idCompañia)
    {
        return Ok(
            db.tblGestionRetiro
            .Where(x => x.idLavanderia == idLavanderia && x.idCompañia == idCompañia)
            .Include(x => x.tblPrendaNGestionRetiro)
            .Include(x => x.idUsuarioNavigation)
                .ThenInclude(user => user.idPersonaNavigation)
            .OrderBy(x => x.isValidado)
            .ThenByDescending(x => x.fechaReg)
            );
    }
}
