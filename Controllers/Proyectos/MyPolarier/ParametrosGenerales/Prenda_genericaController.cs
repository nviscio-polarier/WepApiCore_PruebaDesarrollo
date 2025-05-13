using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApiCore.Class;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers.Proyectos.MyPolarier.General
{
    public class PlantillaPrenda_genericaController : ODataController
    {
        private readonly bdERP db;
        private tblPlantillaPrenda_genericaController pgc;
        private tblPrendaController pc;
        public PlantillaPrenda_genericaController(bdERP context)
        {
            db = context;
            pgc = new tblPlantillaPrenda_genericaController(context);
            pc = new tblPrendaController(context);
        }

        [EnableQuery]
        [HttpGet("odata/PlantillaPrenda_generica/GetCompañias")]
        public async Task<ActionResult> GetCompañias([FromODataUri] int idPlantillaPrenda_generica, [FromODataUri] int idCorporacion, [FromODataUri] bool asociados = true)
        {
            int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
            var idsEntidades = Utils.selectEntidadesVisibles(db, idUsuario, idCorporacion);

            var compañiasNCorporacion = (from comp in db.tblCompañia.Include(x => x.tblPrenda)
                         join ent in db.tblEntidad on comp.idCompañia equals ent.idCompañia
                         from lav in ent.idLavanderia
                         where comp.activo == true && comp.eliminado == false && lav.idCorporacion == idCorporacion && (
                            (asociados && comp.tblPrenda.Any(x => x.idPlantillaPrenda_generica == idPlantillaPrenda_generica && x.eliminado == false)) ||
                            (!asociados && !comp.tblPrenda.Any(x => x.idPlantillaPrenda_generica == idPlantillaPrenda_generica && x.eliminado == false))
                         )
                         select new
                         {
                             comp.idCompañia,
                             comp.denominacion,
                             lav.idLavanderia,
                             denoLav = lav.denominacion,
                         }).Distinct();

            return Ok(compañiasNCorporacion);
        }

        [EnableQuery]
        [HttpPost("odata/PlantillaPrenda_generica/AddCompañia")]
        [Authorize]
        public async Task<ActionResult> AddCompañia([FromODataUri] int idPlantillaPrenda_generica, [FromBody] List<int> idsCompañia)
        {
            var objPrendaGenerica = db
                .tblPlantillaPrenda_generica
                .Include(x => x.idDenoPrendaNavigation)
                    .ThenInclude(x => x.idTipoPrendaNavigation)
                        .ThenInclude(x => x.idFamiliaNavigation)
                .Include(x => x.tblPlantillaPrenda_generica_historico_peso)
                .FirstOrDefault(x => x.idPlantillaPrenda_generica == idPlantillaPrenda_generica);
            if (objPrendaGenerica == null) return NotFound();

            var tblPrenda = db.tblPrenda
                .Include(x => x.tblPrenda_historico_peso)
                .Where(x => x.idPlantillaPrenda_generica == idPlantillaPrenda_generica && idsCompañia.Contains((int)x.idCompañia) && x.eliminado == true)
                .ToList();

            var codigoBase = 
                objPrendaGenerica.idDenoPrendaNavigation.idTipoPrendaNavigation.idFamiliaNavigation.codigo + 
                objPrendaGenerica.idDenoPrendaNavigation.idTipoPrendaNavigation.codigo + 
                objPrendaGenerica.idDenoPrendaNavigation.codigo;

            tblPrenda.AddRange(idsCompañia
                .Where(idCompañia => !tblPrenda.Any(p => p.idCompañia == idCompañia))
                .Select(idCompañia => {
                    
                    var codigo = codigoBase + db.tblPrenda.Count(x => x.codigoPrenda.StartsWith(codigoBase) && x.idCompañia == idCompañia).ToString().PadLeft(2, '0');
                    
                    return new tblPrenda
                    {
                        idCompañia = idCompañia,
                        idPlantillaPrenda_generica = idPlantillaPrenda_generica,
                        idLavanderia = 1, // Obligatorio por estructura BBDD
                        idxGsbs = "", // Obligatorio por estructura BBDD
                        codigoPrenda = codigo,
                    };
                }));

            pgc.UpdatePrenda(tblPrenda, objPrendaGenerica);

            var new_tblPrenda = tblPrenda.Where(x => x.idPrenda == 0).ToList();

            db.tblPrenda.AddRange(new_tblPrenda);

            await db.SaveChangesAsync();

            return Ok(true);
        }

        [EnableQuery]
        [HttpDelete("odata/PlantillaPrenda_generica/DeleteCompañia({idCompañia})")]
        [Authorize]
        public async Task<IActionResult> DeleteCompañia([FromODataUri] int idCompañia, [FromODataUri] int idPlantillaPrenda_generica)
        {
            var objPrenda = db.tblPrenda.FirstOrDefault(x => x.idCompañia == idCompañia && x.idPlantillaPrenda_generica == idPlantillaPrenda_generica);
            if (objPrenda == null) return NotFound();

            return await pc.Delete(objPrenda.idPrenda);
        }

        [EnableQuery]
        [HttpPatch("odata/PlantillaPrenda_generica/UpdateCompañia")]
        [Authorize]
        public async Task<IActionResult> UpdateCompañia([FromODataUri] int idPlantillaPrenda_generica, [FromBody] Dictionary<int, JsonPatchDocument> patchDictionary)
        {
            var objPlantilla = db.tblPlantillaPrenda_generica.FirstOrDefault(x => x.idPlantillaPrenda_generica == idPlantillaPrenda_generica);
            var idsCompañia = patchDictionary.Keys.ToList();
            var tblPrenda = db.tblPrenda
                .Where(x => 
                    x.idPlantillaPrenda_generica == idPlantillaPrenda_generica && 
                    idsCompañia.Contains((int)x.idCompañia) &&
                    x.eliminado == false
                ).ToList();
            if (objPlantilla == null) return NotFound();

            foreach (var item in patchDictionary)
            {
                var prendaSel = tblPrenda.FirstOrDefault(x => x.idCompañia == item.Key);
                if (prendaSel == null) return NotFound();

                var operations_tblPlantillaPrenda_generica_historico_peso = item.Value.Operations.FirstOrDefault(x => x.path == "/tblPlantillaPrenda_generica_historico_peso");
                if (operations_tblPlantillaPrenda_generica_historico_peso != null)
                {
                    var value = JsonConvert.DeserializeObject<tblPlantillaPrenda_generica_historico_peso>(operations_tblPlantillaPrenda_generica_historico_peso.value.ToString());

                    if (value != null)
                    {
                        prendaSel.peso = value.peso;
                        var objHistorico = db.tblPrenda_historico_peso.FirstOrDefault(x => x.idPrenda == prendaSel.idPrenda && x.fecha.Date == value.fecha.Date);
                        if (objHistorico != null)
                        {
                            objHistorico.peso = value.peso;
                        }
                        else
                        {
                            db.tblPrenda_historico_peso.Add(new tblPrenda_historico_peso
                            {
                                idPrenda = prendaSel.idPrenda,
                                peso = value.peso,
                                fecha = value.fecha,
                            });
                        }
                    }

                    item.Value.Operations.Remove(operations_tblPlantillaPrenda_generica_historico_peso);
                }


                item.Value.ApplyTo(prendaSel);
            }

            await db.SaveChangesAsync();

            return Ok(true);
        }
    }
}
