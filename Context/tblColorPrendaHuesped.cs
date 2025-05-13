using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblColorPrendaHuesped", Schema = "MyValet")]
    public partial class tblColorPrendaHuesped
    {
        public tblColorPrendaHuesped()
        {
            tblPrendaNPedidoHuesped = new HashSet<tblPrendaNPedidoHuesped>();
        }

        [Key]
        public byte idColorPrendaHuesped { get; set; }
        public string denominacion { get; set; } = null!;
        public int idLavanderia { get; set; }

        [ForeignKey("idLavanderia")]
        [InverseProperty("tblColorPrendaHuesped")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
        [InverseProperty("idColorPrendaHuespedNavigation")]
        public virtual ICollection<tblPrendaNPedidoHuesped> tblPrendaNPedidoHuesped { get; set; }
    }
}
