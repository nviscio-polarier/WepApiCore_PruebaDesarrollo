using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblDocumentoNNomina", Schema = "RRHH")]
    public partial class tblDocumentoNNomina
    {
        [Key]
        public int idDocumentoNNomina { get; set; }
        public int idNomina { get; set; }
        public string denominacion { get; set; } = null!;
        public string extension { get; set; } = null!;
        public byte[] documento { get; set; } = null!;
        public DateTimeOffset fecha { get; set; }
        public int idUsuario { get; set; }
        public bool isFromGestoria { get; set; }

        [ForeignKey("idNomina")]
        [InverseProperty("tblDocumentoNNomina")]
        public virtual tblNomina idNominaNavigation { get; set; } = null!;
        [ForeignKey("idUsuario")]
        [InverseProperty("tblDocumentoNNomina")]
        public virtual tblUsuario idUsuarioNavigation { get; set; } = null!;
    }
}
