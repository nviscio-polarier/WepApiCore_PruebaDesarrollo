using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAdmIva", Schema = "Administracion")]
    public partial class tblAdmIva
    {
        public tblAdmIva()
        {
            tblIvaNPais = new HashSet<tblIvaNPais>();
        }

        [Key]
        public byte idAdmIva { get; set; }
        public string? denominacion { get; set; }
        [Column(TypeName = "decimal(3, 2)")]
        public decimal? valor { get; set; }

        [InverseProperty("idAdmIvaNavigation")]
        public virtual ICollection<tblIvaNPais> tblIvaNPais { get; set; }
    }
}
