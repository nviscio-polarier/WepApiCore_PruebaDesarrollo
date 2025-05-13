using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    public partial class tblFoto
    {
        [Key]
        public int idFoto { get; set; }
        public int idNoConformidad { get; set; }
        public byte[]? img { get; set; }

        [ForeignKey("idNoConformidad")]
        [InverseProperty("tblFoto")]
        public virtual tblNoConformidad idNoConformidadNavigation { get; set; } = null!;
    }
}
