using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblModeloImpresion_reparto", Schema = "MyReporting")]
    public partial class tblModeloImpresion_reparto
    {
        public tblModeloImpresion_reparto()
        {
            tblEntidad = new HashSet<tblEntidad>();
        }

        [Key]
        public byte idModeloImpresion_reparto { get; set; }
        public string report { get; set; } = null!;
        public string denominacion { get; set; } = null!;
        public string? descripcion { get; set; }

        [InverseProperty("idModeloImpresion_repartoNavigation")]
        public virtual ICollection<tblEntidad> tblEntidad { get; set; }
    }
}
