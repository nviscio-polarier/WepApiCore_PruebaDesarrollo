using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblRecursoNivel", Schema = "Energeticos")]
    public partial class tblRecursoNivel
    {
        public tblRecursoNivel()
        {
            tblControlNivel = new HashSet<tblControlNivel>();
            tblObjetivosKpiNRecursoNivel = new HashSet<tblObjetivosKpiNRecursoNivel>();
        }

        [Key]
        public int idRecursoNivel { get; set; }
        [StringLength(50)]
        public string denominacion { get; set; } = null!;
        public short idTipoRecursoNivel { get; set; }
        [Column(TypeName = "numeric(10, 2)")]
        public decimal margenLectura { get; set; }
        public byte idUnidadMedida { get; set; }
        public int idLavanderia { get; set; }
        public bool activo { get; set; }
        public byte idUnidadMedidaInforme { get; set; }
        [Column(TypeName = "numeric(12, 4)")]
        public decimal? factorConversion { get; set; }
        public bool eliminado { get; set; }
        public int idCategoriaRecurso { get; set; }

        [ForeignKey("idLavanderia")]
        [InverseProperty("tblRecursoNivel")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
        [ForeignKey("idTipoRecursoNivel")]
        [InverseProperty("tblRecursoNivel")]
        public virtual tblTipoRecursoNivel idTipoRecursoNivelNavigation { get; set; } = null!;
        [ForeignKey("idUnidadMedidaInforme")]
        [InverseProperty("tblRecursoNivelidUnidadMedidaInformeNavigation")]
        public virtual tblUnidadMedida idUnidadMedidaInformeNavigation { get; set; } = null!;
        [ForeignKey("idUnidadMedida")]
        [InverseProperty("tblRecursoNivelidUnidadMedidaNavigation")]
        public virtual tblUnidadMedida idUnidadMedidaNavigation { get; set; } = null!;
        [InverseProperty("idRecursoNivelNavigation")]
        public virtual ICollection<tblControlNivel> tblControlNivel { get; set; }
        [InverseProperty("idRecursoNivelNavigation")]
        public virtual ICollection<tblObjetivosKpiNRecursoNivel> tblObjetivosKpiNRecursoNivel { get; set; }
    }
}
