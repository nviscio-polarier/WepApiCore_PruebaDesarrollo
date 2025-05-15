//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.OData.Formatter;
//using Microsoft.AspNetCore.OData.Query;
//using Microsoft.AspNetCore.OData.Routing.Controllers;
//using Microsoft.AspNetCore.SignalR;
//using Microsoft.EntityFrameworkCore;
//using System.ComponentModel.DataAnnotations;
//using System.Text.Json;
//using WebApiCore.Context;
//using WebApiCore.Enums.MyRealBonus;
//using WebApiCore.Hubs;
//using WebApiCore.Services.MyRealBonus;

//namespace WebApiCore.Controllers.Proyectos.MyRealBonus
//{




//    public class tblPersonaTokensController : ODataController
//    {
//        private readonly bdERP db;
//        private readonly CalculoTokenService _calculoTokens;

//        public tblPersonaTokensController(
//            bdERP context,
//            IHubContext<NotificacionesHub> hubContext,
//            CalculoTokenService calculoTokens)
//        {
//            db = context;
//            _calculoTokens = calculoTokens;
//        }


//        [HttpGet("odata/getResumenTokensPersona")]
//        public async Task<ActionResult> getResumenTokensPersona([FromODataUri] int idPersona)
//        {


//            var tokensGanados = await db.tblPersonaTokens
//                .Where(t => t.idPersona == idPersona && t.idTipoEventoToken == 1)
//                .SumAsync(t => t.tokens);

//            // Tokens pendientes: solicitudes en revisión y no aprobadas
//            var tokensPendientes = await db.tblPersonaTokens
//                .Where(t => t.idPersona == idPersona && t.idTipoEventoToken == 5)
//                .Where(t => !db.tblSolicitudesTokens
//                    .Any(s => s.idPersonaToken == t.idPersonaToken && s.idPersona != null))
//                .SumAsync(t => t.tokens);

//            // Tokens canjeados: tipo 2 (aprobados)
//            var tokensCanjeados = await db.tblPersonaTokens
//                .Where(t => t.idPersona == idPersona && t.idTipoEventoToken == 2)
//                .SumAsync(t => t.tokens);

//            var tokensReales = tokensGanados - tokensCanjeados;
//            var tokensDisponibles = tokensReales - tokensPendientes;



//            var resultado = new
//            {
//                IdPersona = idPersona,
//                TokensGanados = tokensGanados,
//                TokensPendientes = tokensPendientes,
//                TokensCanjeados = tokensCanjeados,
//                TokensDisponibles = tokensDisponibles,
//                TokensReales = tokensReales
//            };
//            return Ok(resultado);
//        }

//        /// <summary>
//        /// Obitene el total ganado, total canjeado y saldo actual de una persona en la fecha indicada
//        /// <summary>
//        /// 
//        [EnableQuery]
//        [HttpGet("odata/getTokensPersonaPorFecha")]
//        public async Task<ActionResult> getTokensPersonaPorFecha([FromODataUri] int idPersona, [FromODataUri] DateTime fechaIni, [FromODataUri] DateTime fechaFin)
//        {

//            try
//            {
//                var tokensGanados = await db.tblPersonaTokens
//                    .Where(t => t.idPersona == idPersona && t.idTipoEventoToken == 1 &&
//                           t.fecha.Date >= fechaIni &&
//                           t.fecha.Date <= fechaFin)
//                    .SumAsync(t => t.tokens);
//                var tokensPendientes = await db.tblPersonaTokens
//                    .Where(t => t.idPersona == idPersona && t.idTipoEventoToken == 5)
//                    .Where(t => !db.tblSolicitudesTokens
//                        .Any(s => s.idPersonaToken == t.idPersonaToken && s.idPersona != null))
//                    .SumAsync(t => t.tokens);

//                var tokensCanjeados = await db.tblPersonaTokens
//                    .Where(t => t.idPersona == idPersona && t.idTipoEventoToken == 2)
//                    .SumAsync(t => t.tokens);

//                var tokensReales = tokensGanados - tokensCanjeados;
//                var tokensDisponibles = tokensGanados - tokensPendientes;

