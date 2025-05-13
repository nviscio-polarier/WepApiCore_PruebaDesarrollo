using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using WebApiCore.Class;
using WebApiCore.Context;
using WebApiCore.Enums.RRHH;
using WebApiCore.Hubs;

namespace WebApiCore.Controllers.Proyectos.MyRealBonus
{
    public class tblProductosController : ODataController
    {

        private readonly bdERP db;
        private readonly IHubContext<NotificacionesHub> _hubContext;

        public tblProductosController(bdERP context, IHubContext<NotificacionesHub> hubContext)
        {
            db = context;
            _hubContext = hubContext;
        }


        /// <summary>
        /// Obtiene la lista de productos con sus imágenes
        /// </summary>
        [EnableQuery]
        [HttpGet("odata/getProductos")]
        public async Task<ActionResult> GetProductos()
        {
            try
            {
                var query = db.tblProductosTokens
                    .Include(p => p.idFamiliaProductoNavigation)
                    .Include(p => p.idImagenProductoNavigation)
                    .Select(p => new ProductoDTO
                    {
                        id = p.idProducto,
                        categoriaNombre = p.idFamiliaProductoNavigation.denominacion,
                        categoria = p.idFamiliaProducto,
                        foto = p.idImagenProductoNavigation.imagenBinario != null
                            ? $"data:{p.idImagenProductoNavigation.extension};base64,{Convert.ToBase64String(p.idImagenProductoNavigation.imagenBinario)}"
                            : null,
                        extension = p.idImagenProductoNavigation.extension,
                        title = p.denominacion,
                        price = p.precio,
                        descripcion = p.descripcion,
                        destacado = p.esDestacado
                    });

                var result = await query.ToListAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }



        /// <summary>
        /// Añade una nueva imagen a un producto
        /// </summary>
        [HttpPost("odata/addImagenProducto")]
        public async Task<ActionResult> addImagenProducto([FromBody] ImagenProductoDTO request)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.imagenBase64))
                {
                    return BadRequest("La imagen no puede estar vacía.");
                }

                byte[] imagenBinario;

                string imagenBase64 = request.imagenBase64;

                // Ojo con la coma
                if (imagenBase64.Contains(","))
                {
                    imagenBase64 = imagenBase64.Substring(imagenBase64.IndexOf(",") + 1);
                }
                imagenBinario = Convert.FromBase64String(imagenBase64);


                var insert = new tblImagenesProductos
                {
                    imagenBinario = imagenBinario,
                    extension = request.mimeType

                };

                db.tblImagenesProductos.Add(insert);
                await db.SaveChangesAsync();

                return Ok(new
                {
                    mensaje = "Imagen insertada!",
                    idImagen = insert.idImagen
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error" + ex.Message);
            }
        }

        [HttpPatch("odata/asociarImagenProducto")]
        public async Task<ActionResult> asociarImagenProducto([FromODataUri] int idProducto, [FromODataUri] int idImagen) {
            var producto = await db.tblProductosTokens.FindAsync(idProducto);
            if (producto == null)
            {
                return NotFound($"Producto con ID {idProducto} no encontrado.");
            }

            var imagen = await db.tblImagenesProductos.FindAsync(idImagen);
            if (imagen == null)
            {
                return NotFound($"Imagen con ID {idImagen} no encontrada.");
            }
            producto.idImagenProducto = idImagen;

            await db.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Imagen asociada al producto",
                idProducto = idProducto,
                idImagen = idImagen
            });
        }


        /// <summary>
        /// Crear un nuevo producto
        /// </summary>
        [HttpPost("odata/addProducto")]
        public async Task<ActionResult> addProducto([FromBody] ProductoDTO productoNuevo)
        {
            try
            {
                if (productoNuevo.price == null)
                {
                    return BadRequest("El precio del producto no puede ser nulo.");
                }

                var producto = new tblProductosTokens
                {
                    denominacion = productoNuevo.title,
                    descripcion = productoNuevo.descripcion,
                    precio = productoNuevo.price.Value, 
                    idFamiliaProducto = productoNuevo.idFamiliaProducto,
                    esDestacado = productoNuevo.destacado ?? false
                };

                db.tblProductosTokens.Add(producto);
                await db.SaveChangesAsync();

                return Ok(new
                {
                    mensaje = "Producto creado",
                    idProducto = producto.idProducto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear producto: {ex.Message}");
            }
        }



        /// <summary>
        /// Actualizar un producto existente
        /// </summary>
        [HttpPut("odata/actualizarProducto")]
        public async Task<ActionResult> ActualizarProducto( [FromODataUri] int idProducto, [FromBody] ProductoDTO productoActualizado)
        {
            try
            {
                var producto = await db.tblProductosTokens.FindAsync(idProducto);

                if (producto == null)
                {
                    return NotFound($"Producto con ID {idProducto} no encontrado.");
                }

                // Actualizar campos
                producto.denominacion = productoActualizado.title ?? producto.denominacion;
                producto.descripcion = productoActualizado.descripcion ?? producto.descripcion;
                producto.precio = productoActualizado.price ?? producto.precio;

                if (productoActualizado.destacado.HasValue)
                {
                    producto.esDestacado = productoActualizado.destacado.Value;
                }

                await db.SaveChangesAsync();

                return Ok(new
                {
                    mensaje = "Producto actualizado exitosamente",
                    idProducto = producto.idProducto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar producto: {ex.Message}");
            }
        }


        // DTOS


        /// <summary>
        /// DTO para la respuesta de productos
        /// </summary>
        public class ProductoDTO
        {
            public int id { get; set; }
            public int categoria { get; set; }
            public string categoriaNombre { get; set; }

            public int idFamiliaProducto { get; set; }

            public string descripcion { get; set; }
            public string foto { get; set; }
            public string extension { get; set; }
            public string title { get; set; }
            public int?  price { get; set; } 
            public bool? destacado { get; set; }
        }
    

    /// <summary>
    /// DTO para crear una imagen de producto
    /// </summary>
    public class ImagenProductoDTO
        {
 

            [Required]
            public string imagenBase64 { get; set; }

            [Required]
            public string mimeType { get; set; }


        }
    }
}