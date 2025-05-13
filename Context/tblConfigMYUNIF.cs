using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCore.Context
{
    public partial class tblConfigMYUNIF
    {
        [Key]
        public int idConfig { get; set; }
        public int idAplicacionesNPantalla { get; set; }
        [StringLength(50)]
        public string? bdCU { get; set; }
        [StringLength(2)]
        public string? Idioma { get; set; }
        [StringLength(50)]
        public string? PassConfig { get; set; }
        [StringLength(50)]
        public string? PassAcceso { get; set; }
        public int? TiempoReenvio { get; set; }
        public int? DiasAlmacenamiento { get; set; }
        public bool? HoraServidor { get; set; }
        public bool? PrendasExtra { get; set; }
        public bool? PrendasGenericas { get; set; }
        public bool? PrendasAutomaticas { get; set; }
        public int? BarcodeLenght { get; set; }
        public int? idLavanderia { get; set; }

        [ForeignKey("idAplicacionesNPantalla")]
        [InverseProperty("tblConfigMYUNIF")]
        public virtual tblAplicacionesNPantallas idAplicacionesNPantallaNavigation { get; set; } = null!;
    }
}
