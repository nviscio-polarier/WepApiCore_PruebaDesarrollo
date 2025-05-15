using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Newtonsoft.Json.Serialization;
using WebApiCore.Class;
using WebApiCore.Class.bdERP.Incidencias;
using WebApiCore.Class.Proyectos.MyPolarier.RRHH;
using WebApiCore.Context;
using WebApiCore.Hubs;
using WebApiCore.Security;
using WebApiCore.Services;
using WebApiCore.Services.a3innuva;
using WebApiCore.Services.MyQuality;
//using WebApiCore.Services.MyRealBonus;
using WebApiCore.Services.VIPS;
using static WebApiCore.Controllers.ArticuloController;
using static WebApiCore.Controllers.Proyectos.Administracion.direccionesEntregaController;
using static WebApiCore.Controllers.SolicitudFiniquitoController;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddDbContext<bdERP>(opt => opt
    .UseSqlServer(builder.Configuration.GetConnectionString("bdERP"))
);

builder.Services.AddDbContext<bdGestionAplicaciones>(opt => opt
    .UseSqlServer(builder.Configuration.GetConnectionString("bdGestionAplicaciones"))
);

builder.Services.AddDbContext<bdMyAudit>(opt => opt
    .UseSqlServer(builder.Configuration.GetConnectionString("bdMyAudit"))
);

////ALEX
//builder.Services.AddScoped<CalculoTokenService>();
//builder.Services.AddScoped<PersonaVideoService>();


if (Utils.isProduccion()) // Si estamos en producción, se dan de alta los workers
{
    builder.Services.AddHostedService<SmartHub_Worker>();
    builder.Services.AddHostedService<EnergyHub_Worker>();
    builder.Services.AddHostedService<Estancias_Worker>();
    builder.Services.AddHostedService<PolarierTI_Worker>();
    builder.Services.AddHostedService<A3innuvaAuth_Worker>();
    builder.Services.AddHostedService<Personal_Worker>();
    builder.Services.AddHostedService<SAP_Worker>();
    builder.Services.AddHostedService<VIPS_Worker>();
    builder.Services.AddHostedService<DocumentosSAP_RD_Worker>();
    builder.Services.AddHostedService<AutoValidacion_Worker>();
    builder.Services.AddHostedService<TimbradoMX_Worker>();
    builder.Services.AddHostedService<Notificaciones_Worker>();
}

builder.Services.AddControllers()
    .AddNewtonsoftJson(x =>
    {
        x.SerializerSettings.ContractResolver = new DefaultContractResolver();
        x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    })
      .AddOData(opt =>
       {
           //opt.TimeZone = TimeZoneInfo.Utc;
           opt.AddRouteComponents("odata", GetEdmModel());
           opt.AddRouteComponents("odata/GestionAplicaciones", GetEdmModel_GestionAplicaciones());
           opt.AddRouteComponents("odata/MyPolarier/RRHH", GetEdmModel_RRHH());
           opt.AddRouteComponents("odata/Administracion", GetEdmModel_Administracion());
           opt.Select();
           opt.Filter();
           opt.Expand();
           opt.SetMaxTop(null);
           opt.Count();
           opt.OrderBy();
       }
    );

// configure DI for application services
builder.Services.AddScoped<IUserService_BasicAuth, UserService_BasicAuth>();
builder.Services.AddScoped<IUserService_ExternalAuth, UserService_ExternalAuth>();
builder.Services.AddSignalR();
builder.Services.AddControllers().AddXmlSerializerFormatters();

//Servicios externos
//Firebase
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ExternalServices/Firebase/apppolarier-7c2b4-firebase-adminsdk-eg48s-60de7b15e5.json"))
});

var app = builder.Build();
app.UseCors(x => x
        .SetIsOriginAllowed(origin => true)
       .AllowAnyMethod()
       .AllowAnyHeader()
       .AllowCredentials());

app.UseMiddleware<JwtMiddleware>();
app.UseMiddleware<BasicAuthMiddleware>();
app.UseMiddleware<ExternosAuthMiddleware>();

app.MapControllers();
app.MapHub<NotificacionesHub>("/hub/notificaciones");

app.Run();

