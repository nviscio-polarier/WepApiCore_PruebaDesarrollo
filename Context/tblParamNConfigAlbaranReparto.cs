using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblParamNConfigAlbaranReparto", Schema = "Logistica")]
    public partial class tblParamNConfigAlbaranReparto
    {
        [Key]
        public int idParamNConfigAlbaranReparto { get; set; }
        public int idConfigAlbaranReparto { get; set; }
        public byte idParametro { get; set; }
        public byte posicion { get; set; }

        [ForeignKey("idConfigAlbaranReparto")]
        [InverseProperty("tblParamNConfigAlbaranReparto")]
        public virtual tblConfigAlbaranReparto idConfigAlbaranRepartoNavigation { get; set; } = null!;
        [ForeignKey("idParametro")]
        [InverseProperty("tblParamNConfigAlbaranReparto")]
        public virtual tblParametrosAlbaranReparto idParametroNavigation { get; set; } = null!;
    }
}
