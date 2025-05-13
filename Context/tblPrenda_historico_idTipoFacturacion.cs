using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPrenda_historico_idTipoFacturacion", Schema = "General")]
    public partial class tblPrenda_historico_idTipoFacturacion
    {
        [Key]
        public int idPrenda { get; set; }
        [Key]
        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }
        public byte idTipoFacturacion { get; set; }

        [ForeignKey("idPrenda")]
        [InverseProperty("tblPrenda_historico_idTipoFacturacion")]
        public virtual tblPrenda idPrendaNavigation { get; set; } = null!;
        [ForeignKey("idTipoFacturacion")]
        [InverseProperty("tblPrenda_historico_idTipoFacturacion")]
        public virtual tblTipoFacturacion idTipoFacturacionNavigation { get; set; } = null!;
    }
}
