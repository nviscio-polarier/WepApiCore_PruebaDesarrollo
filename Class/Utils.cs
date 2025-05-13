using FirebaseAdmin.Messaging;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using WebApiCore.Class.notificaciones;
using WebApiCore.Context;

namespace WebApiCore.Class
{
    public class Utils
    {
        public static DateTimeOffset getFechaLavanderia(Context.bdERP db, int idLavanderia, DateTimeOffset fecha)
        {
            var horarioLavanderia = db.tblLavanderia.Select(x => new { x.idZonaHorariaNavigation, x.horarioVerano, x.idLavanderia }).FirstOrDefault(x => x.idLavanderia == idLavanderia);
            int GMT = (horarioLavanderia.horarioVerano == true ? 1 : 0) + Convert.ToInt32(horarioLavanderia.idZonaHorariaNavigation.GMT);
            return aplicarGMT(fecha, GMT);
        }


        public static DateTimeOffset aplicarGMT(DateTimeOffset fecha, int GMT)
        {
            DateTimeOffset reg_fecha = (fecha).ToOffset(TimeSpan.FromHours(GMT));
            return reg_fecha;
        }

        public static string getMD5(string str)
        {
            MD5 md5 = MD5CryptoServiceProvider.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = md5.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

        // Listado de IDS de compañias y entidades genéricas, eliminadas por la implementación de prendas genéricas
        // pero que han de seguir apareciendo en los listados de informes
        private static readonly List<int> entidadesGenericas = new List<int> { 579, 626, 678 };
        public static List<int> selectEntidadesVisibles(Context.bdERP db, int idUsuario, int? idLavanderia, bool showGenerico = false)
        {
            var objUsuario = db.tblUsuario.Where(x => x.idUsuario == idUsuario && !x.isEliminado)
                 .Select(x => new
                 {
                     x.idCargoNavigation,
                     tblEntidadNUsuario = x.idEntidad,
                     tblLavanderiaNUsuario = x.idLavanderia
                 }).FirstOrDefault();

            byte estatus = objUsuario.idCargoNavigation.estatus;
            List<int> idsEntidad = new List<int>();
            if (estatus != 0)
            {
                idsEntidad.AddRange(
                    objUsuario.tblEntidadNUsuario
                    .Where(x => ((x.activo.Equals(true) && x.eliminado.Equals(false)) || (showGenerico && entidadesGenericas.Contains(x.idEntidad))) &&
                     (idLavanderia == null || (idLavanderia != null && x.idEntidadNavigation.Select(y => y.idLavanderia.Select(lav => lav.idLavanderia == idLavanderia)).Count() > 0)))
                    .Select(x => x.idEntidad).ToList());

                if (estatus == 1 || idsEntidad.Count.Equals(0))
                {
                    idsEntidad.AddRange(db.tblEntidad
                        .Where(x => ((x.activo.Equals(true) && x.eliminado.Equals(false)) || (showGenerico && entidadesGenericas.Contains(x.idEntidad))) &&
                        x.idLavanderia.Where(l => objUsuario.tblLavanderiaNUsuario.Select(lavUsuario => lavUsuario.idLavanderia).Contains(l.idLavanderia) &&
                        ((idLavanderia != null && l.idLavanderia == idLavanderia) || idLavanderia == null)).Count() > 0)
                        .Select(x => x.idEntidad).ToList());
                }
            }
            else
            {
                idsEntidad.AddRange(db.tblEntidad
                    .Where(x => x.activo.Equals(true) && x.eliminado.Equals(false) &&
                     ((idLavanderia != null && (x.idLavanderia.Where(lav => lav.idLavanderia == idLavanderia).Count() > 0)) || idLavanderia == null))
                    .Select(x => x.idEntidad)
                    .ToList()
                );
            }

            return idsEntidad;
        }

        /**
         * @param db
         * @param idLavanderia
         * @return
         */
        public static IQueryable<int?> selectPrendasGenericasVisibles(Context.bdERP db, int? idLavanderia)
        {
            var idsVisibles = db.tblEntidad
                    .Where(x => x.idLavanderia.Any(y => y.idLavanderia == idLavanderia))
                    .Join(db.tblPrenda,
                        entidad => entidad.idCompañia,
                        prenda => prenda.idCompañia,
                        (entidad, prenda) => prenda
                    )
                    .Where(y => y.activo == true
                        && y.idPlantillaPrenda_generica != null
                    )
                    .Select(x => x.idPlantillaPrenda_generica)
                    .Distinct();

            return idsVisibles;
        }

        /**
         * @param db
         * @param idUsuario
         * @param filtroTipoTrabajoRRHH -- Si es true, se filtra por los tipos de trabajo que tiene asignado el usuario
         * @return
         */
        public static IQueryable<int> selectPersonasVisibles(Context.bdERP db, int idUsuario, bool filtroTipoTrabajoRRHH = true)
        {
            var objUsuario = db.tblUsuario
               .Where(x => x.idUsuario.Equals(idUsuario) && !x.isEliminado)
               .Select(x => new
               {
                   x.idUsuario,
                   x.enableDatosRRHH,
                   x.idPersona,
                   tblCentroTrabajo = x.idCentroTrabajo.Select(x => x.idCentroTrabajo),
                   tblLavanderia = x.idLavanderia.Select(x => x.idLavanderia)
               }).FirstOrDefault();

            var tblTipoTrabajoNUsuario = db.tblTipoTrabajoNUsuario.Where(x => x.idUsuario.Equals(idUsuario));

            bool hasTblTipoTrabajoNUsuario = tblTipoTrabajoNUsuario.Count() > 0;

            var idsPersona = db.tblPersona.Where(pers =>
                ((pers.idLavanderia != null && objUsuario.tblLavanderia.Contains((int)pers.idLavanderia)) || pers.idCentroTrabajo != null)
                && (pers.idCentroTrabajo == null || (pers.idCentroTrabajo != null && objUsuario.tblCentroTrabajo.Contains((int)(pers.idCentroTrabajo))))
                && (
                    (!filtroTipoTrabajoRRHH && objUsuario.enableDatosRRHH) ||
                    (hasTblTipoTrabajoNUsuario && tblTipoTrabajoNUsuario.FirstOrDefault(x => x.idLavanderia == pers.idLavanderia && x.idTipoTrabajo == pers.idTipoTrabajo) != null) ||
                    !hasTblTipoTrabajoNUsuario
                )
            ).Select(z => z.idPersona);

            return idsPersona;
        }

        public static bool isProduccion()
        {
            var configuation = GetConfiguration();
            string dbName = new SqlConnection(configuation.GetSection("ConnectionStrings").GetSection("bdERP").Value).Database;
            return (dbName == "bdERP");
        }

        public static bool isDevelopment()
        {
            var configuation = Utils.GetConfiguration();
            string dbName = new SqlConnection(configuation.GetSection("ConnectionStrings").GetSection("bdERP").Value).Database;
            return (dbName != "bdERP" && dbName != "bdERPpreproduccion");
        }



        public string GenerarCodigoAbono(Context.bdERP db)
        {
            // Obtener el año actual en formato "YY"
            string añoActual = DateTime.Now.ToString("yy");

            // Consultar el último código de abono para la lavandería dada en tblAbono
            var ultimoCodigoAbono = db.tblAbono
                .Where(a => a.codigo.StartsWith(añoActual))
                .OrderByDescending(a => a.codigo)
                .Select(a => a.codigo)
                .FirstOrDefault();

            // Consultar el último código de solicitud de abono para la lavandería dada
            var ultimoCodigoSolicitud = db.tblSolicitudAbono
                .Where(sa => sa.codigo.StartsWith(añoActual))
                .OrderByDescending(sa => sa.codigo)
                .Select(sa => sa.codigo)
                .FirstOrDefault();

            // Determinar cual es el último código entre ambas tablas
            string ultimoCodigo = null;
            if (!string.IsNullOrEmpty(ultimoCodigoAbono) && !string.IsNullOrEmpty(ultimoCodigoSolicitud))
            {
                // Extraer los números secuenciales para comparar
                int secuencialAbono = int.Parse(ultimoCodigoAbono.Substring(2, 6));
                int secuencialSolicitud = int.Parse(ultimoCodigoSolicitud.Substring(2, 6));

                // Usar el mayor de los dos
                ultimoCodigo = secuencialAbono >= secuencialSolicitud ? ultimoCodigoAbono : ultimoCodigoSolicitud;
            }
            else
            {
                // Usar el que no sea nulo, o null si ambos son nulos
                ultimoCodigo = ultimoCodigoAbono ?? ultimoCodigoSolicitud;
            }

            // Extraer el número secuencial del último código o inicializarlo en 0
            int numeroSecuencial = 0;
            if (!string.IsNullOrEmpty(ultimoCodigo))
            {
                // Extraer los últimos 6 caracteres del código y convertirlos a entero
                string numeroSecuencialStr = ultimoCodigo.Substring(2, 6);
                numeroSecuencial = int.Parse(numeroSecuencialStr);
            }

            // Incrementar el número secuencial
            numeroSecuencial++;

            // Formatear el nuevo código: "YY" + número secuencial (6 dígitos)
            string nuevoCodigo = $"{añoActual}{numeroSecuencial:D6}";
            return nuevoCodigo;
        }

        private static IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }

