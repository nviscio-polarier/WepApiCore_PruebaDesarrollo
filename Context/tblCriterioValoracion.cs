using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCriterioValoracion", Schema = "ControlUso")]
    public partial class tblCriterioValoracion
    {
        public tblCriterioValoracion()
        {
            tblCriterioValoracionNLavanderia = new HashSet<tblCriterioValoracionNLavanderia>();
        }

        [Key]
        public short idCriterioValoracion { get; set; }
        public string denominacion { get; set; } = null!;
        public short? idModulo { get; set; }

        [ForeignKey("idModulo")]
        [InverseProperty("tblCriterioValoracion")]
        public virtual tblModulo? idModuloNavigation { get; set; }
        [InverseProperty("idCriterioValoracionNavigation")]
        public virtual ICollection<tblCriterioValoracionNLavanderia> tblCriterioValoracionNLavanderia { get; set; }
    }
}
