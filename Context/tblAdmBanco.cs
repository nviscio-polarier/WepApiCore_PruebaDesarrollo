using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAdmBanco", Schema = "Administracion")]
    public partial class tblAdmBanco
    {
        public tblAdmBanco()
        {
            tblAdmCuentaBancaria = new HashSet<tblAdmCuentaBancaria>();
        }

        [Key]
        public byte idAdmBanco { get; set; }
        public string? denominacion { get; set; }

        [InverseProperty("idAdmBancoNavigation")]
        public virtual ICollection<tblAdmCuentaBancaria> tblAdmCuentaBancaria { get; set; }
    }
}
