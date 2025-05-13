using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCorreosNLav", Schema = "General")]
    public partial class tblCorreosNLav
    {
        public tblCorreosNLav()
        {
            idEnvio = new HashSet<tblEnvio>();
            idReport = new HashSet<tblReports>();
            idTipoIncidencia = new HashSet<tblTipoIncidencia>();
        }

        public int? idLavanderia { get; set; }
        [Key]
        public int idCorreo { get; set; }
        public string? denominacion { get; set; }

        [ForeignKey("idLavanderia")]
        [InverseProperty("tblCorreosNLav")]
        public virtual tblLavanderia? idLavanderiaNavigation { get; set; }

        [ForeignKey("idCorreo")]
        [InverseProperty("idCorreo")]
        public virtual ICollection<tblEnvio> idEnvio { get; set; }
        [ForeignKey("idCorreo")]
        [InverseProperty("idCorreoNavigation")]
        public virtual ICollection<tblReports> idReport { get; set; }
        [ForeignKey("idCorreo")]
        [InverseProperty("idCorreo")]
        public virtual ICollection<tblTipoIncidencia> idTipoIncidencia { get; set; }
    }
}
