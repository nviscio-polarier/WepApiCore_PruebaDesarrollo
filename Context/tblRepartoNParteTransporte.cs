using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblRepartoNParteTransporte", Schema = "Logistica")]
    public partial class tblRepartoNParteTransporte
    {
        [Key]
        public int idReparto { get; set; }
        [Key]
        public int idParteTransporte { get; set; }
        public byte numCarros { get; set; }
        public byte numCarrosRemontados { get; set; }

        [ForeignKey("idParteTransporte")]
        [InverseProperty("tblRepartoNParteTransporte")]
        public virtual tblParteTransporte idParteTransporteNavigation { get; set; } = null!;
        [ForeignKey("idReparto")]
        [InverseProperty("tblRepartoNParteTransporte")]
        public virtual tblReparto idRepartoNavigation { get; set; } = null!;
    }
}
