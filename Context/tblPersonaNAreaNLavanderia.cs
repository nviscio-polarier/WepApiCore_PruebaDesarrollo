using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPersonaNAreaNLavanderia", Schema = "MyRealData")]
    public partial class tblPersonaNAreaNLavanderia
    {
        [Key]
        public int idPersonaNAreaNLavanderia { get; set; }
        public byte idAreaLavanderia { get; set; }
        public int idLavanderia { get; set; }
        public int idPersona { get; set; }
        public DateTimeOffset fechaIni { get; set; }
        public DateTimeOffset? fechaFin { get; set; }
        public bool isOffline { get; set; }

        [ForeignKey("idAreaLavanderia")]
        [InverseProperty("tblPersonaNAreaNLavanderia")]
        public virtual tblAreaLavanderia idAreaLavanderiaNavigation { get; set; } = null!;
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblPersonaNAreaNLavanderia")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
        [ForeignKey("idPersona")]
        [InverseProperty("tblPersonaNAreaNLavanderia")]
        public virtual tblPersona idPersonaNavigation { get; set; } = null!;
    }
}
