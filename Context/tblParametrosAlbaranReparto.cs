using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblParametrosAlbaranReparto", Schema = "Logistica")]
    public partial class tblParametrosAlbaranReparto
    {
        public tblParametrosAlbaranReparto()
        {
            tblParamNConfigAlbaranReparto = new HashSet<tblParamNConfigAlbaranReparto>();
        }

        [Key]
        public byte idParametro { get; set; }
        [StringLength(50)]
        public string? denominacion { get; set; }

        [InverseProperty("idParametroNavigation")]
        public virtual ICollection<tblParamNConfigAlbaranReparto> tblParamNConfigAlbaranReparto { get; set; }
    }
}
