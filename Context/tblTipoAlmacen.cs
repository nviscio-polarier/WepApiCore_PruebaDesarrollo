using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoAlmacen", Schema = "Office")]
    public partial class tblTipoAlmacen
    {
        public tblTipoAlmacen()
        {
            tblAlmacen = new HashSet<tblAlmacen>();
            tblAlmacenNInventario = new HashSet<tblAlmacenNInventario>();
        }

        [Key]
        public short idTipoAlmacen { get; set; }
        public string denominacion { get; set; } = null!;
        public bool visibleMyOffice { get; set; }

        [InverseProperty("idTipoAlmacenNavigation")]
        public virtual ICollection<tblAlmacen> tblAlmacen { get; set; }
        [InverseProperty("idTipoAlmacenNavigation")]
        public virtual ICollection<tblAlmacenNInventario> tblAlmacenNInventario { get; set; }
    }
}
