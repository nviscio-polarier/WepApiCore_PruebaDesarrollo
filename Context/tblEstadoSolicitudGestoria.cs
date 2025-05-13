using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblEstadoSolicitudGestoria", Schema = "RRHH")]
    public partial class tblEstadoSolicitudGestoria
    {
        [Key]
        public byte idEstadoSolicitudGestoria { get; set; }
        public string denominacion { get; set; } = null!;
    }
}
