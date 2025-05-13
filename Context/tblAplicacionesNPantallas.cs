using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCore.Context
{
    public partial class tblAplicacionesNPantallas
    {
        public tblAplicacionesNPantallas()
        {
            tblConfigMYUNIF = new HashSet<tblConfigMYUNIF>();
            tblConfigPTMYOF = new HashSet<tblConfigPTMYOF>();
            tblConfigPTMYPR = new HashSet<tblConfigPTMYPR>();
            tblConfigTabletMyAudit = new HashSet<tblConfigTabletMyAudit>();
            tblConfigTabletMyInventory = new HashSet<tblConfigTabletMyInventory>();
            tblConfigTabletMyOffice = new HashSet<tblConfigTabletMyOffice>();
            tblConfigTabletMyQuality = new HashSet<tblConfigTabletMyQuality>();
        }

        [Key]
        public int IdAplicacionesNPantalla { get; set; }
        public int IdAplicacion { get; set; }
        public int IdPantalla { get; set; }
        [StringLength(20)]
        public string? Version { get; set; }
        public bool Actualizar { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? FechaUltimaActualizacion { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? FechaUltimaConexion { get; set; }

        [ForeignKey("IdAplicacion")]
        [InverseProperty("tblAplicacionesNPantallas")]
        public virtual tblAplicaciones IdAplicacionNavigation { get; set; } = null!;
        [ForeignKey("IdPantalla")]
        [InverseProperty("tblAplicacionesNPantallas")]
        public virtual tblPantallas IdPantallaNavigation { get; set; } = null!;
        [InverseProperty("idAplicacionesNPantallaNavigation")]
        public virtual ICollection<tblConfigMYUNIF> tblConfigMYUNIF { get; set; }
        [InverseProperty("IdAplicacionesNPantallaNavigation")]
        public virtual ICollection<tblConfigPTMYOF> tblConfigPTMYOF { get; set; }
        [InverseProperty("IdAplicacionesNPantallaNavigation")]
        public virtual ICollection<tblConfigPTMYPR> tblConfigPTMYPR { get; set; }
        [InverseProperty("idAplicacionesNPantallaNavigation")]
        public virtual ICollection<tblConfigTabletMyAudit> tblConfigTabletMyAudit { get; set; }
        [InverseProperty("idAplicacionesNPantallaNavigation")]
        public virtual ICollection<tblConfigTabletMyInventory> tblConfigTabletMyInventory { get; set; }
        [InverseProperty("IdAplicacionesNPantallaNavigation")]
        public virtual ICollection<tblConfigTabletMyOffice> tblConfigTabletMyOffice { get; set; }
        [InverseProperty("idAplicacionesNPantallaNavigation")]
        public virtual ICollection<tblConfigTabletMyQuality> tblConfigTabletMyQuality { get; set; }
    }
}
