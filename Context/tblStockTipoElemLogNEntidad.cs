using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblStockTipoElemLogNEntidad", Schema = "Logistica")]
    public partial class tblStockTipoElemLogNEntidad
    {
        [Key]
        public int idEntidad { get; set; }
        [Key]
        public byte idTipoElemLog { get; set; }
        public int cantidad { get; set; }

        [ForeignKey("idEntidad")]
        [InverseProperty("tblStockTipoElemLogNEntidad")]
        public virtual tblEntidad idEntidadNavigation { get; set; } = null!;
        [ForeignKey("idTipoElemLog")]
        [InverseProperty("tblStockTipoElemLogNEntidad")]
        public virtual tblTipoElemLog idTipoElemLogNavigation { get; set; } = null!;
    }
}
