using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblMantenimientoPrev", Schema = "Assistant")]
    public partial class tblMantenimientoPrev
    {
        public tblMantenimientoPrev()
        {
            idPersona = new HashSet<tblPersona>();
        }

        [Key]
        public int idMantenimientoPrev { get; set; }
        public short idTareaMantenimientoPrev { get; set; }
        public int idUsuario { get; set; }
        public int? idMovimientoRecambio { get; set; }
        public short? cadencia { get; set; }
        public int idMaquina { get; set; }
        [Precision(0)]
        public DateTimeOffset fecha { get; set; }
        [Precision(0)]
        public DateTimeOffset? proxMantenimiento { get; set; }

        [ForeignKey("idMaquina")]
        [InverseProperty("tblMantenimientoPrev")]
        public virtual tblMaquina idMaquinaNavigation { get; set; } = null!;
        [ForeignKey("idMovimientoRecambio")]
        [InverseProperty("tblMantenimientoPrev")]
        public virtual tblMovimientoRecambio? idMovimientoRecambioNavigation { get; set; }
        [ForeignKey("idTareaMantenimientoPrev")]
        [InverseProperty("tblMantenimientoPrev")]
        public virtual tblTareaMantenimientoPrev idTareaMantenimientoPrevNavigation { get; set; } = null!;
        [ForeignKey("idUsuario")]
        [InverseProperty("tblMantenimientoPrev")]
        public virtual tblUsuario idUsuarioNavigation { get; set; } = null!;

        [ForeignKey("idMantenimientoPrev")]
        [InverseProperty("idMantenimientoPrev")]
        public virtual ICollection<tblPersona> idPersona { get; set; }
    }
}
