using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblEntidadNInventario", Schema = "Inventarios")]
    public partial class tblEntidadNInventario
    {
        [Key]
        public int idInventario { get; set; }
        [Key]
        public int idEntidad { get; set; }
        public int? idArchivo_firmaPolarier { get; set; }
        public int? idArchivo_firmaCliente { get; set; }
        public string? firmantePolarier { get; set; }
        public string? firmanteCliente { get; set; }
        public string? correoCliente { get; set; }
        public DateTimeOffset? fechaRegistro { get; set; }

        [ForeignKey("idArchivo_firmaCliente")]
        [InverseProperty("tblEntidadNInventarioidArchivo_firmaClienteNavigation")]
        public virtual tblArchivo? idArchivo_firmaClienteNavigation { get; set; }
        [ForeignKey("idArchivo_firmaPolarier")]
        [InverseProperty("tblEntidadNInventarioidArchivo_firmaPolarierNavigation")]
        public virtual tblArchivo? idArchivo_firmaPolarierNavigation { get; set; }
        [ForeignKey("idEntidad")]
        [InverseProperty("tblEntidadNInventario")]
        public virtual tblEntidad idEntidadNavigation { get; set; } = null!;
        [ForeignKey("idInventario")]
        [InverseProperty("tblEntidadNInventario")]
        public virtual tblInventario idInventarioNavigation { get; set; } = null!;
    }
}
