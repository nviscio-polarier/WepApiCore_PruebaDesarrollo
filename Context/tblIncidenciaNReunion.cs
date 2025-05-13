using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblIncidenciaNReunion", Schema = "AppComercial")]
    public partial class tblIncidenciaNReunion
    {
        public int idIncidencia { get; set; }
        public int idReunion { get; set; }
        [Key]
        public int idIncidenciaNReunion { get; set; }

        [ForeignKey("idIncidencia")]
        [InverseProperty("tblIncidenciaNReunion")]
        public virtual tblIncidencia idIncidenciaNavigation { get; set; } = null!;
        [ForeignKey("idReunion")]
        [InverseProperty("tblIncidenciaNReunion")]
        public virtual tblReunion idReunionNavigation { get; set; } = null!;
    }
}
