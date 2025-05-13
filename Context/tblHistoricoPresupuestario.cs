using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblHistoricoPresupuestario", Schema = "ControlPresupuestario")]
    public partial class tblHistoricoPresupuestario
    {
        [Key]
        public int idHistoricoPresupuestario { get; set; }
        public int idAdmCuentaContable { get; set; }
        public int? idAdmCentroCoste { get; set; }
        public int? idAdmElementoPEP { get; set; }
        public byte idMoneda { get; set; }
        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }
        [StringLength(10)]
        public string? asientoDocumento { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? fechaDocumento { get; set; }
        public string? comentarioDocumento { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal valorPrespupuestado { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal valorReal { get; set; }

        [ForeignKey("idAdmCentroCoste")]
        [InverseProperty("tblHistoricoPresupuestario")]
        public virtual tblAdmCentroCoste? idAdmCentroCosteNavigation { get; set; }
        [ForeignKey("idAdmCuentaContable")]
        [InverseProperty("tblHistoricoPresupuestario")]
        public virtual tblAdmCuentaContable idAdmCuentaContableNavigation { get; set; } = null!;
        [ForeignKey("idAdmElementoPEP")]
        [InverseProperty("tblHistoricoPresupuestario")]
        public virtual tblAdmElementoPEP? idAdmElementoPEPNavigation { get; set; }
        [ForeignKey("idMoneda")]
        [InverseProperty("tblHistoricoPresupuestario")]
        public virtual tblMoneda idMonedaNavigation { get; set; } = null!;
    }
}
