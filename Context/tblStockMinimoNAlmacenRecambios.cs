using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblStockMinimoNAlmacenRecambios", Schema = "Assistant")]
    public partial class tblStockMinimoNAlmacenRecambios
    {
        [Key]
        public int idAlmacen { get; set; }
        [Key]
        public int idRecambio { get; set; }
        public int cantidad { get; set; }

        [ForeignKey("idAlmacen")]
        [InverseProperty("tblStockMinimoNAlmacenRecambios")]
        public virtual tblAlmacenRecambios idAlmacenNavigation { get; set; } = null!;
        [ForeignKey("idRecambio")]
        [InverseProperty("tblStockMinimoNAlmacenRecambios")]
        public virtual tblRecambio idRecambioNavigation { get; set; } = null!;
    }
}
