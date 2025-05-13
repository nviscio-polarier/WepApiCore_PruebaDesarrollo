using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCategoriaConvenio", Schema = "RRHH")]
    public partial class tblCategoriaConvenio
    {
        public tblCategoriaConvenio()
        {
            tblCategoriaInterna = new HashSet<tblCategoriaInterna>();
        }

        [Key]
        public int idCategoriaConvenio { get; set; }
        public string denominacion { get; set; } = null!;
        public bool isOficina { get; set; }
        public int? idPais { get; set; }

        [ForeignKey("idPais")]
        [InverseProperty("tblCategoriaConvenio")]
        public virtual tblPais? idPaisNavigation { get; set; }
        [InverseProperty("idCategoriaConvenioNavigation")]
        public virtual ICollection<tblCategoriaInterna> tblCategoriaInterna { get; set; }
    }
}
