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

public class tblHistoricoAsientoNomina_MXController : ODataController
{
    private readonly bdERP db;

    public tblHistoricoAsientoNomina_MXController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public ActionResult Get([FromODataUri] DateTime fechaDesde, [FromODataUri] DateTime fechaHasta, [FromODataUri] TipoNominaMX? tipoNomina)
    {

        if (tipoNomina == null)
        {
            return BadRequest("Debe especificar la quincena.");
        }

        DateTime inicio;
        DateTime fin;
        if (TipoPagaMXUtils.isPagaQ1((TipoNominaMX)tipoNomina))
        {
            inicio = new Date(fechaDesde.Year, fechaDesde.Month, fechaDesde.Day);
            fin = new Date(fechaHasta.Year, fechaHasta.Month, 15);
        }
        else if (TipoPagaMXUtils.isPagaQ2((TipoNominaMX)tipoNomina))
        {
            inicio = new Date(fechaDesde.Year, fechaDesde.Month, 16);
            fin = new Date(fechaHasta.Year, fechaHasta.Month, fechaHasta.Day);
        }
        else
        {
            inicio = new Date(fechaDesde.Year, fechaDesde.Month, 1);
            fin = new Date(fechaHasta.Year, fechaHasta.Month, DateTime.DaysInMonth(fechaHasta.Year, fechaHasta.Month));
        }

        var tblHistoricoAsientoNomina_MX = db.tblHistoricoAsientoNomina_MX
            .Where(han => han.fechaDesde >= inicio && han.fechaHasta <= fin && han.idTipoNomina_MX == (short)tipoNomina);

        return Ok(tblHistoricoAsientoNomina_MX);
    }
}
