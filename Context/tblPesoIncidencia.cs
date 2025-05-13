using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPesoIncidencia", Schema = "Assistant")]
    public partial class tblPesoIncidencia
    {
        [Key]
        public byte idPeso { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        public decimal valor { get; set; }
    }
}
