using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblMezclaSucioCliente", Schema = "Logistica")]
    public partial class tblMezclaSucioCliente
    {
        [Key]
        public int idMezclaSucioCliente { get; set; }
        public int idEntidad { get; set; }
        public int idLavanderia { get; set; }
        public DateTimeOffset fecha { get; set; }
        [Column(TypeName = "decimal(5, 4)")]
        public decimal porcentajeMuestra { get; set; }
        public short cantidadMuestra { get; set; }
        public int idMovimientoElemLog { get; set; }
        public short cantidad_errorMezcla { get; set; }
        public short cantidad_errorCordon { get; set; }

        [ForeignKey("idEntidad")]
        [InverseProperty("tblMezclaSucioCliente")]
        public virtual tblEntidad idEntidadNavigation { get; set; } = null!;
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblMezclaSucioCliente")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
        [ForeignKey("idMovimientoElemLog")]
        [InverseProperty("tblMezclaSucioCliente")]
        public virtual tblMovimientoElemLog idMovimientoElemLogNavigation { get; set; } = null!;
    }
}
