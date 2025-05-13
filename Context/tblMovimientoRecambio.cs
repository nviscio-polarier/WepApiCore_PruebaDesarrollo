using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblMovimientoRecambio", Schema = "Assistant")]
    public partial class tblMovimientoRecambio
    {
        public tblMovimientoRecambio()
        {
            tblEstadoMovimientoRecambioNMovimientoRecambio = new HashSet<tblEstadoMovimientoRecambioNMovimientoRecambio>();
            tblMantenimientoPrev = new HashSet<tblMantenimientoPrev>();
            tblRecambioNMovimientoRecambio = new HashSet<tblRecambioNMovimientoRecambio>();
        }

        [Key]
        public int idMovimientoRecambio { get; set; }
        public int? idAlmacenOrigen { get; set; }
        public short? idProveedor { get; set; }
        public int? idAlmacenDestino { get; set; }
        [StringLength(100)]
        public string? clienteDestino { get; set; }
        [Precision(0)]
        public DateTimeOffset fecha { get; set; }
        public int idTipoMovimientoRecambio { get; set; }
        public bool isInventario { get; set; }
        public string? observaciones { get; set; }
        public string? numPedidoAsociado { get; set; }
        public string? codigoAlbaranProveedor { get; set; }
        public string? numRegistro { get; set; }
        public bool? isValidado { get; set; }
        public byte? idEstadoMovimientoRecambio { get; set; }

        [ForeignKey("idAlmacenDestino")]
        [InverseProperty("tblMovimientoRecambioidAlmacenDestinoNavigation")]
        public virtual tblAlmacenRecambios? idAlmacenDestinoNavigation { get; set; }
        [ForeignKey("idAlmacenOrigen")]
        [InverseProperty("tblMovimientoRecambioidAlmacenOrigenNavigation")]
        public virtual tblAlmacenRecambios? idAlmacenOrigenNavigation { get; set; }
        [ForeignKey("idEstadoMovimientoRecambio")]
        [InverseProperty("tblMovimientoRecambio")]
        public virtual tblEstadoMovimientoRecambio? idEstadoMovimientoRecambioNavigation { get; set; }
        [ForeignKey("idProveedor")]
        [InverseProperty("tblMovimientoRecambio")]
        public virtual tblProveedor? idProveedorNavigation { get; set; }
        [ForeignKey("idTipoMovimientoRecambio")]
        [InverseProperty("tblMovimientoRecambio")]
        public virtual tblTipoMovimientoRecambio idTipoMovimientoRecambioNavigation { get; set; } = null!;
        [InverseProperty("idMovimientoRecambioNavigation")]
        public virtual ICollection<tblEstadoMovimientoRecambioNMovimientoRecambio> tblEstadoMovimientoRecambioNMovimientoRecambio { get; set; }
        [InverseProperty("idMovimientoRecambioNavigation")]
        public virtual ICollection<tblMantenimientoPrev> tblMantenimientoPrev { get; set; }
        [InverseProperty("idMovimientoRecambioNavigation")]
        public virtual ICollection<tblRecambioNMovimientoRecambio> tblRecambioNMovimientoRecambio { get; set; }
    }
}
