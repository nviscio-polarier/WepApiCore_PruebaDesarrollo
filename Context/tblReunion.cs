using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblReunion", Schema = "AppComercial")]
    public partial class tblReunion
    {
        public tblReunion()
        {
            tblIncidenciaNReunion = new HashSet<tblIncidenciaNReunion>();
            tblParticipantesNReunion = new HashSet<tblParticipantesNReunion>();
        }

        [Key]
        public int idReunion { get; set; }
        public int idTipoReunion { get; set; }
        public int idFormatoReunion { get; set; }
        public int? idEntidad { get; set; }
        [Precision(0)]
        public DateTimeOffset? fecha { get; set; }
        public string? observaciones { get; set; }
        public int? idEncuesta { get; set; }
        public int? idCompañia { get; set; }
        public int? idUsuario { get; set; }

        [ForeignKey("idCompañia")]
        [InverseProperty("tblReunion")]
        public virtual tblCompañia? idCompañiaNavigation { get; set; }
        [ForeignKey("idEntidad")]
        [InverseProperty("tblReunion")]
        public virtual tblEntidad? idEntidadNavigation { get; set; }
        [ForeignKey("idFormatoReunion")]
        [InverseProperty("tblReunion")]
        public virtual tblFormatoReunion idFormatoReunionNavigation { get; set; } = null!;
        [ForeignKey("idTipoReunion")]
        [InverseProperty("tblReunion")]
        public virtual tblTipoReunion idTipoReunionNavigation { get; set; } = null!;
        [ForeignKey("idUsuario")]
        [InverseProperty("tblReunion")]
        public virtual tblUsuario? idUsuarioNavigation { get; set; }
        [InverseProperty("idReunionNavigation")]
        public virtual ICollection<tblIncidenciaNReunion> tblIncidenciaNReunion { get; set; }
        [InverseProperty("idReunionNavigation")]
        public virtual ICollection<tblParticipantesNReunion> tblParticipantesNReunion { get; set; }
    }
}
