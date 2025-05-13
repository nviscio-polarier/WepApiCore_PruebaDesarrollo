using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblModulo", Schema = "GestionInterna")]
    public partial class tblModulo
    {
        public tblModulo()
        {
            tblCriterioValoracion = new HashSet<tblCriterioValoracion>();
            tblModuloNLavanderia_Ponderacion = new HashSet<tblModuloNLavanderia_Ponderacion>();
            idFormulario = new HashSet<tblFormulario>();
            idLavanderia = new HashSet<tblLavanderia>();
        }

        [Key]
        public short idModulo { get; set; }
        [StringLength(50)]
        public string denominacion { get; set; } = null!;
        public short codigo { get; set; }

        [InverseProperty("idModuloNavigation")]
        public virtual ICollection<tblCriterioValoracion> tblCriterioValoracion { get; set; }
        [InverseProperty("idModuloNavigation")]
        public virtual ICollection<tblModuloNLavanderia_Ponderacion> tblModuloNLavanderia_Ponderacion { get; set; }

        [ForeignKey("idModulo")]
        [InverseProperty("idModulo")]
        public virtual ICollection<tblFormulario> idFormulario { get; set; }
        [ForeignKey("idModulo")]
        [InverseProperty("idModulo")]
        public virtual ICollection<tblLavanderia> idLavanderia { get; set; }
    }
}
