using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Class.bdERP.General;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers.Proyectos.MyPolarier.General
{
    public class EstructuraOperativaController : ODataController
    {
        private readonly bdERP db;
        public EstructuraOperativaController(bdERP context)
        {
            db = context;
        }

        [HttpGet("odata/MyPolarier/ParametrosGenerales/EstructuraOperativa/EstructuraOperativa_Maquinas")]
        [Authorize]
        public async Task<ActionResult> EstructuraOperativa_Maquinas([FromODataUri] int idLavanderia)
        {
            var lista = db.tblMaquina
                .Where(m => m.idLavanderia == idLavanderia && m.activo == true && m.eliminado == false &&
                            db.tblPosicionNAreaLavanderiaNLavanderia.Where(p => p.idMaquina == m.idMaquina).Count() > 0)
                .Select(m => new
                {
                    m.idMaquina,
                    m.denominacion,
                    m.etiqueta,
                    m.idTipoMaquinaNCategoriaMaquinaNavigation.idTipoMaquina,
                }).ToList()
            .OrderByDescending(x => x.idTipoMaquina == 1)
            .ThenByDescending(x => x.idTipoMaquina == 34)
            .ThenBy(x => x.denominacion);

            return Ok(lista);
        }

        [EnableQuery]
        [HttpPost("odata/MyPolarier/ParametrosGenerales/EstructuraOperativa/IUD_tblPosicionNAreaLavanderiaNLavanderia")]
        [Authorize]
        public async Task<ActionResult> IUD_tblPosicionNAreaLavanderiaNLavanderia([FromODataUri] int idMaquina, [FromODataUri] int idLavanderia, [FromODataUri] int idAreaLavanderia, [FromBody] tblPosicionNAreaLavanderiaNLavanderiaAgrupado posiciones)
        {
            try
            {
                int i = 0;
                List<tblPosicionNAreaLavanderiaNLavanderia> posicionesModificadas = posiciones.lista
                    .Where(pos => pos.idPosicionNAreaLavanderiaNLavanderia != 0)
                    .OrderBy(pos => pos.numPos)
                    .ToList();

                if (posicionesModificadas.Count > 0)
                {
                    foreach (var pos in posicionesModificadas)
                    {
                        i++;
                        pos.numPos = ((byte)i);
                    }
                    db.tblPosicionNAreaLavanderiaNLavanderia.UpdateRange(posicionesModificadas);
                }

                List<tblPosicionNAreaLavanderiaNLavanderia> posicionesNuevas = posiciones.lista.Where(pos => pos.idPosicionNAreaLavanderiaNLavanderia == 0).ToList();
                if (posicionesNuevas.Count > 0)
                {
                    foreach (var pos in posicionesNuevas)
                    {
                        i++;
                        pos.numPos = ((byte)i);
                    }
                    db.tblPosicionNAreaLavanderiaNLavanderia.AddRange(posicionesNuevas);
                }

                List<tblPosicionNAreaLavanderiaNLavanderia> posicionesEliminadas =
                    db.tblPosicionNAreaLavanderiaNLavanderia
                    .Where(x => x.idLavanderia == idLavanderia && x.idAreaLavanderia == idAreaLavanderia && x.idMaquina == idMaquina)
                    .ToList()
                    .Where(x => posiciones.lista.Find(p => p.idPosicionNAreaLavanderiaNLavanderia == x.idPosicionNAreaLavanderiaNLavanderia) == null)
                    .ToList();

                if (posicionesEliminadas.Count > 0)
                {
                    db.tblPosicionNAreaLavanderiaNLavanderia.RemoveRange(posicionesEliminadas);
                }

                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(posiciones.lista.ToList());
        }

        [EnableQuery]
        [HttpPost("odata/MyPolarier/ParametrosGenerales/EstructuraOrganizativa/IUD_tblStockTipoElemLogNEntidad")]
        [Authorize]
        public async Task<ActionResult> IUD_tblStockTipoElemLogNEntidad([FromBody] tblStockTipoElemLogNEntidad stockTipoElemLogNEntidad)
        {
            try
            {
                var idEntidadStock = stockTipoElemLogNEntidad.idEntidad;
                var idTipoElemLogStock = stockTipoElemLogNEntidad.idTipoElemLog;
                var cantidadStock = stockTipoElemLogNEntidad.cantidad;
                var tblStockTipoElemLogNEntidad = db.tblStockTipoElemLogNEntidad;

                var itemStock = tblStockTipoElemLogNEntidad.FirstOrDefault(x => x.idEntidad == idEntidadStock && x.idTipoElemLog == idTipoElemLogStock);

                //INSERT
                if (itemStock == null && cantidadStock != 0)
                {
                    tblStockTipoElemLogNEntidad.Add(stockTipoElemLogNEntidad);
                    db.SaveChanges();
                }

                //UPDATE OR DELETE
                if (itemStock != null)
                {
                    if (cantidadStock == 0)
                    {
                        tblStockTipoElemLogNEntidad.Remove(itemStock);
                        db.SaveChanges();
                    }

                    itemStock.cantidad = cantidadStock;
                    db.SaveChanges();

                }

                var result = tblStockTipoElemLogNEntidad.Where(x => x.idEntidad == idEntidadStock);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
