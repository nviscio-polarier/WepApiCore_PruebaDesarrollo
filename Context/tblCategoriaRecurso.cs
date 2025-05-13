using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCategoriaRecurso", Schema = "Energeticos")]
    public partial class tblCategoriaRecurso
    {
        public tblCategoriaRecurso()
        {
            tblRecursoContador = new HashSet<tblRecursoContador>();
        }

        [Key]
        public byte idCategoriaRecurso { get; set; }
        public string denominacion { get; set; } = null!;
        public int idTraduccion { get; set; }

        [ForeignKey("idTraduccion")]
        [InverseProperty("tblCategoriaRecurso")]
        public virtual tblTraduccion idTraduccionNavigation { get; set; } = null!;
        [InverseProperty("idCategoriaRecursoNavigation")]
        public virtual ICollection<tblRecursoContador> tblRecursoContador { get; set; }
    }
}
