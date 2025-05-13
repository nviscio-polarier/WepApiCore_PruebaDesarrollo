using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.SignalR;
using System.Data;
using WebApiCore.Context;
using WebApiCore.Hubs;

namespace WebApiCore.Controllers;
[AllowAnonymous]
public class SmartHUBController : ODataController
{
    private readonly bdERP db;

    private readonly IHubContext<NotificacionesHub> _hubContext;
    public SmartHUBController(bdERP context, IHubContext<NotificacionesHub> hubContext)
    {
        db = context;
        _hubContext = hubContext;
    }

    [EnableQuery]
    [HttpGet("odata/SmartHUB/tblPersona")]
    public ActionResult tblPersona([FromODataUri] int idMaquina)
    {
        int idPais = db.tblMaquina.Where(x => x.idMaquina == idMaquina).Select(x => x.idLavanderiaNavigation.idPais).FirstOrDefault();

        return Ok(db.tblPersona.Where(x =>
            (x.idLavanderiaNavigation.idPais == idPais) &&
            x.activo == true &&
            x.eliminado == false)
            .Select(y => new
            {
                idPersona = y.idPersona,
                nombre = y.nombre,
                apellidos = y.apellidos,
                codigoRFID = y.codigoRFID,
                idCategoria = y.idCategoria,
                idCategoriaInterna = y.idCategoriaInterna,
                numDocumentoIdentidad = y.numDocumentoIdentidad
            }));
    }

    [EnableQuery]
    [HttpGet("odata/SmartHUB/tblMaquina")]
    public async Task<ActionResult> tblMaquina([FromODataUri] int idMaquina)
    {
        return Ok(db.tblMaquina.Where(x => x.idMaquina.Equals(idMaquina)).Select(x => new
        {
            denominacion = x.denominacion,
            codigoTipoMaquina = x.idTipoMaquinaNCategoriaMaquinaNavigation.idTipoMaquinaNavigation.codigo
        }));
    }

    [EnableQuery]
    [HttpGet("odata/SmartHUB/tblEntidad")]
    public async Task<ActionResult> tblEntidad([FromODataUri] int idMaquina)
    {
        int idLavanderia = db.tblMaquina.Where(x => x.idMaquina == idMaquina).FirstOrDefault().idLavanderia;

        return Ok(db.tblEntidad.Where(x =>
            x.idLavanderia.Where(l => l.idLavanderia == idLavanderia).Count() > 0 &&
            x.activo == true &&
            x.eliminado == false)
            .Select(y => new
            {
                idEntidad = y.idEntidad,
                idCompañia = y.idCompañia,
                denominacion = y.denominacion
            }));
    }

    [EnableQuery]
    [HttpGet("odata/SmartHUB/tblCompañia")]
    public async Task<ActionResult> tblCompañia([FromODataUri] int idMaquina)
    {
        int idLavanderia = db.tblMaquina.Where(x => x.idMaquina == idMaquina).FirstOrDefault().idLavanderia;

        return Ok(db.tblCompañia.Where(x =>
            x.tblEntidad.Where(y => y.idLavanderia.Where(l => l.idLavanderia == idLavanderia).Count() > 0).Count() > 0 &&
            x.activo == true &&
            x.eliminado == false)
            .Select(y => new
            {
                idCompañia = y.idCompañia,
                denominacion = y.denominacion
            }));
    }

    [EnableQuery]
    [HttpGet("odata/SmartHUB/tblPrendasHora")]
    public async Task<ActionResult> tblPrendasHora(int idMaquina)
    {
        return Ok(db.tblPrendasHora.Where(x => x.idMaquina.Equals(idMaquina)).Select(x => new { x.idFamilia, x.idTipoPrenda, x.numVias, x.prendasHora }));
    }

    [EnableQuery]
    [HttpGet("odata/SmartHUB/tblFamilia")]
    public async Task<ActionResult> tblFamilia(int idMaquina)
    {
        return Ok(db.tblFamilia.Where(x => x.tblPrendasHora.Where(y => y.idMaquina.Equals(idMaquina)).Count() > 0).Select(x => new { x.idFamilia, x.denominacion }));
    }