        public static IEnumerable<IEnumerable<DateTime>> GroupConsecutiveDates(IEnumerable<DateTime> dates)
        {
            return dates
              .Select((date, index) => (date, index))
              .GroupBy(
                group => group.index - (group.date.Year + group.date.Month + group.date.Day),
                (date, index) => index.Select(t => t.date)
               );
        }

        /// <summary>
        /// Prepara el contenido del csv a partir de una lista de objetos.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>String preparado para ser codificado</returns>
        public static string GenerateCSV<T>(IEnumerable<T> data, Func<PropertyInfo, string>? headerSelector = null)
        {
            var csv = new StringBuilder();
            char separator = ';';
            var properties = typeof(T).GetProperties();

            // Escribir encabezados
            csv.AppendLine(string.Join(separator, properties.Select(headerSelector ?? (x => x.Name))));

            // Escribir datos
            foreach (var item in data)
            {
                var values = properties.Select(p => p.GetValue(item)?.ToString() ?? string.Empty);
                csv.AppendLine(string.Join(separator, values));
            }

            return csv.ToString();
        }

        public class Notificaciones
        {
            ////////////////////////////////////////////////////////////////////////////////////////////////
            /// Usar esta tabla para cuando se deba hacer mantenimiento de las notificaciones
            ///     idNotificacion_Estado  | denominacion   | isEstadoFinal
            ///     ========================================================
            ///             1	           | recibida	    | 1	
            ///             2              | eliminada      | 1	
            ///             3	           | leida	        | 1	
            ///             4	           | enviada	    | 0	
            ///             5	           | reenviada	    | 0	
            ///             6	           | fallida 	    | 0	
            ///             7	           | creada  	    | 0	
            /// Se usa el idNotificacion_Estado para operar. La denominación es para nosotros entender el id
            ////////////////////////////////////////////////////////////////////////////////////////////////