//                var resultado = new
//                {
//                    IdPersona = idPersona,
//                    TokensGanados = tokensGanados,
//                    TokensPendientes = tokensPendientes,
//                    TokensCanjeados = tokensCanjeados,
//                    TokensDisponibles = tokensDisponibles,
//                    TokensReales = tokensReales

//                };

//                return Ok(resultado);
//            }
//            catch (Exception ex)
//            {


//                return BadRequest($"Error al obtener resumen de tokens: {ex.Message}");
//            }
//        }

//        /// <summary>
//        /// Obtiene los registros de tokens de una persona en un rango de fechas específico
//        /// </summary>
//        /// <param name="idPersona">ID de la persona</param>
//        /// <param name="fechaIni">Fecha de inicio del período</param>
//        /// <param name="fechaFin">Fecha de fin del período</param>
//        /// <returns>Lista de registros de tokens de la persona</returns>
//        [EnableQuery]
//        [HttpGet("odata/getTokensPersona")]
//        public async Task<ActionResult> getTokensPersona([FromODataUri] int idPersona, [FromODataUri] DateTime fechaIni, [FromODataUri] DateTime fechaFin)
//        {
//            try
//            {
//                var personaTokens = await db.tblPersonaTokens
//                    .Where(t => t.idPersona == idPersona &&
//                           t.fecha.Date >= fechaIni &&
//                           t.fecha.Date <= fechaFin &&
//                           t.idTipoEventoToken == 1)
//                    .Select(t => new
//                    {
//                        t.idPersona,
//                        t.fecha,
//                        t.idLavanderia,
//                        t.tokens,
//                        t.idTipoProducto,
//                        t.idTipoEventoToken
//                    })
//                    .ToListAsync();


//                return Ok(personaTokens);
//            }
//            catch (Exception ex)
//            {

//                return BadRequest($"Error al obtener tokens: {ex.Message}");
//            }
//        }

//        /// <summary>
//        /// Obitene un objeto con TODOS los tokens ganados por una persona en un rango de tiempo específico.
//        /// Útil para el datagrid.
//        /// <summary>
//        ///  
//        [HttpPost("odata/getTokensPorTurnos")]
//        public async Task<ActionResult> getTokensPorTurnos([FromBody] List<TurnoTokenRequest> turnos)
//        {


//            try
//            {
//                // Verificar que haya turnos
//                if (!turnos.Any())
//                {
//                    return BadRequest("No se proporcionaron turnos");
//                }

//                // Obtener el ID de persona del primer turno (asumimos que todos son de la misma persona)
//                int idPersona = turnos.First().idPersona;

//                // Obtener fechas límite de todos los turnos
//                DateTimeOffset minFecha = turnos.Min(t => t.fechaIni);
//                DateTimeOffset maxFecha = turnos.Max(t => t.fechaFin);

//                // Obtener todos los tokens relevantes en una sola consulta
//                var tokensData = await db.tblPersonaTokens
//                    .Where(t => t.idPersona == idPersona &&
//                           t.idTipoEventoToken == 1 &&
//                           t.fecha >= minFecha &&
//                           t.fecha <= maxFecha)
//                    .Select(t => new
//                    {
//                        Fecha = t.fecha,
//                        Tokens = t.tokens,
//                        IdLavanderia = t.idLavanderia
//                    })
//                    .ToListAsync();

//                // Procesar cada turno
//                var turnosConTokens = turnos.Select(turno =>
//                {
//                    // Calcular tokens para este turno con filtro opcional de lavandería
//                    decimal tokensEnTurno = tokensData
//                        .Where(t =>
//                        {
//                            // Filtrar por rango de fechas
//                            bool dentroRangoFecha = t.Fecha >= turno.fechaIni && t.Fecha <= turno.fechaFin;


//                            return dentroRangoFecha;
//                        })
//                        .Sum(t => t.Tokens);

//                    // Crear un nuevo objeto con los tokens
//                    return new TurnoConTokensDto
//                    {
//                        IdPersona = turno.idPersona,
//                        fechaIni = turno.fechaIni,
//                        fechaFin = turno.fechaFin,
//                        Tokens = tokensEnTurno
//                    };
//                }).ToList();

//                return Ok(turnosConTokens);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error: {ex.Message}");
//            }
//        }



