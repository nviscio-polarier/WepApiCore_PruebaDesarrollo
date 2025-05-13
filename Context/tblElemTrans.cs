using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblElemTrans", Schema = "General")]
    public partial class tblElemTrans
    {
        public tblElemTrans()
        {
            tblPlantillaPrenda_genericaelementoOfficeNavigation = new HashSet<tblPlantillaPrenda_generica>();
            tblPlantillaPrenda_genericaelementoPedidoNavigation = new HashSet<tblPlantillaPrenda_generica>();
            tblPlantillaPrenda_genericaelementoRepartoNavigation = new HashSet<tblPlantillaPrenda_generica>();
            tblPrendaelementoOfficeNavigation = new HashSet<tblPrenda>();
            tblPrendaelementoPedidoNavigation = new HashSet<tblPrenda>();
            tblPrendaelementoRepartoNavigation = new HashSet<tblPrenda>();
        }

        [Key]
        public byte idElemTrans { get; set; }
        [StringLength(5)]
        public string? codigo { get; set; }
        [StringLength(50)]
        public string denominacion { get; set; } = null!;

        [InverseProperty("elementoOfficeNavigation")]
        public virtual ICollection<tblPlantillaPrenda_generica> tblPlantillaPrenda_genericaelementoOfficeNavigation { get; set; }
        [InverseProperty("elementoPedidoNavigation")]
        public virtual ICollection<tblPlantillaPrenda_generica> tblPlantillaPrenda_genericaelementoPedidoNavigation { get; set; }
        [InverseProperty("elementoRepartoNavigation")]
        public virtual ICollection<tblPlantillaPrenda_generica> tblPlantillaPrenda_genericaelementoRepartoNavigation { get; set; }
        [InverseProperty("elementoOfficeNavigation")]
        public virtual ICollection<tblPrenda> tblPrendaelementoOfficeNavigation { get; set; }
        [InverseProperty("elementoPedidoNavigation")]
        public virtual ICollection<tblPrenda> tblPrendaelementoPedidoNavigation { get; set; }
        [InverseProperty("elementoRepartoNavigation")]
        public virtual ICollection<tblPrenda> tblPrendaelementoRepartoNavigation { get; set; }
    }
}
