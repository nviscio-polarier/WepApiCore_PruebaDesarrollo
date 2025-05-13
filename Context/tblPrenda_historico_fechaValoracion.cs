using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPrenda_historico_fechaValoracion", Schema = "General")]
    public partial class tblPrenda_historico_fechaValoracion
    {
        [Key]
        public int idPrenda { get; set; }
        [Key]
        [Precision(0)]
        public DateTimeOffset fecha { get; set; }
        [Precision(0)]
        public DateTimeOffset fechaValoracion { get; set; }

        [ForeignKey("idPrenda")]
        [InverseProperty("tblPrenda_historico_fechaValoracion")]
        public virtual tblPrenda idPrendaNavigation { get; set; } = null!;
    }
}
