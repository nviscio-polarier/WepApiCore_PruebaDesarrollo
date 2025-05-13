using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCorreosNEntidad", Schema = "General")]
    public partial class tblCorreosNEntidad
    {
        public tblCorreosNEntidad()
        {
            idReport = new HashSet<tblReports>();
        }

        [Key]
        public int idCorreo { get; set; }
        public int? idEntidad { get; set; }
        public string denominacion { get; set; } = null!;

        [ForeignKey("idEntidad")]
        [InverseProperty("tblCorreosNEntidad")]
        public virtual tblEntidad? idEntidadNavigation { get; set; }

        [ForeignKey("idCorreo")]
        [InverseProperty("idCorreo")]
        public virtual ICollection<tblReports> idReport { get; set; }
    }
}