    [EnableQuery]
    [HttpGet("odata/SmartHUB/tblTipoPrenda")]
    public async Task<ActionResult> tblTipoPrenda(int idMaquina)
    {
        return Ok(db.tblTipoPrenda.Where(x => x.tblPrendasHora.Where(y => y.idMaquina.Equals(idMaquina)).Count() > 0).Select(x => new { x.idTipoPrenda, x.idFamilia, x.denominacion }));
    }

    [EnableQuery]
    [HttpGet("odata/SmartHUB/tblPersonaNMaquina")]
    public async Task<ActionResult> tblPersonaNMaquina(int idMaquina)
    {
        return Ok(db.tblPersonaNMaquina.Where(x => x.idMaquina.Equals(idMaquina) && x.fechaFin == null).Select(x => new { x.idPersona }));
    }

    [EnableQuery]
    [HttpPost("odata/SmartHUB/tblClienteNMaquina_Inicio")]
    public async Task<ActionResult> tblClienteNMaquina_Inicio([FromBody] iuClienteNMaquina clienteNMaquina)
    {
        int idLavanderia = db.tblMaquina.Where(x => x.idMaquina == clienteNMaquina.idMaquina).FirstOrDefault().idLavanderia;

        #region Aplicar offset lavanderia a fecha actual

        var tblLavanderia = db.tblLavanderia
       .Where(x => x.idLavanderia.Equals(idLavanderia))
       .Select(x => new
       {
           x.idLavanderia,
           x.horarioVerano,
           x.idZonaHorariaNavigation
       }).FirstOrDefault();

        int gmt = (tblLavanderia.horarioVerano == true ? 1 : 0) + Convert.ToInt32(tblLavanderia.idZonaHorariaNavigation.GMT);

        DateTimeOffset offset = (DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(gmt)));
        #endregion

        //En caso de que haya otro cliente activo por la misma máquina se cierra
        List<tblClienteNMaquina> clientesActivos = db.tblClienteNMaquina.Where(x => x.idMaquina.Equals(clienteNMaquina.idMaquina) && x.fechaFin == null).ToList();
        foreach (tblClienteNMaquina cli in clientesActivos)
        {
            cli.fechaFin = offset;
        }

        db.tblClienteNMaquina.Add(new tblClienteNMaquina()
        {
            idCompañia = clienteNMaquina.idCompañia,
            idEntidad = clienteNMaquina.idEntidad,
            idMaquina = clienteNMaquina.idMaquina,
            idFamilia = clienteNMaquina.idFamilia,
            idTipoPrenda = clienteNMaquina.idTipoPrenda,
            fechaIni = offset
        });

        await db.SaveChangesAsync();
        List<string> srcs = new List<string> { "tblClienteNMaquina" };
        _hubContext.Clients.Group("SmartHUB_" + idLavanderia).SendAsync("SmartView/signalR_refresh", srcs);

