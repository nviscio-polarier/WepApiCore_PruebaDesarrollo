using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblIdioma", Schema = "General")]
    public partial class tblIdioma
    {
        public tblIdioma()
        {
            tblLavanderia = new HashSet<tblLavanderia>();
            tblUsuario = new HashSet<tblUsuario>();
        }

        [Key]
        public short idIdioma { get; set; }
        [StringLength(50)]
        public string? denominacion { get; set; }
        [StringLength(2)]
        public string? codigo { get; set; }

        [InverseProperty("idIdiomaNavigation")]
        public virtual ICollection<tblLavanderia> tblLavanderia { get; set; }
        [InverseProperty("idIdiomaNavigation")]
        public virtual ICollection<tblUsuario> tblUsuario { get; set; }
    }
}
