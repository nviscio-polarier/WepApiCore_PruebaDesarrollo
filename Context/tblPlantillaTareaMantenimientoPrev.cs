using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPlantillaTareaMantenimientoPrev", Schema = "Maquinaria")]
    public partial class tblPlantillaTareaMantenimientoPrev
    {
        public tblPlantillaTareaMantenimientoPrev()
        {
            tblMaquina = new HashSet<tblMaquina>();
            tblTareaMantenimientoPrev = new HashSet<tblTareaMantenimientoPrev>();
        }

        [Key]
        public byte idPlantillaTareaMantenimientoPrev { get; set; }
        public int idSistemaMaquina { get; set; }
        public string denominacion { get; set; } = null!;

        [ForeignKey("idSistemaMaquina")]
        [InverseProperty("tblPlantillaTareaMantenimientoPrev")]
        public virtual tblSistemaMaquina idSistemaMaquinaNavigation { get; set; } = null!;
        [InverseProperty("idPlantillaTareaMantenimientoPrevNavigation")]
        public virtual ICollection<tblMaquina> tblMaquina { get; set; }
        [InverseProperty("idPlantillaTareaMantenimientoPrevNavigation")]
        public virtual ICollection<tblTareaMantenimientoPrev> tblTareaMantenimientoPrev { get; set; }
    }
}
