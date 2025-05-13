using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblParteTrabajo", Schema = "Incidencias")]
    public partial class tblParteTrabajo1
    {
        public tblParteTrabajo1()
        {
            tblPersonasNParte1 = new HashSet<tblPersonasNParte1>();
        }

        [Key]
        public int idParteTrabajo { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime fecha { get; set; }
        public string resolucion { get; set; } = null!;
        [StringLength(8)]
        public string? idxParteTrabajo { get; set; }

        [InverseProperty("idParteNavigation")]
        public virtual ICollection<tblPersonasNParte1> tblPersonasNParte1 { get; set; }
    }
}
