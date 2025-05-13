using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCalendarioPersonal", Schema = "RRHH")]
    public partial class tblCalendarioPersonal
    {
        [Key]
        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }
        [Key]
        public int idPersona { get; set; }
        public byte idCalendario_Estado { get; set; }
        public int? idLavanderia { get; set; }
        public int? idUsuario_validacion { get; set; }
        [Precision(0)]
        public DateTimeOffset? fecha_validacion { get; set; }
        public int? idJornada { get; set; }
        public int? idCuadrantePersonal { get; set; }

        [ForeignKey("idCalendario_Estado")]
        [InverseProperty("tblCalendarioPersonal")]
        public virtual tblCalendario_Estado idCalendario_EstadoNavigation { get; set; } = null!;
        [ForeignKey("idCuadrantePersonal")]
        [InverseProperty("tblCalendarioPersonal")]
        public virtual tblCuadrantePersonal? idCuadrantePersonalNavigation { get; set; }
        [ForeignKey("idJornada")]
        [InverseProperty("tblCalendarioPersonal")]
        public virtual tblJornada? idJornadaNavigation { get; set; }
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblCalendarioPersonal")]
        public virtual tblLavanderia? idLavanderiaNavigation { get; set; }
        [ForeignKey("idPersona")]
        [InverseProperty("tblCalendarioPersonal")]
        public virtual tblPersona idPersonaNavigation { get; set; } = null!;
        [ForeignKey("idUsuario_validacion")]
        [InverseProperty("tblCalendarioPersonal")]
        public virtual tblUsuario? idUsuario_validacionNavigation { get; set; }
    }
}
