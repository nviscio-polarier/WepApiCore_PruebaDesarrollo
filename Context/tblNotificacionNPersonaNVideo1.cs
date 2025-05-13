using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Keyless]
    [Table("tblNotificacionNPersonaNVideo", Schema = "MyRealLearning")]
    public partial class tblNotificacionNPersonaNVideo1
    {
        public int idNotificacion { get; set; }
        public int idPersonaVideo { get; set; }

        [ForeignKey("idNotificacion")]
        public virtual tblNotificacion idNotificacionNavigation { get; set; } = null!;
        [ForeignKey("idPersonaVideo")]
        public virtual tblVideoNPersona idPersonaVideoNavigation { get; set; } = null!;
    }
}
