using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblHistoricoPartidaContable", Schema = "ControlPresupuestario")]
    public partial class tblHistoricoPartidaContable
    {
        [Key]
        public int idHistoricoPartidaContable { get; set; }
        public int idHistoricoPlanificacion { get; set; }
        public int? idAdmCentroCoste { get; set; }
        public int? idAdmElementoPEP { get; set; }
        public byte idMoneda { get; set; }
        [StringLength(8)]
        public string asientoDocumento { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime fechaDocumento { get; set; }
        public string comentarioDocumento { get; set; } = null!;
        [Column(TypeName = "decimal(18, 2)")]
        public decimal valorReal { get; set; }

        [ForeignKey("idAdmCentroCoste")]
        [InverseProperty("tblHistoricoPartidaContable")]
        public virtual tblAdmCentroCoste? idAdmCentroCosteNavigation { get; set; }
        [ForeignKey("idAdmElementoPEP")]
        [InverseProperty("tblHistoricoPartidaContable")]
        public virtual tblAdmElementoPEP? idAdmElementoPEPNavigation { get; set; }
        [ForeignKey("idHistoricoPlanificacion")]
        [InverseProperty("tblHistoricoPartidaContable")]
        public virtual tblHistoricoPlanificacion idHistoricoPlanificacionNavigation { get; set; } = null!;
        [ForeignKey("idMoneda")]
        [InverseProperty("tblHistoricoPartidaContable")]
        public virtual tblMoneda idMonedaNavigation { get; set; } = null!;
    }
}
