using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAdmCentroCoste", Schema = "Administracion")]
    public partial class tblAdmCentroCoste
    {
        public tblAdmCentroCoste()
        {
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
            tblNomina = new HashSet<tblNomina>();
            tblNomina_MX = new HashSet<tblNomina_MX>();
            tblNomina_RD = new HashSet<tblNomina_RD>();
            tblPersona = new HashSet<tblPersona>();
            idUsuario = new HashSet<tblUsuario>();
        }

        [Key]
        public int idAdmCentroCoste { get; set; }
        public string? denominacion { get; set; }
        public short? idEmpresaPolarier { get; set; }
        public bool? isEliminado { get; set; }
        public string? codigo { get; set; }
        public string? workplaceCode_A3 { get; set; }

        [ForeignKey("idEmpresaPolarier")]
        [InverseProperty("tblAdmCentroCoste")]
        public virtual tblEmpresasPolarier? idEmpresaPolarierNavigation { get; set; }
        [InverseProperty("idAdmCentroCosteNavigation")]
        public virtual ICollection<tblAdmAlbaranCompra> tblAdmAlbaranCompra { get; set; }
        [InverseProperty("idAdmCentroCosteNavigation")]
        public virtual ICollection<tblAdmAlbaranVenta> tblAdmAlbaranVenta { get; set; }
        [InverseProperty("idAdmCentroCosteNavigation")]
        public virtual ICollection<tblAdmCliente> tblAdmCliente { get; set; }
        [InverseProperty("idAdmCentroCosteNavigation")]
        public virtual ICollection<tblAdmFacturaCompra> tblAdmFacturaCompra { get; set; }
        [InverseProperty("idAdmCentroCosteNavigation")]
        public virtual ICollection<tblAdmFacturaVenta> tblAdmFacturaVenta { get; set; }
        [InverseProperty("idAdmCentroCosteNavigation")]
        public virtual ICollection<tblAdmPedidoCliente> tblAdmPedidoCliente { get; set; }
        [InverseProperty("idAdmCentroCosteNavigation")]
        public virtual ICollection<tblAdmPedidoProveedor> tblAdmPedidoProveedor { get; set; }
        [InverseProperty("idAdmCentroCosteNavigation")]
        public virtual ICollection<tblAjustePresupuestario> tblAjustePresupuestario { get; set; }
        [InverseProperty("idAdmCentroCosteNavigation")]
        public virtual ICollection<tblCierrePresupuestario> tblCierrePresupuestario { get; set; }
        [InverseProperty("idAdmCentroCosteNavigation")]
        public virtual ICollection<tblComentarioNCuentaContable> tblComentarioNCuentaContable { get; set; }
        [InverseProperty("idAdmCentroCosteNavigation")]
        public virtual ICollection<tblHistoricoAsientoNomina> tblHistoricoAsientoNomina { get; set; }
        [InverseProperty("idAdmCentroCosteNavigation")]
        public virtual ICollection<tblHistoricoAsientoNomina_MX> tblHistoricoAsientoNomina_MX { get; set; }
        [InverseProperty("idAdmCentroCosteNavigation")]
        public virtual ICollection<tblHistoricoAsientoNomina_RD> tblHistoricoAsientoNomina_RD { get; set; }
        [InverseProperty("idAdmCentroCosteNavigation")]
        public virtual ICollection<tblHistoricoPartidaContable> tblHistoricoPartidaContable { get; set; }
        [InverseProperty("idAdmCentroCosteNavigation")]
        public virtual ICollection<tblHistoricoPlanificacion> tblHistoricoPlanificacion { get; set; }
        [InverseProperty("idAdmCentroCosteNavigation")]
        public virtual ICollection<tblNomina> tblNomina { get; set; }
        [InverseProperty("idAdmCentroCosteNavigation")]
        public virtual ICollection<tblNomina_MX> tblNomina_MX { get; set; }
        [InverseProperty("idAdmCentroCosteNavigation")]
        public virtual ICollection<tblNomina_RD> tblNomina_RD { get; set; }
        [InverseProperty("idAdmCentroCosteNavigation")]
        public virtual ICollection<tblPersona> tblPersona { get; set; }

        [ForeignKey("idAdmCentroCoste")]
        [InverseProperty("idAdmCentroCoste")]
        public virtual ICollection<tblUsuario> idUsuario { get; set; }
    }
}
