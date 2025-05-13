using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    public partial class bdERP_joan
    {
        [Key]
        public int ID { get; set; }
        [StringLength(50)]
        public string? Categoria { get; set; }
        public int? Stock { get; set; }
    }
}
