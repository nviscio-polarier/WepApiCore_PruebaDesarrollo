using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTaquilla", Schema = "Locker")]
    public partial class tblTaquilla
    {
        public tblTaquilla()
        {
            tblTaquilla_estado = new HashSet<tblTaquilla_estado>();
            tblTaquilla_movimiento = new HashSet<tblTaquilla_movimiento>();
        }

        [Key]
        public short idTaquilla { get; set; }
        public int idLavanderia { get; set; }
        public string denominacion { get; set; } = null!;
        public byte maxPosicion { get; set; }

        [ForeignKey("idLavanderia")]
        [InverseProperty("tblTaquilla")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
        [InverseProperty("idTaquillaNavigation")]
        public virtual ICollection<tblTaquilla_estado> tblTaquilla_estado { get; set; }
        [InverseProperty("idTaquillaNavigation")]
        public virtual ICollection<tblTaquilla_movimiento> tblTaquilla_movimiento { get; set; }
    }
}
