using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAccionesCorrectivas", Schema = "Incidencias")]
    public partial class tblAccionesCorrectivas
    {
        [Key]
        public int idAccionCorrectiva { get; set; }
        [Key]
        public int idIncidencia { get; set; }
        public bool estado { get; set; }
        public string denominacion { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }

        [ForeignKey("idIncidencia")]
        [InverseProperty("tblAccionesCorrectivas")]
        public virtual tblIncidencia idIncidenciaNavigation { get; set; } = null!;
    }
}
