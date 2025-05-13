using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers.Proyectos.MyPolarier.RRHH
{
    public class RRHHController : ODataController
    {
        private readonly bdERP db;
        public RRHHController(bdERP context)
        {
            db = context;
        }

        [HttpGet("odata/MyPolarier/RRHH/GetDiasFijados")]
        [Authorize]
        public async Task<ActionResult> GetDiasFijados([FromODataUri] int idLavanderia, [FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta)
        {
            var results = db.tblCuadrantePersonal
                .Where(x => x.idLavanderia == idLavanderia && x.fecha >= fechaDesde && x.fecha <= fechaHasta && x.idCalendario_Estado != null)
                .Select(x => x.fecha).ToList()
                .GroupBy(x => x).Select(x => x.First()).ToList();

            return Ok(results);
        }

        [HttpGet("odata/MyPolarier/RRHH/GetAccesoLavanderias")]
        [Authorize]
        public async Task<ActionResult> GetAccesoLavanderias([FromODataUri] int idPais, [FromODataUri] int? idPersona)
        {
            int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

            tblPersona? persona = db.tblPersona.FirstOrDefault(x => x.idPersona == idPersona);
            tblUsuario? usuario = db.tblUsuario.Include(x => x.idLavanderia).FirstOrDefault(x => x.idPersona == idPersona);

            List<dynamic> tblLavanderias = new();

            tblLavanderias.AddRange(
                db.tblUsuario.Where(x => x.idUsuario.Equals(idUsuario))
                .Include(x => x.idLavanderia)
                .Select(x => x.idLavanderia).First()
                .Where(x => x.idPais == idPais)
                .Select(x => new { x.idLavanderia, x.denominacion, x.idPais, disabled = (x.idLavanderia == persona?.idLavanderia) })
            );

            if (idPersona != null)
            {
                tblLavanderias.AddRange(
               usuario.idLavanderia
               .Where(x => tblLavanderias.FirstOrDefault(y => x.idLavanderia == y.idLavanderia) == null)
               .Select(x => new { x.idLavanderia, x.denominacion, x.idPais, disabled = true }));
            }

            return Ok(tblLavanderias);
        }
    }
}
