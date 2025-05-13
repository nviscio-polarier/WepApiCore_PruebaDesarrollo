using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblImagenNCliente", Schema = "Administracion")]
    public partial class tblImagenNCliente
    {
        [Key]
        public int idImagenNCliente { get; set; }
        public int idAdmCliente { get; set; }
        public byte[] imagen { get; set; } = null!;

        [ForeignKey("idAdmCliente")]
        [InverseProperty("tblImagenNCliente")]
        public virtual tblAdmCliente idAdmClienteNavigation { get; set; } = null!;
    }
}
