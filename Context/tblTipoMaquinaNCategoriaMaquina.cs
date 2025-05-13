using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoMaquinaNCategoriaMaquina", Schema = "Maquinaria")]
    public partial class tblTipoMaquinaNCategoriaMaquina
    {
        public tblTipoMaquinaNCategoriaMaquina()
        {
            tblMaquina = new HashSet<tblMaquina>();
        }

        public short idTipoMaquina { get; set; }
        public int idCategoriaMaquina { get; set; }
        [Key]
        public int idTipoMaquinaNCategoriaMaquina { get; set; }

        [ForeignKey("idCategoriaMaquina")]
        [InverseProperty("tblTipoMaquinaNCategoriaMaquina")]
        public virtual tblCategoriaMaquina idCategoriaMaquinaNavigation { get; set; } = null!;
        [ForeignKey("idTipoMaquina")]
        [InverseProperty("tblTipoMaquinaNCategoriaMaquina")]
        public virtual tblTipoMaquina idTipoMaquinaNavigation { get; set; } = null!;
        [InverseProperty("idTipoMaquinaNCategoriaMaquinaNavigation")]
        public virtual ICollection<tblMaquina> tblMaquina { get; set; }
    }
}
