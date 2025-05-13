using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoKpi_Observaciones", Schema = "General")]
    public partial class tblTipoKpi_Observaciones
    {
        [Key]
        public byte idTipoKpi { get; set; }
        [Key]
        public int idLavanderia { get; set; }
        public string observaciones { get; set; } = null!;

        [ForeignKey("idLavanderia")]
        [InverseProperty("tblTipoKpi_Observaciones")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
        [ForeignKey("idTipoKpi")]
        [InverseProperty("tblTipoKpi_Observaciones")]
        public virtual tblTipoKpi idTipoKpiNavigation { get; set; } = null!;
    }
}
