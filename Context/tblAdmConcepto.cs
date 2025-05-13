using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCore.Context
{
    [Table("tblAdmConcepto", Schema = "Administracion")]
    public partial class tblAdmConcepto
    {
        public tblAdmConcepto()
        {
            tblAdmConceptoNFacturaCompra = new HashSet<tblAdmConceptoNFacturaCompra>();
            tblAdmConceptoNFacturaVenta = new HashSet<tblAdmConceptoNFacturaVenta>();
        }

        [Key]
        public int idAdmConcepto { get; set; }
        public string? descripcion { get; set; }

        [InverseProperty("idAdmConceptoNavigation")]
        public virtual ICollection<tblAdmConceptoNFacturaCompra> tblAdmConceptoNFacturaCompra { get; set; }
        [InverseProperty("idAdmConceptoNavigation")]
        public virtual ICollection<tblAdmConceptoNFacturaVenta> tblAdmConceptoNFacturaVenta { get; set; }
    }
}
