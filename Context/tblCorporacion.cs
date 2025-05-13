using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCorporacion", Schema = "General")]
    public partial class tblCorporacion
    {
        public tblCorporacion()
        {
            tblGrupoPlantillaPrenda_generica = new HashSet<tblGrupoPlantillaPrenda_generica>();
            tblLavanderia = new HashSet<tblLavanderia>();
        }

        [Key]
        public int idCorporacion { get; set; }
        public string denominacion { get; set; } = null!;

        [InverseProperty("idCorporacionNavigation")]
        public virtual ICollection<tblGrupoPlantillaPrenda_generica> tblGrupoPlantillaPrenda_generica { get; set; }
        [InverseProperty("idCorporacionNavigation")]
        public virtual ICollection<tblLavanderia> tblLavanderia { get; set; }
    }
}
