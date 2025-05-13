using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoRecursoNivel", Schema = "Energeticos")]
    public partial class tblTipoRecursoNivel
    {
        public tblTipoRecursoNivel()
        {
            tblRecursoNivel = new HashSet<tblRecursoNivel>();
        }

        [Key]
        public short idTipoRecursoNivel { get; set; }
        [StringLength(50)]
        public string denominacion { get; set; } = null!;

        [InverseProperty("idTipoRecursoNivelNavigation")]
        public virtual ICollection<tblRecursoNivel> tblRecursoNivel { get; set; }
    }
}
