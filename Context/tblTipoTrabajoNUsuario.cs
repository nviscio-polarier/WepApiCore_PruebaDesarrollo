using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoTrabajoNUsuario", Schema = "GestionInterna")]
    public partial class tblTipoTrabajoNUsuario
    {
        [Key]
        public int idUsuario { get; set; }
        [Key]
        public int idLavanderia { get; set; }
        [Key]
        public byte idTipoTrabajo { get; set; }
        public bool gestionaPlusesNomina { get; set; }

        [ForeignKey("idLavanderia")]
        [InverseProperty("tblTipoTrabajoNUsuario")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
        [ForeignKey("idTipoTrabajo")]
        [InverseProperty("tblTipoTrabajoNUsuario")]
        public virtual tblTipoTrabajo idTipoTrabajoNavigation { get; set; } = null!;
        [ForeignKey("idUsuario")]
        [InverseProperty("tblTipoTrabajoNUsuario")]
        public virtual tblUsuario idUsuarioNavigation { get; set; } = null!;
    }
}
