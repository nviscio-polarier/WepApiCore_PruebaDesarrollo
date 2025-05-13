using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoContenedor", Schema = "Logistica")]
    public partial class tblTipoContenedor
    {
        public tblTipoContenedor()
        {
            tblEnvio = new HashSet<tblEnvio>();
        }

        [Key]
        public int idTipoContenedor { get; set; }
        public string denominacion { get; set; } = null!;
        [StringLength(9)]
        public string? colorHexa { get; set; }

        [InverseProperty("idTipoContenedorNavigation")]
        public virtual ICollection<tblEnvio> tblEnvio { get; set; }
    }
}
