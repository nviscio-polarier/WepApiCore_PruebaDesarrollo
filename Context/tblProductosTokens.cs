using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblProductosTokens", Schema = "MyRealBonus")]
    public partial class tblProductosTokens
    {
        public tblProductosTokens()
        {
            tblPersonaTokens = new HashSet<tblPersonaTokens>();
        }

        [Key]
        public int idProducto { get; set; }
        public int idFamiliaProducto { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string denominacion { get; set; } = null!;
        public int precio { get; set; }
        public int? idImagenProducto { get; set; }
        public bool? esDestacado { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? descripcion { get; set; }

        [ForeignKey("idFamiliaProducto")]
        [InverseProperty("tblProductosTokens")]
        public virtual tblFamiliaProducto idFamiliaProductoNavigation { get; set; } = null!;
        [ForeignKey("idImagenProducto")]
        [InverseProperty("tblProductosTokens")]
        public virtual tblImagenesProductos? idImagenProductoNavigation { get; set; }
        [InverseProperty("idTipoProductoNavigation")]
        public virtual ICollection<tblPersonaTokens> tblPersonaTokens { get; set; }
    }
}
