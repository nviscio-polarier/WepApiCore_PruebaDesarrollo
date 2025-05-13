using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblFabricante", Schema = "Assistant")]
    public partial class tblFabricante
    {
        [Key]
        public short idFabricante { get; set; }
        public string denominacion { get; set; } = null!;
    }
}
