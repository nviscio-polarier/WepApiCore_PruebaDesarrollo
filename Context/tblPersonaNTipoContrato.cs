using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPersonaNTipoContrato", Schema = "RRHH")]
    public partial class tblPersonaNTipoContrato
    {
        [Key]
        public int idPersona { get; set; }
        [Key]
        [Column(TypeName = "date")]
        public DateTime fechaAltaContrato { get; set; }
        [Column(TypeName = "date")]
        public DateTime? fechaBajaContrato { get; set; }
        public short? numDiasPeriodoPrueba { get; set; }
        public short? idTipoContrato { get; set; }
        public byte? idMotivoBaja { get; set; }

        [ForeignKey("idMotivoBaja")]
        [InverseProperty("tblPersonaNTipoContrato")]
        public virtual tblMotivoBaja? idMotivoBajaNavigation { get; set; }
        [ForeignKey("idPersona")]
        [InverseProperty("tblPersonaNTipoContrato")]
        public virtual tblPersona idPersonaNavigation { get; set; } = null!;
        [ForeignKey("idTipoContrato")]
        [InverseProperty("tblPersonaNTipoContrato")]
        public virtual tblTipoContrato? idTipoContratoNavigation { get; set; }
    }
}
