using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblSacasPendientes", Schema = "Logistica")]
    public partial class tblSacasPendientes
    {
        [Key]
        public int idSacasPendientes { get; set; }
        public int idEntidad { get; set; }
        public byte? idColorTapa { get; set; }
        public short cantidad { get; set; }

        [ForeignKey("idColorTapa")]
        [InverseProperty("tblSacasPendientes")]
        public virtual tblColorTapa? idColorTapaNavigation { get; set; }
        [ForeignKey("idEntidad")]
        [InverseProperty("tblSacasPendientes")]
        public virtual tblEntidad idEntidadNavigation { get; set; } = null!;
    }
}
