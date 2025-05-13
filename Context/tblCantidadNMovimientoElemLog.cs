using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCantidadNMovimientoElemLog", Schema = "Logistica")]
    [Index("idMovimientoElemLog", Name = "IX_tblCantidadNMovimientoElemLog_idMovimientoElemLog")]
    public partial class tblCantidadNMovimientoElemLog
    {
        public tblCantidadNMovimientoElemLog()
        {
            tblLecturaCarro = new HashSet<tblLecturaCarro>();
        }

        [Key]
        public int idCantidadNMovimientoElemLog { get; set; }
        public int idMovimientoElemLog { get; set; }
        public byte idTipoElemLog { get; set; }
        public short cantidad { get; set; }
        public byte? idColorTapa { get; set; }
        public byte? idMarcaTapa { get; set; }
        public byte? idGrupoPrendaEst { get; set; }
        public bool? isMezcla { get; set; }

        [ForeignKey("idColorTapa")]
        [InverseProperty("tblCantidadNMovimientoElemLog")]
        public virtual tblColorTapa? idColorTapaNavigation { get; set; }
        [ForeignKey("idGrupoPrendaEst")]
        [InverseProperty("tblCantidadNMovimientoElemLog")]
        public virtual tblGrupoPrendaEst? idGrupoPrendaEstNavigation { get; set; }
        [ForeignKey("idMarcaTapa")]
        [InverseProperty("tblCantidadNMovimientoElemLog")]
        public virtual tblMarcaTapa? idMarcaTapaNavigation { get; set; }
        [ForeignKey("idMovimientoElemLog")]
        [InverseProperty("tblCantidadNMovimientoElemLog")]
        public virtual tblMovimientoElemLog idMovimientoElemLogNavigation { get; set; } = null!;
        [ForeignKey("idTipoElemLog")]
        [InverseProperty("tblCantidadNMovimientoElemLog")]
        public virtual tblTipoElemLog idTipoElemLogNavigation { get; set; } = null!;
        [InverseProperty("idCantidadNMovimientoElemLogNavigation")]
        public virtual ICollection<tblLecturaCarro> tblLecturaCarro { get; set; }
    }
}
