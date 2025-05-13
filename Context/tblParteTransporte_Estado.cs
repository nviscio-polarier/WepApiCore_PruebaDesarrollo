using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblParteTransporte_Estado", Schema = "Logistica")]
    public partial class tblParteTransporte_Estado
    {
        public tblParteTransporte_Estado()
        {
            tblParteTransporte = new HashSet<tblParteTransporte>();
        }

        [Key]
        public byte idEstado { get; set; }
        public string denominacion { get; set; } = null!;

        [InverseProperty("idEstadoNavigation")]
        public virtual ICollection<tblParteTransporte> tblParteTransporte { get; set; }
    }
}
