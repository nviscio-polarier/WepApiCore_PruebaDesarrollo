using System.ComponentModel.DataAnnotations;

namespace WebApiCore.Class.bdERP.Incidencias
{
    public class TipoIncidenciaNLavanderia
    {
        [Key]
        public int idLavanderia { get; set; }
        [Key]
        public int idTipoIncidencia { get; set; }
        public string denominacion { get; set; }
        public string icon { get; set; }
        public int numIncidencias { get; set; }
    }
}
