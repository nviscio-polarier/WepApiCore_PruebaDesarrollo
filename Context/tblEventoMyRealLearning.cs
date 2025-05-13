using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblEventoMyRealLearning", Schema = "MyRealLearning")]
    public partial class tblEventoMyRealLearning
    {
        [Key]
        public int idEvento { get; set; }
        public int idPersonaVideo { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime fecha { get; set; }
        public int? idEstadoAnterior { get; set; }
        public int idEstadoNuevo { get; set; }
        public int actor { get; set; }
        [StringLength(255)]
        public string? comentario { get; set; }
        public int idTipoEvento { get; set; }

        [ForeignKey("idTipoEvento")]
        [InverseProperty("tblEventoMyRealLearning")]
        public virtual tblTiposEventoMyRealLearning idTipoEventoNavigation { get; set; } = null!;
    }
}
