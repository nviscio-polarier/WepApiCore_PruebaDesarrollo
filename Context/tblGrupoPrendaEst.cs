using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblGrupoPrendaEst", Schema = "Logistica")]
    public partial class tblGrupoPrendaEst
    {
        public tblGrupoPrendaEst()
        {
            tblCantidadNMovimientoElemLog = new HashSet<tblCantidadNMovimientoElemLog>();
            idLavanderia = new HashSet<tblLavanderia>();
        }

        [Key]
        public byte idGrupoPrendaEst { get; set; }
        public string denominacion { get; set; } = null!;
        public byte idTipoElemLog { get; set; }

        [ForeignKey("idTipoElemLog")]
        [InverseProperty("tblGrupoPrendaEst")]
        public virtual tblTipoElemLog idTipoElemLogNavigation { get; set; } = null!;
        [InverseProperty("idGrupoPrendaEstNavigation")]
        public virtual ICollection<tblCantidadNMovimientoElemLog> tblCantidadNMovimientoElemLog { get; set; }

        [ForeignKey("idGrupoPrendaEst")]
        [InverseProperty("idGrupoPrendaEst")]
        public virtual ICollection<tblLavanderia> idLavanderia { get; set; }
    }
}
