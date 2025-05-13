using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoMaquina", Schema = "Maquinaria")]
    public partial class tblTipoMaquina
    {
        public tblTipoMaquina()
        {
            tblTipoMaquinaNCategoriaMaquina = new HashSet<tblTipoMaquinaNCategoriaMaquina>();
        }

        [Key]
        public short idTipoMaquina { get; set; }
        public string? denominacion { get; set; }
        [StringLength(3)]
        public string? codigo { get; set; }

        [InverseProperty("idTipoMaquinaNavigation")]
        public virtual ICollection<tblTipoMaquinaNCategoriaMaquina> tblTipoMaquinaNCategoriaMaquina { get; set; }
    }
}
