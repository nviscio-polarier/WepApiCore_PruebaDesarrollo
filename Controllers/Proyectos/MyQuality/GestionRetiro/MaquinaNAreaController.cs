using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApiCore.Class.bdERP.Administracion;
using WebApiCore.Context;
using WebApiCore.Security;
namespace WebApiCore.Controllers;

public class MaquinaNAreaController : ODataController
{
    private readonly bdERP db;

    public MaquinaNAreaController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet("odata/MyQuality/GestionRetiro/GetMaquinaNArea")]
    [Authorize]
    public async Task<ActionResult> GetMaquinaNArea([FromODataUri] int idLavanderia)
    {
        if (idLavanderia == null)
        {
            return BadRequest();
        }

        var maquinaNArea = db.tblPosicionNAreaLavanderiaNLavanderia
            .Include(x => x.idAreaLavanderiaNavigation)
            .Include(x => x.idMaquinaNavigation)
            .Where(x => x.idLavanderia == idLavanderia && x.activo == true)
            .Select(x => new {
                x.idMaquina, maquinaDenominacion = x.idMaquinaNavigation.denominacion,
                maquinaEtiqueta = x.idMaquinaNavigation.etiqueta,
                x.idAreaLavanderia, areaDenominacion = x.idAreaLavanderiaNavigation.denominacion
            })
            .Distinct()
            .OrderBy(x => x.maquinaDenominacion)
            .ToList();
        return Ok(maquinaNArea);
    }
}