        return Ok();
    }

    [EnableQuery]
    [HttpPost("odata/SmartHUB/tblClienteNMaquina_Fin")]
    public async Task<ActionResult> tblClienteNMaquina_Fin([FromBody] iuClienteNMaquina clienteNMaquina)
    {
        int idLavanderia = db.tblMaquina.Where(x => x.idMaquina == clienteNMaquina.idMaquina).FirstOrDefault().idLavanderia;

        #region Aplicar offset lavanderia a fecha actual

        var tblLavanderia = db.tblLavanderia
       .Where(x => x.idLavanderia.Equals(idLavanderia))
       .Select(x => new
       {
           x.idLavanderia,
           x.horarioVerano,
           x.idZonaHorariaNavigation
       }).FirstOrDefault();

        int gmt = (tblLavanderia.horarioVerano == true ? 1 : 0) + Convert.ToInt32(tblLavanderia.idZonaHorariaNavigation.GMT);

        DateTimeOffset offset = (DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(gmt)));
        #endregion

        tblClienteNMaquina objClienteNMaquina = db.tblClienteNMaquina.Where(x =>
                    x.idCompañia == clienteNMaquina.idCompañia &&
                    x.idEntidad == clienteNMaquina.idEntidad &&
                    x.idMaquina == clienteNMaquina.idMaquina &&
                    x.idFamilia == clienteNMaquina.idFamilia &&
                    x.idTipoPrenda == clienteNMaquina.idTipoPrenda &&
                    x.fechaFin == null
                ).OrderByDescending(s => s.idClienteNMaquina).FirstOrDefault();

        if (objClienteNMaquina != null)
        {
            objClienteNMaquina.fechaFin = offset;

            await db.SaveChangesAsync();
            List<string> srcs = new List<string> { "tblClienteNMaquina" };
            _hubContext.Clients.Group("SmartHUB_" + idLavanderia).SendAsync("SmartView/signalR_refresh", srcs);
        }
        return Ok();
    }

    [EnableQuery]
    [HttpPost("odata/SmartHUB/tblPersonaNMaquina_Inicio")]
    public async Task<ActionResult> tblPersonaNMaquina_Inicio([FromBody] iuPersonaNMaquina personaNMaquina)
    {
        int idLavanderia = db.tblMaquina.Where(x => x.idMaquina == personaNMaquina.idMaquina).FirstOrDefault().idLavanderia;

        #region Aplicar offset lavanderia a fecha actual

        var tblLavanderia = db.tblLavanderia
       .Where(x => x.idLavanderia.Equals(idLavanderia))
       .Select(x => new
       {
           x.idLavanderia,
           x.horarioVerano,
           x.idZonaHorariaNavigation
       }).FirstOrDefault();

        int gmt = (tblLavanderia.horarioVerano == true ? 1 : 0) + Convert.ToInt32(tblLavanderia.idZonaHorariaNavigation.GMT);

        DateTimeOffset offset = (DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(gmt)));
        #endregion

        //DESLOGUEO SMART HUB
        //En caso de que haya la misma persona activa en otra máquina se desactiva.
        //En caso de que otra persona esté en la misma máquina y posición se desactiva
        List<tblPersonaNMaquina> personaActiva = db.tblPersonaNMaquina.Where(x =>
       (x.idPersona.Equals(personaNMaquina.idPersona) || (x.idMaquina.Equals(personaNMaquina.idMaquina) && x.numPos.Equals(personaNMaquina.numPos))) &&
        x.fechaFin == null).ToList();

        foreach (tblPersonaNMaquina pnm in personaActiva)
        {
            pnm.fechaFin = offset;

            //Si la máquina de la persona desactivada NO tiene personas activas terminamos tblClienteNMaquina
            if (db.tblPersonaNMaquina.Count(x => x.idMaquina.Equals(pnm.idMaquina) && x.idPersona != pnm.idPersona && x.fechaFin == null) == 0)
            {
                List<tblClienteNMaquina> listCnm = db.tblClienteNMaquina.Where(x => x.idMaquina.Equals(pnm.idMaquina) && x.fechaFin == null).ToList();
                foreach (tblClienteNMaquina cnm in listCnm)
                {
                    cnm.fechaFin = offset;
                }
            }

            //DESLOGUEO SMART AREA
            var regActivos_ = db.tblPersonaNAreaNLavanderia.Where(x => x.idPersona.Equals(pnm.idPersona) && x.fechaFin == null);
            if (regActivos_.Count() > 0) //Finalizamos cualquier registro activo
            {
                foreach (tblPersonaNAreaNLavanderia pnanl in regActivos_)
                {
                    pnanl.fechaFin = offset;
                }
            }
        }

        db.tblPersonaNMaquina.Add(new tblPersonaNMaquina()
        {
            idPersona = personaNMaquina.idPersona,
            idMaquina = personaNMaquina.idMaquina,
            numPos = personaNMaquina.numPos,
            fechaIni = offset
        });

        //Finalizamos tblPersonaNAreaNLavanderia e iniciamos de nuevo en el área de Procesado
        var regActivos = db.tblPersonaNAreaNLavanderia.Where(x => x.idPersona.Equals(personaNMaquina.idPersona) && x.fechaFin == null);
        if (regActivos.Count() > 0) //Finalizamos cualquier registro activo
        {
            foreach (tblPersonaNAreaNLavanderia pnanl in regActivos)
            {
                pnanl.fechaFin = offset;
            }
        }

        db.tblPersonaNAreaNLavanderia.Add(new tblPersonaNAreaNLavanderia()
        {
            idAreaLavanderia = 3, //Procesado
            idLavanderia = tblLavanderia.idLavanderia,
            idPersona = personaNMaquina.idPersona,
            fechaIni = offset
        });

        await db.SaveChangesAsync();
        List<string> srcs = new List<string> { "PersonalActivo" };
        _hubContext.Clients.Group("SmartHUB_" + idLavanderia).SendAsync("SmartView/signalR_refresh", srcs);

        return Ok();
    }

    [EnableQuery]
    [HttpPost("odata/SmartHUB/tblPersonaNMaquina_Fin")]
    public async Task<ActionResult> tblPersonaNMaquina_Fin([FromBody] iuPersonaNMaquina personaNMaquina)
    {
        int idLavanderia = db.tblMaquina.Where(x => x.idMaquina == personaNMaquina.idMaquina).FirstOrDefault().idLavanderia;

        #region Aplicar offset lavanderia a fecha actual

        var tblLavanderia = db.tblLavanderia
       .Where(x => x.idLavanderia.Equals(idLavanderia))
       .Select(x => new
       {
           x.idLavanderia,
           x.horarioVerano,
           x.idZonaHorariaNavigation
       }).FirstOrDefault();

        int gmt = (tblLavanderia.horarioVerano == true ? 1 : 0) + Convert.ToInt32(tblLavanderia.idZonaHorariaNavigation.GMT);

        DateTimeOffset offset = (DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(gmt)));
        #endregion

        tblPersonaNMaquina objPersonaNMaquina = db.tblPersonaNMaquina.Where(x =>
                  x.idPersona == personaNMaquina.idPersona &&
                  x.idMaquina == personaNMaquina.idMaquina &&
                  x.fechaFin == null).OrderByDescending(s => s.idPersonaNMaquina).FirstOrDefault();

        if (objPersonaNMaquina != null)
        {
            objPersonaNMaquina.fechaFin = offset;

            //DESLOGUEO SMART AREA
            var regActivos = db.tblPersonaNAreaNLavanderia.Where(x => x.idPersona.Equals(objPersonaNMaquina.idPersona) && x.fechaFin == null);
            if (regActivos.Count() > 0) //Finalizamos cualquier registro activo
            {
                foreach (tblPersonaNAreaNLavanderia pnanl in regActivos)
                {
                    pnanl.fechaFin = offset;
                }
            }

            await db.SaveChangesAsync();
            List<string> srcs = new List<string> { "PersonalActivo" };
            _hubContext.Clients.Group("SmartHUB_" + idLavanderia).SendAsync("SmartView/signalR_refresh", srcs);
        }

        return Ok();
    }

    [EnableQuery]
    [HttpPost("odata/SmartHUB/tblPrendaNMaquina_Insert")]
    public async Task<ActionResult> tblPrendaNMaquina_Insert([FromBody] iPrendaNMaquina prendaNMaquina)
    {
        int idLavanderia = db.tblMaquina.Where(x => x.idMaquina == prendaNMaquina.idMaquina).FirstOrDefault().idLavanderia;

        #region Aplicar offset lavanderia a fecha actual

        var tblLavanderia = db.tblLavanderia
       .Where(x => x.idLavanderia.Equals(idLavanderia))
       .Select(x => new
       {
           x.idLavanderia,
           x.horarioVerano,
           x.idZonaHorariaNavigation
       }).FirstOrDefault();

        int gmt = (tblLavanderia.horarioVerano == true ? 1 : 0) + Convert.ToInt32(tblLavanderia.idZonaHorariaNavigation.GMT);

        DateTimeOffset offset = (DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(gmt)));
        #endregion

        db.tblPrendaNMaquina.Add(new tblPrendaNMaquina()
        {
            idMaquina = prendaNMaquina.idMaquina,
            idFamilia = prendaNMaquina.idFamilia,
            idTipoPrenda = prendaNMaquina.idTipoPrenda,
            numVia = prendaNMaquina.numVia,
            fecha = offset
        });


        await db.SaveChangesAsync();

        return Ok();
    }

    public class iuClienteNMaquina
    {
        public int? idCompañia { get; set; }
        public int? idEntidad { get; set; }
        public int idMaquina { get; set; }
        public byte idFamilia { get; set; }
        public short? idTipoPrenda { get; set; }
    }

    public class iuPersonaNMaquina
    {
        public int idPersona { get; set; }
        public int idMaquina { get; set; }
        public int? numPos { get; set; }
    }

    public class iPrendaNMaquina
    {
        public byte idFamilia { get; set; }
        public short? idTipoPrenda { get; set; }
        public int idMaquina { get; set; }
        public int numVia { get; set; }
    }
}
