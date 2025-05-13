using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblGestionRetiro", Schema = "MyQuality")]
    public partial class tblGestionRetiro
    {
        public tblGestionRetiro()
        {
            tblPrendaNGestionRetiro = new HashSet<tblPrendaNGestionRetiro>();
        }

        [Key]
        public int idGestionRetiro { get; set; }
        public DateTimeOffset? fechaReg { get; set; }
        public int? idUsuario { get; set; }
        public bool? isValidado { get; set; }
        public byte idAreaLavanderia { get; set; }
        public int? idMaquina { get; set; }
        public int? idCompañia { get; set; }
        public string? observaciones { get; set; }
        public DateTimeOffset? fechaValidacion { get; set; }
        public byte? idFamilia { get; set; }
        public int? idLavanderia { get; set; }
        public int? idUsuarioValidador { get; set; }
        public int? idEntidad { get; set; }
        public byte? idGrupoPlantillaPrenda_generica { get; set; }

        [ForeignKey("idAreaLavanderia")]
        [InverseProperty("tblGestionRetiro")]
        public virtual tblAreaLavanderia idAreaLavanderiaNavigation { get; set; } = null!;
        [ForeignKey("idCompañia")]
        [InverseProperty("tblGestionRetiro")]
        public virtual tblCompañia? idCompañiaNavigation { get; set; }
        [ForeignKey("idEntidad")]
        [InverseProperty("tblGestionRetiro")]
        public virtual tblEntidad? idEntidadNavigation { get; set; }
        [ForeignKey("idFamilia")]
        [InverseProperty("tblGestionRetiro")]
        public virtual tblFamilia? idFamiliaNavigation { get; set; }
        [ForeignKey("idGrupoPlantillaPrenda_generica")]
        [InverseProperty("tblGestionRetiro")]
        public virtual tblGrupoPlantillaPrenda_generica? idGrupoPlantillaPrenda_genericaNavigation { get; set; }
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblGestionRetiro")]
        public virtual tblLavanderia? idLavanderiaNavigation { get; set; }
        [ForeignKey("idMaquina")]
        [InverseProperty("tblGestionRetiro")]
        public virtual tblMaquina? idMaquinaNavigation { get; set; }
        [ForeignKey("idUsuario")]
        [InverseProperty("tblGestionRetiroidUsuarioNavigation")]
        public virtual tblUsuario? idUsuarioNavigation { get; set; }
        [ForeignKey("idUsuarioValidador")]
        [InverseProperty("tblGestionRetiroidUsuarioValidadorNavigation")]
        public virtual tblUsuario? idUsuarioValidadorNavigation { get; set; }
        [InverseProperty("idGestionRetiroNavigation")]
        public virtual ICollection<tblPrendaNGestionRetiro> tblPrendaNGestionRetiro { get; set; }
    }
}
