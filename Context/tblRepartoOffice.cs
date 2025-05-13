using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblRepartoOffice", Schema = "Office")]
    [Index("idEntidad", "idLavanderia", "fecha", Name = "IX_tblRepartoOffice_idEntidad_idLavanderia_fecha")]
    [Index("idPedidoExtra", Name = "IX_tblRepartoOffice_idPedidoExtra")]
    [Index("idRevision", Name = "IX_tblRepartoOffice_idRevision")]
    public partial class tblRepartoOffice
    {
        public tblRepartoOffice()
        {
            tblPrendaNRepartoOffice = new HashSet<tblPrendaNRepartoOffice>();
        }

        [Key]
        public int idRepartoOffice { get; set; }
        public int? idRevision { get; set; }
        public int? idPersona { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime fecha { get; set; }
        public bool repartoExtra { get; set; }
        [StringLength(7)]
        public string? idxReparto { get; set; }
        public int? idPedidoExtra { get; set; }
        public int? idEntidad { get; set; }
        public int? idLavanderia { get; set; }
        public int? idArchivo_firma { get; set; }
        public string? firmante { get; set; }
        [Required]
        public bool? isApp { get; set; }

        [ForeignKey("idArchivo_firma")]
        [InverseProperty("tblRepartoOffice")]
        public virtual tblArchivo? idArchivo_firmaNavigation { get; set; }
        [ForeignKey("idEntidad")]
        [InverseProperty("tblRepartoOffice")]
        public virtual tblEntidad? idEntidadNavigation { get; set; }
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblRepartoOffice")]
        public virtual tblLavanderia? idLavanderiaNavigation { get; set; }
        [ForeignKey("idPedidoExtra")]
        [InverseProperty("tblRepartoOffice")]
        public virtual tblPedidosExtra? idPedidoExtraNavigation { get; set; }
        [ForeignKey("idPersona")]
        [InverseProperty("tblRepartoOffice")]
        public virtual tblPersona? idPersonaNavigation { get; set; }
        [ForeignKey("idRevision")]
        [InverseProperty("tblRepartoOffice")]
        public virtual tblRevision? idRevisionNavigation { get; set; }
        [InverseProperty("idRepartoOfficeNavigation")]
        public virtual ICollection<tblPrendaNRepartoOffice> tblPrendaNRepartoOffice { get; set; }
    }
}
