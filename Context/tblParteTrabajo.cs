using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblParteTrabajo", Schema = "Assistant")]
    public partial class tblParteTrabajo
    {
        public tblParteTrabajo()
        {
            tblIncidenciaNParte = new HashSet<tblIncidenciaNParte>();
            tblPersonasNParte = new HashSet<tblPersonasNParte>();
            tblRecambioNParteTrabajo = new HashSet<tblRecambioNParteTrabajo>();
            tblRecambioNParteTrabajoIBS = new HashSet<tblRecambioNParteTrabajoIBS>();
            tblServicioExternoNParteTrabajo = new HashSet<tblServicioExternoNParteTrabajo>();
        }

        [Key]
        public int idParteTrabajo { get; set; }
        [Precision(0)]
        public DateTimeOffset fecha { get; set; }
        public string resolucion { get; set; } = null!;
        [StringLength(8)]
        public string? idxParteTrabajo { get; set; }
        public int idMaquina { get; set; }
        public int idLavanderia { get; set; }
        [StringLength(8)]
        public string? codigo { get; set; }
        public bool? isApp { get; set; }
        public int? idUsuarioCrea { get; set; }

        [ForeignKey("idLavanderia")]
        [InverseProperty("tblParteTrabajo")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
        [ForeignKey("idMaquina")]
        [InverseProperty("tblParteTrabajo")]
        public virtual tblMaquina idMaquinaNavigation { get; set; } = null!;
        [ForeignKey("idUsuarioCrea")]
        [InverseProperty("tblParteTrabajo")]
        public virtual tblUsuario? idUsuarioCreaNavigation { get; set; }
        [InverseProperty("idParteNavigation")]
        public virtual ICollection<tblIncidenciaNParte> tblIncidenciaNParte { get; set; }
        [InverseProperty("idParteNavigation")]
        public virtual ICollection<tblPersonasNParte> tblPersonasNParte { get; set; }
        [InverseProperty("idParteTrabajoNavigation")]
        public virtual ICollection<tblRecambioNParteTrabajo> tblRecambioNParteTrabajo { get; set; }
        [InverseProperty("idParteTrabajoNavigation")]
        public virtual ICollection<tblRecambioNParteTrabajoIBS> tblRecambioNParteTrabajoIBS { get; set; }
        [InverseProperty("idParteTrabajoNavigation")]
        public virtual ICollection<tblServicioExternoNParteTrabajo> tblServicioExternoNParteTrabajo { get; set; }
    }
}
