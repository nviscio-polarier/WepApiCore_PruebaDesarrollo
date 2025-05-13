using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCore.Context
{
    public partial class tblConfigTabletMyAudit
    {
        [Key]
        public int idConfig { get; set; }
        public int? idAplicacionesNPantalla { get; set; }
        public bool? TipoConexion { get; set; }

        [ForeignKey("idAplicacionesNPantalla")]
        [InverseProperty("tblConfigTabletMyAudit")]
        public virtual tblAplicacionesNPantallas? idAplicacionesNPantallaNavigation { get; set; }
    }
}
