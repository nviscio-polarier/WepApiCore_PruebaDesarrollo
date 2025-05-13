using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblFotoNArticuloEnvio", Schema = "Logistica")]
    public partial class tblFotoNArticuloEnvio
    {
        [Key]
        public int idFoto { get; set; }
        [Key]
        public int idArticuloEnvio { get; set; }
        public string nombre { get; set; } = null!;
        public byte[] documento { get; set; } = null!;
        public string extension { get; set; } = null!;

        [ForeignKey("idArticuloEnvio")]
        [InverseProperty("tblFotoNArticuloEnvio")]
        public virtual tblArticuloEnvio idArticuloEnvioNavigation { get; set; } = null!;
    }
}
