using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblEstadoHistoricoAsientoNomina", Schema = "RRHH")]
    public partial class tblEstadoHistoricoAsientoNomina
    {
        public tblEstadoHistoricoAsientoNomina()
        {
            tblHistoricoAsientoNomina = new HashSet<tblHistoricoAsientoNomina>();
            tblNomina = new HashSet<tblNomina>();
        }

        [Key]
        public byte idEstadoHistoricoAsientoNomina { get; set; }
        public string denominacion { get; set; } = null!;

        [InverseProperty("idEstadoHistoricoAsientoNominaNavigation")]
        public virtual ICollection<tblHistoricoAsientoNomina> tblHistoricoAsientoNomina { get; set; }
        [InverseProperty("idEstadoHistoricoAsientoNominaNavigation")]
        public virtual ICollection<tblNomina> tblNomina { get; set; }
    }
}
