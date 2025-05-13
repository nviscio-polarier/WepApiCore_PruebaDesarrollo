using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblDefectoPrendaHuesped", Schema = "MyValet")]
    public partial class tblDefectoPrendaHuesped
    {
        public tblDefectoPrendaHuesped()
        {
            tblPrendaNPedidoHuesped = new HashSet<tblPrendaNPedidoHuesped>();
        }

        [Key]
        public byte idDefectoPrendaHuesped { get; set; }
        public string denominacion { get; set; } = null!;
        public int idLavanderia { get; set; }

        [ForeignKey("idLavanderia")]
        [InverseProperty("tblDefectoPrendaHuesped")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
        [InverseProperty("idDefectoPrendaHuespedNavigation")]
        public virtual ICollection<tblPrendaNPedidoHuesped> tblPrendaNPedidoHuesped { get; set; }
    }
}
