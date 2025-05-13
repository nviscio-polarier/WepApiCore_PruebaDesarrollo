using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblArticuloNAdmAlbaranCompra", Schema = "Administracion")]
    public partial class tblArticuloNAdmAlbaranCompra
    {
        [Key]
        public int idArticuloNAdmAlbaranCompra { get; set; }
        public int idAdmAlbaranCompra { get; set; }
        public int? idGrupoArticulos { get; set; }
        public int idAdmCuentaContable { get; set; }
        public int? idArticuloLenceria { get; set; }
        public int? idArticuloMaquinaria { get; set; }
        public int? idRecambio { get; set; }
        public string? descripcion { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? cantidad { get; set; }
        [Column(TypeName = "decimal(14, 3)")]
        public decimal? precio { get; set; }
        [Column(TypeName = "decimal(5, 4)")]
        public decimal descuento { get; set; }
        public byte? idIvaNPais { get; set; }
        public int? idArticuloLogistico { get; set; }

        [ForeignKey("idAdmAlbaranCompra")]
        [InverseProperty("tblArticuloNAdmAlbaranCompra")]
        public virtual tblAdmAlbaranCompra idAdmAlbaranCompraNavigation { get; set; } = null!;
        [ForeignKey("idAdmCuentaContable")]
        [InverseProperty("tblArticuloNAdmAlbaranCompra")]
        public virtual tblAdmCuentaContable idAdmCuentaContableNavigation { get; set; } = null!;
        [ForeignKey("idArticuloLenceria")]
        [InverseProperty("tblArticuloNAdmAlbaranCompra")]
        public virtual tblArticuloLenceria? idArticuloLenceriaNavigation { get; set; }
        [ForeignKey("idArticuloLogistico")]
        [InverseProperty("tblArticuloNAdmAlbaranCompra")]
        public virtual tblArticuloLogistico? idArticuloLogisticoNavigation { get; set; }
        [ForeignKey("idArticuloMaquinaria")]
        [InverseProperty("tblArticuloNAdmAlbaranCompra")]
        public virtual tblArticuloMaquinaria? idArticuloMaquinariaNavigation { get; set; }
        [ForeignKey("idGrupoArticulos")]
        [InverseProperty("tblArticuloNAdmAlbaranCompra")]
        public virtual tblGrupoArticulos? idGrupoArticulosNavigation { get; set; }
        [ForeignKey("idIvaNPais")]
        [InverseProperty("tblArticuloNAdmAlbaranCompra")]
        public virtual tblIvaNPais? idIvaNPaisNavigation { get; set; }
        [ForeignKey("idRecambio")]
        [InverseProperty("tblArticuloNAdmAlbaranCompra")]
        public virtual tblRecambio? idRecambioNavigation { get; set; }
    }
}
