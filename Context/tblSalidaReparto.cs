using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblSalidaReparto", Schema = "Logistica")]
    public partial class tblSalidaReparto
    {
        public tblSalidaReparto()
        {
            idReparto = new HashSet<tblReparto>();
        }

        [Key]
        public int idSalidaReparto { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? fecha { get; set; }
        public int idVehiculo { get; set; }
        public int? idConductor { get; set; }
        public int? idEstibador1 { get; set; }
        public int? idEstibador2 { get; set; }

        [ForeignKey("idConductor")]
        [InverseProperty("tblSalidaRepartoidConductorNavigation")]
        public virtual tblPersona? idConductorNavigation { get; set; }
        [ForeignKey("idEstibador1")]
        [InverseProperty("tblSalidaRepartoidEstibador1Navigation")]
        public virtual tblPersona? idEstibador1Navigation { get; set; }
        [ForeignKey("idEstibador2")]
        [InverseProperty("tblSalidaRepartoidEstibador2Navigation")]
        public virtual tblPersona? idEstibador2Navigation { get; set; }
        [ForeignKey("idVehiculo")]
        [InverseProperty("tblSalidaReparto")]
        public virtual tblVehiculo idVehiculoNavigation { get; set; } = null!;

        [ForeignKey("idSalidaReparto")]
        [InverseProperty("idSalidaReparto")]
        public virtual ICollection<tblReparto> idReparto { get; set; }
    }
}
