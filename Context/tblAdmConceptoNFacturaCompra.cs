using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCore.Context
{
    [Table("tblAdmConceptoNFacturaCompra", Schema = "Administracion")]
    public partial class tblAdmConceptoNFacturaCompra
    {
        [Key]
        public int idAdmConcepto { get; set; }
        [Key]
        public int idAdmFacturaCompra { get; set; }
        public int? cantidad { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? precio { get; set; }

        [ForeignKey("idAdmConcepto")]
        [InverseProperty("tblAdmConceptoNFacturaCompra")]
        public virtual tblAdmConcepto idAdmConceptoNavigation { get; set; } = null!;
        [ForeignKey("idAdmFacturaCompra")]
        [InverseProperty("tblAdmConceptoNFacturaCompra")]
        public virtual tblAdmFacturaCompra idAdmFacturaCompraNavigation { get; set; } = null!;
    }
}
