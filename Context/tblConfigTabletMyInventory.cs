using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCore.Context
{
    public partial class tblConfigTabletMyInventory
    {
        [Key]
        public int idConfig { get; set; }
        public int? idAplicacionesNPantalla { get; set; }
        public byte? idLavanderia { get; set; }
        public bool? TipoConexion { get; set; }
        public bool? aplicarStock { get; set; }

        [ForeignKey("idAplicacionesNPantalla")]
        [InverseProperty("tblConfigTabletMyInventory")]
        public virtual tblAplicacionesNPantallas? idAplicacionesNPantallaNavigation { get; set; }
    }
}
