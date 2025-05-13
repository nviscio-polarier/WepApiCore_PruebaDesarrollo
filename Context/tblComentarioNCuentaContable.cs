using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblComentarioNCuentaContable", Schema = "ControlPresupuestario")]
    public partial class tblComentarioNCuentaContable
    {
        [Key]
        public int idComentarioNCuentaContable { get; set; }
        public int idAdmCuentaContable { get; set; }
        public int? idAdmCentroCoste { get; set; }
        public int? idAdmElementoPEP { get; set; }
        public string comentario { get; set; } = null!;
        public int? idUsuario { get; set; }
        [Precision(0)]
        public DateTimeOffset? fecha { get; set; }

        [ForeignKey("idAdmCentroCoste")]
        [InverseProperty("tblComentarioNCuentaContable")]
        public virtual tblAdmCentroCoste? idAdmCentroCosteNavigation { get; set; }
        [ForeignKey("idAdmCuentaContable")]
        [InverseProperty("tblComentarioNCuentaContable")]
        public virtual tblAdmCuentaContable idAdmCuentaContableNavigation { get; set; } = null!;
        [ForeignKey("idAdmElementoPEP")]
        [InverseProperty("tblComentarioNCuentaContable")]
        public virtual tblAdmElementoPEP? idAdmElementoPEPNavigation { get; set; }
        [ForeignKey("idUsuario")]
        [InverseProperty("tblComentarioNCuentaContable")]
        public virtual tblUsuario? idUsuarioNavigation { get; set; }
    }
}
