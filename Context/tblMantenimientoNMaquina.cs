using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblMantenimientoNMaquina", Schema = "MyRealData")]
    public partial class tblMantenimientoNMaquina
    {
        [Key]
        public int idMantenimientoNMaquina { get; set; }
        public int idMaquina { get; set; }
        public byte idTipoMantenimientoNMaquina { get; set; }
        public DateTimeOffset fechaIni { get; set; }
        public DateTimeOffset? fechaFin { get; set; }
        public int idUsuario { get; set; }
        public bool isOffline { get; set; }

        [ForeignKey("idTipoMantenimientoNMaquina")]
        [InverseProperty("tblMantenimientoNMaquina")]
        public virtual tblTipoMantenimientoMaquina idTipoMantenimientoNMaquinaNavigation { get; set; } = null!;
        [ForeignKey("idUsuario")]
        [InverseProperty("tblMantenimientoNMaquina")]
        public virtual tblUsuario idUsuarioNavigation { get; set; } = null!;
    }
}
