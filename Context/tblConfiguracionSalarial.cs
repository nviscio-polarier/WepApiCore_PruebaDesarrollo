using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblConfiguracionSalarial", Schema = "RRHH")]
    public partial class tblConfiguracionSalarial
    {
        [Key]
        public short idConfiguracionSalarial { get; set; }
        public int? idLavanderia { get; set; }
        public int? idCentroTrabajo { get; set; }
        public short maxHorasExtraAnual { get; set; }
        public byte maxQuinquenios { get; set; }
        [Column(TypeName = "decimal(4, 3)")]
        public decimal percQuinquenio { get; set; }
        [Column(TypeName = "decimal(4, 3)")]
        public decimal percFestivoTrabajado { get; set; }

        [ForeignKey("idCentroTrabajo")]
        [InverseProperty("tblConfiguracionSalarial")]
        public virtual tblCentroTrabajo? idCentroTrabajoNavigation { get; set; }
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblConfiguracionSalarial")]
        public virtual tblLavanderia? idLavanderiaNavigation { get; set; }
    }
}
