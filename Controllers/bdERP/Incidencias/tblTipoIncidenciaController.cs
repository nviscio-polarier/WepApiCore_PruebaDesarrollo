using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;

namespace WebApiCore.Controllers;

public class tblTipoIncidenciaController : ODataController
{
    private readonly bdERP db;

    public tblTipoIncidenciaController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    public async Task<IQueryable<tblTipoIncidencia>> Get()
    {
        return db.tblTipoIncidencia;
    }

    [EnableQuery]
    [HttpGet]
    public async Task<IQueryable<tblTipoIncidencia>> Get([FromODataUri] int key)
    {
        return db.tblTipoIncidencia.Where(x => x.idTipoIncidencia == key);
    }
}
