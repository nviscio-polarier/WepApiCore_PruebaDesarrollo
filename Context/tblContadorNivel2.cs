using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblContadorNivel2", Schema = "Energeticos")]
    public partial class tblContadorNivel2
    {
        [Key]
        public short idContadorNivel2 { get; set; }
        [StringLength(50)]
        public string denominacion { get; set; } = null!;
        public short idContadorNivel1 { get; set; }
    }
}
