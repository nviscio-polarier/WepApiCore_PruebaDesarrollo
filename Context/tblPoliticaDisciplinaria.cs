using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPoliticaDisciplinaria", Schema = "Incidencias")]
    public partial class tblPoliticaDisciplinaria
    {
        public tblPoliticaDisciplinaria()
        {
            tblIncidencia = new HashSet<tblIncidencia>();
        }

        [Key]
        public byte idPoliticaDisciplinaria { get; set; }
        public int idTraduccion { get; set; }

        [ForeignKey("idTraduccion")]
        [InverseProperty("tblPoliticaDisciplinaria")]
        public virtual tblTraduccion idTraduccionNavigation { get; set; } = null!;
        [InverseProperty("idPoliticaDisciplinariaNavigation")]
        public virtual ICollection<tblIncidencia> tblIncidencia { get; set; }
    }
}
