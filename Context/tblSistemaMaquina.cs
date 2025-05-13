using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblSistemaMaquina", Schema = "Maquinaria")]
    public partial class tblSistemaMaquina
    {
        public tblSistemaMaquina()
        {
            tblCategoriaMaquina = new HashSet<tblCategoriaMaquina>();
            tblPesoNSistemaNLavanderia = new HashSet<tblPesoNSistemaNLavanderia>();
            tblPlantillaTareaMantenimientoPrev = new HashSet<tblPlantillaTareaMantenimientoPrev>();
        }

        [Key]
        public int idSistemaMaquina { get; set; }
        [StringLength(50)]
        public string denominacion { get; set; } = null!;
        public int? orden { get; set; }

        [InverseProperty("idSistemaMaquinaNavigation")]
        public virtual ICollection<tblCategoriaMaquina> tblCategoriaMaquina { get; set; }
        [InverseProperty("idSistemaMaquinaNavigation")]
        public virtual ICollection<tblPesoNSistemaNLavanderia> tblPesoNSistemaNLavanderia { get; set; }
        [InverseProperty("idSistemaMaquinaNavigation")]
        public virtual ICollection<tblPlantillaTareaMantenimientoPrev> tblPlantillaTareaMantenimientoPrev { get; set; }
    }
}
