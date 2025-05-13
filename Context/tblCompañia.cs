using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCompañia", Schema = "General")]
    public partial class tblCompañia
    {
        public tblCompañia()
        {
            tblClienteNMaquina = new HashSet<tblClienteNMaquina>();
            tblEntidad = new HashSet<tblEntidad>();
            tblGestionRetiro = new HashSet<tblGestionRetiro>();
            tblIncidencia = new HashSet<tblIncidencia>();
            tblInventario = new HashSet<tblInventario>();
            tblKgLavadosLavadora = new HashSet<tblKgLavadosLavadora>();
            tblKgLavadosTunel = new HashSet<tblKgLavadosTunel>();
            tblMovimiento = new HashSet<tblMovimiento>();
            tblPrenda = new HashSet<tblPrenda>();
            tblPrendaNMuestreo = new HashSet<tblPrendaNMuestreo>();
            tblPrendaNMuestreo_FS = new HashSet<tblPrendaNMuestreo_FS>();
            tblProduccion = new HashSet<tblProduccion>();
            tblProduccionMaquinaNCliente = new HashSet<tblProduccionMaquinaNCliente>();
            tblReunion = new HashSet<tblReunion>();
            tblUsuario = new HashSet<tblUsuario>();
        }

        [Key]
        public int idCompañia { get; set; }
        [StringLength(50)]
        public string denominacion { get; set; } = null!;
        [StringLength(3)]
        public string? codigoWeb { get; set; }
        [StringLength(5)]
        public string? idxCompañia { get; set; }
        public int idLavanderia { get; set; }
        public bool? activo { get; set; }
        [StringLength(50)]
        public string? codigoPostal { get; set; }
        [StringLength(50)]
        public string? poblacion { get; set; }
        [StringLength(50)]
        public string? pais { get; set; }
        [StringLength(50)]
        public string? telefono { get; set; }
        [StringLength(50)]
        public string? telefono2 { get; set; }
        [StringLength(50)]
        public string? email { get; set; }
        public bool? visibleGP { get; set; }
        public string? direccion { get; set; }
        [StringLength(50)]
        public string? provincia { get; set; }
        public bool? eliminado { get; set; }

        [ForeignKey("idLavanderia")]
        [InverseProperty("tblCompañia")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
        [InverseProperty("idCompañiaNavigation")]
        public virtual ICollection<tblClienteNMaquina> tblClienteNMaquina { get; set; }
        [InverseProperty("idCompañiaNavigation")]
        public virtual ICollection<tblEntidad> tblEntidad { get; set; }
        [InverseProperty("idCompañiaNavigation")]
        public virtual ICollection<tblGestionRetiro> tblGestionRetiro { get; set; }
        [InverseProperty("idCompañiaNavigation")]
        public virtual ICollection<tblIncidencia> tblIncidencia { get; set; }
        [InverseProperty("idCompañiaNavigation")]
        public virtual ICollection<tblInventario> tblInventario { get; set; }
        [InverseProperty("idCompañiaNavigation")]
        public virtual ICollection<tblKgLavadosLavadora> tblKgLavadosLavadora { get; set; }
        [InverseProperty("idCompañiaNavigation")]
        public virtual ICollection<tblKgLavadosTunel> tblKgLavadosTunel { get; set; }
        [InverseProperty("idCompañiaNavigation")]
        public virtual ICollection<tblMovimiento> tblMovimiento { get; set; }
        [InverseProperty("idCompañiaNavigation")]
        public virtual ICollection<tblPrenda> tblPrenda { get; set; }
        [InverseProperty("idCompañiaNavigation")]
        public virtual ICollection<tblPrendaNMuestreo> tblPrendaNMuestreo { get; set; }
        [InverseProperty("idCompañiaNavigation")]
        public virtual ICollection<tblPrendaNMuestreo_FS> tblPrendaNMuestreo_FS { get; set; }
        [InverseProperty("idCompañiaNavigation")]
        public virtual ICollection<tblProduccion> tblProduccion { get; set; }
        [InverseProperty("idCompañiaNavigation")]
        public virtual ICollection<tblProduccionMaquinaNCliente> tblProduccionMaquinaNCliente { get; set; }
        [InverseProperty("idCompañiaNavigation")]
        public virtual ICollection<tblReunion> tblReunion { get; set; }
        [InverseProperty("idCompañiaNavigation")]
        public virtual ICollection<tblUsuario> tblUsuario { get; set; }
    }
}
