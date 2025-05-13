using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAreaLavanderia", Schema = "General")]
    public partial class tblAreaLavanderia
    {
        public tblAreaLavanderia()
        {
            tblAreaLavanderiaNLavanderia = new HashSet<tblAreaLavanderiaNLavanderia>();
            tblGestionRetiro = new HashSet<tblGestionRetiro>();
            tblLayout_SmartView = new HashSet<tblLayout_SmartView>();
            tblPersonaNAreaNLavanderia = new HashSet<tblPersonaNAreaNLavanderia>();
            tblPosicionNAreaLavanderiaNLavanderia = new HashSet<tblPosicionNAreaLavanderiaNLavanderia>();
        }

        [Key]
        public byte idAreaLavanderia { get; set; }
        public string denominacion { get; set; } = null!;
        public int idTraduccion { get; set; }

        [ForeignKey("idTraduccion")]
        [InverseProperty("tblAreaLavanderia")]
        public virtual tblTraduccion idTraduccionNavigation { get; set; } = null!;
        [InverseProperty("idAreaLavanderiaNavigation")]
        public virtual ICollection<tblAreaLavanderiaNLavanderia> tblAreaLavanderiaNLavanderia { get; set; }
        [InverseProperty("idAreaLavanderiaNavigation")]
        public virtual ICollection<tblGestionRetiro> tblGestionRetiro { get; set; }
        [InverseProperty("idAreaLavanderiaNavigation")]
        public virtual ICollection<tblLayout_SmartView> tblLayout_SmartView { get; set; }
        [InverseProperty("idAreaLavanderiaNavigation")]
        public virtual ICollection<tblPersonaNAreaNLavanderia> tblPersonaNAreaNLavanderia { get; set; }
        [InverseProperty("idAreaLavanderiaNavigation")]
        public virtual ICollection<tblPosicionNAreaLavanderiaNLavanderia> tblPosicionNAreaLavanderiaNLavanderia { get; set; }
    }
}
