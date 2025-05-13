using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCentroTrabajo", Schema = "General")]
    public partial class tblCentroTrabajo
    {
        public tblCentroTrabajo()
        {
            tblAdmPedidoProveedor = new HashSet<tblAdmPedidoProveedor>();
            tblCalendarioCentroTrabajo = new HashSet<tblCalendarioCentroTrabajo>();
            tblConfiguracionSalarial = new HashSet<tblConfiguracionSalarial>();
            tblCorreoAltaGestoriaNCentroLav = new HashSet<tblCorreoAltaGestoriaNCentroLav>();
            tblLlamamiento = new HashSet<tblLlamamiento>();
            tblPersona = new HashSet<tblPersona>();
            idUsuario = new HashSet<tblUsuario>();
        }

        [Key]
        public int idCentroTrabajo { get; set; }
        public string denominacion { get; set; } = null!;
        public int? idPais { get; set; }
        public byte? idMoneda { get; set; }
        public string? direccion { get; set; }

        [ForeignKey("idPais")]
        [InverseProperty("tblCentroTrabajo")]
        public virtual tblPais? idPaisNavigation { get; set; }
        [InverseProperty("idCentroTrabajoNavigation")]
        public virtual tblCuentaContableNCentroTrabajo tblCuentaContableNCentroTrabajo { get; set; } = null!;
        [InverseProperty("idCentroTrabajoNavigation")]
        public virtual ICollection<tblAdmPedidoProveedor> tblAdmPedidoProveedor { get; set; }
        [InverseProperty("idCentroTrabajoNavigation")]
        public virtual ICollection<tblCalendarioCentroTrabajo> tblCalendarioCentroTrabajo { get; set; }
        [InverseProperty("idCentroTrabajoNavigation")]
        public virtual ICollection<tblConfiguracionSalarial> tblConfiguracionSalarial { get; set; }
        [InverseProperty("idCentroTrabajoNavigation")]
        public virtual ICollection<tblCorreoAltaGestoriaNCentroLav> tblCorreoAltaGestoriaNCentroLav { get; set; }
        [InverseProperty("idCentroTrabajoNavigation")]
        public virtual ICollection<tblLlamamiento> tblLlamamiento { get; set; }
        [InverseProperty("idCentroTrabajoNavigation")]
        public virtual ICollection<tblPersona> tblPersona { get; set; }

        [ForeignKey("idCentroTrabajo")]
        [InverseProperty("idCentroTrabajo")]
        public virtual ICollection<tblUsuario> idUsuario { get; set; }
    }
}
