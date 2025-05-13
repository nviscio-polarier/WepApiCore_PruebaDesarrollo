using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCargo", Schema = "GestionInterna")]
    public partial class tblCargo
    {
        public tblCargo()
        {
            tblUsuario = new HashSet<tblUsuario>();
            idFormulario = new HashSet<tblFormulario>();
        }

        [Key]
        public short idCargo { get; set; }
        [StringLength(50)]
        public string denominacion { get; set; } = null!;
        public byte estatus { get; set; }
        public bool? isEntidad { get; set; }

        [InverseProperty("idCargoNavigation")]
        public virtual ICollection<tblUsuario> tblUsuario { get; set; }

        [ForeignKey("idCargo")]
        [InverseProperty("idCargo")]
        public virtual ICollection<tblFormulario> idFormulario { get; set; }
    }
}
