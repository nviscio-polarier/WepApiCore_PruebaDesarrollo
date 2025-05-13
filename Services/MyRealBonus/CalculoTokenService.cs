using Dapper;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json;
using WebApiCore.Class;
using WebApiCore.Context;
using WebApiCore.Enums.MyRealBonus;
using WebApiCore.Enums.RRHH;
using WebApiCore.Hubs;

namespace WebApiCore.Services.MyRealBonus
{
    public class CalculoTokenService
    {

        private readonly bdERP db;
        private readonly IHubContext<NotificacionesHub> _hubContext;
        private const int FACTOR_ATENUANTE = 100;

        public CalculoTokenService(bdERP context, IHubContext<NotificacionesHub> hubContext)
        {
            db = context;
            _hubContext = hubContext;
        }

        private async Task<ResultadoCalculoPrendas> calcularTotal(Dictionary<(int?, int?), int> diccionarioPrendasPorTipo, double tiempoTurnoSegundos, int idMaquina)
        {
            var maximosTeoricos = new Dictionary<(int?, int?), double>();

            // Obtener máximos teóricos para cada tipo de prenda en el diccionario
            foreach (var item in diccionarioPrendasPorTipo)
            {
                int? familia = item.Key.Item1;
                int? tipoPrenda = item.Key.Item2;

                if (familia.HasValue && tipoPrenda.HasValue)
                {
                    var maximosTeoricosParaTipoPrenda = await getMaximosTeoricos(idMaquina, tipoPrenda.Value, familia.Value);

                    // Combinar los máximos teóricos
                    foreach (var TipoYFamilia in maximosTeoricosParaTipoPrenda)
                    {
                        maximosTeoricos[TipoYFamilia.Key] = TipoYFamilia.Value;
                    }
                }
            }

            var resultado = new ResultadoCalculoPrendas();

            // Calcular total de prendas
            resultado.prendasRealesProcesadasTotal = diccionarioPrendasPorTipo.Sum(tipo => tipo.Value);
            int tot = resultado.prendasRealesProcesadasTotal;

            // Diccionario para rastrear prendas por tipo de prenda
            var prendasPorTipoPrenda = new Dictionary<(int?, int?), int>();

            foreach (var item in diccionarioPrendasPorTipo)
            {
                int? familia = item.Key.Item1;
                int? tipoPrenda = item.Key.Item2;
                int cantidadPrendas = item.Value;

                // Acumular prendas por familia, tipo de prenda
                if (familia.HasValue && tipoPrenda.HasValue)
                {
                    var key = (familia, tipoPrenda);

                    if (prendasPorTipoPrenda.ContainsKey(key))
                        prendasPorTipoPrenda[key] += cantidadPrendas;
                    else
                        prendasPorTipoPrenda[key] = cantidadPrendas;
                }
            }

            // Procesar los datos acumulados por tipo de prenda
            foreach (var tipoPrenda in prendasPorTipoPrenda)
            {
                var (familia, tipoId) = tipoPrenda.Key;
                int cantidadPrendas = tipoPrenda.Value;

                // Guardar prendas por tipo
                resultado.dictPrendasRealesProcesadasPorTipo[tipoId.Value] = cantidadPrendas;

                // Calcular porcentaje respecto al total
                resultado.dictPorcentajePrendaRespectoAlTotal[tipoId.Value] = (double)cantidadPrendas / tot;

                double maxTeoricoHora = maximosTeoricos.TryGetValue((familia, tipoId), out var valor) ? valor : 600;

                double maxTeoricoSegundo = maxTeoricoHora / 3600.0;
                double maxTeoricoPeriodo = maxTeoricoSegundo * tiempoTurnoSegundos * resultado.dictPorcentajePrendaRespectoAlTotal[tipoId.Value];
                resultado.dictMaxTeoricoPonderado[tipoId.Value] = maxTeoricoPeriodo;
                resultado.dictPrendasRealesProcesadasPorTipo[tipoId.Value] = cantidadPrendas;
            }

            resultado.maxTeoricoPonderadoSuma = resultado.dictMaxTeoricoPonderado.Sum(porcentaje => porcentaje.Value);
            resultado.porcentajeSobreElUmbralDeRendimiento = resultado.prendasRealesProcesadasTotal / resultado.maxTeoricoPonderadoSuma * 100 - 100;
            resultado.tiempoSobreUmbralMins = (resultado.prendasRealesProcesadasTotal * tiempoTurnoSegundos / resultado.maxTeoricoPonderadoSuma - tiempoTurnoSegundos) / 60;
            resultado.umbralPositivo = resultado.porcentajeSobreElUmbralDeRendimiento > 1;

            return resultado;
        }


