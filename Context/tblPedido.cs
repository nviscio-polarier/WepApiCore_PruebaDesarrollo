using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPedido", Schema = "Logistica")]
    [Index("idEntidad", "idTipoProduccion", "fecha", Name = "IX_tblPedido_idEntidad_idTipoProduccion_fecha")]
    public partial class tblPedido
    {
        public tblPedido()
        {
            tblElemLogNPedido = new HashSet<tblElemLogNPedido>();
            tblPrendaNPedido = new HashSet<tblPrendaNPedido>();
            tblReparto = new HashSet<tblReparto>();
        }

        [Key]
        public int idPedido { get; set; }
        public int idEntidad { get; set; }
        public byte idEstadoPedido { get; set; }
        public byte idTipoPedido { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime fecha { get; set; }
        public string? observaciones { get; set; }
        [StringLength(8)]
        public string? codigo { get; set; }
        public int? idLavanderia { get; set; }
        [StringLength(5)]
        public string? idxPedido { get; set; }
        [Column(TypeName = "date")]
        public DateTime? fechaProduccion { get; set; }
        public byte? idTipoProduccion { get; set; }
        public DateTimeOffset? fechaRegistro { get; set; }
        public int? idUsuarioCreador { get; set; }
        public bool isCerrado { get; set; }
        [Column(TypeName = "decimal(3, 2)")]
        public decimal porcentaje { get; set; }
        public bool isApp { get; set; }
        public int? idUsuarioPrepara_activo { get; set; }
        public byte? idTipoPedidoEntidad { get; set; }
        public bool isAutomatico { get; set; }

        [ForeignKey("idEntidad")]
        [InverseProperty("tblPedido")]
        public virtual tblEntidad idEntidadNavigation { get; set; } = null!;
        [ForeignKey("idEstadoPedido")]
        [InverseProperty("tblPedido")]
        public virtual tblEstadoPedido idEstadoPedidoNavigation { get; set; } = null!;
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblPedido")]
        public virtual tblLavanderia? idLavanderiaNavigation { get; set; }
        [ForeignKey("idTipoPedido")]
        [InverseProperty("tblPedido")]
        public virtual tblTipoPedido idTipoPedidoNavigation { get; set; } = null!;
        [ForeignKey("idTipoProduccion")]
        [InverseProperty("tblPedido")]
        public virtual tblTipoProduccion? idTipoProduccionNavigation { get; set; }
        [ForeignKey("idUsuarioCreador")]
        [InverseProperty("tblPedidoidUsuarioCreadorNavigation")]
        public virtual tblUsuario? idUsuarioCreadorNavigation { get; set; }
        [ForeignKey("idUsuarioPrepara_activo")]
        [InverseProperty("tblPedidoidUsuarioPrepara_activoNavigation")]
        public virtual tblUsuario? idUsuarioPrepara_activoNavigation { get; set; }
        [InverseProperty("idPedidoNavigation")]
        public virtual ICollection<tblElemLogNPedido> tblElemLogNPedido { get; set; }
        [InverseProperty("idPedidoNavigation")]
        public virtual ICollection<tblPrendaNPedido> tblPrendaNPedido { get; set; }
        [InverseProperty("idPedidoNavigation")]
        public virtual ICollection<tblReparto> tblReparto { get; set; }
    }
}
