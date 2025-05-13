using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPuerto", Schema = "Logistica")]
    public partial class tblPuerto
    {
        public tblPuerto()
        {
            tblEnvioidPuertoCargaNavigation = new HashSet<tblEnvio>();
            tblEnvioidPuertoDestinoNavigation = new HashSet<tblEnvio>();
        }

        [Key]
        public int idPuerto { get; set; }
        public string denominacion { get; set; } = null!;
        public int? idPais { get; set; }

        [ForeignKey("idPais")]
        [InverseProperty("tblPuerto")]
        public virtual tblPais? idPaisNavigation { get; set; }
        [InverseProperty("idPuertoCargaNavigation")]
        public virtual ICollection<tblEnvio> tblEnvioidPuertoCargaNavigation { get; set; }
        [InverseProperty("idPuertoDestinoNavigation")]
        public virtual ICollection<tblEnvio> tblEnvioidPuertoDestinoNavigation { get; set; }
    }
}
