using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblArticuloNAdmPedidoCliente", Schema = "Administracion")]
    public partial class tblArticuloNAdmPedidoCliente
    {
        [Key]
        public int idArticuloNAdmPedidoCliente { get; set; }
        public int idAdmPedidoCliente { get; set; }
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
        public int? idArticuloLogistico { get; set; }

        [ForeignKey("idAdmCuentaContable")]
        [InverseProperty("tblArticuloNAdmPedidoCliente")]
        public virtual tblAdmCuentaContable idAdmCuentaContableNavigation { get; set; } = null!;
        [ForeignKey("idAdmPedidoCliente")]
        [InverseProperty("tblArticuloNAdmPedidoCliente")]
        public virtual tblAdmPedidoCliente idAdmPedidoClienteNavigation { get; set; } = null!;
        [ForeignKey("idArticuloLenceria")]
        [InverseProperty("tblArticuloNAdmPedidoCliente")]
        public virtual tblArticuloLenceria? idArticuloLenceriaNavigation { get; set; }
        [ForeignKey("idArticuloLogistico")]
        [InverseProperty("tblArticuloNAdmPedidoCliente")]
        public virtual tblArticuloLogistico? idArticuloLogisticoNavigation { get; set; }
        [ForeignKey("idArticuloMaquinaria")]
        [InverseProperty("tblArticuloNAdmPedidoCliente")]
        public virtual tblArticuloMaquinaria? idArticuloMaquinariaNavigation { get; set; }
        [ForeignKey("idGrupoArticulos")]
        [InverseProperty("tblArticuloNAdmPedidoCliente")]
        public virtual tblGrupoArticulos? idGrupoArticulosNavigation { get; set; }
        [ForeignKey("idRecambio")]
        [InverseProperty("tblArticuloNAdmPedidoCliente")]
        public virtual tblRecambio? idRecambioNavigation { get; set; }
    }
}
