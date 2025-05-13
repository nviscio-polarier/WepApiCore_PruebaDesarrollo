using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCorreoAltaGestoriaNCentroLav", Schema = "General")]
    public partial class tblCorreoAltaGestoriaNCentroLav
    {
        [Key]
        public int idCorreoAltaGestoriaNCentroLav { get; set; }
        public int? idLavanderia { get; set; }
        public int? idCentroTrabajo { get; set; }
        public string correo { get; set; } = null!;

        [ForeignKey("idCentroTrabajo")]
        [InverseProperty("tblCorreoAltaGestoriaNCentroLav")]
        public virtual tblCentroTrabajo? idCentroTrabajoNavigation { get; set; }
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblCorreoAltaGestoriaNCentroLav")]
        public virtual tblLavanderia? idLavanderiaNavigation { get; set; }
    }
}
