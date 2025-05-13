using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTiposEventoMyRealLearning", Schema = "MyRealLearning")]
    public partial class tblTiposEventoMyRealLearning
    {
        public tblTiposEventoMyRealLearning()
        {
            tblEventoMyRealLearning = new HashSet<tblEventoMyRealLearning>();
        }

        [Key]
        public int idTipoEvento { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string denominacion { get; set; } = null!;

        [InverseProperty("idTipoEventoNavigation")]
        public virtual ICollection<tblEventoMyRealLearning> tblEventoMyRealLearning { get; set; }
    }
}
