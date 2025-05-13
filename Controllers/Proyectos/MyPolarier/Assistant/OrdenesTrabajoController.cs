using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;
public class OrdenesTrabajoController : ODataController
{
    private readonly bdERP db;

    private readonly Dictionary<string, int> idsCargo = new()
    {
        { "Desarrollador", 1 },
        { "Master", 2 }
    };

    public OrdenesTrabajoController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet("odata/MyPolarier/Assistant/OrdenesTrabajo/recambios")]
    [Authorize]
    public async Task<ActionResult> recambios([FromODataUri] int? idAlmacen, [FromODataUri] DateTimeOffset fecha, [FromODataUri] int? idParteTrabajo)
    {
        if (idAlmacen == null)
            return Ok(new List<Result>());

        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        tblUsuario user = db.tblUsuario
                            .Where(x => x.idUsuario == idUsuario && !x.isEliminado)
                            .Select(x => new tblUsuario { idPersona = x.idPersona, idCargo = x.idCargo, idLavanderia = x.idLavanderia }).FirstOrDefault();
        bool isMasterDev = idsCargo.ContainsValue(user.idCargo);

        var alms = db.tblAlmacenRecambiosNPersona.Where(x => x.idPersona == user.idPersona &&
         x.idAlmacenNavigation.idAlmacenPadre != null && //Solo hijos
         x.idAlmacenNavigation.activo == true && x.idAlmacenNavigation.eliminado == false)
             .Select(y => new { y.idAlmacen, y.idAlmacenNavigation.idAlmacenPadre }).ToList();

        var alms_padres = alms.Select(x => x.idAlmacenPadre).Distinct();
        var alms_hijos = alms.Select(x => x.idAlmacen);

        string idsAlmacen = string.Join('|', alms_hijos);

        tblParteTrabajoController ptc = new(db);

        var cantPrecioRecambio = await ptc.spGet_CantPrecio_ParteTrabajo(fecha, idAlmacen.ToString(), idParteTrabajo);

        List<Result> result = db.tblRecambio.Where(r => cantPrecioRecambio.Select(x => x.idRecambio).Contains(r.idRecambio))
            .Select(r => new Result
            {
                idRecambio = r.idRecambio,
                denominacion = r.denominacion,
                activo = r.activo,
                eliminado = r.eliminado,
                referencia = r.referencia,
                idProveedor = r.idProveedor,
                referenciaInterna = r.referenciaInterna,
                descripcionArticulo = r.descripcionArticulo,
                cantidad = 0
            }).ToList();

        foreach (var r in result)
        {
            var recambio = cantPrecioRecambio.Where(x => x.idRecambio == r.idRecambio).FirstOrDefault();

            r.precioMedioPonderado = recambio.precio;
            r.maxCantidad = recambio.max;
        }

        return Ok(result);
    }

    private class Result
    {
        public int idRecambio { get; set; }
        public string denominacion { get; set; }
        public bool? activo { get; set; }
        public bool eliminado { get; set; }
        public string referencia { get; set; }
        public short? idProveedor { get; set; }
        public string referenciaInterna { get; set; }
        public string? descripcionArticulo { get; set; }
        public int cantidad { get; set; }
        public decimal? precioMedioPonderado { get; set; }
        public int? maxCantidad { get; set; }
    }
}