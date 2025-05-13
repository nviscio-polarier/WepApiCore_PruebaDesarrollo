using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblSeccionNivel1", Schema = "Inventarios")]
    public partial class tblSeccionNivel1
    {
        public tblSeccionNivel1()
        {
            tblSeccionNivel2 = new HashSet<tblSeccionNivel2>();
        }

        [Key]
        public int idSeccionNivel1 { get; set; }
        public int idEntidad { get; set; }
        public string denominacion { get; set; } = null!;
        public int? idRutaSeccion { get; set; }
        public bool eliminado { get; set; }

        [ForeignKey("idEntidad")]
        [InverseProperty("tblSeccionNivel1")]
        public virtual tblEntidad idEntidadNavigation { get; set; } = null!;
        [InverseProperty("idSeccionNivel1Navigation")]
        public virtual ICollection<tblSeccionNivel2> tblSeccionNivel2 { get; set; }
    }
}
