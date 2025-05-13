using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Class;
using WebApiCore.Context;
using WebApiCore.Enums.GestionInterna;
using WebApiCore.Hubs;
using WebApiCore.Security;
using static WebApiCore.Controllers.ControlHorarioController;

namespace WebApiCore.Controllers;
public class AppLogisticaInternaController : ODataController
{

    private readonly bdERP db;
    private readonly IHubContext<NotificacionesHub> _hubContext;
    private readonly int carrosReclamados_restriccionTemporal = 2; //variable que indica el tiempo en horas que se debe esperar para poder reclamar el carro
    public AppLogisticaInternaController(bdERP context, IHubContext<NotificacionesHub> hubContext)
    {
        db = context;
        _hubContext = hubContext;
    }

    [EnableQuery]
    [HttpGet("odata/AppLogisticaInterna/entidades")]
    [Authorize]
    public ActionResult entidades([FromODataUri] int idLavanderia, [FromODataUri] DateTime fecha)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        List<int> idsEntidad = Utils.selectEntidadesVisibles(db, idUsuario, idLavanderia);

        List<int> entidadesActivas = db.tblEntidad.Where(x => idsEntidad.Contains(x.idEntidad) &&
        (idLavanderia == null || (idLavanderia != null && x.idLavanderia.Count(l => l.idLavanderia.Equals(idLavanderia)) > 0)) &&
        (fecha == null || x.tblCalendarioEntidad.Where(c => c.fecha < fecha).OrderByDescending(c => c.fecha).First().idEstado.Equals(1)) //Apertura
        ).Select(x => x.idEntidad).ToList();

        List<int> idsPedido = db.tblPedido.Where(x => entidadesActivas.Contains(x.idEntidad) &&
        //  (x.idEstadoPedido == 1 || x.idEstadoPedido == 2) &&
          x.fecha.Date.Equals(fecha.Date))
          .Select(x => x.idPedido).ToList();

        var entidades = from ent in db.tblEntidad
                        where entidadesActivas.Contains(ent.idEntidad)
                        select new
                        {
                            ent.idEntidad,
                            ent.idCompañia,
                            ent.denominacion,
                            ent.idModeloImpresion_repartoNavigation.report
                        };

        var pedido = from pnp in db.tblPrendaNPedido
                     where idsPedido.Contains(pnp.idPedido)
                     group pnp by new { pnp.idPrenda, pnp.idPedidoNavigation.idEntidad } into g
                     select new
                     {
                         idEntidad = g.Key.idEntidad,
                         idPrenda = g.Key.idPrenda,
                         peticion = g.Sum(pnp => pnp.peticion)
                     };

        //var repartido = from pnr in db.tblPrendaNReparto
        //                where idsPedido.Contains((int)pnr.idRepartoNavigation.idPedido)
        //                group pnr by pnr.idPrenda into g
        //                select new
        //                {
        //                    idPrenda = g.Key,
        //                    repartido = g.Sum(pnr => pnr.cantidad)
        //                };


        var datosPrenda = from pnp in pedido
                          join pre in db.tblPrenda on pnp.idPrenda equals pre.idPrenda
                          join dp in db.tblDenoPrenda on pre.idDenoPrenda equals dp.idDenoPrenda
                          join tp in db.tblTipoPrenda on dp.idTipoPrenda equals tp.idTipoPrenda
                          join bc in db.tblBacsCarro on new { tp.idFamilia, idLavanderia } equals new { bc.idFamilia, bc.idLavanderia }
                          //join pnr in repartido on pnp.idPrenda equals pnr.idPrenda into rep_
                          //from pnr in rep_.DefaultIfEmpty()
                          select new
                          {
                              idEntidad = pnp.idEntidad,
                              cantidad = pnp.peticion,// - (pnr.repartido != null ? pnr.repartido : 0)),
                              peso = pre.peso / 1000f,
                              udsXBacReparto = pre.udsXBacReparto,
                              bacsCarro = bc.cantidad
                          };

        var query = from ent in entidades
                    join prendas in datosPrenda on ent.idEntidad equals prendas.idEntidad into pre_
                    from prendas in pre_.DefaultIfEmpty()
                    select new
                    {
                        idEntidad = ent.idEntidad,
                        idCompañia = ent.idCompañia,
                        denominacion = ent.denominacion,
                        peso = prendas.cantidad == null ? 0 : (prendas.cantidad * prendas.peso),
                        bacs = prendas.cantidad == null ? 0 : ((decimal)prendas.cantidad / prendas.udsXBacReparto),
                        carros = prendas.cantidad == null ? 0 : ((decimal)prendas.cantidad / prendas.udsXBacReparto / prendas.bacsCarro),
                        denominacionReport = ent.report
                    };

        var query_sum = from reg in query
                        group reg by new { reg.idEntidad, reg.idCompañia, reg.denominacion, reg.denominacionReport } into g
                        orderby g.Key.idCompañia, g.Key.denominacion
                        select new
                        {
                            idEntidad = g.Key.idEntidad,
                            idCompañia = g.Key.idCompañia,
                            denominacion = g.Key.denominacion,
                            peso = g.Sum(pre => pre.peso),
                            bacs = Math.Ceiling(g.Sum(pre => (decimal)pre.bacs)),
                            carros = Math.Ceiling(g.Sum(pre => (decimal)pre.carros)),
                            denominacionReport = g.Key.denominacionReport
                        };

