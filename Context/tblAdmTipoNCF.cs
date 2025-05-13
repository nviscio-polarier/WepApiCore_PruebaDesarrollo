using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAdmTipoNCF", Schema = "Administracion")]
    public partial class tblAdmTipoNCF
    {
        public tblAdmTipoNCF()
        {
            tblAdmFacturaVenta = new HashSet<tblAdmFacturaVenta>();
        }

        [Key]
        public byte idAdmTipoNCF { get; set; }
        public string? denominacion { get; set; }
        [StringLength(3)]
        public string? prefijo { get; set; }

        [InverseProperty("idAdmTipoNCFNavigation")]
        public virtual ICollection<tblAdmFacturaVenta> tblAdmFacturaVenta { get; set; }
    }
}
