using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblMovimientoElemLog", Schema = "Logistica")]
    public partial class tblMovimientoElemLog
    {
        public tblMovimientoElemLog()
        {
            tblCantidadNMovimientoElemLog = new HashSet<tblCantidadNMovimientoElemLog>();
            tblMezclaSucioCliente = new HashSet<tblMezclaSucioCliente>();
            tblReparto = new HashSet<tblReparto>();
        }

        [Key]
        public int idMovimientoElemLog { get; set; }
        public byte idEstadoMovimientoElemLog { get; set; }
        [Precision(0)]
        public DateTimeOffset fecha { get; set; }
        public int idEntidad { get; set; }
        public int idLavanderia { get; set; }

        [ForeignKey("idEntidad")]
        [InverseProperty("tblMovimientoElemLog")]
        public virtual tblEntidad idEntidadNavigation { get; set; } = null!;
        [ForeignKey("idEstadoMovimientoElemLog")]
        [InverseProperty("tblMovimientoElemLog")]
        public virtual tblEstadoMovimientoElemLog idEstadoMovimientoElemLogNavigation { get; set; } = null!;
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblMovimientoElemLog")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
        [InverseProperty("idMovimientoElemLogNavigation")]
        public virtual ICollection<tblCantidadNMovimientoElemLog> tblCantidadNMovimientoElemLog { get; set; }
        [InverseProperty("idMovimientoElemLogNavigation")]
        public virtual ICollection<tblMezclaSucioCliente> tblMezclaSucioCliente { get; set; }
        [InverseProperty("idMovimientoElemLogNavigation")]
        public virtual ICollection<tblReparto> tblReparto { get; set; }
    }
}
