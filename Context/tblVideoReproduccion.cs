using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblVideoReproduccion", Schema = "MyRealLearning")]
    public partial class tblVideoReproduccion
    {
        [Key]
        public int idVideoRepro { get; set; }
        public int? idPersonaVideo { get; set; }
        public double? ultimaPosicion { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? fechaRegistro { get; set; }
    }
}
