using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAdmTipoArticulo", Schema = "Administracion")]
    public partial class tblAdmTipoArticulo
    {
        [Key]
        public int idAdmTipoArticulo { get; set; }
        public string? denominacion { get; set; }
        public int idAdmCuentaContableCompra { get; set; }
        public int? idAdmCuentaContableVenta { get; set; }

        [ForeignKey("idAdmCuentaContableCompra")]
        [InverseProperty("tblAdmTipoArticuloidAdmCuentaContableCompraNavigation")]
        public virtual tblAdmCuentaContable idAdmCuentaContableCompraNavigation { get; set; } = null!;
        [ForeignKey("idAdmCuentaContableVenta")]
        [InverseProperty("tblAdmTipoArticuloidAdmCuentaContableVentaNavigation")]
        public virtual tblAdmCuentaContable? idAdmCuentaContableVentaNavigation { get; set; }
    }
}
