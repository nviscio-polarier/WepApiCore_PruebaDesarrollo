using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoIncidencia", Schema = "Incidencias")]
    public partial class tblTipoIncidencia
    {
        public tblTipoIncidencia()
        {
            tblTipoSubIncidencia = new HashSet<tblTipoSubIncidencia>();
            idCorreo = new HashSet<tblCorreosNLav>();
        }

        [Key]
        public byte idTipoIncidencia { get; set; }
        [StringLength(50)]
        public string denominacion { get; set; } = null!;
        [StringLength(2)]
        public string? idxTipoIncidencia { get; set; }
        public int? idTraduccion { get; set; }
        public int? idTraduccion_abr { get; set; }
        public string? icon { get; set; }

        [ForeignKey("idTraduccion")]
        [InverseProperty("tblTipoIncidenciaidTraduccionNavigation")]
        public virtual tblTraduccion? idTraduccionNavigation { get; set; }
        [ForeignKey("idTraduccion_abr")]
        [InverseProperty("tblTipoIncidenciaidTraduccion_abrNavigation")]
        public virtual tblTraduccion? idTraduccion_abrNavigation { get; set; }
        [InverseProperty("idTipoIncidenciaNavigation")]
        public virtual ICollection<tblTipoSubIncidencia> tblTipoSubIncidencia { get; set; }

        [ForeignKey("idTipoIncidencia")]
        [InverseProperty("idTipoIncidencia")]
        public virtual ICollection<tblCorreosNLav> idCorreo { get; set; }
    }
}
