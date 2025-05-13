using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPrenda", Schema = "General")]
    [Index("idDenoPrenda", Name = "IX_tblPrenda_idDenoPrenda")]
    public partial class tblPrenda
    {
        public tblPrenda()
        {
            tblDocumentoPrenda = new HashSet<tblDocumentoPrenda>();
            tblPrecioLavadoPrenda = new HashSet<tblPrecioLavadoPrenda>();
            tblPrendaNAbono = new HashSet<tblPrendaNAbono>();
            tblPrendaNAlmacen = new HashSet<tblPrendaNAlmacen>();
            tblPrendaNAlmacenNInventario = new HashSet<tblPrendaNAlmacenNInventario>();
            tblPrendaNEntidad_NuevoPedido = new HashSet<tblPrendaNEntidad_NuevoPedido>();
            tblPrendaNGestionRetiro = new HashSet<tblPrendaNGestionRetiro>();
            tblPrendaNInventario = new HashSet<tblPrendaNInventario>();
            tblPrendaNLavanderia = new HashSet<tblPrendaNLavanderia>();
            tblPrendaNMovimiento = new HashSet<tblPrendaNMovimiento>();
            tblPrendaNMuestreo = new HashSet<tblPrendaNMuestreo>();
            tblPrendaNMuestreo_FS = new HashSet<tblPrendaNMuestreo_FS>();
            tblPrendaNPedido = new HashSet<tblPrendaNPedido>();
            tblPrendaNPedidoExtra = new HashSet<tblPrendaNPedidoExtra>();
            tblPrendaNProduccion = new HashSet<tblPrendaNProduccion>();
            tblPrendaNReparto = new HashSet<tblPrendaNReparto>();
            tblPrendaNRepartoOffice = new HashSet<tblPrendaNRepartoOffice>();
            tblPrendaNRevision = new HashSet<tblPrendaNRevision>();
            tblPrendaNSolicitudAbono = new HashSet<tblPrendaNSolicitudAbono>();
            tblPrendaNSubAlmacen = new HashSet<tblPrendaNSubAlmacen>();
            tblPrendaNTipoHabitacion = new HashSet<tblPrendaNTipoHabitacion>();
            tblPrendaNUsuarioNEntidad = new HashSet<tblPrendaNUsuarioNEntidad>();
            tblPrenda_historico_fechaValoracion = new HashSet<tblPrenda_historico_fechaValoracion>();
            tblPrenda_historico_idTipoFacturacion = new HashSet<tblPrenda_historico_idTipoFacturacion>();
            tblPrenda_historico_peso = new HashSet<tblPrenda_historico_peso>();
            tblProduccionMaquinaNPrenda = new HashSet<tblProduccionMaquinaNPrenda>();
            idMaquina = new HashSet<tblMaquina>();
        }

        [Key]
        public int idPrenda { get; set; }
        [StringLength(10)]
        public string codigoPrenda { get; set; } = null!;
        public string? denominacion { get; set; }
        public short? udsXBacReparto { get; set; }
        public short? udsXBacOffice { get; set; }
        public int peso { get; set; }
        [Column(TypeName = "decimal(9, 4)")]
        public decimal? precioLavado { get; set; }
        public int? idCompañia { get; set; }
        public int? idEntidad { get; set; }
        public short idDenoPrenda { get; set; }
        [StringLength(10)]
        public string? idxGsbs { get; set; }
        public int idLavanderia { get; set; }
        public bool? activo { get; set; }
        public byte? elementoReparto { get; set; }
        public byte? elementoOffice { get; set; }
        public byte? tipoFact { get; set; }
        public short? udsAlmacenaje { get; set; }
        public byte? idColorTapa { get; set; }
        public short? udsXBacPedido { get; set; }
        public byte? elementoPedido { get; set; }
        [Column(TypeName = "decimal(9, 4)")]
        public decimal? coste { get; set; }
        public int? parStock { get; set; }
        public short? elementoProduccion { get; set; }
        public short? udsXBacProduccion { get; set; }
        public byte? idMarcaTapa { get; set; }
        public bool eliminado { get; set; }
        public bool isExtra { get; set; }
        public byte? idFamiliaFacturacion { get; set; }
        [Column(TypeName = "decimal(4, 3)")]
        public decimal? percPrecioDesmanche { get; set; }
        public bool excluirSmartHub { get; set; }
        public int? idPlantillaPrenda_generica { get; set; }

        [ForeignKey("elementoOffice")]
        [InverseProperty("tblPrendaelementoOfficeNavigation")]
        public virtual tblElemTrans? elementoOfficeNavigation { get; set; }
        [ForeignKey("elementoPedido")]
        [InverseProperty("tblPrendaelementoPedidoNavigation")]
        public virtual tblElemTrans? elementoPedidoNavigation { get; set; }
        [ForeignKey("elementoReparto")]
        [InverseProperty("tblPrendaelementoRepartoNavigation")]
        public virtual tblElemTrans? elementoRepartoNavigation { get; set; }
        [ForeignKey("idColorTapa")]
        [InverseProperty("tblPrenda")]
        public virtual tblColorTapa? idColorTapaNavigation { get; set; }
        [ForeignKey("idCompañia")]
        [InverseProperty("tblPrenda")]
        public virtual tblCompañia? idCompañiaNavigation { get; set; }
        [ForeignKey("idDenoPrenda")]
        [InverseProperty("tblPrenda")]
        public virtual tblDenoPrenda idDenoPrendaNavigation { get; set; } = null!;
        [ForeignKey("idEntidad")]
        [InverseProperty("tblPrenda")]
        public virtual tblEntidad? idEntidadNavigation { get; set; }
        [ForeignKey("idFamiliaFacturacion")]
        [InverseProperty("tblPrenda")]
        public virtual tblFamilia? idFamiliaFacturacionNavigation { get; set; }
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblPrenda")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
        [ForeignKey("idMarcaTapa")]
        [InverseProperty("tblPrenda")]
        public virtual tblMarcaTapa? idMarcaTapaNavigation { get; set; }
        [ForeignKey("idPlantillaPrenda_generica")]
        [InverseProperty("tblPrenda")]
        public virtual tblPlantillaPrenda_generica? idPlantillaPrenda_genericaNavigation { get; set; }
        [ForeignKey("tipoFact")]
        [InverseProperty("tblPrenda")]
        public virtual tblTipoFacturacion? tipoFactNavigation { get; set; }
        [InverseProperty("idPrendaNavigation")]
        public virtual tblPrendaPrecioRefact tblPrendaPrecioRefact { get; set; } = null!;
        [InverseProperty("idPrendaNavigation")]
        public virtual ICollection<tblDocumentoPrenda> tblDocumentoPrenda { get; set; }
        [InverseProperty("idPrendaNavigation")]
        public virtual ICollection<tblPrecioLavadoPrenda> tblPrecioLavadoPrenda { get; set; }
        [InverseProperty("idPrendaNavigation")]
        public virtual ICollection<tblPrendaNAbono> tblPrendaNAbono { get; set; }
        [InverseProperty("idPrendaNavigation")]
        public virtual ICollection<tblPrendaNAlmacen> tblPrendaNAlmacen { get; set; }
        [InverseProperty("idPrendaNavigation")]
        public virtual ICollection<tblPrendaNAlmacenNInventario> tblPrendaNAlmacenNInventario { get; set; }
        [InverseProperty("idPrendaNavigation")]
        public virtual ICollection<tblPrendaNEntidad_NuevoPedido> tblPrendaNEntidad_NuevoPedido { get; set; }
        [InverseProperty("idPrendaNavigation")]
        public virtual ICollection<tblPrendaNGestionRetiro> tblPrendaNGestionRetiro { get; set; }
        [InverseProperty("idPrendaNavigation")]
        public virtual ICollection<tblPrendaNInventario> tblPrendaNInventario { get; set; }
        [InverseProperty("idPrendaNavigation")]
        public virtual ICollection<tblPrendaNLavanderia> tblPrendaNLavanderia { get; set; }
        [InverseProperty("idPrendaNavigation")]
        public virtual ICollection<tblPrendaNMovimiento> tblPrendaNMovimiento { get; set; }
        [InverseProperty("idPrendaNavigation")]
        public virtual ICollection<tblPrendaNMuestreo> tblPrendaNMuestreo { get; set; }
        [InverseProperty("idPrendaNavigation")]
        public virtual ICollection<tblPrendaNMuestreo_FS> tblPrendaNMuestreo_FS { get; set; }
        [InverseProperty("idPrendaNavigation")]
        public virtual ICollection<tblPrendaNPedido> tblPrendaNPedido { get; set; }
        [InverseProperty("idPrendaNavigation")]
        public virtual ICollection<tblPrendaNPedidoExtra> tblPrendaNPedidoExtra { get; set; }
        [InverseProperty("idPrendaNavigation")]
        public virtual ICollection<tblPrendaNProduccion> tblPrendaNProduccion { get; set; }
        [InverseProperty("idPrendaNavigation")]
        public virtual ICollection<tblPrendaNReparto> tblPrendaNReparto { get; set; }
        [InverseProperty("idPrendaNavigation")]
        public virtual ICollection<tblPrendaNRepartoOffice> tblPrendaNRepartoOffice { get; set; }
        [InverseProperty("idPrendaNavigation")]
        public virtual ICollection<tblPrendaNRevision> tblPrendaNRevision { get; set; }
        [InverseProperty("idPrendaNavigation")]
        public virtual ICollection<tblPrendaNSolicitudAbono> tblPrendaNSolicitudAbono { get; set; }
        [InverseProperty("idPrendaNavigation")]
        public virtual ICollection<tblPrendaNSubAlmacen> tblPrendaNSubAlmacen { get; set; }
        [InverseProperty("idPrendaNavigation")]
        public virtual ICollection<tblPrendaNTipoHabitacion> tblPrendaNTipoHabitacion { get; set; }
        [InverseProperty("idPrendaNavigation")]
        public virtual ICollection<tblPrendaNUsuarioNEntidad> tblPrendaNUsuarioNEntidad { get; set; }
        [InverseProperty("idPrendaNavigation")]
        public virtual ICollection<tblPrenda_historico_fechaValoracion> tblPrenda_historico_fechaValoracion { get; set; }
        [InverseProperty("idPrendaNavigation")]
        public virtual ICollection<tblPrenda_historico_idTipoFacturacion> tblPrenda_historico_idTipoFacturacion { get; set; }
        [InverseProperty("idPrendaNavigation")]
        public virtual ICollection<tblPrenda_historico_peso> tblPrenda_historico_peso { get; set; }
        [InverseProperty("idPrendaNavigation")]
        public virtual ICollection<tblProduccionMaquinaNPrenda> tblProduccionMaquinaNPrenda { get; set; }

        [ForeignKey("idPrenda")]
        [InverseProperty("idPrenda")]
        public virtual ICollection<tblMaquina> idMaquina { get; set; }
    }
}
