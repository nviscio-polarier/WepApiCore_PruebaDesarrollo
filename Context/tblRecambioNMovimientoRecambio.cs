using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblRecambioNMovimientoRecambio", Schema = "Assistant")]
    public partial class tblRecambioNMovimientoRecambio
    {
        public tblRecambioNMovimientoRecambio()
        {
            InverseidRecambioNMovimientoRecambioAsociadoNavigation = new HashSet<tblRecambioNMovimientoRecambio>();
        }

        [Key]
        public int idRecambioNMovimientoRecambio { get; set; }
        public int? idRecambioNMovimientoRecambioAsociado { get; set; }
        public int idRecambio { get; set; }
        public int idMovimientoRecambio { get; set; }
        public int? cantidad { get; set; }
        [StringLength(50)]
        public string? ubicacion { get; set; }
        [StringLength(50)]
        public string? referenciaProveedor { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal? precio { get; set; }
        public int? cantidadTeorico { get; set; }
        public bool isApp { get; set; }
        public DateTimeOffset? fecha { get; set; }
        public int? idUsuario { get; set; }

        [ForeignKey("idMovimientoRecambio")]
        [InverseProperty("tblRecambioNMovimientoRecambio")]
        public virtual tblMovimientoRecambio idMovimientoRecambioNavigation { get; set; } = null!;
        [ForeignKey("idRecambioNMovimientoRecambioAsociado")]
        [InverseProperty("InverseidRecambioNMovimientoRecambioAsociadoNavigation")]
        public virtual tblRecambioNMovimientoRecambio? idRecambioNMovimientoRecambioAsociadoNavigation { get; set; }
        [ForeignKey("idRecambio")]
        [InverseProperty("tblRecambioNMovimientoRecambio")]
        public virtual tblRecambio idRecambioNavigation { get; set; } = null!;
        [ForeignKey("idUsuario")]
        [InverseProperty("tblRecambioNMovimientoRecambio")]
        public virtual tblUsuario? idUsuarioNavigation { get; set; }
        [InverseProperty("idRecambioNMovimientoRecambioAsociadoNavigation")]
        public virtual ICollection<tblRecambioNMovimientoRecambio> InverseidRecambioNMovimientoRecambioAsociadoNavigation { get; set; }
    }
}
