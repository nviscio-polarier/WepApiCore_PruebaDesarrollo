using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblMesNAjustePresupuestario", Schema = "ControlPresupuestario")]
    public partial class tblMesNAjustePresupuestario
    {
        [Key]
        public int idAjustePresupuestario { get; set; }
        [Key]
        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }

        [ForeignKey("idAjustePresupuestario")]
        [InverseProperty("tblMesNAjustePresupuestario")]
        public virtual tblAjustePresupuestario idAjustePresupuestarioNavigation { get; set; } = null!;
    }
}
