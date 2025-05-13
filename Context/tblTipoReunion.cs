using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoReunion", Schema = "AppComercial")]
    public partial class tblTipoReunion
    {
        public tblTipoReunion()
        {
            tblReunion = new HashSet<tblReunion>();
        }

        [Key]
        public int idTipoReunion { get; set; }
        public string? denominacion { get; set; }

        [InverseProperty("idTipoReunionNavigation")]
        public virtual ICollection<tblReunion> tblReunion { get; set; }
    }
}
