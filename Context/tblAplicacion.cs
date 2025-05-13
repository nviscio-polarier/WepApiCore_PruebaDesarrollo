using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAplicacion", Schema = "GestionInterna")]
    public partial class tblAplicacion
    {
        public tblAplicacion()
        {
            tblFormulario = new HashSet<tblFormulario>();
            tblUsuario = new HashSet<tblUsuario>();
        }

        [Key]
        public int idAplicacion { get; set; }
        public string? denominacion { get; set; }
        public string? descripcion { get; set; }
        public int idTraduccion { get; set; }
        public int? idFormularioInicio { get; set; }
        public int? orden { get; set; }
        public string? icon { get; set; }
        [StringLength(7)]
        public string? color { get; set; }

        [ForeignKey("idFormularioInicio")]
        [InverseProperty("tblAplicacion")]
        public virtual tblFormulario? idFormularioInicioNavigation { get; set; }
        [ForeignKey("idTraduccion")]
        [InverseProperty("tblAplicacion")]
        public virtual tblTraduccion idTraduccionNavigation { get; set; } = null!;
        [InverseProperty("idAplicacionNavigation")]
        public virtual ICollection<tblFormulario> tblFormulario { get; set; }
        [InverseProperty("idAplicacionInicialNavigation")]
        public virtual ICollection<tblUsuario> tblUsuario { get; set; }
    }
}
