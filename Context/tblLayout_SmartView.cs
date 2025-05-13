using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblLayout_SmartView", Schema = "GestionInterna")]
    public partial class tblLayout_SmartView
    {
        [Key]
        public int idLayout_SmartView { get; set; }
        public int? idLavanderia { get; set; }
        public int? idMaquina { get; set; }
        public byte? idAreaLavanderia { get; set; }
        public byte? x { get; set; }
        public byte? y { get; set; }
        public byte? width { get; set; }
        public byte height { get; set; }

        [ForeignKey("idAreaLavanderia")]
        [InverseProperty("tblLayout_SmartView")]
        public virtual tblAreaLavanderia? idAreaLavanderiaNavigation { get; set; }
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblLayout_SmartView")]
        public virtual tblLavanderia? idLavanderiaNavigation { get; set; }
        [ForeignKey("idMaquina")]
        [InverseProperty("tblLayout_SmartView")]
        public virtual tblMaquina? idMaquinaNavigation { get; set; }
    }
}
