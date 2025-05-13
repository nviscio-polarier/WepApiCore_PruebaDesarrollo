using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoAlmacenajeLimpio", Schema = "General")]
    public partial class tblTipoAlmacenajeLimpio
    {
        public tblTipoAlmacenajeLimpio()
        {
            tblEntidad = new HashSet<tblEntidad>();
        }

        [Key]
        public byte idTipoAlmacenajeLimpio { get; set; }
        public string denominacion { get; set; } = null!;

        [InverseProperty("idTipoAlmacenajeLimpioNavigation")]
        public virtual ICollection<tblEntidad> tblEntidad { get; set; }
    }
}
