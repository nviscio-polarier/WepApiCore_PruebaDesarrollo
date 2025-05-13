using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblEstadoPedido", Schema = "Logistica")]
    public partial class tblEstadoPedido
    {
        public tblEstadoPedido()
        {
            tblPedido = new HashSet<tblPedido>();
        }

        [Key]
        public byte idEstadoPedido { get; set; }
        [StringLength(50)]
        public string denominacion { get; set; } = null!;
        public byte? codigo { get; set; }
        [StringLength(50)]
        public string? abreviatura { get; set; }

        [InverseProperty("idEstadoPedidoNavigation")]
        public virtual ICollection<tblPedido> tblPedido { get; set; }
    }
}