        return Ok(query_sum);
    }

    [EnableQuery]
    [HttpGet("odata/AppLogisticaInterna/prendasPendientes")]
    [Authorize]
    public ActionResult prendasPendientes([FromODataUri] int idCompañia, [FromODataUri] DateTime fecha)
    {
        List<int> idsPedido = db.tblPedido.Where(x => x.idEntidadNavigation.idCompañia.Equals(idCompañia) &&
         x.fecha.Date.Equals(fecha.Date))
         .Select(x => x.idPedido).ToList();

        var pedido = from pnp in db.tblPrendaNPedido
                     where idsPedido.Contains(pnp.idPedido)
                     group pnp by pnp.idPrenda into g
                     select new
                     {
                         idPrenda = g.Key,
                         peticion = g.Sum(pnp => pnp.peticion)
                     };

        var query = from pre in db.tblPrenda
                    join pnp in pedido on pre.idPrenda equals pnp.idPrenda into ped_
                    from pnp in ped_.DefaultIfEmpty()

                    where pre.idCompañia == idCompañia || pnp.idPrenda != null
                    orderby pre.codigoPrenda
                    select new
                    {
                        idPrenda = pre.idPrenda,
                        codigoPrenda = pre.codigoPrenda,
                        denominacion = pre.denominacion,
                        udsXBacReparto = pre.udsXBacReparto,
                        colorTapa = pre.idColorTapaNavigation != null ? pre.idColorTapaNavigation.codigoHexadecimal : "",
                        idFamilia = pre.idDenoPrendaNavigation.idTipoPrendaNavigation.idFamilia,
                        peticion = pnp.peticion != null ? pnp.peticion : 0,
                        peso = pre.peso
                    };

        return Ok(query);
    }

    [EnableQuery]
    [HttpGet("odata/AppLogisticaInterna/resumenStockPrenda")]
    [Authorize]
    public ActionResult resumenStockPrenda([FromODataUri] int idLavanderia, [FromODataUri] int idPrenda, [FromODataUri] DateTime fecha, [FromODataUri] int? idRepartoActual = null)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        List<int> idsEntidad = Utils.selectEntidadesVisibles(db, idUsuario, idLavanderia);

        bool eliminado = false;
        bool? activo = true;
        var query = (from ped in db.tblPedido
                     where ped.fecha.Date.Equals(fecha.Date) && idsEntidad.Contains(ped.idEntidad)
                     join ent in db.tblEntidad on ped.idEntidad equals ent.idEntidad
                     join preNPed in db.tblPrendaNPedido on new { idPrenda, ped.idPedido } equals new { preNPed.idPrenda, preNPed.idPedido }
                     join rep in db.tblReparto on ped.idPedido equals rep.idPedido into rep_
                     from rep in rep_.DefaultIfEmpty()
                     join preNRep in db.tblPrendaNReparto on new { preNPed.idPrenda, rep.idReparto } equals new { preNRep.idPrenda, preNRep.idReparto } into preNRep_
                     from preNRep in preNRep_.DefaultIfEmpty()
                     join pre in db.tblPrenda on preNPed.idPrenda equals pre.idPrenda
                     join entNRut in db.tblEntidadNRutaExpedicion on ped.idEntidad equals entNRut.idEntidad
                     join rut in db.tblRutaExpedicion on new { idLavanderia, entNRut.idRutaExpedicion, activo, eliminado } equals new { rut.idLavanderia, rut.idRutaExpedicion, rut.activo, rut.eliminado }
                     join preNNuevoPed in db.tblPrendaNEntidad_NuevoPedido on new { idPrenda, ped.idEntidad } equals new { preNNuevoPed.idPrenda, preNNuevoPed.idEntidad } into preNNuevoPed_
                     from preNNuevoPed in preNNuevoPed_.DefaultIfEmpty()
                     group preNRep by new
                     {
                         ped.idPedido,
                         ped.codigo,
                         denoEntidad = ent.denominacion,
                         denoRuta = rut.denominacion,
                         preNNuevoPed.stockDefinido,
                         preNPed.peticion,
                         pre.udsXBacReparto
                     } into g
                     select new
                     {
                         g.Key.idPedido,
                         g.Key.denoEntidad,
                         g.Key.denoRuta,
                         peticionXBac = (g.Key.peticion - g.Sum(preNRep => ((idRepartoActual == null || idRepartoActual != preNRep.idReparto) ? preNRep.cantidad : 0))) / g.Key.udsXBacReparto,
                         stockDefinido = g.Key.stockDefinido / g.Key.udsXBacReparto
                     }).Where(x => x.peticionXBac > 0);

        return Ok(query);
    }

    [EnableQuery]
    [HttpGet("odata/AppLogisticaInterna/resumenEstibador")]
    [Authorize]
    public ActionResult resumenEstibador([FromODataUri] int idLavanderia)
    {
        var cantidadesNMovimiento = (from mov in db.tblMovimientoElemLog
                                     where mov.idEstadoMovimientoElemLog == 1 && mov.idLavanderia == idLavanderia && mov.fecha.Date.Equals(DateTime.Now.Date)
                                     join mezcla in db.tblMezclaSucioCliente on mov.idMovimientoElemLog equals mezcla.idMovimientoElemLog into mez_
                                     from mezcla in mez_.DefaultIfEmpty()
                                     select new
                                     {
                                         mov.fecha,
                                         mov.idMovimientoElemLog,
                                         mov.idEntidad,
                                         idMezclaSucioCliente = mezcla != null ? mezcla.idMezclaSucioCliente : (int?)null,
                                         fechaMezcla = mezcla.fecha,
                                         denoCompañia = mov.idEntidadNavigation.idCompañiaNavigation.denominacion,
                                         denoEntidad = mov.idEntidadNavigation.denominacion,
                                         porcentajeMezcla = mezcla != null ? Math.Round(((double)(mezcla.cantidad_errorMezcla + mezcla.cantidad_errorCordon) / mezcla.cantidadMuestra), 2) : (double?)null,
                                         isEditable = true,//!db.tblReparto.Any(x => x.idEntidad == mov.idEntidad && x.fecha.Value > mov.fecha.DateTime)
                                     });

        var result = (from mov in cantidadesNMovimiento
                      orderby (mov.porcentajeMezcla == null) descending,
                              (mov.porcentajeMezcla == null ? 0 : mov.porcentajeMezcla) >= 0.5 descending,
                              mov.fecha descending
                      select new
                      {
                          horaConteo = mov.fecha.ToString("HH:mm"),
                          horaCalidad = mov.fechaMezcla.ToString("HH:mm"),
                          mov.idMovimientoElemLog,
                          mov.idEntidad,
                          mov.idMezclaSucioCliente,
                          mov.denoCompañia,
                          mov.denoEntidad,
                          mov.porcentajeMezcla,
                          mov.isEditable,
                      });

        return Ok(result);
    }

    [EnableQuery]
    [HttpPost("odata/AppLogisticaInterna/IU_tblMovimientoElemLog")]
    [Authorize]
    public async Task<ActionResult> IU_tblMovimientoElemLog([FromODataUri] int? idMovimientoElemLog, [FromBody] tblMovimientoElemLog movimientoElemLog)
    {
        try
        {
            var response = await IU_tblMovimientoElemLog_general(idMovimientoElemLog, movimientoElemLog);

            return Ok(new { idMovimientoElemLog = response });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    public class RegCarroQR
    {
        public string? data { get; set; }
        public int? type { get; set; }
    }

    public class Wrapper_idLectura_idCarro
    {
        public int? idLecturaCarro { get; set; }
        public int? idCarro { get; set; }
        public string? codigo { get; set; }
        public string? nombreUsuario { get; set; }
        public string? entidad { get; set; }
        public bool? isReclamable { get; set; }
    }

    public class Reparto_carros_Wrapper
    {
        public string? observaciones { get; set; }
        public DateTime? fecha { get; set; }
        public int[]? idsLecturaCarros { get; set; }
    }

    [EnableQuery]
    [HttpPost("odata/AppLogisticaInterna/IdentificacionCarrosQR")]
    [Authorize]
    public async Task<ActionResult> IdentificacionCarrosQR([FromBody] List<RegCarroQR> payload, [FromODataUri] int? idLavanderia, [FromODataUri] int? idEntidad)
    {
        try
        {
            if(idLavanderia != null && idEntidad != null)
            {
                return BadRequest();
            }
            DateTimeOffset fecha = DateTime.Now;
            var estados = db.tblEstadoMovimientoElemLog.ToList();
            int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
            foreach (var qr in payload)
            {
                var idCarro = db.tblCarro.FirstOrDefault(x => x.codigo.Equals(qr.data))?.idCarro;
                if (idCarro == null)
                {
                    db.tblCarro.Add(new tblCarro
                    {
                        codigo = qr.data,
                    });
                    db.SaveChanges();
                    idCarro = db.tblCarro.FirstOrDefault(x => x.codigo.Equals(qr.data))?.idCarro;
                }
                db.tblLecturaCarro.Add(new tblLecturaCarro
                {
                    idCarro = idCarro,
                    fecha = fecha,
                    idEstadoMovimientoElemLog = estados.Where(x => x.idEstadoMovimientoElemLog == (int)qr.type).Select(x => x.idEstadoMovimientoElemLog).FirstOrDefault(),
                    idUsuario = idUsuario,
                    idLecturaCarro_Estado = 4,
                    idLavanderia = idLavanderia,
                    idEntidad = idEntidad
                });
            }
            db.SaveChanges();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [EnableQuery]
    [HttpGet("odata/AppLogisticaInterna/CheckPermisosUsuario")]
    [Authorize]
    public async Task<ActionResult> CheckPermisosUsuario()
    {
        /* Datos del usuario */
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        var usuario = db.tblUsuario
           .Where(x => x.idUsuario == idUsuario && !x.isEliminado)
           .Select(x => new tblUsuario { idPersona = x.idPersona, idCargo = x.idCargo, idLavanderia = x.idLavanderia })
           .FirstOrDefault();
        if (usuario == null)
        {
            return BadRequest();
        }
        bool isMasterDev = (short)idsCargo.Desarrollador == usuario.idCargo || (short)idsCargo.Master == usuario.idCargo;

        /* Comerciales */
        List<int?> comerciales = new List<int?>() { 63, 133 };

        /* Encargados */
        List<int?> categoriaInternaEncargado = new List<int?>() { 6, 7, 12, 77 };
        List<int?> categoriaEncargado = new List<int?>() { 1, 12 };
        List<int?> usuariosEncargados = new List<int?>() { 932, 1074 };
        var categorias = db.tblPersona
            .Where(x => x.idPersona == db.tblUsuario.FirstOrDefault(x => x.idUsuario == idUsuario).idPersona)
            .Select(x => new
            {
                x.idCategoria,
                x.idCategoriaInterna
            })
            .FirstOrDefault();
        bool isEncargado = categoriaEncargado.Contains(categorias.idCategoria) 
            || categoriaInternaEncargado.Contains(categorias.idCategoriaInterna) 
            || usuariosEncargados.Contains(idUsuario)
            || (short)idsCargo.Encargado == usuario.idCargo
            || false;

        /* Logísticos */
        List<int?> categoriaLogistico = new List<int?>() { 7 };
        List<int?> categoriaInternaLogistico = new List<int?>() { 9, 10, 56, 57 };
        bool isLogistico = categoriaLogistico.Contains(categorias.idCategoria) 
            || categoriaInternaLogistico.Contains(categorias.idCategoriaInterna) 
            || false;

        /* Asignacion de permisos */

        /* CODIGO COMENTADO TEMPORALMENTE HASTA QUE SE REQUIERA DE LOS PERMISOS */

        //bool identificacionCarros_cliente = false;
        //bool identificacionCarros_lavanderia = false;

        //if(isMasterDev || isEncargado || comerciales.Contains(idUsuario))
        //{
        //    identificacionCarros_cliente = true;
        //}

        //if(isMasterDev || isEncargado || isLogistico)
        //{
        //    identificacionCarros_lavanderia = true;
        //}

        /* CODIGO TEMPORAL QUE DA PERMISOS A TODOS LOS USUARIOS, BORRAR CUANDO SE REQUIERA */
        bool identificacionCarros_cliente = true;
        bool identificacionCarros_lavanderia = true;
        /* ----- Commit de referencia para revertir estos cambios (buscar por etiqueta): Permisos_IdentificacionCarros */

        dynamic wrapper = new
        {
            identificacionCarros_cliente,
            identificacionCarros_lavanderia
        };
        return Ok(wrapper);
    }

    [EnableQuery]
    [HttpPost("odata/AppLogisticaInterna/CheckCarros_sinMovimientoAsociado")]
    [Authorize]
    public async Task<ActionResult> CheckCarros_sinMovimientoAsociado([FromBody] int[] idsLecturaCarros)
    {
        DateTimeOffset fechaLimite = DateTime.Now.AddHours(-carrosReclamados_restriccionTemporal);
        var carrosPorComprobar = db.tblLecturaCarro
            .Where(x => idsLecturaCarros.Contains(x.idLecturaCarro) && x.idLecturaCarro_Estado == 1) //Solo cogemos las lecturas reservadas, las otras no necesitan comprobaciones
            .Select(x => new
            {
                x.idCarro,
                x.idEstadoMovimientoElemLog,
            })
            .ToList();

        var idsCarrosPorComprobar = carrosPorComprobar.Select(x => x.idCarro).ToList();
        var area = carrosPorComprobar.Select(x => x.idEstadoMovimientoElemLog).FirstOrDefault();

        var idsCarrosNlecturasVigentes = db.tblMovimientoElemLog
            .Include(x => x.tblCantidadNMovimientoElemLog)
            .Where(x => x.fecha >= fechaLimite && x.idEstadoMovimientoElemLog == area)
            .SelectMany(x => x.tblCantidadNMovimientoElemLog)
            .SelectMany(x => x.tblLecturaCarro.Where(y => y.idLecturaCarro_Estado == 5))
            .Select(x => x.idCarro)
            .ToList();
        var idsCarrosConMovimiento = idsCarrosPorComprobar.Intersect(idsCarrosNlecturasVigentes);
        var codigoCarrosConMovimiento = db.tblCarro.Where(x => idsCarrosConMovimiento.Contains(x.idCarro)).Select(x => x.codigo).ToList();
        return Ok(codigoCarrosConMovimiento);
    }

    [EnableQuery]
    [HttpPost("odata/AppLogisticaInterna/ReclamarLectura")]
    [Authorize]
    public async Task<ActionResult> ReclamarLectura([FromBody] int idLecturaCarro) //idLecturaCarro de la lectura que ya esta en BBDD, para asi clonar los registros de la lectura anterior
    {
        var lecturaCarro_BBDD = db.tblLecturaCarro.Include(x => x.idCarroNavigation).FirstOrDefault(x => x.idLecturaCarro == idLecturaCarro);
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        if (lecturaCarro_BBDD == null || idUsuario == null)
        {
            return BadRequest();
        }
        Wrapper_idLectura_idCarro wrapper_idLectura_idCarro = await RegisterNewLectura(lecturaCarro_BBDD.idCarro, new RegCarroQR {
            type = lecturaCarro_BBDD.idEstadoMovimientoElemLog,
            data = lecturaCarro_BBDD.idCarroNavigation.codigo
        },
        lecturaCarro_BBDD.idLavanderia, lecturaCarro_BBDD.idEntidad
        );
        return Ok(wrapper_idLectura_idCarro);
    }

    public bool shouldRegister(tblLecturaCarro? registro, int estado, int? idEntidad)
    {
        DateTimeOffset fecha = DateTime.Now;
        if (registro == null  //si no existe lectura previa
            || (registro.idEstadoMovimientoElemLog == 1 && estado == 2)  //si el carro esta en sucio y se intenta pasar a limpio
            || ((fecha - registro.fecha >= TimeSpan.FromHours(carrosReclamados_restriccionTemporal)) && //si han pasado mas horas que las indicadas por la restriccion temporal desde la última lectura
                ( idEntidad == null //no se ha especificado entidad
                || (registro.idEstadoMovimientoElemLog == 2 && estado == 1 && (registro.idEntidad == null || registro.idEntidad == idEntidad)) //si el carro esta en limpio y se intenta pasar a sucio y la entidad es la misma
                || registro.idEstadoMovimientoElemLog == estado //si el carro esta en el mismo estado que se intenta pasar (sucio -> sucio, limpio -> limpio)
                )
               )
            )
        {
            return true;
        }
        return false;
    }

    [EnableQuery]
    [HttpPost("odata/AppLogisticaInterna/TryAppendQRData")]
    [Authorize]
    public async Task<ActionResult> TryAppendQRData([FromBody] RegCarroQR payload, int? idLavanderia, int? idEntidad)
    {
        var idCarro = db.tblCarro.FirstOrDefault(x => x.codigo.Equals(payload.data))?.idCarro;
        if(idCarro == null)
        {
            Wrapper_idLectura_idCarro wrapper_idLectura_idCarro = await RegisterNewLectura(idCarro, payload, idLavanderia, idEntidad);
            return Ok(wrapper_idLectura_idCarro);
        }
        var ultimaEntrada = db.tblLecturaCarro
            .Where(x => x.idCarro == idCarro && x.idLecturaCarro_Estado != 2 && x.idLecturaCarro_Estado != 3 )
            .OrderByDescending(x => x.fecha)
            .FirstOrDefault();

        int estado = (int)payload.type;

        if (shouldRegister(ultimaEntrada, estado, idEntidad))
        {
            Wrapper_idLectura_idCarro wrapper_idLectura_idCarro = await RegisterNewLectura(idCarro, payload, idLavanderia, idEntidad);
            return Ok(wrapper_idLectura_idCarro);
        }
        else /* En esta clausula se puede devolver un objeto con la lectura, pero portando algun tipo de pega que impide el escaneo directo */
        {            
            /* Si la ultima entrada es una lectura reservada y la ha hecho el usuario actual, se le permite volver a escanear el carro y recuperar la misma lectura */
            var id_Lectura_reservada = db.tblLecturaCarro_Estado.FirstOrDefault(x => x.denominacion.Equals("Lectura_reservada"))?.idLecturaCarro_Estado;
            int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
            Wrapper_idLectura_idCarro wrapper_idLectura_idCarro = new()
            {
                idLecturaCarro = ultimaEntrada.idLecturaCarro,
                idCarro = idCarro,
                codigo = payload.data
            };
            if (ultimaEntrada.idLecturaCarro_Estado == id_Lectura_reservada && ultimaEntrada.idUsuario == idUsuario)
            {
                return Ok(wrapper_idLectura_idCarro);
            } else if (ultimaEntrada.idLecturaCarro_Estado == id_Lectura_reservada && ultimaEntrada.idUsuario != idUsuario) /* La reserva es de otro usuario, se comprueba el proceso */
            { 
                /* La nueva lectura se halla en sucio o proceso de estibacion */
                if (estado == 1)
                {
                    var ultimaEntradaAsociada = db.tblLecturaCarro
                    .Where(x => x.idCarro == idCarro && x.idLecturaCarro_Estado != 1 && x.idLecturaCarro_Estado != 2 && x.idLecturaCarro_Estado != 3)
                    .OrderByDescending(x => x.fecha)
                    .FirstOrDefault();
                    if(ultimaEntradaAsociada != null && ultimaEntradaAsociada.idEntidad != idEntidad) /* Si la entidad es distinta, el usuario no deberia poder robar la reserva */
                    {
                        wrapper_idLectura_idCarro.idLecturaCarro = ultimaEntradaAsociada.idLecturaCarro;
                        wrapper_idLectura_idCarro.entidad = db.tblEntidad.Where(x => x.idEntidad == ultimaEntradaAsociada.idEntidad).Select(x => x.denominacion).FirstOrDefault();
                        wrapper_idLectura_idCarro.isReclamable = false;
                        return Ok(wrapper_idLectura_idCarro);
                    }
                }
                /* La nueva lectura se halla en limpio o proceso de preparacion de pedidos */
                /* Y/O */
                /* La reserva se puede reclamar */
                wrapper_idLectura_idCarro.nombreUsuario = db.tblPersona.Where(x => x.idPersona == db.tblUsuario.Where(x => x.idUsuario == ultimaEntrada.idUsuario).Select(x => x.idPersona).FirstOrDefault())
                        .Select(x => x.nombre + " " + x.apellidos)
                        .FirstOrDefault();
                return Ok(wrapper_idLectura_idCarro);
            } else if(estado == 1 && ultimaEntrada.idEstadoMovimientoElemLog == 2 && idEntidad != ultimaEntrada.idEntidad)
            {
                wrapper_idLectura_idCarro.isReclamable = false;
                wrapper_idLectura_idCarro.entidad = db.tblEntidad.Where(x => x.idEntidad == ultimaEntrada.idEntidad).Select(x => x.denominacion).FirstOrDefault();
                return Ok(wrapper_idLectura_idCarro);
            }
        }
        return Ok(false);
    }

    [EnableQuery]
    [HttpPost("odata/AppLogisticaInterna/FlagCarrosAsDeleted")]
    [Authorize]
    public async Task<ActionResult> FlagCarrosAsDeleted([FromBody] int[] idsLecturaCarros)
    {
        var lecturasAEliminar = db.tblLecturaCarro.Where(x => idsLecturaCarros.Contains(x.idLecturaCarro)).ToList();
        foreach (var lectura in lecturasAEliminar)
        {
            lectura.idLecturaCarro_Estado = 2;
        }
        db.SaveChanges();
        return Ok();
    }

    [EnableQuery]
    [HttpPost("odata/AppLogisticaInterna/LecturaCarros_Undo")]
    [Authorize]
    public async Task<ActionResult> lecturaCarros_undo([FromBody] int[] idsLecturaCarro)
    {
        /* TABLA DE IDS Y FLUJO DEL UNDO
         * Lectura_reservada = 1 -> Sin_movimiento = 3
         * Lectura_eliminada = 2 -> Con_movimiento = 5 si idCantidadNMovimientoElemLog != null, sin_movimiento = 3 si idCantidadNMovimientoElemLog == null
         * Sin_movimiento = 3
         * Esperando_movimiento = 4
         * Con_movimiento = 5
         */
        var lecturas = db.tblLecturaCarro.Where(x => idsLecturaCarro.Contains(x.idLecturaCarro));
        var lecturas_reservadas = lecturas
            .Where(x => x.idLecturaCarro_Estado == 1).ToList();
        var lecturas_eliminadas = lecturas
            .Where(x => x.idLecturaCarro_Estado == 2).ToList();

        foreach (var lectura in lecturas_reservadas)
        {
            lectura.idLecturaCarro_Estado = 3;
        }
        foreach (var lectura in lecturas_eliminadas)
        {
            if (lectura.idCantidadNMovimientoElemLog != null)
            {
                lectura.idLecturaCarro_Estado = 5;
            }
            else
            {
                lectura.idLecturaCarro_Estado = 1;
            }
        }
        db.SaveChanges();
        return Ok();
    }

    public async Task<Wrapper_idLectura_idCarro> RegisterNewLectura(int? idCarro, RegCarroQR payload, int? idLavanderia, int? idEntidad)
    {
        if (idCarro == null)
        {
            var newCarro = new tblCarro
            {
                codigo = payload.data,
            };
            db.tblCarro.Add(newCarro);
            db.SaveChanges();
            idCarro = newCarro.idCarro;
        }
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        var newLecturaCarro = new tblLecturaCarro
        {
            idCarro = idCarro,
            fecha = DateTime.Now,
            idEstadoMovimientoElemLog = db.tblEstadoMovimientoElemLog.Where(x => x.idEstadoMovimientoElemLog == (int)payload.type).Select(x => x.idEstadoMovimientoElemLog).FirstOrDefault(),
            idTipoLectura = 1,
            idLecturaCarro_Estado = db.tblLecturaCarro_Estado.FirstOrDefault(x => x.denominacion.Equals("Lectura_reservada"))?.idLecturaCarro_Estado,
            idUsuario = idUsuario,
            idLavanderia = idLavanderia,
            idEntidad = idEntidad
        };
        db.tblLecturaCarro.Add(newLecturaCarro);
        db.SaveChanges();
        Wrapper_idLectura_idCarro wrapper_IdLectura_IdCarro = new()
        {
            idLecturaCarro = newLecturaCarro.idLecturaCarro,
            idCarro = newLecturaCarro.idCarro,
            codigo = payload.data
        };
        return (wrapper_IdLectura_IdCarro);
    }

    [EnableQuery]
    [HttpPost("odata/AppLogisticaInterna/IU_customPatchReparto")]
    [Authorize]
    public ActionResult IU_customPatchReparto([FromODataUri] int idReparto, [FromBody] Reparto_carros_Wrapper datosRepartoNuevo)
    {
        var reparto = db.tblReparto.FirstOrDefault(x => x.idReparto == idReparto);
        if (reparto == null)
        {
            return BadRequest();
        }
        reparto.observaciones = datosRepartoNuevo.observaciones;
        reparto.fecha = datosRepartoNuevo.fecha;
        reparto.numCarros = (byte)datosRepartoNuevo.idsLecturaCarros.Length;

        //obtener el id del movimiento al que va asociado el reparto
        var idMovimiento = db.tblMovimientoElemLog.FirstOrDefault(x => x.idMovimientoElemLog == reparto.idMovimientoElemLog)?.idMovimientoElemLog;
        //obtener la cantidad asociada al reparto
        var cantidad = db.tblCantidadNMovimientoElemLog.Where(x => x.idMovimientoElemLog == idMovimiento && x.idTipoElemLog == 5).FirstOrDefault(); //En los repartos, solo hay un registro de cantidad asociado a carros
        //obtener las lecturas asociadas al reparto
        var lecturas = db.tblLecturaCarro.Where(x => datosRepartoNuevo.idsLecturaCarros.Contains(x.idLecturaCarro)).ToList();
        var lecturasEliminadas = db.tblLecturaCarro
                                    .Where(x => 
                                        x.idCantidadNMovimientoElemLog == cantidad.idCantidadNMovimientoElemLog 
                                        && !datosRepartoNuevo.idsLecturaCarros.Contains(x.idLecturaCarro))
                                    .ToList();
        //Actualizamos las lecturas
        lecturasEliminadas.ForEach(x => x.idCantidadNMovimientoElemLog = null);
        lecturas.ForEach(x => x.idCantidadNMovimientoElemLog = cantidad.idCantidadNMovimientoElemLog);
        //Actualizamos la cantidad
        cantidad.cantidad = (short)datosRepartoNuevo.idsLecturaCarros.Length;
        db.SaveChanges();
        return Ok();
    }

    [EnableQuery]
    [HttpGet("odata/AppLogisticaInterna/getReparto")]
    [Authorize]
    public ActionResult getReparto([FromODataUri] int idPedido)
    {
        try
        {
            return Ok(getDatosReparto(idPedido));
        }
        catch
        {
            return BadRequest();
        }
    }

    [EnableQuery]
    [HttpGet("odata/AppLogisticaInterna/getDataPedido")]
    [Authorize]
    public ActionResult getDataPedido([FromODataUri] int? idPedido)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        if (idPedido == null)
        {
            return BadRequest();
        }

        //PedidosController controller = new PedidosController(db);
        //var idEntidadPedido = db.tblPedido.FirstOrDefault(x => x.idPedido == idPedido).idEntidad;
        dynamic result = getDataPedido_general((int)idPedido, (int)idUsuario);
        return Ok(result);
    }

    [EnableQuery]
    [HttpPost("odata/AppLogisticaInterna/setReparto")]
    [Authorize]
    public async Task<ActionResult> setReparto([FromBody] regSetReparto regReparto, [FromODataUri] int idLavanderia)
    {
        // Inicializa los controladores para manejar las operaciones de reparto y prendas en reparto.
        tblRepartoController controllerReparto = new tblRepartoController(db, _hubContext);
        tblPrendaNRepartoController controllerPrendaReparto = new tblPrendaNRepartoController(db, _hubContext);

        // Obtiene el ID del usuario actualmente autenticado.
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        // Verifica y asigna el ID del movimiento de elemento logístico, si está presente.
        var idMovimientoElemLog_reg = regReparto.tblReparto?.idMovimientoElemLog ?? 0;
        var idMovimientoElemLog = idMovimientoElemLog_reg != 0 ? (int?)idMovimientoElemLog_reg : null;

        // Prepara los datos del reparto para su procesamiento.
        var tblReparto_reg = regReparto?.tblReparto ?? null;
        var idPedidoNReparto_reg = tblReparto_reg?.idPedido;
        var idPedidoReparto = db.tblReparto.FirstOrDefault(x => x.idReparto == tblReparto_reg.idReparto)?.idPedido;
        var idPedido = idPedidoNReparto_reg ?? idPedidoReparto;

        // Verifica si existe un reparto disponible para la lavandería especificada.
        var postRepartoAvailable = db.tblReparto.FirstOrDefault(x => x.idPedido == idPedido && x.idLavanderia == idLavanderia && x.idRepartoEstado == 1) == null;

        // Prepara la lista de prendas para añadir al pedido.
        var prendaNReparto = regReparto.tblReparto?.tblPrendaNReparto?.Select(x => new { x.idPrenda, x.cantidad }).ToList();
        var prendaNPedido = db.tblPrendaNPedido.Where(x => x.idPedido == idPedido).Select(x => new { x.idPrenda }).ToList();

        // Filtra las prendas que no están en el pedido y las añade.
        List<tblPrendaNPedido> prendasAñadidas = prendaNReparto?.Where(pnr => !prendaNPedido.Any(pnp => pnp.idPrenda == pnr.idPrenda))
            .Select(pnr => new tblPrendaNPedido { idPedido = (int)idPedido, isAdded = true, idPrenda = pnr.idPrenda, peticion = pnr.cantidad }).ToList();

        try
        {
            // Si hay prendas para añadir, las inserta en la base de datos.
            if (prendasAñadidas != null && prendasAñadidas.Count > 0)
            {
                foreach (var prenda in prendasAñadidas)
                {
                    db.tblPrendaNPedido.Add(prenda);
                    await db.SaveChangesAsync();
                }
            }

            // Procesa los elementos logísticos asociados al reparto.
            if (regReparto.elemLogReparto != null)
            {
                idMovimientoElemLog = await IUD_tblMovimientoElemLog_sistemaCremallera(idMovimientoElemLog, regReparto.elemLogReparto, idPedido, idLavanderia);
            }

            // Crea un nuevo reparto si no existe uno en preparación para la lavandería.
            if (tblReparto_reg.idReparto == 0 && postRepartoAvailable)
            {
                if (idMovimientoElemLog != null)
                {
                    tblReparto_reg.idMovimientoElemLog = idMovimientoElemLog;
                }

                await controllerReparto.Post(tblReparto_reg, idUsuario);
            }
            // Actualiza un reparto existente.
            else
            {
                var tblRepartoDB = db.tblReparto.FirstOrDefault(x => x.idReparto == tblReparto_reg.idReparto);
                var tblPrendaNRepartoDB = db.tblPrendaNReparto.Where(x => x.idReparto == tblReparto_reg.idReparto);

                // Actualiza los campos del reparto existente.
                if (tblReparto_reg.idUsuarioPrepara != null)
                    tblRepartoDB.idUsuarioPrepara = tblReparto_reg.idUsuarioPrepara;

                if (tblReparto_reg.observaciones != null)
                    tblRepartoDB.observaciones = tblReparto_reg.observaciones;

                if (tblReparto_reg.numCarros != null)
                    tblRepartoDB.numCarros = tblReparto_reg.numCarros;

                if (tblReparto_reg.fecha != null)
                    tblRepartoDB.fecha = tblReparto_reg.fecha;

                if (idMovimientoElemLog != null)
                    tblRepartoDB.idMovimientoElemLog = idMovimientoElemLog;

                // Procesa las prendas asociadas al reparto, insertando, actualizando o eliminando según corresponda.
                var tblPrendaNReparto = tblReparto_reg?.tblPrendaNReparto;

                if (tblPrendaNReparto != null && tblPrendaNReparto.Count > 0)
                {
                    foreach (var prenda in tblReparto_reg.tblPrendaNReparto) // Primero se insertan y modifican las prendas.
                    {
                        var prendaDB = tblPrendaNRepartoDB.FirstOrDefault(x => x.idReparto == tblReparto_reg.idReparto && x.idPrenda == prenda.idPrenda);
                        if (prendaDB == null)
                        {
                            await controllerPrendaReparto.Post(prenda);
                        }
                        else if (prenda.cantidad > 0)
                        {
                            prendaDB.cantidad = prenda.cantidad;
                            await controllerPrendaReparto.Patch(prendaDB.idPrenda, prendaDB.idReparto,
                                new JsonPatchDocument<tblPrendaNReparto>().Replace(x => x.cantidad, prendaDB.cantidad), idUsuario);
                        }
                    }
                    foreach (var prenda in tblReparto_reg.tblPrendaNReparto) // Una vez modificadas las prendas, borra las prendas con cantidad 0.
                    {
                        var prendaDB = tblPrendaNRepartoDB.FirstOrDefault(x => x.idReparto == tblReparto_reg.idReparto && x.idPrenda == prenda.idPrenda);
                        if (prenda.cantidad == 0)
                        {
                            await controllerPrendaReparto.Delete(prendaDB.idPrenda, prendaDB.idReparto);
                        }
                    }
                }

                // Actualiza el reparto en la base de datos con los cambios realizados.
                var tblRepartoDB_patch = new JsonPatchDocument<tblReparto>();

                if(tblRepartoDB.idMovimientoElemLog != null)
                {
                    tblRepartoDB_patch.Replace(x => x.idMovimientoElemLog, tblRepartoDB.idMovimientoElemLog);
                }
                tblRepartoDB_patch.Replace(x => x.idUsuarioPrepara, tblRepartoDB.idUsuarioPrepara);
                tblRepartoDB_patch.Replace(x => x.observaciones, tblRepartoDB.observaciones);
                tblRepartoDB_patch.Replace(x => x.numCarros, tblRepartoDB.numCarros);
                tblRepartoDB_patch.Replace(x => x.fecha, tblRepartoDB.fecha);

                await controllerReparto.Patch(tblReparto_reg.idReparto, tblRepartoDB_patch, idUsuario);
            }

        }
        catch (Exception ex)
        {
            // En caso de error, retorna un BadRequest con la excepción.
            return BadRequest(ex);
        }

        // Retorna un OK con los datos del pedido actualizado.
        return Ok(getDataPedido_general((int)idPedido, (int)idUsuario));
    }

    [EnableQuery]
    [HttpGet("odata/AppLogisticaInterna/getGrupoPrendas")]
    [Authorize]
    public ActionResult getGrupoPrendas([FromODataUri] int idLavanderia)
    {

        var result = db.tblGrupoPrendaEst.Where(x => x.idLavanderia.Select(x => x.idLavanderia).Contains(idLavanderia)).Select(x => new
        {
            x.idGrupoPrendaEst,
            x.denominacion,
            x.idTipoElemLogNavigation.idTipoElemLog
        });

        return Ok(result);
    }

    #region Funciones generales
    private dynamic getDataPedido_general([FromODataUri] int idPedido, int idUsuario)
    {
        try
        {

            PedidosController controller = new PedidosController(db);
            var idEntidadPedido = db.tblPedido.FirstOrDefault(x => x.idPedido == idPedido).idEntidad;

            var getDatosRepartoResult = getDatosReparto_v2(idPedido);
            IEnumerable<dynamic> prendasNuevoPedido = controller.get_prendasNuevoPedido_v2(idEntidadPedido, idUsuario);

            var prendasRepartidas = db.tblPrendaNReparto.Where(x => x.idRepartoNavigation.idPedido == idPedido && x.idRepartoNavigation.idRepartoEstado != 1)
                .Select(x => new { x.idPrenda, x.cantidad });

            var prendaNPedido = db.tblPrendaNPedido
                    .Where(x => x.idPedidoNavigation.idEntidad == idEntidadPedido && x.idPedido == idPedido)
                    .OrderBy(x => x.idPrendaNavigation.codigoPrenda);

            dynamic idsPrendaNPedido = prendaNPedido.Select(x => x.idPrenda).ToList();
            prendasNuevoPedido = prendasNuevoPedido.Where(x => !idsPrendaNPedido.Contains(x.idPrenda));

            var resultPrendaNPedido = prendaNPedido
                    .Select(x => new
                    {
                        x.idPrendaNPedido,
                        x.idPrenda,
                        peticion = (x.peticion - prendasRepartidas.Where(y => x.idPrenda == y.idPrenda).Sum(j => j.cantidad)),
                        x.unidadesRepartidas,
                        x.isAdded,
                        x.idPrendaNavigation.denominacion,
                        x.idPrendaNavigation.codigoPrenda,
                        x.idPrendaNavigation.udsXBacReparto,
                        x.idPrendaNavigation.udsXBacPedido,
                        x.idPrendaNavigation.peso,
                        denoElemTransPedido = x.idPrendaNavigation.elementoPedidoNavigation.denominacion,
                        codigoElemTransPedido = x.idPrendaNavigation.elementoPedidoNavigation.codigo,
                        colorTapa = x.idPrendaNavigation.idColorTapaNavigation.codigoHexadecimal,
                        marca = x.idPrendaNavigation.idMarcaTapa,
                        stockDefinido = x.stockActual != null ? ((x.stockActual + x.peticion) / x.idPrendaNavigation.udsXBacPedido) :
                                                (x.idPrendaNavigation.tblPrendaNEntidad_NuevoPedido
                                                    .FirstOrDefault(x => x.idEntidad == idEntidadPedido).stockDefinido / x.idPrendaNavigation.udsXBacReparto)
                    }).Where(x => x.peticion > 0);



            var elemLogPedido = new
            {
                // Sacas en pedido no repartidas
                sacaSucio = ((db.tblElemLogNPedido.Where(x => x.idPedido == idPedido && x.idTipoElemLog == 4).Count()) -
                               (db.tblReparto
                                .Where(x => x.idPedido == idPedido && x.idRepartoEstado == 3 && x.idMovimientoElemLog != null)
                                .SelectMany(x => x.idMovimientoElemLogNavigation.tblCantidadNMovimientoElemLog
                                    .Where(x => x.idTipoElemLog == 4)
                                    .Select(x => x.cantidad))
                                .Sum(x => (int)x)))
                                > 0,

                sacaRechazo = ((db.tblElemLogNPedido.Where(x => x.idPedido == idPedido && x.idTipoElemLog == 6).Count()) -
                              (db.tblReparto
                                .Where(x => x.idPedido == idPedido && x.idRepartoEstado == 3 && x.idMovimientoElemLog != null)
                                .SelectMany(x => x.idMovimientoElemLogNavigation.tblCantidadNMovimientoElemLog
                                    .Where(x => x.idTipoElemLog == 6)
                                    .Select(x => x.cantidad))
                                .Sum(x => (int)x)))
                                > 0,
                cantidadSacasSucioPedido = db.tblElemLogNPedido.FirstOrDefault(x => x.idPedido == idPedido && x.idTipoElemLog == 4)?.stock,
                cantidadSacasRechazoPedido = db.tblElemLogNPedido.FirstOrDefault(x => x.idPedido == idPedido && x.idTipoElemLog == 6)?.stock
            };


            var elemLogReparto = new
            {
                //Sacas en reparto no validadas
                sacaSucio = (db.tblReparto
                                .Where(x => x.idPedido == idPedido && x.idRepartoEstado != 3 && x.idMovimientoElemLog != null)
                                .SelectMany(x => x.idMovimientoElemLogNavigation.tblCantidadNMovimientoElemLog
                                    .Where(x => x.idTipoElemLog == 4)
                                    .Select(x => x.cantidad))
                                .Sum(x => (int)x)) > 0,

                sacaRechazo = (db.tblReparto
                                .Where(x => x.idPedido == idPedido && x.idRepartoEstado != 3 && x.idMovimientoElemLog != null)
                                .SelectMany(x => x.idMovimientoElemLogNavigation.tblCantidadNMovimientoElemLog
                                    .Where(x => x.idTipoElemLog == 6)
                                    .Select(x => x.cantidad))
                                .Sum(x => (int)x)) > 0,

                //idMovimiento del ultimo reparto con estado distinto a 3
                //idMovimientoElemLog = db.tblReparto
                //                        .Where(x => x.idPedido == idPedido && x.idRepartoEstado != 3 && x.idMovimientoElemLog != null)
                //                            .OrderByDescending(x => x.idReparto)
                //                            .FirstOrDefault()?.idMovimientoElemLog
            };

            var lecturaCarros = db.tblReparto
                                .Where(x => x.idPedido == idPedido && x.idMovimientoElemLog != null)
                                .SelectMany(x => x.idMovimientoElemLogNavigation.tblCantidadNMovimientoElemLog
                                    .Where(x => x.idTipoElemLog == 5)
                                    .SelectMany(x => x.tblLecturaCarro))
                                .Include(x => x.idCarroNavigation)
                                .Select(x => new
                                {
                                    idLecturaCarro = x.idLecturaCarro,
                                    codigo = x.idCarroNavigation.codigo
                                })
                                .ToList();

            var result = new
            {
                tblReparto = getDatosRepartoResult.repartos,
                tblPrendaNEntidad_NuevoPedido = prendasNuevoPedido,
                tblPrendaNPedido = resultPrendaNPedido,
                elemLogPedido,
                elemLogReparto,
                lecturaCarros
            };

            return result;
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    private object totalElemLogPendienteRepartir([FromODataUri] int idEntidad, [FromODataUri] int? idMovimientoElemLog)
    {
        //Se enviará un idMovimientoElemLog cuando ya exista un reparto
        var queryCantNMov = from cantNMov in db.tblCantidadNMovimientoElemLog
                            where cantNMov.idMovimientoElemLog == idMovimientoElemLog && cantNMov.idTipoElemLog == 4
                            select cantNMov;

        var response = (from pend in db.tblSacasPendientes
                        join colorT in db.tblColorTapa on pend.idColorTapa equals colorT.idColorTapa into colorT_
                        from colorT in colorT_.DefaultIfEmpty()
                        join cantNMov in queryCantNMov on new { pend.idColorTapa } equals new { cantNMov.idColorTapa } into cantNMov_
                        from cantNMov in cantNMov_.DefaultIfEmpty()
                        where pend.idEntidad == idEntidad
                        select new
                        {
                            idColorTapa = pend.idColorTapa,
                            codigoHexadecimal = colorT != null ? colorT.codigoHexadecimal : null,
                            cantidadPendiente = pend.cantidad + (cantNMov != null ? cantNMov.cantidad : 0),
                            cantidadRepartida = cantNMov != null ? cantNMov.cantidad : (short?)null
                        }).Where(x => x.cantidadPendiente > 0);

        return response;
    }

    private dynamic getDatosReparto([FromODataUri] int? idPedido)
    {
        if (idPedido == null)
        {
            return null;
        }

        var idEntidad = db.tblPedido.Where(x => x.idPedido == idPedido).FirstOrDefault().idEntidad;
        IEnumerable<dynamic> repartos = (from reparto in db.tblReparto
                                         where reparto.idPedido == idPedido
                                         select new
                                         {
                                             reparto.idReparto,
                                             reparto.observaciones,
                                             reparto.numCarros,
                                             reparto.idUsuarioPrepara,
                                             reparto.idRepartoEstado,
                                             reparto.fecha,
                                             reparto.idEntidad,
                                             reparto.idMovimientoElemLog,
                                             reparto.idPedido,
                                             tblPrendaNReparto = from prenda in reparto.tblPrendaNReparto
                                                                 select new { prenda.idPrenda, prenda.cantidad, prenda.idReparto },
                                         });

        var repartoActual = repartos.Where(x => x.idRepartoEstado == 1).FirstOrDefault();

        if (repartoActual == null)
        {
            repartoActual = repartos.OrderBy(x => x.idReparto).LastOrDefault();
        }

        var result = new
        {
            repartos,
            totalElemLogPendienteRepartir = totalElemLogPendienteRepartir(idEntidad, repartoActual?.idMovimientoElemLog ?? null)
        };

        return result;
    }

    private dynamic getDatosReparto_v2([FromODataUri] int? idPedido)
    {
        if (idPedido == null)
        {
            return null;
        }

        var idEntidad = db.tblPedido.Where(x => x.idPedido == idPedido).FirstOrDefault().idEntidad;
        IEnumerable<dynamic> repartos = (from reparto in db.tblReparto
                                         where reparto.idPedido == idPedido
                                         select new
                                         {
                                             reparto.idReparto,
                                             reparto.observaciones,
                                             reparto.numCarros,
                                             reparto.idUsuarioPrepara,
                                             reparto.idRepartoEstado,
                                             reparto.fecha,
                                             reparto.idEntidad,
                                             reparto.idMovimientoElemLog,
                                             reparto.idPedido,
                                             tblPrendaNReparto = from prenda in reparto.tblPrendaNReparto
                                                                 select new { prenda.idPrenda, prenda.cantidad, prenda.idReparto },
                                         });

        var repartoActual = repartos.Where(x => x.idRepartoEstado == 1).FirstOrDefault();

        if (repartoActual == null)
        {
            //repartoActual = repartos.OrderBy(x => x.idReparto).LastOrDefault();
        }

        var result = new
        {
            repartos,
            totalElemLogPendienteRepartir = totalElemLogPendienteRepartir(idEntidad, repartoActual?.idMovimientoElemLog ?? null)
        };

        return result;
    }

    private async Task<int> IU_tblMovimientoElemLog_general([FromODataUri] int? idMovimientoElemLog, [FromBody] tblMovimientoElemLog movimientoElemLog)
    {
        var isEntrada = movimientoElemLog.idEstadoMovimientoElemLog == 1;

        //Insert tblSacasPendientes
        List<tblSacasPendientes> sacasPend = db.tblSacasPendientes.Where(x => x.idEntidad == movimientoElemLog.idEntidad).ToList();

        var sacasInsert = movimientoElemLog.tblCantidadNMovimientoElemLog
            .Where(x => x.idTipoElemLog.Equals(4) && sacasPend.Count(p => x.idColorTapa == p.idColorTapa) == 0)
            .Select(x => new tblSacasPendientes()
            {
                idEntidad = movimientoElemLog.idEntidad,
                idColorTapa = x.idColorTapa,
                cantidad = 0
            });

        if (sacasInsert.Count() > 0)
        {
            db.tblSacasPendientes.AddRange(sacasInsert);
        }

        await db.SaveChangesAsync();

        sacasPend = db.tblSacasPendientes.Where(x => x.idEntidad == movimientoElemLog.idEntidad).ToList();

        if (idMovimientoElemLog == null) //Insert
        {

            //Update tblSacasPendientes
            foreach (tblCantidadNMovimientoElemLog cantNMov_actual in movimientoElemLog.tblCantidadNMovimientoElemLog.Where(x => x.idTipoElemLog.Equals(4)))
            {
                sacasPend.Where(x => x.idColorTapa == cantNMov_actual.idColorTapa).FirstOrDefault().cantidad += isEntrada ? cantNMov_actual.cantidad : (short)(-1 * cantNMov_actual.cantidad);
            }

            //Update CantidadNMov
            foreach (tblCantidadNMovimientoElemLog cantNMov_actual in movimientoElemLog.tblCantidadNMovimientoElemLog.Where(x => x.idTipoElemLog.Equals(5)))
            {
                var idslecturasEnviadas = cantNMov_actual.tblLecturaCarro.Select(y => y.idLecturaCarro);
                var lecturasCarros_actualizar = db.tblLecturaCarro.Where(x => idslecturasEnviadas.Contains(x.idLecturaCarro));
                foreach (tblLecturaCarro lectura in lecturasCarros_actualizar)
                {
                    lectura.idLecturaCarro_Estado = 5;
                }
                cantNMov_actual.tblLecturaCarro = lecturasCarros_actualizar.ToList();
            }

            //Insert Movimiento
            movimientoElemLog.fecha = DateTimeOffset.UtcNow;
            db.tblMovimientoElemLog.Add(movimientoElemLog);
        }
        else //Update
        {
            var bdd_mov = db.tblMovimientoElemLog.Where(x => x.idMovimientoElemLog.Equals(idMovimientoElemLog));

            List<tblCantidadNMovimientoElemLog> old_cantidadNMov = bdd_mov.Select(x => x.tblCantidadNMovimientoElemLog).First().ToList();

            //Update tblSacasPendientes
            foreach (tblSacasPendientes saca in sacasPend)
            {
                var cantNMov_old = old_cantidadNMov.FirstOrDefault(c => c.idTipoElemLog.Equals(4) && c.idColorTapa.Equals(saca.idColorTapa));
                var cantNMov_actual = movimientoElemLog.tblCantidadNMovimientoElemLog.FirstOrDefault(c => c.idTipoElemLog.Equals(4) && c.idColorTapa.Equals(saca.idColorTapa));

                short cantAnt = cantNMov_old?.cantidad ?? 0;
                short cantActual = cantNMov_actual?.cantidad ?? 0;

                saca.cantidad += isEntrada ? (short)(cantActual - cantAnt) : (short)(-1 * (cantActual - cantAnt));
            }

            //Update CantidadNMov

            var cantidadesNMovimiento_old = db.tblCantidadNMovimientoElemLog.Where(x => x.idMovimientoElemLog.Equals(idMovimientoElemLog)).Include(x => x.tblLecturaCarro).ToList();
            db.tblCantidadNMovimientoElemLog.RemoveRange(old_cantidadNMov);
            foreach (tblCantidadNMovimientoElemLog cant in movimientoElemLog.tblCantidadNMovimientoElemLog)
            {
                tblCantidadNMovimientoElemLog newCantidad = cant;
                if (newCantidad.idTipoElemLog == 5)
                {
                    var idslecturasEnviadas = newCantidad.tblLecturaCarro.Select(y => y.idLecturaCarro);
                    var lecturasCarros_actualizar = db.tblLecturaCarro.Where(x => idslecturasEnviadas.Contains(x.idLecturaCarro));
                    foreach (tblLecturaCarro lectura in lecturasCarros_actualizar)
                    {
                        lectura.idLecturaCarro_Estado = 5;
                    }
                    newCantidad.tblLecturaCarro = lecturasCarros_actualizar.ToList();
                }
                //newCantidad.tblLecturaCarro = cantidadesNMovimiento_old.Where(x => x.idCarro == newCantidad.idCarro).Select(x => x.tblLecturaCarro).FirstOrDefault();
                bdd_mov.First().tblCantidadNMovimientoElemLog.Add(newCantidad);
            }
        }

        await db.SaveChangesAsync();

        return movimientoElemLog.idMovimientoElemLog;
    }


    private async Task<int?> IUD_tblMovimientoElemLog_sistemaCremallera(int? idMovimientoElemLog, elemLogReparto elementos, int? idPedido, int idLavanderia)
    {
        // Recupera el pedido y la entidad asociada desde la base de datos
        var pedido = db.tblPedido.FirstOrDefault(x => x.idPedido == idPedido);
        if (pedido == null) return null; // Si no se encuentra el pedido, retorna null

        var entidad = db.tblEntidad.FirstOrDefault(x => x.idEntidad == pedido.idEntidad);
        if (entidad == null) return null; // Si no se encuentra la entidad, retorna null
        if (idLavanderia == null) return null; // Si no se encuentra la lavandería, retorna null

        // Determina si se debe repartir sucio o rechazo y si hay carros asociados
        var reparteSucio = elementos.sacaSucio;
        var reparteRechazo = elementos.sacaRechazo;

        // Recupera el stock de elementos logísticos por entidad
        var stockElemLogEntidad = db.tblStockTipoElemLogNEntidad.Where(x => x.idEntidad == pedido.idEntidad).ToList();

        if (idMovimientoElemLog != null)
        {
            var movimientoElemLog = db.tblMovimientoElemLog.FirstOrDefault(x => x.idMovimientoElemLog == idMovimientoElemLog);
            if (movimientoElemLog == null) return null; // Si no se encuentra el movimiento, retorna null

            var cantidadNMovimientoElemLog = db.tblCantidadNMovimientoElemLog.Where(x => x.idMovimientoElemLog == idMovimientoElemLog).Include(x => x.tblLecturaCarro).ToList();

            if (reparteSucio || reparteRechazo || (elementos.idsLecturaCarros != null && elementos.idsLecturaCarros.Length > 0))
            {
                // Actualiza o elimina la cantidad de elementos logísticos según corresponda
                UID_Cantidad(cantidadNMovimientoElemLog, stockElemLogEntidad, reparteSucio, 4);
                UID_Cantidad(cantidadNMovimientoElemLog, stockElemLogEntidad, reparteRechazo, 6);

                if (elementos.idsLecturaCarros != null && elementos.idsLecturaCarros.Length > 0)
                {
                    var cantidadNMovimientoElemLog_carro = cantidadNMovimientoElemLog.Where(x => x.idTipoElemLog == 5).First();
                    var idsLecturasEnviadas = elementos.idsLecturaCarros;
                    var lecturasCarros_actualizar = db.tblLecturaCarro.Where(x => idsLecturasEnviadas.Contains(x.idLecturaCarro));
                    foreach (tblLecturaCarro lectura in lecturasCarros_actualizar)
                    {
                        lectura.idLecturaCarro_Estado = 5;
                    }
                    cantidadNMovimientoElemLog_carro.tblLecturaCarro = lecturasCarros_actualizar.ToList();
                    cantidadNMovimientoElemLog_carro.cantidad = (short)elementos.idsLecturaCarros.Length;
                }

                await db.SaveChangesAsync(); // Guarda los cambios en la base de datos

                return idMovimientoElemLog; // Retorna el ID del movimiento
            }
            else if(!(elementos.idsLecturaCarros != null && elementos.idsLecturaCarros.Length > 0))
            {
                // Si no se reparte sucio ni rechazo ni hay carros asociados, elimina el reparto y el movimiento
                var reparto = db.tblReparto.FirstOrDefault(x => x.idMovimientoElemLog == idMovimientoElemLog);
                if (reparto != null) reparto.idMovimientoElemLog = null;

                var cantMov = db.tblCantidadNMovimientoElemLog.Where(x => x.idMovimientoElemLog == idMovimientoElemLog);

                if (cantMov != null && cantMov.Count() > 0)
                {
                    db.tblCantidadNMovimientoElemLog.RemoveRange(cantMov);
                }

                db.tblMovimientoElemLog.Remove(movimientoElemLog);
                await db.SaveChangesAsync(); // Guarda los cambios en la base de datos
                return null; // Retorna null
            }
        }
        else if (reparteSucio || reparteRechazo || (elementos.idsLecturaCarros != null && elementos.idsLecturaCarros.Length > 0))
        {
            // Si se debe repartir sucio o rechazo y no existe un movimiento, crea uno nuevo
            var movimientoElemLog = new tblMovimientoElemLog
            {
                idEntidad = pedido.idEntidad,
                idLavanderia = (int)idLavanderia,
                idEstadoMovimientoElemLog = 2,
                fecha = DateTime.Now
            };

            db.tblMovimientoElemLog.Add(movimientoElemLog);
            await db.SaveChangesAsync(); // Guarda los cambios en la base de datos

            // Agrega la cantidad de elementos logísticos según corresponda
            InsertaCantidad(movimientoElemLog, stockElemLogEntidad, reparteSucio, 4);
            InsertaCantidad(movimientoElemLog, stockElemLogEntidad, reparteRechazo, 6);

            if(elementos.idsLecturaCarros != null && elementos.idsLecturaCarros.Length > 0)
            {
                var cantidadNElemLog = new tblCantidadNMovimientoElemLog
                {
                    idMovimientoElemLog = movimientoElemLog.idMovimientoElemLog,
                    idTipoElemLog = 5,
                    cantidad = (short)elementos.idsLecturaCarros.Length
                };
                var idsLecturasEnviadas = elementos.idsLecturaCarros;
                var lecturasCarros_actualizar = db.tblLecturaCarro.Where(x => idsLecturasEnviadas.Contains(x.idLecturaCarro));
                foreach (tblLecturaCarro lectura in lecturasCarros_actualizar)
                {
                    lectura.idLecturaCarro_Estado = 5;
                }
                cantidadNElemLog.tblLecturaCarro = lecturasCarros_actualizar.ToList();
                movimientoElemLog.tblCantidadNMovimientoElemLog.Add(cantidadNElemLog);
            }

            await db.SaveChangesAsync(); // Guarda los cambios en la base de datos
            return movimientoElemLog.idMovimientoElemLog; // Retorna el ID del nuevo movimiento
        }
        //else if(elementos.idsLecturaCarros != null && elementos.idsLecturaCarros.Length > 0)
        //{
        //    var movimientoElemLog = new tblMovimientoElemLog
        //    {
        //        idEntidad = pedido.idEntidad,
        //        idLavanderia = (int)idLavanderia,
        //        idEstadoMovimientoElemLog = 2,
        //        fecha = DateTime.Now
        //    };
        //    var idsLecturasEnviadas = elementos.idsLecturaCarros;
        //    var lecturasCarros_actualizar = db.tblLecturaCarro.Where(x => idsLecturasEnviadas.Contains(x.idLecturaCarro));
        //    foreach (tblLecturaCarro lectura in lecturasCarros_actualizar)
        //    {
        //        lectura.idLecturaCarro_Estado = 5;
        //    }
        //    movimientoElemLog.tblCantidadNMovimientoElemLog.FirstOrDefault(x => x.idTipoElemLog == 5).tblLecturaCarro = lecturasCarros_actualizar.ToList();
        //}

        return null; // Retorna null si no se cumplen las condiciones anteriores
    }

    // Método para actualizar o eliminar la cantidad de elementos logísticos
    private void UID_Cantidad(List<tblCantidadNMovimientoElemLog> cantidadNMovimientoElemLog, List<tblStockTipoElemLogNEntidad> stockElemLogEntidad, bool condition, int tipoElemLog)
    {
        var cantidad = cantidadNMovimientoElemLog.FirstOrDefault(x => x.idTipoElemLog == tipoElemLog);

        if (condition)
        {
            // Update or add cantidad based on condition
            if (cantidad == null)
            {
                // Add new cantidad if it does not exist
                var newCantidad = new tblCantidadNMovimientoElemLog
                {
                    idMovimientoElemLog = cantidadNMovimientoElemLog.FirstOrDefault().idMovimientoElemLog,
                    idTipoElemLog = (byte)tipoElemLog,
                    cantidad = (short?)stockElemLogEntidad.FirstOrDefault(x => x.idTipoElemLog == tipoElemLog)?.cantidad ?? 0
                };
                db.tblCantidadNMovimientoElemLog.Add(newCantidad);
            }
            else
            {
                // Update existing cantidad
                cantidad.cantidad = (short?)stockElemLogEntidad.FirstOrDefault(x => x.idTipoElemLog == tipoElemLog)?.cantidad ?? 0;
            }
        }
        else if (cantidad != null)
        {
            // Remove cantidad if condition is false and cantidad exists
            db.tblCantidadNMovimientoElemLog.Remove(cantidad);
        }
    }


    // Método para agregar la cantidad de elementos logísticos si es necesario
    private void InsertaCantidad(tblMovimientoElemLog movimientoElemLog, List<tblStockTipoElemLogNEntidad> stockElemLogEntidad, bool condition, int tipoElemLog)
    {
        if (condition)
        {
            // Crea y agrega una nueva cantidad de elementos logísticos si la condición es verdadera
            var cantidad = new tblCantidadNMovimientoElemLog
            {
                idMovimientoElemLog = movimientoElemLog.idMovimientoElemLog,
                idTipoElemLog = (byte)tipoElemLog,
                cantidad = (short?)stockElemLogEntidad.FirstOrDefault(x => x.idTipoElemLog == tipoElemLog)?.cantidad ?? 0
            };
            db.tblCantidadNMovimientoElemLog.Add(cantidad);
        }
    }



    #endregion

    public class elemLogReparto
    {
        public bool sacaSucio { get; set; }
        public bool sacaRechazo { get; set; }
        public int? idMovimientoElemLog { get; set; }
        public int[] idsLecturaCarros { get; set; }
    }

    public class regSetReparto
    {
        public elemLogReparto? elemLogReparto { get; set; }
        public tblReparto tblReparto { get; set; }
    }

}
