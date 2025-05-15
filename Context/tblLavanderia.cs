using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblLavanderia", Schema = "General")]
    public partial class tblLavanderia
    {
        public tblLavanderia()
        {
            tblAbono = new HashSet<tblAbono>();
            tblAdmPedidoProveedor = new HashSet<tblAdmPedidoProveedor>();
            tblAlmacen = new HashSet<tblAlmacen>();
            tblAreaLavanderiaNLavanderia = new HashSet<tblAreaLavanderiaNLavanderia>();
            tblBacsCarro = new HashSet<tblBacsCarro>();
            tblCalendarioLavanderia = new HashSet<tblCalendarioLavanderia>();
            tblCalendarioPersonal = new HashSet<tblCalendarioPersonal>();
            tblCierreDatos_Lavanderia = new HashSet<tblCierreDatos_Lavanderia>();
            tblCierreDatos_Uniformidad = new HashSet<tblCierreDatos_Uniformidad>();
            tblCierreDatos_Valet = new HashSet<tblCierreDatos_Valet>();
            tblColorPrendaHuesped = new HashSet<tblColorPrendaHuesped>();
            tblCompañia = new HashSet<tblCompañia>();
            tblConfiguracionSalarial = new HashSet<tblConfiguracionSalarial>();
            tblCorreoAltaGestoriaNCentroLav = new HashSet<tblCorreoAltaGestoriaNCentroLav>();
            tblCorreosNLav = new HashSet<tblCorreosNLav>();
            tblCriterioValoracionNLavanderia = new HashSet<tblCriterioValoracionNLavanderia>();
            tblCuadrantePersonal = new HashSet<tblCuadrantePersonal>();
            tblDefectoPrendaHuesped = new HashSet<tblDefectoPrendaHuesped>();
            tblEnergyHub = new HashSet<tblEnergyHub>();
            tblEventoPersona = new HashSet<tblEventoPersona>();
            tblGestionRetiro = new HashSet<tblGestionRetiro>();
            tblHistoricoNominas = new HashSet<tblHistoricoNominas>();
            tblIncidencia = new HashSet<tblIncidencia>();
            tblIngresosPresupuestados = new HashSet<tblIngresosPresupuestados>();
            tblJornada = new HashSet<tblJornada>();
            tblJornadaPersona = new HashSet<tblJornadaPersona>();
            tblLayout_SmartView = new HashSet<tblLayout_SmartView>();
            tblLecturaCarro = new HashSet<tblLecturaCarro>();
            tblLlamamiento = new HashSet<tblLlamamiento>();
            tblLogAcciones = new HashSet<tblLogAcciones>();
            tblMaquina = new HashSet<tblMaquina>();
            tblMezclaSucioCliente = new HashSet<tblMezclaSucioCliente>();
            tblModuloNLavanderia_Ponderacion = new HashSet<tblModuloNLavanderia_Ponderacion>();
            tblMovimientoElemLog = new HashSet<tblMovimientoElemLog>();
            tblMovimientoTag = new HashSet<tblMovimientoTag>();
            tblMuestreo = new HashSet<tblMuestreo>();
            tblNodos = new HashSet<tblNodos>();
            tblObjetivosKpi = new HashSet<tblObjetivosKpi>();
            tblObjetivosKpiNRecursoNivel = new HashSet<tblObjetivosKpiNRecursoNivel>();
            tblParadaNParteTransporte = new HashSet<tblParadaNParteTransporte>();
            tblParadaNRutaExpedicion = new HashSet<tblParadaNRutaExpedicion>();
            tblParteTrabajo = new HashSet<tblParteTrabajo>();
            tblParteTransporte = new HashSet<tblParteTransporte>();
            tblPedido = new HashSet<tblPedido>();
            tblPedidosExtra = new HashSet<tblPedidosExtra>();
            tblPersona = new HashSet<tblPersona>();
            tblPersonaNAreaNLavanderia = new HashSet<tblPersonaNAreaNLavanderia>();
            tblPesoNCategoriaNLavanderia = new HashSet<tblPesoNCategoriaNLavanderia>();
            tblPesoNSistemaNLavanderia = new HashSet<tblPesoNSistemaNLavanderia>();
            tblPosicionNAreaLavanderiaNLavanderia = new HashSet<tblPosicionNAreaLavanderiaNLavanderia>();
            tblPrenda = new HashSet<tblPrenda>();
            tblPrendaEjecutivo = new HashSet<tblPrendaEjecutivo>();
            tblPrendaExtra = new HashSet<tblPrendaExtra>();
            tblPrendaHuesped = new HashSet<tblPrendaHuesped>();
            tblPrendaNLavanderia = new HashSet<tblPrendaNLavanderia>();
            tblProduccion = new HashSet<tblProduccion>();
            tblRecursoContador = new HashSet<tblRecursoContador>();
            tblRecursoNivel = new HashSet<tblRecursoNivel>();
            tblReparto = new HashSet<tblReparto>();
            tblRepartoOffice = new HashSet<tblRepartoOffice>();
            tblRepartosValet = new HashSet<tblRepartosValet>();
            tblRevision = new HashSet<tblRevision>();
            tblRutaExpedicion = new HashSet<tblRutaExpedicion>();
            tblTaquilla = new HashSet<tblTaquilla>();
            tblTaquillas_prueba = new HashSet<tblTaquillas_prueba>();
            tblTipoKpi_Observaciones = new HashSet<tblTipoKpi_Observaciones>();
            tblTipoLavado = new HashSet<tblTipoLavado>();
            tblTipoPrendaHuesped = new HashSet<tblTipoPrendaHuesped>();
            tblTipoServicio = new HashSet<tblTipoServicio>();
            tblTipoTrabajoNUsuario = new HashSet<tblTipoTrabajoNUsuario>();
            tblTurno = new HashSet<tblTurno>();
            tblUsuario = new HashSet<tblUsuario>();
            idEntidad = new HashSet<tblEntidad>();
            idFormulario = new HashSet<tblFormulario>();
            idGrupoPrendaEst = new HashSet<tblGrupoPrendaEst>();
            idModulo = new HashSet<tblModulo>();
            idTipoProduccion = new HashSet<tblTipoProduccion>();
            idTipoRechazo = new HashSet<tblTipoRechazo>();
            idUsuario = new HashSet<tblUsuario>();
            idVehiculo = new HashSet<tblVehiculo>();
        }

        [Key]
        public int idLavanderia { get; set; }
        [StringLength(50)]
        public string denominacion { get; set; } = null!;
        [StringLength(3)]
        public string? codigoWeb { get; set; }
        public string? direccion { get; set; }
        [StringLength(50)]
        public string? codigoPostal { get; set; }
        [StringLength(50)]
        public string? poblacion { get; set; }
        [StringLength(50)]
        public string? telefono { get; set; }
        [StringLength(50)]
        public string? telefono2 { get; set; }
        [StringLength(50)]
        public string? email { get; set; }
        public short? idIdioma { get; set; }
        public byte? idMoneda { get; set; }
        public byte? idZonaHoraria { get; set; }
        public bool? horarioVerano { get; set; }
        public bool? gestionaRechazo { get; set; }
        public byte? idUnidadesPeso { get; set; }
        public bool? gpActivo { get; set; }
        public int? idCorporacion { get; set; }
        public string? bdMyUniform { get; set; }
        public int? tipoPedidoHuesped { get; set; }
        public int idPais { get; set; }
        public bool? isNum_codPersona_MyUniform { get; set; }
        public byte? idMonedaLocal { get; set; }
        public bool isMyUniformCantidad { get; set; }
        [Column(TypeName = "decimal(7, 3)")]
        public decimal? precioUnidad_MyUniform { get; set; }
        public bool enableControlHorario { get; set; }
        public string? coordenadas { get; set; }
        public bool isRutaExpedicionPersonalizada { get; set; }
        public int? enableRFID_Personal { get; set; }
        public string? OPC_URL { get; set; }
        public short? idLocalizacion { get; set; }
        public short? idEmpresaPolarier { get; set; }
        public int? idAdmElementoPEP { get; set; }

        [ForeignKey("idAdmElementoPEP")]
        [InverseProperty("tblLavanderia")]
        public virtual tblAdmElementoPEP? idAdmElementoPEPNavigation { get; set; }
        [ForeignKey("idCorporacion")]
        [InverseProperty("tblLavanderia")]
        public virtual tblCorporacion? idCorporacionNavigation { get; set; }
        [ForeignKey("idEmpresaPolarier")]
        [InverseProperty("tblLavanderia")]
        public virtual tblEmpresasPolarier? idEmpresaPolarierNavigation { get; set; }
        [ForeignKey("idIdioma")]
        [InverseProperty("tblLavanderia")]
        public virtual tblIdioma? idIdiomaNavigation { get; set; }
        [ForeignKey("idLocalizacion")]
        [InverseProperty("tblLavanderia")]
        public virtual tblLocalizacion? idLocalizacionNavigation { get; set; }
        [ForeignKey("idMonedaLocal")]
        [InverseProperty("tblLavanderiaidMonedaLocalNavigation")]
        public virtual tblMoneda? idMonedaLocalNavigation { get; set; }
        [ForeignKey("idMoneda")]
        [InverseProperty("tblLavanderiaidMonedaNavigation")]
        public virtual tblMoneda? idMonedaNavigation { get; set; }
        [ForeignKey("idPais")]
        [InverseProperty("tblLavanderia")]
        public virtual tblPais idPaisNavigation { get; set; } = null!;
        [ForeignKey("idUnidadesPeso")]
        [InverseProperty("tblLavanderia")]
        public virtual tblUnidadesPeso? idUnidadesPesoNavigation { get; set; }
        [ForeignKey("idZonaHoraria")]
        [InverseProperty("tblLavanderia")]
        public virtual tblZonaHoraria? idZonaHorariaNavigation { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblAbono> tblAbono { get; set; }
        [InverseProperty("idCentroTrabajo1")]
        public virtual ICollection<tblAdmPedidoProveedor> tblAdmPedidoProveedor { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblAlmacen> tblAlmacen { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblAreaLavanderiaNLavanderia> tblAreaLavanderiaNLavanderia { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblBacsCarro> tblBacsCarro { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblCalendarioLavanderia> tblCalendarioLavanderia { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblCalendarioPersonal> tblCalendarioPersonal { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblCierreDatos_Lavanderia> tblCierreDatos_Lavanderia { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblCierreDatos_Uniformidad> tblCierreDatos_Uniformidad { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblCierreDatos_Valet> tblCierreDatos_Valet { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblColorPrendaHuesped> tblColorPrendaHuesped { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblCompañia> tblCompañia { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblConfiguracionSalarial> tblConfiguracionSalarial { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblCorreoAltaGestoriaNCentroLav> tblCorreoAltaGestoriaNCentroLav { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblCorreosNLav> tblCorreosNLav { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblCriterioValoracionNLavanderia> tblCriterioValoracionNLavanderia { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblCuadrantePersonal> tblCuadrantePersonal { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblDefectoPrendaHuesped> tblDefectoPrendaHuesped { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblEnergyHub> tblEnergyHub { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblEventoPersona> tblEventoPersona { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblGestionRetiro> tblGestionRetiro { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblHistoricoNominas> tblHistoricoNominas { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblIncidencia> tblIncidencia { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblIngresosPresupuestados> tblIngresosPresupuestados { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblJornada> tblJornada { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblJornadaPersona> tblJornadaPersona { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblLayout_SmartView> tblLayout_SmartView { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblLecturaCarro> tblLecturaCarro { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblLlamamiento> tblLlamamiento { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblLogAcciones> tblLogAcciones { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblMaquina> tblMaquina { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblMezclaSucioCliente> tblMezclaSucioCliente { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblModuloNLavanderia_Ponderacion> tblModuloNLavanderia_Ponderacion { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblMovimientoElemLog> tblMovimientoElemLog { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblMovimientoTag> tblMovimientoTag { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblMuestreo> tblMuestreo { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblNodos> tblNodos { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblObjetivosKpi> tblObjetivosKpi { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblObjetivosKpiNRecursoNivel> tblObjetivosKpiNRecursoNivel { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblParadaNParteTransporte> tblParadaNParteTransporte { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblParadaNRutaExpedicion> tblParadaNRutaExpedicion { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblParteTrabajo> tblParteTrabajo { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblParteTransporte> tblParteTransporte { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblPedido> tblPedido { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblPedidosExtra> tblPedidosExtra { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblPersona> tblPersona { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblPersonaNAreaNLavanderia> tblPersonaNAreaNLavanderia { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblPesoNCategoriaNLavanderia> tblPesoNCategoriaNLavanderia { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblPesoNSistemaNLavanderia> tblPesoNSistemaNLavanderia { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblPosicionNAreaLavanderiaNLavanderia> tblPosicionNAreaLavanderiaNLavanderia { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblPrenda> tblPrenda { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblPrendaEjecutivo> tblPrendaEjecutivo { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblPrendaExtra> tblPrendaExtra { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblPrendaHuesped> tblPrendaHuesped { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblPrendaNLavanderia> tblPrendaNLavanderia { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblProduccion> tblProduccion { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblRecursoContador> tblRecursoContador { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblRecursoNivel> tblRecursoNivel { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblReparto> tblReparto { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblRepartoOffice> tblRepartoOffice { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblRepartosValet> tblRepartosValet { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblRevision> tblRevision { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblRutaExpedicion> tblRutaExpedicion { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblTaquilla> tblTaquilla { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblTaquillas_prueba> tblTaquillas_prueba { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblTipoKpi_Observaciones> tblTipoKpi_Observaciones { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblTipoLavado> tblTipoLavado { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblTipoPrendaHuesped> tblTipoPrendaHuesped { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblTipoServicio> tblTipoServicio { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblTipoTrabajoNUsuario> tblTipoTrabajoNUsuario { get; set; }
        [InverseProperty("idLavanderiaNavigation")]
        public virtual ICollection<tblTurno> tblTurno { get; set; }
        [InverseProperty("idLavanderiaInicioNavigation")]
        public virtual ICollection<tblUsuario> tblUsuario { get; set; }

        [ForeignKey("idLavanderia")]
        [InverseProperty("idLavanderia")]
        public virtual ICollection<tblEntidad> idEntidad { get; set; }
        [ForeignKey("idLavanderia")]
        [InverseProperty("idLavanderia")]
        public virtual ICollection<tblFormulario> idFormulario { get; set; }
        [ForeignKey("idLavanderia")]
        [InverseProperty("idLavanderia")]
        public virtual ICollection<tblGrupoPrendaEst> idGrupoPrendaEst { get; set; }
        [ForeignKey("idLavanderia")]
        [InverseProperty("idLavanderia")]
        public virtual ICollection<tblModulo> idModulo { get; set; }
        [ForeignKey("idLavanderia")]
        [InverseProperty("idLavanderia")]
        public virtual ICollection<tblTipoProduccion> idTipoProduccion { get; set; }
        [ForeignKey("idLavanderia")]
        [InverseProperty("idLavanderia")]
        public virtual ICollection<tblTipoRechazo> idTipoRechazo { get; set; }
        [ForeignKey("idLavanderia")]
        [InverseProperty("idLavanderia")]
        public virtual ICollection<tblUsuario> idUsuario { get; set; }
        [ForeignKey("idLavanderia")]
        [InverseProperty("idLavanderia")]
        public virtual ICollection<tblVehiculo> idVehiculo { get; set; }
    }
}
