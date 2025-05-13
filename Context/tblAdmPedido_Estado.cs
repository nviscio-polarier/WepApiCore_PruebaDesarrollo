using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAdmPedido_Estado", Schema = "Administracion")]
    public partial class tblAdmPedido_Estado
    {
        public tblAdmPedido_Estado()
        {
            tblAdmPedidoCliente = new HashSet<tblAdmPedidoCliente>();
            tblAdmPedidoProveedor = new HashSet<tblAdmPedidoProveedor>();
        }

        [Key]
        public byte idAdmPedido_Estado { get; set; }
        public string? denominacion { get; set; }

        [InverseProperty("idAdmPedido_EstadoNavigation")]
        public virtual ICollection<tblAdmPedidoCliente> tblAdmPedidoCliente { get; set; }
        [InverseProperty("idAdmPedido_EstadoNavigation")]
        public virtual ICollection<tblAdmPedidoProveedor> tblAdmPedidoProveedor { get; set; }
    }
}
