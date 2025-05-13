using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoDocumento", Schema = "RRHH")]
    public partial class tblTipoDocumento
    {
        public tblTipoDocumento()
        {
            tblDocumento = new HashSet<tblDocumento>();
        }

        [Key]
        public byte idTipoDocumento { get; set; }
        public string denominacion { get; set; } = null!;
        public int? idTraduccion { get; set; }

        [ForeignKey("idTraduccion")]
        [InverseProperty("tblTipoDocumento")]
        public virtual tblTraduccion? idTraduccionNavigation { get; set; }
        [InverseProperty("idTipoDocumentoNavigation")]
        public virtual ICollection<tblDocumento> tblDocumento { get; set; }
    }
}
