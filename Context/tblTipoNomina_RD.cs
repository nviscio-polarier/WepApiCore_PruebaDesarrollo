using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoNomina_RD", Schema = "RRHH")]
    public partial class tblTipoNomina_RD
    {
        public tblTipoNomina_RD()
        {
            tblNomina_RD = new HashSet<tblNomina_RD>();
        }

        [Key]
        public short idTipoNomina_RD { get; set; }
        public int? idTraduccion { get; set; }
        public string denominacion { get; set; } = null!;

        [ForeignKey("idTraduccion")]
        [InverseProperty("tblTipoNomina_RD")]
        public virtual tblTraduccion? idTraduccionNavigation { get; set; }
        [InverseProperty("idTipoNomina_RDNavigation")]
        public virtual ICollection<tblNomina_RD> tblNomina_RD { get; set; }
    }
}
