using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblModuloNLavanderia_Ponderacion", Schema = "ControlUso")]
    public partial class tblModuloNLavanderia_Ponderacion
    {
        [Key]
        public int idLavanderia { get; set; }
        [Key]
        public short idModulo { get; set; }
        [Column(TypeName = "decimal(5, 4)")]
        public decimal ponderacion { get; set; }

        [ForeignKey("idLavanderia")]
        [InverseProperty("tblModuloNLavanderia_Ponderacion")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
        [ForeignKey("idModulo")]
        [InverseProperty("tblModuloNLavanderia_Ponderacion")]
        public virtual tblModulo idModuloNavigation { get; set; } = null!;
    }
}
