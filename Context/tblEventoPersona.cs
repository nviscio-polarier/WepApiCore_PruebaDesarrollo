using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblEventoPersona", Schema = "MyRealData")]
    public partial class tblEventoPersona
    {
        [Key]
        public int idPersona { get; set; }
        [Key]
        public DateTimeOffset fecha { get; set; }
        public byte idEventoPersona_Estado { get; set; }
        public int idLavanderia { get; set; }
        public bool isRegManual { get; set; }
        public bool isOffline { get; set; }
        public int? idJornada { get; set; }
        public bool isRevisado { get; set; }

        [ForeignKey("idJornada")]
        [InverseProperty("tblEventoPersona")]
        public virtual tblJornada? idJornadaNavigation { get; set; }
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblEventoPersona")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
        [ForeignKey("idPersona")]
        [InverseProperty("tblEventoPersona")]
        public virtual tblPersona idPersonaNavigation { get; set; } = null!;
    }
}
