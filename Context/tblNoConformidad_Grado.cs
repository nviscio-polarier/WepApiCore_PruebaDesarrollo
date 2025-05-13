using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    public partial class tblNoConformidad_Grado
    {
        public tblNoConformidad_Grado()
        {
            tblNoConformidad = new HashSet<tblNoConformidad>();
        }

        [Key]
        public byte idGrado { get; set; }
        public string denominacion { get; set; } = null!;

        [InverseProperty("idGradoNavigation")]
        public virtual ICollection<tblNoConformidad> tblNoConformidad { get; set; }
    }
}
