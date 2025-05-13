using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoRechazo", Schema = "Produccion")]
    public partial class tblTipoRechazo
    {
        public tblTipoRechazo()
        {
            tblRechazoNProduccion = new HashSet<tblRechazoNProduccion>();
            idLavanderia = new HashSet<tblLavanderia>();
        }

        [Key]
        public byte idTipoRechazo { get; set; }
        [StringLength(50)]
        public string denominacion { get; set; } = null!;
        public byte? codigo { get; set; }

        [InverseProperty("idTipoRechazoNavigation")]
        public virtual ICollection<tblRechazoNProduccion> tblRechazoNProduccion { get; set; }

        [ForeignKey("idTipoRechazo")]
        [InverseProperty("idTipoRechazo")]
        public virtual ICollection<tblLavanderia> idLavanderia { get; set; }
    }
}
