using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblArticuloNAdmPedidoProveedor", Schema = "Administracion")]
    public partial class tblArticuloNAdmPedidoProveedor
    {
        [Key]
        public int idArticuloNAdmPedidoProveedor { get; set; }
        public int idAdmPedidoProveedor { get; set; }
        public int idAdmCuentaContable { get; set; }
        public int? idGrupoArticulos { get; set; }
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

        [ForeignKey("idAdmCuentaContable")]
        [InverseProperty("tblArticuloNAdmPedidoProveedor")]
        public virtual tblAdmCuentaContable idAdmCuentaContableNavigation { get; set; } = null!;
        [ForeignKey("idAdmPedidoProveedor")]
        [InverseProperty("tblArticuloNAdmPedidoProveedor")]
        public virtual tblAdmPedidoProveedor idAdmPedidoProveedorNavigation { get; set; } = null!;
        [ForeignKey("idArticuloLenceria")]
        [InverseProperty("tblArticuloNAdmPedidoProveedor")]
        public virtual tblArticuloLenceria? idArticuloLenceriaNavigation { get; set; }
        [ForeignKey("idArticuloLogistico")]
        [InverseProperty("tblArticuloNAdmPedidoProveedor")]
        public virtual tblArticuloLogistico? idArticuloLogisticoNavigation { get; set; }
        [ForeignKey("idArticuloMaquinaria")]
        [InverseProperty("tblArticuloNAdmPedidoProveedor")]
        public virtual tblArticuloMaquinaria? idArticuloMaquinariaNavigation { get; set; }
        [ForeignKey("idGrupoArticulos")]
        [InverseProperty("tblArticuloNAdmPedidoProveedor")]
        public virtual tblGrupoArticulos? idGrupoArticulosNavigation { get; set; }
        [ForeignKey("idIvaNPais")]
        [InverseProperty("tblArticuloNAdmPedidoProveedor")]
        public virtual tblIvaNPais? idIvaNPaisNavigation { get; set; }
        [ForeignKey("idRecambio")]
        [InverseProperty("tblArticuloNAdmPedidoProveedor")]
        public virtual tblRecambio? idRecambioNavigation { get; set; }
    }
}
