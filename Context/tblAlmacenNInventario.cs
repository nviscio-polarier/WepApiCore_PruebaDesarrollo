using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAlmacenNInventario", Schema = "Inventarios")]
    public partial class tblAlmacenNInventario
    {
        public tblAlmacenNInventario()
        {
            tblPrendaNAlmacenNInventario = new HashSet<tblPrendaNAlmacenNInventario>();
        }

        [Key]
        public int idInventario { get; set; }
        [Key]
        public int idAlmacen { get; set; }
        public int? idPersona { get; set; }
        public byte estado { get; set; }
        public short? idTipoAlmacen { get; set; }
        public int? idTipoHabitacion { get; set; }

        [ForeignKey("idAlmacen")]
        [InverseProperty("tblAlmacenNInventario")]
        public virtual tblAlmacen idAlmacenNavigation { get; set; } = null!;
        [ForeignKey("idInventario")]
        [InverseProperty("tblAlmacenNInventario")]
        public virtual tblInventario idInventarioNavigation { get; set; } = null!;
        [ForeignKey("idPersona")]
        [InverseProperty("tblAlmacenNInventario")]
        public virtual tblPersona? idPersonaNavigation { get; set; }
        [ForeignKey("idTipoAlmacen")]
        [InverseProperty("tblAlmacenNInventario")]
        public virtual tblTipoAlmacen? idTipoAlmacenNavigation { get; set; }
        [ForeignKey("idTipoHabitacion")]
        [InverseProperty("tblAlmacenNInventario")]
        public virtual tblTipoHabitacion? idTipoHabitacionNavigation { get; set; }
        [InverseProperty("id")]
        public virtual ICollection<tblPrendaNAlmacenNInventario> tblPrendaNAlmacenNInventario { get; set; }
    }
}
