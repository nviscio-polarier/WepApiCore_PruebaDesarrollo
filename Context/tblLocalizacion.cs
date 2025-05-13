using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblLocalizacion", Schema = "General")]
    public partial class tblLocalizacion
    {
        public tblLocalizacion()
        {
            tblEntidad = new HashSet<tblEntidad>();
            tblLavanderia = new HashSet<tblLavanderia>();
            tblUsuario = new HashSet<tblUsuario>();
        }

        [Key]
        public short idLocalizacion { get; set; }
        public string denominacion { get; set; } = null!;
        public byte idZonaHoraria { get; set; }
        public int? idPais { get; set; }

        [ForeignKey("idPais")]
        [InverseProperty("tblLocalizacion")]
        public virtual tblPais? idPaisNavigation { get; set; }
        [ForeignKey("idZonaHoraria")]
        [InverseProperty("tblLocalizacion")]
        public virtual tblZonaHoraria idZonaHorariaNavigation { get; set; } = null!;
        [InverseProperty("idLocalizacionNavigation")]
        public virtual ICollection<tblEntidad> tblEntidad { get; set; }
        [InverseProperty("idLocalizacionNavigation")]
        public virtual ICollection<tblLavanderia> tblLavanderia { get; set; }
        [InverseProperty("idLocalizacionNavigation")]
        public virtual ICollection<tblUsuario> tblUsuario { get; set; }
    }
}
