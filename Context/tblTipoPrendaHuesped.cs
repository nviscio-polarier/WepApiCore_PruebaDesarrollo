using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoPrendaHuesped", Schema = "MyValet")]
    public partial class tblTipoPrendaHuesped
    {
        public tblTipoPrendaHuesped()
        {
            tblPrendaNPedidoHuesped = new HashSet<tblPrendaNPedidoHuesped>();
        }

        [Key]
        public byte idTipoPrendaHuesped { get; set; }
        public string denominacion { get; set; } = null!;
        public int idLavanderia { get; set; }

        [ForeignKey("idLavanderia")]
        [InverseProperty("tblTipoPrendaHuesped")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
        [InverseProperty("idTipoPrendaHuespedNavigation")]
        public virtual ICollection<tblPrendaNPedidoHuesped> tblPrendaNPedidoHuesped { get; set; }
    }
}
