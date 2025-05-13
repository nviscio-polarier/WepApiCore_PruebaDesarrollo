using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblConfigAlbaranReparto", Schema = "Logistica")]
    public partial class tblConfigAlbaranReparto
    {
        public tblConfigAlbaranReparto()
        {
            tblParamNConfigAlbaranReparto = new HashSet<tblParamNConfigAlbaranReparto>();
        }

        [Key]
        public int idConfigAlbaranReparto { get; set; }
        public int idEntidad { get; set; }

        [ForeignKey("idEntidad")]
        [InverseProperty("tblConfigAlbaranReparto")]
        public virtual tblEntidad idEntidadNavigation { get; set; } = null!;
        [InverseProperty("idConfigAlbaranRepartoNavigation")]
        public virtual ICollection<tblParamNConfigAlbaranReparto> tblParamNConfigAlbaranReparto { get; set; }
    }
}
