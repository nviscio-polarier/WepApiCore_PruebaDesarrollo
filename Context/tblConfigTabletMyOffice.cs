using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCore.Context
{
    public partial class tblConfigTabletMyOffice
    {
        [Key]
        public int idConfig { get; set; }
        public int? IdAplicacionesNPantalla { get; set; }
        public int? IdCompañia { get; set; }
        public int? IdEntidad { get; set; }
        public bool? TipoConexion { get; set; }
        public int? TipoAplicacion { get; set; }
        public bool? CambioEntidad { get; set; }
        public bool? CambioAlmacenes { get; set; }
        public int? TipoAlmacen { get; set; }
        public bool? CodigoRevisor { get; set; }
        public bool? RevisionesRepartosIncompletos { get; set; }
        public bool? RevisionesStockIgualReposicion { get; set; }
        public bool? RepartosEntregaIgualPendiente { get; set; }
        public bool FirmaActiva { get; set; }

        [ForeignKey("IdAplicacionesNPantalla")]
        [InverseProperty("tblConfigTabletMyOffice")]
        public virtual tblAplicacionesNPantallas? IdAplicacionesNPantallaNavigation { get; set; }
    }
}
