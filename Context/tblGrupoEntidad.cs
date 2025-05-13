using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblGrupoEntidad", Schema = "General")]
    public partial class tblGrupoEntidad
    {
        public tblGrupoEntidad()
        {
            tblEntidad = new HashSet<tblEntidad>();
        }

        [Key]
        public byte idGrupoEntidad { get; set; }
        public string denominacion { get; set; } = null!;

        [InverseProperty("idGrupoEntidadNavigation")]
        public virtual ICollection<tblEntidad> tblEntidad { get; set; }
    }
}