//        [EnableQuery]
//        [HttpGet("odata/getLogsToken")]
//        public IQueryable<tblLogsToken> GetLogsToken()
//        {
//            return db.tblLogsToken;
//        }



//        /// <summary>
//        /// Obtiene los registros de tokens agrupados por tipo de evento entre dos fechas.
//        /// 1	Ingreso
//        /// 2	Canje
//        //  3	Devolución
//        //  4	Eliminado manualmente
//        //  5	En revisión
//        //  6	Umbral No Alcanzado
//        /// </summary>
//        [EnableQuery]
//        [HttpGet("odata/getTokensPorTipoEvento")]
//        public async Task<ActionResult> getTokensPorTipoEvento(
//            [FromODataUri] int idPersona,
//            [FromODataUri] DateTime fechaIni,
//            [FromODataUri] DateTime fechaFin,
//            [FromODataUri] int? idEvento = null)
//        {
//            try
//            {
//                var query = db.tblPersonaTokens
//                    .Where(t => t.idPersona == idPersona &&
//                           t.fecha >= fechaIni &&
//                           t.fecha <= fechaFin);

//                if (idEvento.HasValue)
//                {
//                    query = query.Where(t => t.idTipoEventoToken == idEvento.Value);
//                }

//                var tokensPorEvento = await query
//                    .GroupBy(t => t.idTipoEventoToken)
//                    .Select(g => new
//                    {
//                        IdTipoEvento = g.Key,
//                        TotalTokens = g.Sum(t => t.tokens),
//                        CantidadRegistros = g.Count()
//                    })
//                    .ToListAsync();

//                return Ok(tokensPorEvento);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error al obtener tokens por tipo de evento: {ex.Message}");
//            }
//        }
//        /// <summary>
//        /// Solicitud de canje de tokens
//        /// </summary>
//        [HttpPost("odata/solicitarCanje")]
//        public async Task<ActionResult> solicitarCanje(
//            [FromODataUri] int idPersona,
//            [FromODataUri] int tokensASolicitar,
//            [FromODataUri] int? idTipoProducto = null)
//        {
//            try
//            {
//                // Verificar si hay suficientes tokens disponibles (en principio Nico lo pone en la UI también, pero por si las moscas)
//                var ingresos = await db.tblPersonaTokens
//                    .Where(t => t.idPersona == idPersona && t.idTipoEventoToken == 1)
//                    .SumAsync(t => t.tokens);

//                var canjeados = await db.tblPersonaTokens
//                    .Where(t => t.idPersona == idPersona && t.idTipoEventoToken == 2)
//                    .SumAsync(t => t.tokens);

//                var tokensDisponibles = ingresos - canjeados;

//                if (tokensDisponibles < tokensASolicitar)
//                {
//                    return BadRequest("No tiene suficientes tokens disponibles para solicitar el canje.");
//                }

//                // Registrar solicitud de canje en TblPersonaTokens
//                var solicitudCanje = new tblPersonaTokens
//                {
//                    idPersona = idPersona,
//                    fecha = DateTime.Now,
//                    tokens = tokensASolicitar,
//                    idTipoEventoToken = 5, // En revisión
//                    idTipoProducto = idTipoProducto
//                };

//                db.tblPersonaTokens.Add(solicitudCanje);
//                await db.SaveChangesAsync();

//                // Registrar solicitud en tblRevisiones Token
//                var solicitudRevision = new tblSolicitudesTokens
//                {
//                    idPersonaToken = solicitudCanje.idPersonaToken,
//                    fechaSolicitud = DateTime.Now,
//                    idPersona = null,
//                    fechaAprobacion = null
//                };

//                db.tblSolicitudesTokens.Add(solicitudRevision);
//                await db.SaveChangesAsync();

//                await _calculoTokens.InsertarLogTokens(
//                    tipoOperacion: TipoOperacionToken.SOLICITUD_CANJE,
//                    tabla: "TblPersonaTokens",
//                    idPersona: idPersona,
//                    valorAnterior: null,
//                    valorNuevo: tokensASolicitar,
//                    idTipoEventoToken: 5,
//                    mensaje: $"Solicitud de canje de {tokensASolicitar} tokens"
//                );

