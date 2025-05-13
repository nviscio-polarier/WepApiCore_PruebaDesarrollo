using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Class;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;
public class InventarioController : ODataController
{
    private readonly bdERP db;

    public InventarioController(bdERP context)
    {
        db = context;
    }

    private  enum InventarioEstado
    {
        Finalizado = 0,
        Abierto = 1,
        SubInventariosAbiertos = 2
    }


    [EnableQuery]
    [HttpGet("odata/MyPolarier/Inventario/GetInventariosAsociar")]
    [Authorize]
    public async Task<ActionResult> GetInventariosAsociar([FromODataUri] int idLavanderia, [FromODataUri] int? idGrupoInventario_generico)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        var idsEntidades = Utils.selectEntidadesVisibles(db, idUsuario, idLavanderia);

        var query = (from inv in db.tblInventario
                     join entiNInv in db.tblEntidadNInventario on inv.idInventario equals entiNInv.idInventario
                     join ent in db.tblEntidad.Where(x => x.idLavanderia.Any(x => x.idLavanderia == idLavanderia)) on entiNInv.idEntidad equals ent.idEntidad
                     where idsEntidades.Contains(ent.idEntidad)
                     select new
                     {
                         inv.idInventario,
                         inv.idInventarioPadre,
                         ent.idCompañia,
                         inv.denominacion,
                         inv.fecha,
                         estado = inv.estado == true ? InventarioEstado.Abierto 
                            : inv.estado == false && db.tblInventario.Any(x => x.idInventarioPadre == inv.idInventario && x.estado == true) ? InventarioEstado.Finalizado
                            : InventarioEstado.SubInventariosAbiertos,
                         asociado = inv.idGrupoInventario_generico.Any(x => x.idGrupoInventario_generico == idGrupoInventario_generico)
                     }).Distinct();


        return Ok(query);
    }

    [EnableQuery]
    [HttpPatch("odata/MyPolarier/Inventario/AsociarInventario({idGrupoInventario_generico})")]
    [Authorize]
    public async Task<ActionResult> AsociarInventario([FromODataUri] int idGrupoInventario_generico, [FromBody] List<int> idsInventario)
    {
        var gi = await db.tblGrupoInventario_generico.FindAsync(idGrupoInventario_generico);
        if (gi == null) return NotFound();

        var tblInventario = db.tblInventario
            .Include(x => x.idGrupoInventario_generico)
            .Where(x => idsInventario.Contains(x.idInventario))
            .ToList();

        foreach (var inv in tblInventario)
        {
            if (!inv.idGrupoInventario_generico.Contains(gi))
            {
                inv.idGrupoInventario_generico.Add(gi);
            }
        }
        
        await db.SaveChangesAsync();

        return Ok(true);
    }

    [EnableQuery]
    [HttpDelete("odata/MyPolarier/Inventario/DesasociarInventario({idInventario})")]
    [Authorize]
    public async Task<ActionResult> DesasociarInventario([FromODataUri] int idInventario, [FromODataUri] int idGrupoInventario_generico)
    {
        var gi = await db.tblGrupoInventario_generico.FindAsync(idGrupoInventario_generico);
        if (gi == null) return NotFound();

        var inv = db.tblInventario
            .Include(x => x.idGrupoInventario_generico)
            .FirstOrDefault(x => x.idInventario == idInventario);
        if (inv == null) return NotFound();

        inv.idGrupoInventario_generico.Remove(gi);

        await db.SaveChangesAsync();

        return Ok(true);
    }
}
