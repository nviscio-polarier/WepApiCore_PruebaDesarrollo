using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPrendaNPedido", Schema = "Logistica")]
    [Index("idPedido", Name = "IX_tblPrendaNPedido_idPedido")]
    [Index("idPrenda", Name = "IX_tblPrendaNPedido_idPrenda")]
    public partial class tblPrendaNPedido
    {
        [Key]
        public int idPrendaNPedido { get; set; }
        public int idPedido { get; set; }
        public int idPrenda { get; set; }
        public int peticion { get; set; }
        public int? unidadesRepartidas { get; set; }
        public int? rechazo { get; set; }
        public int? retiro { get; set; }
        public int? stockActual { get; set; }
        public bool isAdded { get; set; }

        [ForeignKey("idPedido")]
        [InverseProperty("tblPrendaNPedido")]
        public virtual tblPedido idPedidoNavigation { get; set; } = null!;
        [ForeignKey("idPrenda")]
        [InverseProperty("tblPrendaNPedido")]
        public virtual tblPrenda idPrendaNavigation { get; set; } = null!;
    }
}
