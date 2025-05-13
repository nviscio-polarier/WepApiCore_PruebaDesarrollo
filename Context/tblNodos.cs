using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblNodos", Schema = "MyRealData")]
    public partial class tblNodos
    {
        [Key]
        public short idNodo { get; set; }
        public int idLavanderia { get; set; }
        public string tipoNodo { get; set; } = null!;
        public short? xPos { get; set; }
        public short? yPos { get; set; }
        public int? idMaquina { get; set; }
        public string? classes { get; set; }
        public int? numPos { get; set; }
        public byte? orden { get; set; }
        public byte? col_md { get; set; }
        public byte? col_xl { get; set; }

        [ForeignKey("idLavanderia")]
        [InverseProperty("tblNodos")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
        [ForeignKey("idMaquina")]
        [InverseProperty("tblNodos")]
        public virtual tblMaquina? idMaquinaNavigation { get; set; }
    }
}
