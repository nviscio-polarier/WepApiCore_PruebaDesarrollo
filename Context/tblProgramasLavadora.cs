using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblProgramasLavadora", Schema = "Produccion")]
    public partial class tblProgramasLavadora
    {
        public tblProgramasLavadora()
        {
            tblLecturaLavadoras = new HashSet<tblLecturaLavadoras>();
        }

        [Key]
        public int idPrograma { get; set; }
        public string denominacion { get; set; } = null!;
        [Column(TypeName = "time(0)")]
        public TimeSpan tiempoEstimado { get; set; }

        [InverseProperty("idProgramaNavigation")]
        public virtual ICollection<tblLecturaLavadoras> tblLecturaLavadoras { get; set; }
    }
}
