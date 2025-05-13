using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;
public class InfInventariosController : ODataController
{
    private readonly bdERP db;
    public InfInventariosController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpPost("odata/Informes/InformesEjecutivos/InfInventarios/fn_getFechaAnterior_AnalisisInventario")]
    [Authorize]
    public async Task<ActionResult> fn_getFechaAnterior_AnalisisInventario([FromODataUri] int idInventario)
    {
        var query = (from i in db.tblInventario
                     join eni in db.tblEntidadNInventario on i.idInventario equals eni.idInventario
                     join e in db.tblEntidad on eni.idEntidad equals e.idEntidad
                     where i.idInventario == idInventario
                     select new
                     {
                         idEntidad = (e.inventarioPorEntidad == null || e.inventarioPorEntidad == false) ? (int?)null : e.idEntidad,
                         idCompañia = (e.inventarioPorEntidad == null || e.inventarioPorEntidad == false) ? e.idCompañia : (int?)null
                     }).FirstOrDefault();

        int? idEntidad = query?.idEntidad;
        int? idCompañia = query?.idCompañia;

        var fechaAnterior = db.tblMovimiento
            .Where(m =>
                (idEntidad == null || m.idEntidad == idEntidad)
                && (idCompañia == null || m.idCompañia == idCompañia)
            )
            .OrderBy(m => m.fecha)
            .FirstOrDefault()
            ?.fecha;

        return Ok(fechaAnterior);
    }

    [EnableQuery]
    [HttpGet("odata/Informes/InformesEjecutivos/InfInventarios/spSelectValoracionInventarios_Porcentaje")]
    [Authorize]
    public async Task<ActionResult> spSelectValoracionInventarios_Porcentaje([FromODataUri] int idInventario)
    {
        var connection = db.Database.GetDbConnection();
        var agrupado = await connection.QueryAsync("EXEC [MyReporting].[EF_Inventarios_spSelectValoracionInventarios_Porcentaje] @idInventario", new { idInventario });
        var desglose = await connection.QueryAsync("EXEC [MyReporting].[EF_Inventarios_spSelectValoracionInventarios_desglose] @idInventario", new { idInventario });

        foreach (var grupo in agrupado)
        {
            grupo.desglose = desglose.Where(x => grupo.idPrenda == x.idPrenda).ToList();
        }

        return Ok(agrupado);
    }
}
