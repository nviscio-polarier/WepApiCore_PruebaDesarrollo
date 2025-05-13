using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAdmAlbaran_Estado", Schema = "Administracion")]
    public partial class tblAdmAlbaran_Estado
    {
        public tblAdmAlbaran_Estado()
        {
            tblAdmAlbaranCompra = new HashSet<tblAdmAlbaranCompra>();
            tblAdmAlbaranVenta = new HashSet<tblAdmAlbaranVenta>();
        }

        [Key]
        public byte idAdmAlbaran_Estado { get; set; }
        public string? denominacion { get; set; }

        [InverseProperty("idAdmAlbaran_EstadoNavigation")]
        public virtual ICollection<tblAdmAlbaranCompra> tblAdmAlbaranCompra { get; set; }
        [InverseProperty("idAdmAlbaran_EstadoNavigation")]
        public virtual ICollection<tblAdmAlbaranVenta> tblAdmAlbaranVenta { get; set; }
    }
}
