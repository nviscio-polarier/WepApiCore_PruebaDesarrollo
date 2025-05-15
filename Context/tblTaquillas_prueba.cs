using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTaquillas_prueba", Schema = "MyRealBonus")]
    public partial class tblTaquillas_prueba
    {
        public tblTaquillas_prueba()
        {
            tblTaquillas_Movimiento_prueba = new HashSet<tblTaquillas_Movimiento_prueba>();
        }

        [Key]
        public int idTaquilla { get; set; }
        [StringLength(1)]
        [Unicode(false)]
        public string denominacion { get; set; } = null!;
        public int? idLavanderia { get; set; }
        public int tamaño { get; set; }

        [ForeignKey("idLavanderia")]
        [InverseProperty("tblTaquillas_prueba")]
        public virtual tblLavanderia? idLavanderiaNavigation { get; set; }
        [InverseProperty("idTaquillaNavigation")]
        public virtual ICollection<tblTaquillas_Movimiento_prueba> tblTaquillas_Movimiento_prueba { get; set; }
    }
}
