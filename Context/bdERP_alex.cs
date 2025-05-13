using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    public partial class bdERP_alex
    {
        [Key]
        public int ID { get; set; }
        [StringLength(255)]
        public string? Descripcion { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Precio { get; set; }
    }
}
