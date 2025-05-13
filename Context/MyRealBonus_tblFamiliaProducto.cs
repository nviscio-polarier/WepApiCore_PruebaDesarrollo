using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("MyRealBonus.tblFamiliaProducto", Schema = "MyRealBonus")]
    public partial class MyRealBonus_tblFamiliaProducto
    {
        public MyRealBonus_tblFamiliaProducto()
        {
            TblProductos = new HashSet<tblProductos>();
        }

        [Key]
        public int idFamiliaProducto { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? denominacion { get; set; }

        [InverseProperty("idFamiliaProductoNavigation")]
        public virtual ICollection<tblProductos> TblProductos { get; set; }
    }
}
