using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoServicio", Schema = "MyValet")]
    public partial class tblTipoServicio
    {
        public tblTipoServicio()
        {
            tblPedidoHuesped = new HashSet<tblPedidoHuesped>();
        }

        [Key]
        public byte idTipoServicio { get; set; }
        public string denominacion { get; set; } = null!;
        public int idLavanderia { get; set; }

        [ForeignKey("idLavanderia")]
        [InverseProperty("tblTipoServicio")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
        [InverseProperty("idTipoServicioNavigation")]
        public virtual ICollection<tblPedidoHuesped> tblPedidoHuesped { get; set; }
    }
}
