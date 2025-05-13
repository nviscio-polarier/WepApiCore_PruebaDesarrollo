using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblLogAcciones", Schema = "GestionInterna")]
    public partial class tblLogAcciones
    {
        [Key]
        public int idUsuario { get; set; }
        [Key]
        public DateTimeOffset fecha { get; set; }
        public int? idLavanderia { get; set; }
        public byte idAccion { get; set; }
        public int? idFormulario { get; set; }
        public string? navegador { get; set; }
        public string? navegador_version { get; set; }
        public string? loginUsername { get; set; }

        [ForeignKey("idAccion")]
        [InverseProperty("tblLogAcciones")]
        public virtual tblAccionUsuario idAccionNavigation { get; set; } = null!;
        [ForeignKey("idFormulario")]
        [InverseProperty("tblLogAcciones")]
        public virtual tblFormulario? idFormularioNavigation { get; set; }
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblLogAcciones")]
        public virtual tblLavanderia? idLavanderiaNavigation { get; set; }
        [ForeignKey("idUsuario")]
        [InverseProperty("tblLogAcciones")]
        public virtual tblUsuario idUsuarioNavigation { get; set; } = null!;
    }
}
