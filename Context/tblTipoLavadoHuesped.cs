using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoLavadoHuesped", Schema = "Logistica")]
    public partial class tblTipoLavadoHuesped
    {
        [Key]
        public int idTipoLavado { get; set; }
        public string? denominacion { get; set; }
    }
}
