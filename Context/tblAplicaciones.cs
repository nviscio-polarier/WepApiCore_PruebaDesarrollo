using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCore.Context
{
    public partial class tblAplicaciones
    {
        public tblAplicaciones()
        {
            tblAplicacionesNPantallas = new HashSet<tblAplicacionesNPantallas>();
        }

        [Key]
        public int IdAplicacion { get; set; }
        [StringLength(10)]
        public string Codigo { get; set; } = null!;
        [StringLength(50)]
        public string Nombre { get; set; } = null!;
        [StringLength(20)]
        public string? UltimaVersion { get; set; }
        public string? Url { get; set; }
        public string? Password { get; set; }
        [Column(TypeName = "date")]
        public DateTime? FechaImplementacion { get; set; }
        [StringLength(50)]
        public string? TablaConfig { get; set; }

        [InverseProperty("IdAplicacionNavigation")]
        public virtual ICollection<tblAplicacionesNPantallas> tblAplicacionesNPantallas { get; set; }
    }
}
