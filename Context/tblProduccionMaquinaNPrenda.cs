using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblProduccionMaquinaNPrenda", Schema = "Produccion")]
    public partial class tblProduccionMaquinaNPrenda
    {
        [Key]
        public int idPrenda { get; set; }
        [Key]
        public int idMaquina { get; set; }
        public int piezasHora { get; set; }
        public short numPersonas { get; set; }

        [ForeignKey("idMaquina")]
        [InverseProperty("tblProduccionMaquinaNPrenda")]
        public virtual tblMaquina idMaquinaNavigation { get; set; } = null!;
        [ForeignKey("idPrenda")]
        [InverseProperty("tblProduccionMaquinaNPrenda")]
        public virtual tblPrenda idPrendaNavigation { get; set; } = null!;
    }
}
