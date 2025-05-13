using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblLecturaLavadoras", Schema = "Produccion")]
    public partial class tblLecturaLavadoras
    {
        [Key]
        public int idMaquina { get; set; }
        [Key]
        [Column(TypeName = "datetime")]
        public DateTime fecha { get; set; }
        public int idLecturaDia { get; set; }
        public int idPrograma { get; set; }
        public int? idEntidad { get; set; }
        [Column(TypeName = "decimal(9, 3)")]
        public decimal peso { get; set; }
        [Column(TypeName = "decimal(9, 3)")]
        public decimal caudal { get; set; }
        [Column(TypeName = "decimal(9, 3)")]
        public decimal consumo { get; set; }
        public bool lecturaValida { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? fechaFin { get; set; }

        [ForeignKey("idEntidad")]
        [InverseProperty("tblLecturaLavadoras")]
        public virtual tblEntidad? idEntidadNavigation { get; set; }
        [ForeignKey("idMaquina")]
        [InverseProperty("tblLecturaLavadoras")]
        public virtual tblMaquina idMaquinaNavigation { get; set; } = null!;
        [ForeignKey("idPrograma")]
        [InverseProperty("tblLecturaLavadoras")]
        public virtual tblProgramasLavadora idProgramaNavigation { get; set; } = null!;
    }
}
