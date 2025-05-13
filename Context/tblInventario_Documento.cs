using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblInventario_Documento", Schema = "Inventarios")]
    public partial class tblInventario_Documento
    {
        [Key]
        public int idDocumento { get; set; }
        public int idInventario { get; set; }
        public string nombre { get; set; } = null!;
        public byte[] documento { get; set; } = null!;
        public string extension { get; set; } = null!;

        [ForeignKey("idInventario")]
        [InverseProperty("tblInventario_Documento")]
        public virtual tblInventario idInventarioNavigation { get; set; } = null!;
    }
}
