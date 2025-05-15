using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApiCore.Context
{
    public partial class bdERP : DbContext
    {
        public bdERP()
        {
        }

        public bdERP(DbContextOptions<bdERP> options)
            : base(options)
        {
        }

        public virtual DbSet<bdERP_alex> bdERP_alex { get; set; } = null!;
        public virtual DbSet<bdERP_joan> bdERP_joan { get; set; } = null!;
        public virtual DbSet<bdERP_nico> bdERP_nico { get; set; } = null!;
        public virtual DbSet<tbPeticionCambioDatos_estado> tbPeticionCambioDatos_estado { get; set; } = null!;
        public virtual DbSet<tblAbono> tblAbono { get; set; } = null!;
        public virtual DbSet<tblAccionUsuario> tblAccionUsuario { get; set; } = null!;
        public virtual DbSet<tblAccionesCorrectivas> tblAccionesCorrectivas { get; set; } = null!;
        public virtual DbSet<tblAdmAlbaranCompra> tblAdmAlbaranCompra { get; set; } = null!;
        public virtual DbSet<tblAdmAlbaranVenta> tblAdmAlbaranVenta { get; set; } = null!;
        public virtual DbSet<tblAdmAlbaran_Estado> tblAdmAlbaran_Estado { get; set; } = null!;
        public virtual DbSet<tblAdmArticulo> tblAdmArticulo { get; set; } = null!;
        public virtual DbSet<tblAdmArticuloNAdmAlbaranCompra> tblAdmArticuloNAdmAlbaranCompra { get; set; } = null!;
        public virtual DbSet<tblAdmArticuloNAdmAlbaranVenta> tblAdmArticuloNAdmAlbaranVenta { get; set; } = null!;
        public virtual DbSet<tblAdmArticuloNAdmPedidoCliente> tblAdmArticuloNAdmPedidoCliente { get; set; } = null!;
        public virtual DbSet<tblAdmArticuloNAdmPedidoProveedor> tblAdmArticuloNAdmPedidoProveedor { get; set; } = null!;
        public virtual DbSet<tblAdmArticuloNAdmPresupuestoVenta> tblAdmArticuloNAdmPresupuestoVenta { get; set; } = null!;
        public virtual DbSet<tblAdmBanco> tblAdmBanco { get; set; } = null!;
        public virtual DbSet<tblAdmCentroBeneficio> tblAdmCentroBeneficio { get; set; } = null!;
        public virtual DbSet<tblAdmCentroCoste> tblAdmCentroCoste { get; set; } = null!;
        public virtual DbSet<tblAdmCliente> tblAdmCliente { get; set; } = null!;
        public virtual DbSet<tblAdmConceptoCompra> tblAdmConceptoCompra { get; set; } = null!;
        public virtual DbSet<tblAdmConceptoVenta> tblAdmConceptoVenta { get; set; } = null!;
        public virtual DbSet<tblAdmCondicionPago> tblAdmCondicionPago { get; set; } = null!;
        public virtual DbSet<tblAdmCuentaBancaria> tblAdmCuentaBancaria { get; set; } = null!;
        public virtual DbSet<tblAdmCuentaContable> tblAdmCuentaContable { get; set; } = null!;
        public virtual DbSet<tblAdmElementoPEP> tblAdmElementoPEP { get; set; } = null!;
        public virtual DbSet<tblAdmFacturaCompra> tblAdmFacturaCompra { get; set; } = null!;
        public virtual DbSet<tblAdmFacturaVenta> tblAdmFacturaVenta { get; set; } = null!;
        public virtual DbSet<tblAdmFactura_Estado> tblAdmFactura_Estado { get; set; } = null!;
        public virtual DbSet<tblAdmFormaPago> tblAdmFormaPago { get; set; } = null!;
        public virtual DbSet<tblAdmIva> tblAdmIva { get; set; } = null!;
        public virtual DbSet<tblAdmPedidoCliente> tblAdmPedidoCliente { get; set; } = null!;
        public virtual DbSet<tblAdmPedidoProveedor> tblAdmPedidoProveedor { get; set; } = null!;
        public virtual DbSet<tblAdmPedido_Estado> tblAdmPedido_Estado { get; set; } = null!;
        public virtual DbSet<tblAdmPresupuestoVenta> tblAdmPresupuestoVenta { get; set; } = null!;
        public virtual DbSet<tblAdmPresupuestoVenta_Estado> tblAdmPresupuestoVenta_Estado { get; set; } = null!;
        public virtual DbSet<tblAdmProveedor> tblAdmProveedor { get; set; } = null!;
        public virtual DbSet<tblAdmTipoArticulo> tblAdmTipoArticulo { get; set; } = null!;
        public virtual DbSet<tblAdmTipoCambio> tblAdmTipoCambio { get; set; } = null!;
        public virtual DbSet<tblAdmTipoDescuento> tblAdmTipoDescuento { get; set; } = null!;
        public virtual DbSet<tblAdmTipoElemento> tblAdmTipoElemento { get; set; } = null!;
        public virtual DbSet<tblAdmTipoFactura> tblAdmTipoFactura { get; set; } = null!;
        public virtual DbSet<tblAdmTipoNCF> tblAdmTipoNCF { get; set; } = null!;
        public virtual DbSet<tblAjustePresupuestario> tblAjustePresupuestario { get; set; } = null!;
        public virtual DbSet<tblAlmacen> tblAlmacen { get; set; } = null!;
        public virtual DbSet<tblAlmacenNInventario> tblAlmacenNInventario { get; set; } = null!;
        public virtual DbSet<tblAlmacenNRuta> tblAlmacenNRuta { get; set; } = null!;
        public virtual DbSet<tblAlmacenRecambios> tblAlmacenRecambios { get; set; } = null!;
        public virtual DbSet<tblAlmacenRecambiosNPersona> tblAlmacenRecambiosNPersona { get; set; } = null!;
        public virtual DbSet<tblApartado> tblApartado { get; set; } = null!;
        public virtual DbSet<tblAplicacion> tblAplicacion { get; set; } = null!;
        public virtual DbSet<tblArchivo> tblArchivo { get; set; } = null!;
        public virtual DbSet<tblAreaLavanderia> tblAreaLavanderia { get; set; } = null!;
        public virtual DbSet<tblAreaLavanderiaNLavanderia> tblAreaLavanderiaNLavanderia { get; set; } = null!;
        public virtual DbSet<tblArticuloEnvio> tblArticuloEnvio { get; set; } = null!;
        public virtual DbSet<tblArticuloLenceria> tblArticuloLenceria { get; set; } = null!;
        public virtual DbSet<tblArticuloLogistico> tblArticuloLogistico { get; set; } = null!;
        public virtual DbSet<tblArticuloMaquinaria> tblArticuloMaquinaria { get; set; } = null!;
        public virtual DbSet<tblArticuloNAdmAlbaranCompra> tblArticuloNAdmAlbaranCompra { get; set; } = null!;
        public virtual DbSet<tblArticuloNAdmAlbaranVenta> tblArticuloNAdmAlbaranVenta { get; set; } = null!;
        public virtual DbSet<tblArticuloNAdmPedidoCliente> tblArticuloNAdmPedidoCliente { get; set; } = null!;
        public virtual DbSet<tblArticuloNAdmPedidoProveedor> tblArticuloNAdmPedidoProveedor { get; set; } = null!;
        public virtual DbSet<tblArticuloNAdmPresupuestoVenta> tblArticuloNAdmPresupuestoVenta { get; set; } = null!;
        public virtual DbSet<tblBacsCarro> tblBacsCarro { get; set; } = null!;
        public virtual DbSet<tblBalanceHoras> tblBalanceHoras { get; set; } = null!;
        public virtual DbSet<tblBalanceHorasExtra> tblBalanceHorasExtra { get; set; } = null!;
        public virtual DbSet<tblCalendarioCentroTrabajo> tblCalendarioCentroTrabajo { get; set; } = null!;
        public virtual DbSet<tblCalendarioEntidad> tblCalendarioEntidad { get; set; } = null!;
        public virtual DbSet<tblCalendarioEntidad_Estados> tblCalendarioEntidad_Estados { get; set; } = null!;
        public virtual DbSet<tblCalendarioLavanderia> tblCalendarioLavanderia { get; set; } = null!;
        public virtual DbSet<tblCalendarioPersonal> tblCalendarioPersonal { get; set; } = null!;
        public virtual DbSet<tblCalendario_Estado> tblCalendario_Estado { get; set; } = null!;
        public virtual DbSet<tblCalendario_TipoEstado> tblCalendario_TipoEstado { get; set; } = null!;
        public virtual DbSet<tblCampañaEncuesta> tblCampañaEncuesta { get; set; } = null!;
        public virtual DbSet<tblCantidadNMovimientoElemLog> tblCantidadNMovimientoElemLog { get; set; } = null!;
        public virtual DbSet<tblCargo> tblCargo { get; set; } = null!;
        public virtual DbSet<tblCarpetaDocumentos> tblCarpetaDocumentos { get; set; } = null!;
        public virtual DbSet<tblCarro> tblCarro { get; set; } = null!;
        public virtual DbSet<tblCategoria> tblCategoria { get; set; } = null!;
        public virtual DbSet<tblCategoriaAbono> tblCategoriaAbono { get; set; } = null!;
        public virtual DbSet<tblCategoriaConvenio> tblCategoriaConvenio { get; set; } = null!;
        public virtual DbSet<tblCategoriaInterna> tblCategoriaInterna { get; set; } = null!;
        public virtual DbSet<tblCategoriaInternaNTurno> tblCategoriaInternaNTurno { get; set; } = null!;
        public virtual DbSet<tblCategoriaMaquina> tblCategoriaMaquina { get; set; } = null!;
        public virtual DbSet<tblCategoriaRecurso> tblCategoriaRecurso { get; set; } = null!;
        public virtual DbSet<tblCategoria_Grupo> tblCategoria_Grupo { get; set; } = null!;
        public virtual DbSet<tblCentroTrabajo> tblCentroTrabajo { get; set; } = null!;
        public virtual DbSet<tblCierreDatos_Facturacion> tblCierreDatos_Facturacion { get; set; } = null!;
        public virtual DbSet<tblCierreDatos_Lavanderia> tblCierreDatos_Lavanderia { get; set; } = null!;
        public virtual DbSet<tblCierreDatos_Uniformidad> tblCierreDatos_Uniformidad { get; set; } = null!;
        public virtual DbSet<tblCierreDatos_Valet> tblCierreDatos_Valet { get; set; } = null!;
        public virtual DbSet<tblCierreFactEntidad> tblCierreFactEntidad { get; set; } = null!;
        public virtual DbSet<tblCierrePresupuestario> tblCierrePresupuestario { get; set; } = null!;
        public virtual DbSet<tblCierreRecambioNAlmacen> tblCierreRecambioNAlmacen { get; set; } = null!;
        public virtual DbSet<tblClienteNMaquina> tblClienteNMaquina { get; set; } = null!;
        public virtual DbSet<tblColorPrendaHuesped> tblColorPrendaHuesped { get; set; } = null!;
        public virtual DbSet<tblColorTapa> tblColorTapa { get; set; } = null!;
        public virtual DbSet<tblComentarioNCuentaContable> tblComentarioNCuentaContable { get; set; } = null!;
        public virtual DbSet<tblCompañia> tblCompañia { get; set; } = null!;
        public virtual DbSet<tblComunicado> tblComunicado { get; set; } = null!;
        public virtual DbSet<tblComunicadoNPersona> tblComunicadoNPersona { get; set; } = null!;
        public virtual DbSet<tblComunidadAutonoma> tblComunidadAutonoma { get; set; } = null!;
        public virtual DbSet<tblConceptoNomina> tblConceptoNomina { get; set; } = null!;
        public virtual DbSet<tblConceptoNominaNNomina> tblConceptoNominaNNomina { get; set; } = null!;
        public virtual DbSet<tblConceptoNominaNNomina_Gestoria> tblConceptoNominaNNomina_Gestoria { get; set; } = null!;
        public virtual DbSet<tblConceptosFinancieros> tblConceptosFinancieros { get; set; } = null!;
        public virtual DbSet<tblConfigAlbaranReparto> tblConfigAlbaranReparto { get; set; } = null!;
        public virtual DbSet<tblConfiguracionSalarial> tblConfiguracionSalarial { get; set; } = null!;
        public virtual DbSet<tblContadorNivel1> tblContadorNivel1 { get; set; } = null!;
        public virtual DbSet<tblContadorNivel2> tblContadorNivel2 { get; set; } = null!;
        public virtual DbSet<tblControlAcceso> tblControlAcceso { get; set; } = null!;
        public virtual DbSet<tblControlContador> tblControlContador { get; set; } = null!;
        public virtual DbSet<tblControlNivel> tblControlNivel { get; set; } = null!;
        public virtual DbSet<tblCorporacion> tblCorporacion { get; set; } = null!;
        public virtual DbSet<tblCorreoAltaGestoriaNCentroLav> tblCorreoAltaGestoriaNCentroLav { get; set; } = null!;
        public virtual DbSet<tblCorreosNEntidad> tblCorreosNEntidad { get; set; } = null!;
        public virtual DbSet<tblCorreosNLav> tblCorreosNLav { get; set; } = null!;
        public virtual DbSet<tblCorreosNuevoRecambio> tblCorreosNuevoRecambio { get; set; } = null!;
        public virtual DbSet<tblCriterioValoracion> tblCriterioValoracion { get; set; } = null!;
        public virtual DbSet<tblCriterioValoracionNLavanderia> tblCriterioValoracionNLavanderia { get; set; } = null!;
        public virtual DbSet<tblCuadrantePersonal> tblCuadrantePersonal { get; set; } = null!;
        public virtual DbSet<tblCuentaContableNCentroTrabajo> tblCuentaContableNCentroTrabajo { get; set; } = null!;
        public virtual DbSet<tblCuentaContableNTipoTrabajo> tblCuentaContableNTipoTrabajo { get; set; } = null!;
        public virtual DbSet<tblDatosFinancieros> tblDatosFinancieros { get; set; } = null!;
        public virtual DbSet<tblDatosFinancierosNTrimestre> tblDatosFinancierosNTrimestre { get; set; } = null!;
        public virtual DbSet<tblDatosSalariales> tblDatosSalariales { get; set; } = null!;
        public virtual DbSet<tblDatosSalariales_historico_salarioBase> tblDatosSalariales_historico_salarioBase { get; set; } = null!;
        public virtual DbSet<tblDefectoPrendaHuesped> tblDefectoPrendaHuesped { get; set; } = null!;
        public virtual DbSet<tblDenoPrenda> tblDenoPrenda { get; set; } = null!;
        public virtual DbSet<tblDestinatario> tblDestinatario { get; set; } = null!;
        public virtual DbSet<tblDetalleNNomina> tblDetalleNNomina { get; set; } = null!;
        public virtual DbSet<tblDiaSemana> tblDiaSemana { get; set; } = null!;
        public virtual DbSet<tblDiasCuadrante> tblDiasCuadrante { get; set; } = null!;
        public virtual DbSet<tblDiasLibresPersonal> tblDiasLibresPersonal { get; set; } = null!;
        public virtual DbSet<tblDiasLibresPersonal_Llamamiento> tblDiasLibresPersonal_Llamamiento { get; set; } = null!;
        public virtual DbSet<tblDiscapacidad> tblDiscapacidad { get; set; } = null!;
        public virtual DbSet<tblDocumento> tblDocumento { get; set; } = null!;
        public virtual DbSet<tblDocumentoNNomina> tblDocumentoNNomina { get; set; } = null!;
        public virtual DbSet<tblDocumentoNSolicitudAlta> tblDocumentoNSolicitudAlta { get; set; } = null!;
        public virtual DbSet<tblDocumentoPrenda> tblDocumentoPrenda { get; set; } = null!;
        public virtual DbSet<tblElemLogNPedido> tblElemLogNPedido { get; set; } = null!;
        public virtual DbSet<tblElemTrans> tblElemTrans { get; set; } = null!;
        public virtual DbSet<tblEmbarcador> tblEmbarcador { get; set; } = null!;
        public virtual DbSet<tblEmpresasPolarier> tblEmpresasPolarier { get; set; } = null!;
        public virtual DbSet<tblEncuesta> tblEncuesta { get; set; } = null!;
        public virtual DbSet<tblEncuestaPlantilla> tblEncuestaPlantilla { get; set; } = null!;
        public virtual DbSet<tblEnergyHub> tblEnergyHub { get; set; } = null!;
        public virtual DbSet<tblEntidad> tblEntidad { get; set; } = null!;
        public virtual DbSet<tblEntidadNInventario> tblEntidadNInventario { get; set; } = null!;
        public virtual DbSet<tblEntidadNRutaExpedicion> tblEntidadNRutaExpedicion { get; set; } = null!;
        public virtual DbSet<tblEntidad_historico_idTipoFacturacion> tblEntidad_historico_idTipoFacturacion { get; set; } = null!;
        public virtual DbSet<tblEnvio> tblEnvio { get; set; } = null!;
        public virtual DbSet<tblEnvio_Documento> tblEnvio_Documento { get; set; } = null!;
        public virtual DbSet<tblEstadoCivil> tblEstadoCivil { get; set; } = null!;
        public virtual DbSet<tblEstadoEnergyHub> tblEstadoEnergyHub { get; set; } = null!;
        public virtual DbSet<tblEstadoHistoricoAsientoNomina> tblEstadoHistoricoAsientoNomina { get; set; } = null!;
        public virtual DbSet<tblEstadoMovimientoElemLog> tblEstadoMovimientoElemLog { get; set; } = null!;
        public virtual DbSet<tblEstadoMovimientoRecambio> tblEstadoMovimientoRecambio { get; set; } = null!;
        public virtual DbSet<tblEstadoMovimientoRecambioNMovimientoRecambio> tblEstadoMovimientoRecambioNMovimientoRecambio { get; set; } = null!;
        public virtual DbSet<tblEstadoNomina> tblEstadoNomina { get; set; } = null!;
        public virtual DbSet<tblEstadoNominaNNomina> tblEstadoNominaNNomina { get; set; } = null!;
        public virtual DbSet<tblEstadoOffice> tblEstadoOffice { get; set; } = null!;
        public virtual DbSet<tblEstadoPedido> tblEstadoPedido { get; set; } = null!;
        public virtual DbSet<tblEstadoSmartHub> tblEstadoSmartHub { get; set; } = null!;
        public virtual DbSet<tblEstadoSmartHubNMaquina> tblEstadoSmartHubNMaquina { get; set; } = null!;
        public virtual DbSet<tblEstadoSolicitudAbono> tblEstadoSolicitudAbono { get; set; } = null!;
        public virtual DbSet<tblEstadoSolicitudAlta> tblEstadoSolicitudAlta { get; set; } = null!;
        public virtual DbSet<tblEstadoSolicitudAltaNSolicitudAlta> tblEstadoSolicitudAltaNSolicitudAlta { get; set; } = null!;
        public virtual DbSet<tblEstadoTag> tblEstadoTag { get; set; } = null!;
        public virtual DbSet<tblEstancia> tblEstancia { get; set; } = null!;
        public virtual DbSet<tblEventoPersona> tblEventoPersona { get; set; } = null!;
        public virtual DbSet<tblEventoPersona_Estado> tblEventoPersona_Estado { get; set; } = null!;
        public virtual DbSet<tblFabricante> tblFabricante { get; set; } = null!;
        public virtual DbSet<tblFamilia> tblFamilia { get; set; } = null!;
        public virtual DbSet<tblFormatoDiasLibres> tblFormatoDiasLibres { get; set; } = null!;
        public virtual DbSet<tblFormatoReunion> tblFormatoReunion { get; set; } = null!;
        public virtual DbSet<tblFormulario> tblFormulario { get; set; } = null!;
        public virtual DbSet<tblFormularioNUsuario> tblFormularioNUsuario { get; set; } = null!;
        public virtual DbSet<tblFotoNArticuloEnvio> tblFotoNArticuloEnvio { get; set; } = null!;
        public virtual DbSet<tblFrecuenciaMantenimiento> tblFrecuenciaMantenimiento { get; set; } = null!;
        public virtual DbSet<tblGenero> tblGenero { get; set; } = null!;
        public virtual DbSet<tblGestionRetiro> tblGestionRetiro { get; set; } = null!;
        public virtual DbSet<tblGrupoArticulos> tblGrupoArticulos { get; set; } = null!;
        public virtual DbSet<tblGrupoEmpresarial> tblGrupoEmpresarial { get; set; } = null!;
        public virtual DbSet<tblGrupoEnergetico> tblGrupoEnergetico { get; set; } = null!;
        public virtual DbSet<tblGrupoEntidad> tblGrupoEntidad { get; set; } = null!;
        public virtual DbSet<tblGrupoInventario_generico> tblGrupoInventario_generico { get; set; } = null!;
        public virtual DbSet<tblGrupoPlantillaPrenda_generica> tblGrupoPlantillaPrenda_generica { get; set; } = null!;
        public virtual DbSet<tblGrupoPregunta> tblGrupoPregunta { get; set; } = null!;
        public virtual DbSet<tblGrupoPrendaEst> tblGrupoPrendaEst { get; set; } = null!;
        public virtual DbSet<tblHistoricoAsientoNomina> tblHistoricoAsientoNomina { get; set; } = null!;
        public virtual DbSet<tblHistoricoAsientoNomina_MX> tblHistoricoAsientoNomina_MX { get; set; } = null!;
        public virtual DbSet<tblHistoricoAsientoNomina_RD> tblHistoricoAsientoNomina_RD { get; set; } = null!;
        public virtual DbSet<tblHistoricoNominas> tblHistoricoNominas { get; set; } = null!;
        public virtual DbSet<tblHistoricoPartidaContable> tblHistoricoPartidaContable { get; set; } = null!;
        public virtual DbSet<tblHistoricoPlanificacion> tblHistoricoPlanificacion { get; set; } = null!;
        public virtual DbSet<tblHorarioRepartoNEntidad> tblHorarioRepartoNEntidad { get; set; } = null!;
        public virtual DbSet<tblIdioma> tblIdioma { get; set; } = null!;
        public virtual DbSet<tblImagenNCliente> tblImagenNCliente { get; set; } = null!;
        public virtual DbSet<tblImagenNProveedor> tblImagenNProveedor { get; set; } = null!;
        public virtual DbSet<tblIncidencia> tblIncidencia { get; set; } = null!;
        public virtual DbSet<tblIncidenciaNParte> tblIncidenciaNParte { get; set; } = null!;
        public virtual DbSet<tblIncidenciaNReunion> tblIncidenciaNReunion { get; set; } = null!;
        public virtual DbSet<tblIncidencia_Documento> tblIncidencia_Documento { get; set; } = null!;
        public virtual DbSet<tblIncoterm> tblIncoterm { get; set; } = null!;
        public virtual DbSet<tblIngresosPresupuestados> tblIngresosPresupuestados { get; set; } = null!;
        public virtual DbSet<tblInventario> tblInventario { get; set; } = null!;
        public virtual DbSet<tblInventario_Documento> tblInventario_Documento { get; set; } = null!;
        public virtual DbSet<tblIvaNPais> tblIvaNPais { get; set; } = null!;
        public virtual DbSet<tblJornada> tblJornada { get; set; } = null!;
        public virtual DbSet<tblJornadaPersona> tblJornadaPersona { get; set; } = null!;
        public virtual DbSet<tblKgLavadosLavadora> tblKgLavadosLavadora { get; set; } = null!;
        public virtual DbSet<tblKgLavadosTunel> tblKgLavadosTunel { get; set; } = null!;
        public virtual DbSet<tblKgNPresupuestoKg> tblKgNPresupuestoKg { get; set; } = null!;
        public virtual DbSet<tblKgRealesNPresupuestoKg> tblKgRealesNPresupuestoKg { get; set; } = null!;
        public virtual DbSet<tblLavanderia> tblLavanderia { get; set; } = null!;
        public virtual DbSet<tblLayout_SmartView> tblLayout_SmartView { get; set; } = null!;
        public virtual DbSet<tblLecturaCarro> tblLecturaCarro { get; set; } = null!;
        public virtual DbSet<tblLecturaCarro_Estado> tblLecturaCarro_Estado { get; set; } = null!;
        public virtual DbSet<tblLecturaContador> tblLecturaContador { get; set; } = null!;
        public virtual DbSet<tblLecturaLavadoras> tblLecturaLavadoras { get; set; } = null!;
        public virtual DbSet<tblLibreMensual> tblLibreMensual { get; set; } = null!;
        public virtual DbSet<tblLibreSemanal> tblLibreSemanal { get; set; } = null!;
        public virtual DbSet<tblLicenciaConducir> tblLicenciaConducir { get; set; } = null!;
        public virtual DbSet<tblLlamamiento> tblLlamamiento { get; set; } = null!;
        public virtual DbSet<tblLocalizacion> tblLocalizacion { get; set; } = null!;
        public virtual DbSet<tblLog> tblLog { get; set; } = null!;
        public virtual DbSet<tblLogAcciones> tblLogAcciones { get; set; } = null!;
        public virtual DbSet<tblLogAcciones_App> tblLogAcciones_App { get; set; } = null!;
        public virtual DbSet<tblLogConexiones> tblLogConexiones { get; set; } = null!;
        public virtual DbSet<tblLogError> tblLogError { get; set; } = null!;
        public virtual DbSet<tblMantenimientoNMaquina> tblMantenimientoNMaquina { get; set; } = null!;
        public virtual DbSet<tblMantenimientoPrev> tblMantenimientoPrev { get; set; } = null!;
        public virtual DbSet<tblMaquina> tblMaquina { get; set; } = null!;
        public virtual DbSet<tblMarcaTapa> tblMarcaTapa { get; set; } = null!;
        public virtual DbSet<tblMes> tblMes { get; set; } = null!;
        public virtual DbSet<tblMesNAjustePresupuestario> tblMesNAjustePresupuestario { get; set; } = null!;
        public virtual DbSet<tblMezclaSucioCliente> tblMezclaSucioCliente { get; set; } = null!;
        public virtual DbSet<tblModeloImpresion_reparto> tblModeloImpresion_reparto { get; set; } = null!;
        public virtual DbSet<tblModulo> tblModulo { get; set; } = null!;
        public virtual DbSet<tblModuloNLavanderia_Ponderacion> tblModuloNLavanderia_Ponderacion { get; set; } = null!;
        public virtual DbSet<tblMoneda> tblMoneda { get; set; } = null!;
        public virtual DbSet<tblMotivoBaja> tblMotivoBaja { get; set; } = null!;
        public virtual DbSet<tblMotivoIncumplimientoJornada> tblMotivoIncumplimientoJornada { get; set; } = null!;
        public virtual DbSet<tblMotivoPausa> tblMotivoPausa { get; set; } = null!;
        public virtual DbSet<tblMovimiento> tblMovimiento { get; set; } = null!;
        public virtual DbSet<tblMovimientoElemLog> tblMovimientoElemLog { get; set; } = null!;
        public virtual DbSet<tblMovimientoRecambio> tblMovimientoRecambio { get; set; } = null!;
        public virtual DbSet<tblMovimientoTag> tblMovimientoTag { get; set; } = null!;
        public virtual DbSet<tblMuestreo> tblMuestreo { get; set; } = null!;
        public virtual DbSet<tblNivelCombustible> tblNivelCombustible { get; set; } = null!;
        public virtual DbSet<tblNivelEstudios> tblNivelEstudios { get; set; } = null!;
        public virtual DbSet<tblNodos> tblNodos { get; set; } = null!;
        public virtual DbSet<tblNomina> tblNomina { get; set; } = null!;
        public virtual DbSet<tblNomina_MX> tblNomina_MX { get; set; } = null!;
        public virtual DbSet<tblNomina_RD> tblNomina_RD { get; set; } = null!;
        public virtual DbSet<tblNotificacion> tblNotificacion { get; set; } = null!;
        public virtual DbSet<tblNotificacion_Estado> tblNotificacion_Estado { get; set; } = null!;
        public virtual DbSet<tblNotificacion_Evento> tblNotificacion_Evento { get; set; } = null!;
        public virtual DbSet<tblNotificaciones_TI> tblNotificaciones_TI { get; set; } = null!;
        public virtual DbSet<tblObjetivosKpi> tblObjetivosKpi { get; set; } = null!;
        public virtual DbSet<tblObjetivosKpiNRecursoNivel> tblObjetivosKpiNRecursoNivel { get; set; } = null!;
        public virtual DbSet<tblOpcion> tblOpcion { get; set; } = null!;
        public virtual DbSet<tblPackingList> tblPackingList { get; set; } = null!;
        public virtual DbSet<tblPais> tblPais { get; set; } = null!;
        public virtual DbSet<tblParadaNParteTransporte> tblParadaNParteTransporte { get; set; } = null!;
        public virtual DbSet<tblParadaNRutaExpedicion> tblParadaNRutaExpedicion { get; set; } = null!;
        public virtual DbSet<tblParamNConfigAlbaranReparto> tblParamNConfigAlbaranReparto { get; set; } = null!;
        public virtual DbSet<tblParametrosAlbaranReparto> tblParametrosAlbaranReparto { get; set; } = null!;
        public virtual DbSet<tblParteTrabajo> tblParteTrabajo { get; set; } = null!;
        public virtual DbSet<tblParteTrabajo1> tblParteTrabajo1 { get; set; } = null!;
        public virtual DbSet<tblParteTransporte> tblParteTransporte { get; set; } = null!;
        public virtual DbSet<tblParteTransporte_Estado> tblParteTransporte_Estado { get; set; } = null!;
        public virtual DbSet<tblParteTransporte_Localizacion> tblParteTransporte_Localizacion { get; set; } = null!;
        public virtual DbSet<tblParticipantesNReunion> tblParticipantesNReunion { get; set; } = null!;
        public virtual DbSet<tblPartidaNCierrePresupuestario> tblPartidaNCierrePresupuestario { get; set; } = null!;
        public virtual DbSet<tblPedido> tblPedido { get; set; } = null!;
        public virtual DbSet<tblPedidoHuesped> tblPedidoHuesped { get; set; } = null!;
        public virtual DbSet<tblPedidosExtra> tblPedidosExtra { get; set; } = null!;
        public virtual DbSet<tblPermiso> tblPermiso { get; set; } = null!;
        public virtual DbSet<tblPersona> tblPersona { get; set; } = null!;
        public virtual DbSet<tblPersonaContactoNProveedor> tblPersonaContactoNProveedor { get; set; } = null!;
        public virtual DbSet<tblPersonaCoste> tblPersonaCoste { get; set; } = null!;
        public virtual DbSet<tblPersonaNAreaNLavanderia> tblPersonaNAreaNLavanderia { get; set; } = null!;
        public virtual DbSet<tblPersonaNMaquina> tblPersonaNMaquina { get; set; } = null!;
        public virtual DbSet<tblPersonaNTipoContrato> tblPersonaNTipoContrato { get; set; } = null!;
        public virtual DbSet<tblPersona_PeticionCambioDatos> tblPersona_PeticionCambioDatos { get; set; } = null!;
        public virtual DbSet<tblPersonasNParte> tblPersonasNParte { get; set; } = null!;
        public virtual DbSet<tblPersonasNParte1> tblPersonasNParte1 { get; set; } = null!;
        public virtual DbSet<tblPesoIncidencia> tblPesoIncidencia { get; set; } = null!;
        public virtual DbSet<tblPesoNCategoriaNLavanderia> tblPesoNCategoriaNLavanderia { get; set; } = null!;
        public virtual DbSet<tblPesoNSistemaNLavanderia> tblPesoNSistemaNLavanderia { get; set; } = null!;
        public virtual DbSet<tblPlanificacionNCierrePresupuestario> tblPlanificacionNCierrePresupuestario { get; set; } = null!;
        public virtual DbSet<tblPlantillaPrenda_generica> tblPlantillaPrenda_generica { get; set; } = null!;
        public virtual DbSet<tblPlantillaPrenda_generica_historico_peso> tblPlantillaPrenda_generica_historico_peso { get; set; } = null!;
        public virtual DbSet<tblPlantillaTareaMantenimientoPrev> tblPlantillaTareaMantenimientoPrev { get; set; } = null!;
        public virtual DbSet<tblPoliticaDisciplinaria> tblPoliticaDisciplinaria { get; set; } = null!;
        public virtual DbSet<tblPorcentajeValorPrendaNEntidad> tblPorcentajeValorPrendaNEntidad { get; set; } = null!;
        public virtual DbSet<tblPosicionNAreaLavanderiaNLavanderia> tblPosicionNAreaLavanderiaNLavanderia { get; set; } = null!;
        public virtual DbSet<tblPrecioLavadoPrenda> tblPrecioLavadoPrenda { get; set; } = null!;
        public virtual DbSet<tblPregunta> tblPregunta { get; set; } = null!;
        public virtual DbSet<tblPreguntaNOpcionNPregunta> tblPreguntaNOpcionNPregunta { get; set; } = null!;
        public virtual DbSet<tblPrenda> tblPrenda { get; set; } = null!;
        public virtual DbSet<tblPrendaEjecutivo> tblPrendaEjecutivo { get; set; } = null!;
        public virtual DbSet<tblPrendaEjecutivoNRepartoValet> tblPrendaEjecutivoNRepartoValet { get; set; } = null!;
        public virtual DbSet<tblPrendaExtra> tblPrendaExtra { get; set; } = null!;
        public virtual DbSet<tblPrendaExtraNRepartoValet> tblPrendaExtraNRepartoValet { get; set; } = null!;
        public virtual DbSet<tblPrendaHuesped> tblPrendaHuesped { get; set; } = null!;
        public virtual DbSet<tblPrendaHuespedNRepartoValet> tblPrendaHuespedNRepartoValet { get; set; } = null!;
        public virtual DbSet<tblPrendaNAbono> tblPrendaNAbono { get; set; } = null!;
        public virtual DbSet<tblPrendaNAlmacen> tblPrendaNAlmacen { get; set; } = null!;
        public virtual DbSet<tblPrendaNAlmacenNInventario> tblPrendaNAlmacenNInventario { get; set; } = null!;
        public virtual DbSet<tblPrendaNEntidad_NuevoPedido> tblPrendaNEntidad_NuevoPedido { get; set; } = null!;
        public virtual DbSet<tblPrendaNGestionRetiro> tblPrendaNGestionRetiro { get; set; } = null!;
        public virtual DbSet<tblPrendaNInventario> tblPrendaNInventario { get; set; } = null!;
        public virtual DbSet<tblPrendaNLavanderia> tblPrendaNLavanderia { get; set; } = null!;
        public virtual DbSet<tblPrendaNMaquina> tblPrendaNMaquina { get; set; } = null!;
        public virtual DbSet<tblPrendaNMovimiento> tblPrendaNMovimiento { get; set; } = null!;
        public virtual DbSet<tblPrendaNMuestreo> tblPrendaNMuestreo { get; set; } = null!;
        public virtual DbSet<tblPrendaNMuestreo_FS> tblPrendaNMuestreo_FS { get; set; } = null!;
        public virtual DbSet<tblPrendaNPedido> tblPrendaNPedido { get; set; } = null!;
        public virtual DbSet<tblPrendaNPedidoExtra> tblPrendaNPedidoExtra { get; set; } = null!;
        public virtual DbSet<tblPrendaNPedidoHuesped> tblPrendaNPedidoHuesped { get; set; } = null!;
        public virtual DbSet<tblPrendaNProduccion> tblPrendaNProduccion { get; set; } = null!;
        public virtual DbSet<tblPrendaNReparto> tblPrendaNReparto { get; set; } = null!;
        public virtual DbSet<tblPrendaNRepartoOffice> tblPrendaNRepartoOffice { get; set; } = null!;
        public virtual DbSet<tblPrendaNRevision> tblPrendaNRevision { get; set; } = null!;
        public virtual DbSet<tblPrendaNSolicitudAbono> tblPrendaNSolicitudAbono { get; set; } = null!;
        public virtual DbSet<tblPrendaNSubAlmacen> tblPrendaNSubAlmacen { get; set; } = null!;
        public virtual DbSet<tblPrendaNTipoHabitacion> tblPrendaNTipoHabitacion { get; set; } = null!;
        public virtual DbSet<tblPrendaNUsuarioNEntidad> tblPrendaNUsuarioNEntidad { get; set; } = null!;
        public virtual DbSet<tblPrendaPrecioRefact> tblPrendaPrecioRefact { get; set; } = null!;
        public virtual DbSet<tblPrenda_historico_fechaValoracion> tblPrenda_historico_fechaValoracion { get; set; } = null!;
        public virtual DbSet<tblPrenda_historico_idTipoFacturacion> tblPrenda_historico_idTipoFacturacion { get; set; } = null!;
        public virtual DbSet<tblPrenda_historico_peso> tblPrenda_historico_peso { get; set; } = null!;
        public virtual DbSet<tblPrendasHora> tblPrendasHora { get; set; } = null!;
        public virtual DbSet<tblPresupuestoKg> tblPresupuestoKg { get; set; } = null!;
        public virtual DbSet<tblProduccion> tblProduccion { get; set; } = null!;
        public virtual DbSet<tblProduccionMaquinaNCliente> tblProduccionMaquinaNCliente { get; set; } = null!;
        public virtual DbSet<tblProduccionMaquinaNPrenda> tblProduccionMaquinaNPrenda { get; set; } = null!;
        public virtual DbSet<tblProgramasLavadora> tblProgramasLavadora { get; set; } = null!;
        public virtual DbSet<tblProveedor> tblProveedor { get; set; } = null!;
        public virtual DbSet<tblProyecto> tblProyecto { get; set; } = null!;
        public virtual DbSet<tblPuerto> tblPuerto { get; set; } = null!;
        public virtual DbSet<tblRecambio> tblRecambio { get; set; } = null!;
        public virtual DbSet<tblRecambioNAlmacenRecambios> tblRecambioNAlmacenRecambios { get; set; } = null!;
        public virtual DbSet<tblRecambioNMovimientoRecambio> tblRecambioNMovimientoRecambio { get; set; } = null!;
        public virtual DbSet<tblRecambioNParteTrabajo> tblRecambioNParteTrabajo { get; set; } = null!;
        public virtual DbSet<tblRecambioNParteTrabajoIBS> tblRecambioNParteTrabajoIBS { get; set; } = null!;
        public virtual DbSet<tblRecambioNProveedor> tblRecambioNProveedor { get; set; } = null!;
        public virtual DbSet<tblRechazoNProduccion> tblRechazoNProduccion { get; set; } = null!;
        public virtual DbSet<tblRecoveryPassword> tblRecoveryPassword { get; set; } = null!;
        public virtual DbSet<tblRecursoContador> tblRecursoContador { get; set; } = null!;
        public virtual DbSet<tblRecursoNivel> tblRecursoNivel { get; set; } = null!;
        public virtual DbSet<tblRecursoVirtual_Calculo> tblRecursoVirtual_Calculo { get; set; } = null!;
        public virtual DbSet<tblReparto> tblReparto { get; set; } = null!;
        public virtual DbSet<tblRepartoEstado> tblRepartoEstado { get; set; } = null!;
        public virtual DbSet<tblRepartoNParteTransporte> tblRepartoNParteTransporte { get; set; } = null!;
        public virtual DbSet<tblRepartoOffice> tblRepartoOffice { get; set; } = null!;
        public virtual DbSet<tblRepartosValet> tblRepartosValet { get; set; } = null!;
        public virtual DbSet<tblReports> tblReports { get; set; } = null!;
        public virtual DbSet<tblResponsableTarea> tblResponsableTarea { get; set; } = null!;
        public virtual DbSet<tblRespuesta> tblRespuesta { get; set; } = null!;
        public virtual DbSet<tblReunion> tblReunion { get; set; } = null!;
        public virtual DbSet<tblRevision> tblRevision { get; set; } = null!;
        public virtual DbSet<tblRevisionVehiculo> tblRevisionVehiculo { get; set; } = null!;
        public virtual DbSet<tblRevisionVehiculoNParteTransporte> tblRevisionVehiculoNParteTransporte { get; set; } = null!;
        public virtual DbSet<tblRuta> tblRuta { get; set; } = null!;
        public virtual DbSet<tblRutaExpedicion> tblRutaExpedicion { get; set; } = null!;
        public virtual DbSet<tblRutaNParteTransporte> tblRutaNParteTransporte { get; set; } = null!;
        public virtual DbSet<tblRutaSeccion> tblRutaSeccion { get; set; } = null!;
        public virtual DbSet<tblRuta_webfleet> tblRuta_webfleet { get; set; } = null!;
        public virtual DbSet<tblSacasPendientes> tblSacasPendientes { get; set; } = null!;
        public virtual DbSet<tblSalidaReparto> tblSalidaReparto { get; set; } = null!;
        public virtual DbSet<tblSeccionNivel1> tblSeccionNivel1 { get; set; } = null!;
        public virtual DbSet<tblSeccionNivel2> tblSeccionNivel2 { get; set; } = null!;
        public virtual DbSet<tblServicioExternoNParteTrabajo> tblServicioExternoNParteTrabajo { get; set; } = null!;
        public virtual DbSet<tblSistemaMaquina> tblSistemaMaquina { get; set; } = null!;
        public virtual DbSet<tblSolicitudAbono> tblSolicitudAbono { get; set; } = null!;
        public virtual DbSet<tblSolicitudAlta> tblSolicitudAlta { get; set; } = null!;
        public virtual DbSet<tblStockMinimoNAlmacenRecambios> tblStockMinimoNAlmacenRecambios { get; set; } = null!;
        public virtual DbSet<tblStockTipoElemLogNEntidad> tblStockTipoElemLogNEntidad { get; set; } = null!;
        public virtual DbSet<tblSubAlmacen> tblSubAlmacen { get; set; } = null!;
        public virtual DbSet<tblSubTurno> tblSubTurno { get; set; } = null!;
        public virtual DbSet<tblTag> tblTag { get; set; } = null!;
        public virtual DbSet<tblTallaAlfa> tblTallaAlfa { get; set; } = null!;
        public virtual DbSet<tblTaquilla> tblTaquilla { get; set; } = null!;
        public virtual DbSet<tblTaquilla_estado> tblTaquilla_estado { get; set; } = null!;
        public virtual DbSet<tblTaquilla_movimiento> tblTaquilla_movimiento { get; set; } = null!;
        public virtual DbSet<tblTaquillas_Estado_prueba> tblTaquillas_Estado_prueba { get; set; } = null!;
        public virtual DbSet<tblTaquillas_Movimiento_prueba> tblTaquillas_Movimiento_prueba { get; set; } = null!;
        public virtual DbSet<tblTaquillas_prueba> tblTaquillas_prueba { get; set; } = null!;
        public virtual DbSet<tblTareaMantenimientoPrev> tblTareaMantenimientoPrev { get; set; } = null!;
        public virtual DbSet<tblTareaMaquina> tblTareaMaquina { get; set; } = null!;
        public virtual DbSet<tblTareaPersonaDia> tblTareaPersonaDia { get; set; } = null!;
        public virtual DbSet<tblTasaCambio> tblTasaCambio { get; set; } = null!;
        public virtual DbSet<tblTasaCambioPresupuesto> tblTasaCambioPresupuesto { get; set; } = null!;
        public virtual DbSet<tblTimbradoMXNAdmFacturaVenta> tblTimbradoMXNAdmFacturaVenta { get; set; } = null!;
        public virtual DbSet<tblTipoAbono> tblTipoAbono { get; set; } = null!;
        public virtual DbSet<tblTipoAcceso> tblTipoAcceso { get; set; } = null!;
        public virtual DbSet<tblTipoAlmacen> tblTipoAlmacen { get; set; } = null!;
        public virtual DbSet<tblTipoAlmacenajeLimpio> tblTipoAlmacenajeLimpio { get; set; } = null!;
        public virtual DbSet<tblTipoConsumoLenceria> tblTipoConsumoLenceria { get; set; } = null!;
        public virtual DbSet<tblTipoContenedor> tblTipoContenedor { get; set; } = null!;
        public virtual DbSet<tblTipoContrato> tblTipoContrato { get; set; } = null!;
        public virtual DbSet<tblTipoDiaCuadrante> tblTipoDiaCuadrante { get; set; } = null!;
        public virtual DbSet<tblTipoDocumento> tblTipoDocumento { get; set; } = null!;
        public virtual DbSet<tblTipoDocumentoIdentidad> tblTipoDocumentoIdentidad { get; set; } = null!;
        public virtual DbSet<tblTipoDocumento_Envio> tblTipoDocumento_Envio { get; set; } = null!;
        public virtual DbSet<tblTipoElemLog> tblTipoElemLog { get; set; } = null!;
        public virtual DbSet<tblTipoEncuesta> tblTipoEncuesta { get; set; } = null!;
        public virtual DbSet<tblTipoEventoToken> tblTipoEventoToken { get; set; } = null!;
        public virtual DbSet<tblTipoFacturacion> tblTipoFacturacion { get; set; } = null!;
        public virtual DbSet<tblTipoFacturacionCliente> tblTipoFacturacionCliente { get; set; } = null!;
        public virtual DbSet<tblTipoFueraServicio> tblTipoFueraServicio { get; set; } = null!;
        public virtual DbSet<tblTipoHabitacion> tblTipoHabitacion { get; set; } = null!;
        public virtual DbSet<tblTipoIncidencia> tblTipoIncidencia { get; set; } = null!;
        public virtual DbSet<tblTipoKpi> tblTipoKpi { get; set; } = null!;
        public virtual DbSet<tblTipoKpi_Observaciones> tblTipoKpi_Observaciones { get; set; } = null!;
        public virtual DbSet<tblTipoLavado> tblTipoLavado { get; set; } = null!;
        public virtual DbSet<tblTipoLavadoHuesped> tblTipoLavadoHuesped { get; set; } = null!;
        public virtual DbSet<tblTipoLectura> tblTipoLectura { get; set; } = null!;
        public virtual DbSet<tblTipoMantenimientoMaquina> tblTipoMantenimientoMaquina { get; set; } = null!;
        public virtual DbSet<tblTipoMaquina> tblTipoMaquina { get; set; } = null!;
        public virtual DbSet<tblTipoMaquinaNCategoriaMaquina> tblTipoMaquinaNCategoriaMaquina { get; set; } = null!;
        public virtual DbSet<tblTipoMovimiento> tblTipoMovimiento { get; set; } = null!;
        public virtual DbSet<tblTipoMovimientoRecambio> tblTipoMovimientoRecambio { get; set; } = null!;
        public virtual DbSet<tblTipoMuestreo> tblTipoMuestreo { get; set; } = null!;
        public virtual DbSet<tblTipoNomina> tblTipoNomina { get; set; } = null!;
        public virtual DbSet<tblTipoNomina_MX> tblTipoNomina_MX { get; set; } = null!;
        public virtual DbSet<tblTipoNomina_RD> tblTipoNomina_RD { get; set; } = null!;
        public virtual DbSet<tblTipoNotificacion> tblTipoNotificacion { get; set; } = null!;
        public virtual DbSet<tblTipoPedido> tblTipoPedido { get; set; } = null!;
        public virtual DbSet<tblTipoPregunta> tblTipoPregunta { get; set; } = null!;
        public virtual DbSet<tblTipoPrenda> tblTipoPrenda { get; set; } = null!;
        public virtual DbSet<tblTipoPrendaHuesped> tblTipoPrendaHuesped { get; set; } = null!;
        public virtual DbSet<tblTipoProduccion> tblTipoProduccion { get; set; } = null!;
        public virtual DbSet<tblTipoRechazo> tblTipoRechazo { get; set; } = null!;
        public virtual DbSet<tblTipoRecursoNivel> tblTipoRecursoNivel { get; set; } = null!;
        public virtual DbSet<tblTipoRepartoEntidad> tblTipoRepartoEntidad { get; set; } = null!;
        public virtual DbSet<tblTipoRetiro> tblTipoRetiro { get; set; } = null!;
        public virtual DbSet<tblTipoReunion> tblTipoReunion { get; set; } = null!;
        public virtual DbSet<tblTipoSalidaRecambio> tblTipoSalidaRecambio { get; set; } = null!;
        public virtual DbSet<tblTipoServicio> tblTipoServicio { get; set; } = null!;
        public virtual DbSet<tblTipoSubIncidencia> tblTipoSubIncidencia { get; set; } = null!;
        public virtual DbSet<tblTipoTrabajo> tblTipoTrabajo { get; set; } = null!;
        public virtual DbSet<tblTipoTrabajoNUsuario> tblTipoTrabajoNUsuario { get; set; } = null!;
        public virtual DbSet<tblTipoUsuario> tblTipoUsuario { get; set; } = null!;
        public virtual DbSet<tblTipoVehiculo> tblTipoVehiculo { get; set; } = null!;
        public virtual DbSet<tblToken_Refresh> tblToken_Refresh { get; set; } = null!;
        public virtual DbSet<tblToken_Refresh_Mobile> tblToken_Refresh_Mobile { get; set; } = null!;
        public virtual DbSet<tblTraduccion> tblTraduccion { get; set; } = null!;
        public virtual DbSet<tblTurno> tblTurno { get; set; } = null!;
        public virtual DbSet<tblUnidadMedida> tblUnidadMedida { get; set; } = null!;
        public virtual DbSet<tblUnidadesPeso> tblUnidadesPeso { get; set; } = null!;
        public virtual DbSet<tblUsuario> tblUsuario { get; set; } = null!;
        public virtual DbSet<tblVehiculo> tblVehiculo { get; set; } = null!;
        public virtual DbSet<tblZonaHoraria> tblZonaHoraria { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<bdERP_alex>(entity =>
            {
                entity.Property(e => e.ID).ValueGeneratedNever();
            });

            modelBuilder.Entity<bdERP_joan>(entity =>
            {
                entity.Property(e => e.ID).ValueGeneratedNever();
            });

            modelBuilder.Entity<bdERP_nico>(entity =>
            {
                entity.Property(e => e.ID).ValueGeneratedNever();
            });

            modelBuilder.Entity<tblAbono>(entity =>
            {
                entity.HasKey(e => e.idAbono)
                    .HasName("PK_Logistica.tblAbono");

                entity.HasOne(d => d.idCategoriaAbonoNavigation)
                    .WithMany(p => p.tblAbono)
                    .HasForeignKey(d => d.idCategoriaAbono)
                    .HasConstraintName("FK_tblAbono_tblCategoriaAbono");

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblAbono)
                    .HasForeignKey(d => d.idEntidad)
                    .HasConstraintName("FK_Logistica.tblAbono_tblEntidad");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblAbono)
                    .HasForeignKey(d => d.idLavanderia)
                    .HasConstraintName("FK_tblAbono_tblLavanderia");

                entity.HasOne(d => d.idTipoAbonoNavigation)
                    .WithMany(p => p.tblAbono)
                    .HasForeignKey(d => d.idTipoAbono)
                    .HasConstraintName("FK_Logistica.tblAbono_Logistica.tblTipoAbono");
            });

            modelBuilder.Entity<tblAccionesCorrectivas>(entity =>
            {
                entity.HasKey(e => new { e.idAccionCorrectiva, e.idIncidencia });

                entity.Property(e => e.idAccionCorrectiva).ValueGeneratedOnAdd();

                entity.HasOne(d => d.idIncidenciaNavigation)
                    .WithMany(p => p.tblAccionesCorrectivas)
                    .HasForeignKey(d => d.idIncidencia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAccionesCorrectivas_tblIncidencia");
            });

            modelBuilder.Entity<tblAdmAlbaranCompra>(entity =>
            {
                entity.HasKey(e => e.idAdmAlbaranCompra)
                    .HasName("PK__tblAdmAl__ADBECCA480EC3AE1");

                entity.HasOne(d => d.idAdmAlbaran_EstadoNavigation)
                    .WithMany(p => p.tblAdmAlbaranCompra)
                    .HasForeignKey(d => d.idAdmAlbaran_Estado)
                    .HasConstraintName("FK_tblAdmAlbaranCompra_tblAdmAlbaran_Estado");

                entity.HasOne(d => d.idAdmCentroCosteNavigation)
                    .WithMany(p => p.tblAdmAlbaranCompra)
                    .HasForeignKey(d => d.idAdmCentroCoste)
                    .HasConstraintName("FK__tblAdmAlb__idAdm__122052C0");

                entity.HasOne(d => d.idAdmElementoPEPNavigation)
                    .WithMany(p => p.tblAdmAlbaranCompra)
                    .HasForeignKey(d => d.idAdmElementoPEP)
                    .HasConstraintName("FK__tblAdmAlb__idAdm__131476F9");

                entity.HasOne(d => d.idAdmFormaPagoNavigation)
                    .WithMany(p => p.tblAdmAlbaranCompra)
                    .HasForeignKey(d => d.idAdmFormaPago)
                    .HasConstraintName("FK_tblAdmAlbaranCompra_tblAdmFormaPago");

                entity.HasOne(d => d.idAdmPedidoProveedorNavigation)
                    .WithMany(p => p.tblAdmAlbaranCompra)
                    .HasForeignKey(d => d.idAdmPedidoProveedor)
                    .HasConstraintName("FK_tblAdmAlbaranCompra_tblAdmPedidoProveedor");

                entity.HasOne(d => d.idAdmProveedorNavigation)
                    .WithMany(p => p.tblAdmAlbaranCompra)
                    .HasForeignKey(d => d.idAdmProveedor)
                    .HasConstraintName("FK_tblAdmAlbaranCompra_tblAdmProveedor");

                entity.HasOne(d => d.idAdmTipoCambioNavigation)
                    .WithMany(p => p.tblAdmAlbaranCompra)
                    .HasForeignKey(d => d.idAdmTipoCambio)
                    .HasConstraintName("FK_tblAdmAlbaranCompra_tblAdmTipoCambio");

                entity.HasOne(d => d.idAdmTipoDescuentoNavigation)
                    .WithMany(p => p.tblAdmAlbaranCompra)
                    .HasForeignKey(d => d.idAdmTipoDescuento)
                    .HasConstraintName("FK_tblAdmAlbaranCompra_tblAdmTipoDescuento");

                entity.HasOne(d => d.idEmpresaPolarierNavigation)
                    .WithMany(p => p.tblAdmAlbaranCompra)
                    .HasForeignKey(d => d.idEmpresaPolarier)
                    .HasConstraintName("FK_tblAdmAlbaranCompra_tblEmpresasPolarier");

                entity.HasOne(d => d.idIncotermNavigation)
                    .WithMany(p => p.tblAdmAlbaranCompra)
                    .HasForeignKey(d => d.idIncoterm)
                    .HasConstraintName("FK_tblAdmAlbaranCompra_tblIncoterm");

                entity.HasOne(d => d.idMonedaNavigation)
                    .WithMany(p => p.tblAdmAlbaranCompra)
                    .HasForeignKey(d => d.idMoneda)
                    .HasConstraintName("FK_tblAdmAlbaranCompra_tblMoneda");

                entity.HasOne(d => d.idTipoAlbaranNavigation)
                    .WithMany(p => p.tblAdmAlbaranCompra)
                    .HasForeignKey(d => d.idTipoAlbaran)
                    .HasConstraintName("FK_tblAdmAlbaranCompra_tblAdmTipoElemento");

                entity.HasMany(d => d.idAdmFacturaCompra)
                    .WithMany(p => p.idAdmAlbaranCompra)
                    .UsingEntity<Dictionary<string, object>>(
                        "tblAdmAlbaranCompraNFacturaCompra",
                        l => l.HasOne<tblAdmFacturaCompra>().WithMany().HasForeignKey("idAdmFacturaCompra").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblAdmAlbaranCompraNFacturaCompra_tblAdmFacturaCompra"),
                        r => r.HasOne<tblAdmAlbaranCompra>().WithMany().HasForeignKey("idAdmAlbaranCompra").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblAdmAlbaranCompraNFacturaCompra_tblAdmAlbaranCompra"),
                        j =>
                        {
                            j.HasKey("idAdmAlbaranCompra", "idAdmFacturaCompra").HasName("PK__tblAdmAl__7A1EDDDF66BDF53F");

                            j.ToTable("tblAdmAlbaranCompraNFacturaCompra", "Administracion");
                        });
            });

            modelBuilder.Entity<tblAdmAlbaranVenta>(entity =>
            {
                entity.HasKey(e => e.idAdmAlbaranVenta)
                    .HasName("PK__tblAdmAl__8513215C80412CEC");

                entity.HasOne(d => d.idAdmAlbaran_EstadoNavigation)
                    .WithMany(p => p.tblAdmAlbaranVenta)
                    .HasForeignKey(d => d.idAdmAlbaran_Estado)
                    .HasConstraintName("FK_tblAdmAlbaranVenta_tblAdmAlbaran_Estado");

                entity.HasOne(d => d.idAdmCentroCosteNavigation)
                    .WithMany(p => p.tblAdmAlbaranVenta)
                    .HasForeignKey(d => d.idAdmCentroCoste)
                    .HasConstraintName("FK__tblAdmAlb__idAdm__14089B32");

                entity.HasOne(d => d.idAdmClienteNavigation)
                    .WithMany(p => p.tblAdmAlbaranVenta)
                    .HasForeignKey(d => d.idAdmCliente)
                    .HasConstraintName("FK_tblAdmAlbaranVenta_tblAdmCliente");

                entity.HasOne(d => d.idAdmElementoPEPNavigation)
                    .WithMany(p => p.tblAdmAlbaranVenta)
                    .HasForeignKey(d => d.idAdmElementoPEP)
                    .HasConstraintName("FK__tblAdmAlb__idAdm__14FCBF6B");

                entity.HasOne(d => d.idAdmFormaPagoNavigation)
                    .WithMany(p => p.tblAdmAlbaranVenta)
                    .HasForeignKey(d => d.idAdmFormaPago)
                    .HasConstraintName("FK_tblAdmAlbaranVenta_tblAdmFormaPago");

                entity.HasOne(d => d.idAdmPedidoClienteNavigation)
                    .WithMany(p => p.tblAdmAlbaranVenta)
                    .HasForeignKey(d => d.idAdmPedidoCliente)
                    .HasConstraintName("FK_tblAdmAlbaranVenta_tblAdmPedidoCliente");

                entity.HasOne(d => d.idAdmTipoCambioNavigation)
                    .WithMany(p => p.tblAdmAlbaranVenta)
                    .HasForeignKey(d => d.idAdmTipoCambio)
                    .HasConstraintName("FK_tblAdmAlbaranVenta_tblAdmTipoCambio");

                entity.HasOne(d => d.idAdmTipoDescuentoNavigation)
                    .WithMany(p => p.tblAdmAlbaranVenta)
                    .HasForeignKey(d => d.idAdmTipoDescuento)
                    .HasConstraintName("FK_tblAdmAlbaranVenta_tblAdmTipoDescuento");

                entity.HasOne(d => d.idEmpresaPolarierNavigation)
                    .WithMany(p => p.tblAdmAlbaranVenta)
                    .HasForeignKey(d => d.idEmpresaPolarier)
                    .HasConstraintName("FK_tblAdmAlbaranVenta_tblEmpresasPolarier");

                entity.HasOne(d => d.idIncotermNavigation)
                    .WithMany(p => p.tblAdmAlbaranVenta)
                    .HasForeignKey(d => d.idIncoterm)
                    .HasConstraintName("FK__tblAdmAlb__idInc__501D8539");

                entity.HasOne(d => d.idMonedaNavigation)
                    .WithMany(p => p.tblAdmAlbaranVenta)
                    .HasForeignKey(d => d.idMoneda)
                    .HasConstraintName("FK_tblAdmAlbaranVenta_tblMoneda");

                entity.HasOne(d => d.idTipoAlbaranNavigation)
                    .WithMany(p => p.tblAdmAlbaranVenta)
                    .HasForeignKey(d => d.idTipoAlbaran)
                    .HasConstraintName("FK_tblAdmAlbaranVenta_tblAdmTipoElemento");

                entity.HasOne(d => d.idTipoFacturaNavigation)
                    .WithMany(p => p.tblAdmAlbaranVenta)
                    .HasForeignKey(d => d.idTipoFactura)
                    .HasConstraintName("FK_tblAdmAlbaranVenta_tblAdmTipoFactura");

                entity.HasMany(d => d.idAdmFacturaVenta)
                    .WithMany(p => p.idAdmAlbaranVenta)
                    .UsingEntity<Dictionary<string, object>>(
                        "tblAdmAlbaranVentaNFacturaVenta",
                        l => l.HasOne<tblAdmFacturaVenta>().WithMany().HasForeignKey("idAdmFacturaVenta").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblAdmAlbaranVentaNFacturaVenta_tblAdmFacturaVenta"),
                        r => r.HasOne<tblAdmAlbaranVenta>().WithMany().HasForeignKey("idAdmAlbaranVenta").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblAdmAlbaranVentaNFacturaVenta_tblAdmAlbaranVenta"),
                        j =>
                        {
                            j.HasKey("idAdmAlbaranVenta", "idAdmFacturaVenta").HasName("PK__tblAdmAl__0D586F6C02983382");

                            j.ToTable("tblAdmAlbaranVentaNFacturaVenta", "Administracion");
                        });
            });

            modelBuilder.Entity<tblAdmAlbaran_Estado>(entity =>
            {
                entity.HasKey(e => e.idAdmAlbaran_Estado)
                    .HasName("PK__tblAdmAl__4E013251035BA2E9");
            });

            modelBuilder.Entity<tblAdmArticulo>(entity =>
            {
                entity.HasKey(e => e.idAdmArticulo)
                    .HasName("PK__tblAdmAr__91B1215CFD4EA586");
            });

            modelBuilder.Entity<tblAdmArticuloNAdmAlbaranCompra>(entity =>
            {
                entity.HasKey(e => new { e.idAdmArticulo, e.idAdmAlbaranCompra })
                    .HasName("PK__tblAdmAr__CB6ACD96A0BC940B");

                entity.HasOne(d => d.idAdmAlbaranCompraNavigation)
                    .WithMany(p => p.tblAdmArticuloNAdmAlbaranCompra)
                    .HasForeignKey(d => d.idAdmAlbaranCompra)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAdmArticuloNAdmAlbaranCompra_tblAdmAlbaranCompra");

                entity.HasOne(d => d.idAdmArticuloNavigation)
                    .WithMany(p => p.tblAdmArticuloNAdmAlbaranCompra)
                    .HasForeignKey(d => d.idAdmArticulo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblAdmArt__idAdm__20196C5C");
            });

            modelBuilder.Entity<tblAdmArticuloNAdmAlbaranVenta>(entity =>
            {
                entity.HasKey(e => new { e.idAdmArticulo, e.idAdmAlbaranVenta })
                    .HasName("PK__tblAdmAr__49E01349456F5E35");

                entity.HasOne(d => d.idAdmAlbaranVentaNavigation)
                    .WithMany(p => p.tblAdmArticuloNAdmAlbaranVenta)
                    .HasForeignKey(d => d.idAdmAlbaranVenta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAdmArticuloNAdmAlbaranVenta_tblAdmAlbaranVenta");

                entity.HasOne(d => d.idAdmArticuloNavigation)
                    .WithMany(p => p.tblAdmArticuloNAdmAlbaranVenta)
                    .HasForeignKey(d => d.idAdmArticulo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblAdmArt__idAdm__23E9FD40");
            });

            modelBuilder.Entity<tblAdmArticuloNAdmPedidoCliente>(entity =>
            {
                entity.HasKey(e => new { e.idAdmArticulo, e.idAdmPedidoCliente })
                    .HasName("PK__tblAdmAr__2812547E2DB6737F");

                entity.HasOne(d => d.idAdmArticuloNavigation)
                    .WithMany(p => p.tblAdmArticuloNAdmPedidoCliente)
                    .HasForeignKey(d => d.idAdmArticulo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblAdmArt__idAdm__18784A94");

                entity.HasOne(d => d.idAdmPedidoClienteNavigation)
                    .WithMany(p => p.tblAdmArticuloNAdmPedidoCliente)
                    .HasForeignKey(d => d.idAdmPedidoCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAdmArticuloNAdmPedidoCliente_tblAdmPedidoCliente");
            });

            modelBuilder.Entity<tblAdmArticuloNAdmPedidoProveedor>(entity =>
            {
                entity.HasKey(e => new { e.idAdmArticulo, e.idAdmPedidoProveedor })
                    .HasName("PK__tblAdmAr__C82B02413FA79F45");

                entity.HasOne(d => d.idAdmArticuloNavigation)
                    .WithMany(p => p.tblAdmArticuloNAdmPedidoProveedor)
                    .HasForeignKey(d => d.idAdmArticulo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblAdmArt__idAdm__1C48DB78");

                entity.HasOne(d => d.idAdmPedidoProveedorNavigation)
                    .WithMany(p => p.tblAdmArticuloNAdmPedidoProveedor)
                    .HasForeignKey(d => d.idAdmPedidoProveedor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAdmArticuloNAdmPedidoProveedor_tblAdmPedidoProveedor");
            });

            modelBuilder.Entity<tblAdmArticuloNAdmPresupuestoVenta>(entity =>
            {
                entity.HasKey(e => new { e.idAdmArticulo, e.idAdmPresupuestoVenta })
                    .HasName("PK__tblAdmAr__8B9A9A0CF827D81B");

                entity.HasOne(d => d.idAdmArticuloNavigation)
                    .WithMany(p => p.tblAdmArticuloNAdmPresupuestoVenta)
                    .HasForeignKey(d => d.idAdmArticulo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblAdmArt__idAdm__14A7B9B0");

                entity.HasOne(d => d.idAdmPresupuestoVentaNavigation)
                    .WithMany(p => p.tblAdmArticuloNAdmPresupuestoVenta)
                    .HasForeignKey(d => d.idAdmPresupuestoVenta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAdmArticuloNAdmPresupuestoVenta_tblAdmPresupuestoVenta");
            });

            modelBuilder.Entity<tblAdmCentroBeneficio>(entity =>
            {
                entity.HasKey(e => e.idAdmCentroBeneficio)
                    .HasName("PK__tblAdmCe__9BFD0FEC3B855446");
            });

            modelBuilder.Entity<tblAdmCentroCoste>(entity =>
            {
                entity.HasKey(e => e.idAdmCentroCoste)
                    .HasName("PK__tblAdmCe__A9E4CD7715AA812A");

                entity.Property(e => e.idAdmCentroCoste).ValueGeneratedNever();

                entity.Property(e => e.isEliminado).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.idEmpresaPolarierNavigation)
                    .WithMany(p => p.tblAdmCentroCoste)
                    .HasForeignKey(d => d.idEmpresaPolarier)
                    .HasConstraintName("FK__tblAdmCen__idEmp__2F5BAFEC");
            });

            modelBuilder.Entity<tblAdmCliente>(entity =>
            {
                entity.HasOne(d => d.idAdmCentroCosteNavigation)
                    .WithMany(p => p.tblAdmCliente)
                    .HasForeignKey(d => d.idAdmCentroCoste)
                    .HasConstraintName("FK_tblAdmCliente_tblAdmCentroCoste");

                entity.HasOne(d => d.idAdmCondicionPagoNavigation)
                    .WithMany(p => p.tblAdmCliente)
                    .HasForeignKey(d => d.idAdmCondicionPago)
                    .HasConstraintName("FK_tblAdmCliente_tblAdmCondicionPago");

                entity.HasOne(d => d.idAdmElementoPEPNavigation)
                    .WithMany(p => p.tblAdmCliente)
                    .HasForeignKey(d => d.idAdmElementoPEP)
                    .HasConstraintName("FK_tblAdmCliente_tblAdmElementoPEP");

                entity.HasOne(d => d.idAdmFormaCobroNavigation)
                    .WithMany(p => p.tblAdmCliente)
                    .HasForeignKey(d => d.idAdmFormaCobro)
                    .HasConstraintName("FK_tblAdmCliente_tblAdmFormaPago");

                entity.HasOne(d => d.idCuentaBancariaNavigation)
                    .WithMany(p => p.tblAdmCliente)
                    .HasForeignKey(d => d.idCuentaBancaria)
                    .HasConstraintName("FK_tblAdmCliente_tblAdmCuentaBancaria");

                entity.HasOne(d => d.idEmpresaPolarierNavigation)
                    .WithMany(p => p.tblAdmCliente)
                    .HasForeignKey(d => d.idEmpresaPolarier)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAdmCliente_tblEmpresasPolarier");

                entity.HasOne(d => d.idIvaNPaisNavigation)
                    .WithMany(p => p.tblAdmCliente)
                    .HasForeignKey(d => d.idIvaNPais)
                    .HasConstraintName("FK_tblAdmCliente_tblIvaNPais");

                entity.HasOne(d => d.idMonedaNavigation)
                    .WithMany(p => p.tblAdmCliente)
                    .HasForeignKey(d => d.idMoneda)
                    .HasConstraintName("FK_tblAdmCliente_tblMoneda");

                entity.HasOne(d => d.idPaisNavigation)
                    .WithMany(p => p.tblAdmCliente)
                    .HasForeignKey(d => d.idPais)
                    .HasConstraintName("FK_tblAdmCliente_tblPais");
            });

            modelBuilder.Entity<tblAdmConceptoCompra>(entity =>
            {
                entity.HasKey(e => new { e.idAdmConceptoCompra, e.idAdmFacturaCompra })
                    .HasName("PK__tblAdmCo__6E369D4633A0541B");

                entity.Property(e => e.idAdmConceptoCompra).ValueGeneratedOnAdd();

                entity.HasOne(d => d.idAdmCuentaContableNavigation)
                    .WithMany(p => p.tblAdmConceptoCompra)
                    .HasForeignKey(d => d.idAdmCuentaContable)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAdmConceptoCompra_tblAdmCuentaContable");

                entity.HasOne(d => d.idAdmFacturaCompraNavigation)
                    .WithMany(p => p.tblAdmConceptoCompra)
                    .HasForeignKey(d => d.idAdmFacturaCompra)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAdmConceptoCompra_tblAdmFacturaCompra");

                entity.HasOne(d => d.idIvaNPaisNavigation)
                    .WithMany(p => p.tblAdmConceptoCompra)
                    .HasForeignKey(d => d.idIvaNPais)
                    .HasConstraintName("FK_tblAdmConceptoCompra_tblAdmIvaNPais");
            });

            modelBuilder.Entity<tblAdmConceptoVenta>(entity =>
            {
                entity.HasKey(e => new { e.idAdmConceptoVenta, e.idAdmFacturaVenta })
                    .HasName("PK__tblAdmCo__0B1A9CFAA9BB498D");

                entity.Property(e => e.idAdmConceptoVenta).ValueGeneratedOnAdd();

                entity.HasOne(d => d.idAdmCuentaContableNavigation)
                    .WithMany(p => p.tblAdmConceptoVenta)
                    .HasForeignKey(d => d.idAdmCuentaContable)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAdmConceptoVenta_tblAdmCuentaContable");

                entity.HasOne(d => d.idAdmFacturaVentaNavigation)
                    .WithMany(p => p.tblAdmConceptoVenta)
                    .HasForeignKey(d => d.idAdmFacturaVenta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAdmConceptoVenta_tblAdmFacturaVenta");
            });

            modelBuilder.Entity<tblAdmCuentaBancaria>(entity =>
            {
                entity.HasOne(d => d.idAdmBancoNavigation)
                    .WithMany(p => p.tblAdmCuentaBancaria)
                    .HasForeignKey(d => d.idAdmBanco)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAdmCuentaBancaria_tblAdmBanco");

                entity.HasOne(d => d.idEmpresaPolarierNavigation)
                    .WithMany(p => p.tblAdmCuentaBancaria)
                    .HasForeignKey(d => d.idEmpresaPolarier)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAdmCuentaBancaria_tblEmpresasPolarier");

                entity.HasOne(d => d.idMonedaNavigation)
                    .WithMany(p => p.tblAdmCuentaBancaria)
                    .HasForeignKey(d => d.idMoneda)
                    .HasConstraintName("FK__tblAdmCue__idMon__6700EA91");
            });

            modelBuilder.Entity<tblAdmElementoPEP>(entity =>
            {
                entity.HasKey(e => e.idAdmElementoPEP)
                    .HasName("PK__tblAdmEl__2935335F36CBF9A4");

                entity.Property(e => e.idAdmElementoPEP).ValueGeneratedNever();

                entity.Property(e => e.isEliminado).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.idAdmCentroBeneficioNavigation)
                    .WithMany(p => p.tblAdmElementoPEP)
                    .HasForeignKey(d => d.idAdmCentroBeneficio)
                    .HasConstraintName("FK__tblAdmEle__idAdm__23D4EEA7");

                entity.HasOne(d => d.idAdmElementoPEPPadreNavigation)
                    .WithMany(p => p.InverseidAdmElementoPEPPadreNavigation)
                    .HasForeignKey(d => d.idAdmElementoPEPPadre)
                    .HasConstraintName("FK_tblAdmElementoPEP_tblAdmElementoPEP");

                entity.HasOne(d => d.idEmpresaPolarierNavigation)
                    .WithMany(p => p.tblAdmElementoPEP)
                    .HasForeignKey(d => d.idEmpresaPolarier)
                    .HasConstraintName("FK__tblAdmEle__idEmp__4C81FE7F");
            });

            modelBuilder.Entity<tblAdmFacturaCompra>(entity =>
            {
                entity.HasKey(e => e.idAdmFacturaCompra)
                    .HasName("PK__tblAdmFa__7A0117BC6BEB6B62");

                entity.HasOne(d => d.idAdmCentroCosteNavigation)
                    .WithMany(p => p.tblAdmFacturaCompra)
                    .HasForeignKey(d => d.idAdmCentroCoste)
                    .HasConstraintName("FK__tblAdmFac__idAdm__15F0E3A4");

                entity.HasOne(d => d.idAdmCondicionPagoNavigation)
                    .WithMany(p => p.tblAdmFacturaCompra)
                    .HasForeignKey(d => d.idAdmCondicionPago)
                    .HasConstraintName("FK_tblAdmFacturaCompra_tblAdmCondicionPago");

                entity.HasOne(d => d.idAdmElementoPEPNavigation)
                    .WithMany(p => p.tblAdmFacturaCompra)
                    .HasForeignKey(d => d.idAdmElementoPEP)
                    .HasConstraintName("FK__tblAdmFac__idAdm__16E507DD");

                entity.HasOne(d => d.idAdmFactura_EstadoNavigation)
                    .WithMany(p => p.tblAdmFacturaCompra)
                    .HasForeignKey(d => d.idAdmFactura_Estado)
                    .HasConstraintName("FK_tblAdmFacturaCompra_tblAdmFactura_Estado");

                entity.HasOne(d => d.idAdmFormaPagoNavigation)
                    .WithMany(p => p.tblAdmFacturaCompra)
                    .HasForeignKey(d => d.idAdmFormaPago)
                    .HasConstraintName("FK_tblAdmFacturaCompra_tblAdmFormaPago");

                entity.HasOne(d => d.idAdmProveedorNavigation)
                    .WithMany(p => p.tblAdmFacturaCompra)
                    .HasForeignKey(d => d.idAdmProveedor)
                    .HasConstraintName("FK_tblAdmFacturaCompra_tblAdmProveedor");

                entity.HasOne(d => d.idAdmTipoCambioNavigation)
                    .WithMany(p => p.tblAdmFacturaCompra)
                    .HasForeignKey(d => d.idAdmTipoCambio)
                    .HasConstraintName("FK_tblAdmFacturaCompra_tblAdmTipoCambio");

                entity.HasOne(d => d.idAdmTipoDescuentoNavigation)
                    .WithMany(p => p.tblAdmFacturaCompra)
                    .HasForeignKey(d => d.idAdmTipoDescuento)
                    .HasConstraintName("FK_tblAdmFacturaCompra_tblAdmTipoDescuento");

                entity.HasOne(d => d.idEmpresaPolarierNavigation)
                    .WithMany(p => p.tblAdmFacturaCompra)
                    .HasForeignKey(d => d.idEmpresaPolarier)
                    .HasConstraintName("FK_tblAdmFacturaCompra_tblEmpresasPolarier");

                entity.HasOne(d => d.idIncotermNavigation)
                    .WithMany(p => p.tblAdmFacturaCompra)
                    .HasForeignKey(d => d.idIncoterm)
                    .HasConstraintName("FK_tblAdmFacturaCompra_tblIncoterm");

                entity.HasOne(d => d.idMonedaNavigation)
                    .WithMany(p => p.tblAdmFacturaCompra)
                    .HasForeignKey(d => d.idMoneda)
                    .HasConstraintName("FK_tblAdmFacturaCompra_tblMoneda");

                entity.HasOne(d => d.idTipoAlbaranNavigation)
                    .WithMany(p => p.tblAdmFacturaCompra)
                    .HasForeignKey(d => d.idTipoAlbaran)
                    .HasConstraintName("FK_tblAdmFacturaCompra_tblAdmTipoElemento");

                entity.HasOne(d => d.idTipoFacturaNavigation)
                    .WithMany(p => p.tblAdmFacturaCompra)
                    .HasForeignKey(d => d.idTipoFactura)
                    .HasConstraintName("FK_tblAdmFacturaCompra_tblAdmTipoFactura");
            });

            modelBuilder.Entity<tblAdmFacturaVenta>(entity =>
            {
                entity.HasKey(e => e.idAdmFacturaVenta)
                    .HasName("PK__tblAdmFa__84B4E3090DE914F1");

                entity.HasOne(d => d.idAdmCentroCosteNavigation)
                    .WithMany(p => p.tblAdmFacturaVenta)
                    .HasForeignKey(d => d.idAdmCentroCoste)
                    .HasConstraintName("FK__tblAdmFac__idAdm__17D92C16");

                entity.HasOne(d => d.idAdmClienteNavigation)
                    .WithMany(p => p.tblAdmFacturaVenta)
                    .HasForeignKey(d => d.idAdmCliente)
                    .HasConstraintName("FK_tblAdmFacturaVenta_tblAdmCliente");

                entity.HasOne(d => d.idAdmCondicionPagoNavigation)
                    .WithMany(p => p.tblAdmFacturaVenta)
                    .HasForeignKey(d => d.idAdmCondicionPago)
                    .HasConstraintName("FK_tblAdmFacturaVenta_tblAdmCondicionPago");

                entity.HasOne(d => d.idAdmElementoPEPNavigation)
                    .WithMany(p => p.tblAdmFacturaVenta)
                    .HasForeignKey(d => d.idAdmElementoPEP)
                    .HasConstraintName("FK__tblAdmFac__idAdm__18CD504F");

                entity.HasOne(d => d.idAdmFactura_EstadoNavigation)
                    .WithMany(p => p.tblAdmFacturaVenta)
                    .HasForeignKey(d => d.idAdmFactura_Estado)
                    .HasConstraintName("FK_tblAdmFacturaVenta_tblAdmFactura_Estado");

                entity.HasOne(d => d.idAdmFormaCobroNavigation)
                    .WithMany(p => p.tblAdmFacturaVenta)
                    .HasForeignKey(d => d.idAdmFormaCobro)
                    .HasConstraintName("FK_tblAdmFacturaVenta_tblAdmFormaPago");

                entity.HasOne(d => d.idAdmTipoCambioNavigation)
                    .WithMany(p => p.tblAdmFacturaVenta)
                    .HasForeignKey(d => d.idAdmTipoCambio)
                    .HasConstraintName("FK_tblAdmFacturaVenta_tblAdmTipoCambio");

                entity.HasOne(d => d.idAdmTipoDescuentoNavigation)
                    .WithMany(p => p.tblAdmFacturaVenta)
                    .HasForeignKey(d => d.idAdmTipoDescuento)
                    .HasConstraintName("FK_tblAdmFacturaVenta_tblAdmTipoDescuento");

                entity.HasOne(d => d.idAdmTipoNCFNavigation)
                    .WithMany(p => p.tblAdmFacturaVenta)
                    .HasForeignKey(d => d.idAdmTipoNCF)
                    .HasConstraintName("FK_tblAdmFacturaVenta_tblAdmidTipoNCF");

                entity.HasOne(d => d.idCuentaBancariaNavigation)
                    .WithMany(p => p.tblAdmFacturaVenta)
                    .HasForeignKey(d => d.idCuentaBancaria)
                    .HasConstraintName("FK_tblAdmFacturaVenta_tblAdmCuentaBancaria");

                entity.HasOne(d => d.idEmpresaPolarierNavigation)
                    .WithMany(p => p.tblAdmFacturaVenta)
                    .HasForeignKey(d => d.idEmpresaPolarier)
                    .HasConstraintName("FK_tblAdmFacturaVenta_tblEmpresasPolarier");

                entity.HasOne(d => d.idIncotermNavigation)
                    .WithMany(p => p.tblAdmFacturaVenta)
                    .HasForeignKey(d => d.idIncoterm)
                    .HasConstraintName("FK_tblAdmFacturaVenta_tblIncoterm");

                entity.HasOne(d => d.idIvaNPaisNavigation)
                    .WithMany(p => p.tblAdmFacturaVenta)
                    .HasForeignKey(d => d.idIvaNPais)
                    .HasConstraintName("FK_tblAdmFacturaVenta_tblAdmIvaNPais");

                entity.HasOne(d => d.idMonedaNavigation)
                    .WithMany(p => p.tblAdmFacturaVenta)
                    .HasForeignKey(d => d.idMoneda)
                    .HasConstraintName("FK_tblAdmFacturaVenta_tblMoneda");

                entity.HasOne(d => d.idReferenciaFacturaVentaNavigation)
                    .WithMany(p => p.InverseidReferenciaFacturaVentaNavigation)
                    .HasForeignKey(d => d.idReferenciaFacturaVenta)
                    .HasConstraintName("FK__tblAdmFac__idRef__11B63E6C");

                entity.HasOne(d => d.idTipoAlbaranNavigation)
                    .WithMany(p => p.tblAdmFacturaVenta)
                    .HasForeignKey(d => d.idTipoAlbaran)
                    .HasConstraintName("FK_tblAdmFacturaVenta_tblAdmTipoElemento");

                entity.HasOne(d => d.idTipoFacturaNavigation)
                    .WithMany(p => p.tblAdmFacturaVenta)
                    .HasForeignKey(d => d.idTipoFactura)
                    .HasConstraintName("FK_tblAdmFacturaVenta_tblAdmTipoFactura");
            });

            modelBuilder.Entity<tblAdmFactura_Estado>(entity =>
            {
                entity.HasKey(e => e.idAdmFactura_Estado)
                    .HasName("PK__tblAdmFa__1874AA1F70F4D366");
            });

            modelBuilder.Entity<tblAdmFormaPago>(entity =>
            {
                entity.Property(e => e.isCuentaBancariaRequired).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.idPaisNavigation)
                    .WithMany(p => p.tblAdmFormaPago)
                    .HasForeignKey(d => d.idPais)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAdmFormaPago_tblPais");
            });

            modelBuilder.Entity<tblAdmIva>(entity =>
            {
                entity.HasKey(e => e.idAdmIva)
                    .HasName("PK__tblAdmIv__9F050EA31D822664");
            });

            modelBuilder.Entity<tblAdmPedidoCliente>(entity =>
            {
                entity.HasKey(e => e.idAdmPedidoCliente)
                    .HasName("PK__tblAdmPe__9A37522A203BFB0D");

                entity.HasOne(d => d.idAdmCentroCosteNavigation)
                    .WithMany(p => p.tblAdmPedidoCliente)
                    .HasForeignKey(d => d.idAdmCentroCoste)
                    .HasConstraintName("FK_tblAdmPedidoCliente_tblAdmCentroCoste");

                entity.HasOne(d => d.idAdmClienteNavigation)
                    .WithMany(p => p.tblAdmPedidoCliente)
                    .HasForeignKey(d => d.idAdmCliente)
                    .HasConstraintName("FK_tblAdmPedidoCliente_tblAdmCliente");

                entity.HasOne(d => d.idAdmElementoPEPNavigation)
                    .WithMany(p => p.tblAdmPedidoCliente)
                    .HasForeignKey(d => d.idAdmElementoPEP)
                    .HasConstraintName("FK__tblAdmPed__idAdm__7F0D7E4C");

                entity.HasOne(d => d.idAdmFormaPagoNavigation)
                    .WithMany(p => p.tblAdmPedidoCliente)
                    .HasForeignKey(d => d.idAdmFormaPago)
                    .HasConstraintName("FK_tblAdmPedidoCliente_tblAdmFormaPago");

                entity.HasOne(d => d.idAdmPedido_EstadoNavigation)
                    .WithMany(p => p.tblAdmPedidoCliente)
                    .HasForeignKey(d => d.idAdmPedido_Estado)
                    .HasConstraintName("FK_tblAdmPedidoCliente_tblAdmPedido_Estado");

                entity.HasOne(d => d.idAdmPresupuestoVentaNavigation)
                    .WithMany(p => p.tblAdmPedidoCliente)
                    .HasForeignKey(d => d.idAdmPresupuestoVenta)
                    .HasConstraintName("FK_tblAdmPedidoCliente_tblAdmPresupuestoVenta");

                entity.HasOne(d => d.idAdmTipoCambioNavigation)
                    .WithMany(p => p.tblAdmPedidoCliente)
                    .HasForeignKey(d => d.idAdmTipoCambio)
                    .HasConstraintName("FK_tblAdmPedidoCliente_tblAdmTipoCambio");

                entity.HasOne(d => d.idAdmTipoDescuentoNavigation)
                    .WithMany(p => p.tblAdmPedidoCliente)
                    .HasForeignKey(d => d.idAdmTipoDescuento)
                    .HasConstraintName("FK_tblAdmPedidoCliente_tblAdmTipoDescuento");

                entity.HasOne(d => d.idEmpresaPolarierNavigation)
                    .WithMany(p => p.tblAdmPedidoCliente)
                    .HasForeignKey(d => d.idEmpresaPolarier)
                    .HasConstraintName("FK_tblAdmPedidoCliente_tblEmpresasPolarier");

                entity.HasOne(d => d.idIncotermNavigation)
                    .WithMany(p => p.tblAdmPedidoCliente)
                    .HasForeignKey(d => d.idIncoterm)
                    .HasConstraintName("FK_tblAdmPedidoCliente_tblIncoterm");

                entity.HasOne(d => d.idIvaNPaisNavigation)
                    .WithMany(p => p.tblAdmPedidoCliente)
                    .HasForeignKey(d => d.idIvaNPais)
                    .HasConstraintName("FK_tblAdmPedidoCliente_tblAdmIvaNPais");

                entity.HasOne(d => d.idMonedaNavigation)
                    .WithMany(p => p.tblAdmPedidoCliente)
                    .HasForeignKey(d => d.idMoneda)
                    .HasConstraintName("FK_tblAdmPedidoCliente_tblMoneda");

                entity.HasOne(d => d.idTipoPedidoNavigation)
                    .WithMany(p => p.tblAdmPedidoCliente)
                    .HasForeignKey(d => d.idTipoPedido)
                    .HasConstraintName("FK_tblAdmPedidoCliente_tblAdmTipoElemento");
            });

            modelBuilder.Entity<tblAdmPedidoProveedor>(entity =>
            {
                entity.HasKey(e => e.idAdmPedidoProveedor)
                    .HasName("PK__tblAdmPe__99A231D427BFE794");

                entity.HasOne(d => d.idAdmCentroCosteNavigation)
                    .WithMany(p => p.tblAdmPedidoProveedor)
                    .HasForeignKey(d => d.idAdmCentroCoste)
                    .HasConstraintName("FK_tblAdmPedidoProveedor_tblAdmCentroCoste");

                entity.HasOne(d => d.idAdmElementoPEPNavigation)
                    .WithMany(p => p.tblAdmPedidoProveedor)
                    .HasForeignKey(d => d.idAdmElementoPEP)
                    .HasConstraintName("FK__tblAdmPed__idAdm__0001A285");

                entity.HasOne(d => d.idAdmFormaPagoNavigation)
                    .WithMany(p => p.tblAdmPedidoProveedor)
                    .HasForeignKey(d => d.idAdmFormaPago)
                    .HasConstraintName("FK_tblAdmPedidoProveedor_tblAdmFormaPago");

                entity.HasOne(d => d.idAdmPedido_EstadoNavigation)
                    .WithMany(p => p.tblAdmPedidoProveedor)
                    .HasForeignKey(d => d.idAdmPedido_Estado)
                    .HasConstraintName("FK_tblAdmPedidoProveedor_tblAdmPedido_Estado");

                entity.HasOne(d => d.idAdmProveedorNavigation)
                    .WithMany(p => p.tblAdmPedidoProveedor)
                    .HasForeignKey(d => d.idAdmProveedor)
                    .HasConstraintName("FK_tblAdmPedidoProveedor_tblAdmProveedor");

                entity.HasOne(d => d.idAdmTipoCambioNavigation)
                    .WithMany(p => p.tblAdmPedidoProveedor)
                    .HasForeignKey(d => d.idAdmTipoCambio)
                    .HasConstraintName("FK_tblAdmPedidoProveedor_tblAdmTipoCambio");

                entity.HasOne(d => d.idAdmTipoDescuentoNavigation)
                    .WithMany(p => p.tblAdmPedidoProveedor)
                    .HasForeignKey(d => d.idAdmTipoDescuento)
                    .HasConstraintName("FK_tblAdmPedidoProveedor_tblAdmTipoDescuento");

                entity.HasOne(d => d.idCentroTrabajoNavigation)
                    .WithMany(p => p.tblAdmPedidoProveedor)
                    .HasForeignKey(d => d.idCentroTrabajo)
                    .HasConstraintName("FK__tblAdmPed__idCen__49E588F6");

                entity.HasOne(d => d.idCentroTrabajo1)
                    .WithMany(p => p.tblAdmPedidoProveedor)
                    .HasForeignKey(d => d.idCentroTrabajo)
                    .HasConstraintName("FK__tblAdmPed__idCen__4AD9AD2F");

                entity.HasOne(d => d.idEmpresaPolarierNavigation)
                    .WithMany(p => p.tblAdmPedidoProveedor)
                    .HasForeignKey(d => d.idEmpresaPolarier)
                    .HasConstraintName("FK_tblAdmPedidoProveedor_tblEmpresasPolarier");

                entity.HasOne(d => d.idIncotermNavigation)
                    .WithMany(p => p.tblAdmPedidoProveedor)
                    .HasForeignKey(d => d.idIncoterm)
                    .HasConstraintName("FK_tblAdmPedidoProveedor_tblIncoterm");

                entity.HasOne(d => d.idMonedaNavigation)
                    .WithMany(p => p.tblAdmPedidoProveedor)
                    .HasForeignKey(d => d.idMoneda)
                    .HasConstraintName("FK_tblAdmPedidoProveedor_tblMoneda");

                entity.HasOne(d => d.idTipoPedidoNavigation)
                    .WithMany(p => p.tblAdmPedidoProveedor)
                    .HasForeignKey(d => d.idTipoPedido)
                    .HasConstraintName("FK_tblAdmPedidoProveedor_tblAdmTipoElemento");
            });

            modelBuilder.Entity<tblAdmPedido_Estado>(entity =>
            {
                entity.HasKey(e => e.idAdmPedido_Estado)
                    .HasName("PK__tblAdmPe__8401CEAD2B3FE4A9");
            });

            modelBuilder.Entity<tblAdmPresupuestoVenta>(entity =>
            {
                entity.HasKey(e => e.idAdmPresupuestoVenta)
                    .HasName("PK__tblAdmPr__A2BBB5006F22D406");

                entity.HasOne(d => d.idAdmClienteNavigation)
                    .WithMany(p => p.tblAdmPresupuestoVenta)
                    .HasForeignKey(d => d.idAdmCliente)
                    .HasConstraintName("FK_tblAdmPresupuestoVenta_tblAdmCliente");

                entity.HasOne(d => d.idAdmFormaPagoNavigation)
                    .WithMany(p => p.tblAdmPresupuestoVenta)
                    .HasForeignKey(d => d.idAdmFormaPago)
                    .HasConstraintName("FK_tblAdmPresupuestoVenta_tblAdmFormaPago");

                entity.HasOne(d => d.idAdmPresupuestoVenta_EstadoNavigation)
                    .WithMany(p => p.tblAdmPresupuestoVenta)
                    .HasForeignKey(d => d.idAdmPresupuestoVenta_Estado)
                    .HasConstraintName("FK_tblAdmPresupuestoVenta_tblAdmPresupuestoVenta_Estado");

                entity.HasOne(d => d.idAdmTipoCambioNavigation)
                    .WithMany(p => p.tblAdmPresupuestoVenta)
                    .HasForeignKey(d => d.idAdmTipoCambio)
                    .HasConstraintName("FK_tblAdmPresupuestoVenta_tblAdmTipoCambio");

                entity.HasOne(d => d.idAdmTipoDescuentoNavigation)
                    .WithMany(p => p.tblAdmPresupuestoVenta)
                    .HasForeignKey(d => d.idAdmTipoDescuento)
                    .HasConstraintName("FK_tblAdmPresupuestoVenta_tblAdmTipoDescuento");

                entity.HasOne(d => d.idEmpresaPolarierNavigation)
                    .WithMany(p => p.tblAdmPresupuestoVenta)
                    .HasForeignKey(d => d.idEmpresaPolarier)
                    .HasConstraintName("FK_tblAdmPresupuestoVenta_tblEmpresasPolarier");

                entity.HasOne(d => d.idIvaNPaisNavigation)
                    .WithMany(p => p.tblAdmPresupuestoVenta)
                    .HasForeignKey(d => d.idIvaNPais)
                    .HasConstraintName("FK_tblAdmPresupuestoVenta_tblAdmIvaNPais");

                entity.HasOne(d => d.idMonedaNavigation)
                    .WithMany(p => p.tblAdmPresupuestoVenta)
                    .HasForeignKey(d => d.idMoneda)
                    .HasConstraintName("FK_tblAdmPresupuestoVenta_tblMoneda");

                entity.HasOne(d => d.idTipoPresupuestoNavigation)
                    .WithMany(p => p.tblAdmPresupuestoVenta)
                    .HasForeignKey(d => d.idTipoPresupuesto)
                    .HasConstraintName("FK_tblAdmPresupuestoVenta_tblAdmTipoElemento");
            });

            modelBuilder.Entity<tblAdmPresupuestoVenta_Estado>(entity =>
            {
                entity.HasKey(e => e.idAdmPresupuestoVenta_Estado)
                    .HasName("PK__tblAdmPr__94DD1FB1968A8076");
            });

            modelBuilder.Entity<tblAdmProveedor>(entity =>
            {
                entity.HasOne(d => d.idAdmCondicionPagoNavigation)
                    .WithMany(p => p.tblAdmProveedor)
                    .HasForeignKey(d => d.idAdmCondicionPago)
                    .HasConstraintName("FK__tblAdmPro__idAdm__4CC1F5A1");

                entity.HasOne(d => d.idAdmFormaPagoNavigation)
                    .WithMany(p => p.tblAdmProveedor)
                    .HasForeignKey(d => d.idAdmFormaPago)
                    .HasConstraintName("FK__tblAdmPro__idAdm__4BCDD168");

                entity.HasOne(d => d.idEmpresaPolarierNavigation)
                    .WithMany(p => p.tblAdmProveedor)
                    .HasForeignKey(d => d.idEmpresaPolarier)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAdmProveedor_tblEmpresasPolarier");

                entity.HasOne(d => d.idMonedaNavigation)
                    .WithMany(p => p.tblAdmProveedor)
                    .HasForeignKey(d => d.idMoneda)
                    .HasConstraintName("FK__tblAdmPro__idMon__4DB619DA");

                entity.HasOne(d => d.idPaisNavigation)
                    .WithMany(p => p.tblAdmProveedor)
                    .HasForeignKey(d => d.idPais)
                    .HasConstraintName("FK__tblAdmPro__idPai__4EAA3E13");
            });

            modelBuilder.Entity<tblAdmTipoArticulo>(entity =>
            {
                entity.Property(e => e.idAdmTipoArticulo).ValueGeneratedNever();

                entity.HasOne(d => d.idAdmCuentaContableCompraNavigation)
                    .WithMany(p => p.tblAdmTipoArticuloidAdmCuentaContableCompraNavigation)
                    .HasForeignKey(d => d.idAdmCuentaContableCompra)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAdmTipoArticuloCompra_tblAdmCuentaContable");

                entity.HasOne(d => d.idAdmCuentaContableVentaNavigation)
                    .WithMany(p => p.tblAdmTipoArticuloidAdmCuentaContableVentaNavigation)
                    .HasForeignKey(d => d.idAdmCuentaContableVenta)
                    .HasConstraintName("FK_tblAdmTipoArticuloVenta_tblAdmCuentaContable");
            });

            modelBuilder.Entity<tblAdmTipoCambio>(entity =>
            {
                entity.HasKey(e => e.idAdmTipoCambio)
                    .HasName("PK__tblAdmTi__E8CED6021C26FE38");
            });

            modelBuilder.Entity<tblAdmTipoDescuento>(entity =>
            {
                entity.HasKey(e => e.idAdmTipoDescuento)
                    .HasName("PK__tblAdmTi__E46C61F91DDB7D39");
            });

            modelBuilder.Entity<tblAdmTipoElemento>(entity =>
            {
                entity.HasKey(e => e.idAdmTipoElemento)
                    .HasName("PK__tblAdmTi__29EEC2D899922062");
            });

            modelBuilder.Entity<tblAdmTipoFactura>(entity =>
            {
                entity.HasKey(e => e.idTipoFactura)
                    .HasName("PK__tblAdmTi__8612AF5E9EE4DF8F");

                entity.Property(e => e.idTipoFactura).ValueGeneratedNever();
            });

            modelBuilder.Entity<tblAdmTipoNCF>(entity =>
            {
                entity.Property(e => e.prefijo).IsFixedLength();
            });

            modelBuilder.Entity<tblAjustePresupuestario>(entity =>
            {
                entity.HasOne(d => d.idAdmCentroCosteNavigation)
                    .WithMany(p => p.tblAjustePresupuestario)
                    .HasForeignKey(d => d.idAdmCentroCoste)
                    .HasConstraintName("FK_tblAjustePresupuestario_tblAdmCentroCoste");

                entity.HasOne(d => d.idAdmCuentaContableNavigation)
                    .WithMany(p => p.tblAjustePresupuestario)
                    .HasForeignKey(d => d.idAdmCuentaContable)
                    .HasConstraintName("FK_tblAjustePresupuestario_tblAdmCuentaContable");

                entity.HasOne(d => d.idAdmElementoPEPNavigation)
                    .WithMany(p => p.tblAjustePresupuestario)
                    .HasForeignKey(d => d.idAdmElementoPEP)
                    .HasConstraintName("FK_tblAjustePresupuestario_tblAdmElementoPEP");

                entity.HasOne(d => d.idMonedaNavigation)
                    .WithMany(p => p.tblAjustePresupuestario)
                    .HasForeignKey(d => d.idMoneda)
                    .HasConstraintName("FK_tblAjustePresupuestario_tblMoneda");
            });

            modelBuilder.Entity<tblAlmacen>(entity =>
            {
                entity.HasOne(d => d.idAlmacenPadreNavigation)
                    .WithMany(p => p.InverseidAlmacenPadreNavigation)
                    .HasForeignKey(d => d.idAlmacenPadre)
                    .HasConstraintName("FK_tblAlmacen_tblAlmacen");

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblAlmacen)
                    .HasForeignKey(d => d.idEntidad)
                    .HasConstraintName("FK_tblAlmacen_tblEntidad");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblAlmacen)
                    .HasForeignKey(d => d.idLavanderia)
                    .HasConstraintName("FK_tblAlmacen_tblLavanderia");

                entity.HasOne(d => d.idSeccionNivel2Navigation)
                    .WithMany(p => p.tblAlmacen)
                    .HasForeignKey(d => d.idSeccionNivel2)
                    .HasConstraintName("FK_tblAlmacen_tblSeccionNivel2");

                entity.HasOne(d => d.idTipoAlmacenNavigation)
                    .WithMany(p => p.tblAlmacen)
                    .HasForeignKey(d => d.idTipoAlmacen)
                    .HasConstraintName("FK_tblAlmacen_tblTipoAlmacen");

                entity.HasOne(d => d.idTipoHabitacionNavigation)
                    .WithMany(p => p.tblAlmacen)
                    .HasForeignKey(d => d.idTipoHabitacion)
                    .HasConstraintName("FK_tblAlmacen_tblTipoHabitacion");
            });

            modelBuilder.Entity<tblAlmacenNInventario>(entity =>
            {
                entity.HasKey(e => new { e.idInventario, e.idAlmacen })
                    .HasName("PK__tblAlmac__6AE8135120E2C2B0");

                entity.HasOne(d => d.idAlmacenNavigation)
                    .WithMany(p => p.tblAlmacenNInventario)
                    .HasForeignKey(d => d.idAlmacen)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAlmacenNInventario_tblAlmacen");

                entity.HasOne(d => d.idInventarioNavigation)
                    .WithMany(p => p.tblAlmacenNInventario)
                    .HasForeignKey(d => d.idInventario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAlmacenNInventario_tblInventario");

                entity.HasOne(d => d.idPersonaNavigation)
                    .WithMany(p => p.tblAlmacenNInventario)
                    .HasForeignKey(d => d.idPersona)
                    .HasConstraintName("FK_tblAlmacenNInventario_tblPersona");

                entity.HasOne(d => d.idTipoAlmacenNavigation)
                    .WithMany(p => p.tblAlmacenNInventario)
                    .HasForeignKey(d => d.idTipoAlmacen)
                    .HasConstraintName("FK__tblAlmace__idTip__7B5130AA");

                entity.HasOne(d => d.idTipoHabitacionNavigation)
                    .WithMany(p => p.tblAlmacenNInventario)
                    .HasForeignKey(d => d.idTipoHabitacion)
                    .HasConstraintName("FK__tblAlmace__idTip__7C4554E3");
            });

            modelBuilder.Entity<tblAlmacenNRuta>(entity =>
            {
                entity.HasKey(e => new { e.idAlmacen, e.idRuta })
                    .HasName("PK__tblAlmac__019CCBA012945432");

                entity.HasOne(d => d.idAlmacenNavigation)
                    .WithMany(p => p.tblAlmacenNRuta)
                    .HasForeignKey(d => d.idAlmacen)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAlmacenNRuta_tblAlmacen");

                entity.HasOne(d => d.idRutaNavigation)
                    .WithMany(p => p.tblAlmacenNRuta)
                    .HasForeignKey(d => d.idRuta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAlmacenNRuta_tblRuta");
            });

            modelBuilder.Entity<tblAlmacenRecambios>(entity =>
            {
                entity.Property(e => e.activo).HasDefaultValueSql("((1))");

                entity.Property(e => e.centroCoste).IsFixedLength();

                entity.HasOne(d => d.idAlmacenPadreNavigation)
                    .WithMany(p => p.InverseidAlmacenPadreNavigation)
                    .HasForeignKey(d => d.idAlmacenPadre)
                    .HasConstraintName("FK_tblAlmacenRecambios_tblAlmacenRecambios");

                entity.HasOne(d => d.idMonedaNavigation)
                    .WithMany(p => p.tblAlmacenRecambios)
                    .HasForeignKey(d => d.idMoneda)
                    .HasConstraintName("FK_tblRecambioNParteTrabajo_tblMoneda");

                entity.HasOne(d => d.idPaisNavigation)
                    .WithMany(p => p.tblAlmacenRecambios)
                    .HasForeignKey(d => d.idPais)
                    .HasConstraintName("FK_tblAlmacenRecambios_tblPais");

                entity.HasOne(d => d.idTipoSalidaRecambioNavigation)
                    .WithMany(p => p.tblAlmacenRecambios)
                    .HasForeignKey(d => d.idTipoSalidaRecambio)
                    .HasConstraintName("FK__tblAlmace__idTip__7B320070");
            });

            modelBuilder.Entity<tblAlmacenRecambiosNPersona>(entity =>
            {
                entity.HasKey(e => new { e.idAlmacen, e.idPersona });

                entity.HasOne(d => d.idAlmacenNavigation)
                    .WithMany(p => p.tblAlmacenRecambiosNPersona)
                    .HasForeignKey(d => d.idAlmacen)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAlmacenRecambiosNPersona_tblAlmacenRecambios");

                entity.HasOne(d => d.idPersonaNavigation)
                    .WithMany(p => p.tblAlmacenRecambiosNPersona)
                    .HasForeignKey(d => d.idPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAlmacenRecambiosNPersona_tblPersona");
            });

            modelBuilder.Entity<tblApartado>(entity =>
            {
                entity.Property(e => e.idApartado).ValueGeneratedNever();

                entity.HasOne(d => d.idApartadoPadreNavigation)
                    .WithMany(p => p.InverseidApartadoPadreNavigation)
                    .HasForeignKey(d => d.idApartadoPadre)
                    .HasConstraintName("FK_tblApartado_tblApartado1");

                entity.HasOne(d => d.idTraduccionNavigation)
                    .WithMany(p => p.tblApartado)
                    .HasForeignKey(d => d.idTraduccion)
                    .HasConstraintName("FK_tblApartado_tblTraduccion");
            });

            modelBuilder.Entity<tblAplicacion>(entity =>
            {
                entity.Property(e => e.idAplicacion).ValueGeneratedNever();

                entity.HasOne(d => d.idFormularioInicioNavigation)
                    .WithMany(p => p.tblAplicacion)
                    .HasForeignKey(d => d.idFormularioInicio)
                    .HasConstraintName("FK_tblAplicacion_tblFormularioInicio");

                entity.HasOne(d => d.idTraduccionNavigation)
                    .WithMany(p => p.tblAplicacion)
                    .HasForeignKey(d => d.idTraduccion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAplicacion_tblTraduccion");
            });

            modelBuilder.Entity<tblArchivo>(entity =>
            {
                entity.HasKey(e => e.idArchivo)
                    .HasName("PK_tblArchivo_1");
            });

            modelBuilder.Entity<tblAreaLavanderia>(entity =>
            {
                entity.HasOne(d => d.idTraduccionNavigation)
                    .WithMany(p => p.tblAreaLavanderia)
                    .HasForeignKey(d => d.idTraduccion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAreaLavanderia_tblTraduccion");
            });

            modelBuilder.Entity<tblAreaLavanderiaNLavanderia>(entity =>
            {
                entity.HasKey(e => new { e.idAreaLavanderia, e.idLavanderia });

                entity.Property(e => e.visibleSmartArea).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.idAreaLavanderiaNavigation)
                    .WithMany(p => p.tblAreaLavanderiaNLavanderia)
                    .HasForeignKey(d => d.idAreaLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAreaLavanderiaNLavanderia_tblAreaLavanderia");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblAreaLavanderiaNLavanderia)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAreaLavanderiaNLavanderia_tblLavanderia");
            });

            modelBuilder.Entity<tblArticuloEnvio>(entity =>
            {
                entity.HasOne(d => d.idPackingListNavigation)
                    .WithMany(p => p.tblArticuloEnvio)
                    .HasForeignKey(d => d.idPackingList)
                    .HasConstraintName("FK_tblArticuloEnvio_tblPackingList");
            });

            modelBuilder.Entity<tblArticuloLenceria>(entity =>
            {
                entity.HasOne(d => d.idAdmCuentaContableCompraNavigation)
                    .WithMany(p => p.tblArticuloLenceriaidAdmCuentaContableCompraNavigation)
                    .HasForeignKey(d => d.idAdmCuentaContableCompra)
                    .HasConstraintName("FK_tblArticuloLenceriaCompra_tblAdmCuentaContable");

                entity.HasOne(d => d.idAdmCuentaContableVentaNavigation)
                    .WithMany(p => p.tblArticuloLenceriaidAdmCuentaContableVentaNavigation)
                    .HasForeignKey(d => d.idAdmCuentaContableVenta)
                    .HasConstraintName("FK_tblArticuloLenceriaVenta_tblAdmCuentaContable");

                entity.HasOne(d => d.idDenoPrendaNavigation)
                    .WithMany(p => p.tblArticuloLenceria)
                    .HasForeignKey(d => d.idDenoPrenda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblArticuloLenceria_tblDenoPrenda");
            });

            modelBuilder.Entity<tblArticuloLogistico>(entity =>
            {
                entity.HasKey(e => e.idArticuloLogistico)
                    .HasName("PK__tblArtic__69B51687F48CE318");

                entity.Property(e => e.eliminado).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.idAdmCuentaContableCompraNavigation)
                    .WithMany(p => p.tblArticuloLogisticoidAdmCuentaContableCompraNavigation)
                    .HasForeignKey(d => d.idAdmCuentaContableCompra)
                    .HasConstraintName("FK__tblArticu__idAdm__405C1EBC");

                entity.HasOne(d => d.idAdmCuentaContableVentaNavigation)
                    .WithMany(p => p.tblArticuloLogisticoidAdmCuentaContableVentaNavigation)
                    .HasForeignKey(d => d.idAdmCuentaContableVenta)
                    .HasConstraintName("FK__tblArticu__idAdm__415042F5");
            });

            modelBuilder.Entity<tblArticuloMaquinaria>(entity =>
            {
                entity.HasOne(d => d.idAdmCuentaContableCompraNavigation)
                    .WithMany(p => p.tblArticuloMaquinariaidAdmCuentaContableCompraNavigation)
                    .HasForeignKey(d => d.idAdmCuentaContableCompra)
                    .HasConstraintName("FK_tblArticuloMaquinariaCompra_tblAdmCuentaContable");

                entity.HasOne(d => d.idAdmCuentaContableVentaNavigation)
                    .WithMany(p => p.tblArticuloMaquinariaidAdmCuentaContableVentaNavigation)
                    .HasForeignKey(d => d.idAdmCuentaContableVenta)
                    .HasConstraintName("FK_tblArticuloMaquinariaVenta_tblAdmCuentaContable");

                entity.HasOne(d => d.idCategoriaMaquinaNavigation)
                    .WithMany(p => p.tblArticuloMaquinaria)
                    .HasForeignKey(d => d.idCategoriaMaquina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblArticuloMaquinaria_tblCategoriaMaquina");
            });

            modelBuilder.Entity<tblArticuloNAdmAlbaranCompra>(entity =>
            {
                entity.HasOne(d => d.idAdmAlbaranCompraNavigation)
                    .WithMany(p => p.tblArticuloNAdmAlbaranCompra)
                    .HasForeignKey(d => d.idAdmAlbaranCompra)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblArticuloNAdmAlbaranCompra_tblAdmAlbaranCompra");

                entity.HasOne(d => d.idAdmCuentaContableNavigation)
                    .WithMany(p => p.tblArticuloNAdmAlbaranCompra)
                    .HasForeignKey(d => d.idAdmCuentaContable)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblArticuloNAdmAlbaranCompra_tblAdmCuentaContable");

                entity.HasOne(d => d.idArticuloLenceriaNavigation)
                    .WithMany(p => p.tblArticuloNAdmAlbaranCompra)
                    .HasForeignKey(d => d.idArticuloLenceria)
                    .HasConstraintName("FK_tblArticuloNAdmAlbaranCompra_tblArticuloLenceria");

                entity.HasOne(d => d.idArticuloLogisticoNavigation)
                    .WithMany(p => p.tblArticuloNAdmAlbaranCompra)
                    .HasForeignKey(d => d.idArticuloLogistico)
                    .HasConstraintName("FK__tblArticu__idArt__47FD4084");

                entity.HasOne(d => d.idArticuloMaquinariaNavigation)
                    .WithMany(p => p.tblArticuloNAdmAlbaranCompra)
                    .HasForeignKey(d => d.idArticuloMaquinaria)
                    .HasConstraintName("FK_tblArticuloNAdmAlbaranCompra_tblArticuloMaquinaria");

                entity.HasOne(d => d.idGrupoArticulosNavigation)
                    .WithMany(p => p.tblArticuloNAdmAlbaranCompra)
                    .HasForeignKey(d => d.idGrupoArticulos)
                    .HasConstraintName("FK_tblArticuloNAdmAlbaranCompra_tblGrupoArticulos");

                entity.HasOne(d => d.idIvaNPaisNavigation)
                    .WithMany(p => p.tblArticuloNAdmAlbaranCompra)
                    .HasForeignKey(d => d.idIvaNPais)
                    .HasConstraintName("FK_tblArticuloNAdmAlbaranCompra_tblAdmIvaNPais");

                entity.HasOne(d => d.idRecambioNavigation)
                    .WithMany(p => p.tblArticuloNAdmAlbaranCompra)
                    .HasForeignKey(d => d.idRecambio)
                    .HasConstraintName("FK_tblArticuloNAdmAlbaranCompra_tblRecambio");
            });

            modelBuilder.Entity<tblArticuloNAdmAlbaranVenta>(entity =>
            {
                entity.HasOne(d => d.idAdmAlbaranVentaNavigation)
                    .WithMany(p => p.tblArticuloNAdmAlbaranVenta)
                    .HasForeignKey(d => d.idAdmAlbaranVenta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblArticuloNAdmAlbaranVenta_tblAdmAlbaranVenta");

                entity.HasOne(d => d.idAdmCuentaContableNavigation)
                    .WithMany(p => p.tblArticuloNAdmAlbaranVenta)
                    .HasForeignKey(d => d.idAdmCuentaContable)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblArticuloNAdmAlbaranVenta_tblAdmCuentaContable");

                entity.HasOne(d => d.idArticuloLenceriaNavigation)
                    .WithMany(p => p.tblArticuloNAdmAlbaranVenta)
                    .HasForeignKey(d => d.idArticuloLenceria)
                    .HasConstraintName("FK_tblArticuloNAdmAlbaranVenta_tblArticuloLenceria");

                entity.HasOne(d => d.idArticuloLogisticoNavigation)
                    .WithMany(p => p.tblArticuloNAdmAlbaranVenta)
                    .HasForeignKey(d => d.idArticuloLogistico)
                    .HasConstraintName("FK__tblArticu__idArt__48F164BD");

                entity.HasOne(d => d.idArticuloMaquinariaNavigation)
                    .WithMany(p => p.tblArticuloNAdmAlbaranVenta)
                    .HasForeignKey(d => d.idArticuloMaquinaria)
                    .HasConstraintName("FK_tblArticuloNAdmAlbaranVenta_tblArticuloMaquinaria");

                entity.HasOne(d => d.idGrupoArticulosNavigation)
                    .WithMany(p => p.tblArticuloNAdmAlbaranVenta)
                    .HasForeignKey(d => d.idGrupoArticulos)
                    .HasConstraintName("FK_tblArticuloNAdmAlbaranVenta_tblGrupoArticulos");

                entity.HasOne(d => d.idRecambioNavigation)
                    .WithMany(p => p.tblArticuloNAdmAlbaranVenta)
                    .HasForeignKey(d => d.idRecambio)
                    .HasConstraintName("FK_tblArticuloNAdmAlbaranVenta_tblRecambio");
            });

            modelBuilder.Entity<tblArticuloNAdmPedidoCliente>(entity =>
            {
                entity.HasOne(d => d.idAdmCuentaContableNavigation)
                    .WithMany(p => p.tblArticuloNAdmPedidoCliente)
                    .HasForeignKey(d => d.idAdmCuentaContable)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblArticuloNAdmPedidoCliente_tblAdmCuentaContable");

                entity.HasOne(d => d.idAdmPedidoClienteNavigation)
                    .WithMany(p => p.tblArticuloNAdmPedidoCliente)
                    .HasForeignKey(d => d.idAdmPedidoCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblArticuloNAdmPedidoCliente_tblAdmPedidoCliente");

                entity.HasOne(d => d.idArticuloLenceriaNavigation)
                    .WithMany(p => p.tblArticuloNAdmPedidoCliente)
                    .HasForeignKey(d => d.idArticuloLenceria)
                    .HasConstraintName("FK_tblArticuloNAdmPedidoCliente_tblArticuloLenceria");

                entity.HasOne(d => d.idArticuloLogisticoNavigation)
                    .WithMany(p => p.tblArticuloNAdmPedidoCliente)
                    .HasForeignKey(d => d.idArticuloLogistico)
                    .HasConstraintName("FK__tblArticu__idArt__4614F812");

                entity.HasOne(d => d.idArticuloMaquinariaNavigation)
                    .WithMany(p => p.tblArticuloNAdmPedidoCliente)
                    .HasForeignKey(d => d.idArticuloMaquinaria)
                    .HasConstraintName("FK_tblArticuloNAdmPedidoCliente_tblArticuloMaquinaria");

                entity.HasOne(d => d.idGrupoArticulosNavigation)
                    .WithMany(p => p.tblArticuloNAdmPedidoCliente)
                    .HasForeignKey(d => d.idGrupoArticulos)
                    .HasConstraintName("FK_tblArticuloNAdmPedidoCliente_tblGrupoArticulos");

                entity.HasOne(d => d.idRecambioNavigation)
                    .WithMany(p => p.tblArticuloNAdmPedidoCliente)
                    .HasForeignKey(d => d.idRecambio)
                    .HasConstraintName("FK_tblArticuloNAdmPedidoCliente_tblRecambio");
            });

            modelBuilder.Entity<tblArticuloNAdmPedidoProveedor>(entity =>
            {
                entity.HasOne(d => d.idAdmCuentaContableNavigation)
                    .WithMany(p => p.tblArticuloNAdmPedidoProveedor)
                    .HasForeignKey(d => d.idAdmCuentaContable)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblArticuloNAdmPedidoProveedor_tblAdmCuentaContable");

                entity.HasOne(d => d.idAdmPedidoProveedorNavigation)
                    .WithMany(p => p.tblArticuloNAdmPedidoProveedor)
                    .HasForeignKey(d => d.idAdmPedidoProveedor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblArticuloNAdmPedidoProveedor_tblAdmPedidoProveedor");

                entity.HasOne(d => d.idArticuloLenceriaNavigation)
                    .WithMany(p => p.tblArticuloNAdmPedidoProveedor)
                    .HasForeignKey(d => d.idArticuloLenceria)
                    .HasConstraintName("FK_tblArticuloNAdmPedidoProveedor_tblArticuloLenceria");

                entity.HasOne(d => d.idArticuloLogisticoNavigation)
                    .WithMany(p => p.tblArticuloNAdmPedidoProveedor)
                    .HasForeignKey(d => d.idArticuloLogistico)
                    .HasConstraintName("FK__tblArticu__idArt__4520D3D9");

                entity.HasOne(d => d.idArticuloMaquinariaNavigation)
                    .WithMany(p => p.tblArticuloNAdmPedidoProveedor)
                    .HasForeignKey(d => d.idArticuloMaquinaria)
                    .HasConstraintName("FK_tblArticuloNAdmPedidoProveedor_tblArticuloMaquinaria");

                entity.HasOne(d => d.idGrupoArticulosNavigation)
                    .WithMany(p => p.tblArticuloNAdmPedidoProveedor)
                    .HasForeignKey(d => d.idGrupoArticulos)
                    .HasConstraintName("FK_tblArticuloNAdmPedidoProveedor_tblGrupoArticulos");

                entity.HasOne(d => d.idIvaNPaisNavigation)
                    .WithMany(p => p.tblArticuloNAdmPedidoProveedor)
                    .HasForeignKey(d => d.idIvaNPais)
                    .HasConstraintName("FK_tblArticuloNAdmPedidoProveedor_tblAdmIvaNPais");

                entity.HasOne(d => d.idRecambioNavigation)
                    .WithMany(p => p.tblArticuloNAdmPedidoProveedor)
                    .HasForeignKey(d => d.idRecambio)
                    .HasConstraintName("FK_tblArticuloNAdmPedidoProveedor_tblRecambio");
            });

            modelBuilder.Entity<tblArticuloNAdmPresupuestoVenta>(entity =>
            {
                entity.HasOne(d => d.idAdmCuentaContableNavigation)
                    .WithMany(p => p.tblArticuloNAdmPresupuestoVenta)
                    .HasForeignKey(d => d.idAdmCuentaContable)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblArticuloNAdmPresupuestoVenta_tblAdmCuentaContable");

                entity.HasOne(d => d.idAdmPresupuestoVentaNavigation)
                    .WithMany(p => p.tblArticuloNAdmPresupuestoVenta)
                    .HasForeignKey(d => d.idAdmPresupuestoVenta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblArticuloNAdmPresupuestoVenta_tblAdmPresupuestoVenta");

                entity.HasOne(d => d.idArticuloLenceriaNavigation)
                    .WithMany(p => p.tblArticuloNAdmPresupuestoVenta)
                    .HasForeignKey(d => d.idArticuloLenceria)
                    .HasConstraintName("FK_tblArticuloNAdmPresupuestoVenta_tblArticuloLenceria");

                entity.HasOne(d => d.idArticuloLogisticoNavigation)
                    .WithMany(p => p.tblArticuloNAdmPresupuestoVenta)
                    .HasForeignKey(d => d.idArticuloLogistico)
                    .HasConstraintName("FK__tblArticu__idArt__47091C4B");

                entity.HasOne(d => d.idArticuloMaquinariaNavigation)
                    .WithMany(p => p.tblArticuloNAdmPresupuestoVenta)
                    .HasForeignKey(d => d.idArticuloMaquinaria)
                    .HasConstraintName("FK_tblArticuloNAdmPresupuestoVenta_tblArticuloMaquinaria");

                entity.HasOne(d => d.idGrupoArticulosNavigation)
                    .WithMany(p => p.tblArticuloNAdmPresupuestoVenta)
                    .HasForeignKey(d => d.idGrupoArticulos)
                    .HasConstraintName("FK_tblArticuloNAdmPresupuestoVenta_tblGrupoArticulos");

                entity.HasOne(d => d.idRecambioNavigation)
                    .WithMany(p => p.tblArticuloNAdmPresupuestoVenta)
                    .HasForeignKey(d => d.idRecambio)
                    .HasConstraintName("FK_tblArticuloNAdmPresupuestoVenta_tblRecambio");
            });

            modelBuilder.Entity<tblBacsCarro>(entity =>
            {
                entity.HasKey(e => new { e.idLavanderia, e.idFamilia });

                entity.HasOne(d => d.idFamiliaNavigation)
                    .WithMany(p => p.tblBacsCarro)
                    .HasForeignKey(d => d.idFamilia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblBacsCarro_tblFamilia");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblBacsCarro)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblBacsCarro_tblLavanderia");
            });

            modelBuilder.Entity<tblBalanceHoras>(entity =>
            {
                entity.HasKey(e => e.idBalanceHoras)
                    .HasName("PK__tblBalan__6A9DA9113B84F5F4");

                entity.HasOne(d => d.idJornadaNavigation)
                    .WithMany(p => p.tblBalanceHoras)
                    .HasForeignKey(d => d.idJornada)
                    .HasConstraintName("FK_tblBalanceHoras_tblJornada");

                entity.HasOne(d => d.idPersonaNavigation)
                    .WithMany(p => p.tblBalanceHoras)
                    .HasForeignKey(d => d.idPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblBalanceHoras_tblPersona");
            });

            modelBuilder.Entity<tblBalanceHorasExtra>(entity =>
            {
                entity.HasKey(e => e.idBalanceHorasExtra)
                    .HasName("PK__tblBalan__A5E61D83E09974FA");

                entity.HasOne(d => d.idJornadaNavigation)
                    .WithMany(p => p.tblBalanceHorasExtra)
                    .HasForeignKey(d => d.idJornada)
                    .HasConstraintName("FK_tblBalanceHorasExtra_tblJornada");

                entity.HasOne(d => d.idPersonaNavigation)
                    .WithMany(p => p.tblBalanceHorasExtra)
                    .HasForeignKey(d => d.idPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblBalanceHorasExtra_tblPersona");
            });

            modelBuilder.Entity<tblCalendarioCentroTrabajo>(entity =>
            {
                entity.HasKey(e => new { e.idCentroTrabajo, e.fecha, e.idCalendario_Estado });

                entity.HasOne(d => d.idCalendario_EstadoNavigation)
                    .WithMany(p => p.tblCalendarioCentroTrabajo)
                    .HasForeignKey(d => d.idCalendario_Estado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCalendarioCentroTrabajo_tblCalendarioPersonal_Estado");

                entity.HasOne(d => d.idCentroTrabajoNavigation)
                    .WithMany(p => p.tblCalendarioCentroTrabajo)
                    .HasForeignKey(d => d.idCentroTrabajo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCalendarioCentroTrabajo_tblCentroTrabajo");
            });

            modelBuilder.Entity<tblCalendarioEntidad>(entity =>
            {
                entity.HasKey(e => new { e.idEntidad, e.fecha, e.idEstado });

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblCalendarioEntidad)
                    .HasForeignKey(d => d.idEntidad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCalendarioEntidad_tblEntidad");

                entity.HasOne(d => d.idEstadoNavigation)
                    .WithMany(p => p.tblCalendarioEntidad)
                    .HasForeignKey(d => d.idEstado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCalendarioEntidad_tblCalendarioEntidad_Estados");
            });

            modelBuilder.Entity<tblCalendarioEntidad_Estados>(entity =>
            {
                entity.HasOne(d => d.idTipoEstadoNavigation)
                    .WithMany(p => p.tblCalendarioEntidad_Estados)
                    .HasForeignKey(d => d.idTipoEstado)
                    .HasConstraintName("FK_tblCalendarioEntidad_Estados_tblCalendario_TipoEstado");

                entity.HasOne(d => d.idTraduccionNavigation)
                    .WithMany(p => p.tblCalendarioEntidad_Estados)
                    .HasForeignKey(d => d.idTraduccion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCalendarioEntidad_Estados_tblTraduccion");
            });

            modelBuilder.Entity<tblCalendarioLavanderia>(entity =>
            {
                entity.HasKey(e => new { e.idLavanderia, e.fecha, e.idCalendario_Estado })
                    .HasName("PK__tblCalen__1915FA2B17976045");

                entity.HasOne(d => d.idCalendario_EstadoNavigation)
                    .WithMany(p => p.tblCalendarioLavanderia)
                    .HasForeignKey(d => d.idCalendario_Estado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCalendarioLavanderia_tblCalendarioPersonal_Estado");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblCalendarioLavanderia)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCalendarioLavanderia_tblLavanderia");
            });

            modelBuilder.Entity<tblCalendarioPersonal>(entity =>
            {
                entity.HasKey(e => new { e.fecha, e.idPersona })
                    .HasName("PK__tblCalen__EB539B374F95C64A");

                entity.HasOne(d => d.idCalendario_EstadoNavigation)
                    .WithMany(p => p.tblCalendarioPersonal)
                    .HasForeignKey(d => d.idCalendario_Estado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblCalend__idCal__761910EF");

                entity.HasOne(d => d.idCuadrantePersonalNavigation)
                    .WithMany(p => p.tblCalendarioPersonal)
                    .HasForeignKey(d => d.idCuadrantePersonal)
                    .HasConstraintName("FK_tblCalendarioPersonal_tblCuadrantePersonal");

                entity.HasOne(d => d.idJornadaNavigation)
                    .WithMany(p => p.tblCalendarioPersonal)
                    .HasForeignKey(d => d.idJornada)
                    .HasConstraintName("FK_tblCalendarioPersonal_tblJornada");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblCalendarioPersonal)
                    .HasForeignKey(d => d.idLavanderia)
                    .HasConstraintName("FK_tblCalendarioPersonal_tblLavanderia");

                entity.HasOne(d => d.idPersonaNavigation)
                    .WithMany(p => p.tblCalendarioPersonal)
                    .HasForeignKey(d => d.idPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCalendarioPersonal_idPersona");

                entity.HasOne(d => d.idUsuario_validacionNavigation)
                    .WithMany(p => p.tblCalendarioPersonal)
                    .HasForeignKey(d => d.idUsuario_validacion)
                    .HasConstraintName("FK_tblCalendarioPersonal_tblUsuario");
            });

            modelBuilder.Entity<tblCalendario_Estado>(entity =>
            {
                entity.HasKey(e => e.idCalendario_Estado)
                    .HasName("PK__tblCalen__62EA894AF8AE780C");

                entity.HasOne(d => d.idTraduccionNavigation)
                    .WithMany(p => p.tblCalendario_Estado)
                    .HasForeignKey(d => d.idTraduccion)
                    .HasConstraintName("FK_tblCalendarioPersonal_Estado_tblTraduccion");
            });

            modelBuilder.Entity<tblCalendario_TipoEstado>(entity =>
            {
                entity.Property(e => e.idTipoEstado).ValueGeneratedNever();
            });

            modelBuilder.Entity<tblCampañaEncuesta>(entity =>
            {
                entity.HasOne(d => d.idEncuestaPlantillaNavigation)
                    .WithMany(p => p.tblCampañaEncuesta)
                    .HasForeignKey(d => d.idEncuestaPlantilla)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCampañaEncuesta_tblEncuestaPlantilla");
            });

            modelBuilder.Entity<tblCantidadNMovimientoElemLog>(entity =>
            {
                entity.HasOne(d => d.idColorTapaNavigation)
                    .WithMany(p => p.tblCantidadNMovimientoElemLog)
                    .HasForeignKey(d => d.idColorTapa)
                    .HasConstraintName("FK_tblCantidadNMovimientoElemLog_tblColorTapa");

                entity.HasOne(d => d.idGrupoPrendaEstNavigation)
                    .WithMany(p => p.tblCantidadNMovimientoElemLog)
                    .HasForeignKey(d => d.idGrupoPrendaEst)
                    .HasConstraintName("FK_tblCantidadNMovimientoElemLog_tblGrupoPrendaEst");

                entity.HasOne(d => d.idMarcaTapaNavigation)
                    .WithMany(p => p.tblCantidadNMovimientoElemLog)
                    .HasForeignKey(d => d.idMarcaTapa)
                    .HasConstraintName("FK_tblCantidadNMovimientoElemLog_tblMarcaTapa");

                entity.HasOne(d => d.idMovimientoElemLogNavigation)
                    .WithMany(p => p.tblCantidadNMovimientoElemLog)
                    .HasForeignKey(d => d.idMovimientoElemLog)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCantidadNMovimientoElemLog_tblMovimientoElemlog");

                entity.HasOne(d => d.idTipoElemLogNavigation)
                    .WithMany(p => p.tblCantidadNMovimientoElemLog)
                    .HasForeignKey(d => d.idTipoElemLog)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCantidadNMovimientoElemLog_tblTipoElemLog");
            });

            modelBuilder.Entity<tblCargo>(entity =>
            {
                entity.HasKey(e => e.idCargo)
                    .HasName("PK_tblNivelUsuario");

                entity.Property(e => e.idCargo).ValueGeneratedNever();

                entity.Property(e => e.estatus).HasDefaultValueSql("((2))");
            });

            modelBuilder.Entity<tblCarpetaDocumentos>(entity =>
            {
                entity.Property(e => e.idCarpetaDocumentos).ValueGeneratedNever();
            });

            modelBuilder.Entity<tblCarro>(entity =>
            {
                entity.HasKey(e => e.idCarro)
                    .HasName("PK__tblCarro__3D09E20E733CA8B9");
            });

            modelBuilder.Entity<tblCategoriaAbono>(entity =>
            {
                entity.HasOne(d => d.idTraduccionNavigation)
                    .WithMany(p => p.tblCategoriaAbono)
                    .HasForeignKey(d => d.idTraduccion)
                    .HasConstraintName("FK_tblCategoriaAbono_tblTraduccion");
            });

            modelBuilder.Entity<tblCategoriaConvenio>(entity =>
            {
                entity.HasOne(d => d.idPaisNavigation)
                    .WithMany(p => p.tblCategoriaConvenio)
                    .HasForeignKey(d => d.idPais)
                    .HasConstraintName("FK_tblCategoriaConvenio_tblPais");
            });

            modelBuilder.Entity<tblCategoriaInterna>(entity =>
            {
                entity.HasKey(e => e.idCategoriaInterna)
                    .HasName("PK_tblCategoriaSalarial");

                entity.HasOne(d => d.idCategoriaConvenioNavigation)
                    .WithMany(p => p.tblCategoriaInterna)
                    .HasForeignKey(d => d.idCategoriaConvenio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCategoriaSalarial_tblCategoriaConvenio");

                entity.HasMany(d => d.idUsuario)
                    .WithMany(p => p.idCategoriaInterna)
                    .UsingEntity<Dictionary<string, object>>(
                        "tblCategoriaInternaNUsuario",
                        l => l.HasOne<tblUsuario>().WithMany().HasForeignKey("idUsuario").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblCategoriaInternaNUsuario_tblUsuario"),
                        r => r.HasOne<tblCategoriaInterna>().WithMany().HasForeignKey("idCategoriaInterna").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblCategoriaInternaNUsuario_tblCategoriaInterna"),
                        j =>
                        {
                            j.HasKey("idCategoriaInterna", "idUsuario");

                            j.ToTable("tblCategoriaInternaNUsuario", "GestionInterna");
                        });
            });

            modelBuilder.Entity<tblCategoriaInternaNTurno>(entity =>
            {
                entity.HasKey(e => new { e.idCategoriaInterna, e.idTurno })
                    .HasName("PK_tblCategoriaSalarialNTurno");

                entity.HasOne(d => d.idCategoriaInternaNavigation)
                    .WithMany(p => p.tblCategoriaInternaNTurno)
                    .HasForeignKey(d => d.idCategoriaInterna)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCategoriaSalarialNTurno_tblCategoriaSalarial");

                entity.HasOne(d => d.idTurnoNavigation)
                    .WithMany(p => p.tblCategoriaInternaNTurno)
                    .HasForeignKey(d => d.idTurno)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCategoriaSalarialNTurno_tblTurno");
            });

            modelBuilder.Entity<tblCategoriaMaquina>(entity =>
            {
                entity.HasOne(d => d.idSistemaMaquinaNavigation)
                    .WithMany(p => p.tblCategoriaMaquina)
                    .HasForeignKey(d => d.idSistemaMaquina)
                    .HasConstraintName("FK_tblCategoriaMaquina_tblSistemaMaquina");
            });

            modelBuilder.Entity<tblCategoriaRecurso>(entity =>
            {
                entity.HasOne(d => d.idTraduccionNavigation)
                    .WithMany(p => p.tblCategoriaRecurso)
                    .HasForeignKey(d => d.idTraduccion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCategoriaRecurso_tblTraduccion");
            });

            modelBuilder.Entity<tblCategoria_Grupo>(entity =>
            {
                entity.Property(e => e.idCategoria_Grupo).ValueGeneratedNever();

                entity.HasMany(d => d.idCategoria)
                    .WithMany(p => p.idCategoria_Grupo)
                    .UsingEntity<Dictionary<string, object>>(
                        "tblCategoriaNGrupo",
                        l => l.HasOne<tblCategoria>().WithMany().HasForeignKey("idCategoria").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblCategoriaNGrupo_tblCategoria"),
                        r => r.HasOne<tblCategoria_Grupo>().WithMany().HasForeignKey("idCategoria_Grupo").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblCategoriaNGrupo_tblCategoria_Grupo"),
                        j =>
                        {
                            j.HasKey("idCategoria_Grupo", "idCategoria");

                            j.ToTable("tblCategoriaNGrupo", "RRHH");
                        });
            });

            modelBuilder.Entity<tblCentroTrabajo>(entity =>
            {
                entity.HasKey(e => e.idCentroTrabajo)
                    .HasName("PK__tblCentr__1F98260809EA0221");

                entity.HasOne(d => d.idPaisNavigation)
                    .WithMany(p => p.tblCentroTrabajo)
                    .HasForeignKey(d => d.idPais)
                    .HasConstraintName("FK_tblCentroTrabajo_tblPais");

                entity.HasMany(d => d.idUsuario)
                    .WithMany(p => p.idCentroTrabajo)
                    .UsingEntity<Dictionary<string, object>>(
                        "tblCentroTrabajoNUsuario",
                        l => l.HasOne<tblUsuario>().WithMany().HasForeignKey("idUsuario").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblCentroTrabajoNUsuario_tblUsuario"),
                        r => r.HasOne<tblCentroTrabajo>().WithMany().HasForeignKey("idCentroTrabajo").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblCentroTrabajoNUsuario_tblCentroTrabajo"),
                        j =>
                        {
                            j.HasKey("idCentroTrabajo", "idUsuario");

                            j.ToTable("tblCentroTrabajoNUsuario", "GestionInterna");
                        });
            });

            modelBuilder.Entity<tblCierreDatos_Facturacion>(entity =>
            {
                entity.HasKey(e => new { e.idEntidad, e.año, e.mes });

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblCierreDatos_Facturacion)
                    .HasForeignKey(d => d.idEntidad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCierreDatos_Facturacion_tblEntidad");

                entity.HasOne(d => d.idTipoConsumoLenceriaNavigation)
                    .WithMany(p => p.tblCierreDatos_Facturacion)
                    .HasForeignKey(d => d.idTipoConsumoLenceria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCierreDatos_Facturacion_tblTipoConsumoLenceria");

                entity.HasOne(d => d.idTipoFacturacionClienteNavigation)
                    .WithMany(p => p.tblCierreDatos_Facturacion)
                    .HasForeignKey(d => d.idTipoFacturacionCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCierreDatos_Facturacion_tblTipoFacturacionCliente");
            });

            modelBuilder.Entity<tblCierreDatos_Lavanderia>(entity =>
            {
                entity.HasKey(e => new { e.idLavanderia, e.año, e.mes });

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblCierreDatos_Lavanderia)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCierreDatos_Lavanderia_tblLavanderia");
            });

            modelBuilder.Entity<tblCierreDatos_Uniformidad>(entity =>
            {
                entity.HasKey(e => new { e.idLavanderia, e.año, e.mes });

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblCierreDatos_Uniformidad)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCierreDatos_Uniformidad_tblLavanderia");
            });

            modelBuilder.Entity<tblCierreDatos_Valet>(entity =>
            {
                entity.HasKey(e => new { e.idLavanderia, e.año, e.mes });

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblCierreDatos_Valet)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCierreDatos_Valet_tblLavanderia");
            });

            modelBuilder.Entity<tblCierreFactEntidad>(entity =>
            {
                entity.HasKey(e => new { e.idEntidad, e.fechaDesde, e.fechaHasta });

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblCierreFactEntidad)
                    .HasForeignKey(d => d.idEntidad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCierreFactEntidad_tblEntidad");
            });

            modelBuilder.Entity<tblCierrePresupuestario>(entity =>
            {
                entity.HasOne(d => d.idAdmCentroCosteNavigation)
                    .WithMany(p => p.tblCierrePresupuestario)
                    .HasForeignKey(d => d.idAdmCentroCoste)
                    .HasConstraintName("FK_tblCierrePresupuestario_tblAdmCentroCoste");

                entity.HasOne(d => d.idAdmElementoPEPNavigation)
                    .WithMany(p => p.tblCierrePresupuestario)
                    .HasForeignKey(d => d.idAdmElementoPEP)
                    .HasConstraintName("FK_tblCierrePresupuestario_tblAdmElementoPEP");

                entity.HasOne(d => d.idEmpresaPolarierNavigation)
                    .WithMany(p => p.tblCierrePresupuestario)
                    .HasForeignKey(d => d.idEmpresaPolarier)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCierrePresupuestario_tblEmpresasPolarier");
            });

            modelBuilder.Entity<tblCierreRecambioNAlmacen>(entity =>
            {
                entity.HasKey(e => new { e.fecha, e.idAlmacen, e.idRecambio });

                entity.HasOne(d => d.idAlmacenNavigation)
                    .WithMany(p => p.tblCierreRecambioNAlmacen)
                    .HasForeignKey(d => d.idAlmacen)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCierreRecambioNAlmacen_tblAlmacenRecambios");

                entity.HasOne(d => d.idRecambioNavigation)
                    .WithMany(p => p.tblCierreRecambioNAlmacen)
                    .HasForeignKey(d => d.idRecambio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCierreRecambioNAlmacen_tblRecambio");
            });

            modelBuilder.Entity<tblClienteNMaquina>(entity =>
            {
                entity.HasOne(d => d.idCompañiaNavigation)
                    .WithMany(p => p.tblClienteNMaquina)
                    .HasForeignKey(d => d.idCompañia)
                    .HasConstraintName("FK_tblClienteNMaquina_tblCompañia");

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblClienteNMaquina)
                    .HasForeignKey(d => d.idEntidad)
                    .HasConstraintName("FK_tblClienteNMaquina_tblEntidad");

                entity.HasOne(d => d.idFamiliaNavigation)
                    .WithMany(p => p.tblClienteNMaquina)
                    .HasForeignKey(d => d.idFamilia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblClienteNMaquina_tblFamilia");

                entity.HasOne(d => d.idGrupoPlantillaPrenda_genericaNavigation)
                    .WithMany(p => p.tblClienteNMaquina)
                    .HasForeignKey(d => d.idGrupoPlantillaPrenda_generica)
                    .HasConstraintName("FK_tblClienteNMaquina_tblGrupoPlantillaPrenda_generica");

                entity.HasOne(d => d.idMaquinaNavigation)
                    .WithMany(p => p.tblClienteNMaquina)
                    .HasForeignKey(d => d.idMaquina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblClienteNMaquina_tblMaquina");

                entity.HasOne(d => d.idTipoPrendaNavigation)
                    .WithMany(p => p.tblClienteNMaquina)
                    .HasForeignKey(d => d.idTipoPrenda)
                    .HasConstraintName("FK_tblClienteNMaquina_tblTipoPrenda");
            });

            modelBuilder.Entity<tblColorPrendaHuesped>(entity =>
            {
                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblColorPrendaHuesped)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblColorPrendaHuesped_tblLavanderia");
            });

            modelBuilder.Entity<tblColorTapa>(entity =>
            {
                entity.Property(e => e.idColorTapa).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<tblComentarioNCuentaContable>(entity =>
            {
                entity.HasOne(d => d.idAdmCentroCosteNavigation)
                    .WithMany(p => p.tblComentarioNCuentaContable)
                    .HasForeignKey(d => d.idAdmCentroCoste)
                    .HasConstraintName("FK_tblComentarioNCuentaContable_tblAdmCentroCoste");

                entity.HasOne(d => d.idAdmCuentaContableNavigation)
                    .WithMany(p => p.tblComentarioNCuentaContable)
                    .HasForeignKey(d => d.idAdmCuentaContable)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblComentarioNCuentaContable_tblAdmCuentaContable");

                entity.HasOne(d => d.idAdmElementoPEPNavigation)
                    .WithMany(p => p.tblComentarioNCuentaContable)
                    .HasForeignKey(d => d.idAdmElementoPEP)
                    .HasConstraintName("FK_tblComentarioNCuentaContable_tblAdmElementoPEP");

                entity.HasOne(d => d.idUsuarioNavigation)
                    .WithMany(p => p.tblComentarioNCuentaContable)
                    .HasForeignKey(d => d.idUsuario)
                    .HasConstraintName("FK_tblComentarioNCuentaContable_tblUsuario");
            });

            modelBuilder.Entity<tblCompañia>(entity =>
            {
                entity.Property(e => e.eliminado).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblCompañia)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCompañia_tblLavanderia");
            });

            modelBuilder.Entity<tblComunicado>(entity =>
            {
                entity.Property(e => e.isImportante).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<tblComunicadoNPersona>(entity =>
            {
                entity.HasKey(e => new { e.idPersona, e.idComunicado });

                entity.HasOne(d => d.idComunicadoNavigation)
                    .WithMany(p => p.tblComunicadoNPersona)
                    .HasForeignKey(d => d.idComunicado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblComunicadoNPersona_tblComunicado");

                entity.HasOne(d => d.idPersonaNavigation)
                    .WithMany(p => p.tblComunicadoNPersona)
                    .HasForeignKey(d => d.idPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblComunicadoNPersona_tblPersona");
            });

            modelBuilder.Entity<tblComunidadAutonoma>(entity =>
            {
                entity.Property(e => e.idComunidadAutonoma).ValueGeneratedNever();
            });

            modelBuilder.Entity<tblConceptoNomina>(entity =>
            {
                entity.HasKey(e => e.idConceptoNomina)
                    .HasName("PK__tblConce__75059612A64D3B3D");

                entity.Property(e => e.idConceptoNomina).ValueGeneratedNever();

                entity.HasOne(d => d.idTraduccionNavigation)
                    .WithMany(p => p.tblConceptoNomina)
                    .HasForeignKey(d => d.idTraduccion)
                    .HasConstraintName("FK_tblConceptoNomina_tblTraduccion");
            });

            modelBuilder.Entity<tblConceptoNominaNNomina>(entity =>
            {
                entity.HasKey(e => new { e.idNomina, e.idConceptoNomina })
                    .HasName("PK__tblConce__8C3DEF12DA4C516D");

                entity.HasOne(d => d.idConceptoNominaNavigation)
                    .WithMany(p => p.tblConceptoNominaNNomina)
                    .HasForeignKey(d => d.idConceptoNomina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblConceptoNominaNNomina_tblConceptoNomina");

                entity.HasOne(d => d.idNominaNavigation)
                    .WithMany(p => p.tblConceptoNominaNNomina)
                    .HasForeignKey(d => d.idNomina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblConceptoNominaNNomina_tblNomina");
            });

            modelBuilder.Entity<tblConceptoNominaNNomina_Gestoria>(entity =>
            {
                entity.HasKey(e => new { e.idNomina, e.idConceptoNomina })
                    .HasName("PK__tblConce__8C3DEF1213E107EA");

                entity.HasOne(d => d.idConceptoNominaNavigation)
                    .WithMany(p => p.tblConceptoNominaNNomina_Gestoria)
                    .HasForeignKey(d => d.idConceptoNomina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblConceptoNominaNNomina_Gestoria_tblConceptoNomina");

                entity.HasOne(d => d.idNominaNavigation)
                    .WithMany(p => p.tblConceptoNominaNNomina_Gestoria)
                    .HasForeignKey(d => d.idNomina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblConceptoNominaNNomina_Gestoria_tblNomina");
            });

            modelBuilder.Entity<tblConfigAlbaranReparto>(entity =>
            {
                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblConfigAlbaranReparto)
                    .HasForeignKey(d => d.idEntidad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblConfigAlbaranReparto_tblEntidad");
            });

            modelBuilder.Entity<tblConfiguracionSalarial>(entity =>
            {
                entity.Property(e => e.percFestivoTrabajado).HasDefaultValueSql("((1))");

                entity.Property(e => e.percQuinquenio).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.idCentroTrabajoNavigation)
                    .WithMany(p => p.tblConfiguracionSalarial)
                    .HasForeignKey(d => d.idCentroTrabajo)
                    .HasConstraintName("FK_tblConfiguracionSalarial_tblCentroTrabajo");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblConfiguracionSalarial)
                    .HasForeignKey(d => d.idLavanderia)
                    .HasConstraintName("FK_tblConfiguracionSalarial_tblLavanderia");
            });

            modelBuilder.Entity<tblControlAcceso>(entity =>
            {
                entity.HasKey(e => new { e.idPersona, e.fecha });

                entity.HasOne(d => d.idPersonaNavigation)
                    .WithMany(p => p.tblControlAcceso)
                    .HasForeignKey(d => d.idPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblControlAcceso_tblPersona");

                entity.HasOne(d => d.idTipoAccesoNavigation)
                    .WithMany(p => p.tblControlAcceso)
                    .HasForeignKey(d => d.idTipoAcceso)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblControlAcceso_tblTipoAcceso");
            });

            modelBuilder.Entity<tblControlContador>(entity =>
            {
                entity.HasKey(e => new { e.idRecursoContador, e.fecha });

                entity.HasOne(d => d.idRecursoContadorNavigation)
                    .WithMany(p => p.tblControlContador)
                    .HasForeignKey(d => d.idRecursoContador)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblControlContador_tblRecursoContador");
            });

            modelBuilder.Entity<tblControlNivel>(entity =>
            {
                entity.HasKey(e => new { e.idRecursoNivel, e.fecha });

                entity.HasOne(d => d.idRecursoNivelNavigation)
                    .WithMany(p => p.tblControlNivel)
                    .HasForeignKey(d => d.idRecursoNivel)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblControlNivel_tblRecursoNivel");
            });

            modelBuilder.Entity<tblCorreoAltaGestoriaNCentroLav>(entity =>
            {
                entity.HasOne(d => d.idCentroTrabajoNavigation)
                    .WithMany(p => p.tblCorreoAltaGestoriaNCentroLav)
                    .HasForeignKey(d => d.idCentroTrabajo)
                    .HasConstraintName("FK_tblCorreoAltaGestoriaNCentroLav_tblCentroTrabajo");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblCorreoAltaGestoriaNCentroLav)
                    .HasForeignKey(d => d.idLavanderia)
                    .HasConstraintName("FK_tblCorreoAltaGestoriaNCentroLav_tblLavanderia");
            });

            modelBuilder.Entity<tblCorreosNEntidad>(entity =>
            {
                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblCorreosNEntidad)
                    .HasForeignKey(d => d.idEntidad)
                    .HasConstraintName("FK_tblCorreosNEntidad_tblEntidad");

                entity.HasMany(d => d.idReport)
                    .WithMany(p => p.idCorreo)
                    .UsingEntity<Dictionary<string, object>>(
                        "tblCorreosNEntidadNReport",
                        l => l.HasOne<tblReports>().WithMany().HasForeignKey("idReport").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblCorreosNEntidadNReport_tblReports"),
                        r => r.HasOne<tblCorreosNEntidad>().WithMany().HasForeignKey("idCorreo").HasConstraintName("FK_tblCorreosNEntidadNReport_tblCorreosNEntidad"),
                        j =>
                        {
                            j.HasKey("idCorreo", "idReport");

                            j.ToTable("tblCorreosNEntidadNReport", "General");
                        });
            });

            modelBuilder.Entity<tblCorreosNLav>(entity =>
            {
                entity.HasKey(e => e.idCorreo)
                    .HasName("PK_tblCorreosIncNLav");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblCorreosNLav)
                    .HasForeignKey(d => d.idLavanderia)
                    .HasConstraintName("FK_tblCorreosNLav_tblLavanderia");

                entity.HasMany(d => d.idEnvio)
                    .WithMany(p => p.idCorreo)
                    .UsingEntity<Dictionary<string, object>>(
                        "tblCorreosNEnvio",
                        l => l.HasOne<tblEnvio>().WithMany().HasForeignKey("idEnvio").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblCorreosNEnvio_tblEnvio"),
                        r => r.HasOne<tblCorreosNLav>().WithMany().HasForeignKey("idCorreo").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblCorreosNEnvio_tblCorreosNLav"),
                        j =>
                        {
                            j.HasKey("idCorreo", "idEnvio");

                            j.ToTable("tblCorreosNEnvio", "General");
                        });

                entity.HasMany(d => d.idReport)
                    .WithMany(p => p.idCorreoNavigation)
                    .UsingEntity<Dictionary<string, object>>(
                        "tblCorreosNLavNReport",
                        l => l.HasOne<tblReports>().WithMany().HasForeignKey("idReport").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblCorreosNLavNReport_tblReports"),
                        r => r.HasOne<tblCorreosNLav>().WithMany().HasForeignKey("idCorreo").HasConstraintName("FK_tblCorreosNLavNReport_tblCorreosNLav"),
                        j =>
                        {
                            j.HasKey("idCorreo", "idReport");

                            j.ToTable("tblCorreosNLavNReport", "General");
                        });

                entity.HasMany(d => d.idTipoIncidencia)
                    .WithMany(p => p.idCorreo)
                    .UsingEntity<Dictionary<string, object>>(
                        "tblCorreosNLavNTipoIncidencia",
                        l => l.HasOne<tblTipoIncidencia>().WithMany().HasForeignKey("idTipoIncidencia").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblCorreosIncNLavNTipoIncidencia_tblCorreosIncNLavNTipoIncidencia"),
                        r => r.HasOne<tblCorreosNLav>().WithMany().HasForeignKey("idCorreo").HasConstraintName("FK_tblCorreosIncNLavNTipoIncidencia_tblCorreosIncNLav"),
                        j =>
                        {
                            j.HasKey("idCorreo", "idTipoIncidencia").HasName("PK_tblCorreosIncNLavNTipoIncidencia");

                            j.ToTable("tblCorreosNLavNTipoIncidencia", "General");
                        });
            });

            modelBuilder.Entity<tblCriterioValoracion>(entity =>
            {
                entity.Property(e => e.idCriterioValoracion).ValueGeneratedNever();

                entity.HasOne(d => d.idModuloNavigation)
                    .WithMany(p => p.tblCriterioValoracion)
                    .HasForeignKey(d => d.idModulo)
                    .HasConstraintName("FK_tblCriterioValoracion_tblModulo");
            });

            modelBuilder.Entity<tblCriterioValoracionNLavanderia>(entity =>
            {
                entity.HasKey(e => new { e.idCriterioValoracion, e.idLavanderia });

                entity.HasOne(d => d.idCriterioValoracionNavigation)
                    .WithMany(p => p.tblCriterioValoracionNLavanderia)
                    .HasForeignKey(d => d.idCriterioValoracion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCriterioValoracionNLavanderia_tblCriterioValoracion");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblCriterioValoracionNLavanderia)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCriterioValoracionNLavanderia_tblLavanderia");
            });

            modelBuilder.Entity<tblCuadrantePersonal>(entity =>
            {
                entity.HasKey(e => e.idCuadrantePersonal)
                    .HasName("PK__tblCuadr__70070289A157763E");

                entity.HasOne(d => d.idCalendario_EstadoNavigation)
                    .WithMany(p => p.tblCuadrantePersonal)
                    .HasForeignKey(d => d.idCalendario_Estado)
                    .HasConstraintName("FK__tblCuadra__idEst__1FF970CB");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblCuadrantePersonal)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblCuadra__idLav__20ED9504");

                entity.HasOne(d => d.idPersonaNavigation)
                    .WithMany(p => p.tblCuadrantePersonal)
                    .HasForeignKey(d => d.idPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblCuadra__idPer__21E1B93D");

                entity.HasOne(d => d.idPosicionNAreaLavanderiaNLavanderiaNavigation)
                    .WithMany(p => p.tblCuadrantePersonal)
                    .HasForeignKey(d => d.idPosicionNAreaLavanderiaNLavanderia)
                    .HasConstraintName("FK__tblCuadra__idPos__22D5DD76");

                entity.HasOne(d => d.idTurnoNavigation)
                    .WithMany(p => p.tblCuadrantePersonal)
                    .HasForeignKey(d => d.idTurno)
                    .HasConstraintName("FK__tblCuadra__idTur__23CA01AF");

                entity.HasOne(d => d.idUsuario_validacionNavigation)
                    .WithMany(p => p.tblCuadrantePersonal)
                    .HasForeignKey(d => d.idUsuario_validacion)
                    .HasConstraintName("FK_tblCuadrantePersonal_tblUsuario");
            });

            modelBuilder.Entity<tblCuentaContableNCentroTrabajo>(entity =>
            {
                entity.Property(e => e.idCentroTrabajo).ValueGeneratedNever();

                entity.HasOne(d => d.idAdmCuentaContable_IMSS_MXNavigation)
                    .WithMany(p => p.tblCuentaContableNCentroTrabajoidAdmCuentaContable_IMSS_MXNavigation)
                    .HasForeignKey(d => d.idAdmCuentaContable_IMSS_MX)
                    .HasConstraintName("FK_tblCuentaContableNCentroTrabajo_tblAdmCuentaContable_IMSS_MX");

                entity.HasOne(d => d.idAdmCuentaContable_INFONAVIT_MXNavigation)
                    .WithMany(p => p.tblCuentaContableNCentroTrabajoidAdmCuentaContable_INFONAVIT_MXNavigation)
                    .HasForeignKey(d => d.idAdmCuentaContable_INFONAVIT_MX)
                    .HasConstraintName("FK_tblCuentaContableNCentroTrabajo_tblAdmCuentaContable_INFONAVIT_MX");

                entity.HasOne(d => d.idAdmCuentaContable_ImpEstatalNominas_MXNavigation)
                    .WithMany(p => p.tblCuentaContableNCentroTrabajoidAdmCuentaContable_ImpEstatalNominas_MXNavigation)
                    .HasForeignKey(d => d.idAdmCuentaContable_ImpEstatalNominas_MX)
                    .HasConstraintName("FK_tblCuentaContableNCentroTrabajo_tblAdmCuentaContable_ImpEstatalNominas_MX");

                entity.HasOne(d => d.idAdmCuentaContable_SAR_MXNavigation)
                    .WithMany(p => p.tblCuentaContableNCentroTrabajoidAdmCuentaContable_SAR_MXNavigation)
                    .HasForeignKey(d => d.idAdmCuentaContable_SAR_MX)
                    .HasConstraintName("FK_tblCuentaContableNCentroTrabajo_tblAdmCuentaContable_SAR_MX");

                entity.HasOne(d => d.idAdmCuentaContable_SSEmpresaNavigation)
                    .WithMany(p => p.tblCuentaContableNCentroTrabajoidAdmCuentaContable_SSEmpresaNavigation)
                    .HasForeignKey(d => d.idAdmCuentaContable_SSEmpresa)
                    .HasConstraintName("FK_tblCuentaContableNCentroTrabajo_tblAdmCuentaContable_SSEmpresa");

                entity.HasOne(d => d.idAdmCuentaContable_SalarioNavigation)
                    .WithMany(p => p.tblCuentaContableNCentroTrabajoidAdmCuentaContable_SalarioNavigation)
                    .HasForeignKey(d => d.idAdmCuentaContable_Salario)
                    .HasConstraintName("FK_tblCuentaContableNCentroTrabajo_tblAdmCuentaContable_Salario");

                entity.HasOne(d => d.idAdmCuentaContable_Sueldo_MXNavigation)
                    .WithMany(p => p.tblCuentaContableNCentroTrabajoidAdmCuentaContable_Sueldo_MXNavigation)
                    .HasForeignKey(d => d.idAdmCuentaContable_Sueldo_MX)
                    .HasConstraintName("FK_tblCuentaContableNCentroTrabajo_tblAdmCuentaContable_Sueldo_MX");

                entity.HasOne(d => d.idCentroTrabajoNavigation)
                    .WithOne(p => p.tblCuentaContableNCentroTrabajo)
                    .HasForeignKey<tblCuentaContableNCentroTrabajo>(d => d.idCentroTrabajo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCuentaContableNCentroTrabajo_tblCentroTrabajo");
            });

            modelBuilder.Entity<tblCuentaContableNTipoTrabajo>(entity =>
            {
                entity.HasOne(d => d.idAdmCuentaContable_IMSS_MXNavigation)
                    .WithMany(p => p.tblCuentaContableNTipoTrabajoidAdmCuentaContable_IMSS_MXNavigation)
                    .HasForeignKey(d => d.idAdmCuentaContable_IMSS_MX)
                    .HasConstraintName("FK_tblCuentaContableNTipoTrabajo_tblAdmCuentaContable_IMSS_MX");

                entity.HasOne(d => d.idAdmCuentaContable_INFONAVIT_MXNavigation)
                    .WithMany(p => p.tblCuentaContableNTipoTrabajoidAdmCuentaContable_INFONAVIT_MXNavigation)
                    .HasForeignKey(d => d.idAdmCuentaContable_INFONAVIT_MX)
                    .HasConstraintName("FK_tblCuentaContableNTipoTrabajo_tblAdmCuentaContable_INFONAVIT_MX");

                entity.HasOne(d => d.idAdmCuentaContable_ImpEstatalNominas_MXNavigation)
                    .WithMany(p => p.tblCuentaContableNTipoTrabajoidAdmCuentaContable_ImpEstatalNominas_MXNavigation)
                    .HasForeignKey(d => d.idAdmCuentaContable_ImpEstatalNominas_MX)
                    .HasConstraintName("FK_tblCuentaContableNTipoTrabajo_tblAdmCuentaContable_ImpEstatalNominas_MX");

                entity.HasOne(d => d.idAdmCuentaContable_SAR_MXNavigation)
                    .WithMany(p => p.tblCuentaContableNTipoTrabajoidAdmCuentaContable_SAR_MXNavigation)
                    .HasForeignKey(d => d.idAdmCuentaContable_SAR_MX)
                    .HasConstraintName("FK_tblCuentaContableNTipoTrabajo_tblAdmCuentaContable_SAR_MX");

                entity.HasOne(d => d.idAdmCuentaContable_SSEmpresaNavigation)
                    .WithMany(p => p.tblCuentaContableNTipoTrabajoidAdmCuentaContable_SSEmpresaNavigation)
                    .HasForeignKey(d => d.idAdmCuentaContable_SSEmpresa)
                    .HasConstraintName("FK_tblCuentaContableNTipoTrabajo_tblAdmCuentaContable_SSEmpresa");

                entity.HasOne(d => d.idAdmCuentaContable_SalarioNavigation)
                    .WithMany(p => p.tblCuentaContableNTipoTrabajoidAdmCuentaContable_SalarioNavigation)
                    .HasForeignKey(d => d.idAdmCuentaContable_Salario)
                    .HasConstraintName("FK_tblCuentaContableNTipoTrabajo_tblAdmCuentaContable_Salario");

                entity.HasOne(d => d.idAdmCuentaContable_Sueldo_MXNavigation)
                    .WithMany(p => p.tblCuentaContableNTipoTrabajoidAdmCuentaContable_Sueldo_MXNavigation)
                    .HasForeignKey(d => d.idAdmCuentaContable_Sueldo_MX)
                    .HasConstraintName("FK_tblCuentaContableNTipoTrabajo_tblAdmCuentaContable_Sueldo_MX");

                entity.HasOne(d => d.idTipoTrabajoNavigation)
                    .WithOne(p => p.tblCuentaContableNTipoTrabajo)
                    .HasForeignKey<tblCuentaContableNTipoTrabajo>(d => d.idTipoTrabajo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCuentaContableNTipoTrabajo_tblTipoTrabajo");
            });

            modelBuilder.Entity<tblDatosFinancieros>(entity =>
            {
                entity.HasKey(e => new { e.idConceptoFinanciero, e.año, e.idGrupoEmpresarial });

                entity.HasOne(d => d.idConceptoFinancieroNavigation)
                    .WithMany(p => p.tblDatosFinancieros)
                    .HasForeignKey(d => d.idConceptoFinanciero)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblDatosFinancieros_tblConceptosFinancieros");

                entity.HasOne(d => d.idGrupoEmpresarialNavigation)
                    .WithMany(p => p.tblDatosFinancieros)
                    .HasForeignKey(d => d.idGrupoEmpresarial)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblDatosFinancieros_tblGrupoEmpresarial");
            });

            modelBuilder.Entity<tblDatosFinancierosNTrimestre>(entity =>
            {
                entity.HasKey(e => new { e.idConceptoFinanciero, e.año, e.idGrupoEmpresarial, e.trimestre });

                entity.HasOne(d => d.tblDatosFinancieros)
                    .WithMany(p => p.tblDatosFinancierosNTrimestre)
                    .HasForeignKey(d => new { d.idConceptoFinanciero, d.año, d.idGrupoEmpresarial })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblDatosFinancierosNTrimestre_tblDatosFinancieros");
            });

            modelBuilder.Entity<tblDatosSalariales>(entity =>
            {
                entity.Property(e => e.idPersona).ValueGeneratedNever();

                entity.Property(e => e.isTrienio).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.fechaAntiguedad_idUsuario_modNavigation)
                    .WithMany(p => p.tblDatosSalariales)
                    .HasForeignKey(d => d.fechaAntiguedad_idUsuario_mod)
                    .HasConstraintName("FK_tblDatosSalariales_tblUsuario");

                entity.HasOne(d => d.idPersonaNavigation)
                    .WithOne(p => p.tblDatosSalariales)
                    .HasForeignKey<tblDatosSalariales>(d => d.idPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblDatosSalariales_tblPersona");
            });

            modelBuilder.Entity<tblDatosSalariales_historico_salarioBase>(entity =>
            {
                entity.HasKey(e => new { e.idPersona, e.fecha });

                entity.HasOne(d => d.idPersonaNavigation)
                    .WithMany(p => p.tblDatosSalariales_historico_salarioBase)
                    .HasForeignKey(d => d.idPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblDatosSalariales_historico_salarioBase_tblDatosSalariales");
            });

            modelBuilder.Entity<tblDefectoPrendaHuesped>(entity =>
            {
                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblDefectoPrendaHuesped)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblDefectoPrendaHuesped_tblLavanderia");
            });

            modelBuilder.Entity<tblDenoPrenda>(entity =>
            {
                entity.HasOne(d => d.idTipoPrendaNavigation)
                    .WithMany(p => p.tblDenoPrenda)
                    .HasForeignKey(d => d.idTipoPrenda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblDenoPrenda_tblTipoPrenda");

                entity.HasOne(d => d.idTraduccionNavigation)
                    .WithMany(p => p.tblDenoPrenda)
                    .HasForeignKey(d => d.idTraduccion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblDenoPrenda_tblTraduccion");
            });

            modelBuilder.Entity<tblDetalleNNomina>(entity =>
            {
                entity.HasOne(d => d.idNominaNavigation)
                    .WithMany(p => p.tblDetalleNNomina)
                    .HasForeignKey(d => d.idNomina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblDetalleNNomina_tblNomina");
            });

            modelBuilder.Entity<tblDiasCuadrante>(entity =>
            {
                entity.HasOne(d => d.idPersonaNavigation)
                    .WithMany(p => p.tblDiasCuadrante)
                    .HasForeignKey(d => d.idPersona)
                    .HasConstraintName("FK_tblDiasCuadrante_tblPersona");

                entity.HasOne(d => d.idTipoDiaCuadranteNavigation)
                    .WithMany(p => p.tblDiasCuadrante)
                    .HasForeignKey(d => d.idTipoDiaCuadrante)
                    .HasConstraintName("FK_tblDiasCuadrante_tblTipoDiaCuadrante");

                entity.HasOne(d => d.idTurnoNavigation)
                    .WithMany(p => p.tblDiasCuadrante)
                    .HasForeignKey(d => d.idTurno)
                    .HasConstraintName("FK_tblDiasCuadrante_tblTurno");
            });

            modelBuilder.Entity<tblDiasLibresPersonal>(entity =>
            {
                entity.HasKey(e => e.idDiasLibresPersonal)
                    .HasName("PK__tblDiasL__730D342E810FD7D7");

                entity.HasOne(d => d.idPersonaNavigation)
                    .WithMany(p => p.tblDiasLibresPersonal)
                    .HasForeignKey(d => d.idPersona)
                    .HasConstraintName("FK__tblDiasLi__idPer__26A66E5A");
            });

            modelBuilder.Entity<tblDiasLibresPersonal_Llamamiento>(entity =>
            {
                entity.HasOne(d => d.idLlamamientoNavigation)
                    .WithMany(p => p.tblDiasLibresPersonal_Llamamiento)
                    .HasForeignKey(d => d.idLlamamiento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblDiasLibresPersonal_Llamamiento_tblLlamamiento");
            });

            modelBuilder.Entity<tblDiscapacidad>(entity =>
            {
                entity.HasOne(d => d.idTraduccionNavigation)
                    .WithMany(p => p.tblDiscapacidad)
                    .HasForeignKey(d => d.idTraduccion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblDiscapacidad_tblTraduccion");
            });

            modelBuilder.Entity<tblDocumento>(entity =>
            {
                entity.Property(e => e.idTipoDocumento).HasDefaultValueSql("((1))");

                entity.Property(e => e.isVisible).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.idPersonaNavigation)
                    .WithMany(p => p.tblDocumento)
                    .HasForeignKey(d => d.idPersona)
                    .HasConstraintName("FK_tblDocumento_tblCarpetaDocumentos");

                entity.HasOne(d => d.idTipoDocumentoNavigation)
                    .WithMany(p => p.tblDocumento)
                    .HasForeignKey(d => d.idTipoDocumento)
                    .HasConstraintName("FK_tblDocumento_tblTipoDocumento");

                entity.HasOne(d => d.idUsuarioNavigation)
                    .WithMany(p => p.tblDocumentoidUsuarioNavigation)
                    .HasForeignKey(d => d.idUsuario)
                    .HasConstraintName("FK_tblDocumento_tblUsuario");

                entity.HasOne(d => d.idUsuarioModificacionNavigation)
                    .WithMany(p => p.tblDocumentoidUsuarioModificacionNavigation)
                    .HasForeignKey(d => d.idUsuarioModificacion)
                    .HasConstraintName("FK_tblDocumento_tblUsuario1");
            });

            modelBuilder.Entity<tblDocumentoNNomina>(entity =>
            {
                entity.HasOne(d => d.idNominaNavigation)
                    .WithMany(p => p.tblDocumentoNNomina)
                    .HasForeignKey(d => d.idNomina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblDocumentoNNomina_tblNomina");

                entity.HasOne(d => d.idUsuarioNavigation)
                    .WithMany(p => p.tblDocumentoNNomina)
                    .HasForeignKey(d => d.idUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblDocumentoNNomina_tblUsuario");
            });

            modelBuilder.Entity<tblDocumentoNSolicitudAlta>(entity =>
            {
                entity.HasKey(e => new { e.idSolicitudAlta, e.idDocumento })
                    .HasName("PK_tblDocumentoNSolicitudAlta_1");

                entity.HasOne(d => d.idDocumentoNavigation)
                    .WithMany(p => p.tblDocumentoNSolicitudAlta)
                    .HasForeignKey(d => d.idDocumento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblDocumentoNSolicitudAlta_tblDocumento");

                entity.HasOne(d => d.idSolicitudAltaNavigation)
                    .WithMany(p => p.tblDocumentoNSolicitudAlta)
                    .HasForeignKey(d => d.idSolicitudAlta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblDocumentoNSolicitudAlta_tblSolicitudAlta");
            });

            modelBuilder.Entity<tblDocumentoPrenda>(entity =>
            {
                entity.HasKey(e => e.idDocumento)
                    .HasName("PK__tblDocum__572A36FC844843A3");

                entity.HasOne(d => d.idPrendaNavigation)
                    .WithMany(p => p.tblDocumentoPrenda)
                    .HasForeignKey(d => d.idPrenda)
                    .HasConstraintName("FK_tblDocumentoPrenda_tblPrenda");
            });

            modelBuilder.Entity<tblElemLogNPedido>(entity =>
            {
                entity.HasKey(e => new { e.idTipoElemLog, e.idPedido });

                entity.HasOne(d => d.idPedidoNavigation)
                    .WithMany(p => p.tblElemLogNPedido)
                    .HasForeignKey(d => d.idPedido)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblElemLogNPedido_tblPedido");

                entity.HasOne(d => d.idTipoElemLogNavigation)
                    .WithMany(p => p.tblElemLogNPedido)
                    .HasForeignKey(d => d.idTipoElemLog)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblElemLogNPedido_tblTipoElemLog");
            });

            modelBuilder.Entity<tblElemTrans>(entity =>
            {
                entity.Property(e => e.idElemTrans).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<tblEmpresasPolarier>(entity =>
            {
                entity.Property(e => e.idEmpresaPolarier).ValueGeneratedNever();

                entity.Property(e => e.idMoneda).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.idMonedaNavigation)
                    .WithMany(p => p.tblEmpresasPolarier)
                    .HasForeignKey(d => d.idMoneda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEmpresasPolarier_tblMoneda");

                entity.HasOne(d => d.idPaisNavigation)
                    .WithMany(p => p.tblEmpresasPolarier)
                    .HasForeignKey(d => d.idPais)
                    .HasConstraintName("FK__tblEmpres__idPai__44E0DCB7");
            });

            modelBuilder.Entity<tblEncuesta>(entity =>
            {
                entity.HasOne(d => d.idCampañaEncuestaNavigation)
                    .WithMany(p => p.tblEncuesta)
                    .HasForeignKey(d => d.idCampañaEncuesta)
                    .HasConstraintName("FK_tblEncuesta_tblCampañaEncuesta");

                entity.HasOne(d => d.idEncuestaPlantillaNavigation)
                    .WithMany(p => p.tblEncuesta)
                    .HasForeignKey(d => d.idEncuestaPlantilla)
                    .HasConstraintName("FK_tblEncuesta_tblEncuestaPlantilla");

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblEncuesta)
                    .HasForeignKey(d => d.idEntidad)
                    .HasConstraintName("FK_tblEncuesta_tblEntidad");

                entity.HasOne(d => d.idTipoEncuestaNavigation)
                    .WithMany(p => p.tblEncuesta)
                    .HasForeignKey(d => d.idTipoEncuesta)
                    .HasConstraintName("FK_tblEncuesta_tblTipoEncuesta");

                entity.HasOne(d => d.idUsuarioNavigation)
                    .WithMany(p => p.tblEncuesta)
                    .HasForeignKey(d => d.idUsuario)
                    .HasConstraintName("FK_tblEncuesta_tblUsuario");
            });

            modelBuilder.Entity<tblEncuestaPlantilla>(entity =>
            {
                entity.Property(e => e.idEncuestaPlantilla).ValueGeneratedNever();

                entity.HasOne(d => d.idTipoEncuestaNavigation)
                    .WithMany(p => p.tblEncuestaPlantilla)
                    .HasForeignKey(d => d.idTipoEncuesta)
                    .HasConstraintName("FK_tblEncuestaPlantilla_tblTipoEncuesta");
            });

            modelBuilder.Entity<tblEnergyHub>(entity =>
            {
                entity.Property(e => e.idEnergyHub).ValueGeneratedNever();

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblEnergyHub)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEnergyHub_tblLavanderia");
            });

            modelBuilder.Entity<tblEntidad>(entity =>
            {
                entity.Property(e => e.costeEstancia).HasDefaultValueSql("((0))");

                entity.Property(e => e.enableObservacionesPedido).HasDefaultValueSql("((0))");

                entity.Property(e => e.idModeloImpresion_reparto).HasDefaultValueSql("((1))");

                entity.Property(e => e.inventarioPorPorcentaje).HasDefaultValueSql("((0))");

                entity.Property(e => e.isPrincipal).HasDefaultValueSql("((1))");

                entity.Property(e => e.isTodasPrendaNNuevoPedido_compañia).HasDefaultValueSql("((1))");

                entity.Property(e => e.isTodasPrendaNNuevoPedido_entidad).HasDefaultValueSql("((1))");

                entity.Property(e => e.objKgEstancia).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.idCompañiaNavigation)
                    .WithMany(p => p.tblEntidad)
                    .HasForeignKey(d => d.idCompañia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEntidad_tblCompañia");

                entity.HasOne(d => d.idGrupoEntidadNavigation)
                    .WithMany(p => p.tblEntidad)
                    .HasForeignKey(d => d.idGrupoEntidad)
                    .HasConstraintName("FK_tblEntidad_tblGrupoEntidad");

                entity.HasOne(d => d.idLocalizacionNavigation)
                    .WithMany(p => p.tblEntidad)
                    .HasForeignKey(d => d.idLocalizacion)
                    .HasConstraintName("FK_tblEntidad_tblLocalizacion");

                entity.HasOne(d => d.idModeloImpresion_repartoNavigation)
                    .WithMany(p => p.tblEntidad)
                    .HasForeignKey(d => d.idModeloImpresion_reparto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEntidad_tblModeloImpresion_reparto");

                entity.HasOne(d => d.idMonedaNavigation)
                    .WithMany(p => p.tblEntidad)
                    .HasForeignKey(d => d.idMoneda)
                    .HasConstraintName("FK_tblEntidad_tblMoneda");

                entity.HasOne(d => d.idTipoAlmacenajeLimpioNavigation)
                    .WithMany(p => p.tblEntidad)
                    .HasForeignKey(d => d.idTipoAlmacenajeLimpio)
                    .HasConstraintName("FK_tblEntidad_tblTipoAlmacenajeLimpio");

                entity.HasOne(d => d.idTipoConsumoLenceriaNavigation)
                    .WithMany(p => p.tblEntidad)
                    .HasForeignKey(d => d.idTipoConsumoLenceria)
                    .HasConstraintName("FK_General.tblEntidad_Logistica.tblTipoAbono");

                entity.HasOne(d => d.idTipoFacturacionClienteNavigation)
                    .WithMany(p => p.tblEntidad)
                    .HasForeignKey(d => d.idTipoFacturacionCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEntidad_tblTipoFacturacionCliente");

                entity.HasOne(d => d.idTipoRepartoEntidadNavigation)
                    .WithMany(p => p.tblEntidad)
                    .HasForeignKey(d => d.idTipoRepartoEntidad)
                    .HasConstraintName("FK_tblEntidad_tblTipoRepartoEntidad");

                entity.HasMany(d => d.idEntidadNavigation)
                    .WithMany(p => p.idEntidadSecundaria)
                    .UsingEntity<Dictionary<string, object>>(
                        "tblEntidadNEntidadSecundaria",
                        l => l.HasOne<tblEntidad>().WithMany().HasForeignKey("idEntidad").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblEntidadNEntidadSecundaria_tblEntidad"),
                        r => r.HasOne<tblEntidad>().WithMany().HasForeignKey("idEntidadSecundaria").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblEntidadNEntidadSecundaria_tblEntidad1"),
                        j =>
                        {
                            j.HasKey("idEntidad", "idEntidadSecundaria");

                            j.ToTable("tblEntidadNEntidadSecundaria", "General");
                        });

                entity.HasMany(d => d.idEntidadSecundaria)
                    .WithMany(p => p.idEntidadNavigation)
                    .UsingEntity<Dictionary<string, object>>(
                        "tblEntidadNEntidadSecundaria",
                        l => l.HasOne<tblEntidad>().WithMany().HasForeignKey("idEntidadSecundaria").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblEntidadNEntidadSecundaria_tblEntidad1"),
                        r => r.HasOne<tblEntidad>().WithMany().HasForeignKey("idEntidad").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblEntidadNEntidadSecundaria_tblEntidad"),
                        j =>
                        {
                            j.HasKey("idEntidad", "idEntidadSecundaria");

                            j.ToTable("tblEntidadNEntidadSecundaria", "General");
                        });

                entity.HasMany(d => d.idUsuario)
                    .WithMany(p => p.idEntidad)
                    .UsingEntity<Dictionary<string, object>>(
                        "tblEntidadNUsuario",
                        l => l.HasOne<tblUsuario>().WithMany().HasForeignKey("idUsuario").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblEntidadNUsuario_tblUsuario"),
                        r => r.HasOne<tblEntidad>().WithMany().HasForeignKey("idEntidad").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblEntidadNUsuario_tblEntidad"),
                        j =>
                        {
                            j.HasKey("idEntidad", "idUsuario").HasName("PK__tblEntid__57EB133C2AA3F60B");

                            j.ToTable("tblEntidadNUsuario", "GestionInterna");
                        });
            });

            modelBuilder.Entity<tblEntidadNInventario>(entity =>
            {
                entity.HasKey(e => new { e.idInventario, e.idEntidad });

                entity.HasOne(d => d.idArchivo_firmaClienteNavigation)
                    .WithMany(p => p.tblEntidadNInventarioidArchivo_firmaClienteNavigation)
                    .HasForeignKey(d => d.idArchivo_firmaCliente)
                    .HasConstraintName("FK_tblEntidadNInventario_tblArchivo_Cliente");

                entity.HasOne(d => d.idArchivo_firmaPolarierNavigation)
                    .WithMany(p => p.tblEntidadNInventarioidArchivo_firmaPolarierNavigation)
                    .HasForeignKey(d => d.idArchivo_firmaPolarier)
                    .HasConstraintName("FK_tblEntidadNInventario_tblArchivo_Polarier");

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblEntidadNInventario)
                    .HasForeignKey(d => d.idEntidad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEntidadNInventario_tblEntidad");

                entity.HasOne(d => d.idInventarioNavigation)
                    .WithMany(p => p.tblEntidadNInventario)
                    .HasForeignKey(d => d.idInventario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEntidadNInventario_tblInventario");
            });

            modelBuilder.Entity<tblEntidadNRutaExpedicion>(entity =>
            {
                entity.HasKey(e => new { e.idEntidad, e.idRutaExpedicion })
                    .HasName("PK__tblEntid__60899342D8795809");

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblEntidadNRutaExpedicion)
                    .HasForeignKey(d => d.idEntidad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEntidadNRutaExpedicion_tblEntidad");

                entity.HasOne(d => d.idRutaExpedicionNavigation)
                    .WithMany(p => p.tblEntidadNRutaExpedicion)
                    .HasForeignKey(d => d.idRutaExpedicion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEntidadNRutaExpedicion_tblRutaExpedicion");
            });

            modelBuilder.Entity<tblEntidad_historico_idTipoFacturacion>(entity =>
            {
                entity.HasKey(e => new { e.idEntidad, e.fecha });

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblEntidad_historico_idTipoFacturacion)
                    .HasForeignKey(d => d.idEntidad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEntidad_historico_idTipoFacturacion_tblEntidad");
            });

            modelBuilder.Entity<tblEnvio>(entity =>
            {
                entity.HasOne(d => d.idDestinatarioNavigation)
                    .WithMany(p => p.tblEnvio)
                    .HasForeignKey(d => d.idDestinatario)
                    .HasConstraintName("FK_tblEnvio_tblDestinatario");

                entity.HasOne(d => d.idEmbarcadorNavigation)
                    .WithMany(p => p.tblEnvio)
                    .HasForeignKey(d => d.idEmbarcador)
                    .HasConstraintName("FK_tblEnvio_tblEmbarcador");

                entity.HasOne(d => d.idIncotermClienteNavigation)
                    .WithMany(p => p.tblEnvioidIncotermClienteNavigation)
                    .HasForeignKey(d => d.idIncotermCliente)
                    .HasConstraintName("FK_tblEnvio_tblIncotermCliente");

                entity.HasOne(d => d.idIncotermProvNavigation)
                    .WithMany(p => p.tblEnvioidIncotermProvNavigation)
                    .HasForeignKey(d => d.idIncotermProv)
                    .HasConstraintName("FK_tblEnvio_tblIncotermProv");

                entity.HasOne(d => d.idProyectoNavigation)
                    .WithMany(p => p.tblEnvio)
                    .HasForeignKey(d => d.idProyecto)
                    .HasConstraintName("FK_tblEnvio_tblProyecto");

                entity.HasOne(d => d.idPuertoCargaNavigation)
                    .WithMany(p => p.tblEnvioidPuertoCargaNavigation)
                    .HasForeignKey(d => d.idPuertoCarga)
                    .HasConstraintName("FK_tblEnvio_tblPuertoCarga");

                entity.HasOne(d => d.idPuertoDestinoNavigation)
                    .WithMany(p => p.tblEnvioidPuertoDestinoNavigation)
                    .HasForeignKey(d => d.idPuertoDestino)
                    .HasConstraintName("FK_tblEnvio_tblPuertoDestino");

                entity.HasOne(d => d.idTipoContenedorNavigation)
                    .WithMany(p => p.tblEnvio)
                    .HasForeignKey(d => d.idTipoContenedor)
                    .HasConstraintName("FK_tblEnvio_tblTipoContenedor");

                entity.HasMany(d => d.idTipoDocumento_Envio)
                    .WithMany(p => p.idEnvio)
                    .UsingEntity<Dictionary<string, object>>(
                        "tblEnvioNTipoDocumento_Completo",
                        l => l.HasOne<tblTipoDocumento_Envio>().WithMany().HasForeignKey("idTipoDocumento_Envio").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblEnvioNTipoDocumento_Completo_tblTipoDocumento_Envio"),
                        r => r.HasOne<tblEnvio>().WithMany().HasForeignKey("idEnvio").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblEnvioNTipoDocumento_Completo_tblEnvio"),
                        j =>
                        {
                            j.HasKey("idEnvio", "idTipoDocumento_Envio");

                            j.ToTable("tblEnvioNTipoDocumento_Completo", "Logistica");
                        });
            });

            modelBuilder.Entity<tblEnvio_Documento>(entity =>
            {
                entity.HasKey(e => new { e.idDocumento, e.idEnvio });

                entity.Property(e => e.idDocumento).ValueGeneratedOnAdd();

                entity.Property(e => e.idTipoDocumento_Envio).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.idEnvioNavigation)
                    .WithMany(p => p.tblEnvio_Documento)
                    .HasForeignKey(d => d.idEnvio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEnvio_Documento_tblEnvio");

                entity.HasOne(d => d.idTipoDocumento_EnvioNavigation)
                    .WithMany(p => p.tblEnvio_Documento)
                    .HasForeignKey(d => d.idTipoDocumento_Envio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEnvio_Documento_tblTipoDocumento_Envio");
            });

            modelBuilder.Entity<tblEstadoCivil>(entity =>
            {
                entity.HasOne(d => d.idTraduccionNavigation)
                    .WithMany(p => p.tblEstadoCivil)
                    .HasForeignKey(d => d.idTraduccion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEstadoCivil_tblTraduccion");
            });

            modelBuilder.Entity<tblEstadoEnergyHub>(entity =>
            {
                entity.HasOne(d => d.idEnergyHubNavigation)
                    .WithMany(p => p.tblEstadoEnergyHub)
                    .HasForeignKey(d => d.idEnergyHub)
                    .HasConstraintName("FK_tblEstadoEnergyHub_tblEnergyHub");
            });

            modelBuilder.Entity<tblEstadoMovimientoElemLog>(entity =>
            {
                entity.HasOne(d => d.idTraduccionNavigation)
                    .WithMany(p => p.tblEstadoMovimientoElemLog)
                    .HasForeignKey(d => d.idTraduccion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEstadoMovimientoElemLog_tblTraduccion");
            });

            modelBuilder.Entity<tblEstadoMovimientoRecambioNMovimientoRecambio>(entity =>
            {
                entity.HasKey(e => new { e.idEstadoMovimientoRecambio, e.idMovimientoRecambio, e.fecha });

                entity.HasOne(d => d.idEstadoMovimientoRecambioNavigation)
                    .WithMany(p => p.tblEstadoMovimientoRecambioNMovimientoRecambio)
                    .HasForeignKey(d => d.idEstadoMovimientoRecambio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEstadoMovimientoRecambioNMovimientoRecambio_tblEstadoMovimientoRecambio");

                entity.HasOne(d => d.idMovimientoRecambioNavigation)
                    .WithMany(p => p.tblEstadoMovimientoRecambioNMovimientoRecambio)
                    .HasForeignKey(d => d.idMovimientoRecambio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEstadoMovimientoRecambioNMovimientoRecambio_tblMovimientoRecambio");

                entity.HasOne(d => d.idUsuarioNavigation)
                    .WithMany(p => p.tblEstadoMovimientoRecambioNMovimientoRecambio)
                    .HasForeignKey(d => d.idUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEstadoMovimientoRecambioNMovimientoRecambio_tblUsuario");
            });

            modelBuilder.Entity<tblEstadoNomina>(entity =>
            {
                entity.HasKey(e => e.idEstadoNomina)
                    .HasName("PK__tblEstad__486F14D4452285AD");

                entity.HasOne(d => d.idTraduccionNavigation)
                    .WithMany(p => p.tblEstadoNomina)
                    .HasForeignKey(d => d.idTraduccion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEstadoNomina_tblTraduccion");
            });

            modelBuilder.Entity<tblEstadoNominaNNomina>(entity =>
            {
                entity.HasKey(e => new { e.idNomina, e.idEstadoNomina, e.fecha })
                    .HasName("PK__tblEstad__CD0A532DA25E9D4F");

                entity.HasOne(d => d.idEstadoNominaNavigation)
                    .WithMany(p => p.tblEstadoNominaNNomina)
                    .HasForeignKey(d => d.idEstadoNomina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEstadoNominaNNomina_tblEstadoNomina");

                entity.HasOne(d => d.idNominaNavigation)
                    .WithMany(p => p.tblEstadoNominaNNomina)
                    .HasForeignKey(d => d.idNomina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEstadoNominaNNomina_tblNomina");

                entity.HasOne(d => d.idUsuario_validaNavigation)
                    .WithMany(p => p.tblEstadoNominaNNomina)
                    .HasForeignKey(d => d.idUsuario_valida)
                    .HasConstraintName("FK_tblEstadoNominaNNomina_tblUsuario");
            });

            modelBuilder.Entity<tblEstadoPedido>(entity =>
            {
                entity.Property(e => e.idEstadoPedido).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<tblEstadoSmartHubNMaquina>(entity =>
            {
                entity.HasKey(e => new { e.idMaquina, e.fechaInicio });

                entity.HasOne(d => d.idEstadoSmartHubNavigation)
                    .WithMany(p => p.tblEstadoSmartHubNMaquina)
                    .HasForeignKey(d => d.idEstadoSmartHub)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEstadoSmartHubNMaquina_tblEstadoSmartHub");

                entity.HasOne(d => d.idMaquinaNavigation)
                    .WithMany(p => p.tblEstadoSmartHubNMaquina)
                    .HasForeignKey(d => d.idMaquina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEstadoSmartHubNMaquina_tblMaquina");
            });

            modelBuilder.Entity<tblEstadoSolicitudAlta>(entity =>
            {
                entity.Property(e => e.idEstadoSolicitudAlta).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<tblEstadoSolicitudAltaNSolicitudAlta>(entity =>
            {
                entity.HasKey(e => new { e.idSolicitudAlta, e.idEstadoSolicitudAlta, e.fecha })
                    .HasName("PK__tblEstad__3EB929193139783B");

                entity.HasOne(d => d.idEstadoSolicitudAltaNavigation)
                    .WithMany(p => p.tblEstadoSolicitudAltaNSolicitudAlta)
                    .HasForeignKey(d => d.idEstadoSolicitudAlta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEstadoSolicitudAltaNSolicitudAlta_tblEstadoSolicitudAlta");

                entity.HasOne(d => d.idSolicitudAltaNavigation)
                    .WithMany(p => p.tblEstadoSolicitudAltaNSolicitudAlta)
                    .HasForeignKey(d => d.idSolicitudAlta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEstadoSolicitudAltaNSolicitudAlta_tblSolicitudAlta");

                entity.HasOne(d => d.idUsuarioNavigation)
                    .WithMany(p => p.tblEstadoSolicitudAltaNSolicitudAlta)
                    .HasForeignKey(d => d.idUsuario)
                    .HasConstraintName("FK_tblEstadoSolicitudAltaNSolicitudAlta_tblUsuario");
            });

            modelBuilder.Entity<tblEstadoTag>(entity =>
            {
                entity.Property(e => e.idEstado).ValueGeneratedNever();
            });

            modelBuilder.Entity<tblEstancia>(entity =>
            {
                entity.HasKey(e => new { e.idEntidad, e.fecha });

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblEstancia)
                    .HasForeignKey(d => d.idEntidad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEstancia_tblEntidad");
            });

            modelBuilder.Entity<tblEventoPersona>(entity =>
            {
                entity.HasKey(e => new { e.idPersona, e.fecha });

                entity.HasOne(d => d.idJornadaNavigation)
                    .WithMany(p => p.tblEventoPersona)
                    .HasForeignKey(d => d.idJornada)
                    .HasConstraintName("FK_tblEventoPersona_tblJornada");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblEventoPersona)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEventoPersona_tblLavanderia");

                entity.HasOne(d => d.idPersonaNavigation)
                    .WithMany(p => p.tblEventoPersona)
                    .HasForeignKey(d => d.idPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEventoPersona_tblPersona");
            });

            modelBuilder.Entity<tblEventoPersona_Estado>(entity =>
            {
                entity.HasOne(d => d.idTraduccionNavigation)
                    .WithMany(p => p.tblEventoPersona_Estado)
                    .HasForeignKey(d => d.idTraduccion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEventoPersona_Estado_tblTraduccion");
            });

            modelBuilder.Entity<tblFabricante>(entity =>
            {
                entity.Property(e => e.idFabricante).ValueGeneratedNever();
            });

            modelBuilder.Entity<tblFamilia>(entity =>
            {
                entity.HasKey(e => e.idFamilia)
                    .HasName("PK__tblFamil__CC8AA314C89DB4A6");

                entity.HasOne(d => d.idTraduccionNavigation)
                    .WithMany(p => p.tblFamiliaidTraduccionNavigation)
                    .HasForeignKey(d => d.idTraduccion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblFamilia_tblTraduccion");

                entity.HasOne(d => d.idTraduccion_abrNavigation)
                    .WithMany(p => p.tblFamiliaidTraduccion_abrNavigation)
                    .HasForeignKey(d => d.idTraduccion_abr)
                    .HasConstraintName("FK_tblFamilia_tblTraduccion1");
            });

            modelBuilder.Entity<tblFormatoDiasLibres>(entity =>
            {
                entity.HasKey(e => e.idFormatoDiasLibres)
                    .HasName("PK__tblForma__2ED70DCD95E580C9");

                entity.Property(e => e.idFormatoDiasLibres).ValueGeneratedOnAdd();

                entity.HasOne(d => d.idTraduccionNavigation)
                    .WithMany(p => p.tblFormatoDiasLibres)
                    .HasForeignKey(d => d.idTraduccion)
                    .HasConstraintName("FK__tblFormat__idTra__1B34BBAE");
            });

            modelBuilder.Entity<tblFormulario>(entity =>
            {
                entity.HasOne(d => d.idApartadoNavigation)
                    .WithMany(p => p.tblFormulario)
                    .HasForeignKey(d => d.idApartado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblFormulario_tblApartado");

                entity.HasOne(d => d.idAplicacionNavigation)
                    .WithMany(p => p.tblFormulario)
                    .HasForeignKey(d => d.idAplicacion)
                    .HasConstraintName("FK_tblFormulario_tblAplicacion");

                entity.HasOne(d => d.idTraduccionNavigation)
                    .WithMany(p => p.tblFormulario)
                    .HasForeignKey(d => d.idTraduccion)
                    .HasConstraintName("FK_tblFormulario_tblTraduccion");

                entity.HasMany(d => d.idCargo)
                    .WithMany(p => p.idFormulario)
                    .UsingEntity<Dictionary<string, object>>(
                        "tblFormularioNCargo",
                        l => l.HasOne<tblCargo>().WithMany().HasForeignKey("idCargo").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblFormularioNCargo_tblCargo"),
                        r => r.HasOne<tblFormulario>().WithMany().HasForeignKey("idFormulario").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblFormularioNCargo_tblFormulario"),
                        j =>
                        {
                            j.HasKey("idFormulario", "idCargo").HasName("PK__tblFormu__E051A8ED479A3ADD");

                            j.ToTable("tblFormularioNCargo", "GestionInterna");
                        });

                entity.HasMany(d => d.idLavanderia)
                    .WithMany(p => p.idFormulario)
                    .UsingEntity<Dictionary<string, object>>(
                        "tblFormularioNLavanderia",
                        l => l.HasOne<tblLavanderia>().WithMany().HasForeignKey("idLavanderia").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblFormularioNLavanderia_tblLavanderia"),
                        r => r.HasOne<tblFormulario>().WithMany().HasForeignKey("idFormulario").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblFormularioNLavanderia_tblFormulario"),
                        j =>
                        {
                            j.HasKey("idFormulario", "idLavanderia");

                            j.ToTable("tblFormularioNLavanderia", "GestionInterna");
                        });

                entity.HasMany(d => d.idModulo)
                    .WithMany(p => p.idFormulario)
                    .UsingEntity<Dictionary<string, object>>(
                        "tblFormularioNModulo",
                        l => l.HasOne<tblModulo>().WithMany().HasForeignKey("idModulo").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblFormularioNModulo_tblModulo"),
                        r => r.HasOne<tblFormulario>().WithMany().HasForeignKey("idFormulario").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblFormularioNModulo_tblFormulario"),
                        j =>
                        {
                            j.HasKey("idFormulario", "idModulo").HasName("PK__tblFormu__81DBC914727247A4");

                            j.ToTable("tblFormularioNModulo", "GestionInterna");
                        });
            });

            modelBuilder.Entity<tblFormularioNUsuario>(entity =>
            {
                entity.HasKey(e => new { e.idFormulario, e.idUsuario })
                    .HasName("PK__tblFormu__4450DA116A1BF3CC");

                entity.Property(e => e.isBorrado).HasDefaultValueSql("((1))");

                entity.Property(e => e.isEscritura).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.idFormularioNavigation)
                    .WithMany(p => p.tblFormularioNUsuario)
                    .HasForeignKey(d => d.idFormulario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblFormularioNUsuario_tblFormulario");

                entity.HasOne(d => d.idUsuarioNavigation)
                    .WithMany(p => p.tblFormularioNUsuario)
                    .HasForeignKey(d => d.idUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblFormularioNUsuario_tblUsuario");
            });

            modelBuilder.Entity<tblFotoNArticuloEnvio>(entity =>
            {
                entity.HasKey(e => new { e.idFoto, e.idArticuloEnvio });

                entity.Property(e => e.idFoto).ValueGeneratedOnAdd();

                entity.HasOne(d => d.idArticuloEnvioNavigation)
                    .WithMany(p => p.tblFotoNArticuloEnvio)
                    .HasForeignKey(d => d.idArticuloEnvio)
                    .HasConstraintName("FK_tblFotoNArticuloEnvio_tblArticuloEnvio");
            });

            modelBuilder.Entity<tblFrecuenciaMantenimiento>(entity =>
            {
                entity.HasOne(d => d.idTraduccionNavigation)
                    .WithMany(p => p.tblFrecuenciaMantenimiento)
                    .HasForeignKey(d => d.idTraduccion)
                    .HasConstraintName("FK_tblFrecuenciaMantenimiento_tblTraduccion");
            });

            modelBuilder.Entity<tblGenero>(entity =>
            {
                entity.HasOne(d => d.idTraduccionNavigation)
                    .WithMany(p => p.tblGenero)
                    .HasForeignKey(d => d.idTraduccion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblGenero_tblTraduccion");
            });

            modelBuilder.Entity<tblGestionRetiro>(entity =>
            {
                entity.HasKey(e => e.idGestionRetiro)
                    .HasName("PK__tblGesti__19DAEA11BBF73E36");

                entity.Property(e => e.isValidado).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.idAreaLavanderiaNavigation)
                    .WithMany(p => p.tblGestionRetiro)
                    .HasForeignKey(d => d.idAreaLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblGestio__idAre__3648A49D");

                entity.HasOne(d => d.idCompañiaNavigation)
                    .WithMany(p => p.tblGestionRetiro)
                    .HasForeignKey(d => d.idCompañia)
                    .HasConstraintName("FK__tblGestio__idCom__3830ED0F");

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblGestionRetiro)
                    .HasForeignKey(d => d.idEntidad)
                    .HasConstraintName("FK__tblGestio__idEnt__5E218BCD");

                entity.HasOne(d => d.idFamiliaNavigation)
                    .WithMany(p => p.tblGestionRetiro)
                    .HasForeignKey(d => d.idFamilia)
                    .HasConstraintName("FK__tblGestio__idFam__11D639FD");

                entity.HasOne(d => d.idGrupoPlantillaPrenda_genericaNavigation)
                    .WithMany(p => p.tblGestionRetiro)
                    .HasForeignKey(d => d.idGrupoPlantillaPrenda_generica)
                    .HasConstraintName("FK_tblGestionRetiro_tblGrupoPlantillaPrenda_generica");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblGestionRetiro)
                    .HasForeignKey(d => d.idLavanderia)
                    .HasConstraintName("FK__tblGestio__idLav__36139A73");

                entity.HasOne(d => d.idMaquinaNavigation)
                    .WithMany(p => p.tblGestionRetiro)
                    .HasForeignKey(d => d.idMaquina)
                    .HasConstraintName("FK__tblGestio__idMaq__373CC8D6");

                entity.HasOne(d => d.idUsuarioNavigation)
                    .WithMany(p => p.tblGestionRetiroidUsuarioNavigation)
                    .HasForeignKey(d => d.idUsuario)
                    .HasConstraintName("FK__tblGestio__idUsu__35548064");

                entity.HasOne(d => d.idUsuarioValidadorNavigation)
                    .WithMany(p => p.tblGestionRetiroidUsuarioValidadorNavigation)
                    .HasForeignKey(d => d.idUsuarioValidador)
                    .HasConstraintName("FK__tblGestio__idUsu__3F9D04AD");
            });

            modelBuilder.Entity<tblGrupoArticulos>(entity =>
            {
                entity.HasOne(d => d.idAdmCuentaContableCompraNavigation)
                    .WithMany(p => p.tblGrupoArticulos)
                    .HasForeignKey(d => d.idAdmCuentaContableCompra)
                    .HasConstraintName("FK_tblGrupoArticulosCompra_tblAdmCuentaContable");
            });

            modelBuilder.Entity<tblGrupoEnergetico>(entity =>
            {
                entity.HasOne(d => d.idTipoKpiNavigation)
                    .WithMany(p => p.tblGrupoEnergetico)
                    .HasForeignKey(d => d.idTipoKpi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblGrupoEnergetico_tblTipoKpi");

                entity.HasOne(d => d.idTraduccionNavigation)
                    .WithMany(p => p.tblGrupoEnergetico)
                    .HasForeignKey(d => d.idTraduccion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblGrupoEnergetico_tblTraduccion");
            });

            modelBuilder.Entity<tblGrupoEntidad>(entity =>
            {
                entity.HasKey(e => e.idGrupoEntidad)
                    .HasName("PK_tblEntidad_Grupos");

                entity.Property(e => e.idGrupoEntidad).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<tblGrupoInventario_generico>(entity =>
            {
                entity.HasOne(d => d.idUsuarioNavigation)
                    .WithMany(p => p.tblGrupoInventario_generico)
                    .HasForeignKey(d => d.idUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblGrupoInventario_generico_tblUsuario");

                entity.HasMany(d => d.idInventario)
                    .WithMany(p => p.idGrupoInventario_generico)
                    .UsingEntity<Dictionary<string, object>>(
                        "tblInventarioNtblGrupoInventario_generico",
                        l => l.HasOne<tblInventario>().WithMany().HasForeignKey("idInventario").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblInventarioNtblGrupoInventario_generico_tblInventario"),
                        r => r.HasOne<tblGrupoInventario_generico>().WithMany().HasForeignKey("idGrupoInventario_generico").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblInventarioNtblGrupoInventario_generico_tblGrupoInventario_generico"),
                        j =>
                        {
                            j.HasKey("idGrupoInventario_generico", "idInventario");

                            j.ToTable("tblInventarioNtblGrupoInventario_generico", "Inventarios");
                        });
            });

            modelBuilder.Entity<tblGrupoPlantillaPrenda_generica>(entity =>
            {
                entity.Property(e => e.idGrupoPlantillaPrenda_generica).ValueGeneratedOnAdd();

                entity.HasOne(d => d.idCorporacionNavigation)
                    .WithMany(p => p.tblGrupoPlantillaPrenda_generica)
                    .HasForeignKey(d => d.idCorporacion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblGrupoPlantillaPrenda_generica_tblCorporacion");
            });

            modelBuilder.Entity<tblGrupoPregunta>(entity =>
            {
                entity.Property(e => e.idGrupoPregunta).ValueGeneratedNever();
            });

            modelBuilder.Entity<tblGrupoPrendaEst>(entity =>
            {
                entity.HasOne(d => d.idTipoElemLogNavigation)
                    .WithMany(p => p.tblGrupoPrendaEst)
                    .HasForeignKey(d => d.idTipoElemLog)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblGrupoPrendaEst_tblTipoElemLog");
            });

            modelBuilder.Entity<tblHistoricoAsientoNomina>(entity =>
            {
                entity.HasOne(d => d.idAdmCentroCosteNavigation)
                    .WithMany(p => p.tblHistoricoAsientoNomina)
                    .HasForeignKey(d => d.idAdmCentroCoste)
                    .HasConstraintName("FK_tblHistoricoAsientoNomina_tblAdmCentroCoste");

                entity.HasOne(d => d.idAdmElementoPEPNavigation)
                    .WithMany(p => p.tblHistoricoAsientoNomina)
                    .HasForeignKey(d => d.idAdmElementoPEP)
                    .HasConstraintName("FK_tblHistoricoAsientoNomina_tblAdmElementoPEP");

                entity.HasOne(d => d.idEstadoHistoricoAsientoNominaNavigation)
                    .WithMany(p => p.tblHistoricoAsientoNomina)
                    .HasForeignKey(d => d.idEstadoHistoricoAsientoNomina)
                    .HasConstraintName("FK_tblHistoricoAsientoNomina_tblEstadoHistoricoAsientoNomina");

                entity.HasOne(d => d.idNominaNavigation)
                    .WithMany(p => p.tblHistoricoAsientoNomina)
                    .HasForeignKey(d => d.idNomina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblHistoricoAsientoNomina_tblNomina");

                entity.HasOne(d => d.idUsuarioNavigation)
                    .WithMany(p => p.tblHistoricoAsientoNomina)
                    .HasForeignKey(d => d.idUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblHistoricoAsientoNomina_tblUsuario");
            });

            modelBuilder.Entity<tblHistoricoAsientoNomina_MX>(entity =>
            {
                entity.HasOne(d => d.idAdmCentroCosteNavigation)
                    .WithMany(p => p.tblHistoricoAsientoNomina_MX)
                    .HasForeignKey(d => d.idAdmCentroCoste)
                    .HasConstraintName("FK_tblHistoricoAsientoNomina_MX_tblAdmCentroCoste");

                entity.HasOne(d => d.idAdmElementoPEPNavigation)
                    .WithMany(p => p.tblHistoricoAsientoNomina_MX)
                    .HasForeignKey(d => d.idAdmElementoPEP)
                    .HasConstraintName("FK_tblHistoricoAsientoNomina_MX_tblAdmElementoPEP");

                entity.HasOne(d => d.idNomina_MXNavigation)
                    .WithMany(p => p.tblHistoricoAsientoNomina_MX)
                    .HasForeignKey(d => d.idNomina_MX)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblHistoricoAsientoNomina_MX_tblNomina_MX");

                entity.HasOne(d => d.idTipoNomina_MXNavigation)
                    .WithMany(p => p.tblHistoricoAsientoNomina_MX)
                    .HasForeignKey(d => d.idTipoNomina_MX)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblHistoricoAsientoNomina_MX_tblTipoNomina_MX");

                entity.HasOne(d => d.idUsuarioNavigation)
                    .WithMany(p => p.tblHistoricoAsientoNomina_MX)
                    .HasForeignKey(d => d.idUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblHistoricoAsientoNomina_MX_tblUsuario");
            });

            modelBuilder.Entity<tblHistoricoAsientoNomina_RD>(entity =>
            {
                entity.HasOne(d => d.idAdmCentroCosteNavigation)
                    .WithMany(p => p.tblHistoricoAsientoNomina_RD)
                    .HasForeignKey(d => d.idAdmCentroCoste)
                    .HasConstraintName("FK_tblHistoricoAsientoNomina_RD_tblAdmCentroCoste");

                entity.HasOne(d => d.idAdmElementoPEPNavigation)
                    .WithMany(p => p.tblHistoricoAsientoNomina_RD)
                    .HasForeignKey(d => d.idAdmElementoPEP)
                    .HasConstraintName("FK_tblHistoricoAsientoNomina_RD_tblAdmElementoPEP");

                entity.HasOne(d => d.idNomina_RDNavigation)
                    .WithMany(p => p.tblHistoricoAsientoNomina_RD)
                    .HasForeignKey(d => d.idNomina_RD)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblHistoricoAsientoNomina_RD_tblNomina_RD");

                entity.HasOne(d => d.idUsuarioNavigation)
                    .WithMany(p => p.tblHistoricoAsientoNomina_RD)
                    .HasForeignKey(d => d.idUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblHistoricoAsientoNomina_RD_tblUsuario");
            });

            modelBuilder.Entity<tblHistoricoNominas>(entity =>
            {
                entity.HasKey(e => new { e.idPersona, e.fechaAltaContrato, e.fechaNomina });

                entity.HasOne(d => d.idCategoriaInternaNavigation)
                    .WithMany(p => p.tblHistoricoNominas)
                    .HasForeignKey(d => d.idCategoriaInterna)
                    .HasConstraintName("FK_tblHistoricoNominas_tblCategoriaInterna");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblHistoricoNominas)
                    .HasForeignKey(d => d.idLavanderia)
                    .HasConstraintName("FK_tblHistoricoNominas_tblLavanderia");

                entity.HasOne(d => d.idPersonaNavigation)
                    .WithMany(p => p.tblHistoricoNominas)
                    .HasForeignKey(d => d.idPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblHistoricoNominas_tblPersona");

                entity.HasOne(d => d.idTipoContratoNavigation)
                    .WithMany(p => p.tblHistoricoNominas)
                    .HasForeignKey(d => d.idTipoContrato)
                    .HasConstraintName("FK_tblHistoricoNominas_tblTipoContrato");

                entity.HasOne(d => d.idTipoTrabajoNavigation)
                    .WithMany(p => p.tblHistoricoNominas)
                    .HasForeignKey(d => d.idTipoTrabajo)
                    .HasConstraintName("FK_tblHistoricoNominas_tblTipoTrabajo");
            });

            modelBuilder.Entity<tblHistoricoPartidaContable>(entity =>
            {
                entity.HasOne(d => d.idAdmCentroCosteNavigation)
                    .WithMany(p => p.tblHistoricoPartidaContable)
                    .HasForeignKey(d => d.idAdmCentroCoste)
                    .HasConstraintName("FK_tblHistoricoPartidaContable_tblAdmCentroCoste");

                entity.HasOne(d => d.idAdmElementoPEPNavigation)
                    .WithMany(p => p.tblHistoricoPartidaContable)
                    .HasForeignKey(d => d.idAdmElementoPEP)
                    .HasConstraintName("FK_tblHistoricoPartidaContable_tblAdmElementoPEP");

                entity.HasOne(d => d.idHistoricoPlanificacionNavigation)
                    .WithMany(p => p.tblHistoricoPartidaContable)
                    .HasForeignKey(d => d.idHistoricoPlanificacion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblHistoricoPartidaContable_tblHistoricoPlanificacion");

                entity.HasOne(d => d.idMonedaNavigation)
                    .WithMany(p => p.tblHistoricoPartidaContable)
                    .HasForeignKey(d => d.idMoneda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblHistoricoPartidaContable_tblMoneda");
            });

            modelBuilder.Entity<tblHistoricoPlanificacion>(entity =>
            {
                entity.HasOne(d => d.idAdmCentroCosteNavigation)
                    .WithMany(p => p.tblHistoricoPlanificacion)
                    .HasForeignKey(d => d.idAdmCentroCoste)
                    .HasConstraintName("FK_tblHistoricoPlanificacion_tblAdmCentroCoste");

                entity.HasOne(d => d.idAdmCuentaContableNavigation)
                    .WithMany(p => p.tblHistoricoPlanificacion)
                    .HasForeignKey(d => d.idAdmCuentaContable)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblHistoricoPlanificacion_tblAdmCuentaContable");

                entity.HasOne(d => d.idAdmElementoPEPNavigation)
                    .WithMany(p => p.tblHistoricoPlanificacion)
                    .HasForeignKey(d => d.idAdmElementoPEP)
                    .HasConstraintName("FK_tblHistoricoPlanificacion_tblAdmElementoPEP");

                entity.HasOne(d => d.idEmpresaPolarierNavigation)
                    .WithMany(p => p.tblHistoricoPlanificacion)
                    .HasForeignKey(d => d.idEmpresaPolarier)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblHistoricoPlanificacion_tblEmpresasPolarier");
            });

            modelBuilder.Entity<tblHorarioRepartoNEntidad>(entity =>
            {
                entity.HasKey(e => new { e.idEntidad, e.fecha })
                    .HasName("PK__tblHorar__0FBF20348AA422B2");

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblHorarioRepartoNEntidad)
                    .HasForeignKey(d => d.idEntidad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblHorari__idEnt__3CAABE12");
            });

            modelBuilder.Entity<tblImagenNCliente>(entity =>
            {
                entity.HasOne(d => d.idAdmClienteNavigation)
                    .WithMany(p => p.tblImagenNCliente)
                    .HasForeignKey(d => d.idAdmCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblImagenNCliente_tblAdmCliente");
            });

            modelBuilder.Entity<tblImagenNProveedor>(entity =>
            {
                entity.HasOne(d => d.idAdmProveedorNavigation)
                    .WithMany(p => p.tblImagenNProveedor)
                    .HasForeignKey(d => d.idAdmProveedor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblImagenNProveedor_tblAdmProveedor");
            });

            modelBuilder.Entity<tblIncidencia>(entity =>
            {
                entity.Property(e => e.isApp).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.idCompañiaNavigation)
                    .WithMany(p => p.tblIncidencia)
                    .HasForeignKey(d => d.idCompañia)
                    .HasConstraintName("FK_tblIncidencia_tblCompañia");

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblIncidencia)
                    .HasForeignKey(d => d.idEntidad)
                    .HasConstraintName("FK_tblIncidencia_tblEntidad");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblIncidencia)
                    .HasForeignKey(d => d.idLavanderia)
                    .HasConstraintName("FK_tblIncidencia_tblLavanderia");

                entity.HasOne(d => d.idMaquinaNavigation)
                    .WithMany(p => p.tblIncidencia)
                    .HasForeignKey(d => d.idMaquina)
                    .HasConstraintName("FK_tblIncidencia_tblMaquina");

                entity.HasOne(d => d.idPoliticaDisciplinariaNavigation)
                    .WithMany(p => p.tblIncidencia)
                    .HasForeignKey(d => d.idPoliticaDisciplinaria)
                    .HasConstraintName("FK_tblIncidencia_tblPoliticaDisciplinaria");

                entity.HasOne(d => d.idSubTipoIncidenciaNavigation)
                    .WithMany(p => p.tblIncidencia)
                    .HasForeignKey(d => d.idSubTipoIncidencia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblIncidencia_tblTipoSubIncidencia");

                entity.HasOne(d => d.idUsuarioAfectaNavigation)
                    .WithMany(p => p.tblIncidenciaidUsuarioAfectaNavigation)
                    .HasForeignKey(d => d.idUsuarioAfecta)
                    .HasConstraintName("FK_tblIncidencia_tblPersona");

                entity.HasOne(d => d.idUsuarioCreaNavigation)
                    .WithMany(p => p.tblIncidenciaidUsuarioCreaNavigation)
                    .HasForeignKey(d => d.idUsuarioCrea)
                    .HasConstraintName("FK__tblIncide__idUsu__08362A7C");

                entity.HasOne(d => d.idUsuarioResponsableNavigation)
                    .WithMany(p => p.tblIncidenciaidUsuarioResponsableNavigation)
                    .HasForeignKey(d => d.idUsuarioResponsable)
                    .HasConstraintName("FK_tblIncidencia_tblPersona2");

                entity.HasOne(d => d.idUsuarioRevisorNavigation)
                    .WithMany(p => p.tblIncidenciaidUsuarioRevisorNavigation)
                    .HasForeignKey(d => d.idUsuarioRevisor)
                    .HasConstraintName("FK_tblIncidencia_tblUsuario");

                entity.HasOne(d => d.idVehiculoNavigation)
                    .WithMany(p => p.tblIncidencia)
                    .HasForeignKey(d => d.idVehiculo)
                    .HasConstraintName("FK_tblIncidencia_tblVehiculo");
            });

            modelBuilder.Entity<tblIncidenciaNParte>(entity =>
            {
                entity.HasKey(e => new { e.idIncidencia, e.idParte })
                    .HasName("PK__tblIncid__0BB226258DA3C3C7");

                entity.HasOne(d => d.idIncidenciaNavigation)
                    .WithMany(p => p.tblIncidenciaNParte)
                    .HasForeignKey(d => d.idIncidencia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblIncidenciaNParte_tblIncidenciaNParte");

                entity.HasOne(d => d.idParteNavigation)
                    .WithMany(p => p.tblIncidenciaNParte)
                    .HasForeignKey(d => d.idParte)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblIncidenciaNParte_tblParteTrabajo");
            });

            modelBuilder.Entity<tblIncidenciaNReunion>(entity =>
            {
                entity.HasKey(e => e.idIncidenciaNReunion)
                    .HasName("PK_tblIncidenciaNReunion_1");

                entity.HasOne(d => d.idIncidenciaNavigation)
                    .WithMany(p => p.tblIncidenciaNReunion)
                    .HasForeignKey(d => d.idIncidencia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblIncidenciaNReunion_tblIncidencia");

                entity.HasOne(d => d.idReunionNavigation)
                    .WithMany(p => p.tblIncidenciaNReunion)
                    .HasForeignKey(d => d.idReunion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblIncidenciaNReunion_tblReunion");
            });

            modelBuilder.Entity<tblIncidencia_Documento>(entity =>
            {
                entity.HasKey(e => new { e.idDocumento, e.idIncidencia });

                entity.Property(e => e.idDocumento).ValueGeneratedOnAdd();

                entity.HasOne(d => d.idIncidenciaNavigation)
                    .WithMany(p => p.tblIncidencia_Documento)
                    .HasForeignKey(d => d.idIncidencia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblIncidencia_Documento_tblIncidencia");
            });

            modelBuilder.Entity<tblIngresosPresupuestados>(entity =>
            {
                entity.HasKey(e => new { e.idEntidad, e.fecha, e.idLavanderia });

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblIngresosPresupuestados)
                    .HasForeignKey(d => d.idEntidad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblIngresosPresupuestados_tblEntidad");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblIngresosPresupuestados)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblIngresosPresupuestados_tblLavanderia");
            });

            modelBuilder.Entity<tblInventario>(entity =>
            {
                entity.HasOne(d => d.idCompañiaNavigation)
                    .WithMany(p => p.tblInventario)
                    .HasForeignKey(d => d.idCompañia)
                    .HasConstraintName("FK_tblInventario_tblCompañia");

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblInventario)
                    .HasForeignKey(d => d.idEntidad)
                    .HasConstraintName("FK_tblInventario_tblEntidad");
            });

            modelBuilder.Entity<tblInventario_Documento>(entity =>
            {
                entity.HasKey(e => e.idDocumento)
                    .HasName("PK__tblInven__572A36FC54C1E78D");

                entity.HasOne(d => d.idInventarioNavigation)
                    .WithMany(p => p.tblInventario_Documento)
                    .HasForeignKey(d => d.idInventario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblInventario_Documento_tblInventario");
            });

            modelBuilder.Entity<tblIvaNPais>(entity =>
            {
                entity.HasOne(d => d.idAdmIvaNavigation)
                    .WithMany(p => p.tblIvaNPais)
                    .HasForeignKey(d => d.idAdmIva)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblIvaNPais_tblAdmIva");

                entity.HasOne(d => d.idPaisNavigation)
                    .WithMany(p => p.tblIvaNPais)
                    .HasForeignKey(d => d.idPais)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblIvaNPais_tblPais");
            });

            modelBuilder.Entity<tblJornada>(entity =>
            {
                entity.HasKey(e => e.idJornada)
                    .HasName("PK__tblJorna__236064783ED9AA86");

                entity.HasOne(d => d.idCuadrantePersonalNavigation)
                    .WithMany(p => p.tblJornada)
                    .HasForeignKey(d => d.idCuadrantePersonal)
                    .HasConstraintName("FK_tblJornada_tblCuadrantePersonal");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblJornada)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblJornada_tblLavanderia");

                entity.HasOne(d => d.idMotivoIncumplimiento_horaFinNavigation)
                    .WithMany(p => p.tblJornadaidMotivoIncumplimiento_horaFinNavigation)
                    .HasForeignKey(d => d.idMotivoIncumplimiento_horaFin)
                    .HasConstraintName("FK_tblJornada_tblMotivoIncumplimiento_horaFin");

                entity.HasOne(d => d.idMotivoIncumplimiento_horaIniNavigation)
                    .WithMany(p => p.tblJornadaidMotivoIncumplimiento_horaIniNavigation)
                    .HasForeignKey(d => d.idMotivoIncumplimiento_horaIni)
                    .HasConstraintName("FK_tblJornada_tblMotivoIncumplimiento_horaIni");

                entity.HasOne(d => d.idMotivoIncumplimiento_tiempoDescansoNavigation)
                    .WithMany(p => p.tblJornadaidMotivoIncumplimiento_tiempoDescansoNavigation)
                    .HasForeignKey(d => d.idMotivoIncumplimiento_tiempoDescanso)
                    .HasConstraintName("FK_tblJornada_tblMotivoIncumplimiento_tiempoDescanso");

                entity.HasOne(d => d.idPersonaNavigation)
                    .WithMany(p => p.tblJornada)
                    .HasForeignKey(d => d.idPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblJornada_tblPersona");

                entity.HasOne(d => d.idTipoTrabajoNavigation)
                    .WithMany(p => p.tblJornada)
                    .HasForeignKey(d => d.idTipoTrabajo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblJornada_tblTipoTrabajo");

                entity.HasOne(d => d.idTurnoNavigation)
                    .WithMany(p => p.tblJornada)
                    .HasForeignKey(d => d.idTurno)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblJornada_tblTurno");

                entity.HasOne(d => d.idUsuario_validacionNavigation)
                    .WithMany(p => p.tblJornada)
                    .HasForeignKey(d => d.idUsuario_validacion)
                    .HasConstraintName("FK_tblJornada_tblPersona_idUsuario_validacion");
            });

            modelBuilder.Entity<tblJornadaPersona>(entity =>
            {
                entity.HasKey(e => new { e.idPersona, e.fecha })
                    .HasName("PK__tblJorna__8A69C0736D0BC40F");

                entity.Property(e => e.isRegManual).HasComment("1 - Manual, 2 - Tarjeta, 3 - MyPo");

                entity.Property(e => e.isRevisado_horaEntrada).HasComment("Null - Sin accion, 0 - No añadir, 1 - Añadir");

                entity.Property(e => e.isRevisado_horaSalida).HasComment("Null - Sin accion, 0 - No añadir, 1 - Añadir");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblJornadaPersona)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblJornadaPersona_tblLavanderia");

                entity.HasOne(d => d.idPersonaNavigation)
                    .WithMany(p => p.tblJornadaPersona)
                    .HasForeignKey(d => d.idPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblJornadaPersona_tblPersona");

                entity.HasOne(d => d.idTurnoNavigation)
                    .WithMany(p => p.tblJornadaPersona)
                    .HasForeignKey(d => d.idTurno)
                    .HasConstraintName("FK_tblJornadaPersona_tblTurno");
            });

            modelBuilder.Entity<tblKgLavadosLavadora>(entity =>
            {
                entity.HasOne(d => d.idCompañiaNavigation)
                    .WithMany(p => p.tblKgLavadosLavadora)
                    .HasForeignKey(d => d.idCompañia)
                    .HasConstraintName("FK_tblKgLavadosLavadora_tblCompañia");

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblKgLavadosLavadora)
                    .HasForeignKey(d => d.idEntidad)
                    .HasConstraintName("FK_tblKgLavadosLavadora_tblEntidad");

                entity.HasOne(d => d.idGrupoPlantillaPrenda_genericaNavigation)
                    .WithMany(p => p.tblKgLavadosLavadora)
                    .HasForeignKey(d => d.idGrupoPlantillaPrenda_generica)
                    .HasConstraintName("FK_tblKgLavadosLavadora_tblGrupoPlantillaPrenda_generica");

                entity.HasOne(d => d.idMaquinaNavigation)
                    .WithMany(p => p.tblKgLavadosLavadora)
                    .HasForeignKey(d => d.idMaquina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblKgLavadosLavadora_tblMaquina");
            });

            modelBuilder.Entity<tblKgLavadosTunel>(entity =>
            {
                entity.HasOne(d => d.idCompañiaNavigation)
                    .WithMany(p => p.tblKgLavadosTunel)
                    .HasForeignKey(d => d.idCompañia)
                    .HasConstraintName("FK_tblKgLavadosTunel_tblCompañia");

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblKgLavadosTunel)
                    .HasForeignKey(d => d.idEntidad)
                    .HasConstraintName("FK_tblKgLavadosTunel_tblEntidad");

                entity.HasOne(d => d.idGrupoPlantillaPrenda_genericaNavigation)
                    .WithMany(p => p.tblKgLavadosTunel)
                    .HasForeignKey(d => d.idGrupoPlantillaPrenda_generica)
                    .HasConstraintName("FK_tblKgLavadosTunel_tblGrupoPlantillaPrenda_generica");

                entity.HasOne(d => d.idMaquinaNavigation)
                    .WithMany(p => p.tblKgLavadosTunel)
                    .HasForeignKey(d => d.idMaquina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblKgLavadosTunel_tblMaquina");
            });

            modelBuilder.Entity<tblKgNPresupuestoKg>(entity =>
            {
                entity.HasKey(e => new { e.idPresupuestoKg, e.idMes });

                entity.HasOne(d => d.idMesNavigation)
                    .WithMany(p => p.tblKgNPresupuestoKg)
                    .HasForeignKey(d => d.idMes)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblKgNPresupuestoKg_tblMes");

                entity.HasOne(d => d.idPresupuestoKgNavigation)
                    .WithMany(p => p.tblKgNPresupuestoKg)
                    .HasForeignKey(d => d.idPresupuestoKg)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblKgNPresupuestoKg_tblPresupuestoKg");
            });

            modelBuilder.Entity<tblKgRealesNPresupuestoKg>(entity =>
            {
                entity.HasKey(e => new { e.idPresupuestoKg, e.idMes });

                entity.HasOne(d => d.idMesNavigation)
                    .WithMany(p => p.tblKgRealesNPresupuestoKg)
                    .HasForeignKey(d => d.idMes)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblKgRealesNPresupuestoKg_tblMes");

                entity.HasOne(d => d.idPresupuestoKgNavigation)
                    .WithMany(p => p.tblKgRealesNPresupuestoKg)
                    .HasForeignKey(d => d.idPresupuestoKg)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblKgRealesNPresupuestoKg_tblPresupuestoKg");
            });

            modelBuilder.Entity<tblLavanderia>(entity =>
            {
                entity.Property(e => e.enableRFID_Personal).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.idAdmElementoPEPNavigation)
                    .WithMany(p => p.tblLavanderia)
                    .HasForeignKey(d => d.idAdmElementoPEP)
                    .HasConstraintName("FK_tblLavanderia_tblAdmElementoPEP");

                entity.HasOne(d => d.idCorporacionNavigation)
                    .WithMany(p => p.tblLavanderia)
                    .HasForeignKey(d => d.idCorporacion)
                    .HasConstraintName("FK_tblLavanderia_tblCorporacion");

                entity.HasOne(d => d.idEmpresaPolarierNavigation)
                    .WithMany(p => p.tblLavanderia)
                    .HasForeignKey(d => d.idEmpresaPolarier)
                    .HasConstraintName("FK_tblLavanderia_tblEmpresasPolarier");

                entity.HasOne(d => d.idIdiomaNavigation)
                    .WithMany(p => p.tblLavanderia)
                    .HasForeignKey(d => d.idIdioma)
                    .HasConstraintName("FK_tblLavanderia_tblIdioma");

                entity.HasOne(d => d.idLocalizacionNavigation)
                    .WithMany(p => p.tblLavanderia)
                    .HasForeignKey(d => d.idLocalizacion)
                    .HasConstraintName("FK_tblLavanderia_tblLocalizacion");

                entity.HasOne(d => d.idMonedaNavigation)
                    .WithMany(p => p.tblLavanderiaidMonedaNavigation)
                    .HasForeignKey(d => d.idMoneda)
                    .HasConstraintName("FK_tblLavanderia_tblMoneda");

                entity.HasOne(d => d.idMonedaLocalNavigation)
                    .WithMany(p => p.tblLavanderiaidMonedaLocalNavigation)
                    .HasForeignKey(d => d.idMonedaLocal)
                    .HasConstraintName("FK_tblLavanderia_tblMoneda1");

                entity.HasOne(d => d.idPaisNavigation)
                    .WithMany(p => p.tblLavanderia)
                    .HasForeignKey(d => d.idPais)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblLavanderia_tblPais");

                entity.HasOne(d => d.idUnidadesPesoNavigation)
                    .WithMany(p => p.tblLavanderia)
                    .HasForeignKey(d => d.idUnidadesPeso)
                    .HasConstraintName("FK_tblLavanderia_tblUnidadesPeso");

                entity.HasOne(d => d.idZonaHorariaNavigation)
                    .WithMany(p => p.tblLavanderia)
                    .HasForeignKey(d => d.idZonaHoraria)
                    .HasConstraintName("FK_tblLavanderia_tblZonaHoraria");

                entity.HasMany(d => d.idEntidad)
                    .WithMany(p => p.idLavanderia)
                    .UsingEntity<Dictionary<string, object>>(
                        "tblEntidadNLavanderia",
                        l => l.HasOne<tblEntidad>().WithMany().HasForeignKey("idEntidad").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblEntidadNLavanderia_tblEntidad"),
                        r => r.HasOne<tblLavanderia>().WithMany().HasForeignKey("idLavanderia").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblEntidadNLavanderia_tblLavanderia"),
                        j =>
                        {
                            j.HasKey("idLavanderia", "idEntidad");

                            j.ToTable("tblEntidadNLavanderia", "General");
                        });

                entity.HasMany(d => d.idGrupoPrendaEst)
                    .WithMany(p => p.idLavanderia)
                    .UsingEntity<Dictionary<string, object>>(
                        "tblGrupoPrendaEstNLavanderia",
                        l => l.HasOne<tblGrupoPrendaEst>().WithMany().HasForeignKey("idGrupoPrendaEst").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblGrupoPrendaEstNLavanderia_tblGrupoPrendaEst"),
                        r => r.HasOne<tblLavanderia>().WithMany().HasForeignKey("idLavanderia").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblGrupoPrendaEstNLavanderia_tblLavanderia"),
                        j =>
                        {
                            j.HasKey("idLavanderia", "idGrupoPrendaEst");

                            j.ToTable("tblGrupoPrendaEstNLavanderia", "Logistica");
                        });

                entity.HasMany(d => d.idTipoProduccion)
                    .WithMany(p => p.idLavanderia)
                    .UsingEntity<Dictionary<string, object>>(
                        "tblTipoProduccionNLavanderia",
                        l => l.HasOne<tblTipoProduccion>().WithMany().HasForeignKey("idTipoProduccion").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblTipoProduccionNLavanderia_tblTipoProduccion"),
                        r => r.HasOne<tblLavanderia>().WithMany().HasForeignKey("idLavanderia").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblTipoProduccionNLavanderia_tblLavanderia"),
                        j =>
                        {
                            j.HasKey("idLavanderia", "idTipoProduccion");

                            j.ToTable("tblTipoProduccionNLavanderia", "Produccion");
                        });

                entity.HasMany(d => d.idTipoRechazo)
                    .WithMany(p => p.idLavanderia)
                    .UsingEntity<Dictionary<string, object>>(
                        "tblRechazoNLavanderia",
                        l => l.HasOne<tblTipoRechazo>().WithMany().HasForeignKey("idTipoRechazo").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblRechazoNLavanderia_tblTipoRechazo"),
                        r => r.HasOne<tblLavanderia>().WithMany().HasForeignKey("idLavanderia").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblRechazoNLavanderia_tblLavanderia"),
                        j =>
                        {
                            j.HasKey("idLavanderia", "idTipoRechazo").HasName("PK_tblRechazoNLavanderia_1");

                            j.ToTable("tblRechazoNLavanderia", "Produccion");
                        });

                entity.HasMany(d => d.idUsuario)
                    .WithMany(p => p.idLavanderia)
                    .UsingEntity<Dictionary<string, object>>(
                        "tblLavanderiaNUsuario",
                        l => l.HasOne<tblUsuario>().WithMany().HasForeignKey("idUsuario").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblLavanderiaNUsuario_tblUsuario"),
                        r => r.HasOne<tblLavanderia>().WithMany().HasForeignKey("idLavanderia").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblLavanderiaNUsuario_tblLavanderia"),
                        j =>
                        {
                            j.HasKey("idLavanderia", "idUsuario").HasName("PK__tblLavan__0A2323AA17A4F9A9");

                            j.ToTable("tblLavanderiaNUsuario", "GestionInterna");
                        });
            });

            modelBuilder.Entity<tblLayout_SmartView>(entity =>
            {
                entity.HasKey(e => e.idLayout_SmartView)
                    .HasName("PK__tblLayou__50C38AAAEB70773E");

                entity.Property(e => e.height).HasDefaultValueSql("((1))");

                entity.Property(e => e.width).HasDefaultValueSql("((1))");

                entity.Property(e => e.x).HasDefaultValueSql("((0))");

                entity.Property(e => e.y).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.idAreaLavanderiaNavigation)
                    .WithMany(p => p.tblLayout_SmartView)
                    .HasForeignKey(d => d.idAreaLavanderia)
                    .HasConstraintName("FK_tblLayout_SmartView_tblAreaLavanderia");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblLayout_SmartView)
                    .HasForeignKey(d => d.idLavanderia)
                    .HasConstraintName("FK_tblLayout_SmartView_tblLavanderia");

                entity.HasOne(d => d.idMaquinaNavigation)
                    .WithMany(p => p.tblLayout_SmartView)
                    .HasForeignKey(d => d.idMaquina)
                    .HasConstraintName("FK_tblLayout_SmartView_tblMaquina");
            });

            modelBuilder.Entity<tblLecturaCarro>(entity =>
            {
                entity.HasKey(e => e.idLecturaCarro)
                    .HasName("PK__tblLectu__091F08A752FF0091");

                entity.Property(e => e.idLecturaCarro_Estado).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.idCantidadNMovimientoElemLogNavigation)
                    .WithMany(p => p.tblLecturaCarro)
                    .HasForeignKey(d => d.idCantidadNMovimientoElemLog)
                    .HasConstraintName("FK__tblLectur__idCan__6617B350");

                entity.HasOne(d => d.idCarroNavigation)
                    .WithMany(p => p.tblLecturaCarro)
                    .HasForeignKey(d => d.idCarro)
                    .HasConstraintName("FK__tblLectur__idCar__642F6ADE");

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblLecturaCarro)
                    .HasForeignKey(d => d.idEntidad)
                    .HasConstraintName("FK__tblLectur__idEnt__6ADC686D");

                entity.HasOne(d => d.idEstadoMovimientoElemLogNavigation)
                    .WithMany(p => p.tblLecturaCarro)
                    .HasForeignKey(d => d.idEstadoMovimientoElemLog)
                    .HasConstraintName("FK__tblLectur__idEst__65238F17");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblLecturaCarro)
                    .HasForeignKey(d => d.idLavanderia)
                    .HasConstraintName("FK__tblLectur__idLav__69E84434");

                entity.HasOne(d => d.idLecturaCarro_EstadoNavigation)
                    .WithMany(p => p.tblLecturaCarro)
                    .HasForeignKey(d => d.idLecturaCarro_Estado)
                    .HasConstraintName("FK__tblLectur__idLec__67FFFBC2");

                entity.HasOne(d => d.idTipoLecturaNavigation)
                    .WithMany(p => p.tblLecturaCarro)
                    .HasForeignKey(d => d.idTipoLectura)
                    .HasConstraintName("FK__tblLectur__idTip__670BD789");

                entity.HasOne(d => d.idUsuarioNavigation)
                    .WithMany(p => p.tblLecturaCarro)
                    .HasForeignKey(d => d.idUsuario)
                    .HasConstraintName("FK__tblLectur__idUsu__68F41FFB");
            });

            modelBuilder.Entity<tblLecturaCarro_Estado>(entity =>
            {
                entity.HasKey(e => e.idLecturaCarro_Estado)
                    .HasName("PK__tblLectu__0E6E4CAC799208F0");
            });

            modelBuilder.Entity<tblLecturaContador>(entity =>
            {
                entity.HasKey(e => e.idLecturaContador)
                    .HasName("PK_tblDataControl");

                entity.Property(e => e.idRecursoContador).HasDefaultValueSql("((4))");

                entity.HasOne(d => d.idRecursoContadorNavigation)
                    .WithMany(p => p.tblLecturaContador)
                    .HasForeignKey(d => d.idRecursoContador)
                    .HasConstraintName("FK_tblLecturaContador_tblRecursoContador");
            });

            modelBuilder.Entity<tblLecturaLavadoras>(entity =>
            {
                entity.HasKey(e => new { e.idMaquina, e.fecha });

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblLecturaLavadoras)
                    .HasForeignKey(d => d.idEntidad)
                    .HasConstraintName("FK_tblLecturaLavadoras_tblEntidad");

                entity.HasOne(d => d.idMaquinaNavigation)
                    .WithMany(p => p.tblLecturaLavadoras)
                    .HasForeignKey(d => d.idMaquina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblLecturaLavadoras_tblMaquina");

                entity.HasOne(d => d.idProgramaNavigation)
                    .WithMany(p => p.tblLecturaLavadoras)
                    .HasForeignKey(d => d.idPrograma)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblLecturaLavadoras_tblProgramasLavadora");
            });

            modelBuilder.Entity<tblLibreMensual>(entity =>
            {
                entity.HasOne(d => d.idPersonaNavigation)
                    .WithMany(p => p.tblLibreMensual)
                    .HasForeignKey(d => d.idPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblLibreMensual_tblPersona");
            });

            modelBuilder.Entity<tblLibreSemanal>(entity =>
            {
                entity.HasOne(d => d.idPersonaNavigation)
                    .WithMany(p => p.tblLibreSemanal)
                    .HasForeignKey(d => d.idPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblLibreSemanal_tblPersona");
            });

            modelBuilder.Entity<tblLicenciaConducir>(entity =>
            {
                entity.HasMany(d => d.idPersona)
                    .WithMany(p => p.idLicenciaConducir)
                    .UsingEntity<Dictionary<string, object>>(
                        "tblLicenciaConducirNPersona",
                        l => l.HasOne<tblPersona>().WithMany().HasForeignKey("idPersona").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblLicenciaConducirNPersona_tblPersona"),
                        r => r.HasOne<tblLicenciaConducir>().WithMany().HasForeignKey("idLicenciaConducir").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblLicenciaConducirNPersona_tblLicenciaConducir"),
                        j =>
                        {
                            j.HasKey("idLicenciaConducir", "idPersona");

                            j.ToTable("tblLicenciaConducirNPersona", "RRHH");
                        });
            });

            modelBuilder.Entity<tblLlamamiento>(entity =>
            {
                entity.Property(e => e.activo).HasDefaultValueSql("((1))");

                entity.Property(e => e.idTipoContrato).HasDefaultValueSql("((4))");

                entity.HasOne(d => d.idCategoriaInternaNavigation)
                    .WithMany(p => p.tblLlamamiento)
                    .HasForeignKey(d => d.idCategoriaInterna)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblLlamamiento_tblCategoriaInterna");

                entity.HasOne(d => d.idCentroTrabajoNavigation)
                    .WithMany(p => p.tblLlamamiento)
                    .HasForeignKey(d => d.idCentroTrabajo)
                    .HasConstraintName("FK_tblLlamamiento_tblCentroTrabajo");

                entity.HasOne(d => d.idFormatoDiasLibresNavigation)
                    .WithMany(p => p.tblLlamamiento)
                    .HasForeignKey(d => d.idFormatoDiasLibres)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblLlamamiento_tblFormatoDiasLibres");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblLlamamiento)
                    .HasForeignKey(d => d.idLavanderia)
                    .HasConstraintName("FK_tblLlamamiento_tblLavanderia");

                entity.HasOne(d => d.idPersonaNavigation)
                    .WithMany(p => p.tblLlamamiento)
                    .HasForeignKey(d => d.idPersona)
                    .HasConstraintName("FK_tblLlamamiento_tblPersona");

                entity.HasOne(d => d.idTipoContratoNavigation)
                    .WithMany(p => p.tblLlamamiento)
                    .HasForeignKey(d => d.idTipoContrato)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblLlamamiento_tblTipoContrato");

                entity.HasOne(d => d.idTipoTrabajoNavigation)
                    .WithMany(p => p.tblLlamamiento)
                    .HasForeignKey(d => d.idTipoTrabajo)
                    .HasConstraintName("FK_tblLlamamiento_tblTipoTrabajo");

                entity.HasOne(d => d.idTurnoNavigation)
                    .WithMany(p => p.tblLlamamiento)
                    .HasForeignKey(d => d.idTurno)
                    .HasConstraintName("FK_tblLlamamiento_tblTurno");
            });

            modelBuilder.Entity<tblLocalizacion>(entity =>
            {
                entity.Property(e => e.idLocalizacion).ValueGeneratedNever();

                entity.HasOne(d => d.idPaisNavigation)
                    .WithMany(p => p.tblLocalizacion)
                    .HasForeignKey(d => d.idPais)
                    .HasConstraintName("FK_tblLocalizacion_tblPais");

                entity.HasOne(d => d.idZonaHorariaNavigation)
                    .WithMany(p => p.tblLocalizacion)
                    .HasForeignKey(d => d.idZonaHoraria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblLocalizacion_tblZonaHoraria");
            });

            modelBuilder.Entity<tblLog>(entity =>
            {
                entity.Property(e => e.idToken).HasDefaultValueSql("(newid())");

                entity.Property(e => e.fecha).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.idUsuarioNavigation)
                    .WithMany(p => p.tblLog)
                    .HasForeignKey(d => d.idUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblLog_tblUsuario");
            });

            modelBuilder.Entity<tblLogAcciones>(entity =>
            {
                entity.HasKey(e => new { e.idUsuario, e.fecha });

                entity.HasOne(d => d.idAccionNavigation)
                    .WithMany(p => p.tblLogAcciones)
                    .HasForeignKey(d => d.idAccion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblLogAcciones_tblAccionUsuario");

                entity.HasOne(d => d.idFormularioNavigation)
                    .WithMany(p => p.tblLogAcciones)
                    .HasForeignKey(d => d.idFormulario)
                    .HasConstraintName("FK_tblLogAcciones_tblFormulario");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblLogAcciones)
                    .HasForeignKey(d => d.idLavanderia)
                    .HasConstraintName("FK_tblLogAcciones_tblLavanderia");

                entity.HasOne(d => d.idUsuarioNavigation)
                    .WithMany(p => p.tblLogAcciones)
                    .HasForeignKey(d => d.idUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblLogAcciones_tblUsuario");
            });

            modelBuilder.Entity<tblLogAcciones_App>(entity =>
            {
                entity.HasKey(e => new { e.idUsuario, e.fecha });

                entity.Property(e => e.isAndroid).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.idUsuarioNavigation)
                    .WithMany(p => p.tblLogAcciones_App)
                    .HasForeignKey(d => d.idUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblLogAcciones_App_tblUsuario");
            });

            modelBuilder.Entity<tblLogConexiones>(entity =>
            {
                entity.Property(e => e.tipoConexion).HasComment(" 1 - httpResponse_pulsosEnergyHub");

                entity.HasOne(d => d.idEnergyHubNavigation)
                    .WithMany(p => p.tblLogConexiones)
                    .HasForeignKey(d => d.idEnergyHub)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblLogConexiones_tblEnergyHub");
            });

            modelBuilder.Entity<tblMantenimientoNMaquina>(entity =>
            {
                entity.HasOne(d => d.idTipoMantenimientoNMaquinaNavigation)
                    .WithMany(p => p.tblMantenimientoNMaquina)
                    .HasForeignKey(d => d.idTipoMantenimientoNMaquina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMantenimientoNMaquina_tblTipoMantenimientoMaquina");

                entity.HasOne(d => d.idUsuarioNavigation)
                    .WithMany(p => p.tblMantenimientoNMaquina)
                    .HasForeignKey(d => d.idUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMantenimientoNMaquina_tblUsuario");
            });

            modelBuilder.Entity<tblMantenimientoPrev>(entity =>
            {
                entity.HasOne(d => d.idMaquinaNavigation)
                    .WithMany(p => p.tblMantenimientoPrev)
                    .HasForeignKey(d => d.idMaquina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMantenimientoPrev_tblMaquina");

                entity.HasOne(d => d.idMovimientoRecambioNavigation)
                    .WithMany(p => p.tblMantenimientoPrev)
                    .HasForeignKey(d => d.idMovimientoRecambio)
                    .HasConstraintName("FK_tblMantenimientoPrev_tblMovimientoRecambio");

                entity.HasOne(d => d.idTareaMantenimientoPrevNavigation)
                    .WithMany(p => p.tblMantenimientoPrev)
                    .HasForeignKey(d => d.idTareaMantenimientoPrev)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMantenimientoPrev_tblTareaMantenimientoPrev");

                entity.HasOne(d => d.idUsuarioNavigation)
                    .WithMany(p => p.tblMantenimientoPrev)
                    .HasForeignKey(d => d.idUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMantenimientoPrev_tblUsuario");

                entity.HasMany(d => d.idPersona)
                    .WithMany(p => p.idMantenimientoPrev)
                    .UsingEntity<Dictionary<string, object>>(
                        "tblPersonaNMantenimientoPrev",
                        l => l.HasOne<tblPersona>().WithMany().HasForeignKey("idPersona").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblPersonaNMantenimientoPrev_tblPersona"),
                        r => r.HasOne<tblMantenimientoPrev>().WithMany().HasForeignKey("idMantenimientoPrev").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblPersonaNMantenimientoPrev_tblMantenimientoPrev"),
                        j =>
                        {
                            j.HasKey("idMantenimientoPrev", "idPersona");

                            j.ToTable("tblPersonaNMantenimientoPrev", "Assistant");
                        });
            });

            modelBuilder.Entity<tblMaquina>(entity =>
            {
                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblMaquina)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMaquina_tblLavanderia");

                entity.HasOne(d => d.idPlantillaTareaMantenimientoPrevNavigation)
                    .WithMany(p => p.tblMaquina)
                    .HasForeignKey(d => d.idPlantillaTareaMantenimientoPrev)
                    .HasConstraintName("FK_tblMaquina_tblPlantillaTareaMantenimientoPrev");

                entity.HasOne(d => d.idTipoMaquinaNCategoriaMaquinaNavigation)
                    .WithMany(p => p.tblMaquina)
                    .HasForeignKey(d => d.idTipoMaquinaNCategoriaMaquina)
                    .HasConstraintName("FK_tblMaquina_tblTipoMaquinaNCategoriaMaquina");

                entity.HasMany(d => d.idPrenda)
                    .WithMany(p => p.idMaquina)
                    .UsingEntity<Dictionary<string, object>>(
                        "tblPrendasExcluidasNMaquina",
                        l => l.HasOne<tblPrenda>().WithMany().HasForeignKey("idPrenda").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblPrendasExcluidasNMaquina_tblPrenda"),
                        r => r.HasOne<tblMaquina>().WithMany().HasForeignKey("idMaquina").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblPrendasExcluidasNMaquina_tblMaquina"),
                        j =>
                        {
                            j.HasKey("idMaquina", "idPrenda").HasName("PK__tblPrend__501E3B8B5EE934E5");

                            j.ToTable("tblPrendasExcluidasNMaquina", "Maquinaria");
                        });
            });

            modelBuilder.Entity<tblMesNAjustePresupuestario>(entity =>
            {
                entity.HasKey(e => new { e.idAjustePresupuestario, e.fecha });

                entity.HasOne(d => d.idAjustePresupuestarioNavigation)
                    .WithMany(p => p.tblMesNAjustePresupuestario)
                    .HasForeignKey(d => d.idAjustePresupuestario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMesNAjustePresupuestario_tblAjustePresupuestario");
            });

            modelBuilder.Entity<tblMezclaSucioCliente>(entity =>
            {
                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblMezclaSucioCliente)
                    .HasForeignKey(d => d.idEntidad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMezclaSucioCliente_tblEntidad");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblMezclaSucioCliente)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMezclaSucioCliente_tblLavanderia");

                entity.HasOne(d => d.idMovimientoElemLogNavigation)
                    .WithMany(p => p.tblMezclaSucioCliente)
                    .HasForeignKey(d => d.idMovimientoElemLog)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMezclaSucioCliente_tblMovimientoElemLog");
            });

            modelBuilder.Entity<tblModulo>(entity =>
            {
                entity.HasMany(d => d.idLavanderia)
                    .WithMany(p => p.idModulo)
                    .UsingEntity<Dictionary<string, object>>(
                        "tblModuloNLavanderia",
                        l => l.HasOne<tblLavanderia>().WithMany().HasForeignKey("idLavanderia").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblModuloNLavanderia_tblLavanderia"),
                        r => r.HasOne<tblModulo>().WithMany().HasForeignKey("idModulo").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblModuloNLavanderia_tblModulo"),
                        j =>
                        {
                            j.HasKey("idModulo", "idLavanderia").HasName("PK__tblModul__2B2076E3E585EF60");

                            j.ToTable("tblModuloNLavanderia", "GestionInterna");
                        });
            });

            modelBuilder.Entity<tblModuloNLavanderia_Ponderacion>(entity =>
            {
                entity.HasKey(e => new { e.idLavanderia, e.idModulo });

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblModuloNLavanderia_Ponderacion)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblModuloNLavanderia_Ponderacion_tblLavanderia");

                entity.HasOne(d => d.idModuloNavigation)
                    .WithMany(p => p.tblModuloNLavanderia_Ponderacion)
                    .HasForeignKey(d => d.idModulo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblModuloNLavanderia_Ponderacion_tblModulo");
            });

            modelBuilder.Entity<tblMoneda>(entity =>
            {
                entity.Property(e => e.idMoneda).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<tblMotivoIncumplimientoJornada>(entity =>
            {
                entity.HasOne(d => d.idTraduccionNavigation)
                    .WithMany(p => p.tblMotivoIncumplimientoJornada)
                    .HasForeignKey(d => d.idTraduccion)
                    .HasConstraintName("FK_tblMotivoIncumplimientoJornada_tblTraduccion");
            });

            modelBuilder.Entity<tblMotivoPausa>(entity =>
            {
                entity.Property(e => e.idMotivoPausa).ValueGeneratedNever();
            });

            modelBuilder.Entity<tblMovimiento>(entity =>
            {
                entity.Property(e => e.idTipoMovimiento).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.idCompañiaNavigation)
                    .WithMany(p => p.tblMovimiento)
                    .HasForeignKey(d => d.idCompañia)
                    .HasConstraintName("FK_tblMovimiento_tblCompañia");

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblMovimiento)
                    .HasForeignKey(d => d.idEntidad)
                    .HasConstraintName("FK_tblMovimiento_tblEntidad");

                entity.HasOne(d => d.idGrupoPlantillaPrenda_genericaNavigation)
                    .WithMany(p => p.tblMovimiento)
                    .HasForeignKey(d => d.idGrupoPlantillaPrenda_generica)
                    .HasConstraintName("FK_tblMovimiento_tblGrupoPlantillaPrenda_generica");

                entity.HasOne(d => d.idTipoMovimientoNavigation)
                    .WithMany(p => p.tblMovimiento)
                    .HasForeignKey(d => d.idTipoMovimiento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMovimiento_tblTipoMovimiento");

                entity.HasOne(d => d.idTipoRetiroNavigation)
                    .WithMany(p => p.tblMovimiento)
                    .HasForeignKey(d => d.idTipoRetiro)
                    .HasConstraintName("FK__tblMovimi__idTip__3EA8E074");
            });

            modelBuilder.Entity<tblMovimientoElemLog>(entity =>
            {
                entity.HasKey(e => e.idMovimientoElemLog)
                    .HasName("PK_tblMovimientoElemlog");

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblMovimientoElemLog)
                    .HasForeignKey(d => d.idEntidad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMovimientoElemlog_tblEntidad");

                entity.HasOne(d => d.idEstadoMovimientoElemLogNavigation)
                    .WithMany(p => p.tblMovimientoElemLog)
                    .HasForeignKey(d => d.idEstadoMovimientoElemLog)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMovimientoElemlog_tblEstadoMovimientoElemLog");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblMovimientoElemLog)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMovimientoElemlog_tblLavanderia");
            });

            modelBuilder.Entity<tblMovimientoRecambio>(entity =>
            {
                entity.HasOne(d => d.idAlmacenDestinoNavigation)
                    .WithMany(p => p.tblMovimientoRecambioidAlmacenDestinoNavigation)
                    .HasForeignKey(d => d.idAlmacenDestino)
                    .HasConstraintName("FK_tblMovimientoRecambio_tblAlmacenRecambios1");

                entity.HasOne(d => d.idAlmacenOrigenNavigation)
                    .WithMany(p => p.tblMovimientoRecambioidAlmacenOrigenNavigation)
                    .HasForeignKey(d => d.idAlmacenOrigen)
                    .HasConstraintName("FK_tblMovimientoRecambio_tblAlmacenRecambios");

                entity.HasOne(d => d.idEstadoMovimientoRecambioNavigation)
                    .WithMany(p => p.tblMovimientoRecambio)
                    .HasForeignKey(d => d.idEstadoMovimientoRecambio)
                    .HasConstraintName("FK_tblMovimientoRecambio_tblEstadoMovimientoRecambio");

                entity.HasOne(d => d.idProveedorNavigation)
                    .WithMany(p => p.tblMovimientoRecambio)
                    .HasForeignKey(d => d.idProveedor)
                    .HasConstraintName("FK_tblMovimientoRecambio_tblProveedor");

                entity.HasOne(d => d.idTipoMovimientoRecambioNavigation)
                    .WithMany(p => p.tblMovimientoRecambio)
                    .HasForeignKey(d => d.idTipoMovimientoRecambio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMovimientoRecambio_tblTipoMovimientoRecambio");
            });

            modelBuilder.Entity<tblMovimientoTag>(entity =>
            {
                entity.HasKey(e => new { e.idTag, e.fecha, e.idEstado });

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblMovimientoTag)
                    .HasForeignKey(d => d.idEntidad)
                    .HasConstraintName("FK_tblMovimientoTag_tblEntidad");

                entity.HasOne(d => d.idEstadoNavigation)
                    .WithMany(p => p.tblMovimientoTag)
                    .HasForeignKey(d => d.idEstado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMovimientoTag_tblEstadoTag");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblMovimientoTag)
                    .HasForeignKey(d => d.idLavanderia)
                    .HasConstraintName("FK_tblMovimientoTag_tblLavanderia");

                entity.HasOne(d => d.idTagNavigation)
                    .WithMany(p => p.tblMovimientoTag)
                    .HasForeignKey(d => d.idTag)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMovimientoTag_tblTag");
            });

            modelBuilder.Entity<tblMuestreo>(entity =>
            {
                entity.HasKey(e => e.idMuestreo)
                    .HasName("PK__tblMuest__B5307CD15FC097CD");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblMuestreo)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_idLavanderia");

                entity.HasOne(d => d.idTipoMuestreoNavigation)
                    .WithMany(p => p.tblMuestreo)
                    .HasForeignKey(d => d.idTipoMuestreo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_idTipoMuestreo");
            });

            modelBuilder.Entity<tblNivelCombustible>(entity =>
            {
                entity.HasKey(e => e.idNivelCombustible)
                    .HasName("PK__tblNivel__740DC78DB39FC3A7");

                entity.Property(e => e.idNivelCombustible).ValueGeneratedNever();
            });

            modelBuilder.Entity<tblNivelEstudios>(entity =>
            {
                entity.HasOne(d => d.idTraduccionNavigation)
                    .WithMany(p => p.tblNivelEstudios)
                    .HasForeignKey(d => d.idTraduccion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblNivelEstudios_tblTraduccion");
            });

            modelBuilder.Entity<tblNodos>(entity =>
            {
                entity.Property(e => e.col_md).HasDefaultValueSql("((6))");

                entity.Property(e => e.col_xl).HasDefaultValueSql("((2))");

                entity.Property(e => e.orden).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblNodos)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblNodos_tblLavanderia");

                entity.HasOne(d => d.idMaquinaNavigation)
                    .WithMany(p => p.tblNodos)
                    .HasForeignKey(d => d.idMaquina)
                    .HasConstraintName("FK_tblNodos_tblMaquinaria");
            });

            modelBuilder.Entity<tblNomina>(entity =>
            {
                entity.HasKey(e => e.idNomina)
                    .HasName("PK__tblNomin__BB6DB6739861BF3D");

                entity.Property(e => e.idTipoNomina).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.idAdmCentroCosteNavigation)
                    .WithMany(p => p.tblNomina)
                    .HasForeignKey(d => d.idAdmCentroCoste)
                    .HasConstraintName("FK_tblNomina_tblAdmCentroCoste");

                entity.HasOne(d => d.idAdmCuentaContable_SSEmpresaNavigation)
                    .WithMany(p => p.tblNominaidAdmCuentaContable_SSEmpresaNavigation)
                    .HasForeignKey(d => d.idAdmCuentaContable_SSEmpresa)
                    .HasConstraintName("FK_tblNomina_tblAdmCuentaContable2");

                entity.HasOne(d => d.idAdmCuentaContable_SalarioNavigation)
                    .WithMany(p => p.tblNominaidAdmCuentaContable_SalarioNavigation)
                    .HasForeignKey(d => d.idAdmCuentaContable_Salario)
                    .HasConstraintName("FK_tblNomina_tblAdmCuentaContable1");

                entity.HasOne(d => d.idAdmElementoPEPNavigation)
                    .WithMany(p => p.tblNomina)
                    .HasForeignKey(d => d.idAdmElementoPEP)
                    .HasConstraintName("FK_tblNomina_tblAdmElementoPEP");

                entity.HasOne(d => d.idEmpresaPolarierNavigation)
                    .WithMany(p => p.tblNomina)
                    .HasForeignKey(d => d.idEmpresaPolarier)
                    .HasConstraintName("FK_tblNomina_tblEmpresasPolarier");

                entity.HasOne(d => d.idEstadoHistoricoAsientoNominaNavigation)
                    .WithMany(p => p.tblNomina)
                    .HasForeignKey(d => d.idEstadoHistoricoAsientoNomina)
                    .HasConstraintName("FK_tblNomina_tblEstadoHistoricoAsientoNomina");

                entity.HasOne(d => d.idEstadoNominaNavigation)
                    .WithMany(p => p.tblNomina)
                    .HasForeignKey(d => d.idEstadoNomina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblNomina_tblEstadoNomina");

                entity.HasOne(d => d.idMotivoBajaNavigation)
                    .WithMany(p => p.tblNomina)
                    .HasForeignKey(d => d.idMotivoBaja)
                    .HasConstraintName("FK_tblNomina_tblMotivoBaja");

                entity.HasOne(d => d.idPersonaNavigation)
                    .WithMany(p => p.tblNomina)
                    .HasForeignKey(d => d.idPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblNomina_tblPersona");

                entity.HasOne(d => d.idTipoContratoNavigation)
                    .WithMany(p => p.tblNomina)
                    .HasForeignKey(d => d.idTipoContrato)
                    .HasConstraintName("FK_tblNomina_tblTipoContrato");

                entity.HasOne(d => d.idTipoNominaNavigation)
                    .WithMany(p => p.tblNomina)
                    .HasForeignKey(d => d.idTipoNomina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblNomina_tblTipoNomina");

                entity.HasOne(d => d.idTipoTrabajoNavigation)
                    .WithMany(p => p.tblNomina)
                    .HasForeignKey(d => d.idTipoTrabajo)
                    .HasConstraintName("FK_tblNomina_tblTipoTrabajo");

                entity.HasOne(d => d.idUsuario_modificaNavigation)
                    .WithMany(p => p.tblNomina)
                    .HasForeignKey(d => d.idUsuario_modifica)
                    .HasConstraintName("FK_tblNomina_tblUsuario");
            });

            modelBuilder.Entity<tblNomina_MX>(entity =>
            {
                entity.HasOne(d => d.idAdmCentroCosteNavigation)
                    .WithMany(p => p.tblNomina_MX)
                    .HasForeignKey(d => d.idAdmCentroCoste)
                    .HasConstraintName("FK_tblNomina_MX_tblAdmCentroCoste");

                entity.HasOne(d => d.idAdmCuentaContable_IMSS_MXNavigation)
                    .WithMany(p => p.tblNomina_MXidAdmCuentaContable_IMSS_MXNavigation)
                    .HasForeignKey(d => d.idAdmCuentaContable_IMSS_MX)
                    .HasConstraintName("FK_tblNomina_MX_tblAdmCuentaContable_IMSS_MX");

                entity.HasOne(d => d.idAdmCuentaContable_INFONAVIT_MXNavigation)
                    .WithMany(p => p.tblNomina_MXidAdmCuentaContable_INFONAVIT_MXNavigation)
                    .HasForeignKey(d => d.idAdmCuentaContable_INFONAVIT_MX)
                    .HasConstraintName("FK_tblNomina_MX_tblAdmCuentaContable_INFONAVIT_MX");

                entity.HasOne(d => d.idAdmCuentaContable_ImpEstatalNominas_MXNavigation)
                    .WithMany(p => p.tblNomina_MXidAdmCuentaContable_ImpEstatalNominas_MXNavigation)
                    .HasForeignKey(d => d.idAdmCuentaContable_ImpEstatalNominas_MX)
                    .HasConstraintName("FK_tblNomina_MX_tblAdmCuentaContable_ImpEstatalNominas_MX");

                entity.HasOne(d => d.idAdmCuentaContable_SAR_MXNavigation)
                    .WithMany(p => p.tblNomina_MXidAdmCuentaContable_SAR_MXNavigation)
                    .HasForeignKey(d => d.idAdmCuentaContable_SAR_MX)
                    .HasConstraintName("FK_tblNomina_MX_tblAdmCuentaContable_SAR_MX");

                entity.HasOne(d => d.idAdmCuentaContable_Sueldo_MXNavigation)
                    .WithMany(p => p.tblNomina_MXidAdmCuentaContable_Sueldo_MXNavigation)
                    .HasForeignKey(d => d.idAdmCuentaContable_Sueldo_MX)
                    .HasConstraintName("FK_tblNomina_MX_tblAdmCuentaContable_Sueldo_MX");

                entity.HasOne(d => d.idAdmElementoPEPNavigation)
                    .WithMany(p => p.tblNomina_MX)
                    .HasForeignKey(d => d.idAdmElementoPEP)
                    .HasConstraintName("FK_tblNomina_MX_tblAdmElementoPEP");

                entity.HasOne(d => d.idPersonaNavigation)
                    .WithMany(p => p.tblNomina_MX)
                    .HasForeignKey(d => d.idPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblNomina_MX_tblPersona");

                entity.HasOne(d => d.idTipoNomina_MXNavigation)
                    .WithMany(p => p.tblNomina_MX)
                    .HasForeignKey(d => d.idTipoNomina_MX)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblNomina_MX_tblTipoNomina_MX");

                entity.HasOne(d => d.idTipoTrabajoNavigation)
                    .WithMany(p => p.tblNomina_MX)
                    .HasForeignKey(d => d.idTipoTrabajo)
                    .HasConstraintName("FK_tblNomina_MX_tblTipoTrabajo");
            });

            modelBuilder.Entity<tblNomina_RD>(entity =>
            {
                entity.HasKey(e => e.idNomina_RD)
                    .HasName("PK__tblNomin__83CF6E3B9D00D1F8");

                entity.Property(e => e.contabilizado).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.idAdmCentroCosteNavigation)
                    .WithMany(p => p.tblNomina_RD)
                    .HasForeignKey(d => d.idAdmCentroCoste)
                    .HasConstraintName("FK__tblNomina__idAdm__7F978E31");

                entity.HasOne(d => d.idAdmCuentaContableNavigation)
                    .WithMany(p => p.tblNomina_RD)
                    .HasForeignKey(d => d.idAdmCuentaContable)
                    .HasConstraintName("FK__tblNomina__idAdm__0273FADC");

                entity.HasOne(d => d.idAdmElementoPEPNavigation)
                    .WithMany(p => p.tblNomina_RD)
                    .HasForeignKey(d => d.idAdmElementoPEP)
                    .HasConstraintName("FK__tblNomina__idAdm__008BB26A");

                entity.HasOne(d => d.idPersonaNavigation)
                    .WithMany(p => p.tblNomina_RD)
                    .HasForeignKey(d => d.idPersona)
                    .HasConstraintName("FK__tblNomina__idPer__7EA369F8");

                entity.HasOne(d => d.idTipoNomina_RDNavigation)
                    .WithMany(p => p.tblNomina_RD)
                    .HasForeignKey(d => d.idTipoNomina_RD)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblNomina_RD_tblTipoNomina_RD");
            });

            modelBuilder.Entity<tblNotificacion>(entity =>
            {
                entity.HasKey(e => e.idNotificacion)
                    .HasName("PK__tblNotif__AFE1D7E4B6A11009");

                entity.Property(e => e.fechaEnvio).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.HasOne(d => d.idNotificacion_EstadoNavigation)
                    .WithMany(p => p.tblNotificacion)
                    .HasForeignKey(d => d.idNotificacion_Estado)
                    .HasConstraintName("FK__tblNotifi__idNot__48C67C34");

                entity.HasOne(d => d.idUsuarioNavigation)
                    .WithMany(p => p.tblNotificacion)
                    .HasForeignKey(d => d.idUsuario)
                    .HasConstraintName("FK__tblNotifi__idUsu__46DE33C2");
            });

            modelBuilder.Entity<tblNotificacion_Estado>(entity =>
            {
                entity.HasKey(e => e.idNotificacion_Estado)
                    .HasName("PK__tblNotif__7CEE2A9727963EF0");

                entity.Property(e => e.idNotificacion_Estado).ValueGeneratedNever();
            });

            modelBuilder.Entity<tblNotificacion_Evento>(entity =>
            {
                entity.HasKey(e => e.idNotificacion_Evento)
                    .HasName("PK__tblNotif__A5205F4354123890");

                entity.Property(e => e.fecha).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.HasOne(d => d.idNotificacionNavigation)
                    .WithMany(p => p.tblNotificacion_Evento)
                    .HasForeignKey(d => d.idNotificacion)
                    .HasConstraintName("FK__tblNotifi__idNot__4BA2E8DF");

                entity.HasOne(d => d.idNotificacion_EstadoNavigation)
                    .WithMany(p => p.tblNotificacion_Evento)
                    .HasForeignKey(d => d.idNotificacion_Estado)
                    .HasConstraintName("FK__tblNotifi__idNot__4D8B3151");
            });

            modelBuilder.Entity<tblNotificaciones_TI>(entity =>
            {
                entity.HasOne(d => d.idUsuarioResponsableNavigation)
                    .WithMany(p => p.tblNotificaciones_TI)
                    .HasForeignKey(d => d.idUsuarioResponsable)
                    .HasConstraintName("FK_tblNotificaciones_TI_tblUsuario");
            });

            modelBuilder.Entity<tblObjetivosKpi>(entity =>
            {
                entity.HasKey(e => new { e.idLavanderia, e.idTipoKpi, e.fecha });

                entity.Property(e => e.kpiOperativo).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblObjetivosKpi)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblObjetivosKpi_tblLavanderia");

                entity.HasOne(d => d.idTipoKpiNavigation)
                    .WithMany(p => p.tblObjetivosKpi)
                    .HasForeignKey(d => d.idTipoKpi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblObjetivosKpi_tblTipoKpi");
            });

            modelBuilder.Entity<tblObjetivosKpiNRecursoNivel>(entity =>
            {
                entity.HasKey(e => new { e.idLavanderia, e.fecha, e.idRecursoNivel });

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblObjetivosKpiNRecursoNivel)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblObjetivosKpiNRecursoNivel_tblLavanderia");

                entity.HasOne(d => d.idRecursoNivelNavigation)
                    .WithMany(p => p.tblObjetivosKpiNRecursoNivel)
                    .HasForeignKey(d => d.idRecursoNivel)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblObjetivosKpiNRecursoNivel_tblRecursoNivel");
            });

            modelBuilder.Entity<tblOpcion>(entity =>
            {
                entity.HasMany(d => d.idPregunta)
                    .WithMany(p => p.idOpcion)
                    .UsingEntity<Dictionary<string, object>>(
                        "tblOpcionNPregunta",
                        l => l.HasOne<tblPregunta>().WithMany().HasForeignKey("idPregunta").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblOpcionNPregunta_tblPregunta"),
                        r => r.HasOne<tblOpcion>().WithMany().HasForeignKey("idOpcion").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblOpcionNPregunta_tblOpcion"),
                        j =>
                        {
                            j.HasKey("idOpcion", "idPregunta");

                            j.ToTable("tblOpcionNPregunta", "ControlCalidad");
                        });
            });

            modelBuilder.Entity<tblPackingList>(entity =>
            {
                entity.HasOne(d => d.idEnvioNavigation)
                    .WithMany(p => p.tblPackingList)
                    .HasForeignKey(d => d.idEnvio)
                    .HasConstraintName("FK_tblPackingList_tblEnvio");
            });

            modelBuilder.Entity<tblPais>(entity =>
            {
                entity.Property(e => e.idPais).ValueGeneratedNever();

                entity.HasOne(d => d.idMonedaNavigation)
                    .WithMany(p => p.tblPais)
                    .HasForeignKey(d => d.idMoneda)
                    .HasConstraintName("FK_tblPais_tblMoneda");

                entity.HasOne(d => d.idTraduccionNavigation)
                    .WithMany(p => p.tblPais)
                    .HasForeignKey(d => d.idTraduccion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPais_tblTraduccion");
            });

            modelBuilder.Entity<tblParadaNParteTransporte>(entity =>
            {
                entity.HasKey(e => e.idParadaNParteTransporte)
                    .HasName("PK_tblParadaNRutaExpedicionNParteTransporte");

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblParadaNParteTransporte)
                    .HasForeignKey(d => d.idEntidad)
                    .HasConstraintName("FK_tblParadaNRutaExpedicionNParteTransporte_tblEntidad");

                entity.HasOne(d => d.idIncidenciaNavigation)
                    .WithMany(p => p.tblParadaNParteTransporte)
                    .HasForeignKey(d => d.idIncidencia)
                    .HasConstraintName("FK_tblParadaNRutaExpedicionNParteTransporte_tblIncidencia");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblParadaNParteTransporte)
                    .HasForeignKey(d => d.idLavanderia)
                    .HasConstraintName("FK_tblParadaNRutaExpedicionNParteTransporte_tblLavanderia");

                entity.HasOne(d => d.idMotivoPausaNavigation)
                    .WithMany(p => p.tblParadaNParteTransporte)
                    .HasForeignKey(d => d.idMotivoPausa)
                    .HasConstraintName("FK_tblParadaNParteTransporte_tblMotivoPausa");

                entity.HasOne(d => d.idParteTransporteNavigation)
                    .WithMany(p => p.tblParadaNParteTransporte)
                    .HasForeignKey(d => d.idParteTransporte)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblParadaNRutaExpedicionNParteTransporte_tblParteTransporte");
            });

            modelBuilder.Entity<tblParadaNRutaExpedicion>(entity =>
            {
                entity.Property(e => e.activo).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblParadaNRutaExpedicion)
                    .HasForeignKey(d => d.idEntidad)
                    .HasConstraintName("FK_tblParadaNRutaExpedicion_tblEntidad");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblParadaNRutaExpedicion)
                    .HasForeignKey(d => d.idLavanderia)
                    .HasConstraintName("FK_tblParadaNRutaExpedicion_tblLavanderia");

                entity.HasOne(d => d.idRutaExpedicionNavigation)
                    .WithMany(p => p.tblParadaNRutaExpedicion)
                    .HasForeignKey(d => d.idRutaExpedicion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblParadaNRutaExpedicion_tblRutaExpedicion");
            });

            modelBuilder.Entity<tblParamNConfigAlbaranReparto>(entity =>
            {
                entity.HasOne(d => d.idConfigAlbaranRepartoNavigation)
                    .WithMany(p => p.tblParamNConfigAlbaranReparto)
                    .HasForeignKey(d => d.idConfigAlbaranReparto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblParamNConfigAlbaranReparto_tblConfigAlbaranReparto");

                entity.HasOne(d => d.idParametroNavigation)
                    .WithMany(p => p.tblParamNConfigAlbaranReparto)
                    .HasForeignKey(d => d.idParametro)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblParamNConfigAlbaranReparto_tblParametrosAlbaranReparto");
            });

            modelBuilder.Entity<tblParametrosAlbaranReparto>(entity =>
            {
                entity.Property(e => e.idParametro).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<tblParteTrabajo>(entity =>
            {
                entity.Property(e => e.isApp).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblParteTrabajo)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblParteTrabajo_tblLavanderia");

                entity.HasOne(d => d.idMaquinaNavigation)
                    .WithMany(p => p.tblParteTrabajo)
                    .HasForeignKey(d => d.idMaquina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblParteTrabajo_tblMaquina");

                entity.HasOne(d => d.idUsuarioCreaNavigation)
                    .WithMany(p => p.tblParteTrabajo)
                    .HasForeignKey(d => d.idUsuarioCrea)
                    .HasConstraintName("FK_tblParteTrabajo_tblUsuario");
            });

            modelBuilder.Entity<tblParteTransporte>(entity =>
            {
                entity.HasKey(e => e.idParteTransporte)
                    .HasName("PK__tblParte__A9C3C9C9979EDD00");

                entity.HasOne(d => d.idEstadoNavigation)
                    .WithMany(p => p.tblParteTransporte)
                    .HasForeignKey(d => d.idEstado)
                    .HasConstraintName("FK_tblParteTransporte_tblParteTransporte_Estado");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblParteTransporte)
                    .HasForeignKey(d => d.idLavanderia)
                    .HasConstraintName("FK_tblParteTransporte_tblLavanderia");

                entity.HasOne(d => d.idNivelCombustibleNavigation)
                    .WithMany(p => p.tblParteTransporte)
                    .HasForeignKey(d => d.idNivelCombustible)
                    .HasConstraintName("FK_ParteTransporte_idNivelCombustible");

                entity.HasOne(d => d.idPersonaResponsableNavigation)
                    .WithMany(p => p.tblParteTransporte)
                    .HasForeignKey(d => d.idPersonaResponsable)
                    .HasConstraintName("FK_ParteTransporte_idPersona");

                entity.HasOne(d => d.idRutaExpedicionNavigation)
                    .WithMany(p => p.tblParteTransporte)
                    .HasForeignKey(d => d.idRutaExpedicion)
                    .HasConstraintName("FK_tblParteTransporte_tblRutaExpedicion");

                entity.HasOne(d => d.idUsuarioResponsableNavigation)
                    .WithMany(p => p.tblParteTransporte)
                    .HasForeignKey(d => d.idUsuarioResponsable)
                    .HasConstraintName("FK_tblParteTransporte_tblUsuario");

                entity.HasOne(d => d.idVehiculoNavigation)
                    .WithMany(p => p.tblParteTransporte)
                    .HasForeignKey(d => d.idVehiculo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ParteTransporte_idVehiculo");

                entity.HasMany(d => d.idPersonaTransportista)
                    .WithMany(p => p.idParteTransporte)
                    .UsingEntity<Dictionary<string, object>>(
                        "tblTransportistaNParteTransporte",
                        l => l.HasOne<tblPersona>().WithMany().HasForeignKey("idPersonaTransportista").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_TransportistaNParteTransporte_idPersona"),
                        r => r.HasOne<tblParteTransporte>().WithMany().HasForeignKey("idParteTransporte").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_TransportistaNParteTransporte_idParteTransporte"),
                        j =>
                        {
                            j.HasKey("idParteTransporte", "idPersonaTransportista").HasName("PK__tblTrans__98882A5E656490FF");

                            j.ToTable("tblTransportistaNParteTransporte", "Logistica");
                        });
            });

            modelBuilder.Entity<tblParteTransporte_Localizacion>(entity =>
            {
                entity.HasKey(e => new { e.idParteTransporte, e.fecha });

                entity.HasOne(d => d.idParadaNParteTransporteNavigation)
                    .WithMany(p => p.tblParteTransporte_Localizacion)
                    .HasForeignKey(d => d.idParadaNParteTransporte)
                    .HasConstraintName("FK_tblParteTransporte_Localizacion_tblParadaNParteTransporte");

                entity.HasOne(d => d.idParteTransporteNavigation)
                    .WithMany(p => p.tblParteTransporte_Localizacion)
                    .HasForeignKey(d => d.idParteTransporte)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblParteTransporte_Localizacion_tblParteTransporte");
            });

            modelBuilder.Entity<tblParticipantesNReunion>(entity =>
            {
                entity.HasOne(d => d.idReunionNavigation)
                    .WithMany(p => p.tblParticipantesNReunion)
                    .HasForeignKey(d => d.idReunion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblParticipantesNReunion_tblReunion");
            });

            modelBuilder.Entity<tblPartidaNCierrePresupuestario>(entity =>
            {
                entity.HasOne(d => d.idAdmCuentaContableNavigation)
                    .WithMany(p => p.tblPartidaNCierrePresupuestario)
                    .HasForeignKey(d => d.idAdmCuentaContable)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPartidaNCierrePresupuestario_tblAdmCuentaContable");

                entity.HasOne(d => d.idCierrePresupuestarioNavigation)
                    .WithMany(p => p.tblPartidaNCierrePresupuestario)
                    .HasForeignKey(d => d.idCierrePresupuestario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPartidaNCierrePresupuestario_tblCierrePresupuestario");

                entity.HasOne(d => d.idMonedaNavigation)
                    .WithMany(p => p.tblPartidaNCierrePresupuestario)
                    .HasForeignKey(d => d.idMoneda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPartidaNCierrePresupuestario_tblMoneda");
            });

            modelBuilder.Entity<tblPedido>(entity =>
            {
                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblPedido)
                    .HasForeignKey(d => d.idEntidad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPedido_tblEntidad");

                entity.HasOne(d => d.idEstadoPedidoNavigation)
                    .WithMany(p => p.tblPedido)
                    .HasForeignKey(d => d.idEstadoPedido)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPedido_tblEstadoPedido");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblPedido)
                    .HasForeignKey(d => d.idLavanderia)
                    .HasConstraintName("FK_tblPedido_tblLavanderia");

                entity.HasOne(d => d.idTipoPedidoNavigation)
                    .WithMany(p => p.tblPedido)
                    .HasForeignKey(d => d.idTipoPedido)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPedido_tblTipoPedido");

                entity.HasOne(d => d.idTipoProduccionNavigation)
                    .WithMany(p => p.tblPedido)
                    .HasForeignKey(d => d.idTipoProduccion)
                    .HasConstraintName("FK_tblPedido_tblTipoProduccion");

                entity.HasOne(d => d.idUsuarioCreadorNavigation)
                    .WithMany(p => p.tblPedidoidUsuarioCreadorNavigation)
                    .HasForeignKey(d => d.idUsuarioCreador)
                    .HasConstraintName("FK_tblPedido_tblUsuario");

                entity.HasOne(d => d.idUsuarioPrepara_activoNavigation)
                    .WithMany(p => p.tblPedidoidUsuarioPrepara_activoNavigation)
                    .HasForeignKey(d => d.idUsuarioPrepara_activo)
                    .HasConstraintName("FK_tblPedido_tblUsuario_prepara_activo");
            });

            modelBuilder.Entity<tblPedidoHuesped>(entity =>
            {
                entity.HasOne(d => d.idAlmacenNavigation)
                    .WithMany(p => p.tblPedidoHuesped)
                    .HasForeignKey(d => d.idAlmacen)
                    .HasConstraintName("FK_tblPedidoHuesped_tblAlmacen");

                entity.HasOne(d => d.idTipoServicioNavigation)
                    .WithMany(p => p.tblPedidoHuesped)
                    .HasForeignKey(d => d.idTipoServicio)
                    .HasConstraintName("FK_tblPedidoHuesped_tblTipoServicio");
            });

            modelBuilder.Entity<tblPedidosExtra>(entity =>
            {
                entity.HasOne(d => d.idAlmacenNavigation)
                    .WithMany(p => p.tblPedidosExtra)
                    .HasForeignKey(d => d.idAlmacen)
                    .HasConstraintName("FK_tblPedidosExtra_tblAlmacen");

                entity.HasOne(d => d.idEstadoOfficeNavigation)
                    .WithMany(p => p.tblPedidosExtra)
                    .HasForeignKey(d => d.idEstadoOffice)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPedidosExtra_tblEstadoOffice");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblPedidosExtra)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPedidosExtra_tblLavanderia");

                entity.HasOne(d => d.idPersonaNavigation)
                    .WithMany(p => p.tblPedidosExtra)
                    .HasForeignKey(d => d.idPersona)
                    .HasConstraintName("FK_tblPedidosExtra_tblPersona");

                entity.HasOne(d => d.idSubAlmacenNavigation)
                    .WithMany(p => p.tblPedidosExtra)
                    .HasForeignKey(d => d.idSubAlmacen)
                    .HasConstraintName("FK_tblPedidosExtra_tblSubAlmacen");
            });

            modelBuilder.Entity<tblPermiso>(entity =>
            {
                entity.HasOne(d => d.idTraduccionNavigation)
                    .WithMany(p => p.tblPermiso)
                    .HasForeignKey(d => d.idTraduccion)
                    .HasConstraintName("FK_tblPermiso_tblTraduccion");

                entity.HasMany(d => d.idFormulario)
                    .WithMany(p => p.idPermiso)
                    .UsingEntity<Dictionary<string, object>>(
                        "tblPermisoNFormulario",
                        l => l.HasOne<tblFormulario>().WithMany().HasForeignKey("idFormulario").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblPermisoNFormulario_tblFormulario"),
                        r => r.HasOne<tblPermiso>().WithMany().HasForeignKey("idPermiso").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblPermisoNFormulario_tblPermiso"),
                        j =>
                        {
                            j.HasKey("idPermiso", "idFormulario");

                            j.ToTable("tblPermisoNFormulario", "GestionInterna");
                        });

                entity.HasMany(d => d.idUsuario)
                    .WithMany(p => p.idPermiso)
                    .UsingEntity<Dictionary<string, object>>(
                        "tblPermisoNUsuario",
                        l => l.HasOne<tblUsuario>().WithMany().HasForeignKey("idUsuario").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblPermisoNUsuario_tblUsuario"),
                        r => r.HasOne<tblPermiso>().WithMany().HasForeignKey("idPermiso").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblPermisoNUsuario_tblPermiso"),
                        j =>
                        {
                            j.HasKey("idPermiso", "idUsuario");

                            j.ToTable("tblPermisoNUsuario", "GestionInterna");
                        });
            });

            modelBuilder.Entity<tblPersona>(entity =>
            {
                entity.Property(e => e.isIBANSolicitado).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.idAdmCentroCosteNavigation)
                    .WithMany(p => p.tblPersona)
                    .HasForeignKey(d => d.idAdmCentroCoste)
                    .HasConstraintName("FK_tblPersona_tblAdmCentroCoste");

                entity.HasOne(d => d.idAdmCuentaContable_SSEmpresaNavigation)
                    .WithMany(p => p.tblPersonaidAdmCuentaContable_SSEmpresaNavigation)
                    .HasForeignKey(d => d.idAdmCuentaContable_SSEmpresa)
                    .HasConstraintName("FK_tblPersona_tblAdmCuentaContable1");

                entity.HasOne(d => d.idAdmCuentaContable_SalarioNavigation)
                    .WithMany(p => p.tblPersonaidAdmCuentaContable_SalarioNavigation)
                    .HasForeignKey(d => d.idAdmCuentaContable_Salario)
                    .HasConstraintName("FK_tblPersona_tblAdmCuentaContable");

                entity.HasOne(d => d.idAdmElementoPEPNavigation)
                    .WithMany(p => p.tblPersona)
                    .HasForeignKey(d => d.idAdmElementoPEP)
                    .HasConstraintName("FK_tblPersona_tblAdmElementoPEP");

                entity.HasOne(d => d.idCategoriaNavigation)
                    .WithMany(p => p.tblPersona)
                    .HasForeignKey(d => d.idCategoria)
                    .HasConstraintName("FK_tblPersona_tblCategoria");

                entity.HasOne(d => d.idCategoriaInternaNavigation)
                    .WithMany(p => p.tblPersona)
                    .HasForeignKey(d => d.idCategoriaInterna)
                    .HasConstraintName("FK_tblPersona_tblCategoriaSalarial");

                entity.HasOne(d => d.idCentroTrabajoNavigation)
                    .WithMany(p => p.tblPersona)
                    .HasForeignKey(d => d.idCentroTrabajo)
                    .HasConstraintName("FK_tblPersona_tblCentroTrabajo");

                entity.HasOne(d => d.idComunidadAutonomaNavigation)
                    .WithMany(p => p.tblPersona)
                    .HasForeignKey(d => d.idComunidadAutonoma)
                    .HasConstraintName("FK_tblPersona_tblComunidadAutonoma");

                entity.HasOne(d => d.idDiscapacidadNavigation)
                    .WithMany(p => p.tblPersona)
                    .HasForeignKey(d => d.idDiscapacidad)
                    .HasConstraintName("FK_tblPersona_tblDiscapacidad");

                entity.HasOne(d => d.idDocCertificadoDiscapacidadNavigation)
                    .WithMany(p => p.tblPersonaidDocCertificadoDiscapacidadNavigation)
                    .HasForeignKey(d => d.idDocCertificadoDiscapacidad)
                    .HasConstraintName("FK_tblPersona_tblDocumento_Discapacidad");

                entity.HasOne(d => d.idDocumentoLicenciaConducirNavigation)
                    .WithMany(p => p.tblPersonaidDocumentoLicenciaConducirNavigation)
                    .HasForeignKey(d => d.idDocumentoLicenciaConducir)
                    .HasConstraintName("FK_tblPersona_tblDocumento_LicenciaConducir");

                entity.HasOne(d => d.idEmpresaPolarierNavigation)
                    .WithMany(p => p.tblPersona)
                    .HasForeignKey(d => d.idEmpresaPolarier)
                    .HasConstraintName("FK_tblPersona_tblEmpresasPolarier");

                entity.HasOne(d => d.idEstadoCivilNavigation)
                    .WithMany(p => p.tblPersona)
                    .HasForeignKey(d => d.idEstadoCivil)
                    .HasConstraintName("FK_tblPersona_tblEstadoCivil");

                entity.HasOne(d => d.idFormatoDiasLibresNavigation)
                    .WithMany(p => p.tblPersona)
                    .HasForeignKey(d => d.idFormatoDiasLibres)
                    .HasConstraintName("FK__tblPerson__idFor__1D1D0420");

                entity.HasOne(d => d.idFotoDemandanteEmpleoNavigation)
                    .WithMany(p => p.tblPersonaidFotoDemandanteEmpleoNavigation)
                    .HasForeignKey(d => d.idFotoDemandanteEmpleo)
                    .HasConstraintName("FK_tblPersona_tblDocumento");

                entity.HasOne(d => d.idFotoDocumentoIdentidad_ANavigation)
                    .WithMany(p => p.tblPersonaidFotoDocumentoIdentidad_ANavigation)
                    .HasForeignKey(d => d.idFotoDocumentoIdentidad_A)
                    .HasConstraintName("FK_tblPersona_tblDocumento_IdentidadA");

                entity.HasOne(d => d.idFotoDocumentoIdentidad_BNavigation)
                    .WithMany(p => p.tblPersonaidFotoDocumentoIdentidad_BNavigation)
                    .HasForeignKey(d => d.idFotoDocumentoIdentidad_B)
                    .HasConstraintName("FK_tblPersona_tblDocumento_IdentidadB");

                entity.HasOne(d => d.idFotoIBANNavigation)
                    .WithMany(p => p.tblPersonaidFotoIBANNavigation)
                    .HasForeignKey(d => d.idFotoIBAN)
                    .HasConstraintName("FK_tblPersona_tblDocumento_IBAN");

                entity.HasOne(d => d.idFotoNAFNavigation)
                    .WithMany(p => p.tblPersonaidFotoNAFNavigation)
                    .HasForeignKey(d => d.idFotoNAF)
                    .HasConstraintName("FK_tblPersona_tblDocumento_NAF");

                entity.HasOne(d => d.idFotoPerfilNavigation)
                    .WithMany(p => p.tblPersonaidFotoPerfilNavigation)
                    .HasForeignKey(d => d.idFotoPerfil)
                    .HasConstraintName("FK_tblPersona_tblDocumento_Perfil");

                entity.HasOne(d => d.idGeneroNavigation)
                    .WithMany(p => p.tblPersona)
                    .HasForeignKey(d => d.idGenero)
                    .HasConstraintName("FK_tblPersona_tblGenero");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblPersona)
                    .HasForeignKey(d => d.idLavanderia)
                    .HasConstraintName("FK_tblPersona_tblLavanderia");

                entity.HasOne(d => d.idNivelEstudiosNavigation)
                    .WithMany(p => p.tblPersona)
                    .HasForeignKey(d => d.idNivelEstudios)
                    .HasConstraintName("FK_tblPersona_tblNivelEstudios");

                entity.HasOne(d => d.idPaisNavigation)
                    .WithMany(p => p.tblPersona)
                    .HasForeignKey(d => d.idPais)
                    .HasConstraintName("FK_tblPersona_tblPais");

                entity.HasOne(d => d.idTallaAlfa_CamisetaNavigation)
                    .WithMany(p => p.tblPersona)
                    .HasForeignKey(d => d.idTallaAlfa_Camiseta)
                    .HasConstraintName("FK_tblPersona_tblTallaAlfa");

                entity.HasOne(d => d.idTipoDocumentoIdentidadNavigation)
                    .WithMany(p => p.tblPersonaidTipoDocumentoIdentidadNavigation)
                    .HasForeignKey(d => d.idTipoDocumentoIdentidad)
                    .HasConstraintName("FK_tblPersona_tblTipoDocumentoIdentidad");

                entity.HasOne(d => d.idTipoDocumentoIdentidad_tutorNavigation)
                    .WithMany(p => p.tblPersonaidTipoDocumentoIdentidad_tutorNavigation)
                    .HasForeignKey(d => d.idTipoDocumentoIdentidad_tutor)
                    .HasConstraintName("FK_tblPersona_tblTipoDocumentoIdentidad1");

                entity.HasOne(d => d.idTipoTrabajoNavigation)
                    .WithMany(p => p.tblPersona)
                    .HasForeignKey(d => d.idTipoTrabajo)
                    .HasConstraintName("FK_tblPersona_tblTipoTrabajo");

                entity.HasOne(d => d.idTurnoNavigation)
                    .WithMany(p => p.tblPersona)
                    .HasForeignKey(d => d.idTurno)
                    .HasConstraintName("FK_tblPersona_tblTurno");

                entity.HasOne(d => d.idUsuario_validacion_nominaNavigation)
                    .WithMany(p => p.tblPersona)
                    .HasForeignKey(d => d.idUsuario_validacion_nomina)
                    .HasConstraintName("FK_tblPersona_tblUsuario_validacion_nomina");
            });

            modelBuilder.Entity<tblPersonaContactoNProveedor>(entity =>
            {
                entity.HasOne(d => d.idProveedorNavigation)
                    .WithMany(p => p.tblPersonaContactoNProveedor)
                    .HasForeignKey(d => d.idProveedor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPersonaContactoNProveedor_tblProveedor");
            });

            modelBuilder.Entity<tblPersonaCoste>(entity =>
            {
                entity.HasKey(e => new { e.idPersona, e.fecha });

                entity.HasOne(d => d.idPersonaNavigation)
                    .WithMany(p => p.tblPersonaCoste)
                    .HasForeignKey(d => d.idPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPersonaCoste_tblPersona");
            });

            modelBuilder.Entity<tblPersonaNAreaNLavanderia>(entity =>
            {
                entity.HasOne(d => d.idAreaLavanderiaNavigation)
                    .WithMany(p => p.tblPersonaNAreaNLavanderia)
                    .HasForeignKey(d => d.idAreaLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPersonaNAreaNLavanderia_tblAreaLavanderia");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblPersonaNAreaNLavanderia)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPersonaNAreaNLavanderia_tblLavanderia");

                entity.HasOne(d => d.idPersonaNavigation)
                    .WithMany(p => p.tblPersonaNAreaNLavanderia)
                    .HasForeignKey(d => d.idPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPersonaNAreaNLavanderia_tblPersona");
            });

            modelBuilder.Entity<tblPersonaNMaquina>(entity =>
            {
                entity.HasOne(d => d.idMaquinaNavigation)
                    .WithMany(p => p.tblPersonaNMaquina)
                    .HasForeignKey(d => d.idMaquina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPersonaNMaquina_tblMaquina");

                entity.HasOne(d => d.idPersonaNavigation)
                    .WithMany(p => p.tblPersonaNMaquina)
                    .HasForeignKey(d => d.idPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPersonaNMaquina_tblPersona");
            });

            modelBuilder.Entity<tblPersonaNTipoContrato>(entity =>
            {
                entity.HasKey(e => new { e.idPersona, e.fechaAltaContrato });

                entity.HasOne(d => d.idMotivoBajaNavigation)
                    .WithMany(p => p.tblPersonaNTipoContrato)
                    .HasForeignKey(d => d.idMotivoBaja)
                    .HasConstraintName("FK_tblPersonaNTipoContrato_tblMotivoBaja");

                entity.HasOne(d => d.idPersonaNavigation)
                    .WithMany(p => p.tblPersonaNTipoContrato)
                    .HasForeignKey(d => d.idPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPersonaNTipoContrato_tblPersona");

                entity.HasOne(d => d.idTipoContratoNavigation)
                    .WithMany(p => p.tblPersonaNTipoContrato)
                    .HasForeignKey(d => d.idTipoContrato)
                    .HasConstraintName("FK_tblPersonaNTipoContrato_tblTipoContrato");
            });

            modelBuilder.Entity<tblPersona_PeticionCambioDatos>(entity =>
            {
                entity.HasOne(d => d.idComunidadAutonomaNavigation)
                    .WithMany(p => p.tblPersona_PeticionCambioDatos)
                    .HasForeignKey(d => d.idComunidadAutonoma)
                    .HasConstraintName("FK_tblPersona_PeticionCambioDatos_tblComunidadAutonoma");

                entity.HasOne(d => d.idDiscapacidadNavigation)
                    .WithMany(p => p.tblPersona_PeticionCambioDatos)
                    .HasForeignKey(d => d.idDiscapacidad)
                    .HasConstraintName("FK_tblPersona_PeticionCambioDatos_tblDiscapacidad");

                entity.HasOne(d => d.idDocCertificadoDiscapacidadNavigation)
                    .WithMany(p => p.tblPersona_PeticionCambioDatosidDocCertificadoDiscapacidadNavigation)
                    .HasForeignKey(d => d.idDocCertificadoDiscapacidad)
                    .HasConstraintName("FK_tblPersona_PeticionCambioDatos_tblDocumento");

                entity.HasOne(d => d.idDocumentoLicenciaConducirNavigation)
                    .WithMany(p => p.tblPersona_PeticionCambioDatosidDocumentoLicenciaConducirNavigation)
                    .HasForeignKey(d => d.idDocumentoLicenciaConducir)
                    .HasConstraintName("FK_tblPersona_PeticionCambioDatos_tblDocumento_LicenciaConducir");

                entity.HasOne(d => d.idEstadoCivilNavigation)
                    .WithMany(p => p.tblPersona_PeticionCambioDatos)
                    .HasForeignKey(d => d.idEstadoCivil)
                    .HasConstraintName("FK_tblPersona_PeticionCambioDatos_tblEstadoCivil");

                entity.HasOne(d => d.idFotoDemandanteEmpleoNavigation)
                    .WithMany(p => p.tblPersona_PeticionCambioDatosidFotoDemandanteEmpleoNavigation)
                    .HasForeignKey(d => d.idFotoDemandanteEmpleo)
                    .HasConstraintName("FK_tblPersona_PeticionCambioDatos_tblDocumento_Discapacidad");

                entity.HasOne(d => d.idFotoDocumentoIdentidad_ANavigation)
                    .WithMany(p => p.tblPersona_PeticionCambioDatosidFotoDocumentoIdentidad_ANavigation)
                    .HasForeignKey(d => d.idFotoDocumentoIdentidad_A)
                    .HasConstraintName("FK_tblPersona_PeticionCambioDatos_tblDocumento_IdentidadA");

                entity.HasOne(d => d.idFotoDocumentoIdentidad_BNavigation)
                    .WithMany(p => p.tblPersona_PeticionCambioDatosidFotoDocumentoIdentidad_BNavigation)
                    .HasForeignKey(d => d.idFotoDocumentoIdentidad_B)
                    .HasConstraintName("FK_tblPersona_PeticionCambioDatos_tblDocumento_IdentidadB");

                entity.HasOne(d => d.idFotoIBANNavigation)
                    .WithMany(p => p.tblPersona_PeticionCambioDatosidFotoIBANNavigation)
                    .HasForeignKey(d => d.idFotoIBAN)
                    .HasConstraintName("FK_tblPersona_PeticionCambioDatos_tblDocumento_IBAN");

                entity.HasOne(d => d.idFotoNAFNavigation)
                    .WithMany(p => p.tblPersona_PeticionCambioDatosidFotoNAFNavigation)
                    .HasForeignKey(d => d.idFotoNAF)
                    .HasConstraintName("FK_tblPersona_PeticionCambioDatos_tblDocumento_NAF");

                entity.HasOne(d => d.idFotoPerfilNavigation)
                    .WithMany(p => p.tblPersona_PeticionCambioDatosidFotoPerfilNavigation)
                    .HasForeignKey(d => d.idFotoPerfil)
                    .HasConstraintName("FK_tblPersona_PeticionCambioDatos_tblDocumento_Perfil");

                entity.HasOne(d => d.idGeneroNavigation)
                    .WithMany(p => p.tblPersona_PeticionCambioDatos)
                    .HasForeignKey(d => d.idGenero)
                    .HasConstraintName("FK_tblPersona_PeticionCambioDatos_tblGenero");

                entity.HasOne(d => d.idNivelEstudiosNavigation)
                    .WithMany(p => p.tblPersona_PeticionCambioDatos)
                    .HasForeignKey(d => d.idNivelEstudios)
                    .HasConstraintName("FK_tblPersona_PeticionCambioDatos_tblNivelEstudios");

                entity.HasOne(d => d.idPaisNavigation)
                    .WithMany(p => p.tblPersona_PeticionCambioDatos)
                    .HasForeignKey(d => d.idPais)
                    .HasConstraintName("FK_tblPersona_PeticionCambioDatos_tblPais");

                entity.HasOne(d => d.idPersonaNavigation)
                    .WithMany(p => p.tblPersona_PeticionCambioDatos)
                    .HasForeignKey(d => d.idPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPersona_PeticionCambioDatos_tblPersona");

                entity.HasOne(d => d.idPeticionCambioDatos_estadoNavigation)
                    .WithMany(p => p.tblPersona_PeticionCambioDatos)
                    .HasForeignKey(d => d.idPeticionCambioDatos_estado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPersona_PeticionCambioDatos_tbPeticionCambioDatos_estado");

                entity.HasOne(d => d.idTallaAlfa_CamisetaNavigation)
                    .WithMany(p => p.tblPersona_PeticionCambioDatos)
                    .HasForeignKey(d => d.idTallaAlfa_Camiseta)
                    .HasConstraintName("FK_tblPersona_PeticionCambioDatos_tblTallaAlfa");

                entity.HasOne(d => d.idTipoDocumentoIdentidadNavigation)
                    .WithMany(p => p.tblPersona_PeticionCambioDatosidTipoDocumentoIdentidadNavigation)
                    .HasForeignKey(d => d.idTipoDocumentoIdentidad)
                    .HasConstraintName("FK_tblPersona_PeticionCambioDatos_tblTipoDocumentoIdentidad");

                entity.HasOne(d => d.idTipoDocumentoIdentidad_tutorNavigation)
                    .WithMany(p => p.tblPersona_PeticionCambioDatosidTipoDocumentoIdentidad_tutorNavigation)
                    .HasForeignKey(d => d.idTipoDocumentoIdentidad_tutor)
                    .HasConstraintName("FK_tblPersona_PeticionCambioDatos_tblTipoDocumentoIdentidad1");

                entity.HasMany(d => d.idLicenciaConducir)
                    .WithMany(p => p.idPeticionCambioDatos)
                    .UsingEntity<Dictionary<string, object>>(
                        "tblLicenciaConducirNPeticionCambioDatos",
                        l => l.HasOne<tblLicenciaConducir>().WithMany().HasForeignKey("idLicenciaConducir").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblLicenciaConducirNPeticionCambioDatos_tblLicenciaConducirNPeticionCambioDatos"),
                        r => r.HasOne<tblPersona_PeticionCambioDatos>().WithMany().HasForeignKey("idPeticionCambioDatos").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblLicenciaConducirNPeticionCambioDatos_tblPersona_PeticionCambioDatos"),
                        j =>
                        {
                            j.HasKey("idPeticionCambioDatos", "idLicenciaConducir");

                            j.ToTable("tblLicenciaConducirNPeticionCambioDatos", "RRHH");
                        });
            });

            modelBuilder.Entity<tblPersonasNParte>(entity =>
            {
                entity.HasKey(e => new { e.idPersona, e.idParte })
                    .HasName("PK__tblPerso__F132A311D532367D");

                entity.HasOne(d => d.idParteNavigation)
                    .WithMany(p => p.tblPersonasNParte)
                    .HasForeignKey(d => d.idParte)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPersonasNParte_tblParteTrabajo");

                entity.HasOne(d => d.idPersonaNavigation)
                    .WithMany(p => p.tblPersonasNParte)
                    .HasForeignKey(d => d.idPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPersonasNParte_tblPersona");
            });

            modelBuilder.Entity<tblPersonasNParte1>(entity =>
            {
                entity.HasOne(d => d.idParteNavigation)
                    .WithMany(p => p.tblPersonasNParte1)
                    .HasForeignKey(d => d.idParte)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPersonasNParte_tblParteTrabajo");

                entity.HasOne(d => d.idPersonaNavigation)
                    .WithMany(p => p.tblPersonasNParte1)
                    .HasForeignKey(d => d.idPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPersonasNParte_tblPersona");
            });

            modelBuilder.Entity<tblPesoNCategoriaNLavanderia>(entity =>
            {
                entity.HasKey(e => new { e.idCategoriaMaquina, e.idLavanderia });

                entity.HasOne(d => d.idCategoriaMaquinaNavigation)
                    .WithMany(p => p.tblPesoNCategoriaNLavanderia)
                    .HasForeignKey(d => d.idCategoriaMaquina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPesoNCategoriaNLavanderia_tblCategoriaMaquina");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblPesoNCategoriaNLavanderia)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPesoNCategoriaNLavanderia_tblLavanderia");
            });

            modelBuilder.Entity<tblPesoNSistemaNLavanderia>(entity =>
            {
                entity.HasKey(e => new { e.idSistemaMaquina, e.idLavanderia });

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblPesoNSistemaNLavanderia)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPesoNSistemaNLavanderia_tblLavanderia");

                entity.HasOne(d => d.idSistemaMaquinaNavigation)
                    .WithMany(p => p.tblPesoNSistemaNLavanderia)
                    .HasForeignKey(d => d.idSistemaMaquina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPesoNSistemaNLavanderia_tblSistemaMaquina");
            });

            modelBuilder.Entity<tblPlanificacionNCierrePresupuestario>(entity =>
            {
                entity.HasOne(d => d.idAdmCuentaContableNavigation)
                    .WithMany(p => p.tblPlanificacionNCierrePresupuestario)
                    .HasForeignKey(d => d.idAdmCuentaContable)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPlanificacion_tblAdmCuentaContable");

                entity.HasOne(d => d.idCierrePresupuestarioNavigation)
                    .WithMany(p => p.tblPlanificacionNCierrePresupuestario)
                    .HasForeignKey(d => d.idCierrePresupuestario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPlanificacion_tblCierrePresupuestario");

                entity.HasOne(d => d.idMonedaNavigation)
                    .WithMany(p => p.tblPlanificacionNCierrePresupuestario)
                    .HasForeignKey(d => d.idMoneda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPlanificacion_tblMoneda");
            });

            modelBuilder.Entity<tblPlantillaPrenda_generica>(entity =>
            {
                entity.HasOne(d => d.elementoOfficeNavigation)
                    .WithMany(p => p.tblPlantillaPrenda_genericaelementoOfficeNavigation)
                    .HasForeignKey(d => d.elementoOffice)
                    .HasConstraintName("FK_tblPlantillaPrenda_generica_tblElemTransOffice");

                entity.HasOne(d => d.elementoPedidoNavigation)
                    .WithMany(p => p.tblPlantillaPrenda_genericaelementoPedidoNavigation)
                    .HasForeignKey(d => d.elementoPedido)
                    .HasConstraintName("FK_tblPlantillaPrenda_generica_tblElemTrans");

                entity.HasOne(d => d.elementoRepartoNavigation)
                    .WithMany(p => p.tblPlantillaPrenda_genericaelementoRepartoNavigation)
                    .HasForeignKey(d => d.elementoReparto)
                    .HasConstraintName("FK_tblPlantillaPrenda_generica_tblElemTransReparto");

                entity.HasOne(d => d.idColorTapaNavigation)
                    .WithMany(p => p.tblPlantillaPrenda_generica)
                    .HasForeignKey(d => d.idColorTapa)
                    .HasConstraintName("FK_tblPlantillaPrenda_generica_tblColorTapa");

                entity.HasOne(d => d.idDenoPrendaNavigation)
                    .WithMany(p => p.tblPlantillaPrenda_generica)
                    .HasForeignKey(d => d.idDenoPrenda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPlantillaPrenda_generica_tblDenoPrenda");

                entity.HasOne(d => d.idGrupoPlantillaPrenda_genericaNavigation)
                    .WithMany(p => p.tblPlantillaPrenda_generica)
                    .HasForeignKey(d => d.idGrupoPlantillaPrenda_generica)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPlantillaPrenda_generica_tblGrupoPlantillaPrenda_generica");

                entity.HasOne(d => d.idMarcaTapaNavigation)
                    .WithMany(p => p.tblPlantillaPrenda_generica)
                    .HasForeignKey(d => d.idMarcaTapa)
                    .HasConstraintName("FK_tblPlantillaPrenda_generica_tblMarcaTapa");
            });

            modelBuilder.Entity<tblPlantillaPrenda_generica_historico_peso>(entity =>
            {
                entity.HasKey(e => new { e.idPlantillaPrenda_generica, e.fecha });

                entity.HasOne(d => d.idPlantillaPrenda_genericaNavigation)
                    .WithMany(p => p.tblPlantillaPrenda_generica_historico_peso)
                    .HasForeignKey(d => d.idPlantillaPrenda_generica)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPlantillaPrenda_generica_historico_peso_tblPlantillaPrenda_generica");
            });

            modelBuilder.Entity<tblPlantillaTareaMantenimientoPrev>(entity =>
            {
                entity.Property(e => e.idPlantillaTareaMantenimientoPrev).ValueGeneratedOnAdd();

                entity.HasOne(d => d.idSistemaMaquinaNavigation)
                    .WithMany(p => p.tblPlantillaTareaMantenimientoPrev)
                    .HasForeignKey(d => d.idSistemaMaquina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPlantillaTareaMantenimientoPrev_tblSistemaMaquina");
            });

            modelBuilder.Entity<tblPoliticaDisciplinaria>(entity =>
            {
                entity.HasOne(d => d.idTraduccionNavigation)
                    .WithMany(p => p.tblPoliticaDisciplinaria)
                    .HasForeignKey(d => d.idTraduccion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPoliticaDisciplinaria_tblTraduccion");
            });

            modelBuilder.Entity<tblPorcentajeValorPrendaNEntidad>(entity =>
            {
                entity.HasKey(e => new { e.idEntidad, e.numMeses });

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblPorcentajeValorPrendaNEntidad)
                    .HasForeignKey(d => d.idEntidad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPorcentajeValorPrendaNEntidad_tblEntidad");
            });

            modelBuilder.Entity<tblPosicionNAreaLavanderiaNLavanderia>(entity =>
            {
                entity.Property(e => e.activo).HasDefaultValueSql("((1))");

                entity.Property(e => e.denominacion).HasDefaultValueSql("('')");

                entity.HasOne(d => d.idAreaLavanderiaNavigation)
                    .WithMany(p => p.tblPosicionNAreaLavanderiaNLavanderia)
                    .HasForeignKey(d => d.idAreaLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPosicionNAreaLavanderiaNLavanderia_tblAreaLavanderia");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblPosicionNAreaLavanderiaNLavanderia)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPosicionNAreaLavanderiaNLavanderia_tblLavanderia");

                entity.HasOne(d => d.idMaquinaNavigation)
                    .WithMany(p => p.tblPosicionNAreaLavanderiaNLavanderia)
                    .HasForeignKey(d => d.idMaquina)
                    .HasConstraintName("FK_tblPosicionNAreaLavanderiaNLavanderia_tblMaquina");
            });

            modelBuilder.Entity<tblPrecioLavadoPrenda>(entity =>
            {
                entity.HasKey(e => new { e.idPrenda, e.fecha })
                    .HasName("PK_tblPrecioPrenda");

                entity.HasOne(d => d.idPrendaNavigation)
                    .WithMany(p => p.tblPrecioLavadoPrenda)
                    .HasForeignKey(d => d.idPrenda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrecioLavadoPrenda_tblPrenda");
            });

            modelBuilder.Entity<tblPregunta>(entity =>
            {
                entity.HasOne(d => d.idEncuestaPlantillaNavigation)
                    .WithMany(p => p.tblPregunta)
                    .HasForeignKey(d => d.idEncuestaPlantilla)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPregunta_tblEncuesta");

                entity.HasOne(d => d.idGrupoPreguntaNavigation)
                    .WithMany(p => p.tblPregunta)
                    .HasForeignKey(d => d.idGrupoPregunta)
                    .HasConstraintName("FK_tblPregunta_tblGrupoPregunta");

                entity.HasOne(d => d.idTipoPreguntaNavigation)
                    .WithMany(p => p.tblPregunta)
                    .HasForeignKey(d => d.idTipoPregunta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPregunta_tblTipoPregunta");
            });

            modelBuilder.Entity<tblPreguntaNOpcionNPregunta>(entity =>
            {
                entity.HasKey(e => new { e.idPregunta, e.idOpcion });

                entity.HasOne(d => d.idOpcionNavigation)
                    .WithMany(p => p.tblPreguntaNOpcionNPregunta)
                    .HasForeignKey(d => d.idOpcion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPreguntaNOpcionNPregunta_tblOpcion");

                entity.HasOne(d => d.idPreguntaNavigation)
                    .WithMany(p => p.tblPreguntaNOpcionNPreguntaidPreguntaNavigation)
                    .HasForeignKey(d => d.idPregunta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPreguntaNOpcionNPregunta_tblPregunta");

                entity.HasOne(d => d.idPreguntaAnidadaNavigation)
                    .WithMany(p => p.tblPreguntaNOpcionNPreguntaidPreguntaAnidadaNavigation)
                    .HasForeignKey(d => d.idPreguntaAnidada)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPreguntaNOpcionNPregunta_tblPreguntaAnidada");
            });

            modelBuilder.Entity<tblPrenda>(entity =>
            {
                entity.HasOne(d => d.elementoOfficeNavigation)
                    .WithMany(p => p.tblPrendaelementoOfficeNavigation)
                    .HasForeignKey(d => d.elementoOffice)
                    .HasConstraintName("FK_tblPrenda_tblElemTransOffice");

                entity.HasOne(d => d.elementoPedidoNavigation)
                    .WithMany(p => p.tblPrendaelementoPedidoNavigation)
                    .HasForeignKey(d => d.elementoPedido)
                    .HasConstraintName("FK_tblPrenda_tblElemTrans");

                entity.HasOne(d => d.elementoRepartoNavigation)
                    .WithMany(p => p.tblPrendaelementoRepartoNavigation)
                    .HasForeignKey(d => d.elementoReparto)
                    .HasConstraintName("FK_tblPrenda_tblElemTransReparto");

                entity.HasOne(d => d.idColorTapaNavigation)
                    .WithMany(p => p.tblPrenda)
                    .HasForeignKey(d => d.idColorTapa)
                    .HasConstraintName("FK_tblPrenda_tblColorTapa");

                entity.HasOne(d => d.idCompañiaNavigation)
                    .WithMany(p => p.tblPrenda)
                    .HasForeignKey(d => d.idCompañia)
                    .HasConstraintName("FK_tblPrenda_tblCompañia");

                entity.HasOne(d => d.idDenoPrendaNavigation)
                    .WithMany(p => p.tblPrenda)
                    .HasForeignKey(d => d.idDenoPrenda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrenda_tblDenoPrenda");

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblPrenda)
                    .HasForeignKey(d => d.idEntidad)
                    .HasConstraintName("FK_tblPrenda_tblEntidad");

                entity.HasOne(d => d.idFamiliaFacturacionNavigation)
                    .WithMany(p => p.tblPrenda)
                    .HasForeignKey(d => d.idFamiliaFacturacion)
                    .HasConstraintName("FK_tblPrenda_tblFamilia");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblPrenda)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblLavanderia_tblPrenda");

                entity.HasOne(d => d.idMarcaTapaNavigation)
                    .WithMany(p => p.tblPrenda)
                    .HasForeignKey(d => d.idMarcaTapa)
                    .HasConstraintName("FK_tblPrenda_tblMarcaTapa");

                entity.HasOne(d => d.idPlantillaPrenda_genericaNavigation)
                    .WithMany(p => p.tblPrenda)
                    .HasForeignKey(d => d.idPlantillaPrenda_generica)
                    .HasConstraintName("FK_tblPrenda_tblPlantillaPrenda_generica");

                entity.HasOne(d => d.tipoFactNavigation)
                    .WithMany(p => p.tblPrenda)
                    .HasForeignKey(d => d.tipoFact)
                    .HasConstraintName("FK_tblPrenda_tblTipoFacturacion");
            });

            modelBuilder.Entity<tblPrendaEjecutivo>(entity =>
            {
                entity.HasKey(e => e.idPrendaEjecutivo)
                    .HasName("PK__tblPrend__9510B779E6EDF6B4");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblPrendaEjecutivo)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaEjecutivo_tblLavanderia");
            });

            modelBuilder.Entity<tblPrendaEjecutivoNRepartoValet>(entity =>
            {
                entity.HasKey(e => new { e.idRepartoValet, e.idPrendaEjecutivo });

                entity.Property(e => e.cantidadLavadoDoblado).HasDefaultValueSql("((0))");

                entity.Property(e => e.cantidadLavadoSecadoPlanchado).HasDefaultValueSql("((0))");

                entity.Property(e => e.cantidadLavadoVaporizado).HasDefaultValueSql("((0))");

                entity.Property(e => e.cantidadPlanchado).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.idPrendaEjecutivoNavigation)
                    .WithMany(p => p.tblPrendaEjecutivoNRepartoValet)
                    .HasForeignKey(d => d.idPrendaEjecutivo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaEjecutivoNRepartoValet_tblPrendaEjecutivo");

                entity.HasOne(d => d.idRepartoValetNavigation)
                    .WithMany(p => p.tblPrendaEjecutivoNRepartoValet)
                    .HasForeignKey(d => d.idRepartoValet)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaEjecutivoNRepartoValet_tblRepartosValet");
            });

            modelBuilder.Entity<tblPrendaExtra>(entity =>
            {
                entity.HasKey(e => e.idPrendaExtra)
                    .HasName("PK_tblPrendaExtra_1");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblPrendaExtra)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaExtra_tblLavanderia");
            });

            modelBuilder.Entity<tblPrendaExtraNRepartoValet>(entity =>
            {
                entity.HasKey(e => new { e.idRepartosValet, e.idPrendaExtra });

                entity.Property(e => e.cantidadLavadoPlanchado).HasDefaultValueSql("((0))");

                entity.Property(e => e.cantidadLavadoSeco).HasDefaultValueSql("((0))");

                entity.Property(e => e.cantidadPlanchado).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.idPrendaExtraNavigation)
                    .WithMany(p => p.tblPrendaExtraNRepartoValet)
                    .HasForeignKey(d => d.idPrendaExtra)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaExtraNRepartoValet_tblPrendaExtra");

                entity.HasOne(d => d.idRepartosValetNavigation)
                    .WithMany(p => p.tblPrendaExtraNRepartoValet)
                    .HasForeignKey(d => d.idRepartosValet)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaExtraNRepartoValet_tblRepartosHuesped");
            });

            modelBuilder.Entity<tblPrendaHuesped>(entity =>
            {
                entity.HasKey(e => e.idPrendaHuesped)
                    .HasName("PK_tblPrendaHuesped_1");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblPrendaHuesped)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaHuesped_tblLavanderia");
            });

            modelBuilder.Entity<tblPrendaHuespedNRepartoValet>(entity =>
            {
                entity.HasKey(e => new { e.idRepartoValet, e.idPrendaHuesped })
                    .HasName("PK_tblPrendaHuespedNRepartoHuesped");

                entity.Property(e => e.cantidadLavadoPlanchado).HasDefaultValueSql("((0))");

                entity.Property(e => e.cantidadLavadoSeco).HasDefaultValueSql("((0))");

                entity.Property(e => e.cantidadPlanchado).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.idPrendaHuespedNavigation)
                    .WithMany(p => p.tblPrendaHuespedNRepartoValet)
                    .HasForeignKey(d => d.idPrendaHuesped)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaHuespedNRepartoHuesped_tblPrendaHuesped");

                entity.HasOne(d => d.idRepartoValetNavigation)
                    .WithMany(p => p.tblPrendaHuespedNRepartoValet)
                    .HasForeignKey(d => d.idRepartoValet)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaHuespedNRepartoHuesped_tblRepartosHuesped");
            });

            modelBuilder.Entity<tblPrendaNAbono>(entity =>
            {
                entity.HasKey(e => new { e.idPrenda, e.idAbono });

                entity.HasOne(d => d.idAbonoNavigation)
                    .WithMany(p => p.tblPrendaNAbono)
                    .HasForeignKey(d => d.idAbono)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Logistica.tblPrendaNAbono_Logistica.tblAbono");

                entity.HasOne(d => d.idPrendaNavigation)
                    .WithMany(p => p.tblPrendaNAbono)
                    .HasForeignKey(d => d.idPrenda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Logistica.tblPrendaNAbono_tblPrenda");
            });

            modelBuilder.Entity<tblPrendaNAlmacen>(entity =>
            {
                entity.HasKey(e => new { e.idPrenda, e.idAlmacen });

                entity.HasOne(d => d.idAlmacenNavigation)
                    .WithMany(p => p.tblPrendaNAlmacen)
                    .HasForeignKey(d => d.idAlmacen)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaNAlmacen_tblAlmacen");

                entity.HasOne(d => d.idPrendaNavigation)
                    .WithMany(p => p.tblPrendaNAlmacen)
                    .HasForeignKey(d => d.idPrenda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaNAlmacen_tblPrenda");
            });

            modelBuilder.Entity<tblPrendaNAlmacenNInventario>(entity =>
            {
                entity.HasOne(d => d.idPlantillaPrenda_genericaNavigation)
                    .WithMany(p => p.tblPrendaNAlmacenNInventario)
                    .HasForeignKey(d => d.idPlantillaPrenda_generica)
                    .HasConstraintName("FK_tblPrendaNAlmacenNInventario_tblPlantillaPrenda_generica");

                entity.HasOne(d => d.idPrendaNavigation)
                    .WithMany(p => p.tblPrendaNAlmacenNInventario)
                    .HasForeignKey(d => d.idPrenda)
                    .HasConstraintName("FK_tblPrendaNAlmacenNInventario_tblPrenda");

                entity.HasOne(d => d.id)
                    .WithMany(p => p.tblPrendaNAlmacenNInventario)
                    .HasForeignKey(d => new { d.idInventario, d.idAlmacen })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaNAlmacenNInventario_tblAlmacenNInventario");
            });

            modelBuilder.Entity<tblPrendaNEntidad_NuevoPedido>(entity =>
            {
                entity.HasKey(e => new { e.idPrenda, e.idEntidad });

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblPrendaNEntidad_NuevoPedido)
                    .HasForeignKey(d => d.idEntidad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaNEntidad_NuevoPedido_tblEntidad");

                entity.HasOne(d => d.idPrendaNavigation)
                    .WithMany(p => p.tblPrendaNEntidad_NuevoPedido)
                    .HasForeignKey(d => d.idPrenda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaNEntidad_NuevoPedido_tblPrenda");
            });

            modelBuilder.Entity<tblPrendaNGestionRetiro>(entity =>
            {
                entity.HasOne(d => d.idGestionRetiroNavigation)
                    .WithMany(p => p.tblPrendaNGestionRetiro)
                    .HasForeignKey(d => d.idGestionRetiro)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblPrenda__idGes__3EDDEA9E");

                entity.HasOne(d => d.idPlantillaPrenda_genericaNavigation)
                    .WithMany(p => p.tblPrendaNGestionRetiro)
                    .HasForeignKey(d => d.idPlantillaPrenda_generica)
                    .HasConstraintName("FK_tblPrendaNGestionRetiro_tblPlantillaPrenda_generica");

                entity.HasOne(d => d.idPrendaNavigation)
                    .WithMany(p => p.tblPrendaNGestionRetiro)
                    .HasForeignKey(d => d.idPrenda)
                    .HasConstraintName("FK__tblPrenda__idPre__3FD20ED7");

                entity.HasOne(d => d.idTipoRetiroNavigation)
                    .WithMany(p => p.tblPrendaNGestionRetiro)
                    .HasForeignKey(d => d.idTipoRetiro)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblPrenda__idTip__3DE9C665");
            });

            modelBuilder.Entity<tblPrendaNInventario>(entity =>
            {
                entity.HasOne(d => d.idInventarioNavigation)
                    .WithMany(p => p.tblPrendaNInventario)
                    .HasForeignKey(d => d.idInventario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaNInventario_tblInventario");

                entity.HasOne(d => d.idPlantillaPrenda_genericaNavigation)
                    .WithMany(p => p.tblPrendaNInventario)
                    .HasForeignKey(d => d.idPlantillaPrenda_generica)
                    .HasConstraintName("FK_tblPrendaNInventario_tblPlantillaPrenda_generica");

                entity.HasOne(d => d.idPrendaNavigation)
                    .WithMany(p => p.tblPrendaNInventario)
                    .HasForeignKey(d => d.idPrenda)
                    .HasConstraintName("FK_tblPrendaNInventario_tblPrenda");
            });

            modelBuilder.Entity<tblPrendaNLavanderia>(entity =>
            {
                entity.HasKey(e => new { e.idLavanderia, e.idPrenda });

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblPrendaNLavanderia)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaNLavanderia_tblLavanderia");

                entity.HasOne(d => d.idPrendaNavigation)
                    .WithMany(p => p.tblPrendaNLavanderia)
                    .HasForeignKey(d => d.idPrenda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaNLavanderia_tblPrenda");
            });

            modelBuilder.Entity<tblPrendaNMaquina>(entity =>
            {
                entity.HasOne(d => d.idFamiliaNavigation)
                    .WithMany(p => p.tblPrendaNMaquina)
                    .HasForeignKey(d => d.idFamilia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaNMaquina_tblFamilia");

                entity.HasOne(d => d.idMaquinaNavigation)
                    .WithMany(p => p.tblPrendaNMaquina)
                    .HasForeignKey(d => d.idMaquina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaNMaquina_tblMaquina");

                entity.HasOne(d => d.idTipoPrendaNavigation)
                    .WithMany(p => p.tblPrendaNMaquina)
                    .HasForeignKey(d => d.idTipoPrenda)
                    .HasConstraintName("FK_tblPrendaNMaquina_tblTipoPrenda");
            });

            modelBuilder.Entity<tblPrendaNMovimiento>(entity =>
            {
                entity.Property(e => e.precio).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.idMovimientoNavigation)
                    .WithMany(p => p.tblPrendaNMovimiento)
                    .HasForeignKey(d => d.idMovimiento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaNMovimiento_tblMovimiento");

                entity.HasOne(d => d.idPlantillaPrenda_genericaNavigation)
                    .WithMany(p => p.tblPrendaNMovimiento)
                    .HasForeignKey(d => d.idPlantillaPrenda_generica)
                    .HasConstraintName("FK_tblPrendaNMovimiento_tblPlantillaPrenda_generica");

                entity.HasOne(d => d.idPrendaNavigation)
                    .WithMany(p => p.tblPrendaNMovimiento)
                    .HasForeignKey(d => d.idPrenda)
                    .HasConstraintName("FK_tblPrendaNMovimiento_tblPrenda");
            });

            modelBuilder.Entity<tblPrendaNMuestreo>(entity =>
            {
                entity.HasKey(e => e.idPrendaNMuestreo)
                    .HasName("PK__tblPrend__B2072C77EB2FB5AE");

                entity.HasOne(d => d.idCompañiaNavigation)
                    .WithMany(p => p.tblPrendaNMuestreo)
                    .HasForeignKey(d => d.idCompañia)
                    .HasConstraintName("FK_idCompañia");

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblPrendaNMuestreo)
                    .HasForeignKey(d => d.idEntidad)
                    .HasConstraintName("FK_idEntidad");

                entity.HasOne(d => d.idMuestreoNavigation)
                    .WithMany(p => p.tblPrendaNMuestreo)
                    .HasForeignKey(d => d.idMuestreo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_idMuestreo");

                entity.HasOne(d => d.idPrendaNavigation)
                    .WithMany(p => p.tblPrendaNMuestreo)
                    .HasForeignKey(d => d.idPrenda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_idPrenda");
            });

            modelBuilder.Entity<tblPrendaNMuestreo_FS>(entity =>
            {
                entity.HasKey(e => e.idPrendaNMuestreoFS)
                    .HasName("PK__tblPrend__EB265ADCA7424DE7");

                entity.HasOne(d => d.idCompañiaNavigation)
                    .WithMany(p => p.tblPrendaNMuestreo_FS)
                    .HasForeignKey(d => d.idCompañia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_idCompañiaFS");

                entity.HasOne(d => d.idMuestreoNavigation)
                    .WithMany(p => p.tblPrendaNMuestreo_FS)
                    .HasForeignKey(d => d.idMuestreo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_idMuestreoFS");

                entity.HasOne(d => d.idPrendaNavigation)
                    .WithMany(p => p.tblPrendaNMuestreo_FS)
                    .HasForeignKey(d => d.idPrenda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_idPrendaFS");

                entity.HasOne(d => d.idTipoFSNavigation)
                    .WithMany(p => p.tblPrendaNMuestreo_FS)
                    .HasForeignKey(d => d.idTipoFS)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_idTipoFS");
            });

            modelBuilder.Entity<tblPrendaNPedido>(entity =>
            {
                entity.HasOne(d => d.idPedidoNavigation)
                    .WithMany(p => p.tblPrendaNPedido)
                    .HasForeignKey(d => d.idPedido)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaNPedido_tblPedido");

                entity.HasOne(d => d.idPrendaNavigation)
                    .WithMany(p => p.tblPrendaNPedido)
                    .HasForeignKey(d => d.idPrenda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaNPedido_tblPrenda");
            });

            modelBuilder.Entity<tblPrendaNPedidoExtra>(entity =>
            {
                entity.HasKey(e => new { e.idPrenda, e.idPedidoExtra });

                entity.HasOne(d => d.idPedidoExtraNavigation)
                    .WithMany(p => p.tblPrendaNPedidoExtra)
                    .HasForeignKey(d => d.idPedidoExtra)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaNPedidoExtra_tblPedidosExtra");

                entity.HasOne(d => d.idPrendaNavigation)
                    .WithMany(p => p.tblPrendaNPedidoExtra)
                    .HasForeignKey(d => d.idPrenda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaNPedidoExtra_tblPrenda");
            });

            modelBuilder.Entity<tblPrendaNPedidoHuesped>(entity =>
            {
                entity.HasOne(d => d.idColorPrendaHuespedNavigation)
                    .WithMany(p => p.tblPrendaNPedidoHuesped)
                    .HasForeignKey(d => d.idColorPrendaHuesped)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaNPedidoHuesped_tblColorPrendaHuesped");

                entity.HasOne(d => d.idDefectoPrendaHuespedNavigation)
                    .WithMany(p => p.tblPrendaNPedidoHuesped)
                    .HasForeignKey(d => d.idDefectoPrendaHuesped)
                    .HasConstraintName("FK_tblPrendaNPedidoHuesped_tblDefectoPrendaHuesped");

                entity.HasOne(d => d.idPedidoHuespedNavigation)
                    .WithMany(p => p.tblPrendaNPedidoHuesped)
                    .HasForeignKey(d => d.idPedidoHuesped)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaNPedidoHuesped_tblPedidoHuesped");

                entity.HasOne(d => d.idPrendaHuespedNavigation)
                    .WithMany(p => p.tblPrendaNPedidoHuesped)
                    .HasForeignKey(d => d.idPrendaHuesped)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaNPedidoHuesped_tblPrendaHuesped");

                entity.HasOne(d => d.idTipoLavadoNavigation)
                    .WithMany(p => p.tblPrendaNPedidoHuesped)
                    .HasForeignKey(d => d.idTipoLavado)
                    .HasConstraintName("FK_tblPrendaNPedidoHuesped_tblTipoLavado");

                entity.HasOne(d => d.idTipoPrendaHuespedNavigation)
                    .WithMany(p => p.tblPrendaNPedidoHuesped)
                    .HasForeignKey(d => d.idTipoPrendaHuesped)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaNPedidoHuesped_tblTipoPrendaHuesped");
            });

            modelBuilder.Entity<tblPrendaNProduccion>(entity =>
            {
                entity.HasOne(d => d.idPlantillaPrenda_genericaNavigation)
                    .WithMany(p => p.tblPrendaNProduccion)
                    .HasForeignKey(d => d.idPlantillaPrenda_generica)
                    .HasConstraintName("FK_tblPrendaNProduccion_tblPlantillaPrenda_generica");

                entity.HasOne(d => d.idPrendaNavigation)
                    .WithMany(p => p.tblPrendaNProduccion)
                    .HasForeignKey(d => d.idPrenda)
                    .HasConstraintName("FK_tblPrendaNProduccion_tblPrenda");

                entity.HasOne(d => d.idProduccionNavigation)
                    .WithMany(p => p.tblPrendaNProduccion)
                    .HasForeignKey(d => d.idProduccion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaNProduccion_tblProduccion");
            });

            modelBuilder.Entity<tblPrendaNReparto>(entity =>
            {
                entity.HasKey(e => new { e.idPrenda, e.idReparto });

                entity.HasOne(d => d.idPrendaNavigation)
                    .WithMany(p => p.tblPrendaNReparto)
                    .HasForeignKey(d => d.idPrenda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaNReparto_tblPrenda");

                entity.HasOne(d => d.idRepartoNavigation)
                    .WithMany(p => p.tblPrendaNReparto)
                    .HasForeignKey(d => d.idReparto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaNReparto_tblReparto");
            });

            modelBuilder.Entity<tblPrendaNRepartoOffice>(entity =>
            {
                entity.HasKey(e => new { e.idPrenda, e.idRepartoOffice });

                entity.HasOne(d => d.idPrendaNavigation)
                    .WithMany(p => p.tblPrendaNRepartoOffice)
                    .HasForeignKey(d => d.idPrenda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaNRepartoOffice_tblPrenda");

                entity.HasOne(d => d.idRepartoOfficeNavigation)
                    .WithMany(p => p.tblPrendaNRepartoOffice)
                    .HasForeignKey(d => d.idRepartoOffice)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaNRepartoOffice_tblRepartoOffice");
            });

            modelBuilder.Entity<tblPrendaNRevision>(entity =>
            {
                entity.HasKey(e => new { e.idPrenda, e.idRevision });

                entity.HasOne(d => d.idPrendaNavigation)
                    .WithMany(p => p.tblPrendaNRevision)
                    .HasForeignKey(d => d.idPrenda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaNRevision_tblPrenda");

                entity.HasOne(d => d.idRevisionNavigation)
                    .WithMany(p => p.tblPrendaNRevision)
                    .HasForeignKey(d => d.idRevision)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaNRevision_tblRevision");
            });

            modelBuilder.Entity<tblPrendaNSolicitudAbono>(entity =>
            {
                entity.HasKey(e => new { e.idPrenda, e.idSolicitudAbono });

                entity.HasOne(d => d.idPrendaNavigation)
                    .WithMany(p => p.tblPrendaNSolicitudAbono)
                    .HasForeignKey(d => d.idPrenda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaNSolicitudAbono_tblPrenda");

                entity.HasOne(d => d.idSolicitudAbonoNavigation)
                    .WithMany(p => p.tblPrendaNSolicitudAbono)
                    .HasForeignKey(d => d.idSolicitudAbono)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaNSolicitudAbono_tblSolicitudAbono");
            });

            modelBuilder.Entity<tblPrendaNSubAlmacen>(entity =>
            {
                entity.HasKey(e => new { e.idPrenda, e.idSubAlmacen });

                entity.HasOne(d => d.idPrendaNavigation)
                    .WithMany(p => p.tblPrendaNSubAlmacen)
                    .HasForeignKey(d => d.idPrenda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaNSubAlmacen_tblPrenda");

                entity.HasOne(d => d.idSubAlmacenNavigation)
                    .WithMany(p => p.tblPrendaNSubAlmacen)
                    .HasForeignKey(d => d.idSubAlmacen)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaNSubAlmacen_tblSubAlmacen");
            });

            modelBuilder.Entity<tblPrendaNTipoHabitacion>(entity =>
            {
                entity.HasKey(e => new { e.idTipoHabitacion, e.idPrenda });

                entity.HasOne(d => d.idPrendaNavigation)
                    .WithMany(p => p.tblPrendaNTipoHabitacion)
                    .HasForeignKey(d => d.idPrenda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaNTipoHabitacion_tblPrenda");

                entity.HasOne(d => d.idTipoHabitacionNavigation)
                    .WithMany(p => p.tblPrendaNTipoHabitacion)
                    .HasForeignKey(d => d.idTipoHabitacion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaNTipoHabitacion_tblTipoHabitacion");
            });

            modelBuilder.Entity<tblPrendaNUsuarioNEntidad>(entity =>
            {
                entity.HasKey(e => new { e.idPrenda, e.idUsuario, e.idEntidad });

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblPrendaNUsuarioNEntidad)
                    .HasForeignKey(d => d.idEntidad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaNUsuarioNEntidad_tblEntidad");

                entity.HasOne(d => d.idPrendaNavigation)
                    .WithMany(p => p.tblPrendaNUsuarioNEntidad)
                    .HasForeignKey(d => d.idPrenda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaNUsuarioNEntidad_tblPrenda");

                entity.HasOne(d => d.idUsuarioNavigation)
                    .WithMany(p => p.tblPrendaNUsuarioNEntidad)
                    .HasForeignKey(d => d.idUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaNUsuarioNEntidad_tblUsuario");
            });

            modelBuilder.Entity<tblPrendaPrecioRefact>(entity =>
            {
                entity.Property(e => e.idPrenda).ValueGeneratedNever();

                entity.HasOne(d => d.idPrendaNavigation)
                    .WithOne(p => p.tblPrendaPrecioRefact)
                    .HasForeignKey<tblPrendaPrecioRefact>(d => d.idPrenda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendaPrecioRefact_tblPrenda");
            });

            modelBuilder.Entity<tblPrenda_historico_fechaValoracion>(entity =>
            {
                entity.HasKey(e => new { e.idPrenda, e.fecha });

                entity.HasOne(d => d.idPrendaNavigation)
                    .WithMany(p => p.tblPrenda_historico_fechaValoracion)
                    .HasForeignKey(d => d.idPrenda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrenda_historico_fechaValoracion_tblPrenda");
            });

            modelBuilder.Entity<tblPrenda_historico_idTipoFacturacion>(entity =>
            {
                entity.HasKey(e => new { e.idPrenda, e.fecha });

                entity.HasOne(d => d.idPrendaNavigation)
                    .WithMany(p => p.tblPrenda_historico_idTipoFacturacion)
                    .HasForeignKey(d => d.idPrenda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrenda_historico_idTipoFacturacion_tblPrenda");

                entity.HasOne(d => d.idTipoFacturacionNavigation)
                    .WithMany(p => p.tblPrenda_historico_idTipoFacturacion)
                    .HasForeignKey(d => d.idTipoFacturacion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrenda_historico_idTipoFacturacion_tblTipoFacturacion");
            });

            modelBuilder.Entity<tblPrenda_historico_peso>(entity =>
            {
                entity.HasKey(e => new { e.idPrenda, e.fecha });

                entity.HasOne(d => d.idPrendaNavigation)
                    .WithMany(p => p.tblPrenda_historico_peso)
                    .HasForeignKey(d => d.idPrenda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrenda_historico_peso_tblPrenda");
            });

            modelBuilder.Entity<tblPrendasHora>(entity =>
            {
                entity.HasOne(d => d.idFamiliaNavigation)
                    .WithMany(p => p.tblPrendasHora)
                    .HasForeignKey(d => d.idFamilia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendasHora_tblFamilia");

                entity.HasOne(d => d.idMaquinaNavigation)
                    .WithMany(p => p.tblPrendasHora)
                    .HasForeignKey(d => d.idMaquina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrendasHora_tblMaquina");

                entity.HasOne(d => d.idTipoPrendaNavigation)
                    .WithMany(p => p.tblPrendasHora)
                    .HasForeignKey(d => d.idTipoPrenda)
                    .HasConstraintName("FK_tblPrendasHora_tblTipoPrenda");
            });

            modelBuilder.Entity<tblPresupuestoKg>(entity =>
            {
                entity.HasOne(d => d.idAdmElementoPEPNavigation)
                    .WithMany(p => p.tblPresupuestoKg)
                    .HasForeignKey(d => d.idAdmElementoPEP)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPresupuestoKg_tblAdmElementoPEP");
            });

            modelBuilder.Entity<tblProduccion>(entity =>
            {
                entity.HasOne(d => d.idCompañiaNavigation)
                    .WithMany(p => p.tblProduccion)
                    .HasForeignKey(d => d.idCompañia)
                    .HasConstraintName("FK_tblProduccion_tblCompañia");

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblProduccion)
                    .HasForeignKey(d => d.idEntidad)
                    .HasConstraintName("FK_tblProduccion_tblEntidad");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblProduccion)
                    .HasForeignKey(d => d.idLavanderia)
                    .HasConstraintName("FK_tblProduccion_tblLavanderia");

                entity.HasOne(d => d.idMaquinaNavigation)
                    .WithMany(p => p.tblProduccion)
                    .HasForeignKey(d => d.idMaquina)
                    .HasConstraintName("FK_tblProduccion_tblMaquina");

                entity.HasOne(d => d.idTipoProduccionNavigation)
                    .WithMany(p => p.tblProduccion)
                    .HasForeignKey(d => d.idTipoProduccion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblProduccion_tblTipoProduccion");

                entity.HasOne(d => d.idTurnoNavigation)
                    .WithMany(p => p.tblProduccion)
                    .HasForeignKey(d => d.idTurno)
                    .HasConstraintName("FK_tblProduccion_tblTurno");
            });

            modelBuilder.Entity<tblProduccionMaquinaNCliente>(entity =>
            {
                entity.HasOne(d => d.idCompañiaNavigation)
                    .WithMany(p => p.tblProduccionMaquinaNCliente)
                    .HasForeignKey(d => d.idCompañia)
                    .HasConstraintName("FK_tblProduccionMaquinaNCliente_tblCompañia");

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblProduccionMaquinaNCliente)
                    .HasForeignKey(d => d.idEntidad)
                    .HasConstraintName("FK_tblProduccionMaquinaNCliente_tblEntidad");

                entity.HasOne(d => d.idMaquinaNavigation)
                    .WithMany(p => p.tblProduccionMaquinaNCliente)
                    .HasForeignKey(d => d.idMaquina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblProduccionMaquinaNCliente_tblMaquina");
            });

            modelBuilder.Entity<tblProduccionMaquinaNPrenda>(entity =>
            {
                entity.HasKey(e => new { e.idPrenda, e.idMaquina });

                entity.HasOne(d => d.idMaquinaNavigation)
                    .WithMany(p => p.tblProduccionMaquinaNPrenda)
                    .HasForeignKey(d => d.idMaquina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblProduccionMaquinaNPrenda_tblMaquina");

                entity.HasOne(d => d.idPrendaNavigation)
                    .WithMany(p => p.tblProduccionMaquinaNPrenda)
                    .HasForeignKey(d => d.idPrenda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblProduccionMaquinaNPrenda_tblPrenda");
            });

            modelBuilder.Entity<tblProgramasLavadora>(entity =>
            {
                entity.Property(e => e.idPrograma).ValueGeneratedNever();
            });

            modelBuilder.Entity<tblProveedor>(entity =>
            {
                entity.Property(e => e.activo).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.idPaisNavigation)
                    .WithMany(p => p.tblProveedor)
                    .HasForeignKey(d => d.idPais)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblProveedor_tblPais");
            });

            modelBuilder.Entity<tblPuerto>(entity =>
            {
                entity.HasOne(d => d.idPaisNavigation)
                    .WithMany(p => p.tblPuerto)
                    .HasForeignKey(d => d.idPais)
                    .HasConstraintName("FK_tblPuerto_tblPais");
            });

            modelBuilder.Entity<tblRecambio>(entity =>
            {
                entity.Property(e => e.activo).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.idProveedorNavigation)
                    .WithMany(p => p.tblRecambio)
                    .HasForeignKey(d => d.idProveedor)
                    .HasConstraintName("FK_tblRecambio_tblProveedor");
            });

            modelBuilder.Entity<tblRecambioNAlmacenRecambios>(entity =>
            {
                entity.HasKey(e => new { e.idAlmacen, e.idRecambio });

                entity.HasOne(d => d.idAlmacenNavigation)
                    .WithMany(p => p.tblRecambioNAlmacenRecambios)
                    .HasForeignKey(d => d.idAlmacen)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRecambioNAlmacenRecambios_tblAlmacenRecambios");

                entity.HasOne(d => d.idRecambioNavigation)
                    .WithMany(p => p.tblRecambioNAlmacenRecambios)
                    .HasForeignKey(d => d.idRecambio)
                    .HasConstraintName("FK_tblRecambioNAlmacenRecambios_tblRecambio");
            });

            modelBuilder.Entity<tblRecambioNMovimientoRecambio>(entity =>
            {
                entity.HasOne(d => d.idMovimientoRecambioNavigation)
                    .WithMany(p => p.tblRecambioNMovimientoRecambio)
                    .HasForeignKey(d => d.idMovimientoRecambio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRecambioNMovimientoRecambio_tblMovimientoRecambio");

                entity.HasOne(d => d.idRecambioNavigation)
                    .WithMany(p => p.tblRecambioNMovimientoRecambio)
                    .HasForeignKey(d => d.idRecambio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRecambioNMovimientoRecambio_tblRecambio");

                entity.HasOne(d => d.idRecambioNMovimientoRecambioAsociadoNavigation)
                    .WithMany(p => p.InverseidRecambioNMovimientoRecambioAsociadoNavigation)
                    .HasForeignKey(d => d.idRecambioNMovimientoRecambioAsociado)
                    .HasConstraintName("FK_tblRecambioNMovimientoRecambio_tblRecambioNMovimientoRecambio");

                entity.HasOne(d => d.idUsuarioNavigation)
                    .WithMany(p => p.tblRecambioNMovimientoRecambio)
                    .HasForeignKey(d => d.idUsuario)
                    .HasConstraintName("FK_tblRecambioNMovimientoRecambio_tblUsuario");
            });

            modelBuilder.Entity<tblRecambioNParteTrabajo>(entity =>
            {
                entity.HasKey(e => new { e.idRecambio, e.idParteTrabajo, e.idAlmacen });

                entity.HasOne(d => d.idAlmacenNavigation)
                    .WithMany(p => p.tblRecambioNParteTrabajo)
                    .HasForeignKey(d => d.idAlmacen)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRecambioNParteTrabajo_tblAlmacenRecambios");

                entity.HasOne(d => d.idParteTrabajoNavigation)
                    .WithMany(p => p.tblRecambioNParteTrabajo)
                    .HasForeignKey(d => d.idParteTrabajo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRecambioNParteTrabajo_tblParteTrabajo");

                entity.HasOne(d => d.idRecambioNavigation)
                    .WithMany(p => p.tblRecambioNParteTrabajo)
                    .HasForeignKey(d => d.idRecambio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRecambioNParteTrabajo_tblRecambio");
            });

            modelBuilder.Entity<tblRecambioNParteTrabajoIBS>(entity =>
            {
                entity.HasKey(e => e.idRecambioNParteTrabajo)
                    .HasName("PK__tblRecam__8C047A52A9B23FA1");

                entity.HasOne(d => d.idParteTrabajoNavigation)
                    .WithMany(p => p.tblRecambioNParteTrabajoIBS)
                    .HasForeignKey(d => d.idParteTrabajo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRecambioNParteTrabajoIBS_tblParteTrabajo");
            });

            modelBuilder.Entity<tblRecambioNProveedor>(entity =>
            {
                entity.Property(e => e.activo).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.idPaisNavigation)
                    .WithMany(p => p.tblRecambioNProveedor)
                    .HasForeignKey(d => d.idPais)
                    .HasConstraintName("FK_tblRecambioNProveedor_tblPais");

                entity.HasOne(d => d.idProveedorNavigation)
                    .WithMany(p => p.tblRecambioNProveedor)
                    .HasForeignKey(d => d.idProveedor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRecambioNProveedor_tblProveedor");

                entity.HasOne(d => d.idRecambioNavigation)
                    .WithMany(p => p.tblRecambioNProveedor)
                    .HasForeignKey(d => d.idRecambio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRecambioNProveedor_tblRecambio");
            });

            modelBuilder.Entity<tblRechazoNProduccion>(entity =>
            {
                entity.HasOne(d => d.idPrendaNProduccionNavigation)
                    .WithMany(p => p.tblRechazoNProduccion)
                    .HasForeignKey(d => d.idPrendaNProduccion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRechazoNProduccion_tblPrendaNProduccion");

                entity.HasOne(d => d.idTipoRechazoNavigation)
                    .WithMany(p => p.tblRechazoNProduccion)
                    .HasForeignKey(d => d.idTipoRechazo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRechazoNProduccion_tblTipoRechazo");
            });

            modelBuilder.Entity<tblRecoveryPassword>(entity =>
            {
                entity.HasKey(e => new { e.idUsuario, e.fecha });

                entity.HasOne(d => d.idUsuarioNavigation)
                    .WithMany(p => p.tblRecoveryPassword)
                    .HasForeignKey(d => d.idUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRecoveryPassword_tblUsuario");
            });

            modelBuilder.Entity<tblRecursoContador>(entity =>
            {
                entity.Property(e => e.factorConversion).HasDefaultValueSql("((1))");

                entity.Property(e => e.valorPulsoEnergyHub).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.idCategoriaRecursoNavigation)
                    .WithMany(p => p.tblRecursoContador)
                    .HasForeignKey(d => d.idCategoriaRecurso)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRecursoContador_tblCategoriaRecurso");

                entity.HasOne(d => d.idGrupoEnergeticoNavigation)
                    .WithMany(p => p.tblRecursoContador)
                    .HasForeignKey(d => d.idGrupoEnergetico)
                    .HasConstraintName("FK_tblRecursoContador_tblGrupoEnergetico");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblRecursoContador)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRecursoContador_tblLavanderia");

                entity.HasOne(d => d.idUnidadMedida_ContadorNavigation)
                    .WithMany(p => p.tblRecursoContadoridUnidadMedida_ContadorNavigation)
                    .HasForeignKey(d => d.idUnidadMedida_Contador)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRecursoContador_tblUnidadMedida_Contador");

                entity.HasOne(d => d.idUnidadMedida_KpiNavigation)
                    .WithMany(p => p.tblRecursoContadoridUnidadMedida_KpiNavigation)
                    .HasForeignKey(d => d.idUnidadMedida_Kpi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRecursoContador_tblUnidadMedida_Kpi");
            });

            modelBuilder.Entity<tblRecursoNivel>(entity =>
            {
                entity.Property(e => e.idCategoriaRecurso).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblRecursoNivel)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRecursoNivel_tblLavanderia");

                entity.HasOne(d => d.idTipoRecursoNivelNavigation)
                    .WithMany(p => p.tblRecursoNivel)
                    .HasForeignKey(d => d.idTipoRecursoNivel)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRecursoNivel_tblTipoRecursoNivel");

                entity.HasOne(d => d.idUnidadMedidaNavigation)
                    .WithMany(p => p.tblRecursoNivelidUnidadMedidaNavigation)
                    .HasForeignKey(d => d.idUnidadMedida)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRecursoNivel_tblUnidadMedida");

                entity.HasOne(d => d.idUnidadMedidaInformeNavigation)
                    .WithMany(p => p.tblRecursoNivelidUnidadMedidaInformeNavigation)
                    .HasForeignKey(d => d.idUnidadMedidaInforme)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRecursoNivel_tblUnidadMedida1");
            });

            modelBuilder.Entity<tblRecursoVirtual_Calculo>(entity =>
            {
                entity.HasKey(e => new { e.idRecursoVirtual, e.idRecursoContador })
                    .HasName("PK__tblRecur__D900E6C0C826CF49");

                entity.HasOne(d => d.idRecursoContadorNavigation)
                    .WithMany(p => p.tblRecursoVirtual_CalculoidRecursoContadorNavigation)
                    .HasForeignKey(d => d.idRecursoContador)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblRecurs__idRec__2C3F4C1F");

                entity.HasOne(d => d.idRecursoVirtualNavigation)
                    .WithMany(p => p.tblRecursoVirtual_CalculoidRecursoVirtualNavigation)
                    .HasForeignKey(d => d.idRecursoVirtual)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblRecurs__idRec__2B4B27E6");
            });

            modelBuilder.Entity<tblReparto>(entity =>
            {
                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblReparto)
                    .HasForeignKey(d => d.idEntidad)
                    .HasConstraintName("FK_tblReparto_tblEntidad");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblReparto)
                    .HasForeignKey(d => d.idLavanderia)
                    .HasConstraintName("FK_tblReparto_tblLavanderia");

                entity.HasOne(d => d.idMovimientoElemLogNavigation)
                    .WithMany(p => p.tblReparto)
                    .HasForeignKey(d => d.idMovimientoElemLog)
                    .HasConstraintName("FK_tblReparto_tblMovimientoElemLog");

                entity.HasOne(d => d.idPedidoNavigation)
                    .WithMany(p => p.tblReparto)
                    .HasForeignKey(d => d.idPedido)
                    .HasConstraintName("FK_tblReparto_tblPedido");

                entity.HasOne(d => d.idRepartoEstadoNavigation)
                    .WithMany(p => p.tblReparto)
                    .HasForeignKey(d => d.idRepartoEstado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblReparto_tblRepartoEstado");

                entity.HasOne(d => d.idTipoProduccionNavigation)
                    .WithMany(p => p.tblReparto)
                    .HasForeignKey(d => d.idTipoProduccion)
                    .HasConstraintName("FK_tblReparto_tblTipoProduccion");

                entity.HasOne(d => d.idUsuarioPreparaNavigation)
                    .WithMany(p => p.tblReparto)
                    .HasForeignKey(d => d.idUsuarioPrepara)
                    .HasConstraintName("FK_tblReparto_tblUsuario");

                entity.HasMany(d => d.idSalidaReparto)
                    .WithMany(p => p.idReparto)
                    .UsingEntity<Dictionary<string, object>>(
                        "tblRepartosNSalida",
                        l => l.HasOne<tblSalidaReparto>().WithMany().HasForeignKey("idSalidaReparto").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblRepartosNSalida_tblSalidaReparto"),
                        r => r.HasOne<tblReparto>().WithMany().HasForeignKey("idReparto").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblRepartosNSalida_tblReparto"),
                        j =>
                        {
                            j.HasKey("idReparto", "idSalidaReparto").HasName("PK__tblRepar__2321F91A5032CF2B");

                            j.ToTable("tblRepartosNSalida", "Logistica");
                        });
            });

            modelBuilder.Entity<tblRepartoEstado>(entity =>
            {
                entity.HasKey(e => e.idRepartoEstado)
                    .HasName("PK__tblRepar__9AA35B440DE7789F");
            });

            modelBuilder.Entity<tblRepartoNParteTransporte>(entity =>
            {
                entity.HasKey(e => new { e.idReparto, e.idParteTransporte });

                entity.HasOne(d => d.idParteTransporteNavigation)
                    .WithMany(p => p.tblRepartoNParteTransporte)
                    .HasForeignKey(d => d.idParteTransporte)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRepartoNParteTransporte_tblParteTransporte");

                entity.HasOne(d => d.idRepartoNavigation)
                    .WithMany(p => p.tblRepartoNParteTransporte)
                    .HasForeignKey(d => d.idReparto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRepartoNParteTransporte_tblReparto");
            });

            modelBuilder.Entity<tblRepartoOffice>(entity =>
            {
                entity.Property(e => e.isApp).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.idArchivo_firmaNavigation)
                    .WithMany(p => p.tblRepartoOffice)
                    .HasForeignKey(d => d.idArchivo_firma)
                    .HasConstraintName("FK_tblRepartoOffice_tblArchivo");

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblRepartoOffice)
                    .HasForeignKey(d => d.idEntidad)
                    .HasConstraintName("FK_tblRepartoOffice_tblEntidad");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblRepartoOffice)
                    .HasForeignKey(d => d.idLavanderia)
                    .HasConstraintName("FK_tblRepartoOffice_tblLavanderia");

                entity.HasOne(d => d.idPedidoExtraNavigation)
                    .WithMany(p => p.tblRepartoOffice)
                    .HasForeignKey(d => d.idPedidoExtra)
                    .HasConstraintName("FK_tblRepartoOffice_tblPedidosExtra");

                entity.HasOne(d => d.idPersonaNavigation)
                    .WithMany(p => p.tblRepartoOffice)
                    .HasForeignKey(d => d.idPersona)
                    .HasConstraintName("FK_tblRepartoOffice_tblPersona");

                entity.HasOne(d => d.idRevisionNavigation)
                    .WithMany(p => p.tblRepartoOffice)
                    .HasForeignKey(d => d.idRevision)
                    .HasConstraintName("FK_tblRepartoOffice_tblRevision");
            });

            modelBuilder.Entity<tblRepartosValet>(entity =>
            {
                entity.HasKey(e => e.idRepartoValet)
                    .HasName("PK_tblRepartosHuesped");

                entity.Property(e => e.idTipoRepartoValet).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblRepartosValet)
                    .HasForeignKey(d => d.idEntidad)
                    .HasConstraintName("FK_tblRepartosHuesped_tblEntidad");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblRepartosValet)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRepartosHuesped_tblLavanderia");
            });

            modelBuilder.Entity<tblReports>(entity =>
            {
                entity.HasOne(d => d.idFormularioNavigation)
                    .WithMany(p => p.tblReports)
                    .HasForeignKey(d => d.idFormulario)
                    .HasConstraintName("FK_tblReports_tblFormulario");
            });

            modelBuilder.Entity<tblRespuesta>(entity =>
            {
                entity.HasOne(d => d.idCampañaEncuestaNavigation)
                    .WithMany(p => p.tblRespuesta)
                    .HasForeignKey(d => d.idCampañaEncuesta)
                    .HasConstraintName("FK_tblRespuesta_tblCampañaEncuesta");

                entity.HasOne(d => d.idEncuestaNavigation)
                    .WithMany(p => p.tblRespuesta)
                    .HasForeignKey(d => d.idEncuesta)
                    .HasConstraintName("FK_tblRespuesta_tblEncuesta");

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblRespuesta)
                    .HasForeignKey(d => d.idEntidad)
                    .HasConstraintName("FK_tblRespuesta_tblEntidad");

                entity.HasOne(d => d.idOpcionNavigation)
                    .WithMany(p => p.tblRespuesta)
                    .HasForeignKey(d => d.idOpcion)
                    .HasConstraintName("FK_tblRespuesta_tblOpcion");

                entity.HasOne(d => d.idPreguntaNavigation)
                    .WithMany(p => p.tblRespuesta)
                    .HasForeignKey(d => d.idPregunta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRespuesta_tblPregunta");

                entity.HasOne(d => d.idUsuarioNavigation)
                    .WithMany(p => p.tblRespuesta)
                    .HasForeignKey(d => d.idUsuario)
                    .HasConstraintName("FK_tblRespuesta_tblUsuario");
            });

            modelBuilder.Entity<tblReunion>(entity =>
            {
                entity.HasOne(d => d.idCompañiaNavigation)
                    .WithMany(p => p.tblReunion)
                    .HasForeignKey(d => d.idCompañia)
                    .HasConstraintName("FK_tblReunion_tblCompañia");

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblReunion)
                    .HasForeignKey(d => d.idEntidad)
                    .HasConstraintName("FK_tblReunion_tblEntidad");

                entity.HasOne(d => d.idFormatoReunionNavigation)
                    .WithMany(p => p.tblReunion)
                    .HasForeignKey(d => d.idFormatoReunion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblReunion_tblFormatoReunion");

                entity.HasOne(d => d.idTipoReunionNavigation)
                    .WithMany(p => p.tblReunion)
                    .HasForeignKey(d => d.idTipoReunion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblReunion_tblTipoReunion");

                entity.HasOne(d => d.idUsuarioNavigation)
                    .WithMany(p => p.tblReunion)
                    .HasForeignKey(d => d.idUsuario)
                    .HasConstraintName("FK_tblReunion_tblUsuario");
            });

            modelBuilder.Entity<tblRevision>(entity =>
            {
                entity.HasOne(d => d.idAlmacenNavigation)
                    .WithMany(p => p.tblRevision)
                    .HasForeignKey(d => d.idAlmacen)
                    .HasConstraintName("FK_Table_1_tblAlmacen");

                entity.HasOne(d => d.idEstadoOfficeNavigation)
                    .WithMany(p => p.tblRevision)
                    .HasForeignKey(d => d.idEstadoOffice)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Revision_tblEstadoOffice");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblRevision)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblLavanderia_tblRevision");

                entity.HasOne(d => d.idPersonaNavigation)
                    .WithMany(p => p.tblRevision)
                    .HasForeignKey(d => d.idPersona)
                    .HasConstraintName("FK_Revision_tblPersona");

                entity.HasOne(d => d.idRutaNavigation)
                    .WithMany(p => p.tblRevision)
                    .HasForeignKey(d => d.idRuta)
                    .HasConstraintName("FK_tblRevision_tblRuta");

                entity.HasOne(d => d.idSubAlmacenNavigation)
                    .WithMany(p => p.tblRevision)
                    .HasForeignKey(d => d.idSubAlmacen)
                    .HasConstraintName("FK_Revision_tblSubAlmacen");
            });

            modelBuilder.Entity<tblRevisionVehiculo>(entity =>
            {
                entity.HasKey(e => e.idRevisionVehiculo)
                    .HasName("PK__tblRevis__90F3BC90772FAAD9");
            });

            modelBuilder.Entity<tblRevisionVehiculoNParteTransporte>(entity =>
            {
                entity.HasKey(e => new { e.idRevisionVehiculo, e.idParteTransporte })
                    .HasName("PK__tblRevis__1A6F800C4CE7E844");

                entity.HasOne(d => d.idParteTransporteNavigation)
                    .WithMany(p => p.tblRevisionVehiculoNParteTransporte)
                    .HasForeignKey(d => d.idParteTransporte)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRevisionVehiculoNParteTransporte_idParteTransporte");

                entity.HasOne(d => d.idRevisionVehiculoNavigation)
                    .WithMany(p => p.tblRevisionVehiculoNParteTransporte)
                    .HasForeignKey(d => d.idRevisionVehiculo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRevisionVehiculoNParteTransporte_idRevisionVehiculo");
            });

            modelBuilder.Entity<tblRuta>(entity =>
            {
                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblRuta)
                    .HasForeignKey(d => d.idEntidad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRuta_tblEntidad");
            });

            modelBuilder.Entity<tblRutaExpedicion>(entity =>
            {
                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblRutaExpedicion)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRutaExpedicion_tblLavanderia");
            });

            modelBuilder.Entity<tblRutaNParteTransporte>(entity =>
            {
                entity.HasKey(e => e.idRutaNParteTransporte)
                    .HasName("PK_idRutaNParteTransporte");

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblRutaNParteTransporte)
                    .HasForeignKey(d => d.idEntidad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRutaNParteTransporte_idEntidad");

                entity.HasOne(d => d.idParteTransporteNavigation)
                    .WithMany(p => p.tblRutaNParteTransporte)
                    .HasForeignKey(d => d.idParteTransporte)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRutaNParteTransporte_idParteTransporte");

                entity.HasOne(d => d.idRepartoNavigation)
                    .WithMany(p => p.tblRutaNParteTransporte)
                    .HasForeignKey(d => d.idReparto)
                    .HasConstraintName("FK_tblRutaNParteTransporte_idReparto");
            });

            modelBuilder.Entity<tblRutaSeccion>(entity =>
            {
                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblRutaSeccion)
                    .HasForeignKey(d => d.idEntidad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRutaSeccion_tblEntidad");
            });

            modelBuilder.Entity<tblSacasPendientes>(entity =>
            {
                entity.HasOne(d => d.idColorTapaNavigation)
                    .WithMany(p => p.tblSacasPendientes)
                    .HasForeignKey(d => d.idColorTapa)
                    .HasConstraintName("FK_tblSacasPendientes_tblColorTapa");

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblSacasPendientes)
                    .HasForeignKey(d => d.idEntidad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSacasPendientes_tblEntidad");
            });

            modelBuilder.Entity<tblSalidaReparto>(entity =>
            {
                entity.HasKey(e => e.idSalidaReparto)
                    .HasName("PK_salidaReparto");

                entity.HasOne(d => d.idConductorNavigation)
                    .WithMany(p => p.tblSalidaRepartoidConductorNavigation)
                    .HasForeignKey(d => d.idConductor)
                    .HasConstraintName("FK_tblSalidaReparto_tblPersona");

                entity.HasOne(d => d.idEstibador1Navigation)
                    .WithMany(p => p.tblSalidaRepartoidEstibador1Navigation)
                    .HasForeignKey(d => d.idEstibador1)
                    .HasConstraintName("FK_tblSalidaReparto_tblPersona1");

                entity.HasOne(d => d.idEstibador2Navigation)
                    .WithMany(p => p.tblSalidaRepartoidEstibador2Navigation)
                    .HasForeignKey(d => d.idEstibador2)
                    .HasConstraintName("FK_tblSalidaReparto_tblPersona2");

                entity.HasOne(d => d.idVehiculoNavigation)
                    .WithMany(p => p.tblSalidaReparto)
                    .HasForeignKey(d => d.idVehiculo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSalidaReparto_tblVehiculos");
            });

            modelBuilder.Entity<tblSeccionNivel1>(entity =>
            {
                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblSeccionNivel1)
                    .HasForeignKey(d => d.idEntidad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSeccionNivel1_tblEntidad");
            });

            modelBuilder.Entity<tblSeccionNivel2>(entity =>
            {
                entity.HasOne(d => d.idSeccionNivel1Navigation)
                    .WithMany(p => p.tblSeccionNivel2)
                    .HasForeignKey(d => d.idSeccionNivel1)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSeccionNivel2_tblSeccionNivel1");
            });

            modelBuilder.Entity<tblServicioExternoNParteTrabajo>(entity =>
            {
                entity.HasOne(d => d.idParteTrabajoNavigation)
                    .WithMany(p => p.tblServicioExternoNParteTrabajo)
                    .HasForeignKey(d => d.idParteTrabajo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblServicioExternoNParteTrabajo_tblParteTrabajo");
            });

            modelBuilder.Entity<tblSolicitudAbono>(entity =>
            {
                entity.Property(e => e.idEstadoSolicitudAbono).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.idAbonoNavigation)
                    .WithMany(p => p.tblSolicitudAbono)
                    .HasForeignKey(d => d.idAbono)
                    .HasConstraintName("FK_tblSolicitudAbono_tblAbono");

                entity.HasOne(d => d.idCategoriaAbonoNavigation)
                    .WithMany(p => p.tblSolicitudAbono)
                    .HasForeignKey(d => d.idCategoriaAbono)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSolicitudAbono_tblCategoriaAbono");

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblSolicitudAbono)
                    .HasForeignKey(d => d.idEntidad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSolicitudAbono_tblEntidad");

                entity.HasOne(d => d.idEstadoSolicitudAbonoNavigation)
                    .WithMany(p => p.tblSolicitudAbono)
                    .HasForeignKey(d => d.idEstadoSolicitudAbono)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSolicitudAbono_tblEstadoSolicitudAbono");

                entity.HasOne(d => d.idUsuarioNavigation)
                    .WithMany(p => p.tblSolicitudAbono)
                    .HasForeignKey(d => d.idUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSolicitudAbono_tblUsuario");
            });

            modelBuilder.Entity<tblSolicitudAlta>(entity =>
            {
                entity.Property(e => e.idSolicitudAlta).ValueGeneratedNever();

                entity.Property(e => e.idEstadoSolicitudAlta).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.idEstadoSolicitudAltaNavigation)
                    .WithMany(p => p.tblSolicitudAlta)
                    .HasForeignKey(d => d.idEstadoSolicitudAlta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSolicitudAlta_tblEstadoSolicitudAlta");

                entity.HasOne(d => d.idSolicitudAltaNavigation)
                    .WithOne(p => p.tblSolicitudAlta)
                    .HasForeignKey<tblSolicitudAlta>(d => d.idSolicitudAlta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSolicitudAlta_tblLlamamiento");

                entity.HasOne(d => d.idUsuario_validacionNavigation)
                    .WithMany(p => p.tblSolicitudAlta)
                    .HasForeignKey(d => d.idUsuario_validacion)
                    .HasConstraintName("FK_tblSolicitudAlta_tblUsuario");
            });

            modelBuilder.Entity<tblStockMinimoNAlmacenRecambios>(entity =>
            {
                entity.HasKey(e => new { e.idAlmacen, e.idRecambio });

                entity.HasOne(d => d.idAlmacenNavigation)
                    .WithMany(p => p.tblStockMinimoNAlmacenRecambios)
                    .HasForeignKey(d => d.idAlmacen)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblStockMinimoNAlmacenRecambios_tblAlmacenRecambios");

                entity.HasOne(d => d.idRecambioNavigation)
                    .WithMany(p => p.tblStockMinimoNAlmacenRecambios)
                    .HasForeignKey(d => d.idRecambio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblStockMinimoNAlmacenRecambios_tblRecambio");
            });

            modelBuilder.Entity<tblStockTipoElemLogNEntidad>(entity =>
            {
                entity.HasKey(e => new { e.idEntidad, e.idTipoElemLog })
                    .HasName("PK_tblStockTipoElemLogNEntidad_1");

                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblStockTipoElemLogNEntidad)
                    .HasForeignKey(d => d.idEntidad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblStockTipoElemLogNEntidad_tblEntidad");

                entity.HasOne(d => d.idTipoElemLogNavigation)
                    .WithMany(p => p.tblStockTipoElemLogNEntidad)
                    .HasForeignKey(d => d.idTipoElemLog)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblStockTipoElemLogNEntidad_tblTipoElemLog");
            });

            modelBuilder.Entity<tblSubAlmacen>(entity =>
            {
                entity.HasOne(d => d.idAlmacenNavigation)
                    .WithMany(p => p.tblSubAlmacen)
                    .HasForeignKey(d => d.idAlmacen)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSubAlmacen_tblAlmacen");
            });

            modelBuilder.Entity<tblSubTurno>(entity =>
            {
                entity.HasKey(e => e.idSubTurno)
                    .HasName("PK_tblSubTurno2");

                entity.HasOne(d => d.idTurnoNavigation)
                    .WithMany(p => p.tblSubTurno)
                    .HasForeignKey(d => d.idTurno)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSubTurno_tblTurno");
            });

            modelBuilder.Entity<tblTag>(entity =>
            {
                entity.HasOne(d => d.idEstadoNavigation)
                    .WithMany(p => p.tblTag)
                    .HasForeignKey(d => d.idEstado)
                    .HasConstraintName("FK_tblTag_tblEstadoTag");
            });

            modelBuilder.Entity<tblTaquilla>(entity =>
            {
                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblTaquilla)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTaquilla_tblLavanderia");
            });

            modelBuilder.Entity<tblTaquilla_estado>(entity =>
            {
                entity.HasKey(e => new { e.idTaquilla, e.numPosicion });

                entity.HasOne(d => d.idTaquillaNavigation)
                    .WithMany(p => p.tblTaquilla_estado)
                    .HasForeignKey(d => d.idTaquilla)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTaquilla_estado_tblTaquilla");

                entity.HasOne(d => d.idVehiculoNavigation)
                    .WithMany(p => p.tblTaquilla_estado)
                    .HasForeignKey(d => d.idVehiculo)
                    .HasConstraintName("FK_tblTaquilla_estado_tblVehiculo");
            });

            modelBuilder.Entity<tblTaquilla_movimiento>(entity =>
            {
                entity.HasOne(d => d.idPersonaNavigation)
                    .WithMany(p => p.tblTaquilla_movimiento)
                    .HasForeignKey(d => d.idPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTaquilla_movimiento_tblPersona");

                entity.HasOne(d => d.idTaquillaNavigation)
                    .WithMany(p => p.tblTaquilla_movimiento)
                    .HasForeignKey(d => d.idTaquilla)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTaquilla_movimiento_tblTaquilla");

                entity.HasOne(d => d.idVehiculoNavigation)
                    .WithMany(p => p.tblTaquilla_movimiento)
                    .HasForeignKey(d => d.idVehiculo)
                    .HasConstraintName("FK_tblTaquilla_movimiento_tblVehiculo");
            });

            modelBuilder.Entity<tblTaquillas_Estado_prueba>(entity =>
            {
                entity.HasOne(d => d.idTaquillaNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.idTaquilla)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblTaquil__idTaq__070E92C7");

                entity.HasOne(d => d.idVehiculoNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.idVehiculo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblTaquil__idVeh__0802B700");
            });

            modelBuilder.Entity<tblTaquillas_Movimiento_prueba>(entity =>
            {
                entity.HasKey(e => e.idMovimiento)
                    .HasName("PK__tblTaqui__6285217382A888D9");

                entity.HasOne(d => d.idPersonaNavigation)
                    .WithMany(p => p.tblTaquillas_Movimiento_prueba)
                    .HasForeignKey(d => d.idPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblTaquil__idPer__0249DDAA");

                entity.HasOne(d => d.idTaquillaNavigation)
                    .WithMany(p => p.tblTaquillas_Movimiento_prueba)
                    .HasForeignKey(d => d.idTaquilla)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblTaquil__idTaq__00619538");

                entity.HasOne(d => d.idVehiculoNavigation)
                    .WithMany(p => p.tblTaquillas_Movimiento_prueba)
                    .HasForeignKey(d => d.idVehiculo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblTaquil__idVeh__0155B971");
            });

            modelBuilder.Entity<tblTaquillas_prueba>(entity =>
            {
                entity.HasKey(e => e.idTaquilla)
                    .HasName("PK__tblTaqui__03ADAA5AF9016604");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblTaquillas_prueba)
                    .HasForeignKey(d => d.idLavanderia)
                    .HasConstraintName("FK__tblTaquil__idLav__75E406C5");
            });

            modelBuilder.Entity<tblTareaMantenimientoPrev>(entity =>
            {
                entity.HasOne(d => d.idPlantillaTareaMantenimientoPrevNavigation)
                    .WithMany(p => p.tblTareaMantenimientoPrev)
                    .HasForeignKey(d => d.idPlantillaTareaMantenimientoPrev)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTareaMantenimientoPrev_tblPlantillaTareaMantenimientoPrev");
            });

            modelBuilder.Entity<tblTareaMaquina>(entity =>
            {
                entity.HasOne(d => d.idDiaSemanaNavigation)
                    .WithMany(p => p.tblTareaMaquina)
                    .HasForeignKey(d => d.idDiaSemana)
                    .HasConstraintName("FK_tblTareaMaquina_tblDiaSemana");

                entity.HasOne(d => d.idFrecuenciaMantenimientoNavigation)
                    .WithMany(p => p.tblTareaMaquina)
                    .HasForeignKey(d => d.idFrecuenciaMantenimiento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTareaMaquina_tblFrecuenciaMantenimiento");

                entity.HasOne(d => d.idMaquinaNavigation)
                    .WithMany(p => p.tblTareaMaquina)
                    .HasForeignKey(d => d.idMaquina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTareaMaquina_tblMaquina");

                entity.HasOne(d => d.idMesNavigation)
                    .WithMany(p => p.tblTareaMaquina)
                    .HasForeignKey(d => d.idMes)
                    .HasConstraintName("FK_tblTareaMaquina_tblMes");

                entity.HasOne(d => d.idResponsableTareaNavigation)
                    .WithMany(p => p.tblTareaMaquina)
                    .HasForeignKey(d => d.idResponsableTarea)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTareaMaquina_tblResponsableTarea");
            });

            modelBuilder.Entity<tblTareaPersonaDia>(entity =>
            {
                entity.HasKey(e => new { e.fecha, e.idTareaMaquina })
                    .HasName("PK__tblTarea__0D5D43A21D9F0900");

                entity.HasOne(d => d.idPersonaNavigation)
                    .WithMany(p => p.tblTareaPersonaDia)
                    .HasForeignKey(d => d.idPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTareaPersonaDia_tblPersona");

                entity.HasOne(d => d.idTareaMaquinaNavigation)
                    .WithMany(p => p.tblTareaPersonaDia)
                    .HasForeignKey(d => d.idTareaMaquina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTareaPersonaDia_tblTareaMaquina");
            });

            modelBuilder.Entity<tblTasaCambio>(entity =>
            {
                entity.HasKey(e => new { e.idMonedaOrigen, e.idMonedaDestino, e.fecha })
                    .HasName("PK__tblTasaC__24B4B75EBA9F0795");

                entity.HasOne(d => d.idMonedaDestinoNavigation)
                    .WithMany(p => p.tblTasaCambioidMonedaDestinoNavigation)
                    .HasForeignKey(d => d.idMonedaDestino)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblTasaCa__idMon__251E189B");

                entity.HasOne(d => d.idMonedaOrigenNavigation)
                    .WithMany(p => p.tblTasaCambioidMonedaOrigenNavigation)
                    .HasForeignKey(d => d.idMonedaOrigen)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblTasaCa__idMon__2429F462");
            });

            modelBuilder.Entity<tblTasaCambioPresupuesto>(entity =>
            {
                entity.HasKey(e => new { e.idMonedaDestino, e.año })
                    .HasName("PK__tblTasaC__910D2D813BFCDDF3");

                entity.HasOne(d => d.idMonedaDestinoNavigation)
                    .WithMany(p => p.tblTasaCambioPresupuesto)
                    .HasForeignKey(d => d.idMonedaDestino)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblTasaCa__idMon__44CBCE1E");
            });

            modelBuilder.Entity<tblTimbradoMXNAdmFacturaVenta>(entity =>
            {
                entity.Property(e => e.idAdmFacturaVenta).ValueGeneratedNever();

                entity.HasOne(d => d.idAdmFacturaVentaNavigation)
                    .WithOne(p => p.tblTimbradoMXNAdmFacturaVenta)
                    .HasForeignKey<tblTimbradoMXNAdmFacturaVenta>(d => d.idAdmFacturaVenta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTimbradoMXNAdmFacturaVenta_tblAdmFacturaVenta");
            });

            modelBuilder.Entity<tblTipoAbono>(entity =>
            {
                entity.HasKey(e => e.idTipoAbono)
                    .HasName("PK__tblTipoA__D65817008AC2398A");
            });

            modelBuilder.Entity<tblTipoAlmacen>(entity =>
            {
                entity.Property(e => e.idTipoAlmacen).ValueGeneratedNever();
            });

            modelBuilder.Entity<tblTipoConsumoLenceria>(entity =>
            {
                entity.HasKey(e => e.idTipoConsumoLenceria)
                    .HasName("PK__tblTipoC__7FFC1A1047CE618A");
            });

            modelBuilder.Entity<tblTipoDiaCuadrante>(entity =>
            {
                entity.Property(e => e.idTipoDiaCuadrante).ValueGeneratedNever();
            });

            modelBuilder.Entity<tblTipoDocumento>(entity =>
            {
                entity.Property(e => e.idTipoDocumento).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.idTraduccionNavigation)
                    .WithMany(p => p.tblTipoDocumento)
                    .HasForeignKey(d => d.idTraduccion)
                    .HasConstraintName("FK_tblTipoDocumento_tblTraduccion");
            });

            modelBuilder.Entity<tblTipoElemLog>(entity =>
            {
                entity.HasOne(d => d.idTraduccionNavigation)
                    .WithMany(p => p.tblTipoElemLog)
                    .HasForeignKey(d => d.idTraduccion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTipoElemLog_tblTraduccion");
            });

            modelBuilder.Entity<tblTipoEventoToken>(entity =>
            {
                entity.HasKey(e => e.idTipoEventoToken)
                    .HasName("PK__tblTipoE__2C5C52293404A75B");

                entity.Property(e => e.idTipoEventoToken).ValueGeneratedNever();
            });

            modelBuilder.Entity<tblTipoFacturacion>(entity =>
            {
                entity.HasKey(e => e.idTipoFacturacion)
                    .HasName("PK_idTipoFacturacion");
            });

            modelBuilder.Entity<tblTipoFueraServicio>(entity =>
            {
                entity.HasKey(e => e.idTipoFS)
                    .HasName("PK__tblTipoF__13EB8CD5E2699190");
            });

            modelBuilder.Entity<tblTipoHabitacion>(entity =>
            {
                entity.HasOne(d => d.idEntidadNavigation)
                    .WithMany(p => p.tblTipoHabitacion)
                    .HasForeignKey(d => d.idEntidad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTipoHabitacion_tblEntidad");
            });

            modelBuilder.Entity<tblTipoIncidencia>(entity =>
            {
                entity.Property(e => e.idTipoIncidencia).ValueGeneratedOnAdd();

                entity.HasOne(d => d.idTraduccionNavigation)
                    .WithMany(p => p.tblTipoIncidenciaidTraduccionNavigation)
                    .HasForeignKey(d => d.idTraduccion)
                    .HasConstraintName("FK_tblTipoIncidencia_tblTraduccion");

                entity.HasOne(d => d.idTraduccion_abrNavigation)
                    .WithMany(p => p.tblTipoIncidenciaidTraduccion_abrNavigation)
                    .HasForeignKey(d => d.idTraduccion_abr)
                    .HasConstraintName("FK_tblTipoIncidencia_tblTraduccion1");
            });

            modelBuilder.Entity<tblTipoKpi_Observaciones>(entity =>
            {
                entity.HasKey(e => new { e.idTipoKpi, e.idLavanderia });

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblTipoKpi_Observaciones)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTipoKpi_Observaciones_tblLavanderia");

                entity.HasOne(d => d.idTipoKpiNavigation)
                    .WithMany(p => p.tblTipoKpi_Observaciones)
                    .HasForeignKey(d => d.idTipoKpi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTipoKpi_Observaciones_tblTipoKpi");
            });

            modelBuilder.Entity<tblTipoLavado>(entity =>
            {
                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblTipoLavado)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTipoLavado_tblLavanderia");
            });

            modelBuilder.Entity<tblTipoLectura>(entity =>
            {
                entity.HasKey(e => e.idTipoLectura)
                    .HasName("PK__tblTipoL__E08D9009C41CBA0B");
            });

            modelBuilder.Entity<tblTipoMaquinaNCategoriaMaquina>(entity =>
            {
                entity.HasOne(d => d.idCategoriaMaquinaNavigation)
                    .WithMany(p => p.tblTipoMaquinaNCategoriaMaquina)
                    .HasForeignKey(d => d.idCategoriaMaquina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTipoMaquinaNCategoriaMaquina_tblCategoriaMaquina");

                entity.HasOne(d => d.idTipoMaquinaNavigation)
                    .WithMany(p => p.tblTipoMaquinaNCategoriaMaquina)
                    .HasForeignKey(d => d.idTipoMaquina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTipoMaquinaNCategoriaMaquina_tblTipoMaquina");
            });

            modelBuilder.Entity<tblTipoMovimiento>(entity =>
            {
                entity.Property(e => e.idTipoMovimiento).ValueGeneratedNever();

                entity.HasOne(d => d.idTraduccionNavigation)
                    .WithMany(p => p.tblTipoMovimiento)
                    .HasForeignKey(d => d.idTraduccion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTipoMovimiento_tblTraduccion");
            });

            modelBuilder.Entity<tblTipoMovimientoRecambio>(entity =>
            {
                entity.Property(e => e.idTipoMovimientoRecambio).ValueGeneratedNever();
            });

            modelBuilder.Entity<tblTipoMuestreo>(entity =>
            {
                entity.HasKey(e => e.idTipoMuestreo)
                    .HasName("PK__tblTipoM__366DFA5D59036D0F");
            });

            modelBuilder.Entity<tblTipoNomina>(entity =>
            {
                entity.Property(e => e.idTipoNomina).ValueGeneratedNever();

                entity.HasOne(d => d.idTraduccionNavigation)
                    .WithMany(p => p.tblTipoNomina)
                    .HasForeignKey(d => d.idTraduccion)
                    .HasConstraintName("FK_tblTipoNomina_tblTraduccion");
            });

            modelBuilder.Entity<tblTipoNomina_MX>(entity =>
            {
                entity.Property(e => e.idTipoNomina_MX).ValueGeneratedNever();

                entity.HasOne(d => d.idTraduccionNavigation)
                    .WithMany(p => p.tblTipoNomina_MX)
                    .HasForeignKey(d => d.idTraduccion)
                    .HasConstraintName("FK_tblTipoNomina_MX_tblTraduccion");
            });

            modelBuilder.Entity<tblTipoNomina_RD>(entity =>
            {
                entity.Property(e => e.idTipoNomina_RD).ValueGeneratedNever();

                entity.HasOne(d => d.idTraduccionNavigation)
                    .WithMany(p => p.tblTipoNomina_RD)
                    .HasForeignKey(d => d.idTraduccion)
                    .HasConstraintName("FK_tblTipoNomina_RD_tblTraduccion");
            });

            modelBuilder.Entity<tblTipoNotificacion>(entity =>
            {
                entity.HasOne(d => d.idTraduccionDenominacionNavigation)
                    .WithMany(p => p.tblTipoNotificacionidTraduccionDenominacionNavigation)
                    .HasForeignKey(d => d.idTraduccionDenominacion)
                    .HasConstraintName("FK_tblTipoNotificacion_tblTraduccionDenominacion");

                entity.HasOne(d => d.idTraduccionDescripcionNavigation)
                    .WithMany(p => p.tblTipoNotificacionidTraduccionDescripcionNavigation)
                    .HasForeignKey(d => d.idTraduccionDescripcion)
                    .HasConstraintName("FK_tblTipoNotificacion_tblTraduccionDesripcion");
            });

            modelBuilder.Entity<tblTipoPedido>(entity =>
            {
                entity.HasKey(e => e.idTipoPedido)
                    .HasName("PK_tblTipoPedido_1");

                entity.Property(e => e.idTipoPedido).ValueGeneratedOnAdd();

                entity.HasMany(d => d.idEntidad)
                    .WithMany(p => p.idTipoPedido)
                    .UsingEntity<Dictionary<string, object>>(
                        "tblTipoPedidoNEntidad",
                        l => l.HasOne<tblEntidad>().WithMany().HasForeignKey("idEntidad").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblTipoPedidoNEntidad_tblEntidad"),
                        r => r.HasOne<tblTipoPedido>().WithMany().HasForeignKey("idTipoPedido").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblTipoPedidoNEntidad_tblTipoPedido"),
                        j =>
                        {
                            j.HasKey("idTipoPedido", "idEntidad");

                            j.ToTable("tblTipoPedidoNEntidad", "Logistica");
                        });
            });

            modelBuilder.Entity<tblTipoPregunta>(entity =>
            {
                entity.Property(e => e.idTipoPregunta).ValueGeneratedNever();
            });

            modelBuilder.Entity<tblTipoPrenda>(entity =>
            {
                entity.HasOne(d => d.idFamiliaNavigation)
                    .WithMany(p => p.tblTipoPrenda)
                    .HasForeignKey(d => d.idFamilia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTipoPrenda_tblFamilia");

                entity.HasOne(d => d.idTraduccionNavigation)
                    .WithMany(p => p.tblTipoPrenda)
                    .HasForeignKey(d => d.idTraduccion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTipoPrenda_tblTraduccion");
            });

            modelBuilder.Entity<tblTipoPrendaHuesped>(entity =>
            {
                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblTipoPrendaHuesped)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTipoPrendaHuesped_tblLavanderia");
            });

            modelBuilder.Entity<tblTipoProduccion>(entity =>
            {
                entity.Property(e => e.idTipoProduccion).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<tblTipoRechazo>(entity =>
            {
                entity.Property(e => e.idTipoRechazo).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<tblTipoRetiro>(entity =>
            {
                entity.HasKey(e => e.idTipoRetiro)
                    .HasName("PK__tblTipoR__B289E86BF6135014");

                entity.Property(e => e.idTipoRetiro).ValueGeneratedNever();

                entity.HasOne(d => d.idTraduccionNavigation)
                    .WithMany(p => p.tblTipoRetiro)
                    .HasForeignKey(d => d.idTraduccion)
                    .HasConstraintName("FK__tblTipoRe__idTra__3B0D59BA");
            });

            modelBuilder.Entity<tblTipoSalidaRecambio>(entity =>
            {
                entity.HasKey(e => e.idTipoSalidaRecambio)
                    .HasName("PK__tblTipoS__936B0ECB6BCBFB3A");

                entity.HasOne(d => d.idTraduccionNavigation)
                    .WithMany(p => p.tblTipoSalidaRecambio)
                    .HasForeignKey(d => d.idTraduccion)
                    .HasConstraintName("FK__tblTipoSa__idTra__7A3DDC37");
            });

            modelBuilder.Entity<tblTipoServicio>(entity =>
            {
                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblTipoServicio)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTipoServicio_tblLavanderia");
            });

            modelBuilder.Entity<tblTipoSubIncidencia>(entity =>
            {
                entity.HasKey(e => e.idSubTipoIncidencia)
                    .HasName("PK_tblSubTipoIncidencia");

                entity.HasOne(d => d.idTipoIncidenciaNavigation)
                    .WithMany(p => p.tblTipoSubIncidencia)
                    .HasForeignKey(d => d.idTipoIncidencia)
                    .HasConstraintName("FK_tblTipoSubIncidencia_tblTipoIncidencia");

                entity.HasOne(d => d.idTraduccionNavigation)
                    .WithMany(p => p.tblTipoSubIncidencia)
                    .HasForeignKey(d => d.idTraduccion)
                    .HasConstraintName("FK_tblTipoSubIncidencia_tblTraduccion");
            });

            modelBuilder.Entity<tblTipoTrabajo>(entity =>
            {
                entity.HasKey(e => e.idTipoTrabajo)
                    .HasName("PK__tblTipoT__4893AC7164C088F2");
            });

            modelBuilder.Entity<tblTipoTrabajoNUsuario>(entity =>
            {
                entity.HasKey(e => new { e.idUsuario, e.idLavanderia, e.idTipoTrabajo })
                    .HasName("PK_tblTipoTrabajoNUsuario_1");

                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblTipoTrabajoNUsuario)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTipoTrabajoNUsuario_tblLavanderia");

                entity.HasOne(d => d.idTipoTrabajoNavigation)
                    .WithMany(p => p.tblTipoTrabajoNUsuario)
                    .HasForeignKey(d => d.idTipoTrabajo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTipoTrabajoNUsuario_tblTipoTrabajo");

                entity.HasOne(d => d.idUsuarioNavigation)
                    .WithMany(p => p.tblTipoTrabajoNUsuario)
                    .HasForeignKey(d => d.idUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTipoTrabajoNUsuario_tblUsuario");
            });

            modelBuilder.Entity<tblTipoUsuario>(entity =>
            {
                entity.Property(e => e.idTipoUsuario).ValueGeneratedNever();
            });

            modelBuilder.Entity<tblTipoVehiculo>(entity =>
            {
                entity.Property(e => e.idTipoVehiculo).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<tblToken_Refresh>(entity =>
            {
                entity.Property(e => e.idToken).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.idUsuarioNavigation)
                    .WithMany(p => p.tblToken_Refresh)
                    .HasForeignKey(d => d.idUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblToken_Refresh_tblUsuario");
            });

            modelBuilder.Entity<tblToken_Refresh_Mobile>(entity =>
            {
                entity.Property(e => e.idToken).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.idUsuarioNavigation)
                    .WithMany(p => p.tblToken_Refresh_Mobile)
                    .HasForeignKey(d => d.idUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblToken_Refresh_Mobile_tblUsuario");
            });

            modelBuilder.Entity<tblTurno>(entity =>
            {
                entity.HasOne(d => d.idLavanderiaNavigation)
                    .WithMany(p => p.tblTurno)
                    .HasForeignKey(d => d.idLavanderia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTurno_tblLavanderia");

                entity.HasOne(d => d.idTurnoPadreNavigation)
                    .WithMany(p => p.InverseidTurnoPadreNavigation)
                    .HasForeignKey(d => d.idTurnoPadre)
                    .HasConstraintName("FK_tblTurno_tblTurno");
            });

            modelBuilder.Entity<tblUnidadMedida>(entity =>
            {
                entity.Property(e => e.idUnidadMedida).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<tblUnidadesPeso>(entity =>
            {
                entity.Property(e => e.idUnidadesPeso).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<tblUsuario>(entity =>
            {
                entity.HasOne(d => d.idAplicacionInicialNavigation)
                    .WithMany(p => p.tblUsuario)
                    .HasForeignKey(d => d.idAplicacionInicial)
                    .HasConstraintName("FK_tblUsuario_tblAplicacion");

                entity.HasOne(d => d.idCargoNavigation)
                    .WithMany(p => p.tblUsuario)
                    .HasForeignKey(d => d.idCargo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblUsuario_tblCargo");

                entity.HasOne(d => d.idCompañiaNavigation)
                    .WithMany(p => p.tblUsuario)
                    .HasForeignKey(d => d.idCompañia)
                    .HasConstraintName("FK_tblUsuario_tblCompañia");

                entity.HasOne(d => d.idFormularioInicioNavigation)
                    .WithMany(p => p.tblUsuario)
                    .HasForeignKey(d => d.idFormularioInicio)
                    .HasConstraintName("FK_tblUsuario_tblFormulario");

                entity.HasOne(d => d.idIdiomaNavigation)
                    .WithMany(p => p.tblUsuario)
                    .HasForeignKey(d => d.idIdioma)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblIdioma_tblUsuario");

                entity.HasOne(d => d.idLavanderiaInicioNavigation)
                    .WithMany(p => p.tblUsuario)
                    .HasForeignKey(d => d.idLavanderiaInicio)
                    .HasConstraintName("FK_tblUsuario_tblLavanderia");

                entity.HasOne(d => d.idLocalizacionNavigation)
                    .WithMany(p => p.tblUsuario)
                    .HasForeignKey(d => d.idLocalizacion)
                    .HasConstraintName("FK_tblUsuario_tblLocalizacion");

                entity.HasOne(d => d.idPersonaNavigation)
                    .WithMany(p => p.tblUsuario)
                    .HasForeignKey(d => d.idPersona)
                    .HasConstraintName("FK_tblUsuario_tblPersona");

                entity.HasOne(d => d.idTipoUsuarioNavigation)
                    .WithMany(p => p.tblUsuario)
                    .HasForeignKey(d => d.idTipoUsuario)
                    .HasConstraintName("FK_tblUsuario_tblTipoUsuario");

                entity.HasMany(d => d.idAdmCentroCoste)
                    .WithMany(p => p.idUsuario)
                    .UsingEntity<Dictionary<string, object>>(
                        "tblAdmCentroCosteNUsuario",
                        l => l.HasOne<tblAdmCentroCoste>().WithMany().HasForeignKey("idAdmCentroCoste").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__tblAdmCen__idAdm__7410CCEC"),
                        r => r.HasOne<tblUsuario>().WithMany().HasForeignKey("idUsuario").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__tblAdmCen__idUsu__731CA8B3"),
                        j =>
                        {
                            j.HasKey("idUsuario", "idAdmCentroCoste").HasName("PK__tblAdmCe__0EC96F719788BCA1");

                            j.ToTable("tblAdmCentroCosteNUsuario", "Administracion");
                        });

                entity.HasMany(d => d.idAdmElementoPEP)
                    .WithMany(p => p.idUsuario)
                    .UsingEntity<Dictionary<string, object>>(
                        "tblAdmElementoPEPNUsuario",
                        l => l.HasOne<tblAdmElementoPEP>().WithMany().HasForeignKey("idAdmElementoPEP").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__tblAdmEle__idAdm__77E15DD0"),
                        r => r.HasOne<tblUsuario>().WithMany().HasForeignKey("idUsuario").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__tblAdmEle__idUsu__76ED3997"),
                        j =>
                        {
                            j.HasKey("idUsuario", "idAdmElementoPEP").HasName("PK__tblAdmEl__86C47093E6BB6C5F");

                            j.ToTable("tblAdmElementoPEPNUsuario", "Administracion");
                        });

                entity.HasMany(d => d.idEmpresaPolarier)
                    .WithMany(p => p.idUsuario)
                    .UsingEntity<Dictionary<string, object>>(
                        "tblEmpresaPolarierNUsuario",
                        l => l.HasOne<tblEmpresasPolarier>().WithMany().HasForeignKey("idEmpresaPolarier").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__tblEmpres__idEmp__660CC658"),
                        r => r.HasOne<tblUsuario>().WithMany().HasForeignKey("idUsuario").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__tblEmpres__idUsu__6518A21F"),
                        j =>
                        {
                            j.HasKey("idUsuario", "idEmpresaPolarier").HasName("PK__tblEmpre__3C523A8E7FF01948");

                            j.ToTable("tblEmpresaPolarierNUsuario", "GestionInterna");
                        });
            });

            modelBuilder.Entity<tblVehiculo>(entity =>
            {
                entity.HasKey(e => e.idVehiculo)
                    .HasName("PK_tblVehiculos");

                entity.HasOne(d => d.idTipoVehiculoNavigation)
                    .WithMany(p => p.tblVehiculo)
                    .HasForeignKey(d => d.idTipoVehiculo)
                    .HasConstraintName("FK_tblVehiculo_tblTipoVehiculo");

                entity.HasMany(d => d.idLavanderia)
                    .WithMany(p => p.idVehiculo)
                    .UsingEntity<Dictionary<string, object>>(
                        "tblVehiculoNLavanderia",
                        l => l.HasOne<tblLavanderia>().WithMany().HasForeignKey("idLavanderia").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__tblVehicu__idLav__23DF1048"),
                        r => r.HasOne<tblVehiculo>().WithMany().HasForeignKey("idVehiculo").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__tblVehicu__idVeh__22EAEC0F"),
                        j =>
                        {
                            j.HasKey("idVehiculo", "idLavanderia").HasName("PK__tblVehic__5FAE4C69C154013B");

                            j.ToTable("tblVehiculoNLavanderia", "Logistica");
                        });
            });

            modelBuilder.Entity<tblZonaHoraria>(entity =>
            {
                entity.Property(e => e.idZonaHoraria).ValueGeneratedOnAdd();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
