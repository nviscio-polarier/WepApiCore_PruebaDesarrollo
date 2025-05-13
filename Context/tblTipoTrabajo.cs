using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoTrabajo", Schema = "RRHH")]
    public partial class tblTipoTrabajo
    {
        public tblTipoTrabajo()
        {
            tblHistoricoNominas = new HashSet<tblHistoricoNominas>();
            tblJornada = new HashSet<tblJornada>();
            tblLlamamiento = new HashSet<tblLlamamiento>();
            tblNomina = new HashSet<tblNomina>();
            tblNomina_MX = new HashSet<tblNomina_MX>();
            tblPersona = new HashSet<tblPersona>();
            tblTipoTrabajoNUsuario = new HashSet<tblTipoTrabajoNUsuario>();
        }

        [StringLength(50)]
        public string denominacion { get; set; } = null!;
        [Key]
        public byte idTipoTrabajo { get; set; }
        public string? icon { get; set; }

        [InverseProperty("idTipoTrabajoNavigation")]
        public virtual tblCuentaContableNTipoTrabajo tblCuentaContableNTipoTrabajo { get; set; } = null!;
        [InverseProperty("idTipoTrabajoNavigation")]
        public virtual ICollection<tblHistoricoNominas> tblHistoricoNominas { get; set; }
        [InverseProperty("idTipoTrabajoNavigation")]
        public virtual ICollection<tblJornada> tblJornada { get; set; }
        [InverseProperty("idTipoTrabajoNavigation")]
        public virtual ICollection<tblLlamamiento> tblLlamamiento { get; set; }
        [InverseProperty("idTipoTrabajoNavigation")]
        public virtual ICollection<tblNomina> tblNomina { get; set; }
        [InverseProperty("idTipoTrabajoNavigation")]
        public virtual ICollection<tblNomina_MX> tblNomina_MX { get; set; }
        [InverseProperty("idTipoTrabajoNavigation")]
        public virtual ICollection<tblPersona> tblPersona { get; set; }
        [InverseProperty("idTipoTrabajoNavigation")]
        public virtual ICollection<tblTipoTrabajoNUsuario> tblTipoTrabajoNUsuario { get; set; }
    }
}
