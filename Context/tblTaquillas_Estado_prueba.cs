using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTaquillas_Estado_prueba", Schema = "MyRealBonus")]
    public partial class tblTaquillas_Estado_prueba
    {
        [Key]
        public int idEstado { get; set; }
        public int idTaquilla { get; set; }
        public int posicion { get; set; }
        public bool? disponible { get; set; }

        [ForeignKey("idTaquilla")]
        [InverseProperty("tblTaquillas_Estado_prueba")]
        public virtual tblTaquillas_prueba idTaquillaNavigation { get; set; } = null!;
    }
}