//                return Ok(new
//                {
//                    IdPersonaToken = solicitudCanje.idPersonaToken,
//                    Message = "Solicitud de canje registrada correctamente",
//                    FechaSolicitud = solicitudCanje.fecha,
//                    TokensSolicitados = solicitudCanje.tokens,
//                });
//            }
//            catch (Exception ex)
//            {
//                await _calculoTokens.InsertarLogTokens(
//                    tipoOperacion: TipoOperacionToken.ERROR_SOLICITUD_CANJE,
//                    tabla: "TblPersonaTokens",
//                    idPersona: idPersona,
//                    esError: true,
//                    mensajeError: ex.Message,
//                    detalleError: ex.StackTrace
//                );

//                return BadRequest($"Error al solicitar canje de tokens: {ex.Message}");
//            }
//        }


//        [HttpPost("odata/aprobarSolicitudCanje")]
//        public async Task<ActionResult> AprobarSolicitudCanje(
//            [FromODataUri] int idPersonaToken,
//            [FromODataUri] int idPersonaAprobador)

//        {


//            var yaAprobada = await db.tblSolicitudesTokens
//                .AnyAsync(s => s.idPersonaToken == idPersonaToken && s.idPersona != null);

//            if (yaAprobada)
//            {
//                return BadRequest("Esta solicitud ya fue  procesada.");
//            }

//            try
//            {
//                // Buscar la solicitud de tokens original
//                var solicitudToken = await db.tblPersonaTokens
//                    .FirstOrDefaultAsync(t => t.idPersonaToken == idPersonaToken);

//                if (solicitudToken == null)
//                {
//                    return NotFound("Solicitud de tokens no encontrada.");
//                }

//                // Verificar que esté en estado de revisión
//                if (solicitudToken.idTipoEventoToken != 5) // 5 = En revisión
//                {
//                    return BadRequest("La solicitud no está en estado de revisión.");
//                }

//                // Insertar nuevo registro de tokens canjeados
//                var tokenCanjeado = new tblPersonaTokens
//                {
//                    idPersona = solicitudToken.idPersona,
//                    fecha = DateTime.Now,
//                    idLavanderia = solicitudToken.idLavanderia,
//                    tokens = solicitudToken.tokens,
//                    idTipoEventoToken = 2, // 2 = Canjeado
//                    idTipoProducto = solicitudToken.idTipoProducto
//                };

//                db.tblPersonaTokens.Add(tokenCanjeado);

//                // Actualizar registro de revisión
//                var solicitudRevision = await db.tblSolicitudesTokens
//                    .FirstOrDefaultAsync(r => r.idPersonaToken == idPersonaToken);

//                if (solicitudRevision != null)
//                {
//                    var aprobacionRevision = new tblSolicitudesTokens
//                    {
//                        idPersonaToken = idPersonaToken,
//                        fechaSolicitud = solicitudRevision.fechaSolicitud,
//                        idPersona = idPersonaAprobador,
//                        fechaAprobacion = DateTime.Now
//                    };

//                    db.tblSolicitudesTokens.Add(aprobacionRevision);
//                }

//                var tokensGanados = await db.tblPersonaTokens
//                     .Where(t => t.idPersona == solicitudToken.idPersona && t.idTipoEventoToken == 1)
//                     .SumAsync(t => t.tokens);

//                var tokensCanjeados = await db.tblPersonaTokens
//                    .Where(t => t.idPersona == solicitudToken.idPersona && t.idTipoEventoToken == 2)
//                    .SumAsync(t => t.tokens);

//                // Guardar cambios
//                await db.SaveChangesAsync();

//                // Cálculo solo para el log


//                float saldoAnterior = tokensGanados - tokensCanjeados;
//                float saldoNuevo = saldoAnterior - solicitudToken.tokens;

//                // Registrar log de aprobación
//                await _calculoTokens.InsertarLogTokens(
//                    tipoOperacion: TipoOperacionToken.APROBAR_CANJE,
//                    tabla: "TblPersonaTokens",
//                    idPersona: solicitudToken.idPersona,
//                    valorAnterior: saldoAnterior,
//                    valorNuevo: saldoNuevo,
//                    idTipoEventoToken: 2,
//                    mensaje: $"Solicitud de canje aprobada. Tokens canjeados: {solicitudToken.tokens}"
//                );

