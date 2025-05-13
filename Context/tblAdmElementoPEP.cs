using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAdmElementoPEP", Schema = "Administracion")]
    public partial class tblAdmElementoPEP
    {
        public tblAdmElementoPEP()
        {
            InverseidAdmElementoPEPPadreNavigation = new HashSet<tblAdmElementoPEP>();
            tblAdmAlbaranCompra = new HashSet<tblAdmAlbaranCompra>();
            tblAdmAlbaranVenta = new HashSet<tblAdmAlbaranVenta>();
            tblAdmCliente = new HashSet<tblAdmCliente>();
            tblAdmFacturaCompra = new HashSet<tblAdmFacturaCompra>();
            tblAdmFacturaVenta = new HashSet<tblAdmFacturaVenta>();
            tblAdmPedidoCliente = new HashSet<tblAdmPedidoCliente>();
            tblAdmPedidoProveedor = new HashSet<tblAdmPedidoProveedor>();
            tblAjustePresupuestario = new HashSet<tblAjustePresupuestario>();
            tblCierrePresupuestario = new HashSet<tblCierrePresupuestario>();
            tblComentarioNCuentaContable = new HashSet<tblComentarioNCuentaContable>();
            tblHistoricoAsientoNomina = new HashSet<tblHistoricoAsientoNomina>();
            tblHistoricoAsientoNomina_MX = new HashSet<tblHistoricoAsientoNomina_MX>();
            tblHistoricoAsientoNomina_RD = new HashSet<tblHistoricoAsientoNomina_RD>();
            tblHistoricoPartidaContable = new HashSet<tblHistoricoPartidaContable>();
            tblHistoricoPlanificacion = new HashSet<tblHistoricoPlanificacion>();
            tblLavanderia = new HashSet<tblLavanderia>();
            tblNomina = new HashSet<tblNomina>();
            tblNomina_MX = new HashSet<tblNomina_MX>();
            tblNomina_RD = new HashSet<tblNomina_RD>();
            tblPersona = new HashSet<tblPersona>();
            tblPresupuestoKg = new HashSet<tblPresupuestoKg>();
            idUsuario = new HashSet<tblUsuario>();
        }

        [Key]
        public int idAdmElementoPEP { get; set; }
        public string? denominacion { get; set; }
        public string? codigo { get; set; }
        public short? idEmpresaPolarier { get; set; }
        public bool? isEliminado { get; set; }
        public int? idAdmCentroBeneficio { get; set; }
        public int? idAdmElementoPEPPadre { get; set; }
        public string? workplaceCode_A3 { get; set; }

        [ForeignKey("idAdmCentroBeneficio")]
        [InverseProperty("tblAdmElementoPEP")]
        public virtual tblAdmCentroBeneficio? idAdmCentroBeneficioNavigation { get; set; }
        [ForeignKey("idAdmElementoPEPPadre")]
        [InverseProperty("InverseidAdmElementoPEPPadreNavigation")]
        public virtual tblAdmElementoPEP? idAdmElementoPEPPadreNavigation { get; set; }
        [ForeignKey("idEmpresaPolarier")]
        [InverseProperty("tblAdmElementoPEP")]
        public virtual tblEmpresasPolarier? idEmpresaPolarierNavigation { get; set; }
        [InverseProperty("idAdmElementoPEPPadreNavigation")]
        public virtual ICollection<tblAdmElementoPEP> InverseidAdmElementoPEPPadreNavigation { get; set; }
        [InverseProperty("idAdmElementoPEPNavigation")]
        public virtual ICollection<tblAdmAlbaranCompra> tblAdmAlbaranCompra { get; set; }
        [InverseProperty("idAdmElementoPEPNavigation")]
        public virtual ICollection<tblAdmAlbaranVenta> tblAdmAlbaranVenta { get; set; }
        [InverseProperty("idAdmElementoPEPNavigation")]
        public virtual ICollection<tblAdmCliente> tblAdmCliente { get; set; }
        [InverseProperty("idAdmElementoPEPNavigation")]
        public virtual ICollection<tblAdmFacturaCompra> tblAdmFacturaCompra { get; set; }
        [InverseProperty("idAdmElementoPEPNavigation")]
        public virtual ICollection<tblAdmFacturaVenta> tblAdmFacturaVenta { get; set; }
        [InverseProperty("idAdmElementoPEPNavigation")]
        public virtual ICollection<tblAdmPedidoCliente> tblAdmPedidoCliente { get; set; }
        [InverseProperty("idAdmElementoPEPNavigation")]
        public virtual ICollection<tblAdmPedidoProveedor> tblAdmPedidoProveedor { get; set; }
        [InverseProperty("idAdmElementoPEPNavigation")]
        public virtual ICollection<tblAjustePresupuestario> tblAjustePresupuestario { get; set; }
        [InverseProperty("idAdmElementoPEPNavigation")]
        public virtual ICollection<tblCierrePresupuestario> tblCierrePresupuestario { get; set; }
        [InverseProperty("idAdmElementoPEPNavigation")]
        public virtual ICollection<tblComentarioNCuentaContable> tblComentarioNCuentaContable { get; set; }
        [InverseProperty("idAdmElementoPEPNavigation")]
        public virtual ICollection<tblHistoricoAsientoNomina> tblHistoricoAsientoNomina { get; set; }
        [InverseProperty("idAdmElementoPEPNavigation")]
        public virtual ICollection<tblHistoricoAsientoNomina_MX> tblHistoricoAsientoNomina_MX { get; set; }
        [InverseProperty("idAdmElementoPEPNavigation")]
        public virtual ICollection<tblHistoricoAsientoNomina_RD> tblHistoricoAsientoNomina_RD { get; set; }
        [InverseProperty("idAdmElementoPEPNavigation")]
        public virtual ICollection<tblHistoricoPartidaContable> tblHistoricoPartidaContable { get; set; }
        [InverseProperty("idAdmElementoPEPNavigation")]
        public virtual ICollection<tblHistoricoPlanificacion> tblHistoricoPlanificacion { get; set; }
        [InverseProperty("idAdmElementoPEPNavigation")]
        public virtual ICollection<tblLavanderia> tblLavanderia { get; set; }
        [InverseProperty("idAdmElementoPEPNavigation")]
        public virtual ICollection<tblNomina> tblNomina { get; set; }
        [InverseProperty("idAdmElementoPEPNavigation")]
        public virtual ICollection<tblNomina_MX> tblNomina_MX { get; set; }
        [InverseProperty("idAdmElementoPEPNavigation")]
        public virtual ICollection<tblNomina_RD> tblNomina_RD { get; set; }
        [InverseProperty("idAdmElementoPEPNavigation")]
        public virtual ICollection<tblPersona> tblPersona { get; set; }
        [InverseProperty("idAdmElementoPEPNavigation")]
        public virtual ICollection<tblPresupuestoKg> tblPresupuestoKg { get; set; }

        [ForeignKey("idAdmElementoPEP")]
        [InverseProperty("idAdmElementoPEP")]
        public virtual ICollection<tblUsuario> idUsuario { get; set; }
    }
}
