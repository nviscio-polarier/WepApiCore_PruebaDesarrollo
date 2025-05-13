using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Keyless]
    [Table("tblAjusteNHistoricoPresupuestario", Schema = "ControlPresupuestario")]
    public partial class tblAjusteNHistoricoPresupuestario
    {
        public int? idAjustePresupuestario { get; set; }
        public int? idHistoricoPresupuestario { get; set; }

        [ForeignKey("idAjustePresupuestario")]
        public virtual tblAjustePresupuestario? idAjustePresupuestarioNavigation { get; set; }
        [ForeignKey("idHistoricoPresupuestario")]
        public virtual tblHistoricoPresupuestario? idHistoricoPresupuestarioNavigation { get; set; }
    }
}
