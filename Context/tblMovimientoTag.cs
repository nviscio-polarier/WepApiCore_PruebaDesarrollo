using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblMovimientoTag", Schema = "RFID")]
    public partial class tblMovimientoTag
    {
        [Key]
        public int idTag { get; set; }
        [Key]
        public short idEstado { get; set; }
        [Key]
        public DateTimeOffset fecha { get; set; }
        public int? idLavanderia { get; set; }
        public int? idEntidad { get; set; }

        [ForeignKey("idEntidad")]
        [InverseProperty("tblMovimientoTag")]
        public virtual tblEntidad? idEntidadNavigation { get; set; }
        [ForeignKey("idEstado")]
        [InverseProperty("tblMovimientoTag")]
        public virtual tblEstadoTag idEstadoNavigation { get; set; } = null!;
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblMovimientoTag")]
        public virtual tblLavanderia? idLavanderiaNavigation { get; set; }
        [ForeignKey("idTag")]
        [InverseProperty("tblMovimientoTag")]
        public virtual tblTag idTagNavigation { get; set; } = null!;
    }
}
