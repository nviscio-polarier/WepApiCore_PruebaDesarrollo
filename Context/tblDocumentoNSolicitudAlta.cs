using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblDocumentoNSolicitudAlta", Schema = "RRHH")]
    public partial class tblDocumentoNSolicitudAlta
    {
        [Key]
        public int idSolicitudAlta { get; set; }
        [Key]
        public int idDocumento { get; set; }
        public bool isFromGestoria { get; set; }

        [ForeignKey("idDocumento")]
        [InverseProperty("tblDocumentoNSolicitudAlta")]
        public virtual tblDocumento idDocumentoNavigation { get; set; } = null!;
        [ForeignKey("idSolicitudAlta")]
        [InverseProperty("tblDocumentoNSolicitudAlta")]
        public virtual tblSolicitudAlta idSolicitudAltaNavigation { get; set; } = null!;
    }
}
