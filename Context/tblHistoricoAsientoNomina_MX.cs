using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblHistoricoAsientoNomina_MX", Schema = "RRHH")]
    public partial class tblHistoricoAsientoNomina_MX
    {
        [Key]
        public int idHistoricoAsientoNomina_MX { get; set; }
        public int idNomina_MX { get; set; }
        public short idTipoNomina_MX { get; set; }
        public int? idAdmCentroCoste { get; set; }
        public int? idAdmElementoPEP { get; set; }
        public int idUsuario { get; set; }
        [Precision(0)]
        public DateTimeOffset fecha { get; set; }
        [Column(TypeName = "date")]
        public DateTime? fechaContabilizado { get; set; }
        [Column(TypeName = "date")]
        public DateTime fechaDesde { get; set; }
        [Column(TypeName = "date")]
        public DateTime fechaHasta { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal sueldo { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal horasExtras { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal primaVacacional { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal primaDominical { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal bono { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal descansoTrabajado { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal aguinaldo { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal valesDespensa { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal fondoAhorro { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal otraPercepcion { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal gastosSindicales { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal PTU { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal infonavitEmpleado { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal fonacotEmpleado { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal IMSSEmpleado { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal SAREmpleado { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal ISREmpleado { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal subsidioEmpleo { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal devolucionPrestamo { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal otrasDeducciones { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal descAlimentos { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal totalDeducciones { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal totalSP { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal totalSV { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal percepcionNeta { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal IMSSPatronal { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal infonavitPatronal { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal SARPatronal { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal impuestoEstatalSobreNominas { get; set; }

        [ForeignKey("idAdmCentroCoste")]
        [InverseProperty("tblHistoricoAsientoNomina_MX")]
        public virtual tblAdmCentroCoste? idAdmCentroCosteNavigation { get; set; }
        [ForeignKey("idAdmElementoPEP")]
        [InverseProperty("tblHistoricoAsientoNomina_MX")]
        public virtual tblAdmElementoPEP? idAdmElementoPEPNavigation { get; set; }
        [ForeignKey("idNomina_MX")]
        [InverseProperty("tblHistoricoAsientoNomina_MX")]
        public virtual tblNomina_MX idNomina_MXNavigation { get; set; } = null!;
        [ForeignKey("idTipoNomina_MX")]
        [InverseProperty("tblHistoricoAsientoNomina_MX")]
        public virtual tblTipoNomina_MX idTipoNomina_MXNavigation { get; set; } = null!;
        [ForeignKey("idUsuario")]
        [InverseProperty("tblHistoricoAsientoNomina_MX")]
        public virtual tblUsuario idUsuarioNavigation { get; set; } = null!;
    }
}
