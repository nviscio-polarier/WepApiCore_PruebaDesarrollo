using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblFormatoReunion", Schema = "AppComercial")]
    public partial class tblFormatoReunion
    {
        public tblFormatoReunion()
        {
            tblReunion = new HashSet<tblReunion>();
        }

        [Key]
        public int idFormatoReunion { get; set; }
        public string denominacion { get; set; } = null!;

        [InverseProperty("idFormatoReunionNavigation")]
        public virtual ICollection<tblReunion> tblReunion { get; set; }
    }
}
