using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblKgRealesNPresupuestoKg", Schema = "ControlPresupuestario")]
    public partial class tblKgRealesNPresupuestoKg
    {
        [Key]
        public int idPresupuestoKg { get; set; }
        [Key]
        public byte idMes { get; set; }
        public int valor { get; set; }

        [ForeignKey("idMes")]
        [InverseProperty("tblKgRealesNPresupuestoKg")]
        public virtual tblMes idMesNavigation { get; set; } = null!;
        [ForeignKey("idPresupuestoKg")]
        [InverseProperty("tblKgRealesNPresupuestoKg")]
        public virtual tblPresupuestoKg idPresupuestoKgNavigation { get; set; } = null!;
    }
}
