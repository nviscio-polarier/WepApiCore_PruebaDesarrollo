using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPrendaNRepartoOffice", Schema = "Office")]
    [Index("idRepartoOffice", Name = "IX_tblPrendaNRepartoOffice_idRepartoOffice")]
    public partial class tblPrendaNRepartoOffice
    {
        [Key]
        public int idPrenda { get; set; }
        [Key]
        public int idRepartoOffice { get; set; }
        public int? cantidad { get; set; }
        public int? pendientes { get; set; }

        [ForeignKey("idPrenda")]
        [InverseProperty("tblPrendaNRepartoOffice")]
        public virtual tblPrenda idPrendaNavigation { get; set; } = null!;
        [ForeignKey("idRepartoOffice")]
        [InverseProperty("tblPrendaNRepartoOffice")]
        public virtual tblRepartoOffice idRepartoOfficeNavigation { get; set; } = null!;
    }
}
