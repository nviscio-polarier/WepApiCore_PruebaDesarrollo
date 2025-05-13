using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblArchivo", Schema = "General")]
    public partial class tblArchivo
    {
        public tblArchivo()
        {
            tblEntidadNInventarioidArchivo_firmaClienteNavigation = new HashSet<tblEntidadNInventario>();
            tblEntidadNInventarioidArchivo_firmaPolarierNavigation = new HashSet<tblEntidadNInventario>();
            tblRepartoOffice = new HashSet<tblRepartoOffice>();
        }

        [Key]
        public int idArchivo { get; set; }
        public byte[] archivo { get; set; } = null!;

        [InverseProperty("idArchivo_firmaClienteNavigation")]
        public virtual ICollection<tblEntidadNInventario> tblEntidadNInventarioidArchivo_firmaClienteNavigation { get; set; }
        [InverseProperty("idArchivo_firmaPolarierNavigation")]
        public virtual ICollection<tblEntidadNInventario> tblEntidadNInventarioidArchivo_firmaPolarierNavigation { get; set; }
        [InverseProperty("idArchivo_firmaNavigation")]
        public virtual ICollection<tblRepartoOffice> tblRepartoOffice { get; set; }
    }
}
