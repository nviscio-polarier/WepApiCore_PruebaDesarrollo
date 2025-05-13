using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblRegistroCarro", Schema = "Logistica")]
    public partial class tblRegistroCarro
    {
        [Key]
        public int idCarro { get; set; }
        public string? codigo { get; set; }
    }
}
