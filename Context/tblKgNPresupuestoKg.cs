using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblKgNPresupuestoKg", Schema = "ControlPresupuestario")]
    public partial class tblKgNPresupuestoKg
    {
        [Key]
        public int idPresupuestoKg { get; set; }
        [Key]
        public byte idMes { get; set; }
        public int valor { get; set; }

        [ForeignKey("idMes")]
        [InverseProperty("tblKgNPresupuestoKg")]
        public virtual tblMes idMesNavigation { get; set; } = null!;
        [ForeignKey("idPresupuestoKg")]
        [InverseProperty("tblKgNPresupuestoKg")]
        public virtual tblPresupuestoKg idPresupuestoKgNavigation { get; set; } = null!;
    }
}
