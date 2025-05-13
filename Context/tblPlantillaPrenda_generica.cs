using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPlantillaPrenda_generica", Schema = "General")]
    public partial class tblPlantillaPrenda_generica
    {
        public tblPlantillaPrenda_generica()
        {
            tblPlantillaPrenda_generica_historico_peso = new HashSet<tblPlantillaPrenda_generica_historico_peso>();
            tblPrenda = new HashSet<tblPrenda>();
            tblPrendaNAlmacenNInventario = new HashSet<tblPrendaNAlmacenNInventario>();
            tblPrendaNGestionRetiro = new HashSet<tblPrendaNGestionRetiro>();
            tblPrendaNInventario = new HashSet<tblPrendaNInventario>();
            tblPrendaNMovimiento = new HashSet<tblPrendaNMovimiento>();
            tblPrendaNProduccion = new HashSet<tblPrendaNProduccion>();
        }

        [Key]
        public int idPlantillaPrenda_generica { get; set; }
        public byte idGrupoPlantillaPrenda_generica { get; set; }
        public string? denominacion { get; set; }
        public short idDenoPrenda { get; set; }
        [StringLength(10)]
        public string codigoDenoPrenda { get; set; } = null!;
        public bool? activo { get; set; }
        public bool eliminado { get; set; }
        public byte? idColorTapa { get; set; }
        public byte? idMarcaTapa { get; set; }
        public short? udsAlmacenaje { get; set; }
        public byte? elementoReparto { get; set; }
        public short? udsXBacReparto { get; set; }
        public byte? elementoOffice { get; set; }
        public short? udsXBacOffice { get; set; }
        public byte? elementoPedido { get; set; }
        public short? udsXBacPedido { get; set; }
        public short? elementoProduccion { get; set; }
        public short? udsXBacProduccion { get; set; }

        [ForeignKey("elementoOffice")]
        [InverseProperty("tblPlantillaPrenda_genericaelementoOfficeNavigation")]
        public virtual tblElemTrans? elementoOfficeNavigation { get; set; }
        [ForeignKey("elementoPedido")]
        [InverseProperty("tblPlantillaPrenda_genericaelementoPedidoNavigation")]
        public virtual tblElemTrans? elementoPedidoNavigation { get; set; }
        [ForeignKey("elementoReparto")]
        [InverseProperty("tblPlantillaPrenda_genericaelementoRepartoNavigation")]
        public virtual tblElemTrans? elementoRepartoNavigation { get; set; }
        [ForeignKey("idColorTapa")]
        [InverseProperty("tblPlantillaPrenda_generica")]
        public virtual tblColorTapa? idColorTapaNavigation { get; set; }
        [ForeignKey("idDenoPrenda")]
        [InverseProperty("tblPlantillaPrenda_generica")]
        public virtual tblDenoPrenda idDenoPrendaNavigation { get; set; } = null!;
        [ForeignKey("idGrupoPlantillaPrenda_generica")]
        [InverseProperty("tblPlantillaPrenda_generica")]
        public virtual tblGrupoPlantillaPrenda_generica idGrupoPlantillaPrenda_genericaNavigation { get; set; } = null!;
        [ForeignKey("idMarcaTapa")]
        [InverseProperty("tblPlantillaPrenda_generica")]
        public virtual tblMarcaTapa? idMarcaTapaNavigation { get; set; }
        [InverseProperty("idPlantillaPrenda_genericaNavigation")]
        public virtual ICollection<tblPlantillaPrenda_generica_historico_peso> tblPlantillaPrenda_generica_historico_peso { get; set; }
        [InverseProperty("idPlantillaPrenda_genericaNavigation")]
        public virtual ICollection<tblPrenda> tblPrenda { get; set; }
        [InverseProperty("idPlantillaPrenda_genericaNavigation")]
        public virtual ICollection<tblPrendaNAlmacenNInventario> tblPrendaNAlmacenNInventario { get; set; }
        [InverseProperty("idPlantillaPrenda_genericaNavigation")]
        public virtual ICollection<tblPrendaNGestionRetiro> tblPrendaNGestionRetiro { get; set; }
        [InverseProperty("idPlantillaPrenda_genericaNavigation")]
        public virtual ICollection<tblPrendaNInventario> tblPrendaNInventario { get; set; }
        [InverseProperty("idPlantillaPrenda_genericaNavigation")]
        public virtual ICollection<tblPrendaNMovimiento> tblPrendaNMovimiento { get; set; }
        [InverseProperty("idPlantillaPrenda_genericaNavigation")]
        public virtual ICollection<tblPrendaNProduccion> tblPrendaNProduccion { get; set; }
    }
}
