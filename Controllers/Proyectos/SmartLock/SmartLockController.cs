using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers.Proyectos.Locker;
[BasicAuth]

public class SmartLockController : ODataController
{

    private readonly bdERP db;

    public SmartLockController(bdERP context)
    {
        db = context;
    }

    [EnableQuery]
    [HttpPost("odata/SmartLock/getVehiculosLocker")]
    public async Task<ActionResult> getVehiculosLocker([FromBody] List<int> idsTaquilla)
    {

        var idVehiculosTaquilla = db.tblTaquilla_estado.Where(x => idsTaquilla.Contains(x.idTaquilla)).Select(x => x.idVehiculo).ToList();

        var vehiculosTaquilla = db.tblVehiculo.Where(x => idVehiculosTaquilla.Contains(x.idVehiculo)).Select(x => new
        {
            x.idVehiculo,
            x.matricula,
            x.denominacion,
            x.idTipoVehiculo,
            x.codigoRFID
        }).ToList();

        var idPrimeraTaquilla = idsTaquilla[0];
        var idLavanderia = db.tblTaquilla.FirstOrDefault(x => x.idTaquilla == idPrimeraTaquilla).idLavanderia;

        var vehiculosLavanderia = db.tblVehiculo.Where(x => x.idLavanderia.Any(vnl => vnl.idLavanderia == idLavanderia) && !x.eliminado).Select(x => new
        {
            x.idVehiculo,
            x.matricula,
            x.denominacion,
            x.idTipoVehiculo,
            x.codigoRFID
        }).ToList();

        return Ok(new { vehiculosLavanderia, vehiculosTaquilla });
    }

    [EnableQuery]
    [HttpPost("odata/SmartLock/getPosicionTaquilla")]
    public async Task<ActionResult> getPosicionTaquilla([FromBody] List<int> idsTaquilla, [FromODataUri] int? idVehiculo)
    {
        int? posicion;
        int? idTaquilla;
        var taquilla_estado = db.tblTaquilla_estado;
        var estadoTaquilla = taquilla_estado.FirstOrDefault(x => idsTaquilla.Contains(x.idTaquilla) && x.idVehiculo == idVehiculo);

        if (estadoTaquilla == null)
            return Ok("FULL");

        posicion = estadoTaquilla?.numPosicion;
        idTaquilla = estadoTaquilla?.idTaquilla;

        return Ok(new { posicion, idTaquilla });
    }

    [EnableQuery]
    [HttpPost("odata/SmartLock/setDatosTaquilla")]
    public async Task<ActionResult> setDatosTaquilla([FromODataUri] int idTaquilla, [FromBody] List<RegTaquilla> registros)
    {
        try
        {
            var idLavanderia = db.tblTaquilla.FirstOrDefault(x => x.idTaquilla == idTaquilla).idLavanderia;

            DateTimeOffset offset = DateTimeOffset.UtcNow;
            int gmt = db.tblLavanderia
              .Where(x => x.idLavanderia.Equals(idLavanderia))
              .Select(x => (x.horarioVerano == true ? 1 : 0) + Convert.ToInt32(x.idZonaHorariaNavigation.GMT)).FirstOrDefault();

            offset = (DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(gmt)));

            //Actualizo datos de tblTaquilla_estado
            foreach (RegTaquilla reg in registros)
            {
                tblTaquilla_estado taquilla = db.tblTaquilla_estado.FirstOrDefault(x => x.idTaquilla == idTaquilla && x.numPosicion == reg.numPosicion);
                if (reg.isRecogida == true)
                {
                    taquilla.idVehiculo = null;
                }
                else
                {
                    taquilla.idVehiculo = reg.idVehiculo;
                }

            }

            //Inserto registro en tblTaquilla_movimiento
            foreach (RegTaquilla reg in registros)
            {
                tblTaquilla_movimiento movimiento = new()
                {
                    idTaquilla = (short)idTaquilla,
                    idVehiculo = reg.idVehiculo,
                    idPersona = (int)reg.idPersona,
                    isRecogida = (bool)reg.isRecogida,
                    numPosicion = (byte)reg.numPosicion,
                    fecha = offset
                };

                db.tblTaquilla_movimiento.Add(movimiento);
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }

        await db.SaveChangesAsync();
        return Ok(true);
    }

    [EnableQuery]
    [HttpGet("odata/SmartLock/tblPersona")]
    public ActionResult tblPersona([FromODataUri] int idLavanderia)
    {
        int idPais = db.tblLavanderia.Where(x => x.idLavanderia == idLavanderia).Select(x => x.idPais).FirstOrDefault();

        return Ok(db.tblPersona
                    .Where(x => x.idLavanderiaNavigation.idPais == idPais &&
                        x.idTipoTrabajo == 6 &&
                        x.activo && !x.eliminado)
                            .Select(y => new
                            {
                                y.idPersona,
                                y.nombre,
                                y.apellidos,
                                y.codigoRFID,
                                y.idCategoriaInterna
                            }));
    }

    public class RegTaquilla
    {
        public int? idVehiculo { get; set; }
        public int? numPosicion { get; set; }
        public int? idPersona { get; set; }
        public bool? isRecogida { get; set; }
    }

}
