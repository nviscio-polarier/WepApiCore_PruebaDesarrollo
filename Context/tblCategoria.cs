using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCategoria", Schema = "RRHH")]
    public partial class tblCategoria
    {
        public tblCategoria()
        {
            tblPersona = new HashSet<tblPersona>();
            idCategoria_Grupo = new HashSet<tblCategoria_Grupo>();
        }

        [Key]
        public short idCategoria { get; set; }
        public string denominacion { get; set; } = null!;
        [StringLength(2)]
        public string? idxCategoria { get; set; }
        public byte? codigo { get; set; }

        [InverseProperty("idCategoriaNavigation")]
        public virtual ICollection<tblPersona> tblPersona { get; set; }

        [ForeignKey("idCategoria")]
        [InverseProperty("idCategoria")]
        public virtual ICollection<tblCategoria_Grupo> idCategoria_Grupo { get; set; }
    }
}
