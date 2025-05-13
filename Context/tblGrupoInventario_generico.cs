using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblGrupoInventario_generico", Schema = "Inventarios")]
    public partial class tblGrupoInventario_generico
    {
        public tblGrupoInventario_generico()
        {
            idInventario = new HashSet<tblInventario>();
        }

        [Key]
        public int idGrupoInventario_generico { get; set; }
        public string denominacion { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }
        public int idUsuario { get; set; }
        [Precision(0)]
        public DateTimeOffset fechaReg { get; set; }

        [ForeignKey("idUsuario")]
        [InverseProperty("tblGrupoInventario_generico")]
        public virtual tblUsuario idUsuarioNavigation { get; set; } = null!;

        [ForeignKey("idGrupoInventario_generico")]
        [InverseProperty("idGrupoInventario_generico")]
        public virtual ICollection<tblInventario> idInventario { get; set; }
    }
}
