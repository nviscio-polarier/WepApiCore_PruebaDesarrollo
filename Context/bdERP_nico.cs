using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    public partial class bdERP_nico
    {
        [Key]
        public int ID { get; set; }
        [StringLength(100)]
        public string? Nombre { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Fecha { get; set; }
    }
}
