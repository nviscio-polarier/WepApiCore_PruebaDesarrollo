using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblEstadosVideo", Schema = "MyRealLearning")]
    public partial class tblEstadosVideo
    {
        public tblEstadosVideo()
        {
            tblVideoNPersona = new HashSet<tblVideoNPersona>();
        }

        [Key]
        public int idEstado { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string denominacion { get; set; } = null!;

        [InverseProperty("idEstadoNavigation")]
        public virtual ICollection<tblVideoNPersona> tblVideoNPersona { get; set; }
    }
}
