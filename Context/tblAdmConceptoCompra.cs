using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAdmConceptoCompra", Schema = "Administracion")]
    public partial class tblAdmConceptoCompra
    {
        [Key]
        public int idAdmConceptoCompra { get; set; }
        [Key]
        public int idAdmFacturaCompra { get; set; }
        public int idAdmCuentaContable { get; set; }
        public string? descripcion { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? cantidad { get; set; }
        [Column(TypeName = "decimal(14, 3)")]
        public decimal? precio { get; set; }
        [Column(TypeName = "decimal(5, 4)")]
        public decimal descuento { get; set; }
        public byte? idIvaNPais { get; set; }

        [ForeignKey("idAdmCuentaContable")]
        [InverseProperty("tblAdmConceptoCompra")]
        public virtual tblAdmCuentaContable idAdmCuentaContableNavigation { get; set; } = null!;
        [ForeignKey("idAdmFacturaCompra")]
        [InverseProperty("tblAdmConceptoCompra")]
        public virtual tblAdmFacturaCompra idAdmFacturaCompraNavigation { get; set; } = null!;
        [ForeignKey("idIvaNPais")]
        [InverseProperty("tblAdmConceptoCompra")]
        public virtual tblIvaNPais? idIvaNPaisNavigation { get; set; }
    }
}
