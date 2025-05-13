using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAdmFactura_Estado", Schema = "Administracion")]
    public partial class tblAdmFactura_Estado
    {
        public tblAdmFactura_Estado()
        {
            tblAdmFacturaCompra = new HashSet<tblAdmFacturaCompra>();
            tblAdmFacturaVenta = new HashSet<tblAdmFacturaVenta>();
        }

        [Key]
        public byte idAdmFactura_Estado { get; set; }
        public string? denominacion { get; set; }

        [InverseProperty("idAdmFactura_EstadoNavigation")]
        public virtual ICollection<tblAdmFacturaCompra> tblAdmFacturaCompra { get; set; }
        [InverseProperty("idAdmFactura_EstadoNavigation")]
        public virtual ICollection<tblAdmFacturaVenta> tblAdmFacturaVenta { get; set; }
    }
}
