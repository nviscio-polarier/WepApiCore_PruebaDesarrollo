using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoMantenimientoMaquina", Schema = "Assistant")]
    public partial class tblTipoMantenimientoMaquina
    {
        public tblTipoMantenimientoMaquina()
        {
            tblMantenimientoNMaquina = new HashSet<tblMantenimientoNMaquina>();
        }

        [Key]
        public byte idTipoMantenimientoMaquina { get; set; }
        public string denominacion { get; set; } = null!;

        [InverseProperty("idTipoMantenimientoNMaquinaNavigation")]
        public virtual ICollection<tblMantenimientoNMaquina> tblMantenimientoNMaquina { get; set; }
    }
}
