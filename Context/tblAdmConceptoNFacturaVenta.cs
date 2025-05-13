using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCore.Context
{
    [Table("tblAdmConceptoNFacturaVenta", Schema = "Administracion")]
    public partial class tblAdmConceptoNFacturaVenta
    {
        [Key]
        public int idAdmConcepto { get; set; }
        [Key]
        public int idAdmFacturaVenta { get; set; }
        public int? cantidad { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? precio { get; set; }

        [ForeignKey("idAdmConcepto")]
        [InverseProperty("tblAdmConceptoNFacturaVenta")]
        public virtual tblAdmConcepto idAdmConceptoNavigation { get; set; } = null!;
        [ForeignKey("idAdmFacturaVenta")]
        [InverseProperty("tblAdmConceptoNFacturaVenta")]
        public virtual tblAdmFacturaVenta idAdmFacturaVentaNavigation { get; set; } = null!;
    }
}
