using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblProduccionMaquinaNCliente", Schema = "Produccion")]
    public partial class tblProduccionMaquinaNCliente
    {
        [Key]
        public int idProduccionMaquinaNCliente { get; set; }
        public int idMaquina { get; set; }
        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }
        public int? idEntidad { get; set; }
        public int? idCompañia { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime horaInicio { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime horaFin { get; set; }

        [ForeignKey("idCompañia")]
        [InverseProperty("tblProduccionMaquinaNCliente")]
        public virtual tblCompañia? idCompañiaNavigation { get; set; }
        [ForeignKey("idEntidad")]
        [InverseProperty("tblProduccionMaquinaNCliente")]
        public virtual tblEntidad? idEntidadNavigation { get; set; }
        [ForeignKey("idMaquina")]
        [InverseProperty("tblProduccionMaquinaNCliente")]
        public virtual tblMaquina idMaquinaNavigation { get; set; } = null!;
    }
}
