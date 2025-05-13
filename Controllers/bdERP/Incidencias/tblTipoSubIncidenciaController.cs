using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;

namespace WebApiCore.Controllers;

public class tblTipoSubIncidenciaController : ODataController
{
    private readonly bdERP db;

    public tblTipoSubIncidenciaController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    public async Task<IQueryable<tblTipoSubIncidencia>> Get()
    {
        return db.tblTipoSubIncidencia;
    }

    [EnableQuery]
    [HttpGet]
    public async Task<IQueryable<tblTipoSubIncidencia>> Get([FromODataUri] int key)
    {
        return db.tblTipoSubIncidencia.Where(x => x.idSubTipoIncidencia == key);
    }
}
