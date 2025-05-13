using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPrendasHora", Schema = "Maquinaria")]
    public partial class tblPrendasHora
    {
        [Key]
        public int idPrendasHora { get; set; }
        public int idMaquina { get; set; }
        public byte idFamilia { get; set; }
        public short? idTipoPrenda { get; set; }
        public short prendasHora { get; set; }
        public byte? numVias { get; set; }

        [ForeignKey("idFamilia")]
        [InverseProperty("tblPrendasHora")]
        public virtual tblFamilia idFamiliaNavigation { get; set; } = null!;
        [ForeignKey("idMaquina")]
        [InverseProperty("tblPrendasHora")]
        public virtual tblMaquina idMaquinaNavigation { get; set; } = null!;
        [ForeignKey("idTipoPrenda")]
        [InverseProperty("tblPrendasHora")]
        public virtual tblTipoPrenda? idTipoPrendaNavigation { get; set; }
    }
}
