using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblPersonaNTipoContratoController : ODataController
{
    private readonly bdERP db;

    public tblPersonaNTipoContratoController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<IQueryable<tblPersonaNTipoContrato>> Get([FromODataUri] int idPersona)
    {
        return db.tblPersonaNTipoContrato.Where(x => x.idPersona == idPersona);
    }
}
