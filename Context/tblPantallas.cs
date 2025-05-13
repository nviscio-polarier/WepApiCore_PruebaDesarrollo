using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCore.Context
{
    public partial class tblPantallas
    {
        public tblPantallas()
        {
            tblAplicacionesNPantallas = new HashSet<tblAplicacionesNPantallas>();
        }

        [Key]
        public int IdPantalla { get; set; }
        public string Codigo { get; set; } = null!;
        [StringLength(50)]
        public string? Descripcion { get; set; }
        public int? idLavanderia { get; set; }

        [InverseProperty("IdPantallaNavigation")]
        public virtual ICollection<tblAplicacionesNPantallas> tblAplicacionesNPantallas { get; set; }
    }
}
