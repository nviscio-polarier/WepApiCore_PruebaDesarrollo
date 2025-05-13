using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAdmCondicionPago", Schema = "Administracion")]
    public partial class tblAdmCondicionPago
    {
        public tblAdmCondicionPago()
        {
            tblAdmCliente = new HashSet<tblAdmCliente>();
            tblAdmFacturaCompra = new HashSet<tblAdmFacturaCompra>();
            tblAdmFacturaVenta = new HashSet<tblAdmFacturaVenta>();
            tblAdmProveedor = new HashSet<tblAdmProveedor>();
        }

        [Key]
        public int idAdmCondicionPago { get; set; }
        public string codigoCondicion { get; set; } = null!;
        public string codigoTermino { get; set; } = null!;
        public string denominacionCondicion { get; set; } = null!;
        public string denominacionTermino { get; set; } = null!;
        public short? numDiasPago { get; set; }

        [InverseProperty("idAdmCondicionPagoNavigation")]
        public virtual ICollection<tblAdmCliente> tblAdmCliente { get; set; }
        [InverseProperty("idAdmCondicionPagoNavigation")]
        public virtual ICollection<tblAdmFacturaCompra> tblAdmFacturaCompra { get; set; }
        [InverseProperty("idAdmCondicionPagoNavigation")]
        public virtual ICollection<tblAdmFacturaVenta> tblAdmFacturaVenta { get; set; }
        [InverseProperty("idAdmCondicionPagoNavigation")]
        public virtual ICollection<tblAdmProveedor> tblAdmProveedor { get; set; }
    }
}
