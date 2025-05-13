using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblArticuloEnvio", Schema = "Logistica")]
    public partial class tblArticuloEnvio
    {
        public tblArticuloEnvio()
        {
            tblFotoNArticuloEnvio = new HashSet<tblFotoNArticuloEnvio>();
        }

        [Key]
        public int idArticuloEnvio { get; set; }
        [StringLength(50)]
        public string descripcion { get; set; } = null!;
        public int? pesoNeto { get; set; }
        public int? pesoBruto { get; set; }
        [StringLength(50)]
        public string? dimensiones { get; set; }
        public int idPackingList { get; set; }
        public bool isEnDestino { get; set; }
        public string? observaciones { get; set; }

        [ForeignKey("idPackingList")]
        [InverseProperty("tblArticuloEnvio")]
        public virtual tblPackingList idPackingListNavigation { get; set; } = null!;
        [InverseProperty("idArticuloEnvioNavigation")]
        public virtual ICollection<tblFotoNArticuloEnvio> tblFotoNArticuloEnvio { get; set; }
    }
}
