using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPesoNSistemaNLavanderia", Schema = "Maquinaria")]
    public partial class tblPesoNSistemaNLavanderia
    {
        [Key]
        public int idSistemaMaquina { get; set; }
        [Key]
        public int idLavanderia { get; set; }
        [Column(TypeName = "numeric(3, 0)")]
        public decimal? peso { get; set; }

        [ForeignKey("idLavanderia")]
        [InverseProperty("tblPesoNSistemaNLavanderia")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
        [ForeignKey("idSistemaMaquina")]
        [InverseProperty("tblPesoNSistemaNLavanderia")]
        public virtual tblSistemaMaquina idSistemaMaquinaNavigation { get; set; } = null!;
    }
}
