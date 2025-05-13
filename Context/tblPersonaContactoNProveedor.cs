using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPersonaContactoNProveedor", Schema = "Assistant")]
    public partial class tblPersonaContactoNProveedor
    {
        [Key]
        public short idPersonaContacto { get; set; }
        public short idProveedor { get; set; }
        public string nombre { get; set; } = null!;
        public string? apellidos { get; set; }
        public string telefono { get; set; } = null!;
        public string? telefono2 { get; set; }

        [ForeignKey("idProveedor")]
        [InverseProperty("tblPersonaContactoNProveedor")]
        public virtual tblProveedor idProveedorNavigation { get; set; } = null!;
    }
}
