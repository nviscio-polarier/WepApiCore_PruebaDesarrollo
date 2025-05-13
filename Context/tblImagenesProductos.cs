using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblImagenesProductos", Schema = "MyRealBonus")]
    public partial class tblImagenesProductos
    {
        public tblImagenesProductos()
        {
            tblProductosTokens = new HashSet<tblProductosTokens>();
        }

        [Key]
        public int idImagen { get; set; }
        public byte[]? imagenBinario { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string? extension { get; set; }

        [InverseProperty("idImagenProductoNavigation")]
        public virtual ICollection<tblProductosTokens> tblProductosTokens { get; set; }
    }
}
