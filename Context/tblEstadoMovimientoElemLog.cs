using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblEstadoMovimientoElemLog", Schema = "Logistica")]
    public partial class tblEstadoMovimientoElemLog
    {
        public tblEstadoMovimientoElemLog()
        {
            tblLecturaCarro = new HashSet<tblLecturaCarro>();
            tblMovimientoElemLog = new HashSet<tblMovimientoElemLog>();
        }

        [Key]
        public byte idEstadoMovimientoElemLog { get; set; }
        public string? denominacion { get; set; }
        public int idTraduccion { get; set; }

        [ForeignKey("idTraduccion")]
        [InverseProperty("tblEstadoMovimientoElemLog")]
        public virtual tblTraduccion idTraduccionNavigation { get; set; } = null!;
        [InverseProperty("idEstadoMovimientoElemLogNavigation")]
        public virtual ICollection<tblLecturaCarro> tblLecturaCarro { get; set; }
        [InverseProperty("idEstadoMovimientoElemLogNavigation")]
        public virtual ICollection<tblMovimientoElemLog> tblMovimientoElemLog { get; set; }
    }
}
