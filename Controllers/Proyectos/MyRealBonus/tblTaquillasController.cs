using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Routing.Template;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using WebApiCore.Class;
using WebApiCore.Context;
using WebApiCore.Enums.Assistant;
using WebApiCore.Enums.RRHH;
using WebApiCore.Hubs;

namespace WebApiCore.Controllers.Proyectos.MyRealBonus
{
    public class tblTaquillasController : ODataController
    {
        private readonly bdERP db;

        public tblTaquillasController(bdERP context)
        {
            db = context;
        }

        ///<summary>
        /// HELPERS: Llamadas no explicitamente relacionadas con las taquillas
        /// </summary>
  
        ///<summary>
        /// Se recoge la lista de lavanderias
        /// </summary>

        [EnableQuery]
        [HttpGet("odata/getLavanderias")]
        public async Task<ActionResult> GetLavanderias()
        {
            try
            {
                var query = db.tblLavanderia
                    .Select(p => new LavanderiaDTO
                    {
                        idLavanderia = p.idLavanderia,
                        denominacion = p.denominacion
                    });

                var result = await query.ToListAsync();
                return Ok(result);

            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error solicitu Lavanderia: {e.Message}");
            }
        }

        ///<summary>
        /// Se recoge la lista de personas categoria = transporte y idLavanderia = idLavanderia seleccionada 
        /// </summary>

        [EnableQuery]
        [HttpGet("odata/getPersonasLavanderia")]
        public async Task<ActionResult> GetPersonasLavanderia([FromODataUri] int idLavanderia)
        {
            try
            {
                var query = db.tblPersona
                .Where(p => p.idLavanderia == idLavanderia
                            && p.activo == true
                            && p.eliminado == false
                            && p.idTipoTrabajo == 6
                )
                .Select(p => new PersonaDTO
                {
                    idPersona = p.idPersona,
                    nombre = p.nombre,
                    apellidos = p.apellidos,
                });

                var result = await query.ToListAsync();
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error solicitu Personas: {e.Message}");
            }
        }

        [EnableQuery]
        [HttpGet("odata/getVehiculoLavanderia")]
        public async Task<ActionResult> GetVehiculoLavanderia([FromODataUri] int idLavanderia)
        {
            try
            {
                var query = db.tblVehiculo
                .Where(p => p.idLavanderia.Any(i => i.idLavanderia == idLavanderia) && p.eliminado == false
                )
                .Select(p => new VehiculoDTO
                {
                    idVehiculo = p.idVehiculo,
                    matricula = p.matricula,
                    denominacion = p.denominacion,
                });

                var result = await query.ToListAsync();
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error solicitu Personas: {e.Message}");
            }
        }

        ///<summary>
        /// TAQUILLAS:
        /// </summary>

        ///<summary>
        /// Se recoge la lista de taquillas
        /// </summary>

        [EnableQuery]
        [HttpGet("odata/getTaquillas")]
        public async Task<ActionResult> GetTaquillas()
        {
            try
            {
                var query = db.tblTaquillas_prueba
                    .Select(p => new TaquillasDTO
                    {
                        idTaquilla = p.idTaquilla,
                        denominacion = p.denominacion,
                        idLavanderia = p.idLavanderia,
                        tamaño = p.tamaño
                    });

                var result = await query.ToListAsync();
                return Ok(result);

            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error solicitu Lavanderia: {e.Message}");
            }
        }

        ///<summary>
        /// Se recoge la lista de taquillas por lavanderia
        /// </summary>

        [EnableQuery]
        [HttpGet("odata/getTaquillasLavanderia")]
        public async Task<ActionResult> GetTaquillasLavanderia([FromODataUri] int idLavanderia)
        {
            try
            {
                var query = db.tblTaquillas_prueba
                .Where(p => p.idLavanderia == idLavanderia
                )
                .Select(p => new TaquillasLvanderiaDTO
                {
                    idTaquilla = p.idTaquilla,
                    denominacion = p.denominacion,
                    tamaño = p.tamaño,
                });

                var result = await query.ToListAsync();
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error solicitu Personas: {e.Message}");
            }
        }

        ///<summary>
        /// Se recoge la lista de registros
        /// </summary>

