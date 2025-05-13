using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblLogConexiones", Schema = "MyRealData")]
    public partial class tblLogConexiones
    {
        [Key]
        public int idLogConexion { get; set; }
        /// <summary>
        ///  1 - httpResponse_pulsosEnergyHub
        /// </summary>
        public string tipoConexion { get; set; } = null!;
        [Precision(0)]
        public DateTimeOffset fecha { get; set; }
        public string codigoRespuesta { get; set; } = null!;
        public int idEnergyHub { get; set; }

        [ForeignKey("idEnergyHub")]
        [InverseProperty("tblLogConexiones")]
        public virtual tblEnergyHub idEnergyHubNavigation { get; set; } = null!;
    }
}
