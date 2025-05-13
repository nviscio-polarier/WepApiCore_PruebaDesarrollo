using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblEstadoTag", Schema = "RFID")]
    public partial class tblEstadoTag
    {
        public tblEstadoTag()
        {
            tblMovimientoTag = new HashSet<tblMovimientoTag>();
            tblTag = new HashSet<tblTag>();
        }

        [Key]
        public short idEstado { get; set; }
        public string denominacion { get; set; } = null!;

        [InverseProperty("idEstadoNavigation")]
        public virtual ICollection<tblMovimientoTag> tblMovimientoTag { get; set; }
        [InverseProperty("idEstadoNavigation")]
        public virtual ICollection<tblTag> tblTag { get; set; }
    }
}
