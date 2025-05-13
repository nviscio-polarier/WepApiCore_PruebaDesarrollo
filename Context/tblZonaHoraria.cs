using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblZonaHoraria", Schema = "General")]
    public partial class tblZonaHoraria
    {
        public tblZonaHoraria()
        {
            tblLavanderia = new HashSet<tblLavanderia>();
            tblLocalizacion = new HashSet<tblLocalizacion>();
        }

        [Key]
        public byte idZonaHoraria { get; set; }
        [StringLength(50)]
        public string? denominacion { get; set; }
        [StringLength(3)]
        public string? GMT { get; set; }

        [InverseProperty("idZonaHorariaNavigation")]
        public virtual ICollection<tblLavanderia> tblLavanderia { get; set; }
        [InverseProperty("idZonaHorariaNavigation")]
        public virtual ICollection<tblLocalizacion> tblLocalizacion { get; set; }
    }
}
