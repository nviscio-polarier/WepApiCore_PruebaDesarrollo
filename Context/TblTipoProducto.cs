using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("TblTipoProducto", Schema = "MyRealBonus")]
    public partial class TblTipoProducto
    {
        public TblTipoProducto()
        {
            TblPersonaTokens = new HashSet<tblPersonaTokens>();
        }

        [Key]
        public int idTipoProducto { get; set; }
        public int? idFamiliaProducto { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? denominacion { get; set; }

        [ForeignKey("idFamiliaProducto")]
        [InverseProperty("TblTipoProducto")]
        public virtual tblFamiliaProducto? idFamiliaProductoNavigation { get; set; }
        [InverseProperty("idTipoProductoNavigation")]
        public virtual ICollection<tblPersonaTokens> TblPersonaTokens { get; set; }
    }
}
