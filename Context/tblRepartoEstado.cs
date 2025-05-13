using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblRepartoEstado", Schema = "Logistica")]
    public partial class tblRepartoEstado
    {
        public tblRepartoEstado()
        {
            tblReparto = new HashSet<tblReparto>();
        }

        [StringLength(20)]
        public string denominacion { get; set; } = null!;
        [Key]
        public byte idRepartoEstado { get; set; }

        [InverseProperty("idRepartoEstadoNavigation")]
        public virtual ICollection<tblReparto> tblReparto { get; set; }
    }
}
