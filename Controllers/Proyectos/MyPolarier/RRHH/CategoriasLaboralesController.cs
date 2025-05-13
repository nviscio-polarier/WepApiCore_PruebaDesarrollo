using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Class.Proyectos.MyPolarier.RRHH;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers.Proyectos.MyPolarier.RRHH
{
    public class CategoriasLaboralesController : ODataController
    {
        private readonly bdERP db;
        public CategoriasLaboralesController(bdERP context)
        {
            db = context;
        }

        [EnableQuery]
        [HttpPost("odata/CategoriasLaborales/PatchMasivo")]
        [Authorize]
        public async Task<ActionResult> PatchMasivo([FromBody] PatchMasivoDatosSalariales data)
        {

            var categoriaInterna = data.categoriaInterna;
            var idsPersonasExcluidas = data.idsPersonasExcluidas;

            // Hacer un patch en los datos de tblCategoriaInterna;
            var tblCategoriaInterna_mod = db.tblCategoriaInterna.FirstOrDefault(x => x.idCategoriaInterna == categoriaInterna.idCategoriaInterna);

            tblCategoriaInterna_mod.salarioBase = categoriaInterna.salarioBase;
            tblCategoriaInterna_mod.plusAsistencia = categoriaInterna.plusAsistencia;
            tblCategoriaInterna_mod.plusResponsabilidad = categoriaInterna.plusResponsabilidad;
            tblCategoriaInterna_mod.plusPeligrosidad = categoriaInterna.plusPeligrosidad;
            tblCategoriaInterna_mod.incentivo = categoriaInterna.incentivo;
            tblCategoriaInterna_mod.percSegSocial = categoriaInterna.percSegSocial;
            tblCategoriaInterna_mod.plusProductividad = categoriaInterna.plusProductividad;
            tblCategoriaInterna_mod.impHoraExtra = categoriaInterna.impHoraExtra;

            // Selecionar todas aquellas personas de la categoria interna que no esten en la lista de excluidos;

            var datosSalariales = db.tblDatosSalariales.Where(x => x.idPersonaNavigation.eliminado == false &&
            x.idPersonaNavigation.idCategoriaInterna == categoriaInterna.idCategoriaInterna &&
            !idsPersonasExcluidas.Any(ids => ids == x.idPersona));

            // Modificar los datos salariales de las personas seleccionadas;

            foreach (var ds in datosSalariales)
            {
                ds.salarioBase = categoriaInterna.salarioBase;
                ds.plusAsistencia = categoriaInterna.plusAsistencia;
                ds.plusResponsabilidad = categoriaInterna.plusResponsabilidad;
                ds.plusPeligrosidad = categoriaInterna.plusPeligrosidad;
                ds.incentivo = categoriaInterna.incentivo;
                ds.percSegSocial = categoriaInterna.percSegSocial;
                ds.plusProductividad = categoriaInterna.plusProductividad;
                ds.impHoraExtra = categoriaInterna.impHoraExtra;
            }

            await db.SaveChangesAsync();
            return Ok(true);
        }
    }
}

