using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblRecambioNParteTrabajoIBS", Schema = "Assistant")]
    public partial class tblRecambioNParteTrabajoIBS
    {
        [Key]
        public int idRecambioNParteTrabajo { get; set; }
        public int idParteTrabajo { get; set; }
        public string referenciaRecambio { get; set; } = null!;
        public string denominacion { get; set; } = null!;
        public short cantidad { get; set; }
        [Column(TypeName = "money")]
        public decimal precio { get; set; }

        [ForeignKey("idParteTrabajo")]
        [InverseProperty("tblRecambioNParteTrabajoIBS")]
        public virtual tblParteTrabajo idParteTrabajoNavigation { get; set; } = null!;
    }
}
