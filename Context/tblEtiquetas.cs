using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblEtiquetas", Schema = "MyRealLearning")]
    public partial class tblEtiquetas
    {
        public tblEtiquetas()
        {
            idVideo = new HashSet<tblVideo>();
        }

        [Key]
        public int idEtiqueta { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string denominacion { get; set; } = null!;

        [ForeignKey("idEtiqueta")]
        [InverseProperty("idEtiqueta")]
        public virtual ICollection<tblVideo> idVideo { get; set; }
    }
}
