using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblEnvio_Documento", Schema = "Logistica")]
    public partial class tblEnvio_Documento
    {
        [Key]
        public int idDocumento { get; set; }
        [Key]
        public int idEnvio { get; set; }
        public string nombre { get; set; } = null!;
        public byte[] documento { get; set; } = null!;
        public string extension { get; set; } = null!;
        public int idTipoDocumento_Envio { get; set; }

        [ForeignKey("idEnvio")]
        [InverseProperty("tblEnvio_Documento")]
        public virtual tblEnvio idEnvioNavigation { get; set; } = null!;
        [ForeignKey("idTipoDocumento_Envio")]
        [InverseProperty("tblEnvio_Documento")]
        public virtual tblTipoDocumento_Envio idTipoDocumento_EnvioNavigation { get; set; } = null!;
    }
}
