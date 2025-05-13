using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.ComponentModel.DataAnnotations;
using WebApiCore.Class.externos.SAP.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class ArticuloController : ODataController
{
    private readonly SAPWrap sap = new();
    private readonly bdERP db;
    public ArticuloController(bdERP context)
    {
        db = context;
    }

    public class DescripcionArticulo
    {
        public DescripcionArticulo(string descripcion)
        {
            this.descripcion = descripcion;
        }
        [Key]
        public string descripcion { get; set; }
    }

    [EnableQuery]
    [HttpGet]
    [Route("odata/Administracion/AsociativaArticulo")]
    [Authorize]
    public async Task<ActionResult> GetAsociativaArticulo()
    {
        List<string> list = new();

        var tblArticuloNAdmAlbaranCompra = db.tblArticuloNAdmAlbaranCompra.Where(x => x.descripcion != null).Select(x => x.descripcion).Distinct();
        var tblArticuloNAdmAlbaranVenta = db.tblArticuloNAdmAlbaranVenta.Where(x => x.descripcion != null).Select(x => x.descripcion).Distinct();
        var tblArticuloNAdmPedidoCliente = db.tblArticuloNAdmPedidoCliente.Where(x => x.descripcion != null).Select(x => x.descripcion).Distinct();
        var tblArticuloNAdmPedidoProveedor = db.tblArticuloNAdmPedidoProveedor.Where(x => x.descripcion != null).Select(x => x.descripcion).Distinct();
        var tblArticuloNAdmPresupuestoVenta = db.tblArticuloNAdmPresupuestoVenta.Where(x => x.descripcion != null).Select(x => x.descripcion).Distinct();

        list.AddRange(tblArticuloNAdmAlbaranCompra);
        list.AddRange(tblArticuloNAdmAlbaranVenta);
        list.AddRange(tblArticuloNAdmPedidoCliente);
        list.AddRange(tblArticuloNAdmPedidoProveedor);
        list.AddRange(tblArticuloNAdmPresupuestoVenta);

        return Ok(list.Distinct().Select(x => new DescripcionArticulo(x)));
    }

    [EnableQuery]
    [HttpGet]
    [Route("odata/Administracion/Articulo")]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] bool enableArticulosLibres = false)
    {

        var list = new List<Articulo>();

        var articulosRecambios = db.tblRecambio.Where(x => x.activo == true && x.eliminado == false).Select(x => new Articulo
        {
            tipoArticulo = 1, // Recambio
            idArticulo = x.idRecambio,
            codigo = x.referenciaInterna,
            denominacion = x.denominacion,
            idAdmCuentaContableCompra = db.tblAdmTipoArticulo.First(x => x.idAdmTipoArticulo == 1).idAdmCuentaContableCompra,
            idAdmCuentaContableVenta = db.tblAdmTipoArticulo.First(x => x.idAdmTipoArticulo == 1).idAdmCuentaContableVenta
        });

        var articulosLenceria = db.tblArticuloLenceria.Where(x => x.eliminado == false).Select(x => new Articulo
        {
            tipoArticulo = 2, // Lenceria
            idArticulo = x.idArticuloLenceria,
            codigo = x.codigo,
            denominacion = x.denominacion,
            idAdmCuentaContableCompra = x.idAdmCuentaContableCompra ?? db.tblAdmTipoArticulo.First(x => x.idAdmTipoArticulo == 2).idAdmCuentaContableCompra,
            idAdmCuentaContableVenta = x.idAdmCuentaContableVenta ?? db.tblAdmTipoArticulo.First(x => x.idAdmTipoArticulo == 2).idAdmCuentaContableVenta,
            idDenoPrenda = x.idDenoPrenda
        });

        var articulosMaquinaria = db.tblArticuloMaquinaria.Where(x => x.eliminado == false).Select(x => new Articulo
        {
            tipoArticulo = 3, // Maquinaria
            idArticulo = x.idArticuloMaquinaria,
            codigo = x.codigo,
            denominacion = x.denominacion,
            idAdmCuentaContableCompra = x.idAdmCuentaContableCompra ?? db.tblAdmTipoArticulo.First(x => x.idAdmTipoArticulo == 3).idAdmCuentaContableCompra,
            idAdmCuentaContableVenta = x.idAdmCuentaContableVenta ?? db.tblAdmTipoArticulo.First(x => x.idAdmTipoArticulo == 3).idAdmCuentaContableVenta,
            idCategoriaMaquina = x.idCategoriaMaquina
        });

        var articulosLogistico = db.tblArticuloLogistico.Where(x => x.eliminado == false).Select(x => new Articulo
        {
            tipoArticulo = 5, // Logistica
            idArticulo = x.idArticuloLogistico,
            denominacion = x.denominacion,
            idAdmCuentaContableCompra = x.idAdmCuentaContableCompra ?? db.tblAdmTipoArticulo.First(x => x.idAdmTipoArticulo == 3).idAdmCuentaContableCompra,
            idAdmCuentaContableVenta = x.idAdmCuentaContableVenta ?? db.tblAdmTipoArticulo.First(x => x.idAdmTipoArticulo == 3).idAdmCuentaContableVenta
        });


        list.AddRange(articulosMaquinaria);
        list.AddRange(articulosLenceria);
        list.AddRange(articulosRecambios);
        list.AddRange(articulosLogistico);

        if (enableArticulosLibres)
        {
            var articulosLibre = db.tblGrupoArticulos.Where(x => x.isEliminado == false).Select(x => new Articulo
            {
                tipoArticulo = 4,
                idArticulo = x.idGrupoArticulos,
                codigo = x.codigo,
                denominacion = x.denominacion,
                idAdmCuentaContableCompra = x.idAdmCuentaContableCompra ?? db.tblAdmTipoArticulo.First(x => x.idAdmTipoArticulo == 4).idAdmCuentaContableCompra,
            });
            list.AddRange(articulosLibre);
        }

        return Ok(list);
    }

    [HttpPost]
    [Route("odata/Administracion/Articulo")]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] dynamic articulo)
    {

        if (articulo == null)
            return BadRequest("No se ha recibido ningún artículo.");

        var tipoArticulo = (int)articulo.tipoArticulo;

        switch (tipoArticulo)
        {
            case 2: // Lenceria
                var objArticuloLenceria = new tblArticuloLenceria
                {
                    denominacion = articulo.denominacion,
                    idDenoPrenda = articulo.idDenoPrenda,
                    idAdmCuentaContableCompra = articulo.idAdmCuentaContableCompra,
                    idAdmCuentaContableVenta = articulo.idAdmCuentaContableVenta,
                    codigo = ""
                };

                db.tblArticuloLenceria.Add(objArticuloLenceria);
                break;

            case 3: // Maquinaria
                var objArticuloMaquinaria = new tblArticuloMaquinaria
                {
                    denominacion = articulo.denominacion,
                    idCategoriaMaquina = articulo.idCategoriaMaquina,
                    idAdmCuentaContableCompra = articulo.idAdmCuentaContableCompra,
                    idAdmCuentaContableVenta = articulo.idAdmCuentaContableVenta,
                    codigo = ""
                };

                db.tblArticuloMaquinaria.Add(objArticuloMaquinaria);
                break;

            case 5: // Logistica
                var objArticuloLogistico = new tblArticuloLogistico
                {
                    denominacion = articulo.denominacion,
                    idAdmCuentaContableCompra = articulo.idAdmCuentaContableCompra,
                    idAdmCuentaContableVenta = articulo.idAdmCuentaContableVenta,
                };

                db.tblArticuloLogistico.Add(objArticuloLogistico);
                break;

            default:
                return BadRequest();
        }

        await db.SaveChangesAsync();

        return Ok(true);
    }

    [HttpDelete]
    [Route("odata/Administracion/Articulo(tipoArticulo={tipoArticulo},idArticulo={idArticulo})")]
    [Authorize]
    public async Task<ActionResult> Delete([FromODataUri] int idArticulo, [FromODataUri] int tipoArticulo)
    {
        switch (tipoArticulo)
        {
            case 2: // Lenceria
                var objArticuloLenceria = db.tblArticuloLenceria.Find(idArticulo);
                if (objArticuloLenceria == null)
                    return NotFound();

                objArticuloLenceria.eliminado = true;
                break;

            case 3: // Maquinaria
                var objArticuloMaquinaria = db.tblArticuloMaquinaria.Find(idArticulo);
                if (objArticuloMaquinaria == null)
                    return NotFound();

                objArticuloMaquinaria.eliminado = true;
                break;
            case 5: // Logistica
                var objArticuloLogistico = db.tblArticuloLogistico.Find(idArticulo);
                if (objArticuloLogistico == null)
                    return NotFound();

                objArticuloLogistico.eliminado = true;
                break;

            default:
                return BadRequest();
        }

        await db.SaveChangesAsync();

        return Ok(true);
    }

    [HttpPatch]
    [Route("odata/Administracion/Articulo(tipoArticulo={tipoArticulo},idArticulo={idArticulo})")]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int idArticulo, [FromODataUri] int tipoArticulo, [FromBody] JsonPatchDocument<dynamic> articulo)
    {

        switch (tipoArticulo)
        {
            case 2: // Lenceria
                var objArticuloLenceria = db.tblArticuloLenceria.Find(idArticulo);
                if (objArticuloLenceria == null)
                    return NotFound();

                articulo.ApplyTo(objArticuloLenceria);
                break;

            case 3: // Maquinaria
                var objArticuloMaquinaria = db.tblArticuloMaquinaria.Find(idArticulo);
                if (objArticuloMaquinaria == null)
                    return NotFound();

                articulo.ApplyTo(objArticuloMaquinaria);
                break;
            case 5: // Logistica
                var objArticuloLogistico = db.tblArticuloLogistico.Find(idArticulo);
                if (objArticuloLogistico == null)
                    return NotFound();

                articulo.ApplyTo(objArticuloLogistico);
                break;
            default:
                return BadRequest();
        }

        await db.SaveChangesAsync();

        return Ok(true);
    }

    public class Articulo
    {
        public int tipoArticulo { get; set; }
        public int? idArticulo { get; set; }
        public string codigo { get; set; }
        public string denominacion { get; set; }
        public int? idAdmCuentaContableCompra { get; set; }
        public int? idAdmCuentaContableVenta { get; set; }
        public int? idDenoPrenda { get; set; }
        public int? idCategoriaMaquina { get; set; }
    }
}
