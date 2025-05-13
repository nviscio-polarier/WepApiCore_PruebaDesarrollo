using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblDiaSemana", Schema = "General")]
    public partial class tblDiaSemana
    {
        public tblDiaSemana()
        {
            tblTareaMaquina = new HashSet<tblTareaMaquina>();
        }

        [Key]
        public byte idDiaSemana { get; set; }
        [StringLength(50)]
        public string denominacion { get; set; } = null!;

        [InverseProperty("idDiaSemanaNavigation")]
        public virtual ICollection<tblTareaMaquina> tblTareaMaquina { get; set; }
    }
}
