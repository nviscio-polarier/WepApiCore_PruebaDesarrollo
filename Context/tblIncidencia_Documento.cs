using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblIncidencia_Documento", Schema = "Incidencias")]
    public partial class tblIncidencia_Documento
    {
        [Key]
        public int idDocumento { get; set; }
        [Key]
        public int idIncidencia { get; set; }
        public string nombre { get; set; } = null!;
        public byte[]? documento { get; set; }
        public string extension { get; set; } = null!;

        [ForeignKey("idIncidencia")]
        [InverseProperty("tblIncidencia_Documento")]
        public virtual tblIncidencia idIncidenciaNavigation { get; set; } = null!;
    }
}
