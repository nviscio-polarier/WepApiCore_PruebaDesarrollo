using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblFamilia", Schema = "General")]
    public partial class tblFamilia
    {
        public tblFamilia()
        {
            tblBacsCarro = new HashSet<tblBacsCarro>();
            tblClienteNMaquina = new HashSet<tblClienteNMaquina>();
            tblGestionRetiro = new HashSet<tblGestionRetiro>();
            tblPrenda = new HashSet<tblPrenda>();
            tblPrendaNMaquina = new HashSet<tblPrendaNMaquina>();
            tblPrendasHora = new HashSet<tblPrendasHora>();
            tblTipoPrenda = new HashSet<tblTipoPrenda>();
        }

        [StringLength(1)]
        public string codigo { get; set; } = null!;
        [StringLength(50)]
        public string denominacion { get; set; } = null!;
        [StringLength(50)]
        public string? abreviatura { get; set; }
        public int idTraduccion { get; set; }
        public int? idTraduccion_abr { get; set; }
        [Key]
        public byte idFamilia { get; set; }

        [ForeignKey("idTraduccion")]
        [InverseProperty("tblFamiliaidTraduccionNavigation")]
        public virtual tblTraduccion idTraduccionNavigation { get; set; } = null!;
        [ForeignKey("idTraduccion_abr")]
        [InverseProperty("tblFamiliaidTraduccion_abrNavigation")]
        public virtual tblTraduccion? idTraduccion_abrNavigation { get; set; }
        [InverseProperty("idFamiliaNavigation")]
        public virtual ICollection<tblBacsCarro> tblBacsCarro { get; set; }
        [InverseProperty("idFamiliaNavigation")]
        public virtual ICollection<tblClienteNMaquina> tblClienteNMaquina { get; set; }
        [InverseProperty("idFamiliaNavigation")]
        public virtual ICollection<tblGestionRetiro> tblGestionRetiro { get; set; }
        [InverseProperty("idFamiliaFacturacionNavigation")]
        public virtual ICollection<tblPrenda> tblPrenda { get; set; }
        [InverseProperty("idFamiliaNavigation")]
        public virtual ICollection<tblPrendaNMaquina> tblPrendaNMaquina { get; set; }
        [InverseProperty("idFamiliaNavigation")]
        public virtual ICollection<tblPrendasHora> tblPrendasHora { get; set; }
        [InverseProperty("idFamiliaNavigation")]
        public virtual ICollection<tblTipoPrenda> tblTipoPrenda { get; set; }
    }
}
