using iText.Kernel.XMP.Impl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApiCore.Context;
using WebApiCore.Enums.GestionInterna;
using WebApiCore.Security;
namespace WebApiCore.Controllers;

public class PrendasNFamiliaController : ODataController
{
    private readonly bdERP db;
    public PrendasNFamiliaController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet("odata/MyQuality/GestionRetiro/GetPrendas")]
    [Authorize]
    public async Task<ActionResult> GetPrendas([FromODataUri] int idCompañia, [FromODataUri] int idEntidad, [FromODataUri] int idGrupoPrenda, [FromODataUri] int idLavanderia)
    {
        if ((idCompañia == null || idCompañia == 0) && (idEntidad == null || idEntidad == 0) && (idGrupoPrenda == null || idGrupoPrenda == 0))
        {
            return BadRequest();
        }

        try
        {
            if(idCompañia != null && idCompañia != 0)
            {
                var result = db.tblPrenda
                    .Where(x => x.activo == true && x.idCompañia == idCompañia && x.idPlantillaPrenda_generica == null)
                    .Include(x => x.idDenoPrendaNavigation)
                        .ThenInclude(x => x.idTipoPrendaNavigation)
                            .ThenInclude(x => x.idFamiliaNavigation)
                    .Include(x => x.idColorTapaNavigation)
                    .Select(x => new
                    {
                        x.idPrenda,
                        x.denominacion,
                        x.codigoPrenda,
                        x.idColorTapaNavigation.codigoHexadecimal,
                        x.idDenoPrendaNavigation.idTipoPrendaNavigation.idFamiliaNavigation.idFamilia
                    })
                    .OrderBy(x => x.denominacion);
                return Ok(result);
            } else if (idEntidad != null && idEntidad != 0)
            {
                var result = db.tblPrenda
                    .Where(x => x.activo == true && x.idEntidad == idEntidad && x.idPlantillaPrenda_generica == null)
                    .Include(x => x.idDenoPrendaNavigation)
                        .ThenInclude(x => x.idTipoPrendaNavigation)
                            .ThenInclude(x => x.idFamiliaNavigation)
                    .Include(x => x.idColorTapaNavigation)
                    .Select(x => new
                    {
                        x.idPrenda,
                        x.denominacion,
                        x.codigoPrenda,
                        x.idColorTapaNavigation.codigoHexadecimal,
                        x.idDenoPrendaNavigation.idTipoPrendaNavigation.idFamiliaNavigation.idFamilia
                    })
                    .OrderBy(x => x.denominacion);
                return Ok(result);
            }
            else
            {
                var idsVisibles = db.tblEntidad
                    .Where(x => x.idLavanderia.Any(y => y.idLavanderia == idLavanderia))
                    .Join(db.tblPrenda,
                        entidad => entidad.idCompañia,
                        prenda => prenda.idCompañia,
                        (entidad, prenda) => prenda
                    )
                    .Where(y => y.activo == true
                        && y.idPlantillaPrenda_generica != null
                    )
                    .Select(x => x.idPlantillaPrenda_generica)
                    .Distinct();

                var result = db.tblPlantillaPrenda_generica
                    .Where(x => x.activo == true && x.idGrupoPlantillaPrenda_generica == idGrupoPrenda && idsVisibles.Contains(x.idPlantillaPrenda_generica))
                    .Include(x => x.idDenoPrendaNavigation)
                        .ThenInclude(x => x.idTipoPrendaNavigation)
                            .ThenInclude(x => x.idFamiliaNavigation)
                    .Include(x => x.idColorTapaNavigation)
                    .Select(x => new
                    {
                        idPrenda = x.idPlantillaPrenda_generica,
                        x.denominacion,
                        x.codigoDenoPrenda,
                        x.idColorTapaNavigation.codigoHexadecimal,
                        x.idDenoPrendaNavigation.idTipoPrendaNavigation.idFamiliaNavigation.idFamilia
                    })
                    .OrderBy(x => x.denominacion);
                var count = result.ToList().Count;
                return Ok(result);
            }

        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [EnableQuery]
    [HttpPost("odata/MyQuality/GestionRetiro/GetPrendasByIds")]
    [Authorize]
    public async Task<ActionResult> GetPrendasByIds([FromBody] IntArrayWrapper intArrayWrapper, [FromODataUri] bool isGenerico)
    {
        int[] ids = intArrayWrapper.ids;
        if (ids == null)
        {
            return BadRequest();
        }

        try
        {
            if (isGenerico)
            {
                var result = db.tblPlantillaPrenda_generica
                    .Where(x => ids.Contains(x.idPlantillaPrenda_generica))
                    .Include(x => x.idDenoPrendaNavigation)
                        .ThenInclude(x => x.idTipoPrendaNavigation)
                            .ThenInclude(x => x.idFamiliaNavigation)
                    .Include(x => x.idColorTapaNavigation)
                    .Select(x => new
                    {
                        idPrenda = x.idPlantillaPrenda_generica,
                        x.denominacion,
                        x.codigoDenoPrenda,
                        x.idColorTapaNavigation.codigoHexadecimal,
                        x.idDenoPrendaNavigation.idTipoPrendaNavigation.idFamiliaNavigation.idFamilia
                    })
                    .OrderBy(x => x.denominacion); ;

                return Ok(result);
            } else
            {
                var result = db.tblPrenda
                    .Where(x => ids.Contains(x.idPrenda))
                    .Include(x => x.idDenoPrendaNavigation)
                        .ThenInclude(x => x.idTipoPrendaNavigation)
                            .ThenInclude(x => x.idFamiliaNavigation)
                    .Include(x => x.idColorTapaNavigation)
                    .Select(x => new
                    {
                        x.idPrenda,
                        x.denominacion,
                        x.codigoPrenda,
                        x.idColorTapaNavigation.codigoHexadecimal,
                        x.idDenoPrendaNavigation.idTipoPrendaNavigation.idFamiliaNavigation.idFamilia
                    })
                    .OrderBy(x => x.denominacion); ;

                return Ok(result);
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    public class IntArrayWrapper
    {
        public int[] ids { get; set; }
    }
}
