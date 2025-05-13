using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblRegistroEstado", Schema = "Logistica")]
    public partial class tblRegistroEstado
    {
        public tblRegistroEstado()
        {
            tblRegistroLectura = new HashSet<tblRegistroLectura>();
        }

        [Key]
        public byte idEstado { get; set; }
        public string? denominacion { get; set; }
        public int? idTraduccion { get; set; }

        [ForeignKey("idTraduccion")]
        [InverseProperty("tblRegistroEstado")]
        public virtual tblTraduccion? idTraduccionNavigation { get; set; }
        [InverseProperty("idEstadoNavigation")]
        public virtual ICollection<tblRegistroLectura> tblRegistroLectura { get; set; }
    }
}
