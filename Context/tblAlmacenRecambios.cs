using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAlmacenRecambios", Schema = "Assistant")]
    public partial class tblAlmacenRecambios
    {
        public tblAlmacenRecambios()
        {
            InverseidAlmacenPadreNavigation = new HashSet<tblAlmacenRecambios>();
            tblAlmacenRecambiosNPersona = new HashSet<tblAlmacenRecambiosNPersona>();
            tblCierreRecambioNAlmacen = new HashSet<tblCierreRecambioNAlmacen>();
            tblMovimientoRecambioidAlmacenDestinoNavigation = new HashSet<tblMovimientoRecambio>();
            tblMovimientoRecambioidAlmacenOrigenNavigation = new HashSet<tblMovimientoRecambio>();
            tblRecambioNAlmacenRecambios = new HashSet<tblRecambioNAlmacenRecambios>();
            tblRecambioNParteTrabajo = new HashSet<tblRecambioNParteTrabajo>();
            tblStockMinimoNAlmacenRecambios = new HashSet<tblStockMinimoNAlmacenRecambios>();
        }

        [Key]
        public int idAlmacen { get; set; }
        public string denominacion { get; set; } = null!;
        public string? direccion { get; set; }
        public string? poblacion { get; set; }
        [StringLength(10)]
        public string? codigoPostal { get; set; }
        public string? telefono { get; set; }
        [StringLength(3)]
        public string centroCoste { get; set; } = null!;
        [Required]
        public bool? activo { get; set; }
        public bool eliminado { get; set; }
        public int? idAlmacenPadre { get; set; }
        public int? idPais { get; set; }
        public byte? idMoneda { get; set; }
        public byte? idTipoSalidaRecambio { get; set; }

        [ForeignKey("idAlmacenPadre")]
        [InverseProperty("InverseidAlmacenPadreNavigation")]
        public virtual tblAlmacenRecambios? idAlmacenPadreNavigation { get; set; }
        [ForeignKey("idMoneda")]
        [InverseProperty("tblAlmacenRecambios")]
        public virtual tblMoneda? idMonedaNavigation { get; set; }
        [ForeignKey("idPais")]
        [InverseProperty("tblAlmacenRecambios")]
        public virtual tblPais? idPaisNavigation { get; set; }
        [ForeignKey("idTipoSalidaRecambio")]
        [InverseProperty("tblAlmacenRecambios")]
        public virtual tblTipoSalidaRecambio? idTipoSalidaRecambioNavigation { get; set; }
        [InverseProperty("idAlmacenPadreNavigation")]
        public virtual ICollection<tblAlmacenRecambios> InverseidAlmacenPadreNavigation { get; set; }
        [InverseProperty("idAlmacenNavigation")]
        public virtual ICollection<tblAlmacenRecambiosNPersona> tblAlmacenRecambiosNPersona { get; set; }
        [InverseProperty("idAlmacenNavigation")]
        public virtual ICollection<tblCierreRecambioNAlmacen> tblCierreRecambioNAlmacen { get; set; }
        [InverseProperty("idAlmacenDestinoNavigation")]
        public virtual ICollection<tblMovimientoRecambio> tblMovimientoRecambioidAlmacenDestinoNavigation { get; set; }
        [InverseProperty("idAlmacenOrigenNavigation")]
        public virtual ICollection<tblMovimientoRecambio> tblMovimientoRecambioidAlmacenOrigenNavigation { get; set; }
        [InverseProperty("idAlmacenNavigation")]
        public virtual ICollection<tblRecambioNAlmacenRecambios> tblRecambioNAlmacenRecambios { get; set; }
        [InverseProperty("idAlmacenNavigation")]
        public virtual ICollection<tblRecambioNParteTrabajo> tblRecambioNParteTrabajo { get; set; }
        [InverseProperty("idAlmacenNavigation")]
        public virtual ICollection<tblStockMinimoNAlmacenRecambios> tblStockMinimoNAlmacenRecambios { get; set; }
    }
}
