using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCarro", Schema = "Logistica")]
    public partial class tblCarro
    {
        public tblCarro()
        {
            tblLecturaCarro = new HashSet<tblLecturaCarro>();
        }

        [Key]
        public int idCarro { get; set; }
        public string? codigo { get; set; }

        [InverseProperty("idCarroNavigation")]
        public virtual ICollection<tblLecturaCarro> tblLecturaCarro { get; set; }
    }
}
