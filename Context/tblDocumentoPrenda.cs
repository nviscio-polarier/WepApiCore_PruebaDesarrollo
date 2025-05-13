using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblDocumentoPrenda", Schema = "General")]
    public partial class tblDocumentoPrenda
    {
        [Key]
        public int idDocumento { get; set; }
        public int? idPrenda { get; set; }
        public string nombre { get; set; } = null!;
        public byte[] documento { get; set; } = null!;
        public string extension { get; set; } = null!;

        [ForeignKey("idPrenda")]
        [InverseProperty("tblDocumentoPrenda")]
        public virtual tblPrenda? idPrendaNavigation { get; set; }
    }
}
