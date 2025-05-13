using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblEstadoOffice", Schema = "Office")]
    public partial class tblEstadoOffice
    {
        public tblEstadoOffice()
        {
            tblPedidosExtra = new HashSet<tblPedidosExtra>();
            tblRevision = new HashSet<tblRevision>();
        }

        [Key]
        public int idEstadoOffice { get; set; }
        [StringLength(50)]
        public string denominacion { get; set; } = null!;
        public byte? codigo { get; set; }

        [InverseProperty("idEstadoOfficeNavigation")]
        public virtual ICollection<tblPedidosExtra> tblPedidosExtra { get; set; }
        [InverseProperty("idEstadoOfficeNavigation")]
        public virtual ICollection<tblRevision> tblRevision { get; set; }
    }
}
