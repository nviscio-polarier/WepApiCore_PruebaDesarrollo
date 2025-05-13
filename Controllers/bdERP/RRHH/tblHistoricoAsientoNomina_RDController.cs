using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData.Edm;
using WebApiCore.Context;
using WebApiCore.Enums.General;
using WebApiCore.Enums.RRHH;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblHistoricoAsientoNomina_RDController : ODataController
{
    private readonly bdERP db;

    public tblHistoricoAsientoNomina_RDController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public ActionResult Get([FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta, [FromODataUri] TipoNominaRD? fortnight)
    {

        if (fortnight == null)
        {
            return BadRequest("Debe especificar la quincena.");
        }

        DateTime inicio;
        DateTime fin;
        if (fortnight == TipoNominaRD.NominaQ1)
        {
            inicio = new Date(fechaDesde.Year, fechaDesde.Month, fechaDesde.Day);
            fin = new Date(fechaHasta.Year, fechaHasta.Month, 15);
        }
        else if (fortnight == TipoNominaRD.NominaQ2)
        {
            inicio = new Date(fechaDesde.Year, fechaDesde.Month, 16);
            fin = new Date(fechaHasta.Year, fechaHasta.Month, fechaHasta.Day);
        }
        else
        {
            inicio = new Date(fechaDesde.Year, 1, 1);
            fin = new Date(fechaHasta.Year, 12, 31);
        }

        var tblHistoricoAsientoNomina_RD = db.tblHistoricoAsientoNomina_RD
            .Where(han => han.fechaDesde >= inicio && han.fechaHasta <= fin && han.idNomina_RDNavigation.idTipoNomina_RD == (short)fortnight);

        return Ok(tblHistoricoAsientoNomina_RD);
    }
}
