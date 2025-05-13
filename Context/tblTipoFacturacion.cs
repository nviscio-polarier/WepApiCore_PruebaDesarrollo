using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoFacturacion", Schema = "General")]
    public partial class tblTipoFacturacion
    {
        public tblTipoFacturacion()
        {
            tblPrenda = new HashSet<tblPrenda>();
            tblPrenda_historico_idTipoFacturacion = new HashSet<tblPrenda_historico_idTipoFacturacion>();
        }

        [StringLength(50)]
        public string denominacion { get; set; } = null!;
        [Key]
        public byte idTipoFacturacion { get; set; }
        public string? icon { get; set; }

        [InverseProperty("tipoFactNavigation")]
        public virtual ICollection<tblPrenda> tblPrenda { get; set; }
        [InverseProperty("idTipoFacturacionNavigation")]
        public virtual ICollection<tblPrenda_historico_idTipoFacturacion> tblPrenda_historico_idTipoFacturacion { get; set; }
    }
}
