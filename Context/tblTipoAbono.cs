using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoAbono", Schema = "Logistica")]
    public partial class tblTipoAbono
    {
        public tblTipoAbono()
        {
            tblAbono = new HashSet<tblAbono>();
        }

        [StringLength(50)]
        public string? denominacion { get; set; }
        [Key]
        public byte idTipoAbono { get; set; }

        [InverseProperty("idTipoAbonoNavigation")]
        public virtual ICollection<tblAbono> tblAbono { get; set; }
    }
}
