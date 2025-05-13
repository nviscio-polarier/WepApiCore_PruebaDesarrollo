using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblHistoricoAsientoNomina_RD", Schema = "RRHH")]
    public partial class tblHistoricoAsientoNomina_RD
    {
        [Key]
        public int idHistoricoAsientoNomina_RD { get; set; }
        public int idNomina_RD { get; set; }
        public int? idAdmElementoPEP { get; set; }
        public int? idAdmCentroCoste { get; set; }
        public int idUsuario { get; set; }
        [Precision(0)]
        public DateTimeOffset fecha { get; set; }
        [Column(TypeName = "date")]
        public DateTime? fechaContabilizado { get; set; }
        [Column(TypeName = "date")]
        public DateTime fechaDesde { get; set; }
        [Column(TypeName = "date")]
        public DateTime fechaHasta { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal sueldo { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal diasPropina { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal diasFeriados { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal horasNocturnas { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal horasExtras35 { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal primaVacacional { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal gratificacion { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal otrosIngresos { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal salarioRetroactivo { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal incentivos { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal subsidPorEnferm { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal subsidioPorMaternidad { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal ayudaPorNacimiento { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal ayudaPorMuerte { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal reembolsoOtrosDescuentos { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal saldoAFavorISR { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal reembolsoISR { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal totalIngresos { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal impSobreLaRenta { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal seguroMedicoPrivado { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal descuentoDeLicenciaMedica { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal sindicato { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal anticipoNomina { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal descAFP { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal descSFS { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal dependAdicionalesSFS { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal otrosDescuentos { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal ahorroCoop { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal prestamoCoop { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal ordenDeCompra { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal infotep { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal descuentosOtros { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal totalDescuentos { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal neto { get; set; }

        [ForeignKey("idAdmCentroCoste")]
        [InverseProperty("tblHistoricoAsientoNomina_RD")]
        public virtual tblAdmCentroCoste? idAdmCentroCosteNavigation { get; set; }
        [ForeignKey("idAdmElementoPEP")]
        [InverseProperty("tblHistoricoAsientoNomina_RD")]
        public virtual tblAdmElementoPEP? idAdmElementoPEPNavigation { get; set; }
        [ForeignKey("idNomina_RD")]
        [InverseProperty("tblHistoricoAsientoNomina_RD")]
        public virtual tblNomina_RD idNomina_RDNavigation { get; set; } = null!;
        [ForeignKey("idUsuario")]
        [InverseProperty("tblHistoricoAsientoNomina_RD")]
        public virtual tblUsuario idUsuarioNavigation { get; set; } = null!;
    }
}
