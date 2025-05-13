using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAlmacen", Schema = "Office")]
    public partial class tblAlmacen
    {
        public tblAlmacen()
        {
            InverseidAlmacenPadreNavigation = new HashSet<tblAlmacen>();
            tblAlmacenNInventario = new HashSet<tblAlmacenNInventario>();
            tblAlmacenNRuta = new HashSet<tblAlmacenNRuta>();
            tblPedidoHuesped = new HashSet<tblPedidoHuesped>();
            tblPedidosExtra = new HashSet<tblPedidosExtra>();
            tblPrendaNAlmacen = new HashSet<tblPrendaNAlmacen>();
            tblRevision = new HashSet<tblRevision>();
            tblSubAlmacen = new HashSet<tblSubAlmacen>();
        }

        [Key]
        public int idAlmacen { get; set; }
        public bool activo { get; set; }
        [StringLength(50)]
        public string denominacion { get; set; } = null!;
        public int? idEntidad { get; set; }
        [StringLength(5)]
        public string? idxAlmacen { get; set; }
        public int? idUltimaRevision { get; set; }
        public short? idTipoAlmacen { get; set; }
        public int? idAlmacenPadre { get; set; }
        public int? idLavanderia { get; set; }
        public int? idSeccionNivel2 { get; set; }
        public int? idTipoHabitacion { get; set; }
        public bool eliminado { get; set; }

        [ForeignKey("idAlmacenPadre")]
        [InverseProperty("InverseidAlmacenPadreNavigation")]
        public virtual tblAlmacen? idAlmacenPadreNavigation { get; set; }
        [ForeignKey("idEntidad")]
        [InverseProperty("tblAlmacen")]
        public virtual tblEntidad? idEntidadNavigation { get; set; }
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblAlmacen")]
        public virtual tblLavanderia? idLavanderiaNavigation { get; set; }
        [ForeignKey("idSeccionNivel2")]
        [InverseProperty("tblAlmacen")]
        public virtual tblSeccionNivel2? idSeccionNivel2Navigation { get; set; }
        [ForeignKey("idTipoAlmacen")]
        [InverseProperty("tblAlmacen")]
        public virtual tblTipoAlmacen? idTipoAlmacenNavigation { get; set; }
        [ForeignKey("idTipoHabitacion")]
        [InverseProperty("tblAlmacen")]
        public virtual tblTipoHabitacion? idTipoHabitacionNavigation { get; set; }
        [InverseProperty("idAlmacenPadreNavigation")]
        public virtual ICollection<tblAlmacen> InverseidAlmacenPadreNavigation { get; set; }
        [InverseProperty("idAlmacenNavigation")]
        public virtual ICollection<tblAlmacenNInventario> tblAlmacenNInventario { get; set; }
        [InverseProperty("idAlmacenNavigation")]
        public virtual ICollection<tblAlmacenNRuta> tblAlmacenNRuta { get; set; }
        [InverseProperty("idAlmacenNavigation")]
        public virtual ICollection<tblPedidoHuesped> tblPedidoHuesped { get; set; }
        [InverseProperty("idAlmacenNavigation")]
        public virtual ICollection<tblPedidosExtra> tblPedidosExtra { get; set; }
        [InverseProperty("idAlmacenNavigation")]
        public virtual ICollection<tblPrendaNAlmacen> tblPrendaNAlmacen { get; set; }
        [InverseProperty("idAlmacenNavigation")]
        public virtual ICollection<tblRevision> tblRevision { get; set; }
        [InverseProperty("idAlmacenNavigation")]
        public virtual ICollection<tblSubAlmacen> tblSubAlmacen { get; set; }
    }
}
