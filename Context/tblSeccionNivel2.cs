using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblSeccionNivel2", Schema = "Inventarios")]
    public partial class tblSeccionNivel2
    {
        public tblSeccionNivel2()
        {
            tblAlmacen = new HashSet<tblAlmacen>();
        }

        [Key]
        public int idSeccionNivel2 { get; set; }
        public int idSeccionNivel1 { get; set; }
        public string denominacion { get; set; } = null!;
        public bool eliminado { get; set; }

        [ForeignKey("idSeccionNivel1")]
        [InverseProperty("tblSeccionNivel2")]
        public virtual tblSeccionNivel1 idSeccionNivel1Navigation { get; set; } = null!;
        [InverseProperty("idSeccionNivel2Navigation")]
        public virtual ICollection<tblAlmacen> tblAlmacen { get; set; }
    }
}
