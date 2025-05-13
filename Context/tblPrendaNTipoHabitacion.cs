using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPrendaNTipoHabitacion", Schema = "Inventarios")]
    public partial class tblPrendaNTipoHabitacion
    {
        [Key]
        public int idTipoHabitacion { get; set; }
        [Key]
        public int idPrenda { get; set; }
        public int parStock { get; set; }

        [ForeignKey("idPrenda")]
        [InverseProperty("tblPrendaNTipoHabitacion")]
        public virtual tblPrenda idPrendaNavigation { get; set; } = null!;
        [ForeignKey("idTipoHabitacion")]
        [InverseProperty("tblPrendaNTipoHabitacion")]
        public virtual tblTipoHabitacion idTipoHabitacionNavigation { get; set; } = null!;
    }
}
