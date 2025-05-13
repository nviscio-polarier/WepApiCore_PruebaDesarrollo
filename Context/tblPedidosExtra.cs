using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPedidosExtra", Schema = "Office")]
    [Index("idAlmacen", Name = "IX_tblPedidosExtra_idAlmacen")]
    public partial class tblPedidosExtra
    {
        public tblPedidosExtra()
        {
            tblPrendaNPedidoExtra = new HashSet<tblPrendaNPedidoExtra>();
            tblRepartoOffice = new HashSet<tblRepartoOffice>();
        }

        [Key]
        public int idPedidoExtra { get; set; }
        public int? idPersona { get; set; }
        public int idEstadoOffice { get; set; }
        public int? idAlmacen { get; set; }
        public int idLavanderia { get; set; }
        public int? idSubAlmacen { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? fecha { get; set; }
        public string? notas { get; set; }
        [StringLength(5)]
        public string? idxPedidoExtra { get; set; }

        [ForeignKey("idAlmacen")]
        [InverseProperty("tblPedidosExtra")]
        public virtual tblAlmacen? idAlmacenNavigation { get; set; }
        [ForeignKey("idEstadoOffice")]
        [InverseProperty("tblPedidosExtra")]
        public virtual tblEstadoOffice idEstadoOfficeNavigation { get; set; } = null!;
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblPedidosExtra")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
        [ForeignKey("idPersona")]
        [InverseProperty("tblPedidosExtra")]
        public virtual tblPersona? idPersonaNavigation { get; set; }
        [ForeignKey("idSubAlmacen")]
        [InverseProperty("tblPedidosExtra")]
        public virtual tblSubAlmacen? idSubAlmacenNavigation { get; set; }
        [InverseProperty("idPedidoExtraNavigation")]
        public virtual ICollection<tblPrendaNPedidoExtra> tblPrendaNPedidoExtra { get; set; }
        [InverseProperty("idPedidoExtraNavigation")]
        public virtual ICollection<tblRepartoOffice> tblRepartoOffice { get; set; }
    }
}
