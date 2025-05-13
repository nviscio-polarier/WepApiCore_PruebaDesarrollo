using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoProduccion", Schema = "Produccion")]
    public partial class tblTipoProduccion
    {
        public tblTipoProduccion()
        {
            tblPedido = new HashSet<tblPedido>();
            tblProduccion = new HashSet<tblProduccion>();
            tblReparto = new HashSet<tblReparto>();
            idLavanderia = new HashSet<tblLavanderia>();
        }

        [Key]
        public byte idTipoProduccion { get; set; }
        [StringLength(50)]
        public string denominacion { get; set; } = null!;
        public byte? codigo { get; set; }
        [StringLength(5)]
        public string? siglas { get; set; }

        [InverseProperty("idTipoProduccionNavigation")]
        public virtual ICollection<tblPedido> tblPedido { get; set; }
        [InverseProperty("idTipoProduccionNavigation")]
        public virtual ICollection<tblProduccion> tblProduccion { get; set; }
        [InverseProperty("idTipoProduccionNavigation")]
        public virtual ICollection<tblReparto> tblReparto { get; set; }

        [ForeignKey("idTipoProduccion")]
        [InverseProperty("idTipoProduccion")]
        public virtual ICollection<tblLavanderia> idLavanderia { get; set; }
    }
}
