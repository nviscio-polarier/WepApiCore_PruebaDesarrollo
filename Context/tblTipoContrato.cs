using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoContrato", Schema = "RRHH")]
    public partial class tblTipoContrato
    {
        public tblTipoContrato()
        {
            tblHistoricoNominas = new HashSet<tblHistoricoNominas>();
            tblLlamamiento = new HashSet<tblLlamamiento>();
            tblNomina = new HashSet<tblNomina>();
            tblPersonaNTipoContrato = new HashSet<tblPersonaNTipoContrato>();
        }

        [Key]
        public short idTipoContrato { get; set; }
        public string denominacion { get; set; } = null!;
        public short? numDiasPeriodoPrueba { get; set; }

        [InverseProperty("idTipoContratoNavigation")]
        public virtual ICollection<tblHistoricoNominas> tblHistoricoNominas { get; set; }
        [InverseProperty("idTipoContratoNavigation")]
        public virtual ICollection<tblLlamamiento> tblLlamamiento { get; set; }
        [InverseProperty("idTipoContratoNavigation")]
        public virtual ICollection<tblNomina> tblNomina { get; set; }
        [InverseProperty("idTipoContratoNavigation")]
        public virtual ICollection<tblPersonaNTipoContrato> tblPersonaNTipoContrato { get; set; }
    }
}
