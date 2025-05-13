using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblFormulario", Schema = "GestionInterna")]
    public partial class tblFormulario
    {
        public tblFormulario()
        {
            tblAplicacion = new HashSet<tblAplicacion>();
            tblFormularioNUsuario = new HashSet<tblFormularioNUsuario>();
            tblLogAcciones = new HashSet<tblLogAcciones>();
            tblReports = new HashSet<tblReports>();
            tblUsuario = new HashSet<tblUsuario>();
            idCargo = new HashSet<tblCargo>();
            idLavanderia = new HashSet<tblLavanderia>();
            idModulo = new HashSet<tblModulo>();
            idPermiso = new HashSet<tblPermiso>();
        }

        [Key]
        public int idFormulario { get; set; }
        [StringLength(50)]
        public string denominacion { get; set; } = null!;
        public string formulario { get; set; } = null!;
        public int idApartado { get; set; }
        public bool informe { get; set; }
        public string? icon { get; set; }
        public int? orden { get; set; }
        public int? idTraduccion { get; set; }
        public bool visibleEntidad { get; set; }
        public int? idAplicacion { get; set; }

        [ForeignKey("idApartado")]
        [InverseProperty("tblFormulario")]
        public virtual tblApartado idApartadoNavigation { get; set; } = null!;
        [ForeignKey("idAplicacion")]
        [InverseProperty("tblFormulario")]
        public virtual tblAplicacion? idAplicacionNavigation { get; set; }
        [ForeignKey("idTraduccion")]
        [InverseProperty("tblFormulario")]
        public virtual tblTraduccion? idTraduccionNavigation { get; set; }
        [InverseProperty("idFormularioInicioNavigation")]
        public virtual ICollection<tblAplicacion> tblAplicacion { get; set; }
        [InverseProperty("idFormularioNavigation")]
        public virtual ICollection<tblFormularioNUsuario> tblFormularioNUsuario { get; set; }
        [InverseProperty("idFormularioNavigation")]
        public virtual ICollection<tblLogAcciones> tblLogAcciones { get; set; }
        [InverseProperty("idFormularioNavigation")]
        public virtual ICollection<tblReports> tblReports { get; set; }
        [InverseProperty("idFormularioInicioNavigation")]
        public virtual ICollection<tblUsuario> tblUsuario { get; set; }

        [ForeignKey("idFormulario")]
        [InverseProperty("idFormulario")]
        public virtual ICollection<tblCargo> idCargo { get; set; }
        [ForeignKey("idFormulario")]
        [InverseProperty("idFormulario")]
        public virtual ICollection<tblLavanderia> idLavanderia { get; set; }
        [ForeignKey("idFormulario")]
        [InverseProperty("idFormulario")]
        public virtual ICollection<tblModulo> idModulo { get; set; }
        [ForeignKey("idFormulario")]
        [InverseProperty("idFormulario")]
        public virtual ICollection<tblPermiso> idPermiso { get; set; }
    }
}
