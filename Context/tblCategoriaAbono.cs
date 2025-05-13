using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCategoriaAbono", Schema = "Logistica")]
    public partial class tblCategoriaAbono
    {
        public tblCategoriaAbono()
        {
            tblAbono = new HashSet<tblAbono>();
            tblSolicitudAbono = new HashSet<tblSolicitudAbono>();
        }

        public string denominacion { get; set; } = null!;
        [Key]
        public byte idCategoriaAbono { get; set; }
        public int? idTraduccion { get; set; }

        [ForeignKey("idTraduccion")]
        [InverseProperty("tblCategoriaAbono")]
        public virtual tblTraduccion? idTraduccionNavigation { get; set; }
        [InverseProperty("idCategoriaAbonoNavigation")]
        public virtual ICollection<tblAbono> tblAbono { get; set; }
        [InverseProperty("idCategoriaAbonoNavigation")]
        public virtual ICollection<tblSolicitudAbono> tblSolicitudAbono { get; set; }
    }
}
