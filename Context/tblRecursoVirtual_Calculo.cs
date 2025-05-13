using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblRecursoVirtual_Calculo", Schema = "Energeticos")]
    public partial class tblRecursoVirtual_Calculo
    {
        [Key]
        public int idRecursoVirtual { get; set; }
        [Key]
        public int idRecursoContador { get; set; }
        public short operando { get; set; }

        [ForeignKey("idRecursoContador")]
        [InverseProperty("tblRecursoVirtual_CalculoidRecursoContadorNavigation")]
        public virtual tblRecursoContador idRecursoContadorNavigation { get; set; } = null!;
        [ForeignKey("idRecursoVirtual")]
        [InverseProperty("tblRecursoVirtual_CalculoidRecursoVirtualNavigation")]
        public virtual tblRecursoContador idRecursoVirtualNavigation { get; set; } = null!;
    }
}
