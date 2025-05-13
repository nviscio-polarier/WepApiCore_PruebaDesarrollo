using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblIncidenciaNParte", Schema = "Assistant")]
    public partial class tblIncidenciaNParte
    {
        [Key]
        public int idIncidencia { get; set; }
        [Key]
        public int idParte { get; set; }
        public byte estadoInicial { get; set; }
        public byte estadoActual { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime fecha { get; set; }
        public int? peso { get; set; }
        public byte? pesoInicial { get; set; }

        [ForeignKey("idIncidencia")]
        [InverseProperty("tblIncidenciaNParte")]
        public virtual tblIncidencia idIncidenciaNavigation { get; set; } = null!;
        [ForeignKey("idParte")]
        [InverseProperty("tblIncidenciaNParte")]
        public virtual tblParteTrabajo idParteNavigation { get; set; } = null!;
    }
}
