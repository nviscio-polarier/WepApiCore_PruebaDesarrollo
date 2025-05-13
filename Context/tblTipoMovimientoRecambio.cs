using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoMovimientoRecambio", Schema = "Assistant")]
    public partial class tblTipoMovimientoRecambio
    {
        public tblTipoMovimientoRecambio()
        {
            tblMovimientoRecambio = new HashSet<tblMovimientoRecambio>();
        }

        [Key]
        public int idTipoMovimientoRecambio { get; set; }
        public string denominacion { get; set; } = null!;

        [InverseProperty("idTipoMovimientoRecambioNavigation")]
        public virtual ICollection<tblMovimientoRecambio> tblMovimientoRecambio { get; set; }
    }
}
