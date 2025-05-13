using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoMovimiento", Schema = "Inventarios")]
    public partial class tblTipoMovimiento
    {
        public tblTipoMovimiento()
        {
            tblMovimiento = new HashSet<tblMovimiento>();
        }

        [Key]
        public int idTipoMovimiento { get; set; }
        public string denominacion { get; set; } = null!;
        public bool isEntrada { get; set; }
        public int idTraduccion { get; set; }

        [ForeignKey("idTraduccion")]
        [InverseProperty("tblTipoMovimiento")]
        public virtual tblTraduccion idTraduccionNavigation { get; set; } = null!;
        [InverseProperty("idTipoMovimientoNavigation")]
        public virtual ICollection<tblMovimiento> tblMovimiento { get; set; }
    }
}
