using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCategoriaInterna", Schema = "RRHH")]
    public partial class tblCategoriaInterna
    {
        public tblCategoriaInterna()
        {
            tblCategoriaInternaNTurno = new HashSet<tblCategoriaInternaNTurno>();
            tblHistoricoNominas = new HashSet<tblHistoricoNominas>();
            tblLlamamiento = new HashSet<tblLlamamiento>();
            tblPersona = new HashSet<tblPersona>();
            idUsuario = new HashSet<tblUsuario>();
        }

        [Key]
        public int idCategoriaInterna { get; set; }
        public int idCategoriaConvenio { get; set; }
        public string denominacion { get; set; } = null!;
        [Column(TypeName = "decimal(14, 2)")]
        public decimal salarioBase { get; set; }
        [Column(TypeName = "decimal(14, 2)")]
        public decimal plusAsistencia { get; set; }
        [Column(TypeName = "decimal(14, 2)")]
        public decimal plusResponsabilidad { get; set; }
        [Column(TypeName = "decimal(14, 2)")]
        public decimal plusPeligrosidad { get; set; }
        [Column(TypeName = "decimal(14, 2)")]
        public decimal incentivo { get; set; }
        [Column(TypeName = "decimal(14, 2)")]
        public decimal impHoraExtra { get; set; }
        [Column(TypeName = "decimal(4, 3)")]
        public decimal percSegSocial { get; set; }
        [Column(TypeName = "decimal(14, 2)")]
        public decimal plusProductividad { get; set; }

        [ForeignKey("idCategoriaConvenio")]
        [InverseProperty("tblCategoriaInterna")]
        public virtual tblCategoriaConvenio idCategoriaConvenioNavigation { get; set; } = null!;
        [InverseProperty("idCategoriaInternaNavigation")]
        public virtual ICollection<tblCategoriaInternaNTurno> tblCategoriaInternaNTurno { get; set; }
        [InverseProperty("idCategoriaInternaNavigation")]
        public virtual ICollection<tblHistoricoNominas> tblHistoricoNominas { get; set; }
        [InverseProperty("idCategoriaInternaNavigation")]
        public virtual ICollection<tblLlamamiento> tblLlamamiento { get; set; }
        [InverseProperty("idCategoriaInternaNavigation")]
        public virtual ICollection<tblPersona> tblPersona { get; set; }

        [ForeignKey("idCategoriaInterna")]
        [InverseProperty("idCategoriaInterna")]
        public virtual ICollection<tblUsuario> idUsuario { get; set; }
    }
}
