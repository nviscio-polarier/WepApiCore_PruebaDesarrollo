using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCore.Context
{
    public partial class tblConfigPTMYOF
    {
        [Key]
        public int idConfig { get; set; }
        public int IdAplicacionesNPantalla { get; set; }
        public int? idCompañia { get; set; }
        public int? idEntidad { get; set; }
        [StringLength(2)]
        public string? Idioma { get; set; }
        public int? TiempoActualizar { get; set; }
        public int? TiempoCierreAutomatico { get; set; }
        public int? TipoEntregaPendiente { get; set; }
        public bool? ValidarPersona { get; set; }
        public int? Peligro { get; set; }
        public int? Alerta { get; set; }
        public bool? Imprimir { get; set; }
        public int? NCopiasValidar { get; set; }
        public int? PrendasImpresion { get; set; }
        [StringLength(50)]
        public string? TituloAlbaran { get; set; }
        public int? OrdenacionGrids { get; set; }
        public int? idLavanderia { get; set; }

        [ForeignKey("IdAplicacionesNPantalla")]
        [InverseProperty("tblConfigPTMYOF")]
        public virtual tblAplicacionesNPantallas IdAplicacionesNPantallaNavigation { get; set; } = null!;
    }
}
