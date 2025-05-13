using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCategoria_Grupo", Schema = "RRHH")]
    public partial class tblCategoria_Grupo
    {
        public tblCategoria_Grupo()
        {
            idCategoria = new HashSet<tblCategoria>();
        }

        [Key]
        public short idCategoria_Grupo { get; set; }
        public string denominacion { get; set; } = null!;

        [ForeignKey("idCategoria_Grupo")]
        [InverseProperty("idCategoria_Grupo")]
        public virtual ICollection<tblCategoria> idCategoria { get; set; }
    }
}
