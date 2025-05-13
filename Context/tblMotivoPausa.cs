using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblMotivoPausa", Schema = "Logistica")]
    public partial class tblMotivoPausa
    {
        public tblMotivoPausa()
        {
            tblParadaNParteTransporte = new HashSet<tblParadaNParteTransporte>();
        }

        [Key]
        public int idMotivoPausa { get; set; }
        public string denominacion { get; set; } = null!;

        [InverseProperty("idMotivoPausaNavigation")]
        public virtual ICollection<tblParadaNParteTransporte> tblParadaNParteTransporte { get; set; }
    }
}
