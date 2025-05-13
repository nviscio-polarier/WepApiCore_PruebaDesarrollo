using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCore.Context
{
    public partial class tblConfigPTMYPR
    {
        [Key]
        public int IdConfig { get; set; }
        public int? IdAplicacionesNPantalla { get; set; }
        public int? IdLavanderia { get; set; }
        [StringLength(5)]
        public string? MaquinaConfig { get; set; }
        [StringLength(2)]
        public string? Idioma { get; set; }
        [StringLength(2)]
        public string? IdiomaNumerico { get; set; }
        [StringLength(50)]
        public string? PassConfig { get; set; }
        public int? DiasAlmacenamiento { get; set; }
        public int? MinutosSincronizar { get; set; }
        public int? MantenerPrendaInsert { get; set; }
        public int? MantenerConfigSesion { get; set; }
        public int? CambioDia { get; set; }
        [StringLength(50)]
        public string? PassCambioDia { get; set; }
        [StringLength(50)]
        public string? DiaCambiado { get; set; }
        public int? DiasMin { get; set; }
        public int? DiasMax { get; set; }

        [ForeignKey("IdAplicacionesNPantalla")]
        [InverseProperty("tblConfigPTMYPR")]
        public virtual tblAplicacionesNPantallas? IdAplicacionesNPantallaNavigation { get; set; }
    }
}
