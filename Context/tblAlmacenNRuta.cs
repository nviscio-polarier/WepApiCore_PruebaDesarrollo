using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAlmacenNRuta", Schema = "Office")]
    public partial class tblAlmacenNRuta
    {
        [Key]
        public int idAlmacen { get; set; }
        [Key]
        public int idRuta { get; set; }
        public short orden { get; set; }

        [ForeignKey("idAlmacen")]
        [InverseProperty("tblAlmacenNRuta")]
        public virtual tblAlmacen idAlmacenNavigation { get; set; } = null!;
        [ForeignKey("idRuta")]
        [InverseProperty("tblAlmacenNRuta")]
        public virtual tblRuta idRutaNavigation { get; set; } = null!;
    }
}
