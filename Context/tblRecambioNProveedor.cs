using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblRecambioNProveedor", Schema = "Assistant")]
    public partial class tblRecambioNProveedor
    {
        [Key]
        public int idRecambioNProveedor { get; set; }
        public int idRecambio { get; set; }
        public short idProveedor { get; set; }
        public string? referencia { get; set; }
        [Required]
        public bool? activo { get; set; }
        [StringLength(50)]
        public string? codigoBarras { get; set; }
        [StringLength(50)]
        public string? fabricante { get; set; }
        [StringLength(50)]
        public string? refFabricante { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal? ultimoPrecio { get; set; }
        public int? idPais { get; set; }
        public string? codigoFabricante { get; set; }
        public string? codigoBarrasFabricante { get; set; }
        public string? observaciones { get; set; }
        public byte? orden { get; set; }

        [ForeignKey("idPais")]
        [InverseProperty("tblRecambioNProveedor")]
        public virtual tblPais? idPaisNavigation { get; set; }
        [ForeignKey("idProveedor")]
        [InverseProperty("tblRecambioNProveedor")]
        public virtual tblProveedor idProveedorNavigation { get; set; } = null!;
        [ForeignKey("idRecambio")]
        [InverseProperty("tblRecambioNProveedor")]
        public virtual tblRecambio idRecambioNavigation { get; set; } = null!;
    }
}
