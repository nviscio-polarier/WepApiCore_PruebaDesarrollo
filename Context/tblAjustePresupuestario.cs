using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAjustePresupuestario", Schema = "ControlPresupuestario")]
    public partial class tblAjustePresupuestario
    {
        public tblAjustePresupuestario()
        {
            tblMesNAjustePresupuestario = new HashSet<tblMesNAjustePresupuestario>();
        }

        [Key]
        public int idAjustePresupuestario { get; set; }
        public int? idAdmCentroCoste { get; set; }
        public int? idAdmElementoPEP { get; set; }
        public int? idAdmCuentaContable { get; set; }
        public byte? idMoneda { get; set; }
        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal valor { get; set; }
        public string? observaciones { get; set; }

        [ForeignKey("idAdmCentroCoste")]
        [InverseProperty("tblAjustePresupuestario")]
        public virtual tblAdmCentroCoste? idAdmCentroCosteNavigation { get; set; }
        [ForeignKey("idAdmCuentaContable")]
        [InverseProperty("tblAjustePresupuestario")]
        public virtual tblAdmCuentaContable? idAdmCuentaContableNavigation { get; set; }
        [ForeignKey("idAdmElementoPEP")]
        [InverseProperty("tblAjustePresupuestario")]
        public virtual tblAdmElementoPEP? idAdmElementoPEPNavigation { get; set; }
        [ForeignKey("idMoneda")]
        [InverseProperty("tblAjustePresupuestario")]
        public virtual tblMoneda? idMonedaNavigation { get; set; }
        [InverseProperty("idAjustePresupuestarioNavigation")]
        public virtual ICollection<tblMesNAjustePresupuestario> tblMesNAjustePresupuestario { get; set; }
    }
}
