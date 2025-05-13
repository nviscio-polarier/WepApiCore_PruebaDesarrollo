using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoRepartoEntidad", Schema = "General")]
    public partial class tblTipoRepartoEntidad
    {
        public tblTipoRepartoEntidad()
        {
            tblEntidad = new HashSet<tblEntidad>();
        }

        [Key]
        public byte idTipoRepartoEntidad { get; set; }
        public string denominacion { get; set; } = null!;

        [InverseProperty("idTipoRepartoEntidadNavigation")]
        public virtual ICollection<tblEntidad> tblEntidad { get; set; }
    }
}
