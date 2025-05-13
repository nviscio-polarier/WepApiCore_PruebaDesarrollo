using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoUsuario", Schema = "GestionInterna")]
    public partial class tblTipoUsuario
    {
        public tblTipoUsuario()
        {
            tblUsuario = new HashSet<tblUsuario>();
        }

        [Key]
        public int idTipoUsuario { get; set; }
        public string denominacion { get; set; } = null!;

        [InverseProperty("idTipoUsuarioNavigation")]
        public virtual ICollection<tblUsuario> tblUsuario { get; set; }
    }
}
