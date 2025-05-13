using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblRecambioNParteTrabajo", Schema = "Assistant")]
    public partial class tblRecambioNParteTrabajo
    {
        [Key]
        public int idRecambio { get; set; }
        [Key]
        public int idParteTrabajo { get; set; }
        public int cantidad { get; set; }
        [Key]
        public int idAlmacen { get; set; }
        [Column(TypeName = "decimal(10, 4)")]
        public decimal? precio { get; set; }

        [ForeignKey("idAlmacen")]
        [InverseProperty("tblRecambioNParteTrabajo")]
        public virtual tblAlmacenRecambios idAlmacenNavigation { get; set; } = null!;
        [ForeignKey("idParteTrabajo")]
        [InverseProperty("tblRecambioNParteTrabajo")]
        public virtual tblParteTrabajo idParteTrabajoNavigation { get; set; } = null!;
        [ForeignKey("idRecambio")]
        [InverseProperty("tblRecambioNParteTrabajo")]
        public virtual tblRecambio idRecambioNavigation { get; set; } = null!;
    }
}
