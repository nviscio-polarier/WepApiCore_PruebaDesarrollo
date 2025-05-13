using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblVideo", Schema = "MyRealLearning")]
    public partial class tblVideo
    {
        public tblVideo()
        {
            tblVideoNPersona = new HashSet<tblVideoNPersona>();
            idEtiqueta = new HashSet<tblEtiquetas>();
        }

        [Key]
        public int idVideo { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string url { get; set; } = null!;
        [StringLength(100)]
        [Unicode(false)]
        public string titulo { get; set; } = null!;
        [Column(TypeName = "text")]
        public string? descripcion { get; set; }
        [Column(TypeName = "date")]
        public DateTime? fechaSubida { get; set; }
        public int idCategoria { get; set; }
        public int duracionSegundos { get; set; }
        public byte[]? imagen { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string? youtubeId { get; set; }

        [ForeignKey("idCategoria")]
        [InverseProperty("tblVideo")]
        public virtual tblCategoriasVideos idCategoriaNavigation { get; set; } = null!;
        [InverseProperty("idVideoNavigation")]
        public virtual ICollection<tblVideoNPersona> tblVideoNPersona { get; set; }

        [ForeignKey("idVideo")]
        [InverseProperty("idVideo")]
        public virtual ICollection<tblEtiquetas> idEtiqueta { get; set; }
    }
}
