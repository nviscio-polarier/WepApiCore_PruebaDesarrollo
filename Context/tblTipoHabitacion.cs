using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoHabitacion", Schema = "Inventarios")]
    public partial class tblTipoHabitacion
    {
        public tblTipoHabitacion()
        {
            tblAlmacen = new HashSet<tblAlmacen>();
            tblAlmacenNInventario = new HashSet<tblAlmacenNInventario>();
            tblPrendaNTipoHabitacion = new HashSet<tblPrendaNTipoHabitacion>();
        }

        [Key]
        public int idTipoHabitacion { get; set; }
        public int idEntidad { get; set; }
        public string denominacion { get; set; } = null!;
        public bool eliminado { get; set; }

        [ForeignKey("idEntidad")]
        [InverseProperty("tblTipoHabitacion")]
        public virtual tblEntidad idEntidadNavigation { get; set; } = null!;
        [InverseProperty("idTipoHabitacionNavigation")]
        public virtual ICollection<tblAlmacen> tblAlmacen { get; set; }
        [InverseProperty("idTipoHabitacionNavigation")]
        public virtual ICollection<tblAlmacenNInventario> tblAlmacenNInventario { get; set; }
        [InverseProperty("idTipoHabitacionNavigation")]
        public virtual ICollection<tblPrendaNTipoHabitacion> tblPrendaNTipoHabitacion { get; set; }
    }
}
