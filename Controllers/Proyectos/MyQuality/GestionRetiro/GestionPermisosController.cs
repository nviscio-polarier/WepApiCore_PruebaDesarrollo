using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApiCore.Context;
using WebApiCore.Security;
namespace WebApiCore.Controllers;

public class GestionPermisosController : ODataController
{
    private readonly bdERP db;

    public GestionPermisosController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet("odata/MyQuality/GestionRetiro/GetIsEncargado")]
    [Authorize]
    public async Task<ActionResult> GetIsEncargado()
    {
        int? idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        var objUsuario = db.tblUsuario.FirstOrDefault(x => x.idUsuario.Equals(idUsuario));
        if (objUsuario == null)
        {
            return BadRequest();
        }
        List<int?> categoriaInternaEncargado = new List<int?>() { 6, 7, 12, 42, 43, 44, 77 };
        List<int?> categoriaEncargado = new List<int?>() { 1, 12 };

        /*short?*/
        var categoriasUsuario = db.tblUsuario
            .Include(x => x.idPersonaNavigation)
            .Where(x => x.idUsuario == idUsuario)
            .Select(x => new
            {
                x.idPersonaNavigation.idCategoria,
                x.idPersonaNavigation.idCategoriaInterna
            })
            .FirstOrDefault();
        bool isEncargado = false;
        if (categoriasUsuario != null)
        {
            if (categoriasUsuario.idCategoria == null)
            {
                isEncargado = categoriaInternaEncargado.Contains(categoriasUsuario.idCategoriaInterna);
            }
            else if (categoriasUsuario.idCategoriaInterna == null)
            {
                isEncargado = categoriaEncargado.Contains(categoriasUsuario.idCategoria);
            }
            else
            {
                isEncargado = categoriaInternaEncargado.Contains(categoriasUsuario.idCategoriaInterna) || categoriaEncargado.Contains(categoriasUsuario.idCategoria);
            }
        }

        return Ok(isEncargado);
    }
}
