using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblGrupoEmpresarial", Schema = "Finanzas")]
    public partial class tblGrupoEmpresarial
    {
        public tblGrupoEmpresarial()
        {
            tblDatosFinancieros = new HashSet<tblDatosFinancieros>();
        }

        [Key]
        public byte idGrupoEmpresarial { get; set; }
        public string denominacion { get; set; } = null!;

        [InverseProperty("idGrupoEmpresarialNavigation")]
        public virtual ICollection<tblDatosFinancieros> tblDatosFinancieros { get; set; }
    }
}
