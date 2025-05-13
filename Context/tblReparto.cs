using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblReparto", Schema = "Logistica")]
    [Index("idLavanderia", "fecha", Name = "IX_tblReparto_idLavanderia_fecha")]
    [Index("idPedido", Name = "IX_tblReparto_idPedido")]
    [Index("idRepartoEstado", "fecha", Name = "IX_tblReparto_idRepartoEstado_fecha")]
    [Index("idRepartoEstado", "idEntidad", Name = "IX_tblReparto_idRepartoEstado_idEntidad")]
    public partial class tblReparto
    {
        public tblReparto()
        {
            tblPrendaNReparto = new HashSet<tblPrendaNReparto>();
            tblRepartoNParteTransporte = new HashSet<tblRepartoNParteTransporte>();
            tblRutaNParteTransporte = new HashSet<tblRutaNParteTransporte>();
            idSalidaReparto = new HashSet<tblSalidaReparto>();
        }

        [Key]
        public int idReparto { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? fecha { get; set; }
        public int? idPedido { get; set; }
        public byte idRepartoEstado { get; set; }
        [Column(TypeName = "date")]
        public DateTime? fechaProduccion { get; set; }
        public int? idLavanderia { get; set; }
        public int? idEntidad { get; set; }
        [StringLength(8)]
        public string? codigo { get; set; }
        public string? observaciones { get; set; }
        [StringLength(5)]
        public string? idxReparto { get; set; }
        public byte? idTipoProduccion { get; set; }
        public int? idPedidoCliente { get; set; }
        public int? idUsuarioPrepara { get; set; }
        public byte? numCarros { get; set; }
        public bool isApp { get; set; }
        public int? idMovimientoElemLog { get; set; }

        [ForeignKey("idEntidad")]
        [InverseProperty("tblReparto")]
        public virtual tblEntidad? idEntidadNavigation { get; set; }
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblReparto")]
        public virtual tblLavanderia? idLavanderiaNavigation { get; set; }
        [ForeignKey("idMovimientoElemLog")]
        [InverseProperty("tblReparto")]
        public virtual tblMovimientoElemLog? idMovimientoElemLogNavigation { get; set; }
        [ForeignKey("idPedido")]
        [InverseProperty("tblReparto")]
        public virtual tblPedido? idPedidoNavigation { get; set; }
        [ForeignKey("idRepartoEstado")]
        [InverseProperty("tblReparto")]
        public virtual tblRepartoEstado idRepartoEstadoNavigation { get; set; } = null!;
        [ForeignKey("idTipoProduccion")]
        [InverseProperty("tblReparto")]
        public virtual tblTipoProduccion? idTipoProduccionNavigation { get; set; }
        [ForeignKey("idUsuarioPrepara")]
        [InverseProperty("tblReparto")]
        public virtual tblUsuario? idUsuarioPreparaNavigation { get; set; }
        [InverseProperty("idRepartoNavigation")]
        public virtual ICollection<tblPrendaNReparto> tblPrendaNReparto { get; set; }
        [InverseProperty("idRepartoNavigation")]
        public virtual ICollection<tblRepartoNParteTransporte> tblRepartoNParteTransporte { get; set; }
        [InverseProperty("idRepartoNavigation")]
        public virtual ICollection<tblRutaNParteTransporte> tblRutaNParteTransporte { get; set; }

        [ForeignKey("idReparto")]
        [InverseProperty("idReparto")]
        public virtual ICollection<tblSalidaReparto> idSalidaReparto { get; set; }
    }
}
