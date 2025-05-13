using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblImagenNProveedor", Schema = "Administracion")]
    public partial class tblImagenNProveedor
    {
        [Key]
        public int idImagenNProveedor { get; set; }
        public int idAdmProveedor { get; set; }
        public byte[] imagen { get; set; } = null!;

        [ForeignKey("idAdmProveedor")]
        [InverseProperty("tblImagenNProveedor")]
        public virtual tblAdmProveedor idAdmProveedorNavigation { get; set; } = null!;
    }
}
