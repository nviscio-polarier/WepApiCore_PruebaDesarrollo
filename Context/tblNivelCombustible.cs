using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblNivelCombustible", Schema = "Logistica")]
    public partial class tblNivelCombustible
    {
        public tblNivelCombustible()
        {
            tblParteTransporte = new HashSet<tblParteTransporte>();
        }

        [Key]
        public int idNivelCombustible { get; set; }
        public string estadoCombustible { get; set; } = null!;

        [InverseProperty("idNivelCombustibleNavigation")]
        public virtual ICollection<tblParteTransporte> tblParteTransporte { get; set; }
    }
}
