using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPrenda_historico_peso", Schema = "General")]
    public partial class tblPrenda_historico_peso
    {
        [Key]
        public int idPrenda { get; set; }
        [Key]
        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }
        public int peso { get; set; }

        [ForeignKey("idPrenda")]
        [InverseProperty("tblPrenda_historico_peso")]
        public virtual tblPrenda idPrendaNavigation { get; set; } = null!;
    }
}
