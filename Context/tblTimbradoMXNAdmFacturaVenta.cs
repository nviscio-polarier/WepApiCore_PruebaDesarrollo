using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTimbradoMXNAdmFacturaVenta", Schema = "Administracion")]
    public partial class tblTimbradoMXNAdmFacturaVenta
    {
        [Key]
        public int idAdmFacturaVenta { get; set; }
        public string? formaPago { get; set; }
        public string? folioFiscal { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? certificacion { get; set; }
        public string? serieCertificadoSAT { get; set; }
        public string? serieCertificado { get; set; }
        public string? selloCFD { get; set; }
        public string? selloSAT { get; set; }
        public string? linkQR { get; set; }

        [ForeignKey("idAdmFacturaVenta")]
        [InverseProperty("tblTimbradoMXNAdmFacturaVenta")]
        public virtual tblAdmFacturaVenta idAdmFacturaVentaNavigation { get; set; } = null!;
    }
}
