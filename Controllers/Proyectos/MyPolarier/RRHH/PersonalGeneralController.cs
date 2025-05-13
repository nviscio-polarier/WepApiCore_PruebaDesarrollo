using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers.Proyectos.MyPolarier.RRHH;

[Route("odata/MyPolarier/RRHH/PersonalGeneral")]
public class PersonalGeneralController : ODataController
{
    private readonly bdERP db;

    public PersonalGeneralController(bdERP context)
    {
        db = context;
    }

    [HttpGet("GetDatosSalariales")]
    [Authorize]
    public async Task<ActionResult> GetDatosSalariales([FromODataUri] int idPersona)
    {
        int idUsuario = int.Parse(HttpContext.Items["idUsuario"].ToString());

        var fechaDesde = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var fechaHasta = DateTime.Today.AddMonths(1).AddDays(-DateTime.Today.Day);

        var connection = db.Database.GetDbConnection();
        var datosSalarialesNPersona = (await connection.QueryAsync("EXEC [MyReporting].[EF_gestionNominas_datosSalariales] @idUsuario, @fechaDesde, @fechaHasta, @idListPersona", new { idUsuario, fechaDesde, fechaHasta, idListPersona = idPersona.ToString() }))
            .ToList()
            .Select(ds => new
            {
                ds.idPersona,
                ds.nombre,
                ds.apellidos,
                ds.salarioBase,
                ds.plusAntiguedad,
                ds.plusAsistencia,
                ds.plusResponsabilidad,
                ds.plusPeligrosidad,
                ds.incentivo,
                ds.pagaExtra1,
                ds.pagaExtra2,
                ds.segAccidenteConvenio
            }).FirstOrDefault();

        return Ok(datosSalarialesNPersona);
    }
}
