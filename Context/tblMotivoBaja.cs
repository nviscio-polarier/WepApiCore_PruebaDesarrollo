using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblMotivoBaja", Schema = "RRHH")]
    public partial class tblMotivoBaja
    {
        public tblMotivoBaja()
        {
            tblNomina = new HashSet<tblNomina>();
            tblPersonaNTipoContrato = new HashSet<tblPersonaNTipoContrato>();
        }

        [Key]
        public byte idMotivoBaja { get; set; }
        public string denominacion { get; set; } = null!;

        [InverseProperty("idMotivoBajaNavigation")]
        public virtual ICollection<tblNomina> tblNomina { get; set; }
        [InverseProperty("idMotivoBajaNavigation")]
        public virtual ICollection<tblPersonaNTipoContrato> tblPersonaNTipoContrato { get; set; }
    }
}
