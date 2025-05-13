using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPrendaNLavanderia", Schema = "Logistica")]
    public partial class tblPrendaNLavanderia
    {
        [Key]
        public int idLavanderia { get; set; }
        [Key]
        public int idPrenda { get; set; }
        public int stock { get; set; }

        [ForeignKey("idLavanderia")]
        [InverseProperty("tblPrendaNLavanderia")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
        [ForeignKey("idPrenda")]
        [InverseProperty("tblPrendaNLavanderia")]
        public virtual tblPrenda idPrendaNavigation { get; set; } = null!;
    }
}
