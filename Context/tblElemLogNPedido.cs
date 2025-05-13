using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblElemLogNPedido", Schema = "Logistica")]
    public partial class tblElemLogNPedido
    {
        [Key]
        public byte idTipoElemLog { get; set; }
        [Key]
        public int idPedido { get; set; }
        public short stock { get; set; }

        [ForeignKey("idPedido")]
        [InverseProperty("tblElemLogNPedido")]
        public virtual tblPedido idPedidoNavigation { get; set; } = null!;
        [ForeignKey("idTipoElemLog")]
        [InverseProperty("tblElemLogNPedido")]
        public virtual tblTipoElemLog idTipoElemLogNavigation { get; set; } = null!;
    }
}
