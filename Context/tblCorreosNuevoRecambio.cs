using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCorreosNuevoRecambio", Schema = "Assistant")]
    public partial class tblCorreosNuevoRecambio
    {
        [Key]
        public int idCorreo { get; set; }
        public string denominacion { get; set; } = null!;
    }
}
