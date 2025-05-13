using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoPrenda", Schema = "General")]
    public partial class tblTipoPrenda
    {
        public tblTipoPrenda()
        {
            tblClienteNMaquina = new HashSet<tblClienteNMaquina>();
            tblDenoPrenda = new HashSet<tblDenoPrenda>();
            tblPrendaNMaquina = new HashSet<tblPrendaNMaquina>();
            tblPrendasHora = new HashSet<tblPrendasHora>();
        }

        [Key]
        public short idTipoPrenda { get; set; }
        [StringLength(1)]
        public string codigo { get; set; } = null!;
        [StringLength(50)]
        public string denominacion { get; set; } = null!;
        public byte idFamilia { get; set; }
        public int idTraduccion { get; set; }

        [ForeignKey("idFamilia")]
        [InverseProperty("tblTipoPrenda")]
        public virtual tblFamilia idFamiliaNavigation { get; set; } = null!;
        [ForeignKey("idTraduccion")]
        [InverseProperty("tblTipoPrenda")]
        public virtual tblTraduccion idTraduccionNavigation { get; set; } = null!;
        [InverseProperty("idTipoPrendaNavigation")]
        public virtual ICollection<tblClienteNMaquina> tblClienteNMaquina { get; set; }
        [InverseProperty("idTipoPrendaNavigation")]
        public virtual ICollection<tblDenoPrenda> tblDenoPrenda { get; set; }
        [InverseProperty("idTipoPrendaNavigation")]
        public virtual ICollection<tblPrendaNMaquina> tblPrendaNMaquina { get; set; }
        [InverseProperty("idTipoPrendaNavigation")]
        public virtual ICollection<tblPrendasHora> tblPrendasHora { get; set; }
    }
}
