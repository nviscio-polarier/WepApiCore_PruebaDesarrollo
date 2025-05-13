using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoNomina_MX", Schema = "RRHH")]
    public partial class tblTipoNomina_MX
    {
        public tblTipoNomina_MX()
        {
            tblHistoricoAsientoNomina_MX = new HashSet<tblHistoricoAsientoNomina_MX>();
            tblNomina_MX = new HashSet<tblNomina_MX>();
        }

        [Key]
        public short idTipoNomina_MX { get; set; }
        public int? idTraduccion { get; set; }
        public string denominacion { get; set; } = null!;

        [ForeignKey("idTraduccion")]
        [InverseProperty("tblTipoNomina_MX")]
        public virtual tblTraduccion? idTraduccionNavigation { get; set; }
        [InverseProperty("idTipoNomina_MXNavigation")]
        public virtual ICollection<tblHistoricoAsientoNomina_MX> tblHistoricoAsientoNomina_MX { get; set; }
        [InverseProperty("idTipoNomina_MXNavigation")]
        public virtual ICollection<tblNomina_MX> tblNomina_MX { get; set; }
    }
}
