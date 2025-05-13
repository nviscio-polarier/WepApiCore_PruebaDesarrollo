using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPresupuestoKg", Schema = "ControlPresupuestario")]
    [Index("idAdmElementoPEP", "año", Name = "UK_tblPresupuestoKg", IsUnique = true)]
    public partial class tblPresupuestoKg
    {
        public tblPresupuestoKg()
        {
            tblKgNPresupuestoKg = new HashSet<tblKgNPresupuestoKg>();
            tblKgRealesNPresupuestoKg = new HashSet<tblKgRealesNPresupuestoKg>();
        }

        [Key]
        public int idPresupuestoKg { get; set; }
        public int idAdmElementoPEP { get; set; }
        public short año { get; set; }

        [ForeignKey("idAdmElementoPEP")]
        [InverseProperty("tblPresupuestoKg")]
        public virtual tblAdmElementoPEP idAdmElementoPEPNavigation { get; set; } = null!;
        [InverseProperty("idPresupuestoKgNavigation")]
        public virtual ICollection<tblKgNPresupuestoKg> tblKgNPresupuestoKg { get; set; }
        [InverseProperty("idPresupuestoKgNavigation")]
        public virtual ICollection<tblKgRealesNPresupuestoKg> tblKgRealesNPresupuestoKg { get; set; }
    }
}