static IEdmModel GetEdmModel()
{
    ODataConventionModelBuilder builder = new();

    #region Administracion
    builder.EntitySet<tblAdmAlbaranCompra>("tblAdmAlbaranCompra");
    builder.EntitySet<tblAdmAlbaranVenta>("tblAdmAlbaranVenta");
    builder.EntitySet<tblAdmPedidoCliente>("tblAdmPedidoCliente");
    builder.EntitySet<tblAdmPedidoProveedor>("tblAdmPedidoProveedor");
    builder.EntitySet<tblAdmPresupuestoVenta>("tblAdmPresupuestoVenta");
    builder.EntitySet<tblAdmTipoElemento>("tblAdmTipoElemento");
    builder.EntitySet<tblAdmAlbaran_Estado>("tblAdmAlbaran_Estado");
    builder.EntitySet<tblAdmCentroCoste>("tblAdmCentroCoste");
    builder.EntitySet<tblAdmElementoPEP>("tblAdmElementoPEP");
    builder.EntitySet<tblIvaNPais>("tblIvaNPais");
    builder.EntitySet<tblAdmPedido_Estado>("tblAdmPedido_Estado");
    builder.EntitySet<tblAdmPresupuestoVenta_Estado>("tblAdmPresupuestoVenta_Estado");
    builder.EntitySet<tblAdmTipoDescuento>("tblAdmTipoDescuento");
    builder.EntitySet<tblAdmTipoElemento>("tblAdmTipoElemento");
    builder.EntitySet<tblAdmTipoCambio>("tblAdmTipoCambio");
    builder.EntitySet<tblAdmFactura_Estado>("tblAdmFactura_Estado");
    builder.EntitySet<tblAdmTipoFactura>("tblAdmTipoFactura");
    builder.EntitySet<tblAdmFacturaCompra>("tblAdmFacturaCompra");
    builder.EntitySet<tblAdmFacturaVenta>("tblAdmFacturaVenta");
    builder.EntitySet<tblAdmTipoFactura>("tblAdmTipoFactura");
    builder.EntitySet<tblAdmBanco>("tblAdmBanco");
    builder.EntitySet<tblAdmTipoNCF>("tblAdmTipoNCF");
    builder.EntitySet<tblArticuloLenceria>("tblArticuloLenceria");
    builder.EntitySet<tblArticuloLibre>("tblArticuloLibre");
    builder.EntitySet<tblArticuloMaquinaria>("tblArticuloMaquinaria");
    builder.EntitySet<tblArticuloNAdmAlbaranCompra>("tblArticuloNAdmAlbaranCompra");
    builder.EntitySet<tblArticuloNAdmAlbaranVenta>("tblArticuloNAdmAlbaranVenta");
    builder.EntitySet<tblArticuloNAdmPedidoCliente>("tblArticuloNAdmPedidoCliente");
    builder.EntitySet<tblArticuloNAdmPedidoProveedor>("tblArticuloNAdmPedidoProveedor");
    builder.EntitySet<tblArticuloNAdmPresupuestoVenta>("tblArticuloNAdmPresupuestoVenta");
    builder.EntitySet<tblAdmTipoArticulo>("tblAdmTipoArticulo");
    builder.EntitySet<tblAdmCliente>("tblAdmCliente");
    builder.EntitySet<tblImagenNCliente>("tblImagenNCliente");
    builder.EntitySet<tblImagenNProveedor>("tblImagenNProveedor");
    builder.EntitySet<tblAdmProveedor>("tblAdmProveedor");
    builder.EntitySet<tblAdmFormaPago>("tblAdmFormaPago");
    builder.EntitySet<tblAdmCuentaContable>("tblAdmCuentaContable");
    builder.EntitySet<tblAdmCuentaBancaria>("tblAdmCuentaBancaria");
    builder.EntitySet<tblAdmCondicionPago>("tblAdmCondicionPago");


    #endregion

    #region General
    builder.EntitySet<tblLavanderia>("tblLavanderia");
    builder.EntitySet<tblCentroTrabajo>("tblCentroTrabajo");
    builder.EntitySet<tblEntidad>("tblEntidad");
    builder.EntitySet<tblCompañia>("tblCompañia");

    builder.EntitySet<tblCalendarioLavanderia>("tblCalendarioLavanderia");
    builder.EntityType<tblCalendarioLavanderia>().Collection.Action("fn_IU_tblCalendarioLavanderia");
    builder.EntitySet<tblCalendarioCentroTrabajo>("tblCalendarioCentroTrabajo");
    builder.EntityType<tblCalendarioCentroTrabajo>().Collection.Action("fn_IU_tblCalendarioCentroTrabajo");

    builder.EntitySet<tblHorarioRepartoNEntidad>("tblHorarioRepartoNEntidad");

    builder.EntitySet<tblTipoNotificacion>("tblTipoNotificacion");

    builder.EntitySet<tblAbono>("tblAbono");
    builder.EntitySet<tblCategoriaAbono>("tblCategoriaAbono");
    builder.EntitySet<tblTipoAbono>("tblTipoAbono");
    builder.EntitySet<tblEstadoSolicitudAbono>("tblEstadoSolicitudAbono");

    builder.EntitySet<tblPrenda>("tblPrenda");
    builder.EntitySet<tblElemTrans>("tblElemTrans");
    builder.EntitySet<tblColorTapa>("tblColorTapa");
    builder.EntitySet<tblMarcaTapa>("tblMarcaTapa");
    builder.EntitySet<tblPrendaNEntidad_NuevoPedido>("tblPrendaNEntidad_NuevoPedido");
    builder.EntitySet<tblFamilia>("tblFamilia");
    builder.EntitySet<tblTipoPrenda>("tblTipoPrenda");

    builder.EntitySet<tblCorreoAltaGestoriaNCentroLav>("tblCorreoAltaGestoriaNCentroLav");

    builder.EntitySet<tblCorreosNLav>("tblCorreosNLav");
    builder.EntitySet<tblTipoDocumento_Envio>("tblTipoDocumento_Envio");

    builder.EntitySet<tblPais>("tblPais");
    builder.EntitySet<tblComunidadAutonoma>("tblComunidadAutonoma");
    builder.EntitySet<tblLocalizacion>("tblLocalizacion");
    builder.EntitySet<tblDocumentoPrenda>("tblDocumentoPrenda");
    builder.EntitySet<tblCierreFactEntidad>("tblCierreFactEntidad");
    builder.EntitySet<tblTipoConsumoLenceria>("tblTipoConsumoLenceria");
    builder.EntitySet<tblTipoFacturacionCliente>("tblTipoFacturacionCliente");

    builder.EntitySet<tblAreaLavanderia>("tblAreaLavanderia");
    builder.EntitySet<tblAreaLavanderiaNLavanderia>("tblAreaLavanderiaNLavanderia");
    builder.EntitySet<tblPosicionNAreaLavanderiaNLavanderia>("tblPosicionNAreaLavanderiaNLavanderia");
    builder.EntitySet<tblArchivo>("tblArchivo");
    builder.EntitySet<tblMoneda>("tblMoneda");
    builder.EntitySet<tblTasaCambio>("tblTasaCambio");

    builder.EntitySet<tblMarcaTapa>("tblMarcaTapa");
    builder.EntitySet<tblGrupoPlantillaPrenda_generica>("tblGrupoPlantillaPrenda_generica");
    builder.EntitySet<tblPlantillaPrenda_generica>("tblPlantillaPrenda_generica");
    builder.EntitySet<tblTipoAlmacenajeLimpio>("tblTipoAlmacenajeLimpio");

    #endregion

    #region Gestoria

    builder.EntitySet<tblSolicitudAlta>("tblSolicitudAlta");
    builder.EntitySet<tblEstadoSolicitudAlta>("tblEstadoSolicitudAlta");
    builder.EntitySet<tblDocumentoNSolicitudAlta>("tblDocumentoNSolicitudAlta");
    builder.EntitySet<tblTipoDocumento>("tblTipoDocumento");

    #endregion

    #region Facturación

    builder.EntitySet<tblCierreDatos_Facturacion>("tblCierreDatos_Facturacion");
    EntityTypeConfiguration<tblCierreDatos_Facturacion> ETC_tblCierreDatos_Facturacion = builder.EntityType<tblCierreDatos_Facturacion>();
    ETC_tblCierreDatos_Facturacion.Property(c => c.idEntidad).Order = 1;
    ETC_tblCierreDatos_Facturacion.Property(c => c.año).Order = 2;
    ETC_tblCierreDatos_Facturacion.Property(c => c.mes).Order = 3;


    builder.EntitySet<tblCierreDatos_Lavanderia>("tblCierreDatos_Lavanderia");
    EntityTypeConfiguration<tblCierreDatos_Lavanderia> ETC_tblCierreDatos_Lavanderia = builder.EntityType<tblCierreDatos_Lavanderia>();
    ETC_tblCierreDatos_Lavanderia.Property(c => c.idLavanderia).Order = 1;
    ETC_tblCierreDatos_Lavanderia.Property(c => c.año).Order = 2;
    ETC_tblCierreDatos_Lavanderia.Property(c => c.mes).Order = 3;

    builder.EntitySet<tblCierreDatos_Valet>("tblCierreDatos_Valet");
    EntityTypeConfiguration<tblCierreDatos_Valet> ETC_tblCierreDatos_Valet = builder.EntityType<tblCierreDatos_Valet>();
    ETC_tblCierreDatos_Valet.Property(c => c.idLavanderia).Order = 1;
    ETC_tblCierreDatos_Valet.Property(c => c.año).Order = 2;
    ETC_tblCierreDatos_Valet.Property(c => c.mes).Order = 3;

    builder.EntitySet<tblCierreDatos_Uniformidad>("tblCierreDatos_Uniformidad");
    EntityTypeConfiguration<tblCierreDatos_Uniformidad> ETC_tblCierreDatos_Uniformidad = builder.EntityType<tblCierreDatos_Uniformidad>();
    ETC_tblCierreDatos_Uniformidad.Property(c => c.idLavanderia).Order = 1;
    ETC_tblCierreDatos_Uniformidad.Property(c => c.año).Order = 2;
    ETC_tblCierreDatos_Uniformidad.Property(c => c.mes).Order = 3;

    #endregion

    #region Finanzas

    builder.EntitySet<tblEmpresasPolarier>("tblEmpresasPolarier");

    #endregion

    #region Control presupuestario

    builder.EntitySet<tblComentarioNCuentaContable>("tblComentarioNCuentaContable");
    builder.EntitySet<tblPresupuestoKg>("tblPresupuestoKg");

    #endregion

    #region Gestión Interna
    builder.EntitySet<tblUsuario>("tblUsuario");
    builder.EntitySet<tblFormularioNUsuario>("tblFormularioNUsuario");
    builder.EntitySet<tblLogError>("tblLogError");
    builder.EntitySet<tblNotificacion>("tblNotificacion");
    builder.EntitySet<tblNotificacion_Evento>("tblNotificacion_Evento");
    builder.EntitySet<tblLayout_SmartView>("tblLayout_SmartView");
    builder.EntitySet<tblPermiso>("tblPermiso");
    builder.EntitySet<tblLogAcciones_App>("tblLogAcciones_App");
    #endregion

    #region RRHH
    builder.EntitySet<tblPersona>("tblPersona");
    builder.EntitySet<tblPersona_PeticionCambioDatos>("tblPersona_PeticionCambioDatos");
    builder.EntitySet<tblPersonaNTipoContrato>("tblPersonaNTipoContrato");
    builder.EntityType<tblPersona>().Collection.Action("SendMail_DatosPersonales");

    builder.EntitySet<tblCategoria>("tblCategoria");
    builder.EntitySet<tblTurno>("tblTurno");
    builder.EntitySet<tblTipoContrato>("tblTipoContrato");
    builder.EntitySet<tblMotivoBaja>("tblMotivoBaja");

    builder.EntityType<tblPersona>().Collection.Action("fn_importPersonas");
    builder.EntityType<tblPersona>().Collection.Action("fn_isNumDocIdentidadPersonaExists");
    builder.EntityType<tblPersona>().Collection.Action("fn_isEmailExists");
    builder.EntityType<tblPersona>().Collection.Action("fn_CheckCodigoGestoria");
    builder.EntityType<tblPersona>().Collection.Action("fn_SendMail_emailBienvenida_masivo");

    builder.EntitySet<tblTipoTrabajo>("tblTipoTrabajo");
    builder.EntitySet<tblNivelEstudios>("tblNivelEstudios");
    builder.EntitySet<tblEstadoCivil>("tblEstadoCivil");
    builder.EntitySet<tblTipoDocumentoIdentidad>("tblTipoDocumentoIdentidad");
    builder.EntitySet<tblGenero>("tblGenero");
    builder.EntitySet<tblTallaAlfa>("tblTallaAlfa");
    builder.EntitySet<tblDiscapacidad>("tblDiscapacidad");
    builder.EntitySet<tblComunicado>("tblComunicado");

    builder.EntitySet<tblComunicadoNPersona>("tblComunicadoNPersona");
    EntityTypeConfiguration<tblComunicadoNPersona> ETC_tblComunicadoNPersona = builder.EntityType<tblComunicadoNPersona>();
    ETC_tblComunicadoNPersona.Property(c => c.idPersona).Order = 1;
    ETC_tblComunicadoNPersona.Property(c => c.idComunicado).Order = 2;

    builder.EntitySet<tblCalendarioPersonal>("tblCalendarioPersonal");
    builder.EntitySet<tblCalendario_Estado>("tblCalendario_Estado");
    builder.EntityType<tblCalendarioPersonal>().Collection.Action("fn_IU_tblCalendarioPersonal");
    builder.EntitySet<tblComunidadAutonoma>("tblComunidadAutonoma");
    builder.EntitySet<tblCuadrantePersonal>("tblCuadrantePersonal");
    // builder.EntityType<tblCuadrantePersonal>().Collection.Action("PostMasivo");
    // builder.EntityType<tblCuadrantePersonal>().Collection.Action("CambiarPosicion");

    builder.EntitySet<tblCarpetaDocumentos>("tblCarpetaDocumentos");
    builder.EntitySet<tblDocumento>("tblDocumento");
    builder.EntityType<tblDocumento>().Collection.Action("Sign");
    builder.EntityType<tblDocumento>().Collection.Action("fn_importacionMasivaDocumentos");
    builder.EntityType<tblDocumento>().Collection.Action("PostMasivo");
    builder.EntityType<tblDocumento>().Collection.Action("PostMasivoMultDoc");

    builder.EntitySet<tblFormatoDiasLibres>("tblFormatoDiasLibres");
    builder.EntitySet<tblDiasLibresPersonal>("tblDiasLibresPersonal");
    builder.EntityType<tblDiasLibresPersonal>().Collection.Action("PostDiasLibres");

    builder.EntitySet<tblLlamamiento>("tblLlamamiento");
    builder.EntitySet<tblDiasLibresPersonal_Llamamiento>("tblDiasLibresPersonal_Llamamiento");

    builder.EntitySet<tblJornada>("tblJornada");
    builder.EntitySet<tblJornadaPersona>("tblJornadaPersona");
    builder.EntitySet<tblCategoriaConvenio>("tblCategoriaConvenio");
    builder.EntitySet<tblCategoriaInterna>("tblCategoriaInterna");

    builder.EntitySet<tblMotivoIncumplimientoJornada>("tblMotivoIncumplimientoJornada");
    builder.EntitySet<tblNomina>("tblNomina");
    builder.EntitySet<tblEstadoNomina>("tblEstadoNomina");
    builder.EntitySet<tblEstadoNominaNNomina>("tblEstadoNominaNNomina");
    builder.EntitySet<tblConceptoNominaNNomina>("tblConceptoNominaNNomina");

    builder.EntitySet<tblHistoricoAsientoNomina>("tblHistoricoAsientoNomina");
    builder.EntitySet<tblHistoricoAsientoNomina_RD>("tblHistoricoAsientoNomina_RD");
    builder.EntitySet<tblHistoricoAsientoNomina_MX>("tblHistoricoAsientoNomina_MX");
    builder.EntitySet<tblTipoNomina_MX>("tblTipoNomina_MX");
    builder.EntitySet<tblTipoNomina_RD>("tblTipoNomina_RD");

    builder.EntitySet<tblBalanceHoras>("tblBalanceHoras");
    builder.EntitySet<tblBalanceHorasExtra>("tblBalanceHorasExtra");

    builder.EntitySet<tblDocumentoNNomina>("tblDocumentoNNomina");
    builder.EntitySet<tblLicenciaConducir>("tblLicenciaConducir");
    builder.EntitySet<tblLicenciaConducirNPersona_PeticionCambioDatos>("tblLicenciaConducirNPersona_PeticionCambioDatos");

    #endregion

    #region Logística

    builder.EntitySet<tblPedido>("tblPedido");
    builder.EntitySet<tblPrendaNPedido>("tblPrendaNPedido");
    builder.EntitySet<tblTipoPedido>("tblTipoPedido");
    builder.EntitySet<tblEstadoPedido>("tblEstadoPedido");

    builder.EntitySet<tblReparto>("tblReparto");
    builder.EntitySet<tblPrendaNReparto>("tblPrendaNReparto").EntityType.HasKey(x => new { x.idPrenda, x.idReparto });
    builder.EntitySet<tblRepartoEstado>("tblRepartoEstado");

    builder.EntitySet<tblSolicitudAbono>("tblSolicitudAbono");
    builder.EntitySet<tblPrendaNSolicitudAbono>("tblPrendaNSolicitudAbono");
    builder.EntitySet<tblAbono>("tblAbono");
    builder.EntitySet<tblCategoriaAbono>("tblCategoriaAbono");

    builder.EntitySet<tblEnvio>("tblEnvio");
    builder.EntitySet<tblPackingList>("tblPackingList");
    builder.EntitySet<tblFotoNArticuloEnvio>("tblFotoNArticuloEnvio");
    builder.EntitySet<tblPuerto>("tblPuerto");
    builder.EntitySet<tblIncoterm>("tblIncoterm");
    builder.EntitySet<tblProyecto>("tblProyecto");
    builder.EntitySet<tblDestinatario>("tblDestinatario");
    builder.EntitySet<tblEmbarcador>("tblEmbarcador");
    builder.EntitySet<tblTipoContenedor>("tblTipoContenedor");
    builder.EntitySet<tblEnvio_Documento>("tblEnvio_Documento");

    builder.EntitySet<tblRutaExpedicion>("tblRutaExpedicion");
    builder.EntitySet<tblEntidadNRutaExpedicion>("tblEntidadNRutaExpedicion");
    builder.EntitySet<tblVehiculo>("tblVehiculo");
    builder.EntitySet<tblParteTransporte>("tblParteTransporte");
    builder.EntitySet<tblParadaNRutaExpedicion>("tblParadaNRutaExpedicion");
    builder.EntitySet<tblParadaNParteTransporte>("tblParadaNParteTransporte");
    builder.EntitySet<tblMotivoPausa>("tblMotivoPausa");
    builder.EntitySet<tblParteTransporte_Localizacion>("tblParteTransporte_Localizacion");

    builder.EntitySet<tblPrendaNLavanderia>("tblPrendaNLavanderia");

    builder.EntitySet<tblTipoElemLog>("tblTipoElemLog");

    builder.EntitySet<tblEstadoMovimientoElemLog>("tblEstadoMovimientoElemLog");
    builder.EntitySet<tblMovimientoElemLog>("tblMovimientoElemLog");
    builder.EntitySet<tblCantidadNMovimientoElemLog>("tblCantidadNMovimientoElemLog");
    builder.EntitySet<tblMezclaSucioCliente>("tblMezclaSucioCliente");

    builder.EntitySet<tblGrupoPrendaEst>("tblGrupoPrendaEst");

    builder.EntitySet<tblStockTipoElemLogNEntidad>("tblStockTipoElemLogNEntidad");
    EntityTypeConfiguration<tblStockTipoElemLogNEntidad> ETC_tblStockTipoElemLogNEntidad = builder.EntityType<tblStockTipoElemLogNEntidad>();
    ETC_tblStockTipoElemLogNEntidad.Property(c => c.idEntidad).Order = 1;
    ETC_tblStockTipoElemLogNEntidad.Property(c => c.idTipoElemLog).Order = 2;

    builder.EntitySet<tblElemLogNPedido>("tblElemLogNPedido");
    EntityTypeConfiguration<tblElemLogNPedido> ETC_tblElemLogNPedido = builder.EntityType<tblElemLogNPedido>();
    ETC_tblElemLogNPedido.Property(c => c.idPedido).Order = 1;
    ETC_tblElemLogNPedido.Property(c => c.idTipoElemLog).Order = 2;

    #endregion

    #region MyRealData
    builder.EntitySet<tblLecturaContador>("tblLecturaContador");
    builder.EntityType<tblLecturaContador>().Collection.Action("PostMasivo");
    builder.EntitySet<tblLogConexiones>("tblLogConexiones");
    builder.EntityType<tblLogConexiones>().Collection.Action("PostMasivo");
    builder.EntitySet<tblEstadoSmartHubNMaquina>("tblEstadoSmartHubNMaquina");
    builder.EntitySet<tblClienteNMaquina>("tblClienteNMaquina");

    builder.EntitySet<tblEventoPersona_Estado>("tblEventoPersona_Estado");

    builder.EntitySet<tblPersonaNAreaNLavanderia>("tblPersonaNAreaNLavanderia");
    #endregion

    #region MyReporting
    #endregion

    #region MyValet
    builder.EntitySet<tblPrendaEjecutivo>("tblPrendaEjecutivo");
    #endregion

    #region ControlCalidad
    builder.EntitySet<tblPregunta>("tblPregunta");
    builder.EntitySet<tblRespuesta>("tblRespuesta");
    builder.EntitySet<tblEncuestaPlantilla>("tblEncuestaPlantilla");
    builder.EntitySet<tblCampañaEncuesta>("tblCampañaEncuesta");
    builder.EntitySet<tblEncuesta>("tblEncuesta");

    builder.EntitySet<tblArea>("tblArea");
    builder.EntitySet<tblAmbito>("tblAmbito");
    builder.EntitySet<tblSubAmbito>("tblSubAmbito");
    builder.EntitySet<tblPuntoRevision>("tblPuntoRevision");
    #endregion

    #region Producción
    builder.EntitySet<tblLecturaLavadoras>("tblLecturaLavadoras");
    builder.EntitySet<tblKgLavadosTunel>("tblKgLavadosTunel");
    builder.EntitySet<tblKgLavadosLavadora>("tblKgLavadosLavadora");
    #endregion

    #region Maquinaria

    builder.EntitySet<tblMaquina>("tblMaquina");
    builder.EntitySet<tblPrendasHora>("tblPrendasHora");
    builder.EntitySet<tblTareaMantenimientoPrev>("tblTareaMantenimientoPrev");
    builder.EntitySet<tblSistemaMaquina>("tblSistemaMaquina");
    builder.EntitySet<tblPlantillaTareaMantenimientoPrev>("tblPlantillaTareaMantenimientoPrev");
    builder.EntitySet<tblTareaMantenimientoPrev>("tblTareaMantenimientoPrev");

    #endregion

    #region Assistant

    builder.EntitySet<tblProveedor>("tblProveedor");
    builder.EntitySet<tblParteTrabajo>("tblParteTrabajo");
    builder.EntitySet<tblIncidenciaNParte>("tblIncidenciaNParte");
    builder.EntitySet<tblPersonasNParte>("tblPersonasNParte");
    builder.EntitySet<tblTipoSalidaRecambio>("tblTipoSalidaRecambio");
    builder.EntitySet<tblAlmacenRecambios>("tblAlmacenRecambios");
    builder.EntitySet<tblRecambio>("tblRecambio");
    builder.EntitySet<tblAlmacenRecambios>("tblAlmacenRecambios");
    builder.EntitySet<tblRecambioNParteTrabajo>("tblRecambioNParteTrabajo");
    builder.EntitySet<tblRecambioNAlmacenRecambios>("tblRecambioNAlmacenRecambios");
    builder.EntitySet<tblRecambioNProveedor>("tblRecambioNProveedor");
    builder.EntitySet<tblCierreRecambioNAlmacen>("tblCierreRecambioNAlmacen");
    builder.EntitySet<tblMovimientoRecambio>("tblMovimientoRecambio");
    builder.EntitySet<tblMantenimientoPrev>("tblMantenimientoPrev");
    builder.EntitySet<tblAlmacenRecambiosNPersona>("tblAlmacenRecambiosNPersona");
    builder.EntityType<tblMovimientoRecambio>().Collection.Action("fn_isNumPedidoAsociadoExists");
    builder.EntityType<tblMovimientoRecambio>().Collection.Action("fn_isNumRegistroExists");

    #endregion

    #region Incidencias
    builder.EntitySet<tblIncidencia>("tblIncidencia");
    builder.EntitySet<tblTipoSubIncidencia>("tblTipoSubIncidencia");
    builder.EntitySet<tblTipoIncidencia>("tblTipoIncidencia");
    builder.EntitySet<TipoIncidenciaNLavanderia>("TipoIncidenciaNLavanderia");
    builder.EntitySet<tblIncidencia_Documento>("tblIncidencia_Documento");
    #endregion

    #region Energeticos

    builder.EntitySet<tblCategoriaRecurso>("tblCategoriaRecurso");
    builder.EntitySet<tblGrupoEnergetico>("tblGrupoEnergetico");
    builder.EntitySet<tblRecursoContador>("tblRecursoContador");
    builder.EntitySet<tblControlContador>("tblControlContador");
    builder.EntitySet<tblUnidadMedida>("tblUnidadMedida");

    #endregion

    #region AppComercial
    builder.EntitySet<tblTipoReunion>("tblTipoReunion");
    builder.EntitySet<tblFormatoReunion>("tblFormatoReunion");
    builder.EntitySet<tblReunion>("tblReunion");
    builder.EntitySet<tblIncidenciaNReunion>("tblIncidenciaNReunion");
    builder.EntitySet<tblParticipantesNReunion>("tblParticipantesNReunion");
    builder.EntitySet<tblIncidenciaNReunion>("tblIncidenciaNReunion");
    #endregion

    #region Office
    builder.EntitySet<tblPrendaNRevision>("tblPrendaNRevision");
    builder.EntitySet<tblPrendaNPedidoExtra>("tblPrendaNPedidoExtra");
    #endregion

    #region Inventarios
    builder.EntitySet<tblMovimiento>("tblMovimiento");
    builder.EntitySet<tblTipoMovimiento>("tblTipoMovimiento");
    builder.EntitySet<tblInventario>("tblInventario");
    builder.EntitySet<tblInventario_Documento>("tblInventario_Documento");
    builder.EntitySet<tblGrupoInventario_generico>("tblGrupoInventario_generico");
    #endregion

    #region Control Presupuestario

    builder.EntitySet<tblAjustePresupuestario>("tblAjustePresupuestario");
    builder.EntitySet<tblTasaCambioPresupuesto>("tblTasaCambioPresupuesto");

    #endregion

    #region MyQuality

    builder.EntitySet<tblGestionRetiro>("tblGestionRetiro");
    builder.EntitySet<tblTipoRetiro>("tblTipoRetiro");
    builder.EntitySet<tblPrendaNGestionRetiro>("tblPrendaNGestionRetiro");

    #endregion

    return builder.GetEdmModel();
}

