using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblGrupoPlantillaPrenda_generica", Schema = "General")]
    public partial class tblGrupoPlantillaPrenda_generica
    {
        public tblGrupoPlantillaPrenda_generica()
        {
            tblClienteNMaquina = new HashSet<tblClienteNMaquina>();
            tblGestionRetiro = new HashSet<tblGestionRetiro>();
            tblKgLavadosLavadora = new HashSet<tblKgLavadosLavadora>();
            tblKgLavadosTunel = new HashSet<tblKgLavadosTunel>();
            tblMovimiento = new HashSet<tblMovimiento>();
            tblPlantillaPrenda_generica = new HashSet<tblPlantillaPrenda_generica>();
        }

        [Key]
        public byte idGrupoPlantillaPrenda_generica { get; set; }
        public int idCorporacion { get; set; }
        public string? denominacion { get; set; }

        [ForeignKey("idCorporacion")]
        [InverseProperty("tblGrupoPlantillaPrenda_generica")]
        public virtual tblCorporacion idCorporacionNavigation { get; set; } = null!;
        [InverseProperty("idGrupoPlantillaPrenda_genericaNavigation")]
        public virtual ICollection<tblClienteNMaquina> tblClienteNMaquina { get; set; }
        [InverseProperty("idGrupoPlantillaPrenda_genericaNavigation")]
        public virtual ICollection<tblGestionRetiro> tblGestionRetiro { get; set; }
        [InverseProperty("idGrupoPlantillaPrenda_genericaNavigation")]
        public virtual ICollection<tblKgLavadosLavadora> tblKgLavadosLavadora { get; set; }
        [InverseProperty("idGrupoPlantillaPrenda_genericaNavigation")]
        public virtual ICollection<tblKgLavadosTunel> tblKgLavadosTunel { get; set; }
        [InverseProperty("idGrupoPlantillaPrenda_genericaNavigation")]
        public virtual ICollection<tblMovimiento> tblMovimiento { get; set; }
        [InverseProperty("idGrupoPlantillaPrenda_genericaNavigation")]
        public virtual ICollection<tblPlantillaPrenda_generica> tblPlantillaPrenda_generica { get; set; }
    }
}