        /// <summary>
        /// Obtiene las prendas procesadas en una máquina específica durante un período determinado, solo si es plegadora
        /// </summary>
        private async Task<Dictionary<(int?, int?), int>> getDiccionarioPrendas(
        int idMaquina,
        string denominacionMaquina,
        DateTimeOffset? fechaIni,
        DateTimeOffset? fechaFin)
            {
            try
            {
                // Si no es plegadora nada
                if (string.IsNullOrEmpty(denominacionMaquina) || !denominacionMaquina.ToLower().Contains("pleg"))
                {
                    return new Dictionary<(int?, int?), int>();
                }

                // Coger las prendas procesadas en esta máquina durante este periodo
                var prendas = await db.tblPrendaNMaquina
                    .Where(p => p.idMaquina == idMaquina &&
                           p.fecha >= fechaIni &&
                           p.fecha <= fechaFin)
                    .ToListAsync();

                // Crear el diccionario agrupando por familia y tipo de prenda
                var diccionarioPrendas = new Dictionary<(int?, int?), int>();
                foreach (var prenda in prendas)
                {
                    var key = (prenda.idFamilia, prenda.idTipoPrenda);

                    if (diccionarioPrendas.ContainsKey(key))
                        diccionarioPrendas[key]++;
                    else
                        diccionarioPrendas[key] = 1;
                }

                return diccionarioPrendas;
            }
            catch (Exception)
            {
                return new Dictionary<(int?, int?), int>();
            }
        }

        private async Task<Dictionary<(int?, int?), double>> getMaximosTeoricos(int idMaquina, int idTipoPrenda, int idFamilia)
        {
            var maximosTeoricosDB = await db.tblPrendasHora
                .Where(p => p.idMaquina == idMaquina &&
                            p.idTipoPrenda == idTipoPrenda &&
                            p.idFamilia == idFamilia)
                .ToDictionaryAsync(
                    x => ((int?)x.idFamilia, (int?)x.idTipoPrenda),
                    x => (double)x.prendasHora
                );

            return maximosTeoricosDB;
        }

        /// <summary>
        /// Calcula los tokens a ganar basados en el resultado del cálculo de prendas
        /// </summary>
        private int calcularTokensAGanar(ResultadoCalculoPrendas resultadoCalculoPrendas)
        {
            if (!resultadoCalculoPrendas.umbralPositivo)
            {
                return 0;
            }

            double tokensAGanarSinRedondear = resultadoCalculoPrendas.porcentajeSobreElUmbralDeRendimiento *
                                             resultadoCalculoPrendas.tiempoSobreUmbralMins / FACTOR_ATENUANTE;
            int tokensAGanar = (int)Math.Round(tokensAGanarSinRedondear);

            return tokensAGanar;
        }


