using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTareaMantenimientoPrev", Schema = "Maquinaria")]
    public partial class tblTareaMantenimientoPrev
    {
        public tblTareaMantenimientoPrev()
        {
            tblMantenimientoPrev = new HashSet<tblMantenimientoPrev>();
        }

        [Key]
        public short idTareaMantenimientoPrev { get; set; }
        public byte idPlantillaTareaMantenimientoPrev { get; set; }
        public string denominacion { get; set; } = null!;
        public short cadencia { get; set; }
        [Column(TypeName = "time(0)")]
        public TimeSpan tiempo { get; set; }
        public byte numTecnicos { get; set; }

        [ForeignKey("idPlantillaTareaMantenimientoPrev")]
        [InverseProperty("tblTareaMantenimientoPrev")]
        public virtual tblPlantillaTareaMantenimientoPrev idPlantillaTareaMantenimientoPrevNavigation { get; set; } = null!;
        [InverseProperty("idTareaMantenimientoPrevNavigation")]
        public virtual ICollection<tblMantenimientoPrev> tblMantenimientoPrev { get; set; }
    }
}
