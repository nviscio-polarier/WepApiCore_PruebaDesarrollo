using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblRuta", Schema = "Office")]
    public partial class tblRuta1
    {
        public tblRuta1()
        {
            tblAlmacenNRuta = new HashSet<tblAlmacenNRuta>();
            tblRevision = new HashSet<tblRevision>();
        }

        [Key]
        public int idRuta { get; set; }
        public int codigo { get; set; }
        [StringLength(50)]
        public string denominacion { get; set; } = null!;
        public int idEntidad { get; set; }
        public bool? activo { get; set; }
        public bool eliminado { get; set; }

        [ForeignKey("idEntidad")]
        [InverseProperty("tblRuta1")]
        public virtual tblEntidad idEntidadNavigation { get; set; } = null!;
        [InverseProperty("idRutaNavigation")]
        public virtual ICollection<tblAlmacenNRuta> tblAlmacenNRuta { get; set; }
        [InverseProperty("idRutaNavigation")]
        public virtual ICollection<tblRevision> tblRevision { get; set; }
    }
}
