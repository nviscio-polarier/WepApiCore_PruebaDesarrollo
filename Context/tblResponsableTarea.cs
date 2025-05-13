using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblResponsableTarea", Schema = "Assistant")]
    public partial class tblResponsableTarea
    {
        public tblResponsableTarea()
        {
            tblTareaMaquina = new HashSet<tblTareaMaquina>();
        }

        [Key]
        public byte idResponsableTarea { get; set; }
        [StringLength(50)]
        public string denominacion { get; set; } = null!;

        [InverseProperty("idResponsableTareaNavigation")]
        public virtual ICollection<tblTareaMaquina> tblTareaMaquina { get; set; }
    }
}
