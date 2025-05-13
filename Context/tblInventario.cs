using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblInventario", Schema = "Inventarios")]
    public partial class tblInventario
    {
        public tblInventario()
        {
            tblAlmacenNInventario = new HashSet<tblAlmacenNInventario>();
            tblEntidadNInventario = new HashSet<tblEntidadNInventario>();
            tblInventario_Documento = new HashSet<tblInventario_Documento>();
            tblPrendaNInventario = new HashSet<tblPrendaNInventario>();
            idGrupoInventario_generico = new HashSet<tblGrupoInventario_generico>();
        }

        [Key]
        public int idInventario { get; set; }
        public string denominacion { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }
        public bool? estado { get; set; }
        public int? idInventarioPadre { get; set; }
        public int? idCompañia { get; set; }
        public int? idEntidad { get; set; }
        [Column(TypeName = "date")]
        public DateTime? fechaCierre { get; set; }
        [Precision(3)]
        public DateTimeOffset fechaRegistro { get; set; }

        [ForeignKey("idCompañia")]
        [InverseProperty("tblInventario")]
        public virtual tblCompañia? idCompañiaNavigation { get; set; }
        [ForeignKey("idEntidad")]
        [InverseProperty("tblInventario")]
        public virtual tblEntidad? idEntidadNavigation { get; set; }
        [InverseProperty("idInventarioNavigation")]
        public virtual ICollection<tblAlmacenNInventario> tblAlmacenNInventario { get; set; }
        [InverseProperty("idInventarioNavigation")]
        public virtual ICollection<tblEntidadNInventario> tblEntidadNInventario { get; set; }
        [InverseProperty("idInventarioNavigation")]
        public virtual ICollection<tblInventario_Documento> tblInventario_Documento { get; set; }
        [InverseProperty("idInventarioNavigation")]
        public virtual ICollection<tblPrendaNInventario> tblPrendaNInventario { get; set; }

        [ForeignKey("idInventario")]
        [InverseProperty("idInventario")]
        public virtual ICollection<tblGrupoInventario_generico> idGrupoInventario_generico { get; set; }
    }
}
