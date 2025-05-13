using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAdmPresupuestoVenta_Estado", Schema = "Administracion")]
    public partial class tblAdmPresupuestoVenta_Estado
    {
        public tblAdmPresupuestoVenta_Estado()
        {
            tblAdmPresupuestoVenta = new HashSet<tblAdmPresupuestoVenta>();
        }

        [Key]
        public byte idAdmPresupuestoVenta_Estado { get; set; }
        public string? denominacion { get; set; }

        [InverseProperty("idAdmPresupuestoVenta_EstadoNavigation")]
        public virtual ICollection<tblAdmPresupuestoVenta> tblAdmPresupuestoVenta { get; set; }
    }
}
