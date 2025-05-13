using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblDatosSalariales", Schema = "RRHH")]
    public partial class tblDatosSalariales
    {
        public tblDatosSalariales()
        {
            tblDatosSalariales_historico_salarioBase = new HashSet<tblDatosSalariales_historico_salarioBase>();
        }

        [Key]
        public int idPersona { get; set; }
        [Column(TypeName = "decimal(14, 2)")]
        public decimal? salarioBase { get; set; }
        [Column(TypeName = "decimal(14, 2)")]
        public decimal? plusAsistencia { get; set; }
        [Column(TypeName = "decimal(14, 2)")]
        public decimal? plusResponsabilidad { get; set; }
        [Column(TypeName = "decimal(14, 2)")]
        public decimal? plusPeligrosidad { get; set; }
        [Column(TypeName = "decimal(14, 2)")]
        public decimal? incentivo { get; set; }
        public short? numPagas { get; set; }
        [Column(TypeName = "decimal(4, 3)")]
        public decimal? percSegSocial { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? fechaAntiguedad { get; set; }
        public bool? isTrienio { get; set; }
        [Column(TypeName = "decimal(14, 2)")]
        public decimal? plusProductividad { get; set; }
        [Column(TypeName = "decimal(14, 2)")]
        public decimal? impHoraExtra { get; set; }
        [Column(TypeName = "decimal(14, 2)")]
        public decimal? salarioBrutoMensual { get; set; }
        [Column(TypeName = "decimal(14, 2)")]
        public decimal? acuerdoNC { get; set; }
        [Column(TypeName = "decimal(14, 2)")]
        public decimal? salarioEspecie { get; set; }
        public int? fechaAntiguedad_idUsuario_mod { get; set; }
        [Precision(0)]
        public DateTimeOffset? fechaAntiguedad_fecha_mod { get; set; }

        [ForeignKey("fechaAntiguedad_idUsuario_mod")]
        [InverseProperty("tblDatosSalariales")]
        public virtual tblUsuario? fechaAntiguedad_idUsuario_modNavigation { get; set; }
        [ForeignKey("idPersona")]
        [InverseProperty("tblDatosSalariales")]
        public virtual tblPersona idPersonaNavigation { get; set; } = null!;
        [InverseProperty("idPersonaNavigation")]
        public virtual ICollection<tblDatosSalariales_historico_salarioBase> tblDatosSalariales_historico_salarioBase { get; set; }
    }
}
