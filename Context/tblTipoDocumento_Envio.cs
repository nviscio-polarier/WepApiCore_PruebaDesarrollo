using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoDocumento_Envio", Schema = "Logistica")]
    public partial class tblTipoDocumento_Envio
    {
        public tblTipoDocumento_Envio()
        {
            tblEnvio_Documento = new HashSet<tblEnvio_Documento>();
            idEnvio = new HashSet<tblEnvio>();
        }

        [Key]
        public int idTipoDocumento_Envio { get; set; }
        public string denominacion { get; set; } = null!;

        [InverseProperty("idTipoDocumento_EnvioNavigation")]
        public virtual ICollection<tblEnvio_Documento> tblEnvio_Documento { get; set; }

        [ForeignKey("idTipoDocumento_Envio")]
        [InverseProperty("idTipoDocumento_Envio")]
        public virtual ICollection<tblEnvio> idEnvio { get; set; }
    }
}
