using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPermiso", Schema = "GestionInterna")]
    public partial class tblPermiso
    {
        public tblPermiso()
        {
            idFormulario = new HashSet<tblFormulario>();
            idUsuario = new HashSet<tblUsuario>();
        }

        [Key]
        public int idPermiso { get; set; }
        public string denominacion { get; set; } = null!;
        public string codigo { get; set; } = null!;
        public int? idTraduccion { get; set; }

        [ForeignKey("idTraduccion")]
        [InverseProperty("tblPermiso")]
        public virtual tblTraduccion? idTraduccionNavigation { get; set; }

        [ForeignKey("idPermiso")]
        [InverseProperty("idPermiso")]
        public virtual ICollection<tblFormulario> idFormulario { get; set; }
        [ForeignKey("idPermiso")]
        [InverseProperty("idPermiso")]
        public virtual ICollection<tblUsuario> idUsuario { get; set; }
    }
}
