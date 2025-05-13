using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAdmCentroBeneficio", Schema = "Administracion")]
    public partial class tblAdmCentroBeneficio
    {
        public tblAdmCentroBeneficio()
        {
            tblAdmElementoPEP = new HashSet<tblAdmElementoPEP>();
        }

        [Key]
        public int idAdmCentroBeneficio { get; set; }
        public string? codigo { get; set; }
        public string? denominacion { get; set; }

        [InverseProperty("idAdmCentroBeneficioNavigation")]
        public virtual ICollection<tblAdmElementoPEP> tblAdmElementoPEP { get; set; }
    }
}
