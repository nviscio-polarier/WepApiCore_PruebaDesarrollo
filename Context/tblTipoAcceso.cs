using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoAcceso", Schema = "RRHH")]
    public partial class tblTipoAcceso
    {
        public tblTipoAcceso()
        {
            tblControlAcceso = new HashSet<tblControlAcceso>();
        }

        [Key]
        public byte idTipoAcceso { get; set; }
        public string denominacion { get; set; } = null!;

        [InverseProperty("idTipoAccesoNavigation")]
        public virtual ICollection<tblControlAcceso> tblControlAcceso { get; set; }
    }
}
