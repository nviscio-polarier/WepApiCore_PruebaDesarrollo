using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAlmacenRecambiosNPersona", Schema = "Assistant")]
    public partial class tblAlmacenRecambiosNPersona
    {
        [Key]
        public int idAlmacen { get; set; }
        [Key]
        public int idPersona { get; set; }
        public bool isConsulta { get; set; }
        public bool isSalida { get; set; }

        [ForeignKey("idAlmacen")]
        [InverseProperty("tblAlmacenRecambiosNPersona")]
        public virtual tblAlmacenRecambios idAlmacenNavigation { get; set; } = null!;
        [ForeignKey("idPersona")]
        [InverseProperty("tblAlmacenRecambiosNPersona")]
        public virtual tblPersona idPersonaNavigation { get; set; } = null!;
    }
}
