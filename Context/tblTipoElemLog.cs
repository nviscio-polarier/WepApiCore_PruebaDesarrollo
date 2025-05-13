using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoElemLog", Schema = "Logistica")]
    public partial class tblTipoElemLog
    {
        public tblTipoElemLog()
        {
            tblCantidadNMovimientoElemLog = new HashSet<tblCantidadNMovimientoElemLog>();
            tblElemLogNPedido = new HashSet<tblElemLogNPedido>();
            tblGrupoPrendaEst = new HashSet<tblGrupoPrendaEst>();
            tblStockTipoElemLogNEntidad = new HashSet<tblStockTipoElemLogNEntidad>();
        }

        [Key]
        public byte idTipoElemLog { get; set; }
        public string denominacion { get; set; } = null!;
        public int idTraduccion { get; set; }

        [ForeignKey("idTraduccion")]
        [InverseProperty("tblTipoElemLog")]
        public virtual tblTraduccion idTraduccionNavigation { get; set; } = null!;
        [InverseProperty("idTipoElemLogNavigation")]
        public virtual ICollection<tblCantidadNMovimientoElemLog> tblCantidadNMovimientoElemLog { get; set; }
        [InverseProperty("idTipoElemLogNavigation")]
        public virtual ICollection<tblElemLogNPedido> tblElemLogNPedido { get; set; }
        [InverseProperty("idTipoElemLogNavigation")]
        public virtual ICollection<tblGrupoPrendaEst> tblGrupoPrendaEst { get; set; }
        [InverseProperty("idTipoElemLogNavigation")]
        public virtual ICollection<tblStockTipoElemLogNEntidad> tblStockTipoElemLogNEntidad { get; set; }
    }
}
