using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblLogError", Schema = "GestionInterna")]
    public partial class tblLogError
    {
        [Key]
        public int idError { get; set; }
        public string? denominacion { get; set; }
        public string? error { get; set; }
    }
}
