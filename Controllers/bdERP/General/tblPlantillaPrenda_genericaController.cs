using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblPlantillaPrenda_genericaController : ODataController
{
    private readonly bdERP db;

    public tblPlantillaPrenda_genericaController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<IQueryable<tblPlantillaPrenda_generica>> Get([FromODataUri] int idGrupoPlantillaPrenda_generica, [FromODataUri] bool todas = false)
    {
        return db.tblPlantillaPrenda_generica.Where(x => x.idGrupoPlantillaPrenda_generica == idGrupoPlantillaPrenda_generica || todas);
    }

    [EnableQuery]
    [HttpGet("odata/tblPlantillaPrenda_generica({key})")]
    [Authorize]
    public async Task<IQueryable<tblPlantillaPrenda_generica>> Get([FromODataUri] int key)
    {
        return db.tblPlantillaPrenda_generica.Where(x => x.idPlantillaPrenda_generica == key);
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblPlantillaPrenda_generica> prendaGenerica)
    {
        var entity = db.tblPlantillaPrenda_generica
            .FirstOrDefault(x => x.idPlantillaPrenda_generica == key);
        if (entity == null)
        {
            return NotFound();
        }

        var operations_tblPlantillaPrenda_generica_historico_peso = prendaGenerica.Operations.FirstOrDefault(x => x.path == "/tblPlantillaPrenda_generica_historico_peso");
        if (operations_tblPlantillaPrenda_generica_historico_peso != null)
        {
            db.tblPlantillaPrenda_generica_historico_peso.RemoveRange(db.tblPlantillaPrenda_generica_historico_peso.Where(x => x.idPlantillaPrenda_generica == key));
        }

        var operations_denominacion = prendaGenerica.Operations.FirstOrDefault(x => x.path == "/denominacion");
        if (operations_denominacion != null)
        {
            var denominacion = operations_denominacion.value as string;

            var prenda = db.tblPrenda.Where(x => x.idPlantillaPrenda_generica == key);
            foreach (var item in prenda)
            {
                item.denominacion = denominacion;
            }
        }

        prendaGenerica.ApplyTo(entity);

        await db.SaveChangesAsync();
        return Updated(entity);
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Post([FromBody] tblPlantillaPrenda_generica entity)
    {
        if (entity == null)
        {
            return BadRequest(ModelState);
        }

        var grupoGenerico = db.tblGrupoPlantillaPrenda_generica.FirstOrDefault(x => x.idGrupoPlantillaPrenda_generica == entity.idGrupoPlantillaPrenda_generica);
        if (grupoGenerico == null)
            return BadRequest("Grupo prenda generica no encontrado");

        var denoPrenda = db.tblDenoPrenda
            .Include(x => x.idTipoPrendaNavigation)
            .ThenInclude(x => x.idFamiliaNavigation)
            .FirstOrDefault(x => x.idDenoPrenda == entity.idDenoPrenda);
        if (denoPrenda == null)
            return BadRequest("Deno prenda no encontrado");

        entity.codigoDenoPrenda = denoPrenda.idTipoPrendaNavigation.idFamiliaNavigation.codigo + denoPrenda.idTipoPrendaNavigation.codigo + denoPrenda.codigo;

        entity.activo = true;
        db.tblPlantillaPrenda_generica.Add(entity);

        await db.SaveChangesAsync();

        return Created(entity);
    }

    public void UpdatePrenda(ICollection<tblPrenda> tblPrenda, tblPlantillaPrenda_generica objPrendaGenerica)
    {
        PropertyInfo[] PrendaGenerica_properties = typeof(tblPlantillaPrenda_generica).GetProperties();
        PropertyInfo[] Prenda_properties = typeof(tblPrenda).GetProperties();
        foreach (PropertyInfo property in PrendaGenerica_properties)
        {
            string propertyName = property.Name;
            object propertyValue = property.GetValue(objPrendaGenerica);

            if (!Prenda_properties.Any(x => x.Name == propertyName) && propertyName != "tblPlantillaPrenda_generica_historico_peso") continue;

            foreach (var item in tblPrenda)
            {
                if (propertyName == "tblPlantillaPrenda_generica_historico_peso")
                {
                    var historicoPeso = ((ICollection<tblPlantillaPrenda_generica_historico_peso>)propertyValue).OrderByDescending(x => x.fecha).FirstOrDefault();
                    if (historicoPeso != null)
                    {
                        var currentHistoricoPeso = item.tblPrenda_historico_peso.OrderByDescending(x => x.fecha).FirstOrDefault(x => x.fecha == historicoPeso.fecha);
                        if (currentHistoricoPeso != null)
                        {
                            currentHistoricoPeso.peso = historicoPeso.peso;
                        }
                        else
                        {
                            item.tblPrenda_historico_peso.Add(new tblPrenda_historico_peso
                            {
                                idPrenda = item.idPrenda,
                                peso = historicoPeso.peso,
                                fecha = historicoPeso.fecha
                            });
                        }
                        item.peso = historicoPeso.peso;
                    }
                }
                else
                {
                    var prendaProperty = Prenda_properties.FirstOrDefault(x => x.Name == propertyName);
                    prendaProperty?.SetValue(item, propertyValue);
                }
            }
        }
    }
}
