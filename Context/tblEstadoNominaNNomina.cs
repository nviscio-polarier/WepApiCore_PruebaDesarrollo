using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblEstadoNominaNNomina", Schema = "RRHH")]
    public partial class tblEstadoNominaNNomina
    {
        [Key]
        public int idNomina { get; set; }
        [Key]
        public byte idEstadoNomina { get; set; }
        [Key]
        [Precision(0)]
        public DateTimeOffset fecha { get; set; }
        public int? idUsuario_valida { get; set; }
        public string? observaciones { get; set; }

        [ForeignKey("idEstadoNomina")]
        [InverseProperty("tblEstadoNominaNNomina")]
        public virtual tblEstadoNomina idEstadoNominaNavigation { get; set; } = null!;
        [ForeignKey("idNomina")]
        [InverseProperty("tblEstadoNominaNNomina")]
        public virtual tblNomina idNominaNavigation { get; set; } = null!;
        [ForeignKey("idUsuario_valida")]
        [InverseProperty("tblEstadoNominaNNomina")]
        public virtual tblUsuario? idUsuario_validaNavigation { get; set; }
    }
}
