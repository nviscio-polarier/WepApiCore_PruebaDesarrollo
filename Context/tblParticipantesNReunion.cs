using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblParticipantesNReunion", Schema = "AppComercial")]
    public partial class tblParticipantesNReunion
    {
        [Key]
        public int idParticipanteNReunion { get; set; }
        public int idReunion { get; set; }
        public string nombre { get; set; } = null!;

        [ForeignKey("idReunion")]
        [InverseProperty("tblParticipantesNReunion")]
        public virtual tblReunion idReunionNavigation { get; set; } = null!;
    }
}
