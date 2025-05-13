using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblProveedor", Schema = "Assistant")]
    public partial class tblProveedor
    {
        public tblProveedor()
        {
            tblMovimientoRecambio = new HashSet<tblMovimientoRecambio>();
            tblPersonaContactoNProveedor = new HashSet<tblPersonaContactoNProveedor>();
            tblRecambio = new HashSet<tblRecambio>();
            tblRecambioNProveedor = new HashSet<tblRecambioNProveedor>();
        }

        [Key]
        public short idProveedor { get; set; }
        public string nombreComercial { get; set; } = null!;
        public string? nombreFiscal { get; set; }
        public string? CIF { get; set; }
        public string? direccion { get; set; }
        public string? codigoPostal { get; set; }
        public string? poblacion { get; set; }
        public string? telefono { get; set; }
        public string? telefono2 { get; set; }
        public string? email { get; set; }
        public int idPais { get; set; }
        public string? paginaWeb { get; set; }
        [Required]
        public bool? activo { get; set; }
        public bool eliminado { get; set; }

        [ForeignKey("idPais")]
        [InverseProperty("tblProveedor")]
        public virtual tblPais idPaisNavigation { get; set; } = null!;
        [InverseProperty("idProveedorNavigation")]
        public virtual ICollection<tblMovimientoRecambio> tblMovimientoRecambio { get; set; }
        [InverseProperty("idProveedorNavigation")]
        public virtual ICollection<tblPersonaContactoNProveedor> tblPersonaContactoNProveedor { get; set; }
        [InverseProperty("idProveedorNavigation")]
        public virtual ICollection<tblRecambio> tblRecambio { get; set; }
        [InverseProperty("idProveedorNavigation")]
        public virtual ICollection<tblRecambioNProveedor> tblRecambioNProveedor { get; set; }
    }
}
