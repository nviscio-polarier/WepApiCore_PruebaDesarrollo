using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblMaquina", Schema = "Maquinaria")]
    [Index("idLavanderia", Name = "IX_tblMaquina_idLavanderia")]
    public partial class tblMaquina
    {
        public tblMaquina()
        {
            tblClienteNMaquina = new HashSet<tblClienteNMaquina>();
            tblEstadoSmartHubNMaquina = new HashSet<tblEstadoSmartHubNMaquina>();
            tblGestionRetiro = new HashSet<tblGestionRetiro>();
            tblIncidencia = new HashSet<tblIncidencia>();
            tblKgLavadosLavadora = new HashSet<tblKgLavadosLavadora>();
            tblKgLavadosTunel = new HashSet<tblKgLavadosTunel>();
            tblLayout_SmartView = new HashSet<tblLayout_SmartView>();
            tblLecturaLavadoras = new HashSet<tblLecturaLavadoras>();
            tblMantenimientoPrev = new HashSet<tblMantenimientoPrev>();
            tblNodos = new HashSet<tblNodos>();
            tblParteTrabajo = new HashSet<tblParteTrabajo>();
            tblPersonaNMaquina = new HashSet<tblPersonaNMaquina>();
            tblPosicionNAreaLavanderiaNLavanderia = new HashSet<tblPosicionNAreaLavanderiaNLavanderia>();
            tblPrendaNMaquina = new HashSet<tblPrendaNMaquina>();
            tblPrendasHora = new HashSet<tblPrendasHora>();
            tblProduccion = new HashSet<tblProduccion>();
            tblProduccionMaquinaNCliente = new HashSet<tblProduccionMaquinaNCliente>();
            tblProduccionMaquinaNPrenda = new HashSet<tblProduccionMaquinaNPrenda>();
            tblTareaMaquina = new HashSet<tblTareaMaquina>();
            idPrenda = new HashSet<tblPrenda>();
        }

        [Key]
        public int idMaquina { get; set; }
        [StringLength(50)]
        public string denominacion { get; set; } = null!;
        [StringLength(5)]
        public string? idxGsbs { get; set; }
        public int idLavanderia { get; set; }
        public bool? activo { get; set; }
        public string? etiqueta { get; set; }
        public string? numSerie { get; set; }
        [Column(TypeName = "numeric(5, 2)")]
        public decimal? peso { get; set; }
        public bool? activoMyProduction { get; set; }
        public short? capacidad { get; set; }
        public bool restrictiva { get; set; }
        public short? añoFabricacion { get; set; }
        public bool eliminado { get; set; }
        [StringLength(2)]
        public string? codigoLav { get; set; }
        [Column(TypeName = "date")]
        public DateTime? fechaInicioGarantia { get; set; }
        [Column(TypeName = "date")]
        public DateTime? fechaFinGarantia { get; set; }
        public int? idTipoMaquinaNCategoriaMaquina { get; set; }
        public byte? numPosicion { get; set; }
        public bool? posicionVia1Izquierda { get; set; }
        public byte? idPlantillaTareaMantenimientoPrev { get; set; }

        [ForeignKey("idLavanderia")]
        [InverseProperty("tblMaquina")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
        [ForeignKey("idPlantillaTareaMantenimientoPrev")]
        [InverseProperty("tblMaquina")]
        public virtual tblPlantillaTareaMantenimientoPrev? idPlantillaTareaMantenimientoPrevNavigation { get; set; }
        [ForeignKey("idTipoMaquinaNCategoriaMaquina")]
        [InverseProperty("tblMaquina")]
        public virtual tblTipoMaquinaNCategoriaMaquina? idTipoMaquinaNCategoriaMaquinaNavigation { get; set; }
        [InverseProperty("idMaquinaNavigation")]
        public virtual ICollection<tblClienteNMaquina> tblClienteNMaquina { get; set; }
        [InverseProperty("idMaquinaNavigation")]
        public virtual ICollection<tblEstadoSmartHubNMaquina> tblEstadoSmartHubNMaquina { get; set; }
        [InverseProperty("idMaquinaNavigation")]
        public virtual ICollection<tblGestionRetiro> tblGestionRetiro { get; set; }
        [InverseProperty("idMaquinaNavigation")]
        public virtual ICollection<tblIncidencia> tblIncidencia { get; set; }
        [InverseProperty("idMaquinaNavigation")]
        public virtual ICollection<tblKgLavadosLavadora> tblKgLavadosLavadora { get; set; }
        [InverseProperty("idMaquinaNavigation")]
        public virtual ICollection<tblKgLavadosTunel> tblKgLavadosTunel { get; set; }
        [InverseProperty("idMaquinaNavigation")]
        public virtual ICollection<tblLayout_SmartView> tblLayout_SmartView { get; set; }
        [InverseProperty("idMaquinaNavigation")]
        public virtual ICollection<tblLecturaLavadoras> tblLecturaLavadoras { get; set; }
        [InverseProperty("idMaquinaNavigation")]
        public virtual ICollection<tblMantenimientoPrev> tblMantenimientoPrev { get; set; }
        [InverseProperty("idMaquinaNavigation")]
        public virtual ICollection<tblNodos> tblNodos { get; set; }
        [InverseProperty("idMaquinaNavigation")]
        public virtual ICollection<tblParteTrabajo> tblParteTrabajo { get; set; }
        [InverseProperty("idMaquinaNavigation")]
        public virtual ICollection<tblPersonaNMaquina> tblPersonaNMaquina { get; set; }
        [InverseProperty("idMaquinaNavigation")]
        public virtual ICollection<tblPosicionNAreaLavanderiaNLavanderia> tblPosicionNAreaLavanderiaNLavanderia { get; set; }
        [InverseProperty("idMaquinaNavigation")]
        public virtual ICollection<tblPrendaNMaquina> tblPrendaNMaquina { get; set; }
        [InverseProperty("idMaquinaNavigation")]
        public virtual ICollection<tblPrendasHora> tblPrendasHora { get; set; }
        [InverseProperty("idMaquinaNavigation")]
        public virtual ICollection<tblProduccion> tblProduccion { get; set; }
        [InverseProperty("idMaquinaNavigation")]
        public virtual ICollection<tblProduccionMaquinaNCliente> tblProduccionMaquinaNCliente { get; set; }
        [InverseProperty("idMaquinaNavigation")]
        public virtual ICollection<tblProduccionMaquinaNPrenda> tblProduccionMaquinaNPrenda { get; set; }
        [InverseProperty("idMaquinaNavigation")]
        public virtual ICollection<tblTareaMaquina> tblTareaMaquina { get; set; }

        [ForeignKey("idMaquina")]
        [InverseProperty("idMaquina")]
        public virtual ICollection<tblPrenda> idPrenda { get; set; }
    }
}
