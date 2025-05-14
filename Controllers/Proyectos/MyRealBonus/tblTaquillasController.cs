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

        ///<summary>
        /// Se recoge la lista de vehiculos por idLavanderia = idLavanderia seleccionada 
        /// </summary>

        [EnableQuery]
        [HttpGet("odata/getVehiculoLavanderia")]
        public async Task<ActionResult> GetVehiculoLavanderia([FromODataUri] int idLavanderia)
        {
            try
            {
                var query = db.tblVehiculo
                .Where(p => p.eliminado == false
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

        [EnableQuery]
        [HttpGet("odata/getTaquillas")]
        public async Task<ActionResult> GetTaquillas()
        {
            try
            {
                var query = db.tblTaquillas_prueba
                    .Select(p => new TaquillaDTO 
                    {
                        idTaquilla = p.idTaquilla,
                        denominaconTaquilla = p.denominacion,
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















        //[EnableQuery]
        //[HttpGet("odata/getVehiculoLavanderia")]
        //public async Task<ActionResult> GetVehiculoLavanderia([FromODataUri] int idLavanderia)
        //{
        //    try
        //    {
        //        var query = db.tblVehiculoNLavanderia
        //            .Where(vl => vl.idLavanderia == idLavanderia && vl.tblVehiculo.eliminado == false)
        //            .Select(vl => new VehiculoDTO
        //            {
        //                idVehiculo = vl.tblVehiculo.idVehiculo,
        //                matricula = vl.tblVehiculo.matricula,
        //                denominacion = vl.tblVehiculo.denominacion
        //            });

        //        var result = await query.ToListAsync();
        //        return Ok(result);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, $"Error solicitud Vehiculos: {e.Message}");
        //    }
        //}

        /// <summary>
        /// DTO GET Lavanderia
        /// </summary>
        public class LavanderiaDTO
        {
            public int idLavanderia { get; set; }
            public string denominacion { get; set; }
        }

        /// <summary>
        /// DTO GET Info Persona
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
        /// DTO POST Registro 
        /// </summary>

        public class RegistroRecogidaDTO
        {
            public int idMovimientos { get; set; }
            public int idTaquilla { get; set; }
            public int idVehiculo { get; set; }
            public int idPersona { get; set; }
            public Date fechaRecogida { get; set; }
            public int posicion { get; set; }

        }

        public class TaquillaDTO 
        { 
            public int idTaquilla { get; set; }
            public string denominaconTaquilla {  get; set; }
            public int idLavanderia { get; set; }
            public string tamaño { get; set; }

        }
}
