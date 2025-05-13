using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoFacturacionCliente", Schema = "General")]
    public partial class tblTipoFacturacionCliente
    {
        public tblTipoFacturacionCliente()
        {
            tblCierreDatos_Facturacion = new HashSet<tblCierreDatos_Facturacion>();
            tblEntidad = new HashSet<tblEntidad>();
        }

        [Key]
        public byte idTipoFacturacionCliente { get; set; }
        public string denominacion { get; set; } = null!;
        public string? icon { get; set; }

        [InverseProperty("idTipoFacturacionClienteNavigation")]
        public virtual ICollection<tblCierreDatos_Facturacion> tblCierreDatos_Facturacion { get; set; }
        [InverseProperty("idTipoFacturacionClienteNavigation")]
        public virtual ICollection<tblEntidad> tblEntidad { get; set; }
    }
}
