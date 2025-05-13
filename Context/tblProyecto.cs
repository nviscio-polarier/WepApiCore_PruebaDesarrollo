using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblProyecto", Schema = "Logistica")]
    public partial class tblProyecto
    {
        public tblProyecto()
        {
            tblEnvio = new HashSet<tblEnvio>();
        }

        [Key]
        public int idProyecto { get; set; }
        public string denominacion { get; set; } = null!;

        [InverseProperty("idProyectoNavigation")]
        public virtual ICollection<tblEnvio> tblEnvio { get; set; }
    }
}
