using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblFamiliaProducto", Schema = "MyRealBonus")]
    public partial class tblFamiliaProducto
    {
        public tblFamiliaProducto()
        {
            tblProductosTokens = new HashSet<tblProductosTokens>();
        }

        [Key]
        public int idFamiliaProducto { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? denominacion { get; set; }

        [InverseProperty("idFamiliaProductoNavigation")]
        public virtual ICollection<tblProductosTokens> tblProductosTokens { get; set; }
    }
}
