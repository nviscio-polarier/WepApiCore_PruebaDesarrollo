using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblEntidad_historico_idTipoFacturacion", Schema = "General")]
    public partial class tblEntidad_historico_idTipoFacturacion
    {
        [Key]
        public int idEntidad { get; set; }
        [Key]
        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }
        public byte idTipoFacturacion { get; set; }

        [ForeignKey("idEntidad")]
        [InverseProperty("tblEntidad_historico_idTipoFacturacion")]
        public virtual tblEntidad idEntidadNavigation { get; set; } = null!;
    }
}