//                return Ok(new
//                {
//                    Message = "Solicitud de canje aprobada correctamente",
//                    TokensCanjeados = solicitudToken.tokens
//                });
//            }
//            catch (Exception ex)
//            {
//                // Registrar error
//                await _calculoTokens.InsertarLogTokens(
//                    tipoOperacion: TipoOperacionToken.ERROR_APROBAR_CANJE,
//                    tabla: "TblPersonaTokens",
//                    idPersona: 0,
//                    esError: true,
//                    mensajeError: ex.Message,
//                    detalleError: ex.StackTrace
//                );

//                return BadRequest($"Error al aprobar solicitud de canje: {ex.Message}");
//            }
//        }

//        /// <summary>
//        /// Rechazar solicitud de canje de tokens
//        /// </summary>
//        [HttpPost("odata/rechazarSolicitudCanje")]
//        public async Task<ActionResult> rechazarSolicitudCanje(
//        [FromODataUri] int idPersonaToken,
//        [FromODataUri] int idPersonaRevisor,
//        [FromODataUri] string motivoRechazo)
//        {



//            var yaProcesada = await db.tblSolicitudesTokens
//                .AnyAsync(s => s.idPersonaToken == idPersonaToken && s.idPersona != null);

//            if (yaProcesada)
//            {
//                return BadRequest("Esta solicitud ya fue procesada.");
//            }

//            using (var transaction = await db.Database.BeginTransactionAsync())
//            {
//                try
//                {
//                    // Buscar la solicitud de tokens original
//                    var solicitudToken = await db.tblPersonaTokens
//                        .FirstOrDefaultAsync(t => t.idPersonaToken == idPersonaToken);

//                    if (solicitudToken == null)
//                    {
//                        return NotFound("Solicitud de tokens no encontrada.");
//                    }

//                    // Verificar que esté en estado de revisión
//                    if (solicitudToken.idTipoEventoToken != 5)
//                    {
//                        return BadRequest("La solicitud no está para revisar.");
//                    }

//                    db.tblPersonaTokens.Add(new tblPersonaTokens
//                    {
//                        idPersona = solicitudToken.idPersona,
//                        fecha = DateTime.Now,
//                        idLavanderia = solicitudToken.idLavanderia,
//                        tokens = 0,
//                        idTipoEventoToken = 4,
//                        idTipoProducto = solicitudToken.idTipoProducto
//                    });

//                    // Insertar un registro de devolución 
//                    var tokenDevuelto = new tblPersonaTokens
//                    {
//                        idPersona = solicitudToken.idPersona,
//                        fecha = DateTime.Now,
//                        idLavanderia = solicitudToken.idLavanderia,
//                        tokens = solicitudToken.tokens,

//                        idTipoEventoToken = 3, // 3 = Devolución
//                        idTipoProducto = solicitudToken.idTipoProducto
//                    };

//                    db.tblPersonaTokens.Add(tokenDevuelto);

//                    // Actualizar registro de revisión
//                    var solicitudRevision = await db.tblSolicitudesTokens
//                        .FirstOrDefaultAsync(r => r.idPersonaToken == idPersonaToken);

//                    var tokensGanados = await db.tblPersonaTokens
//                        .Where(t => t.idPersona == solicitudToken.idPersona && t.idTipoEventoToken == 1)
//                        .SumAsync(t => t.tokens);

//                    var tokensCanjeados = await db.tblPersonaTokens
//                        .Where(t => t.idPersona == solicitudToken.idPersona && t.idTipoEventoToken == 2)
//                        .SumAsync(t => t.tokens);

//                    float saldoAnterior = tokensGanados - tokensCanjeados;
//                    float saldoNuevo = saldoAnterior; // No cambia, se devuelve

//                    if (solicitudRevision != null)
//                    {
//                        var rechazoRevision = new tblSolicitudesTokens
//                        {
//                            idPersonaToken = idPersonaToken,
//                            fechaSolicitud = solicitudRevision.fechaSolicitud,
//                            idPersona = idPersonaRevisor,
//                            fechaAprobacion = DateTime.Now
//                        };

//                        db.tblSolicitudesTokens.Add(rechazoRevision);
//                    }



//                    // Guardar cambios
//                    await db.SaveChangesAsync();

