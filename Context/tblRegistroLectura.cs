using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblRegistroLectura", Schema = "Logistica")]
    public partial class tblRegistroLectura
    {
        [Key]
        public int idRegistroLectura { get; set; }
        public int? idCarro { get; set; }
        public DateTimeOffset? fecha { get; set; }
        public byte? idEstado { get; set; }
        public int? idEntidad { get; set; }

        [ForeignKey("idEntidad")]
        [InverseProperty("tblRegistroLectura")]
        public virtual tblEntidad? idEntidadNavigation { get; set; }
        [ForeignKey("idEstado")]
        [InverseProperty("tblRegistroLectura")]
        public virtual tblRegistroEstado? idEstadoNavigation { get; set; }
    }
}
