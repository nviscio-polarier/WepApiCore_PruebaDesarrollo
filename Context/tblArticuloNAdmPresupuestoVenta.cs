using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblArticuloNAdmPresupuestoVenta", Schema = "Administracion")]
    public partial class tblArticuloNAdmPresupuestoVenta
    {
        [Key]
        public int idArticuloNAdmPresupuestoVenta { get; set; }
        public int idAdmPresupuestoVenta { get; set; }
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
        [Column(TypeName = "decimal(5, 4)")]
        public decimal iva { get; set; }
        public int? idArticuloLogistico { get; set; }

        [ForeignKey("idAdmCuentaContable")]
        [InverseProperty("tblArticuloNAdmPresupuestoVenta")]
        public virtual tblAdmCuentaContable idAdmCuentaContableNavigation { get; set; } = null!;
        [ForeignKey("idAdmPresupuestoVenta")]
        [InverseProperty("tblArticuloNAdmPresupuestoVenta")]
        public virtual tblAdmPresupuestoVenta idAdmPresupuestoVentaNavigation { get; set; } = null!;
        [ForeignKey("idArticuloLenceria")]
        [InverseProperty("tblArticuloNAdmPresupuestoVenta")]
        public virtual tblArticuloLenceria? idArticuloLenceriaNavigation { get; set; }
        [ForeignKey("idArticuloLogistico")]
        [InverseProperty("tblArticuloNAdmPresupuestoVenta")]
        public virtual tblArticuloLogistico? idArticuloLogisticoNavigation { get; set; }
        [ForeignKey("idArticuloMaquinaria")]
        [InverseProperty("tblArticuloNAdmPresupuestoVenta")]
        public virtual tblArticuloMaquinaria? idArticuloMaquinariaNavigation { get; set; }
        [ForeignKey("idGrupoArticulos")]
        [InverseProperty("tblArticuloNAdmPresupuestoVenta")]
        public virtual tblGrupoArticulos? idGrupoArticulosNavigation { get; set; }
        [ForeignKey("idRecambio")]
        [InverseProperty("tblArticuloNAdmPresupuestoVenta")]
        public virtual tblRecambio? idRecambioNavigation { get; set; }
    }
}
