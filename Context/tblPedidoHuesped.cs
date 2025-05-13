using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPedidoHuesped", Schema = "MyValet")]
    public partial class tblPedidoHuesped
    {
        public tblPedidoHuesped()
        {
            tblPrendaNPedidoHuesped = new HashSet<tblPrendaNPedidoHuesped>();
        }

        [Key]
        public int idPedidoHuesped { get; set; }
        public int? idAlmacen { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? fechaLlamada { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? fechaRecepcion { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? fechaEntrega { get; set; }
        public string? observaciones { get; set; }
        public byte? idTipoServicio { get; set; }
        public string? personaSolicita { get; set; }
        public int? idPesonaRecibeLLamada { get; set; }
        public int? idPersonaRecibeRecepcion { get; set; }

        [ForeignKey("idAlmacen")]
        [InverseProperty("tblPedidoHuesped")]
        public virtual tblAlmacen? idAlmacenNavigation { get; set; }
        [ForeignKey("idTipoServicio")]
        [InverseProperty("tblPedidoHuesped")]
        public virtual tblTipoServicio? idTipoServicioNavigation { get; set; }
        [InverseProperty("idPedidoHuespedNavigation")]
        public virtual ICollection<tblPrendaNPedidoHuesped> tblPrendaNPedidoHuesped { get; set; }
    }
}
