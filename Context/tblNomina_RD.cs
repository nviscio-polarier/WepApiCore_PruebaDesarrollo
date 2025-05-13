using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblNomina_RD", Schema = "RRHH")]
    public partial class tblNomina_RD
    {
        public tblNomina_RD()
        {
            tblHistoricoAsientoNomina_RD = new HashSet<tblHistoricoAsientoNomina_RD>();
        }

        [Key]
        public int idNomina_RD { get; set; }
        public int? idPersona { get; set; }
        public string? nombreCompleto { get; set; }
        [Column(TypeName = "date")]
        public DateTime? inicioPeriodo { get; set; }
        [Column(TypeName = "date")]
        public DateTime? finPeriodo { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? sueldo { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? diasPropina { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? diasFeriados { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? horasNocturnas { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? horasExtras35 { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? primaVacacional { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? gratificacion { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? otrosIngresos { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? salarioRetroactivo { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? incentivos { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? subsidPorEnferm { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? ayudaPorNacimiento { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? subsidioPorMaternidad { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? ayudaPorMuerte { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? reembolsoOtrosDescuentos { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? saldoAFavorISR { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? reembolsoISR { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? totalIngresos { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? impSobreLaRenta { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? seguroMedicoPrivado { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? descuentoDeLicenciaMedica { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? sindicato { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? anticipoNomina { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? descAFP { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? descSFS { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? dependAdicionalesSFS { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? otrosDescuentos { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? ahorroCoop { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? prestamoCoop { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? ordenDeCompra { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? infotep { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? descuentosOtros { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? totalDescuentos { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? neto { get; set; }
        public bool? contabilizado { get; set; }
        public int? idAdmCentroCoste { get; set; }
        public int? idAdmElementoPEP { get; set; }
        public int? idAdmCuentaContable { get; set; }
        public short idTipoNomina_RD { get; set; }

        [ForeignKey("idAdmCentroCoste")]
        [InverseProperty("tblNomina_RD")]
        public virtual tblAdmCentroCoste? idAdmCentroCosteNavigation { get; set; }
        [ForeignKey("idAdmCuentaContable")]
        [InverseProperty("tblNomina_RD")]
        public virtual tblAdmCuentaContable? idAdmCuentaContableNavigation { get; set; }
        [ForeignKey("idAdmElementoPEP")]
        [InverseProperty("tblNomina_RD")]
        public virtual tblAdmElementoPEP? idAdmElementoPEPNavigation { get; set; }
        [ForeignKey("idPersona")]
        [InverseProperty("tblNomina_RD")]
        public virtual tblPersona? idPersonaNavigation { get; set; }
        [ForeignKey("idTipoNomina_RD")]
        [InverseProperty("tblNomina_RD")]
        public virtual tblTipoNomina_RD idTipoNomina_RDNavigation { get; set; } = null!;
        [InverseProperty("idNomina_RDNavigation")]
        public virtual ICollection<tblHistoricoAsientoNomina_RD> tblHistoricoAsientoNomina_RD { get; set; }
    }
}
