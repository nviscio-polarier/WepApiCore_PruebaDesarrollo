using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCriterioValoracionNLavanderia", Schema = "ControlUso")]
    public partial class tblCriterioValoracionNLavanderia
    {
        [Key]
        public short idCriterioValoracion { get; set; }
        [Key]
        public int idLavanderia { get; set; }
        [Column(TypeName = "decimal(5, 4)")]
        public decimal ponderacion { get; set; }

        [ForeignKey("idCriterioValoracion")]
        [InverseProperty("tblCriterioValoracionNLavanderia")]
        public virtual tblCriterioValoracion idCriterioValoracionNavigation { get; set; } = null!;
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblCriterioValoracionNLavanderia")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
    }
}
