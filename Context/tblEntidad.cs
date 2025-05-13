using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblEntidad", Schema = "General")]
    public partial class tblEntidad
    {
        public tblEntidad()
        {
            tblAbono = new HashSet<tblAbono>();
            tblAlmacen = new HashSet<tblAlmacen>();
            tblCalendarioEntidad = new HashSet<tblCalendarioEntidad>();
            tblCierreDatos_Facturacion = new HashSet<tblCierreDatos_Facturacion>();
            tblCierreFactEntidad = new HashSet<tblCierreFactEntidad>();
            tblClienteNMaquina = new HashSet<tblClienteNMaquina>();
            tblConfigAlbaranReparto = new HashSet<tblConfigAlbaranReparto>();
            tblCorreosNEntidad = new HashSet<tblCorreosNEntidad>();
            tblEncuesta = new HashSet<tblEncuesta>();
            tblEntidadNInventario = new HashSet<tblEntidadNInventario>();
            tblEntidadNRutaExpedicion = new HashSet<tblEntidadNRutaExpedicion>();
            tblEntidad_historico_idTipoFacturacion = new HashSet<tblEntidad_historico_idTipoFacturacion>();
            tblEstancia = new HashSet<tblEstancia>();
            tblGestionRetiro = new HashSet<tblGestionRetiro>();
            tblHorarioRepartoNEntidad = new HashSet<tblHorarioRepartoNEntidad>();
            tblIncidencia = new HashSet<tblIncidencia>();
            tblIngresosPresupuestados = new HashSet<tblIngresosPresupuestados>();
            tblInventario = new HashSet<tblInventario>();
            tblKgLavadosLavadora = new HashSet<tblKgLavadosLavadora>();
            tblKgLavadosTunel = new HashSet<tblKgLavadosTunel>();
            tblLecturaCarro = new HashSet<tblLecturaCarro>();
            tblLecturaLavadoras = new HashSet<tblLecturaLavadoras>();
            tblMezclaSucioCliente = new HashSet<tblMezclaSucioCliente>();
            tblMovimiento = new HashSet<tblMovimiento>();
            tblMovimientoElemLog = new HashSet<tblMovimientoElemLog>();
            tblMovimientoTag = new HashSet<tblMovimientoTag>();
            tblParadaNParteTransporte = new HashSet<tblParadaNParteTransporte>();
            tblParadaNRutaExpedicion = new HashSet<tblParadaNRutaExpedicion>();
            tblPedido = new HashSet<tblPedido>();
            tblPorcentajeValorPrendaNEntidad = new HashSet<tblPorcentajeValorPrendaNEntidad>();
            tblPrenda = new HashSet<tblPrenda>();
            tblPrendaNEntidad_NuevoPedido = new HashSet<tblPrendaNEntidad_NuevoPedido>();
            tblPrendaNMuestreo = new HashSet<tblPrendaNMuestreo>();
            tblPrendaNUsuarioNEntidad = new HashSet<tblPrendaNUsuarioNEntidad>();
            tblProduccion = new HashSet<tblProduccion>();
            tblProduccionMaquinaNCliente = new HashSet<tblProduccionMaquinaNCliente>();
            tblReparto = new HashSet<tblReparto>();
            tblRepartoOffice = new HashSet<tblRepartoOffice>();
            tblRepartosValet = new HashSet<tblRepartosValet>();
            tblRespuesta = new HashSet<tblRespuesta>();
            tblReunion = new HashSet<tblReunion>();
            tblRuta = new HashSet<tblRuta>();
            tblRutaNParteTransporte = new HashSet<tblRutaNParteTransporte>();
            tblRutaSeccion = new HashSet<tblRutaSeccion>();
            tblSacasPendientes = new HashSet<tblSacasPendientes>();
            tblSeccionNivel1 = new HashSet<tblSeccionNivel1>();
            tblSolicitudAbono = new HashSet<tblSolicitudAbono>();
            tblStockTipoElemLogNEntidad = new HashSet<tblStockTipoElemLogNEntidad>();
            tblTipoHabitacion = new HashSet<tblTipoHabitacion>();
            idEntidadNavigation = new HashSet<tblEntidad>();
            idEntidadSecundaria = new HashSet<tblEntidad>();
            idLavanderia = new HashSet<tblLavanderia>();
            idTipoPedido = new HashSet<tblTipoPedido>();
            idUsuario = new HashSet<tblUsuario>();
        }

        [Key]
        public int idEntidad { get; set; }
        public string? denominacion { get; set; }
        [StringLength(10)]
        public string codigoWeb { get; set; } = null!;
        public int idCompañia { get; set; }
        [StringLength(5)]
        public string? idxEntidad { get; set; }
        public bool? activo { get; set; }
        [StringLength(50)]
        public string? codigoPostal { get; set; }
        [StringLength(50)]
        public string? pais { get; set; }
        [StringLength(50)]
        public string? poblacion { get; set; }
        [StringLength(50)]
        public string? provincia { get; set; }
        [StringLength(50)]
        public string? telefono { get; set; }
        [StringLength(50)]
        public string? telefono2 { get; set; }
        [StringLength(50)]
        public string? email { get; set; }
        public int? categoria { get; set; }
        public int? plazas { get; set; }
        public int? habitaciones { get; set; }
        public bool? visibleGP { get; set; }
        public byte? idTipoConsumoLenceria { get; set; }
        public string? direccion { get; set; }
        public bool? reparteAlmGeneral { get; set; }
        public byte? idTipoRepartoEntidad { get; set; }
        public byte? reparteRechazoRetiro { get; set; }
        public bool? inventarioPorEntidad { get; set; }
        [Column(TypeName = "decimal(8, 3)")]
        public decimal? costeEstancia { get; set; }
        [Column(TypeName = "decimal(8, 3)")]
        public decimal? objKgEstancia { get; set; }
        public byte idTipoFacturacionCliente { get; set; }
        public bool eliminado { get; set; }
        public byte? idGrupoEntidad { get; set; }
        public byte idModeloImpresion_reparto { get; set; }
        public short? idLocalizacion { get; set; }
        public byte? idMoneda { get; set; }
        [Required]
        public bool? isPrincipal { get; set; }
        public bool? isTodasPrendaNNuevoPedido_compañia { get; set; }
        public bool? isTodasPrendaNNuevoPedido_entidad { get; set; }
        public byte? idTipoPedidoEntidad { get; set; }
        public string? coordenadas { get; set; }
        public bool? enableObservacionesPedido { get; set; }
        public bool enablePlanificadorPedidos { get; set; }
        public bool? inventarioPorPorcentaje { get; set; }
        public byte? idTipoAlmacenajeLimpio { get; set; }

        [ForeignKey("idCompañia")]
        [InverseProperty("tblEntidad")]
        public virtual tblCompañia idCompañiaNavigation { get; set; } = null!;
        [ForeignKey("idGrupoEntidad")]
        [InverseProperty("tblEntidad")]
        public virtual tblGrupoEntidad? idGrupoEntidadNavigation { get; set; }
        [ForeignKey("idLocalizacion")]
        [InverseProperty("tblEntidad")]
        public virtual tblLocalizacion? idLocalizacionNavigation { get; set; }
        [ForeignKey("idModeloImpresion_reparto")]
        [InverseProperty("tblEntidad")]
        public virtual tblModeloImpresion_reparto idModeloImpresion_repartoNavigation { get; set; } = null!;
        [ForeignKey("idMoneda")]
        [InverseProperty("tblEntidad")]
        public virtual tblMoneda? idMonedaNavigation { get; set; }
        [ForeignKey("idTipoAlmacenajeLimpio")]
        [InverseProperty("tblEntidad")]
        public virtual tblTipoAlmacenajeLimpio? idTipoAlmacenajeLimpioNavigation { get; set; }
        [ForeignKey("idTipoConsumoLenceria")]
        [InverseProperty("tblEntidad")]
        public virtual tblTipoConsumoLenceria? idTipoConsumoLenceriaNavigation { get; set; }
        [ForeignKey("idTipoFacturacionCliente")]
        [InverseProperty("tblEntidad")]
        public virtual tblTipoFacturacionCliente idTipoFacturacionClienteNavigation { get; set; } = null!;
        [ForeignKey("idTipoRepartoEntidad")]
        [InverseProperty("tblEntidad")]
        public virtual tblTipoRepartoEntidad? idTipoRepartoEntidadNavigation { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblAbono> tblAbono { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblAlmacen> tblAlmacen { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblCalendarioEntidad> tblCalendarioEntidad { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblCierreDatos_Facturacion> tblCierreDatos_Facturacion { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblCierreFactEntidad> tblCierreFactEntidad { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblClienteNMaquina> tblClienteNMaquina { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblConfigAlbaranReparto> tblConfigAlbaranReparto { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblCorreosNEntidad> tblCorreosNEntidad { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblEncuesta> tblEncuesta { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblEntidadNInventario> tblEntidadNInventario { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblEntidadNRutaExpedicion> tblEntidadNRutaExpedicion { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblEntidad_historico_idTipoFacturacion> tblEntidad_historico_idTipoFacturacion { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblEstancia> tblEstancia { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblGestionRetiro> tblGestionRetiro { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblHorarioRepartoNEntidad> tblHorarioRepartoNEntidad { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblIncidencia> tblIncidencia { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblIngresosPresupuestados> tblIngresosPresupuestados { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblInventario> tblInventario { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblKgLavadosLavadora> tblKgLavadosLavadora { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblKgLavadosTunel> tblKgLavadosTunel { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblLecturaCarro> tblLecturaCarro { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblLecturaLavadoras> tblLecturaLavadoras { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblMezclaSucioCliente> tblMezclaSucioCliente { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblMovimiento> tblMovimiento { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblMovimientoElemLog> tblMovimientoElemLog { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblMovimientoTag> tblMovimientoTag { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblParadaNParteTransporte> tblParadaNParteTransporte { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblParadaNRutaExpedicion> tblParadaNRutaExpedicion { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblPedido> tblPedido { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblPorcentajeValorPrendaNEntidad> tblPorcentajeValorPrendaNEntidad { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblPrenda> tblPrenda { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblPrendaNEntidad_NuevoPedido> tblPrendaNEntidad_NuevoPedido { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblPrendaNMuestreo> tblPrendaNMuestreo { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblPrendaNUsuarioNEntidad> tblPrendaNUsuarioNEntidad { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblProduccion> tblProduccion { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblProduccionMaquinaNCliente> tblProduccionMaquinaNCliente { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblReparto> tblReparto { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblRepartoOffice> tblRepartoOffice { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblRepartosValet> tblRepartosValet { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblRespuesta> tblRespuesta { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblReunion> tblReunion { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblRuta> tblRuta { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblRutaNParteTransporte> tblRutaNParteTransporte { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblRutaSeccion> tblRutaSeccion { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblSacasPendientes> tblSacasPendientes { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblSeccionNivel1> tblSeccionNivel1 { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblSolicitudAbono> tblSolicitudAbono { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblStockTipoElemLogNEntidad> tblStockTipoElemLogNEntidad { get; set; }
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblTipoHabitacion> tblTipoHabitacion { get; set; }

        [ForeignKey("idEntidadSecundaria")]
        [InverseProperty("idEntidadSecundaria")]
        public virtual ICollection<tblEntidad> idEntidadNavigation { get; set; }
        [ForeignKey("idEntidad")]
        [InverseProperty("idEntidadNavigation")]
        public virtual ICollection<tblEntidad> idEntidadSecundaria { get; set; }
        [ForeignKey("idEntidad")]
        [InverseProperty("idEntidad")]
        public virtual ICollection<tblLavanderia> idLavanderia { get; set; }
        [ForeignKey("idEntidad")]
        [InverseProperty("idEntidad")]
        public virtual ICollection<tblTipoPedido> idTipoPedido { get; set; }
        [ForeignKey("idEntidad")]
        [InverseProperty("idEntidad")]
        public virtual ICollection<tblUsuario> idUsuario { get; set; }
    }
}
