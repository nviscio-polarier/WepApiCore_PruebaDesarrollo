using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblHistoricoAsientoNomina", Schema = "RRHH")]
    public partial class tblHistoricoAsientoNomina
    {
        [Key]
        public int idHistoricoAsientoNomina { get; set; }
        public int idNomina { get; set; }
        public int? idAdmElementoPEP { get; set; }
        public int? idAdmCentroCoste { get; set; }
        public int idUsuario { get; set; }
        [Precision(0)]
        public DateTimeOffset fecha { get; set; }
        [Column(TypeName = "date")]
        public DateTime fechaDesde { get; set; }
        [Column(TypeName = "date")]
        public DateTime fechaHasta { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal salarioBruto { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal tributacionIRPF { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal segSocialTrabajador { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal liquidoPercibir { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal embargo { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal conceptoNEspecie { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal segSocialEmpresa { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal descuentosSalariales { get; set; }
        public byte? idEstadoHistoricoAsientoNomina { get; set; }
        [Column(TypeName = "date")]
        public DateTime? fechaContabilizado { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal? descuentoPreaviso { get; set; }
        [Column(TypeName = "decimal(11, 2)")]
        public decimal? plusAsistenciaMesAnterior { get; set; }

        [ForeignKey("idAdmCentroCoste")]
        [InverseProperty("tblHistoricoAsientoNomina")]
        public virtual tblAdmCentroCoste? idAdmCentroCosteNavigation { get; set; }
        [ForeignKey("idAdmElementoPEP")]
        [InverseProperty("tblHistoricoAsientoNomina")]
        public virtual tblAdmElementoPEP? idAdmElementoPEPNavigation { get; set; }
        [ForeignKey("idEstadoHistoricoAsientoNomina")]
        [InverseProperty("tblHistoricoAsientoNomina")]
        public virtual tblEstadoHistoricoAsientoNomina? idEstadoHistoricoAsientoNominaNavigation { get; set; }
        [ForeignKey("idNomina")]
        [InverseProperty("tblHistoricoAsientoNomina")]
        public virtual tblNomina idNominaNavigation { get; set; } = null!;
        [ForeignKey("idUsuario")]
        [InverseProperty("tblHistoricoAsientoNomina")]
        public virtual tblUsuario idUsuarioNavigation { get; set; } = null!;
    }
}
