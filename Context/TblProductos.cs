using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblProductos", Schema = "MyRealBonus")]
    public partial class tblProductos
    {
        public tblProductos()
        {
            tblPersonaTokens = new HashSet<tblPersonaTokens>();
        }

        [Key]
        public int idProducto { get; set; }
        public int? idFamiliaProducto { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? denominacion { get; set; }

        [ForeignKey("idFamiliaProducto")]
        [InverseProperty("tblProductos")]
        public virtual tblFamiliaProducto? idFamiliaProductoNavigation { get; set; }
        [InverseProperty("IdProductoNavigation")]
        public virtual tblImagenesProductos tblImagenesProductos { get; set; } = null!;
        [InverseProperty("idTipoProductoNavigation")]
        public virtual ICollection<tblPersonaTokens> tblPersonaTokens { get; set; }
    }
}
