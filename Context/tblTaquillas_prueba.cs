using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTaquillas_prueba", Schema = "MyRealBonus")]
    public partial class tblTaquillas_prueba
    {
        [Key]
        public int idTaquilla { get; set; }
        public int idLavanderia { get; set; }
        public string denominacion { get; set; }
        public int tamaño { get; set; }

        [ForeignKey("idLavanderia")]
        [InverseProperty("tblTaquillas_prueba")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
    }
}

