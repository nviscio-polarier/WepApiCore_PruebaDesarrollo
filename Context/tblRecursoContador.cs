using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblRecursoContador", Schema = "Energeticos")]
    public partial class tblRecursoContador
    {
        public tblRecursoContador()
        {
            tblControlContador = new HashSet<tblControlContador>();
            tblLecturaContador = new HashSet<tblLecturaContador>();
            tblRecursoVirtual_CalculoidRecursoContadorNavigation = new HashSet<tblRecursoVirtual_Calculo>();
            tblRecursoVirtual_CalculoidRecursoVirtualNavigation = new HashSet<tblRecursoVirtual_Calculo>();
        }

        [Key]
        public int idRecursoContador { get; set; }
        public string denominacion { get; set; } = null!;
        public byte idUnidadMedida_Contador { get; set; }
        [Column(TypeName = "decimal(12, 4)")]
        public decimal? factorNormalizacion { get; set; }
        [Column(TypeName = "decimal(12, 4)")]
        public decimal? poderCalorifico { get; set; }
        public byte idUnidadMedida_Kpi { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? margen { get; set; }
        public int idLavanderia { get; set; }
        public bool activo { get; set; }
        public bool eliminado { get; set; }
        public byte idCategoriaRecurso { get; set; }
        [Column(TypeName = "numeric(9, 4)")]
        public decimal valorPulsoEnergyHub { get; set; }
        public byte? idGrupoEnergetico { get; set; }
        [Column(TypeName = "decimal(12, 3)")]
        public decimal? densidad { get; set; }
        public bool sumaInforme { get; set; }
        public bool isVisibleDashboard { get; set; }
        public bool isVirtual { get; set; }
        public bool? isAutomatico { get; set; }
        public double factorConversion { get; set; }

        [ForeignKey("idCategoriaRecurso")]
        [InverseProperty("tblRecursoContador")]
        public virtual tblCategoriaRecurso idCategoriaRecursoNavigation { get; set; } = null!;
        [ForeignKey("idGrupoEnergetico")]
        [InverseProperty("tblRecursoContador")]
        public virtual tblGrupoEnergetico? idGrupoEnergeticoNavigation { get; set; }
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblRecursoContador")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
        [ForeignKey("idUnidadMedida_Contador")]
        [InverseProperty("tblRecursoContadoridUnidadMedida_ContadorNavigation")]
        public virtual tblUnidadMedida idUnidadMedida_ContadorNavigation { get; set; } = null!;
        [ForeignKey("idUnidadMedida_Kpi")]
        [InverseProperty("tblRecursoContadoridUnidadMedida_KpiNavigation")]
        public virtual tblUnidadMedida idUnidadMedida_KpiNavigation { get; set; } = null!;
        [InverseProperty("idRecursoContadorNavigation")]
        public virtual ICollection<tblControlContador> tblControlContador { get; set; }
        [InverseProperty("idRecursoContadorNavigation")]
        public virtual ICollection<tblLecturaContador> tblLecturaContador { get; set; }
        [InverseProperty("idRecursoContadorNavigation")]
        public virtual ICollection<tblRecursoVirtual_Calculo> tblRecursoVirtual_CalculoidRecursoContadorNavigation { get; set; }
        [InverseProperty("idRecursoVirtualNavigation")]
        public virtual ICollection<tblRecursoVirtual_Calculo> tblRecursoVirtual_CalculoidRecursoVirtualNavigation { get; set; }
    }
}
