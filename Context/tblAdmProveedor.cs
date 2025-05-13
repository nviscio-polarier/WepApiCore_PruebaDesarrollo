using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAdmProveedor", Schema = "Administracion")]
    public partial class tblAdmProveedor
    {
        public tblAdmProveedor()
        {
            tblAdmAlbaranCompra = new HashSet<tblAdmAlbaranCompra>();
            tblAdmFacturaCompra = new HashSet<tblAdmFacturaCompra>();
            tblAdmPedidoProveedor = new HashSet<tblAdmPedidoProveedor>();
            tblImagenNProveedor = new HashSet<tblImagenNProveedor>();
        }

        [Key]
        public int idAdmProveedor { get; set; }
        [StringLength(8)]
        public string codigo { get; set; } = null!;
        public string nombreFiscal { get; set; } = null!;
        public string nombreComercial { get; set; } = null!;
        public string CIF { get; set; } = null!;
        public string direccion { get; set; } = null!;
        public short idEmpresaPolarier { get; set; }
        public bool isEliminado { get; set; }
        public int? idAdmFormaPago { get; set; }
        public int? idAdmCondicionPago { get; set; }
        public byte? idMoneda { get; set; }
        public int? idPais { get; set; }
        public string? provincia { get; set; }
        public string? poblacion { get; set; }
        public string? codigoPostal { get; set; }
        public string? telfMovil { get; set; }
        public string? telfFijo { get; set; }
        public string? email { get; set; }

        [ForeignKey("idAdmCondicionPago")]
        [InverseProperty("tblAdmProveedor")]
        public virtual tblAdmCondicionPago? idAdmCondicionPagoNavigation { get; set; }
        [ForeignKey("idAdmFormaPago")]
        [InverseProperty("tblAdmProveedor")]
        public virtual tblAdmFormaPago? idAdmFormaPagoNavigation { get; set; }
        [ForeignKey("idEmpresaPolarier")]
        [InverseProperty("tblAdmProveedor")]
        public virtual tblEmpresasPolarier idEmpresaPolarierNavigation { get; set; } = null!;
        [ForeignKey("idMoneda")]
        [InverseProperty("tblAdmProveedor")]
        public virtual tblMoneda? idMonedaNavigation { get; set; }
        [ForeignKey("idPais")]
        [InverseProperty("tblAdmProveedor")]
        public virtual tblPais? idPaisNavigation { get; set; }
        [InverseProperty("idAdmProveedorNavigation")]
        public virtual ICollection<tblAdmAlbaranCompra> tblAdmAlbaranCompra { get; set; }
        [InverseProperty("idAdmProveedorNavigation")]
        public virtual ICollection<tblAdmFacturaCompra> tblAdmFacturaCompra { get; set; }
        [InverseProperty("idAdmProveedorNavigation")]
        public virtual ICollection<tblAdmPedidoProveedor> tblAdmPedidoProveedor { get; set; }
        [InverseProperty("idAdmProveedorNavigation")]
        public virtual ICollection<tblImagenNProveedor> tblImagenNProveedor { get; set; }
    }
}
