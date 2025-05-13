using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoPedido", Schema = "Logistica")]
    public partial class tblTipoPedido
    {
        public tblTipoPedido()
        {
            tblPedido = new HashSet<tblPedido>();
            idEntidad = new HashSet<tblEntidad>();
        }

        [Key]
        public byte idTipoPedido { get; set; }
        [StringLength(50)]
        public string denominacion { get; set; } = null!;
        public byte codigo { get; set; }

        [InverseProperty("idTipoPedidoNavigation")]
        public virtual ICollection<tblPedido> tblPedido { get; set; }

        [ForeignKey("idTipoPedido")]
        [InverseProperty("idTipoPedido")]
        public virtual ICollection<tblEntidad> idEntidad { get; set; }
    }
}
