using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Enums.RRHH;
using WebApiCore.Security;

namespace WebApiCore.Controllers.Proyectos.MyPolarier.RRHH
{
    public class ValidacionNominaController : ODataController
    {
        private readonly bdERP db;

        public ValidacionNominaController(bdERP context)
        {
            db = context;
        }

        [HttpGet("odata/MyPolarier/RRHH/ValidacionNomina/idsPersonaGestionables")]
        [Authorize]
        public async Task<ActionResult> idsPersonaGestionables()
        {
            int idUsuario = int.Parse(HttpContext.Items["idUsuario"].ToString());

            var objUsuario = db.tblUsuario
                                .Include(x => x.idLavanderia)
                                .Include(x => x.idCentroTrabajo)
                                .FirstOrDefault(x => x.idUsuario == idUsuario);

            if (objUsuario == null) { return BadRequest(); }

            var lavanderias = objUsuario.idLavanderia.Select(x => x.idLavanderia);
            var centros = objUsuario.idCentroTrabajo.Select(x => x.idCentroTrabajo);

            var idsPersona = db.tblNomina.Where(x =>
                x.idEstadoNomina >= (byte)idsEstadoNomina.ValidadoGestoria && // Estado superior o igual a validación de gestoría
                x.idPersonaNavigation.idUsuario_validacion_nomina == idUsuario // Si el usuario es validador de la persona
           ).Select(x => x.idPersona);

            return Ok(idsPersona);
        }

        [HttpPost("odata/MyPolarier/RRHH/ValidacionNomina/DespacharNomina")]
        [Authorize]
        public async Task<ActionResult> GetDiasFijados([FromODataUri] bool isValidada, [FromBody] estadoNomina respuesta)
        {
            int idUsuario = int.Parse(HttpContext.Items["idUsuario"].ToString());

            var objUsuario = db.tblUsuario.FirstOrDefault(x => x.idUsuario == idUsuario);
            var objNomina = db.tblNomina.FirstOrDefault(x => x.idNomina == respuesta.idNomina);

            if (objUsuario == null || objNomina == null || objNomina.idEstadoNomina != (byte)idsEstadoNomina.ValidadoGestoria) { return BadRequest(); }

            objNomina.idEstadoNomina = (byte)(isValidada ? idsEstadoNomina.ValidadoEncargado : idsEstadoNomina.SolicitudCambioRRHH);

            var objEstadoNominaNNomina = new tblEstadoNominaNNomina
            {
                idNomina = objNomina.idNomina,
                idEstadoNomina = objNomina.idEstadoNomina,
                fecha = DateTimeOffset.UtcNow,
                idUsuario_valida = idUsuario,
                observaciones = respuesta.observaciones
            };

            db.tblEstadoNominaNNomina.Add(objEstadoNominaNNomina);

            await db.SaveChangesAsync();

            return Ok(true);
        }

    }
    public class estadoNomina
    {
        public int idNomina;
        public string? observaciones;
    }
}
