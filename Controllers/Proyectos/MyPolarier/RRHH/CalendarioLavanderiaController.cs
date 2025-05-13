using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Enums.RRHH;
using WebApiCore.Security;
using static WebApiCore.Controllers.Proyectos.MyPolarier.RRHH.CalendarioController;

namespace WebApiCore.Controllers.Proyectos.MyPolarier.RRHH
{
    public class CalendarioLavanderiaController : ODataController
    {
        private readonly bdERP db;
        private CalendarioController cc;
        public CalendarioLavanderiaController(bdERP context)
        {
            db = context;
            cc = new(context);
        }

        [HttpGet("odata/RRHH/CalendarioLavanderia/GetPaises")]
        [Authorize]
        public ActionResult GetPaises()
        {
            int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

            var result = db.tblPais
                .Where(p => p.tblLavanderia.Any(l => l.idUsuario.Select(u => u.idUsuario).Contains(idUsuario)))
                .Select(p => new { p.idPais, p.denominacion });

            return Ok(result);
        }

        [EnableQuery]
        [HttpGet("odata/RRHH/CalendarioLavanderia/GetEventosNCalendarioLavanderia")]
        [Authorize]
        public ActionResult GetEventosNCalendarioLavanderia([FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta, [FromODataUri] string idsLavanderia)
        {
            string[] numberStrings = idsLavanderia.Split("|");
            List<int> lavanderias = Array.ConvertAll(numberStrings, int.Parse).ToList();

            var tblCalendario_Estado = db.tblCalendario_Estado.ToList();

            List<DateTime> dias = GenerarListaDias(fechaDesde, fechaHasta);

            var tblCalendarioLavanderia = db.tblCalendarioLavanderia
                .Where(cl => cl.idCalendario_Estado == (byte)idsCalendario_Estado.CierreNomina && cl.fecha >= fechaDesde && cl.fecha <= fechaHasta && lavanderias.Contains(cl.idLavanderia))
                .ToList();

            var diasBloqueados = cc.GetEstadoBloqueadoNCalendarioLavanderia(fechaDesde, fechaHasta, lavanderias);

            var tblLavanderia = db.tblLavanderia.Where(l => lavanderias.Contains(l.idLavanderia));

            var result = (
                    from d in dias

                    join cl in tblCalendarioLavanderia
                    on d.Date equals cl.fecha.Date into clGroup
                    from cl in clGroup.DefaultIfEmpty()

                    let idCalendario_Estado = cl?.idCalendario_Estado ?? cc.idCalendario_EstadoSinEvento

                    join ce in tblCalendario_Estado
                    on idCalendario_Estado equals ce.idCalendario_Estado into ceGroup
                    from ce in ceGroup.DefaultIfEmpty()

                    join db in diasBloqueados
                    on d.Date equals db.fecha.Date

                    let idLavanderia = cl?.idLavanderia ?? db?.idLavanderia

                    join l in tblLavanderia
                    on idLavanderia equals l.idLavanderia

                    where
                        db.isBloqueado
                        || cl?.idCalendario_Estado != null

                    select new
                    {
                        idCalendario_EstadoNavigation = ce ?? new() { idCalendario_Estado = idCalendario_Estado, traduccion = "sinPrevisionesDia" },
                        fecha = d,
                        idLavanderia,
                        idLavanderiaNavigation = l,
                        db.isBloqueado
                    }
                )
                .GroupBy(cl => new { cl.idCalendario_EstadoNavigation, cl.fecha })
                .Select(group => new
                {
                    group.Key.idCalendario_EstadoNavigation.idCalendario_Estado,
                    group.Key.idCalendario_EstadoNavigation.traduccion,
                    group.Key.idCalendario_EstadoNavigation.colorHexa,
                    group.Key.fecha,
                    isBloqueado = group.Any(cl => cl.isBloqueado),
                    tblLavanderia = group.Select(cl => new
                    {
                        cl?.idLavanderia,
                        cl?.idLavanderiaNavigation?.denominacion
                    }).Distinct()
                });

            return Ok(result);
        }

        [HttpPost("odata/RRHH/CalendarioLavanderia/AddEventosNCalendarioLavanderia")]
        [Authorize]
        public async Task<bool> AddEventosNCalendarioLavanderia([FromBody] List<tblCalendarioLavanderia> calendarioLavanderia)
        {
            db.tblCalendarioLavanderia.AddRange(calendarioLavanderia);
            await db.SaveChangesAsync();

            return true;
        }

        [HttpDelete("odata/RRHH/CalendarioLavanderia/DeleteEventosNCalendarioLavanderia")]
        [Authorize]
        public async Task<bool> DeleteEventosNCalendarioLavanderia([FromODataUri] int año, [FromODataUri] int mes, string idsLavanderia)
        {
            string[] numberStrings = idsLavanderia.Split("|");
            List<int> lavanderias = Array.ConvertAll(numberStrings, int.Parse).ToList();

            var entities = db.tblCalendarioLavanderia.Where(cl => cl.idCalendario_Estado == (byte)idsCalendario_Estado.CierreNomina && cl.fecha.Year == año && cl.fecha.Month == mes && lavanderias.Contains(cl.idLavanderia));

            db.tblCalendarioLavanderia.RemoveRange(entities);
            await db.SaveChangesAsync();

            return true;
        }
    }
}

