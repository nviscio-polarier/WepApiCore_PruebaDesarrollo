using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoDiaCuadrante", Schema = "RRHH")]
    public partial class tblTipoDiaCuadrante
    {
        public tblTipoDiaCuadrante()
        {
            tblDiasCuadrante = new HashSet<tblDiasCuadrante>();
        }

        [Key]
        public int idTipoDiaCuadrante { get; set; }
        [StringLength(50)]
        public string denominacion { get; set; } = null!;
        [StringLength(2)]
        public string? abreviatura { get; set; }

        [InverseProperty("idTipoDiaCuadranteNavigation")]
        public virtual ICollection<tblDiasCuadrante> tblDiasCuadrante { get; set; }
    }
}
