using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCarpetaDocumentos", Schema = "RRHH")]
    public partial class tblCarpetaDocumentos
    {
        [Key]
        public short idCarpetaDocumentos { get; set; }
        public string denominacion { get; set; } = null!;
        [StringLength(50)]
        public string? icon { get; set; }
    }
}
