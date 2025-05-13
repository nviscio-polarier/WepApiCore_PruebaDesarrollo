using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoNomina", Schema = "RRHH")]
    public partial class tblTipoNomina
    {
        public tblTipoNomina()
        {
            tblNomina = new HashSet<tblNomina>();
        }

        [Key]
        public short idTipoNomina { get; set; }
        public int? idTraduccion { get; set; }
        public string denominacion { get; set; } = null!;

        [ForeignKey("idTraduccion")]
        [InverseProperty("tblTipoNomina")]
        public virtual tblTraduccion? idTraduccionNavigation { get; set; }
        [InverseProperty("idTipoNominaNavigation")]
        public virtual ICollection<tblNomina> tblNomina { get; set; }
    }
}
