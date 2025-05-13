using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblLecturaCarro_Estado", Schema = "MyRealData")]
    public partial class tblLecturaCarro_Estado
    {
        public tblLecturaCarro_Estado()
        {
            tblLecturaCarro = new HashSet<tblLecturaCarro>();
        }

        [Key]
        public byte idLecturaCarro_Estado { get; set; }
        public string? denominacion { get; set; }

        [InverseProperty("idLecturaCarro_EstadoNavigation")]
        public virtual ICollection<tblLecturaCarro> tblLecturaCarro { get; set; }
    }
}