//                    // Registrar log de rechazo
//                    await _calculoTokens.InsertarLogTokens(
//                        tipoOperacion: TipoOperacionToken.RECHAZAR_CANJE,
//                        tabla: "TblPersonaTokens",
//                        idPersona: solicitudToken.idPersona,
//                        valorAnterior: saldoAnterior,
//                        valorNuevo: saldoNuevo,
//                        idTipoEventoToken: 4,
//                        mensaje: $"Solicitud de canje rechazada por {idPersonaRevisor}. Motivo: {motivoRechazo}. Tokens devueltos: {solicitudToken.tokens}"
//                    );


//                    await transaction.CommitAsync();

//                    return Ok(new
//                    {
//                        Message = "Solicitud de canje rechazada correctamente",
//                        TokensRechazados = solicitudToken.tokens,
//                        TokensDevueltos = solicitudToken.tokens,
//                        MotivoRechazo = motivoRechazo
//                    });
//                }
//                catch (Exception ex)
//                {
//                    await transaction.RollbackAsync();

//                    // Registrar error
//                    await _calculoTokens.InsertarLogTokens(
//                        tipoOperacion: TipoOperacionToken.ERROR_RECHAZAR_CANJE,
//                        tabla: "TblPersonaTokens",
//                        idPersona: 0,
//                        esError: true,
//                        mensajeError: ex.Message,
//                        detalleError: ex.StackTrace
//                    );

//                    return BadRequest($"Error al rechazar solicitud de canje: {ex.Message}");
//                }
//            }
//        }


//        // ====================== //




//        [HttpGet("odata/getTurnosTest")]
//        public async Task<ActionResult> getTurnosPorPersona(
//                    [FromODataUri] DateTime fechaIni,
//                    [FromODataUri] DateTime fechaFin)
//        {
//            var dict = await _calculoTokens.getTurnosPorPersona(fechaIni, fechaFin);

//            int totalTurnos = 0;
//            foreach (var humano in dict)
//            {
//                totalTurnos += humano.Value.Count;
//                System.Diagnostics.Debug.WriteLine($"CRISTOREY >> Persona {humano.Key}: {humano.Value.Count} turnos");
//            }

//            System.Diagnostics.Debug.WriteLine($"CRISTOREY >> Total turnos: {totalTurnos}");

//            return Ok(dict);
//        }
    

//    [HttpGet("odata/tokentest")]
//        public async Task<ActionResult<ResumenTokensDTO>> GetResumenTokensMasivos(
//        [FromODataUri] DateTime fechaIni,
//        [FromODataUri] DateTime fechaFin)
//        {
//            var turnosPorPersona = await _calculoTokens.getTurnosPorPersona(fechaIni, fechaFin);
//            var resumenPorPersona = await _calculoTokens.calcularTokensRetroactivamente(turnosPorPersona);

//            var resumen = new ResumenTokensDTO
//            {
//                TokensTotales = resumenPorPersona.Values.Sum(),
//                PersonasProcesadas = resumenPorPersona.Count,
//                ResumenPorPersona = resumenPorPersona
//            };

//            return Ok(resumen);
//        }


//    }

//    }




//        public class TurnoTokenRequest
//    {
//        public int idPersona { get; set; }
//        public DateTimeOffset fechaIni { get; set; }
//        public DateTimeOffset fechaFin { get; set; }
//    }

//        public class TurnoConTokensDto
//    {
//        public int IdPersona { get; set; }
//        public DateTimeOffset fechaIni { get; set; }
//        public DateTimeOffset fechaFin { get; set; }
//        public decimal Tokens { get; set; }
//    }

//        public class PersonaTurnoDTO
//    {
//        public int idPersona { get; set; }
//        public int idMaquina { get; set; }
//        public string denominacion { get; set; }
//        public DateTimeOffset fechaIni { get; set; }  
//        public DateTimeOffset fechaFin { get; set; }
//    public double intervaloTiempoSegundos { get; set; }
//    }

//        public class ResumenTokensDTO
//        {
//            public int TokensTotales { get; set; }
//            public int PersonasProcesadas { get; set; }
//            public Dictionary<int, int> ResumenPorPersona { get; set; } = new();
//        }



