using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblArticuloNAdmAlbaranVenta", Schema = "Administracion")]
    public partial class tblArticuloNAdmAlbaranVenta
    {
        [Key]
        public int idArticuloNAdmAlbaranVenta { get; set; }
        public int idAdmAlbaranVenta { get; set; }
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

        [ForeignKey("idAdmAlbaranVenta")]
        [InverseProperty("tblArticuloNAdmAlbaranVenta")]
        public virtual tblAdmAlbaranVenta idAdmAlbaranVentaNavigation { get; set; } = null!;
        [ForeignKey("idAdmCuentaContable")]
        [InverseProperty("tblArticuloNAdmAlbaranVenta")]
        public virtual tblAdmCuentaContable idAdmCuentaContableNavigation { get; set; } = null!;
        [ForeignKey("idArticuloLenceria")]
        [InverseProperty("tblArticuloNAdmAlbaranVenta")]
        public virtual tblArticuloLenceria? idArticuloLenceriaNavigation { get; set; }
        [ForeignKey("idArticuloLogistico")]
        [InverseProperty("tblArticuloNAdmAlbaranVenta")]
        public virtual tblArticuloLogistico? idArticuloLogisticoNavigation { get; set; }
        [ForeignKey("idArticuloMaquinaria")]
        [InverseProperty("tblArticuloNAdmAlbaranVenta")]
        public virtual tblArticuloMaquinaria? idArticuloMaquinariaNavigation { get; set; }
        [ForeignKey("idGrupoArticulos")]
        [InverseProperty("tblArticuloNAdmAlbaranVenta")]
        public virtual tblGrupoArticulos? idGrupoArticulosNavigation { get; set; }
        [ForeignKey("idRecambio")]
        [InverseProperty("tblArticuloNAdmAlbaranVenta")]
        public virtual tblRecambio? idRecambioNavigation { get; set; }
    }
}
