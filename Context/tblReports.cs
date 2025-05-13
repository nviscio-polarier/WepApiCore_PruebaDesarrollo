using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblReports", Schema = "General")]
    public partial class tblReports
    {
        public tblReports()
        {
            idCorreo = new HashSet<tblCorreosNEntidad>();
            idCorreoNavigation = new HashSet<tblCorreosNLav>();
        }

        [Key]
        public int idReport { get; set; }
        public string denominacion { get; set; } = null!;
        public byte[] layoutData { get; set; } = null!;
        public int? idFormulario { get; set; }
        public string? nombreVisible { get; set; }

        [ForeignKey("idFormulario")]
        [InverseProperty("tblReports")]
        public virtual tblFormulario? idFormularioNavigation { get; set; }

        [ForeignKey("idReport")]
        [InverseProperty("idReport")]
        public virtual ICollection<tblCorreosNEntidad> idCorreo { get; set; }
        [ForeignKey("idReport")]
        [InverseProperty("idReport")]
        public virtual ICollection<tblCorreosNLav> idCorreoNavigation { get; set; }
    }
}
