using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using WebApiCore.Class.externos;
using WebApiCore.Context;

namespace WebApiCore.Services
{
    public class Estancias_Worker : IHostedService, IDisposable
    {
        private Timer? _timer = null;
        private readonly bdERP db;

        public Estancias_Worker(IServiceProvider serviceProvider)
        {
            IServiceScope scope = serviceProvider.CreateScope();
            db = scope.ServiceProvider.GetRequiredService<bdERP>();
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            var runTime = new TimeSpan(2, 0, 0);
            var currentTime = DateTime.Now.TimeOfDay;

            // Si runTime aún no ha ocurrido hoy, resta la hora actual de runTime
            // De lo contrario, calcula el tiempo hasta runTime de mañana
            var timeToGo = currentTime < runTime ? runTime - currentTime : TimeSpan.FromDays(1) + runTime - currentTime;

            _timer = new Timer(Tareas, null, timeToGo, TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }

        private async void Tareas(object? state)
        {
            await ImportarEstancias();
            PlanificarPedidos(db);
        }

        async private Task ImportarEstancias()
        {
            var fechaHasta = DateTime.Today.AddDays(31);
            var fechaDesde = DateTime.Today.AddDays(-31);

            TimonHotelDataService timonHotelDS = new(db, fechaDesde, fechaHasta);
            AllSunDataService allsunDS = new(db, fechaDesde, fechaHasta);
            FergusDataService fergusDS = new(db, fechaDesde, fechaHasta);
            UniversalDataService universalDS = new(db, fechaDesde, fechaHasta);
            MacHotelsDataService macHotelsDS = new(db, fechaDesde, fechaHasta);

            List<int> entidades = new();
            entidades.AddRange(timonHotelDS.entidades);     // Timon Hotel
            entidades.AddRange(allsunDS.entidades);         // Allsun
            entidades.AddRange(fergusDS.entidades);         // Fergus
            entidades.AddRange(universalDS.entidades);      // Universal
            entidades.AddRange(macHotelsDS.entidades);      // MacHotels

            var cierres = db.tblCierreFactEntidad   // Calculo de fechas de cierre facturación entidad
                                        .Where(cierre =>
                                            entidades.Contains(cierre.idEntidad)
                                            && (
                                                (cierre.fechaDesde >= fechaDesde && cierre.fechaDesde <= fechaHasta)
                                                || (cierre.fechaHasta >= fechaDesde && cierre.fechaHasta <= fechaHasta)
                                                || (fechaDesde >= cierre.fechaDesde && fechaDesde <= cierre.fechaHasta)
                                                || (fechaHasta >= cierre.fechaDesde && fechaHasta <= cierre.fechaHasta)
                                             )
                                        ).ToList();

            //var estFactAbierta = (from est in db.tblEstancia  // Estancias que se borrarán para su posterior inserción, cuya fecha esta fuera del cierre de facturación
            //                      where
            //                          entidades.Contains(est.idEntidad)
            //                          && est.fecha >= fechaDesde && est.fecha <= fechaHasta
            //                      select est).ToList()
            //                                .Where(est => !cierres
            //                                    .Where(cierre => cierre.idEntidad == est.idEntidad)
            //                                    .Any(cierre => est.fecha.Date >= cierre.fechaDesde.Date && est.fecha.Date <= cierre.fechaHasta.Date)
            //                                );

            //db.tblEstancia.RemoveRange(estFactAbierta);

            db.tblLogError.Add(new tblLogError
            {
                denominacion = "EstanciasWorker - Importar",
                error = JsonConvert.SerializeObject(new
                {
                    msg = "Empezar proceso de importación de estancias",
                    fecha = DateTime.Now
                })
            });

            try
            {
                db.tblLogError.Add(new tblLogError
                {
                    denominacion = "EstanciasWorker - Importar",
                    error = JsonConvert.SerializeObject(new
                    {
                        msg = "timonHotelDS - Empezar proceso de importación de estancias",
                        fecha = DateTime.Now
                    })
                });
                await timonHotelDS.ImportarEstancias(cierres);    //Timon Hotel
                db.tblLogError.Add(new tblLogError
                {
                    denominacion = "EstanciasWorker - Importar",
                    error = JsonConvert.SerializeObject(new
                    {
                        msg = "timonHotelDS - Terminar proceso de importación de estancias",
                        fecha = DateTime.Now
                    })
                });

                db.tblLogError.Add(new tblLogError
                {
                    denominacion = "EstanciasWorker - Importar",
                    error = JsonConvert.SerializeObject(new
                    {
                        msg = "allsunDS - Empezar proceso de importación de estancias",
                        fecha = DateTime.Now
                    })
                });
                await allsunDS.ImportarEstancias(cierres);        //Allsun
                db.tblLogError.Add(new tblLogError
                {
                    denominacion = "EstanciasWorker - Importar",
                    error = JsonConvert.SerializeObject(new
                    {
                        msg = "allsunDS - Terminar proceso de importación de estancias",
                        fecha = DateTime.Now
                    })
                });

                db.tblLogError.Add(new tblLogError
                {
                    denominacion = "EstanciasWorker - Importar",
                    error = JsonConvert.SerializeObject(new
                    {
                        msg = "fergusDS - Empezar proceso de importación de estancias",
                        fecha = DateTime.Now
                    })
                });
                await fergusDS.ImportarEstancias(cierres);        //Fergus
                db.tblLogError.Add(new tblLogError
                {
                    denominacion = "EstanciasWorker - Importar",
                    error = JsonConvert.SerializeObject(new
                    {
                        msg = "fergusDS - Terminar proceso de importación de estancias",
                        fecha = DateTime.Now
                    })
                });

                db.tblLogError.Add(new tblLogError
                {
                    denominacion = "EstanciasWorker - Importar",
                    error = JsonConvert.SerializeObject(new
                    {
                        msg = "universalDS - Empezar proceso de importación de estancias",
                        fecha = DateTime.Now
                    })
                });
                await universalDS.ImportarEstancias(cierres);     //Universal
                db.tblLogError.Add(new tblLogError
                {
                    denominacion = "EstanciasWorker - Importar",
                    error = JsonConvert.SerializeObject(new
                    {
                        msg = "universalDS - Terminar proceso de importación de estancias",
                        fecha = DateTime.Now
                    })
                });
                
                db.SaveChanges();

                db.tblLogError.Add(new tblLogError
                {
                    denominacion = "EstanciasWorker - Importar",
                    error = JsonConvert.SerializeObject(new
                    {
                        msg = "macHotelsDS - Empezar proceso de importación de estancias",
                        fecha = DateTime.Now
                    })
                });
                await macHotelsDS.ImportarEstancias(cierres);      //MacHotels
                db.tblLogError.Add(new tblLogError
                {
                    denominacion = "EstanciasWorker - Importar",
                    error = JsonConvert.SerializeObject(new
                    {
                        msg = "macHotelsDS - Terminar proceso de importación de estancias",
                        fecha = DateTime.Now
                    })
                });

                // TODO: Importar estancias asíncronamente
                //List<Task> tasks = new()
                //{
                //    timonHotelDS.ImportarEstancias(cierres),    //Timon Hotel
                //    allsunDS.ImportarEstancias(cierres),        //Allsun
                //    fergusDS.ImportarEstancias(cierres),        //Fergus
                //    universalDS.ImportarEstancias(cierres),     //Universal
                //    macHotelsDS.ImportarEstancias(cierres)      //MacHotels
                //};

                //Task.WaitAll(tasks.ToArray());
            }
            // TODO: Controlar excepciones asíncronas
            //catch (AggregateException ae)
            //{
            //    ae.Handle(ex =>
            //    {
            //        if (ex is ImportarEstanciaException iex)
            //        {
            //            db.tblLogError.Add(new tblLogError
            //            {
            //                denominacion = "EstanciasWorker - Importar",
            //                error = iex.ToJson()
            //            });
            //            return true;
            //        }
            //        return false;
            //    });
            //}
            finally
            {
                db.tblLogError.Add(new tblLogError
                {
                    denominacion = "EstanciasWorker - Importar",
                    error = JsonConvert.SerializeObject(new
                    {
                        msg = "Terminar proceso de importación de estancias",
                        fecha = DateTime.Now
                    })
                });
                db.SaveChanges();
            }

        }

        static public void PlanificarPedidos(bdERP db, int? idEntidad = null)
        {
            int diasDesde = -1; // Días previos utilizados para calcular los nuevos pedidos, el valor mínimo es -2 para tener en cuenta 1 día a partir de ayer.
            int diasHasta = 1; // Cantidad de días - pedidos que se van a generar

            var fechaHasta = DateTime.Today.AddDays(diasHasta);
            var fechaDesde = DateTime.Today.AddDays(diasDesde);

            var entidades = db.tblEntidad.AsNoTracking()
                .Where(x => x.enablePlanificadorPedidos == true && (idEntidad == null || x.idEntidad == idEntidad))
                .Include(x => x.tblPrendaNEntidad_NuevoPedido)
                .Include(x => x.tblReparto.Where(r => r.fecha.Value.Date > fechaDesde.Date && r.fecha.Value.Date < DateTime.Now.Date))
                    .ThenInclude(x => x.tblPrendaNReparto)
                .Include(x => x.tblEstancia.Where(es => es.fecha.Date >= fechaDesde.Date && es.fecha.Date <= fechaHasta.Date));

            var prendas = db.tblPrenda
                .Where(x => x.eliminado == false && x.activo == true &&
                    entidades.SelectMany(x => x.tblPrendaNEntidad_NuevoPedido).Select(x => x.idPrenda).Contains(x.idPrenda)
                    )
                .ToDictionary(x => x.idPrenda, x => x.udsXBacPedido);

            var resultParameter = new SqlParameter
            {
                ParameterName = "@result",
                SqlDbType = SqlDbType.NVarChar,
                Direction = ParameterDirection.Output,
                Size = 8
            };

            db.Database.ExecuteSqlRaw("SET @result = (SELECT Logistica.EF_funCodigoPedido());", resultParameter);
            int lastCodigo = Int32.Parse((string)resultParameter.Value) - 1;

            for (int i = 0; i < diasHasta; i++)
            {
                var fecha = DateTime.Now.AddDays(i);
                var isHoy = fecha.Date == DateTime.Now.Date;

                foreach (var entidad in entidades)
                {
                    tblPedido pedido = db.tblPedido
                        .Include(x => x.tblPrendaNPedido)
                        .FirstOrDefault(x => x.isAutomatico == true && x.idEntidad == entidad.idEntidad && x.fecha.Date == fecha.Date) ??
                        new tblPedido
                        {
                            idEntidad = entidad.idEntidad,
                            fecha = DateTime.Now,
                            fechaRegistro = DateTime.Now,
                            observaciones = "Pedido autogenerado por el planificador",
                            idEstadoPedido = 1, // Pendiente
                            idTipoPedido = 1, // Normal
                            isCerrado = false,
                            porcentaje = 0,
                            isApp = false,
                            isAutomatico = true,
                        };

                    if (pedido.idEstadoPedido == 1)
                    {
                        if (pedido.idPedido > 0)
                        {
                            pedido.fechaRegistro = DateTime.Now;
                            db.tblPrendaNPedido.RemoveRange(pedido.tblPrendaNPedido);
                        }
                        else
                        {
                            lastCodigo++;
                            pedido.codigo = $"{lastCodigo}";
                        }

                        var tblPrendasNReparto = entidad.tblReparto
                            .SelectMany(x => x.tblPrendaNReparto)
                            .GroupBy(x => x.idPrenda);

                        int sumaEstanciasAnteriores = entidad.tblEstancia
                            .Where(x => isHoy ? x.fecha.Date < DateTime.Now.Date : x.fecha.Date == fecha.Date.AddDays(-1))
                            .Select(x => isHoy ? (x.estanciasReal > 0 ? x.estanciasReal : x.estanciasPrevistas) : x.estanciasPrevistas)
                            .Sum() ?? 0;

                        if (sumaEstanciasAnteriores > 0)
                        {
                            foreach (var prendaNEntidad in entidad.tblPrendaNEntidad_NuevoPedido)
                            {
                                decimal totalPrendaXEstancia = sumaEstanciasAnteriores * prendaNEntidad.ratio ?? 0;

                                int sumaUdsPrendaReparto = isHoy ? tblPrendasNReparto
                                    .Where(x => x.Key == prendaNEntidad.idPrenda)
                                    .Sum(x => x.Select(x => x.cantidad).Sum()) : 0;

                                decimal udsBacs = (totalPrendaXEstancia - sumaUdsPrendaReparto) / (prendas[prendaNEntidad.idPrenda] ?? 1);

                                int peticion = Convert.ToInt32(Math.Ceiling(udsBacs) * (prendas[prendaNEntidad.idPrenda] ?? 1));

                                pedido.tblPrendaNPedido.Add(new tblPrendaNPedido
                                {
                                    idPrenda = prendaNEntidad.idPrenda,
                                    peticion = peticion < 0 ? 0 : peticion,
                                });
                            }
                        }
                        else
                        {
                            pedido.isCerrado = true;
                        }

                        if (pedido.idPedido == 0) db.tblPedido.Add(pedido);
                    }
                }
            }

            db.SaveChanges();
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public class ImportarEstanciaException : AggregateException
        {
            public string compania { get; set; }
            public int? entidad { get; set; }
            public string urlConsumida { get; set; }
            public DateTime fecha { get; set; }

            public ImportarEstanciaException(string message, string compania, string urlConsumida, DateTime fecha, int? entidad = null) : base(message)
            {
                this.compania = compania;
                this.entidad = entidad;
                this.urlConsumida = urlConsumida;
                this.fecha = fecha;
            }

            public string ToJson()
            {
                return JsonConvert.SerializeObject(new
                {
                    compania,
                    entidad,
                    fecha,
                    urlConsumida,
                    Message
                });
            }
        }

    }
}