            private readonly Context.bdERP db;

            public Notificaciones(Context.bdERP bdContext)
            {
                db = bdContext;
            }

            public async Task<bool> SendMessageAsync(NotificacionRequest request)
            {
                try
                {
                    int? idNotificacion = null;
                    if (request.idUsuario != null)
                    {
                        tblNotificacion notificationRecord = new()
                        {
                            idUsuario = request.idUsuario,
                            titulo = request.title,
                            texto = request.body,
                            idNotificacion_Estado = 7, //estado de notificacion "creada"
                            tblNotificacion_Evento = new List<tblNotificacion_Evento>()
                                                {
                                                    new tblNotificacion_Evento
                                                    {
                                                        idNotificacion_Estado = 7 //estado de notificacion "creada"
                                                    }
                                                }
                        };
                        db.tblNotificacion.Add(notificationRecord);
                        db.SaveChanges();
                        idNotificacion = notificationRecord.idNotificacion;
                    }
                    else  //En caso de que el mensaje no tenga un destinatario se supondra que es un broadcast.
                    {
                        request.topic = "global";
                    }

                    //Creación del objeto de envío.
                    Message notification = new()
                    {
                        Notification = new Notification()
                        {
                            Title = request.title,
                            Body = request.body,
                        },
                        Token = request.notificacionToken,
                        Topic = request.topic,
                        Android = new AndroidConfig()
                        {
                            Priority = Priority.High
                        }
                    };

                    //En caso de no ser broadcast se enviará el idNotificación para el sistema ACK
                    if (idNotificacion != null)
                    {
                        notification.Data = new Dictionary<string, string>
                        {
                            {"idNotificacion", idNotificacion.ToString()}
                        };
                    }

                    string? result = "";
                    try
                    {
                        if (isProduccion())
                        {
                            FirebaseMessaging? messaging = FirebaseMessaging.DefaultInstance;
                            result = await messaging.SendAsync(message: notification);
                        } else
                        {
                            throw new Exception("No se puede enviar notificaciones en entorno de desarrollo");
                        }
                    }
                    catch (FirebaseMessagingException ex)
                    {
                        db.tblNotificacion_Evento.Add(new tblNotificacion_Evento()
                        {
                            idNotificacion = idNotificacion,
                            idNotificacion_Estado = 6 //estado de notificacion "fallida"
                        });
                        return false;
                    }

                    //Generamos el evento de notificación del envío.
                    if (idNotificacion != null)
                    {
                        bool resultOk = !string.IsNullOrEmpty(result);
                        tblNotificacion_Evento evento = new()
                        {
                            idNotificacion = idNotificacion,
                        };

                        if (resultOk)
                        {
                            evento.idMessage = result.Substring(result.LastIndexOf('/') + 1);
                            evento.idNotificacion_Estado = 4; //estado de notificacion "enviada"
                                                              //eventoCreacion.idMessage = result.Substring(result.LastIndexOf('/') + 1);
                        }
                        else
                        {
                            evento.idNotificacion_Estado = 6; //estado de notificacion "fallida"
                        }
                        db.tblNotificacion_Evento.Add(evento);
                        db.SaveChanges();

                        return resultOk;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
                return true;
            }

            public async Task<bool> SendMessageAsync(List<NotificacionRequest> request)
            {
                try
                {
                    foreach (NotificacionRequest requestItem in request)
                    {
                        await SendMessageAsync(requestItem);
                    }
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }

            }
        }

        public static Dictionary<int, Dictionary<DateTime, bool>> GetPersonasNFechasContratadas(Context.bdERP db, List<int> idsPersona, DateTime fechaDesde, DateTime fechaHasta)
        {
            var resultados = new Dictionary<int, Dictionary<DateTime, bool>>();

            var tblPersona = db.tblPersona
                .Include(x => x.tblPersonaNTipoContrato)
                .Where(x => idsPersona.Contains(x.idPersona))
                .ToList();

            foreach (var persona in tblPersona)
            {
                var dicPersona = new Dictionary<DateTime, bool>();
                for (var fecha = fechaDesde; fecha <= fechaHasta; fecha = fecha.AddDays(1))
                {
                    bool estaDentroDeContrato = persona.tblPersonaNTipoContrato.Any(contrato =>
                        fecha >= contrato.fechaAltaContrato && (fecha <= contrato.fechaBajaContrato || contrato.fechaBajaContrato == null));

                    dicPersona.Add(fecha, estaDentroDeContrato);
                }
                resultados.Add(persona.idPersona, dicPersona);
            }

            return resultados;
        }

        public static async Task<bool> FinalizaClienteNMaquinaAsync(Context.bdERP db, DateTimeOffset fechaEventPers, int idMaquina, int idPersona)
        {
            try
            {

                List<tblClienteNMaquina> clienteNMaquina = db.tblClienteNMaquina.Where(x => x.idMaquina == idMaquina
                                                                                            && x.fechaIni < fechaEventPers
                                                                                            && x.fechaFin == null).ToList();

                List<tblPersonaNMaquina> personaNMaquina = db.tblPersonaNMaquina.Where(x => x.idMaquina == idMaquina
                                                                                            && x.idPersona != idPersona
                                                                                            && x.fechaIni < fechaEventPers
                                                                                            && x.fechaFin == null
                                                                                            && x.numPos != null).ToList();
                if (personaNMaquina.Count == 0)
                {
                    foreach (var item in clienteNMaquina)
                    {
                        item.fechaFin = fechaEventPers;
                    }
                }

                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
