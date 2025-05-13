using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblLlamamientoController : ODataController
{
    private readonly bdERP db;

    public tblLlamamientoController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get([FromODataUri] bool todas = false)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        var objUsuario = db.tblUsuario.Include(x => x.tblTipoTrabajoNUsuario).Where(x => x.idUsuario == idUsuario && !x.isEliminado).FirstOrDefault();
        if (objUsuario == null)
            return BadRequest();

        var tblTipoTrabajoNUsuario = db.tblTipoTrabajoNUsuario.Where(x => x.idUsuario.Equals(idUsuario));

        var idsLlamamiento = db.tblLlamamiento.Where(llam => (!todas && (
            llam.idLavanderiaNavigation.idUsuario.Select(y => y.idUsuario).Contains(idUsuario) &&
            (
                objUsuario.enableDatosRRHH  ||
                (
                    tblTipoTrabajoNUsuario.Count() == 0 || (
                        tblTipoTrabajoNUsuario.Count() > 0 &&
                        tblTipoTrabajoNUsuario.FirstOrDefault(x => x.idLavanderia == llam.idLavanderia && x.idTipoTrabajo == llam.idTipoTrabajo) != null
                    )
                )
            )
        )) || todas).Select(x => x.idLlamamiento);

        return Ok(db.tblLlamamiento.Where(x => idsLlamamiento.Contains(x.idLlamamiento)));
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblLlamamiento llamamiento)
    {
        try
        {
            llamamiento.codigoLlamamiento = GetCodigoLlamamiento(db, llamamiento.idLavanderia, llamamiento.idCentroTrabajo);
            
            var objTipoContrato = db.tblTipoContrato.FirstOrDefault(x => x.idTipoContrato == llamamiento.idTipoContrato);
            llamamiento.numDiasPeriodoPrueba = objTipoContrato.numDiasPeriodoPrueba;

            db.tblLlamamiento.Add(llamamiento);
            await db.SaveChangesAsync();

            return Created(llamamiento);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblLlamamiento> llamamiento)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var entity = await db.tblLlamamiento
            .Include(x => x.tblDiasLibresPersonal_Llamamiento)
            .FirstOrDefaultAsync(x => x.idLlamamiento.Equals(key));

        if (entity == null)
        {
            return NotFound();
        }

        var operations_tblDiasLibresPersonal = llamamiento.Operations.FirstOrDefault(x => x.path.Equals("/tblDiasLibresPersonal_Llamamiento"));
        if (operations_tblDiasLibresPersonal != null)
        {
            db.tblDiasLibresPersonal_Llamamiento.RemoveRange(entity.tblDiasLibresPersonal_Llamamiento);
        }

        if (entity.idPersona != null)
        {
            var objPersona = db.tblPersona
                .Include(x => x.tblDatosSalariales)
                .Include(x => x.tblPersonaNTipoContrato)
                .FirstOrDefault(x => x.idPersona == entity.idPersona);

            var operations_idTurno = llamamiento.Operations.FirstOrDefault(x => x.path.Equals("/idTurno"));
            if (operations_idTurno != null)
            {
                int idTurno = Convert.ToInt32(operations_idTurno.value);
                objPersona.idTurno = idTurno;
            }
        }

        llamamiento.ApplyTo(entity);

        if (entity.idPersona != null)
        {
            var objPersona = db.tblPersona.Include(x => x.tblDiasLibresPersonal).First(x => x.idPersona == entity.idPersona);
            objPersona.idTurno = entity.idTurno;
            objPersona.idCategoriaInterna = entity.idCategoriaInterna;
            objPersona.idLavanderia = entity.idLavanderia;
            objPersona.idCentroTrabajo = entity.idCentroTrabajo;
            objPersona.idTipoTrabajo = entity.idTipoTrabajo;
            objPersona.idFormatoDiasLibres = entity.idFormatoDiasLibres;
            objPersona.tblDiasLibresPersonal.Clear();
            objPersona.tblDiasLibresPersonal =
                entity.tblDiasLibresPersonal_Llamamiento
                .Select(x => new tblDiasLibresPersonal { idDiaMes = x.idDiaMes, idDiaSemana = x.idDiaSemana, numDia = x.numDia })
                .ToList();

        }

        try
        {
            await db.SaveChangesAsync();
            return Updated(entity);
        }
        catch (DbUpdateConcurrencyException)
        {
            return NotFound();
        }
    }

    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<ActionResult> Delete([FromODataUri] int key)
    {
        var entity = await db.tblLlamamiento.Include(x => x.tblDiasLibresPersonal_Llamamiento).FirstOrDefaultAsync(x => x.idLlamamiento.Equals(key));

        if (entity == null)
        {
            return NotFound();
        }

        db.tblDiasLibresPersonal_Llamamiento.RemoveRange(entity.tblDiasLibresPersonal_Llamamiento);
        try
        {
            db.tblLlamamiento.Remove(entity);
            await db.SaveChangesAsync();

        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
        return NoContent();
    }

    static public byte GetCodigoLlamamiento(bdERP db, int? idLavanderia, int? idCentroTrabajo)
    {
        byte codigo = 1;
        var codigosAsignados = db.tblLlamamiento
            .Where(x => x.activo == true && (x.idLavanderia == idLavanderia || x.idCentroTrabajo == idCentroTrabajo))
            .Select(x => x.codigoLlamamiento).Distinct().ToList();

        while (codigosAsignados.Contains(codigo))
        {
            codigo += 1;
        }
        return codigo;
    }
}