static IEdmModel GetEdmModel_Administracion()
{

    ODataConventionModelBuilder builder = new();

    builder.EntityType<DescripcionArticulo>().Collection.Function("GetAsociativaArticulo")
    .ReturnsCollectionFromEntitySet<DescripcionArticulo>("DescripcionArticulo");

    builder.EntityType<DireccionEntrega>().Collection.Function("GetDireccionesEntrega")
        .ReturnsCollectionFromEntitySet<DireccionEntrega>("DireccionEntrega");

    return builder.GetEdmModel();
}

static IEdmModel GetEdmModel_RRHH()
{

    ODataConventionModelBuilder builder = new();

    builder.EntityType<PersonaLlamamiento>().Collection.Function("GetPersonasDiscontinuas")
        .ReturnsCollectionFromEntitySet<PersonaLlamamiento>("Llamamiento");

    builder.EntityType<ModeloFiniquito>().Collection.Function("Get")
        .ReturnsCollectionFromEntitySet<ModeloFiniquito>("SolicitudFiniquito");

    return builder.GetEdmModel();
}

static IEdmModel GetEdmModel_GestionAplicaciones()
{
    ODataConventionModelBuilder builder = new();

    builder.EntitySet<tblAplicacionesNPantallas>("tblAplicacionesNPantallas");
    builder.EntitySet<tblConfigPTMYPR>("tblConfigPTMYPR");

    return builder.GetEdmModel();
}
