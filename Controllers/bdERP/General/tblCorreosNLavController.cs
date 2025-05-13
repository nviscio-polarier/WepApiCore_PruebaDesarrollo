using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblCorreosNLavController : ODataController
{
    private readonly bdERP db;

    public tblCorreosNLavController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] int idLavanderia)
    {
        return Ok(db.tblCorreosNLav.Where(x => x.idLavanderia == idLavanderia));
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> OverrideMasivo(HttpRequestMessage bodyParam, [FromODataUri] int idLavanderia)
    {
        List<tblCorreosNLav> correosNLav = bodyParam.Content.ReadAsAsync<List<tblCorreosNLav>>().Result;
        List<tblCorreosNLav> correos = new List<tblCorreosNLav>();
        foreach (tblCorreosNLav correo in correosNLav)
        {
            List<tblTipoIncidencia> tiposInci = new List<tblTipoIncidencia>();
            List<tblReports> tiposReport = new List<tblReports>();
            foreach (tblTipoIncidencia incidencia in correo.idTipoIncidencia)
            {
                tiposInci.Add(db.tblTipoIncidencia.FirstOrDefault(e => e.idTipoIncidencia == incidencia.idTipoIncidencia));
            }
            correo.idTipoIncidencia = tiposInci;

            foreach (tblReports report in correo.idReport)
            {
                tiposReport.Add(db.tblReports.FirstOrDefault(e => e.idReport == report.idReport));
            }
            correo.idReport = tiposReport;

            correos.Add(new tblCorreosNLav()
            {
                idLavanderia = idLavanderia,
                denominacion = correo.denominacion,
                idTipoIncidencia = new List<tblTipoIncidencia>(tiposInci),
                idReport = new List<tblReports>(tiposReport)
            });
        }

        //Juntar correos con misma denominacion
        List<tblCorreosNLav> correosAgrupados = new List<tblCorreosNLav>();

        foreach (tblCorreosNLav correo in correos)
        {
            if (correosAgrupados.Exists(x => x.denominacion == correo.denominacion && x.idLavanderia == correo.idLavanderia))
            {
                tblCorreosNLav correo_original = correosAgrupados.Find(x => x.denominacion == correo.denominacion && x.idLavanderia == correo.idLavanderia);
                correo_original.idReport.Add(correo.idReport.FirstOrDefault());
                correo_original.idTipoIncidencia.Add(correo.idTipoIncidencia.FirstOrDefault());
            }
            else
            {
                correosAgrupados.Add(correo);
            }
        }

        db.tblCorreosNLav.RemoveRange(db.tblCorreosNLav.Where(x => x.idLavanderia == idLavanderia));

        db.tblCorreosNLav.AddRange(correosAgrupados.Where(x => x.idReport.Count > 0 || x.idTipoIncidencia.Count > 0));
        await db.SaveChangesAsync();
        return Ok(correos);
    }
}