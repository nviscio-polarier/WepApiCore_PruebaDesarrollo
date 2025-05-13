using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    public partial class tblEstado
    {
        public tblEstado()
        {
            tblNoConformidad = new HashSet<tblNoConformidad>();
        }

        [Key]
        public byte idEstado { get; set; }
        public string denominacion { get; set; } = null!;

        [InverseProperty("idEstadoNavigation")]
        public virtual ICollection<tblNoConformidad> tblNoConformidad { get; set; }
    }
}
