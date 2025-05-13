using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoLectura", Schema = "MyRealData")]
    public partial class tblTipoLectura
    {
        public tblTipoLectura()
        {
            tblLecturaCarro = new HashSet<tblLecturaCarro>();
        }

        [Key]
        public byte idTipoLectura { get; set; }
        public string? denominacion { get; set; }

        [InverseProperty("idTipoLecturaNavigation")]
        public virtual ICollection<tblLecturaCarro> tblLecturaCarro { get; set; }
    }
}
