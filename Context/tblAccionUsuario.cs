using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAccionUsuario", Schema = "GestionInterna")]
    public partial class tblAccionUsuario
    {
        public tblAccionUsuario()
        {
            tblLogAcciones = new HashSet<tblLogAcciones>();
        }

        [Key]
        public byte idAccion { get; set; }
        public string denominacion { get; set; } = null!;

        [InverseProperty("idAccionNavigation")]
        public virtual ICollection<tblLogAcciones> tblLogAcciones { get; set; }
    }
}
