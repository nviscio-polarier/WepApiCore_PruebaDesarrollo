using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblGestionRetiroController : ODataController
{
    private readonly bdERP db;
    public tblGestionRetiroController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> Get() //se pueden agregar filtros a la consulta
    {
        var today = DateTimeOffset.UtcNow.Date;
        var startOfYesterday = today.AddDays(-1);
        var endOfToday = today.AddDays(1).AddSeconds(-1);
        
        return Ok(db.tblGestionRetiro
            .OrderBy(x => x.isValidado)
            .ThenByDescending(x => x.fechaReg)  /*.Where(x => x.fechaReg >= startOfYesterday && x.fechaReg < endOfToday)*/);
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblGestionRetiro gestionRetiro)
    {
        int? idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        var objUsuario = db.tblUsuario.FirstOrDefault(x => x.idUsuario.Equals(idUsuario));
        if (objUsuario == null)
        {
            return BadRequest();
        }
        try
        {
            gestionRetiro.fechaReg = DateTimeOffset.UtcNow;
            //extract idUsuario from token
            gestionRetiro.idUsuario = idUsuario;
            db.tblGestionRetiro.Add(gestionRetiro);
            await db.SaveChangesAsync();

            return Created(gestionRetiro);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblGestionRetiro> gestionRetiro, [FromODataUri] bool validar)
    {
        tblGestionRetiro entity = db.tblGestionRetiro.Include(x => x.tblPrendaNGestionRetiro).FirstOrDefault(x => x.idGestionRetiro.Equals(key));
        if (entity == null || gestionRetiro == null)
            return BadRequest();

        db.tblPrendaNGestionRetiro.RemoveRange(entity.tblPrendaNGestionRetiro);
        var idUsuarioCreador = entity.idUsuario;
        gestionRetiro.ApplyTo(entity);
        entity.idUsuario = idUsuarioCreador;

        if (validar)
        {
            entity.isValidado = true;
            entity.fechaValidacion = DateTimeOffset.UtcNow;

            int? idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
            var objUsuario = db.tblUsuario.FirstOrDefault(x => x.idUsuario.Equals(idUsuario));
            if (objUsuario == null)
            {
                return BadRequest();
            }

            entity.idUsuarioValidador = idUsuario;

            CrearMovimiento(entity);

        }


        await db.SaveChangesAsync();

        return Ok(entity);
    }

    public async void CrearMovimiento(tblGestionRetiro entity)
    {
        int[] idTipoRetiros = GetIdTipoRetiros(entity);
        foreach(int idTipoRetiro in idTipoRetiros)
        {
            tblMovimiento movimiento = await GetMovimiento(entity, idTipoRetiro);

            List<tblPrendaNMovimiento> prendasRetiradas = new();
            foreach (var prenda in entity.tblPrendaNGestionRetiro)
            {
                if (prenda.idTipoRetiro != idTipoRetiro) continue;

                tblPrendaNMovimiento prendaNMovimiento = await SetPrendaNMovimiento(prenda, movimiento);
                prendasRetiradas.Add(prendaNMovimiento);
            }
            movimiento.tblPrendaNMovimiento = movimiento.tblPrendaNMovimiento.Concat(prendasRetiradas).ToList();
            if(isNuevoMovimiento(movimiento)) // Si el movimiento no existe, se agrega
            {
                db.tblMovimiento.Add(movimiento); //comprobar si el movimiento ya esta en las tracked entities
            }
        }
    }

    private async Task<tblMovimiento> GetMovimiento(tblGestionRetiro entity, int idTipoRetiro)
    {
        DateTime fechaGestionRetiro = entity.fechaReg.Value.Date;
        fechaGestionRetiro = fechaGestionRetiro.AddHours(12);

        var currentEntities = db.ChangeTracker.Entries<tblMovimiento>().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified).Select(e => e.Entity);

        tblMovimiento? movimiento = currentEntities
            .Where(x =>
                x.fecha == fechaGestionRetiro &&
                x.isApp == true &&
                x.idTipoRetiro == idTipoRetiro &&
                x.idCompañia == entity.idCompañia &&
                x.idEntidad == entity.idEntidad &&
                x.idGrupoPlantillaPrenda_generica == entity.idGrupoPlantillaPrenda_generica
                ).FirstOrDefault();

        if (movimiento == null)
        {
            movimiento = db.tblMovimiento
                .Where(x =>
                    x.fecha == fechaGestionRetiro &&
                    x.isApp == true &&
                    x.idTipoRetiro == idTipoRetiro &&
                    x.idCompañia == entity.idCompañia &&
                    x.idEntidad == entity.idEntidad &&
                    x.idGrupoPlantillaPrenda_generica == entity.idGrupoPlantillaPrenda_generica
                    )
                    .Include(x => x.tblPrendaNMovimiento).FirstOrDefault();
        }

        if (movimiento != null)
        {
            return movimiento;
        }

        tblMovimiento tblMovimiento = new()
        {
            idCompañia = entity.idCompañia,
            idEntidad = entity.idEntidad,
            idGrupoPlantillaPrenda_generica = entity.idGrupoPlantillaPrenda_generica,
            fecha = fechaGestionRetiro,
            idTipoMovimiento = 9, //Tipo de movimiento: Retiro
            tblPrendaNMovimiento = new List<tblPrendaNMovimiento>(),
            idTipoRetiro = idTipoRetiro,
            isApp = true,
        };

        return tblMovimiento;
    }

    private int[] GetIdTipoRetiros(tblGestionRetiro entity)
    {
        return entity.tblPrendaNGestionRetiro.Select(x => x.idTipoRetiro).Distinct().ToArray();
    }

    private async Task<tblPrendaNMovimiento> SetPrendaNMovimiento(tblPrendaNGestionRetiro prenda, tblMovimiento movimiento)
    {
        var prendaNMovimiento = movimiento.tblPrendaNMovimiento.FirstOrDefault(x => x.idPrenda == prenda.idPrenda || x.idPlantillaPrenda_generica == prenda.idPlantillaPrenda_generica);
        if (prendaNMovimiento != null)
        {
            prendaNMovimiento.cantidad += prenda.cantidad;
            return prendaNMovimiento;
        }
        if(prenda.idPrenda != null)
        {
            return new tblPrendaNMovimiento
            {
                idPrenda = prenda.idPrenda,
                idMovimiento = movimiento.idMovimiento,
                cantidad = prenda.cantidad,
            };
        } else if(prenda.idPlantillaPrenda_generica != null)
        {
            return new tblPrendaNMovimiento
            {
                idPlantillaPrenda_generica = prenda.idPlantillaPrenda_generica,
                idMovimiento = movimiento.idMovimiento,
                cantidad = prenda.cantidad,
            };
        }
        return null;
    }

    private bool isNuevoMovimiento(tblMovimiento movimiento)
    {
        if(movimiento.idMovimiento == 0) //si es 0, no existe en la base de datos
        {
            return true;
        }
        return false;
    }
}
