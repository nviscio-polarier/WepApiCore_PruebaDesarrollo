using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblSubAlmacen", Schema = "Office")]
    public partial class tblSubAlmacen
    {
        public tblSubAlmacen()
        {
            tblPedidosExtra = new HashSet<tblPedidosExtra>();
            tblPrendaNSubAlmacen = new HashSet<tblPrendaNSubAlmacen>();
            tblRevision = new HashSet<tblRevision>();
        }

        [Key]
        public int idSubAlmacen { get; set; }
        public string codigoSubAlmacen { get; set; } = null!;
        public string denominacion { get; set; } = null!;
        public int idAlmacen { get; set; }
        public bool activo { get; set; }
        [StringLength(5)]
        public string idxSubAlmacen { get; set; } = null!;
        public int? idUltimaRevision { get; set; }

        [ForeignKey("idAlmacen")]
        [InverseProperty("tblSubAlmacen")]
        public virtual tblAlmacen idAlmacenNavigation { get; set; } = null!;
        [InverseProperty("idSubAlmacenNavigation")]
        public virtual ICollection<tblPedidosExtra> tblPedidosExtra { get; set; }
        [InverseProperty("idSubAlmacenNavigation")]
        public virtual ICollection<tblPrendaNSubAlmacen> tblPrendaNSubAlmacen { get; set; }
        [InverseProperty("idSubAlmacenNavigation")]
        public virtual ICollection<tblRevision> tblRevision { get; set; }
    }
}
