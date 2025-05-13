using WebApiCore.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Formatter;
using WebApiCore.Context;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using static WebApiCore.Controllers.Proyectos.MyPolarier.ControlPresupuestario.KgPresupuestadosController;

namespace WebApiCore.Controllers.Proyectos.MyPolarier.ControlPresupuestario
{
    public class KgPresupuestadosController : ODataController
    {
        private readonly bdERP db;

        public KgPresupuestadosController(bdERP context)
        {
            db = context;
        }

        [EnableQuery]
        [HttpGet("odata/MyPolarier/ControlPresupuestario/GetPresupuestos")]
        [Authorize]
        public ActionResult GetProyectos([FromODataUri] short idEmpresaPolarier, [FromODataUri] string idsElementoPEPJSON, [FromODataUri] short año)
        {
            if (idsElementoPEPJSON == null)
            {
                return BadRequest("No se han recibido elementos pep");
            }

            var idsElementoPEP = JsonConvert.DeserializeObject<List<int?>>(idsElementoPEPJSON);

            var tblAdmElementoPEP = db.tblAdmElementoPEP
                .Include(x => x.tblPresupuestoKg)
                .Where(x => idsElementoPEP.Contains(x.idAdmElementoPEPPadre) && x.idEmpresaPolarier == idEmpresaPolarier)
                .ToList();

            var tblPresupuestoKg = db.tblPresupuestoKg
                .Where(x => idsElementoPEP.Contains(x.idAdmElementoPEPNavigation.idAdmElementoPEPPadre) && x.año == año)
                .Select(x => new PresupuestoKg
                {
                    idPresupuestoKg = x.idPresupuestoKg,
                    idAdmElementoPEPPadre = x.idAdmElementoPEPNavigation.idAdmElementoPEPPadre,
                    idAdmElementoPEP = x.idAdmElementoPEP,
                    denominacion = x.idAdmElementoPEPNavigation.codigo + " - " + x.idAdmElementoPEPNavigation.denominacion,
                    año = x.año,
                    tblKgNPresupuestoKg = x.tblKgNPresupuestoKg.Select(kg => new KgNPresupuestoKg
                        {
                            idMes = kg.idMes,
                            valor = kg.valor
                        }).ToList(),
                    tblKgRealesNPresupuestoKg = x.tblKgRealesNPresupuestoKg.Select(kg => new KgNPresupuestoKg
                        {
                            idMes = kg.idMes,
                            valor = kg.valor
                        }).ToList()
                })
                .ToList();

            tblPresupuestoKg.AddRange(
                tblAdmElementoPEP
                    .Where(pep => !tblPresupuestoKg.Any(pkg => pkg.idAdmElementoPEP == pep.idAdmElementoPEP))
                    .Select(pep => new PresupuestoKg {
                        idAdmElementoPEPPadre = pep.idAdmElementoPEPPadre,
                        idAdmElementoPEP = pep.idAdmElementoPEP, 
                        denominacion =  pep.codigo + " - " + pep.denominacion,
                        año = año, 
                        tblKgNPresupuestoKg = new(),
                        tblKgRealesNPresupuestoKg = new()
                    })
            );

            foreach (var mes in db.tblMes.ToList())
            {
                foreach (var item in tblPresupuestoKg
                    .Where(x => !x.tblKgNPresupuestoKg.Any(kg => kg.idMes == mes.idMes)))
                {
                    item.tblKgNPresupuestoKg.Add(new KgNPresupuestoKg { idMes = mes.idMes, valor = 0 });
                }
                foreach (var item in tblPresupuestoKg
                    .Where(x => !x.tblKgRealesNPresupuestoKg.Any(kg => kg.idMes == mes.idMes)))
                {
                    item.tblKgRealesNPresupuestoKg.Add(new KgNPresupuestoKg { idMes = mes.idMes, valor = 0 });
                }
            }

            return Ok(tblPresupuestoKg);
        }

        [EnableQuery]
        [HttpPatch("odata/MyPolarier/ControlPresupuestario/PresupuestoKg({key})")]
        [Authorize]
        public ActionResult PresupuestoKgPatch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblPresupuestoKg> presupuestoKg)
        {
            var idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
            var objUsuario = db.tblUsuario.FirstOrDefault(x => x.idUsuario.Equals(idUsuario));
            if (objUsuario == null)
            {
                return BadRequest();
            }

            if (presupuestoKg == null) return BadRequest();

            tblPresupuestoKg entity = db.tblPresupuestoKg.FirstOrDefault(x => x.idPresupuestoKg == key);
            if (entity == null) return NotFound();

            var operations_tblKgNPresupuestoKg = presupuestoKg.Operations.Where(x => x.path.StartsWith("/tblKgNPresupuestoKg"));
            foreach (var operation in operations_tblKgNPresupuestoKg)
            {
                var idMes = byte.Parse(operation.path[(operation.path.LastIndexOf('-') + 1)..]);

                var kgNpresupuestoKg = db.tblKgNPresupuestoKg.FirstOrDefault(x => x.idPresupuestoKg == key && x.idMes == idMes);
                var newValue = int.Parse(operation.value.ToString());

                if (kgNpresupuestoKg != null)
                {
                    kgNpresupuestoKg.valor = newValue;
                } 
                else
                {
                    db.tblKgNPresupuestoKg.Add(new tblKgNPresupuestoKg
                    {
                        idPresupuestoKg = key,
                        idMes = idMes,
                        valor = newValue
                    });
                }

            }
            presupuestoKg.Operations.RemoveAll(x => x.path.StartsWith("/tblKgNPresupuestoKg"));


            // TODO: Eliminar cuando los PEP se vinculen a lavandería y se calcule atuomáticamente
            var operations_tblKgRealesNPresupuestoKg = presupuestoKg.Operations.Where(x => x.path.StartsWith("/tblKgRealesNPresupuestoKg"));
            foreach (var operation in operations_tblKgRealesNPresupuestoKg)
            {
                var idMes = byte.Parse(operation.path[(operation.path.LastIndexOf('-') + 1)..]);

                var kgNpresupuestoKg = db.tblKgRealesNPresupuestoKg.FirstOrDefault(x => x.idPresupuestoKg == key && x.idMes == idMes);
                var newValue = int.Parse(operation.value.ToString());

                if (kgNpresupuestoKg != null)
                {
                    kgNpresupuestoKg.valor = newValue;
                }
                else
                {
                    db.tblKgRealesNPresupuestoKg.Add(new tblKgRealesNPresupuestoKg
                    {
                        idPresupuestoKg = key,
                        idMes = idMes,
                        valor = newValue
                    });
                }
            }
            presupuestoKg.Operations.RemoveAll(x => x.path.StartsWith("/tblKgRealesNPresupuestoKg"));

            presupuestoKg.ApplyTo(entity);
            db.SaveChanges();

            return Updated(entity);
        }

        public class PresupuestoKg
        {
            public int? idPresupuestoKg { get; set; }
            public int? idAdmElementoPEPPadre { get; set; }
            public string denominacion { get; set; }
            public short año { get; set; }
            public int idAdmElementoPEP { get; set; }
            public List<KgNPresupuestoKg> tblKgNPresupuestoKg { get; set; }
            public List<KgNPresupuestoKg> tblKgRealesNPresupuestoKg { get; set; }
        }

        public class KgNPresupuestoKg
        {
            public int idMes { get; set; }
            public decimal valor { get; set; }
        }
    }
}
