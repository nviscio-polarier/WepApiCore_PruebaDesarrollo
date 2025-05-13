using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoSalidaRecambio", Schema = "Assistant")]
    public partial class tblTipoSalidaRecambio
    {
        public tblTipoSalidaRecambio()
        {
            tblAlmacenRecambios = new HashSet<tblAlmacenRecambios>();
        }

        [Key]
        public byte idTipoSalidaRecambio { get; set; }
        public int? idTraduccion { get; set; }
        public string? denominacion { get; set; }

        [ForeignKey("idTraduccion")]
        [InverseProperty("tblTipoSalidaRecambio")]
        public virtual tblTraduccion? idTraduccionNavigation { get; set; }
        [InverseProperty("idTipoSalidaRecambioNavigation")]
        public virtual ICollection<tblAlmacenRecambios> tblAlmacenRecambios { get; set; }
    }
}
