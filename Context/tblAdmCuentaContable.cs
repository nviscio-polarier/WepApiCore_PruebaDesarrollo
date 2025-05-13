using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAdmCuentaContable", Schema = "Administracion")]
    public partial class tblAdmCuentaContable
    {
        public tblAdmCuentaContable()
        {
            tblAdmConceptoCompra = new HashSet<tblAdmConceptoCompra>();
            tblAdmConceptoVenta = new HashSet<tblAdmConceptoVenta>();
            tblAdmTipoArticuloidAdmCuentaContableCompraNavigation = new HashSet<tblAdmTipoArticulo>();
            tblAdmTipoArticuloidAdmCuentaContableVentaNavigation = new HashSet<tblAdmTipoArticulo>();
            tblAjustePresupuestario = new HashSet<tblAjustePresupuestario>();
            tblArticuloLenceriaidAdmCuentaContableCompraNavigation = new HashSet<tblArticuloLenceria>();
            tblArticuloLenceriaidAdmCuentaContableVentaNavigation = new HashSet<tblArticuloLenceria>();
            tblArticuloLogisticoidAdmCuentaContableCompraNavigation = new HashSet<tblArticuloLogistico>();
            tblArticuloLogisticoidAdmCuentaContableVentaNavigation = new HashSet<tblArticuloLogistico>();
            tblArticuloMaquinariaidAdmCuentaContableCompraNavigation = new HashSet<tblArticuloMaquinaria>();
            tblArticuloMaquinariaidAdmCuentaContableVentaNavigation = new HashSet<tblArticuloMaquinaria>();
            tblArticuloNAdmAlbaranCompra = new HashSet<tblArticuloNAdmAlbaranCompra>();
            tblArticuloNAdmAlbaranVenta = new HashSet<tblArticuloNAdmAlbaranVenta>();
            tblArticuloNAdmPedidoCliente = new HashSet<tblArticuloNAdmPedidoCliente>();
            tblArticuloNAdmPedidoProveedor = new HashSet<tblArticuloNAdmPedidoProveedor>();
            tblArticuloNAdmPresupuestoVenta = new HashSet<tblArticuloNAdmPresupuestoVenta>();
            tblComentarioNCuentaContable = new HashSet<tblComentarioNCuentaContable>();
            tblCuentaContableNCentroTrabajoidAdmCuentaContable_IMSS_MXNavigation = new HashSet<tblCuentaContableNCentroTrabajo>();
            tblCuentaContableNCentroTrabajoidAdmCuentaContable_INFONAVIT_MXNavigation = new HashSet<tblCuentaContableNCentroTrabajo>();
            tblCuentaContableNCentroTrabajoidAdmCuentaContable_ImpEstatalNominas_MXNavigation = new HashSet<tblCuentaContableNCentroTrabajo>();
            tblCuentaContableNCentroTrabajoidAdmCuentaContable_SAR_MXNavigation = new HashSet<tblCuentaContableNCentroTrabajo>();
            tblCuentaContableNCentroTrabajoidAdmCuentaContable_SSEmpresaNavigation = new HashSet<tblCuentaContableNCentroTrabajo>();
            tblCuentaContableNCentroTrabajoidAdmCuentaContable_SalarioNavigation = new HashSet<tblCuentaContableNCentroTrabajo>();
            tblCuentaContableNCentroTrabajoidAdmCuentaContable_Sueldo_MXNavigation = new HashSet<tblCuentaContableNCentroTrabajo>();
            tblCuentaContableNTipoTrabajoidAdmCuentaContable_IMSS_MXNavigation = new HashSet<tblCuentaContableNTipoTrabajo>();
            tblCuentaContableNTipoTrabajoidAdmCuentaContable_INFONAVIT_MXNavigation = new HashSet<tblCuentaContableNTipoTrabajo>();
            tblCuentaContableNTipoTrabajoidAdmCuentaContable_ImpEstatalNominas_MXNavigation = new HashSet<tblCuentaContableNTipoTrabajo>();
            tblCuentaContableNTipoTrabajoidAdmCuentaContable_SAR_MXNavigation = new HashSet<tblCuentaContableNTipoTrabajo>();
            tblCuentaContableNTipoTrabajoidAdmCuentaContable_SSEmpresaNavigation = new HashSet<tblCuentaContableNTipoTrabajo>();
            tblCuentaContableNTipoTrabajoidAdmCuentaContable_SalarioNavigation = new HashSet<tblCuentaContableNTipoTrabajo>();
            tblCuentaContableNTipoTrabajoidAdmCuentaContable_Sueldo_MXNavigation = new HashSet<tblCuentaContableNTipoTrabajo>();
            tblGrupoArticulos = new HashSet<tblGrupoArticulos>();
            tblHistoricoPlanificacion = new HashSet<tblHistoricoPlanificacion>();
            tblNomina_MXidAdmCuentaContable_IMSS_MXNavigation = new HashSet<tblNomina_MX>();
            tblNomina_MXidAdmCuentaContable_INFONAVIT_MXNavigation = new HashSet<tblNomina_MX>();
            tblNomina_MXidAdmCuentaContable_ImpEstatalNominas_MXNavigation = new HashSet<tblNomina_MX>();
            tblNomina_MXidAdmCuentaContable_SAR_MXNavigation = new HashSet<tblNomina_MX>();
            tblNomina_MXidAdmCuentaContable_Sueldo_MXNavigation = new HashSet<tblNomina_MX>();
            tblNomina_RD = new HashSet<tblNomina_RD>();
            tblNominaidAdmCuentaContable_SSEmpresaNavigation = new HashSet<tblNomina>();
            tblNominaidAdmCuentaContable_SalarioNavigation = new HashSet<tblNomina>();
            tblPartidaNCierrePresupuestario = new HashSet<tblPartidaNCierrePresupuestario>();
            tblPersonaidAdmCuentaContable_SSEmpresaNavigation = new HashSet<tblPersona>();
            tblPersonaidAdmCuentaContable_SalarioNavigation = new HashSet<tblPersona>();
            tblPlanificacionNCierrePresupuestario = new HashSet<tblPlanificacionNCierrePresupuestario>();
        }

        [Key]
        public int idAdmCuentaContable { get; set; }
        [StringLength(8)]
        public string codigo { get; set; } = null!;
        public string denominacion { get; set; } = null!;

        [InverseProperty("idAdmCuentaContableNavigation")]
        public virtual ICollection<tblAdmConceptoCompra> tblAdmConceptoCompra { get; set; }
        [InverseProperty("idAdmCuentaContableNavigation")]
        public virtual ICollection<tblAdmConceptoVenta> tblAdmConceptoVenta { get; set; }
        [InverseProperty("idAdmCuentaContableCompraNavigation")]
        public virtual ICollection<tblAdmTipoArticulo> tblAdmTipoArticuloidAdmCuentaContableCompraNavigation { get; set; }
        [InverseProperty("idAdmCuentaContableVentaNavigation")]
        public virtual ICollection<tblAdmTipoArticulo> tblAdmTipoArticuloidAdmCuentaContableVentaNavigation { get; set; }
        [InverseProperty("idAdmCuentaContableNavigation")]
        public virtual ICollection<tblAjustePresupuestario> tblAjustePresupuestario { get; set; }
        [InverseProperty("idAdmCuentaContableCompraNavigation")]
        public virtual ICollection<tblArticuloLenceria> tblArticuloLenceriaidAdmCuentaContableCompraNavigation { get; set; }
        [InverseProperty("idAdmCuentaContableVentaNavigation")]
        public virtual ICollection<tblArticuloLenceria> tblArticuloLenceriaidAdmCuentaContableVentaNavigation { get; set; }
        [InverseProperty("idAdmCuentaContableCompraNavigation")]
        public virtual ICollection<tblArticuloLogistico> tblArticuloLogisticoidAdmCuentaContableCompraNavigation { get; set; }
        [InverseProperty("idAdmCuentaContableVentaNavigation")]
        public virtual ICollection<tblArticuloLogistico> tblArticuloLogisticoidAdmCuentaContableVentaNavigation { get; set; }
        [InverseProperty("idAdmCuentaContableCompraNavigation")]
        public virtual ICollection<tblArticuloMaquinaria> tblArticuloMaquinariaidAdmCuentaContableCompraNavigation { get; set; }
        [InverseProperty("idAdmCuentaContableVentaNavigation")]
        public virtual ICollection<tblArticuloMaquinaria> tblArticuloMaquinariaidAdmCuentaContableVentaNavigation { get; set; }
        [InverseProperty("idAdmCuentaContableNavigation")]
        public virtual ICollection<tblArticuloNAdmAlbaranCompra> tblArticuloNAdmAlbaranCompra { get; set; }
        [InverseProperty("idAdmCuentaContableNavigation")]
        public virtual ICollection<tblArticuloNAdmAlbaranVenta> tblArticuloNAdmAlbaranVenta { get; set; }
        [InverseProperty("idAdmCuentaContableNavigation")]
        public virtual ICollection<tblArticuloNAdmPedidoCliente> tblArticuloNAdmPedidoCliente { get; set; }
        [InverseProperty("idAdmCuentaContableNavigation")]
        public virtual ICollection<tblArticuloNAdmPedidoProveedor> tblArticuloNAdmPedidoProveedor { get; set; }
        [InverseProperty("idAdmCuentaContableNavigation")]
        public virtual ICollection<tblArticuloNAdmPresupuestoVenta> tblArticuloNAdmPresupuestoVenta { get; set; }
        [InverseProperty("idAdmCuentaContableNavigation")]
        public virtual ICollection<tblComentarioNCuentaContable> tblComentarioNCuentaContable { get; set; }
        [InverseProperty("idAdmCuentaContable_IMSS_MXNavigation")]
        public virtual ICollection<tblCuentaContableNCentroTrabajo> tblCuentaContableNCentroTrabajoidAdmCuentaContable_IMSS_MXNavigation { get; set; }
        [InverseProperty("idAdmCuentaContable_INFONAVIT_MXNavigation")]
        public virtual ICollection<tblCuentaContableNCentroTrabajo> tblCuentaContableNCentroTrabajoidAdmCuentaContable_INFONAVIT_MXNavigation { get; set; }
        [InverseProperty("idAdmCuentaContable_ImpEstatalNominas_MXNavigation")]
        public virtual ICollection<tblCuentaContableNCentroTrabajo> tblCuentaContableNCentroTrabajoidAdmCuentaContable_ImpEstatalNominas_MXNavigation { get; set; }
        [InverseProperty("idAdmCuentaContable_SAR_MXNavigation")]
        public virtual ICollection<tblCuentaContableNCentroTrabajo> tblCuentaContableNCentroTrabajoidAdmCuentaContable_SAR_MXNavigation { get; set; }
        [InverseProperty("idAdmCuentaContable_SSEmpresaNavigation")]
        public virtual ICollection<tblCuentaContableNCentroTrabajo> tblCuentaContableNCentroTrabajoidAdmCuentaContable_SSEmpresaNavigation { get; set; }
        [InverseProperty("idAdmCuentaContable_SalarioNavigation")]
        public virtual ICollection<tblCuentaContableNCentroTrabajo> tblCuentaContableNCentroTrabajoidAdmCuentaContable_SalarioNavigation { get; set; }
        [InverseProperty("idAdmCuentaContable_Sueldo_MXNavigation")]
        public virtual ICollection<tblCuentaContableNCentroTrabajo> tblCuentaContableNCentroTrabajoidAdmCuentaContable_Sueldo_MXNavigation { get; set; }
        [InverseProperty("idAdmCuentaContable_IMSS_MXNavigation")]
        public virtual ICollection<tblCuentaContableNTipoTrabajo> tblCuentaContableNTipoTrabajoidAdmCuentaContable_IMSS_MXNavigation { get; set; }
        [InverseProperty("idAdmCuentaContable_INFONAVIT_MXNavigation")]
        public virtual ICollection<tblCuentaContableNTipoTrabajo> tblCuentaContableNTipoTrabajoidAdmCuentaContable_INFONAVIT_MXNavigation { get; set; }
        [InverseProperty("idAdmCuentaContable_ImpEstatalNominas_MXNavigation")]
        public virtual ICollection<tblCuentaContableNTipoTrabajo> tblCuentaContableNTipoTrabajoidAdmCuentaContable_ImpEstatalNominas_MXNavigation { get; set; }
        [InverseProperty("idAdmCuentaContable_SAR_MXNavigation")]
        public virtual ICollection<tblCuentaContableNTipoTrabajo> tblCuentaContableNTipoTrabajoidAdmCuentaContable_SAR_MXNavigation { get; set; }
        [InverseProperty("idAdmCuentaContable_SSEmpresaNavigation")]
        public virtual ICollection<tblCuentaContableNTipoTrabajo> tblCuentaContableNTipoTrabajoidAdmCuentaContable_SSEmpresaNavigation { get; set; }
        [InverseProperty("idAdmCuentaContable_SalarioNavigation")]
        public virtual ICollection<tblCuentaContableNTipoTrabajo> tblCuentaContableNTipoTrabajoidAdmCuentaContable_SalarioNavigation { get; set; }
        [InverseProperty("idAdmCuentaContable_Sueldo_MXNavigation")]
        public virtual ICollection<tblCuentaContableNTipoTrabajo> tblCuentaContableNTipoTrabajoidAdmCuentaContable_Sueldo_MXNavigation { get; set; }
        [InverseProperty("idAdmCuentaContableCompraNavigation")]
        public virtual ICollection<tblGrupoArticulos> tblGrupoArticulos { get; set; }
        [InverseProperty("idAdmCuentaContableNavigation")]
        public virtual ICollection<tblHistoricoPlanificacion> tblHistoricoPlanificacion { get; set; }
        [InverseProperty("idAdmCuentaContable_IMSS_MXNavigation")]
        public virtual ICollection<tblNomina_MX> tblNomina_MXidAdmCuentaContable_IMSS_MXNavigation { get; set; }
        [InverseProperty("idAdmCuentaContable_INFONAVIT_MXNavigation")]
        public virtual ICollection<tblNomina_MX> tblNomina_MXidAdmCuentaContable_INFONAVIT_MXNavigation { get; set; }
        [InverseProperty("idAdmCuentaContable_ImpEstatalNominas_MXNavigation")]
        public virtual ICollection<tblNomina_MX> tblNomina_MXidAdmCuentaContable_ImpEstatalNominas_MXNavigation { get; set; }
        [InverseProperty("idAdmCuentaContable_SAR_MXNavigation")]
        public virtual ICollection<tblNomina_MX> tblNomina_MXidAdmCuentaContable_SAR_MXNavigation { get; set; }
        [InverseProperty("idAdmCuentaContable_Sueldo_MXNavigation")]
        public virtual ICollection<tblNomina_MX> tblNomina_MXidAdmCuentaContable_Sueldo_MXNavigation { get; set; }
        [InverseProperty("idAdmCuentaContableNavigation")]
        public virtual ICollection<tblNomina_RD> tblNomina_RD { get; set; }
        [InverseProperty("idAdmCuentaContable_SSEmpresaNavigation")]
        public virtual ICollection<tblNomina> tblNominaidAdmCuentaContable_SSEmpresaNavigation { get; set; }
        [InverseProperty("idAdmCuentaContable_SalarioNavigation")]
        public virtual ICollection<tblNomina> tblNominaidAdmCuentaContable_SalarioNavigation { get; set; }
        [InverseProperty("idAdmCuentaContableNavigation")]
        public virtual ICollection<tblPartidaNCierrePresupuestario> tblPartidaNCierrePresupuestario { get; set; }
        [InverseProperty("idAdmCuentaContable_SSEmpresaNavigation")]
        public virtual ICollection<tblPersona> tblPersonaidAdmCuentaContable_SSEmpresaNavigation { get; set; }
        [InverseProperty("idAdmCuentaContable_SalarioNavigation")]
        public virtual ICollection<tblPersona> tblPersonaidAdmCuentaContable_SalarioNavigation { get; set; }
        [InverseProperty("idAdmCuentaContableNavigation")]
        public virtual ICollection<tblPlanificacionNCierrePresupuestario> tblPlanificacionNCierrePresupuestario { get; set; }
    }
}
