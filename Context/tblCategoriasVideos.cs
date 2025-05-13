using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCategoriasVideos", Schema = "MyRealLearning")]
    public partial class tblCategoriasVideos
    {
        public tblCategoriasVideos()
        {
            tblVideo = new HashSet<tblVideo>();
        }

        [Key]
        public int idCategoria { get; set; }
        [StringLength(75)]
        [Unicode(false)]
        public string denominacion { get; set; } = null!;

        [InverseProperty("idCategoriaNavigation")]
        public virtual ICollection<tblVideo> tblVideo { get; set; }
    }
}
