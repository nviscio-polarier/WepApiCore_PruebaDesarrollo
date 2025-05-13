using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCierreDatos_Facturacion", Schema = "General")]
    public partial class tblCierreDatos_Facturacion
    {
        [Key]
        public int idEntidad { get; set; }
        [Key]
        public short año { get; set; }
        [Key]
        public byte mes { get; set; }
        [Column(TypeName = "date")]
        public DateTime fechaDesde { get; set; }
        [Column(TypeName = "date")]
        public DateTime fechaHasta { get; set; }
        public bool isCerrado { get; set; }
        public byte idTipoConsumoLenceria { get; set; }
        public byte idTipoFacturacionCliente { get; set; }
        [Column(TypeName = "decimal(8, 3)")]
        public decimal costeEstancia { get; set; }
        [Column(TypeName = "decimal(8, 3)")]
        public decimal objKgEstancia { get; set; }

        [ForeignKey("idEntidad")]
        [InverseProperty("tblCierreDatos_Facturacion")]
        public virtual tblEntidad idEntidadNavigation { get; set; } = null!;
        [ForeignKey("idTipoConsumoLenceria")]
        [InverseProperty("tblCierreDatos_Facturacion")]
        public virtual tblTipoConsumoLenceria idTipoConsumoLenceriaNavigation { get; set; } = null!;
        [ForeignKey("idTipoFacturacionCliente")]
        [InverseProperty("tblCierreDatos_Facturacion")]
        public virtual tblTipoFacturacionCliente idTipoFacturacionClienteNavigation { get; set; } = null!;
    }
}
