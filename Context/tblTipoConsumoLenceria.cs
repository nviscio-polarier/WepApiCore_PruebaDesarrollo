using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoConsumoLenceria", Schema = "General")]
    public partial class tblTipoConsumoLenceria
    {
        public tblTipoConsumoLenceria()
        {
            tblCierreDatos_Facturacion = new HashSet<tblCierreDatos_Facturacion>();
            tblEntidad = new HashSet<tblEntidad>();
        }

        [StringLength(50)]
        public string denominacion { get; set; } = null!;
        public byte? codigo { get; set; }
        [Key]
        public byte idTipoConsumoLenceria { get; set; }

        [InverseProperty("idTipoConsumoLenceriaNavigation")]
        public virtual ICollection<tblCierreDatos_Facturacion> tblCierreDatos_Facturacion { get; set; }
        [InverseProperty("idTipoConsumoLenceriaNavigation")]
        public virtual ICollection<tblEntidad> tblEntidad { get; set; }
    }
}
