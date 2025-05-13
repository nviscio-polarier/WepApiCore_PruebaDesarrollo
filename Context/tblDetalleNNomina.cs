using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblDetalleNNomina", Schema = "RRHH")]
    public partial class tblDetalleNNomina
    {
        [Key]
        public int idDetalleNNomina { get; set; }
        public int idNomina { get; set; }
        public string detalle { get; set; } = null!;

        [ForeignKey("idNomina")]
        [InverseProperty("tblDetalleNNomina")]
        public virtual tblNomina idNominaNavigation { get; set; } = null!;
    }
}
