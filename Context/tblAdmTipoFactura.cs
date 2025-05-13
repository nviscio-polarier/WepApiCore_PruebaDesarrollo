using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAdmTipoFactura", Schema = "Administracion")]
    public partial class tblAdmTipoFactura
    {
        public tblAdmTipoFactura()
        {
            tblAdmAlbaranVenta = new HashSet<tblAdmAlbaranVenta>();
            tblAdmFacturaCompra = new HashSet<tblAdmFacturaCompra>();
            tblAdmFacturaVenta = new HashSet<tblAdmFacturaVenta>();
        }

        [Key]
        public int idTipoFactura { get; set; }
        public string? denominacion { get; set; }

        [InverseProperty("idTipoFacturaNavigation")]
        public virtual ICollection<tblAdmAlbaranVenta> tblAdmAlbaranVenta { get; set; }
        [InverseProperty("idTipoFacturaNavigation")]
        public virtual ICollection<tblAdmFacturaCompra> tblAdmFacturaCompra { get; set; }
        [InverseProperty("idTipoFacturaNavigation")]
        public virtual ICollection<tblAdmFacturaVenta> tblAdmFacturaVenta { get; set; }
    }
}
