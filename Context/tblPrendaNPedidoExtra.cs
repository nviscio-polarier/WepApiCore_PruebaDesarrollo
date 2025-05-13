using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPrendaNPedidoExtra", Schema = "Office")]
    public partial class tblPrendaNPedidoExtra
    {
        [Key]
        public int idPrenda { get; set; }
        [Key]
        public int idPedidoExtra { get; set; }
        public int pedido { get; set; }
        public int repartido { get; set; }

        [ForeignKey("idPedidoExtra")]
        [InverseProperty("tblPrendaNPedidoExtra")]
        public virtual tblPedidosExtra idPedidoExtraNavigation { get; set; } = null!;
        [ForeignKey("idPrenda")]
        [InverseProperty("tblPrendaNPedidoExtra")]
        public virtual tblPrenda idPrendaNavigation { get; set; } = null!;
    }
}
