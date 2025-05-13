using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblDiasFestivos", Schema = "RRHH")]
    public partial class tblDiasFestivos
    {
        [Key]
        public int idLavanderia { get; set; }
        [Key]
        [Column(TypeName = "date")]
        public DateTime dia { get; set; }

        [ForeignKey("idLavanderia")]
        [InverseProperty("tblDiasFestivos")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
    }
}
