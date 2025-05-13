using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPrendaNPedidoHuesped", Schema = "MyValet")]
    public partial class tblPrendaNPedidoHuesped
    {
        [Key]
        public int idPrendaNPedidoHuesped { get; set; }
        public int idPedidoHuesped { get; set; }
        public int idPrendaHuesped { get; set; }
        public byte? idTipoLavado { get; set; }
        public byte idColorPrendaHuesped { get; set; }
        public byte? idDefectoPrendaHuesped { get; set; }
        public string marca { get; set; } = null!;
        public byte idTipoPrendaHuesped { get; set; }
        public bool procesado { get; set; }

        [ForeignKey("idColorPrendaHuesped")]
        [InverseProperty("tblPrendaNPedidoHuesped")]
        public virtual tblColorPrendaHuesped idColorPrendaHuespedNavigation { get; set; } = null!;
        [ForeignKey("idDefectoPrendaHuesped")]
        [InverseProperty("tblPrendaNPedidoHuesped")]
        public virtual tblDefectoPrendaHuesped? idDefectoPrendaHuespedNavigation { get; set; }
        [ForeignKey("idPedidoHuesped")]
        [InverseProperty("tblPrendaNPedidoHuesped")]
        public virtual tblPedidoHuesped idPedidoHuespedNavigation { get; set; } = null!;
        [ForeignKey("idPrendaHuesped")]
        [InverseProperty("tblPrendaNPedidoHuesped")]
        public virtual tblPrendaHuesped idPrendaHuespedNavigation { get; set; } = null!;
        [ForeignKey("idTipoLavado")]
        [InverseProperty("tblPrendaNPedidoHuesped")]
        public virtual tblTipoLavado? idTipoLavadoNavigation { get; set; }
        [ForeignKey("idTipoPrendaHuesped")]
        [InverseProperty("tblPrendaNPedidoHuesped")]
        public virtual tblTipoPrendaHuesped idTipoPrendaHuespedNavigation { get; set; } = null!;
    }
}
