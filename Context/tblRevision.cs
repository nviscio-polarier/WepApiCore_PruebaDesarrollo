using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblRevision", Schema = "Office")]
    [Index("idAlmacen", Name = "IX_tblRevision_idAlmacen")]
    public partial class tblRevision
    {
        public tblRevision()
        {
            tblPrendaNRevision = new HashSet<tblPrendaNRevision>();
            tblRepartoOffice = new HashSet<tblRepartoOffice>();
        }

        [Key]
        public int idRevision { get; set; }
        public int? idPersona { get; set; }
        public int? idAlmacen { get; set; }
        public int? idSubAlmacen { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime fecha { get; set; }
        public string? notas { get; set; }
        public int idEstadoOffice { get; set; }
        [StringLength(7)]
        public string? idxRevision { get; set; }
        public int idLavanderia { get; set; }
        public int? idRuta { get; set; }
        [Column(TypeName = "decimal(5, 4)")]
        public decimal porcentaje { get; set; }

        [ForeignKey("idAlmacen")]
        [InverseProperty("tblRevision")]
        public virtual tblAlmacen? idAlmacenNavigation { get; set; }
        [ForeignKey("idEstadoOffice")]
        [InverseProperty("tblRevision")]
        public virtual tblEstadoOffice idEstadoOfficeNavigation { get; set; } = null!;
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblRevision")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
        [ForeignKey("idPersona")]
        [InverseProperty("tblRevision")]
        public virtual tblPersona? idPersonaNavigation { get; set; }
        [ForeignKey("idRuta")]
        [InverseProperty("tblRevision")]
        public virtual tblRuta? idRutaNavigation { get; set; }
        [ForeignKey("idSubAlmacen")]
        [InverseProperty("tblRevision")]
        public virtual tblSubAlmacen? idSubAlmacenNavigation { get; set; }
        [InverseProperty("idRevisionNavigation")]
        public virtual ICollection<tblPrendaNRevision> tblPrendaNRevision { get; set; }
        [InverseProperty("idRevisionNavigation")]
        public virtual ICollection<tblRepartoOffice> tblRepartoOffice { get; set; }
    }
}
