using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTag", Schema = "RFID")]
    public partial class tblTag
    {
        public tblTag()
        {
            tblMovimientoTag = new HashSet<tblMovimientoTag>();
        }

        [Key]
        public int idTag { get; set; }
        public string tagCode { get; set; } = null!;
        public string denominacion { get; set; } = null!;
        public short? idEstado { get; set; }

        [ForeignKey("idEstado")]
        [InverseProperty("tblTag")]
        public virtual tblEstadoTag? idEstadoNavigation { get; set; }
        [InverseProperty("idTagNavigation")]
        public virtual ICollection<tblMovimientoTag> tblMovimientoTag { get; set; }
    }
}
