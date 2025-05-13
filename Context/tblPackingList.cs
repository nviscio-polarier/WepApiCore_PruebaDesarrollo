using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPackingList", Schema = "Logistica")]
    public partial class tblPackingList
    {
        public tblPackingList()
        {
            tblArticuloEnvio = new HashSet<tblArticuloEnvio>();
        }

        [Key]
        public int idPackingList { get; set; }
        public int idEnvio { get; set; }
        [StringLength(50)]
        public string? personaRecibido { get; set; }
        [Column(TypeName = "date")]
        public DateTime? fecha { get; set; }

        [ForeignKey("idEnvio")]
        [InverseProperty("tblPackingList")]
        public virtual tblEnvio idEnvioNavigation { get; set; } = null!;
        [InverseProperty("idPackingListNavigation")]
        public virtual ICollection<tblArticuloEnvio> tblArticuloEnvio { get; set; }
    }
}
