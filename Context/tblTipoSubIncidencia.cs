using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoSubIncidencia", Schema = "Incidencias")]
    public partial class tblTipoSubIncidencia
    {
        public tblTipoSubIncidencia()
        {
            tblIncidencia = new HashSet<tblIncidencia>();
        }

        [Key]
        public short idSubTipoIncidencia { get; set; }
        [StringLength(50)]
        public string denominacion { get; set; } = null!;
        [StringLength(4)]
        public string idxSubTipoIncidencia { get; set; } = null!;
        public byte? idTipoIncidencia { get; set; }
        public int? idTraduccion { get; set; }

        [ForeignKey("idTipoIncidencia")]
        [InverseProperty("tblTipoSubIncidencia")]
        public virtual tblTipoIncidencia? idTipoIncidenciaNavigation { get; set; }
        [ForeignKey("idTraduccion")]
        [InverseProperty("tblTipoSubIncidencia")]
        public virtual tblTraduccion? idTraduccionNavigation { get; set; }
        [InverseProperty("idSubTipoIncidenciaNavigation")]
        public virtual ICollection<tblIncidencia> tblIncidencia { get; set; }
    }
}