        [EnableQuery]
        [HttpGet("odata/getRegistros")]
        public async Task<ActionResult> GetRegistros()
        {
            try
            {
                var query = db.tblTaquillas_Estado_prueba
                    .Select(p => new EstadoDTO
                    {
                        idTaquilla = p.idTaquilla,
                        posicion = p.posicion,
                        disponible = p.disponible,

                    });

                var result = await query.ToListAsync();
                return Ok(result);

            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error solicitu Lavanderia: {e.Message}");
            }
        }

        ///<summary>
        /// Postea el estado de la posicion ( disponible = si/no )
        /// </summary>

        [HttpPost("odata/postEstadoTaquilla")]
        public async Task<ActionResult> PostEstadoTaquilla([FromBody] EstadoTaquillaDTO nuevoEstado)
        {
            try
            {
                var estado = new tblTaquillas_Estado_prueba
                {
                    idTaquilla = nuevoEstado.idTaquilla,
                    posicion = nuevoEstado.posicion,
                    disponible = nuevoEstado.disponible
                };

                db.tblTaquillas_Estado_prueba.Add(estado);
                await db.SaveChangesAsync();

                return Ok(new
                {
                    mensaje = "Registro creado correctamente",
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear el registro: {ex.Message}");
            }
        }

        ///<summary>
        /// Postea el log de movimiento 
        /// </summary>

        [HttpPost("odata/postMovimientoTaquilla")]
        public async Task<ActionResult> PostMovimientoTaquilla([FromBody] MovimientoTaquillaDTO nuevoMovimiento)
        {
            try
            {
                var movimiento = new tblTaquillas_Movimiento_prueba
                {
                    idTaquilla = nuevoMovimiento.idTaquilla,
                    idVehiculo = nuevoMovimiento.idVehiculo,
                    idPersona = nuevoMovimiento.idPersona,
                    fechaRecogida = nuevoMovimiento.fechaRecogida,
                    fechaDejar = nuevoMovimiento.fechaDejar,
                    posicion = nuevoMovimiento.posicion
                };

                db.tblTaquillas_Movimiento_prueba.Add(movimiento);
                await db.SaveChangesAsync();

                return Ok(new
                {
                    mensaje = "Movimiento creado correctamente",
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear el movimiento: {ex.Message}");
            }
        }
    }

        /// <summary>
        /// DTO para recibir solo id y denominacion
        /// </summary>
        public class LavanderiaDTO
        {
            public int idLavanderia { get; set; }
            public string denominacion { get; set; }
        }

        /// <summary>
        /// DTO para recibir los datos de persona
        /// </summary>

        public class PersonaDTO
        {
            public int idPersona { get; set; }
            public string nombre { get; set; }
            public string apellidos { get; set; }
        }


        /// <summary>
        /// DTO GET Info Vehiculo
        /// </summary>

        public class VehiculoDTO
        {
            public int idVehiculo { get; set; }
            public string matricula { get; set; }
            public string denominacion { get; set; }
        }

        /// <summary>
        /// DTO GET Info Taquillas
        /// </summary>

        public class TaquillasDTO
        {
            public int idTaquilla { get; set; }
            public string denominacion { get; set; }
            public int? idLavanderia { get; set; }
            public int tamaño { get; set; }
        }

        /// <summary>
        /// DTO GET Taquilla de lavanderia selecionada
        /// </summary>

        public class TaquillasLvanderiaDTO
        {
            public int idTaquilla { get; set; }
            public string denominacion { get; set; }
            public int tamaño { get; set; }
        }

        /// <summary>
        /// DTO GET estado taquilla + posicion 
        /// </summary>

        public class EstadoDTO
        {
            public int idTaquilla { get; set; }
            public int posicion { get; set; }
            public bool? disponible { get; set; }
        }

        /// <summary>
        /// DTO POST estado posicion taquilla
        /// </summary>

        public class EstadoTaquillaDTO
        {
            public int idTaquilla { get; set; }
            public int posicion { get; set; }
            public bool? disponible { get; set; }
        }

        /// <summary>
        /// DTO POST movimiento ( entrada salida )
        /// </summary>

        public class MovimientoTaquillaDTO
        {
            public int idTaquilla { get; set; }
            public int idVehiculo { get; set; }
            public int idPersona { get; set; }
            public DateTime? fechaRecogida { get; set; }
            public DateTime? fechaDejar { get; set; }
            public int? posicion { get; set; }
        }
}

