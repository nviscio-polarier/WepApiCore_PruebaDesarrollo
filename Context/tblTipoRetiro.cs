using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoRetiro", Schema = "Produccion")]
    public partial class tblTipoRetiro
    {
        public tblTipoRetiro()
        {
            tblMovimiento = new HashSet<tblMovimiento>();
            tblPrendaNGestionRetiro = new HashSet<tblPrendaNGestionRetiro>();
        }

        [Key]
        public int idTipoRetiro { get; set; }
        public string? denominacion { get; set; }
        public int? idTraduccion { get; set; }

        [ForeignKey("idTraduccion")]
        [InverseProperty("tblTipoRetiro")]
        public virtual tblTraduccion? idTraduccionNavigation { get; set; }
        [InverseProperty("idTipoRetiroNavigation")]
        public virtual ICollection<tblMovimiento> tblMovimiento { get; set; }
        [InverseProperty("idTipoRetiroNavigation")]
        public virtual ICollection<tblPrendaNGestionRetiro> tblPrendaNGestionRetiro { get; set; }
    }
}