        /// <summary>
        /// Función pública que calcula los tokens ganados por una persona en una máquina durante un período
        /// </summary>
        /// <param name="idPersona">ID de la persona</param>
        /// <param name="idMaquina">ID de la máquina</param>
        /// <param name="fechaIni">Fecha de inicio del período</param>
        /// <param name="fechaFin">Fecha de fin del período</param>
        /// <param name="tiempoTurnoSegundos">Duración del turno en segundos</param>
        /// <returns>Resultado con los cálculos y tokens ganados</returns>
        public async Task<(ResultadoCalculoPrendas Resultado, int TokensGanados, string Mensaje)> calcularTokensParaPersona(
            int idPersona,
            int idMaquina,
            DateTimeOffset? fechaIni,
            DateTimeOffset? fechaFin,
            double tiempoTurnoSegundos)
        {
            try
            {
                var maquina = await db.tblMaquina
                    .FirstOrDefaultAsync(m => m.idMaquina == idMaquina);

                // Obtener diccionario de prendas
                var diccionarioPrendas = await getDiccionarioPrendas(
                    idMaquina,
                    maquina.denominacion,
                    fechaIni,
                    fechaFin);

                if (!diccionarioPrendas.Any())
                {
                    await InsertarLogTokens(
                        tipoOperacion: TipoOperacionToken.INSERT,
                        tabla: "TblPersonaTokens",
                        idPersona: idPersona,
                        valorAnterior: null,
                        valorNuevo: null,
                        esError: true,
                        mensajeError: $"{MensajesErrorTokens.MaquinaNoEsPlegadora} o {MensajesErrorTokens.NingunaPrendaProcesada}",
                        detalleError: null,
                        mensaje: "",
                        idTipoEventoToken: -1
                    ); return (null, 0, $"{MensajesErrorTokens.MaquinaNoEsPlegadora} o {MensajesErrorTokens.NingunaPrendaProcesada}");

                }



                // Calcular
                var resultadoCalculo = await calcularTotal(diccionarioPrendas, tiempoTurnoSegundos, idMaquina);
                int tokensGanados = calcularTokensAGanar(resultadoCalculo);

                if (tokensGanados > 0)
                {
                    // Insertar tokens ganados en la base de datos (isCanje falso por defecto)
                    await InsertarTokens(
                        idPersona: idPersona,
                        fecha: (DateTimeOffset)fechaFin,
                        idLavanderia: maquina.idLavanderia,
                        tokens: tokensGanados,
                        idTipoProducto: null,
                        idTipoEventoToken: 1,
                        fechaIniTurno: fechaIni,
                        fechaFinTurno: fechaFin
                    );
                } else
                {
                    await InsertarLogTokens(
                        tipoOperacion: TipoOperacionToken.INSERT,
                        tabla: "TblPersonaTokens",
                        idPersona: idPersona,
                        idTipoEventoToken: 6,
                        mensaje: $"{MensajesErrorTokens.UmbralInsuficiente} en: ({resultadoCalculo.porcentajeSobreElUmbralDeRendimiento} en : {resultadoCalculo.tiempoSobreUmbralMins})"
                    );
                }

                return (resultadoCalculo, tokensGanados, "calculo ok");
            }
            catch (Exception ex)
            {
                await InsertarLogTokens(
                    tipoOperacion: TipoOperacionToken.INSERT,
                    tabla: "TblPersonaTokens",
                    idPersona: idPersona,
                    valorAnterior: null,
                    valorNuevo: 0,
                    esError: true,
                    mensajeError: ex.Message
                ); return (null, 0, $"Error: {ex.Message}");
            }
        }


        /// <summary>
        /// Inserta tokens para una persona en un turno concreto.
        /// </summary>
        /// <param name="idPersona">ID de la persona</param>
        /// <param name="fecha"></param>
        /// <param name="idLavanderia">ID de la lavandería</param>
        /// <param name="tokens">Cantidad de tokens</param>
        /// <param name="isCanje">Si es un canje </param>

        /// <returns>Task</returns>
        private async Task InsertarTokens(
            int idPersona,
            DateTimeOffset fecha,
            int idLavanderia,
            int tokens,
            DateTimeOffset? fechaIniTurno,
            DateTimeOffset? fechaFinTurno,
            int? idTipoProducto = null,
            int? idTipoEventoToken = 1


            )
        {
            // Insertar en la tabla de tokens
            db.tblPersonaTokens.Add(new tblPersonaTokens
            {
                idPersona = idPersona,
                fecha = fecha,
                idLavanderia = idLavanderia,
                tokens = tokens,
                idTipoProducto = idTipoProducto,
                idTipoEventoToken = idTipoEventoToken,
                fechaIniTurno = fechaIniTurno,
                fechaFinTurno = fechaFinTurno   
            });

            await db.SaveChangesAsync();

            await InsertarLogTokens(
                tipoOperacion: TipoOperacionToken.INSERT,
                tabla: "TblPersonaTokens",
                idPersona: idPersona,
                valorNuevo: tokens,
                idTipoEventoToken: 1
            );
        }

