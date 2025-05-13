using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAuxPlegadoras", Schema = "MyRealBonus")]
    public partial class tblAuxPlegadoras
    {
        [Key]
        public int idMaquina { get; set; }
        public int? idTipoMaquina { get; set; }
    }
}
