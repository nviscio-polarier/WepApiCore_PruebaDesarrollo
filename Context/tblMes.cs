using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblMes", Schema = "General")]
    public partial class tblMes
    {
        public tblMes()
        {
            tblKgNPresupuestoKg = new HashSet<tblKgNPresupuestoKg>();
            tblKgRealesNPresupuestoKg = new HashSet<tblKgRealesNPresupuestoKg>();
            tblTareaMaquina = new HashSet<tblTareaMaquina>();
        }

        [Key]
        public byte idMes { get; set; }
        [StringLength(50)]
        public string denominacion { get; set; } = null!;

        [InverseProperty("idMesNavigation")]
        public virtual ICollection<tblKgNPresupuestoKg> tblKgNPresupuestoKg { get; set; }
        [InverseProperty("idMesNavigation")]
        public virtual ICollection<tblKgRealesNPresupuestoKg> tblKgRealesNPresupuestoKg { get; set; }
        [InverseProperty("idMesNavigation")]
        public virtual ICollection<tblTareaMaquina> tblTareaMaquina { get; set; }
    }
}