        public  async Task InsertarLogTokens(
            TipoOperacionToken tipoOperacion,
            string tabla,
            int idPersona,
            float? valorAnterior = null,
            float? valorNuevo = null,
            bool esError = false,
            string mensajeError = null,
            string detalleError = null,
            string mensaje = "",
            int idTipoEventoToken = -1
            )
        {
            db.tblLogsToken.Add(new tblLogsToken
            {
                tipoOperacion = tipoOperacion.ToString(),
                tabla = tabla,
                fechaHora = DateTime.Now,
                idPersona = idPersona,
                valorAnterior = valorAnterior,
                valorNuevo = valorNuevo,
                esError = esError,
                mensajeError = mensajeError,
                detalleError = detalleError,
                mensaje = mensaje,
                idTipoEventoToken = idTipoEventoToken,
            });

            await db.SaveChangesAsync();
        }




        public double obtenerTiempoTurnoEnSegundos(DateTimeOffset? inicio, DateTimeOffset? fin)
        {
            // Calcula la diferencia entre las fechas
            TimeSpan duracion = (TimeSpan)(fin - inicio);

            // Devuelve el total de segundos como double
            return duracion.TotalSeconds;
        }

        // ========================= //

        public async Task<Dictionary<int, List<PersonaTurnoDTO>>> getTurnosPorPersona(DateTime fechaIni, DateTime fechaFin)
        {
            var resultado = new Dictionary<int, List<PersonaTurnoDTO>>();

            var personas = await db.tblPersonaNMaquina
                .Where(p => p.fechaIni >= fechaIni && p.fechaFin <= fechaFin)
                .Select(p => p.idPersona)
                .Distinct()
                .ToListAsync();

            var connection = db.Database.GetDbConnection();

            foreach (var idPersona in personas)
            {
                var turnos = (await connection.QueryAsync<PersonaTurnoDTO>(
                    "[MyRealData].[EF_calcularRendimientoPersonav2]",
                    new { idPersona, fechaIni, fechaFin },
                    commandType: CommandType.StoredProcedure
                )).ToList();

                if (turnos.Any())
                {
                    resultado[idPersona] = turnos;
                }
            }

            return resultado;
        }

        public async Task<Dictionary<int, int>> calcularTokensRetroactivamente(Dictionary<int, List<PersonaTurnoDTO>> listaPersonaTurno)
        {
            var resumenPorPersona = new Dictionary<int, int>();

            foreach (var personaTurno in listaPersonaTurno)
            {
                int idPersona = personaTurno.Key;
                var turnoLista = personaTurno.Value;
                int tokensPorPersona = 0;

                foreach (var turno in turnoLista)
                {
                    var (_, tokens, _) = await calcularTokensParaPersona(
                        idPersona,
                        turno.idMaquina,
                        turno.fechaIni,
                        turno.fechaFin,
                        turno.intervaloTiempoSegundos
                    );

                    if (tokens > 0)
                    {
                        tokensPorPersona += tokens;
                    }
                }

                resumenPorPersona[idPersona] = tokensPorPersona;
            }

            return resumenPorPersona;
        }


        public class ResultadoCalculoPrendas
        {
            public Dictionary<int, int> dictPrendasRealesProcesadasPorTipo { get; set; } = new Dictionary<int, int>();
            public int prendasRealesProcesadasTotal { get; set; }
            public Dictionary<int, double> dictPorcentajePrendaRespectoAlTotal { get; set; } = new Dictionary<int, double>();
            public Dictionary<int, double> dictMaxTeoricoPonderado { get; set; } = new Dictionary<int, double>();
            public double maxTeoricoPonderadoSuma { get; set;  }
            public double porcentajeSobreElUmbralDeRendimiento { get; set; }
            public double tiempoSobreUmbralMins { get; set; }
            public bool umbralPositivo { get; set; }
        }


        public class PersonaTurnoDTO
        {
            public int idPersona { get; set; }
            public int idMaquina { get; set; }
            public string denominacion { get; set; }
            public DateTimeOffset fechaIni { get; set; }
            public DateTimeOffset fechaFin { get; set; }
            public double intervaloTiempoSegundos { get; set; }
        }

        public class ResumenTokensDTO
        {
            public int TokensTotales { get; set; }
            public int PersonasProcesadas { get; set; }
            public Dictionary<int, int> ResumenPorPersona { get; set; } = new();
        }




    }
}