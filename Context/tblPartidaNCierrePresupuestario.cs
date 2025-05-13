using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPartidaNCierrePresupuestario", Schema = "ControlPresupuestario")]
    public partial class tblPartidaNCierrePresupuestario
    {
        [Key]
        public int idPartidaNCierrePresupuestario { get; set; }
        public int idCierrePresupuestario { get; set; }
        public int idAdmCuentaContable { get; set; }
        [StringLength(10)]
        public string? asientoDocumento { get; set; }
        public string? comentarioDocumento { get; set; }
        [Column(TypeName = "date")]
        public DateTime? fechaDocumento { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal valor { get; set; }
        public byte idMoneda { get; set; }

        [ForeignKey("idAdmCuentaContable")]
        [InverseProperty("tblPartidaNCierrePresupuestario")]
        public virtual tblAdmCuentaContable idAdmCuentaContableNavigation { get; set; } = null!;
        [ForeignKey("idCierrePresupuestario")]
        [InverseProperty("tblPartidaNCierrePresupuestario")]
        public virtual tblCierrePresupuestario idCierrePresupuestarioNavigation { get; set; } = null!;
        [ForeignKey("idMoneda")]
        [InverseProperty("tblPartidaNCierrePresupuestario")]
        public virtual tblMoneda idMonedaNavigation { get; set; } = null!;
    }
}
