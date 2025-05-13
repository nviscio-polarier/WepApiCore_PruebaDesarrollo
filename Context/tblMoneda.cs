using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblMoneda", Schema = "General")]
    public partial class tblMoneda
    {
        public tblMoneda()
        {
            tblAdmAlbaranCompra = new HashSet<tblAdmAlbaranCompra>();
            tblAdmAlbaranVenta = new HashSet<tblAdmAlbaranVenta>();
            tblAdmCliente = new HashSet<tblAdmCliente>();
            tblAdmCuentaBancaria = new HashSet<tblAdmCuentaBancaria>();
            tblAdmFacturaCompra = new HashSet<tblAdmFacturaCompra>();
            tblAdmFacturaVenta = new HashSet<tblAdmFacturaVenta>();
            tblAdmPedidoCliente = new HashSet<tblAdmPedidoCliente>();
            tblAdmPedidoProveedor = new HashSet<tblAdmPedidoProveedor>();
            tblAdmPresupuestoVenta = new HashSet<tblAdmPresupuestoVenta>();
            tblAdmProveedor = new HashSet<tblAdmProveedor>();
            tblAjustePresupuestario = new HashSet<tblAjustePresupuestario>();
            tblAlmacenRecambios = new HashSet<tblAlmacenRecambios>();
            tblEmpresasPolarier = new HashSet<tblEmpresasPolarier>();
            tblEntidad = new HashSet<tblEntidad>();
            tblHistoricoPartidaContable = new HashSet<tblHistoricoPartidaContable>();
            tblLavanderiaidMonedaLocalNavigation = new HashSet<tblLavanderia>();
            tblLavanderiaidMonedaNavigation = new HashSet<tblLavanderia>();
            tblPais = new HashSet<tblPais>();
            tblPartidaNCierrePresupuestario = new HashSet<tblPartidaNCierrePresupuestario>();
            tblPlanificacionNCierrePresupuestario = new HashSet<tblPlanificacionNCierrePresupuestario>();
            tblTasaCambioPresupuesto = new HashSet<tblTasaCambioPresupuesto>();
            tblTasaCambioidMonedaDestinoNavigation = new HashSet<tblTasaCambio>();
            tblTasaCambioidMonedaOrigenNavigation = new HashSet<tblTasaCambio>();
        }

        [Key]
        public byte idMoneda { get; set; }
        [StringLength(50)]
        public string? denominacion { get; set; }
        [StringLength(3)]
        public string? codigo { get; set; }
        public string? simbolo { get; set; }

        [InverseProperty("idMonedaNavigation")]
        public virtual ICollection<tblAdmAlbaranCompra> tblAdmAlbaranCompra { get; set; }
        [InverseProperty("idMonedaNavigation")]
        public virtual ICollection<tblAdmAlbaranVenta> tblAdmAlbaranVenta { get; set; }
        [InverseProperty("idMonedaNavigation")]
        public virtual ICollection<tblAdmCliente> tblAdmCliente { get; set; }
        [InverseProperty("idMonedaNavigation")]
        public virtual ICollection<tblAdmCuentaBancaria> tblAdmCuentaBancaria { get; set; }
        [InverseProperty("idMonedaNavigation")]
        public virtual ICollection<tblAdmFacturaCompra> tblAdmFacturaCompra { get; set; }
        [InverseProperty("idMonedaNavigation")]
        public virtual ICollection<tblAdmFacturaVenta> tblAdmFacturaVenta { get; set; }
        [InverseProperty("idMonedaNavigation")]
        public virtual ICollection<tblAdmPedidoCliente> tblAdmPedidoCliente { get; set; }
        [InverseProperty("idMonedaNavigation")]
        public virtual ICollection<tblAdmPedidoProveedor> tblAdmPedidoProveedor { get; set; }
        [InverseProperty("idMonedaNavigation")]
        public virtual ICollection<tblAdmPresupuestoVenta> tblAdmPresupuestoVenta { get; set; }
        [InverseProperty("idMonedaNavigation")]
        public virtual ICollection<tblAdmProveedor> tblAdmProveedor { get; set; }
        [InverseProperty("idMonedaNavigation")]
        public virtual ICollection<tblAjustePresupuestario> tblAjustePresupuestario { get; set; }
        [InverseProperty("idMonedaNavigation")]
        public virtual ICollection<tblAlmacenRecambios> tblAlmacenRecambios { get; set; }
        [InverseProperty("idMonedaNavigation")]
        public virtual ICollection<tblEmpresasPolarier> tblEmpresasPolarier { get; set; }
        [InverseProperty("idMonedaNavigation")]
        public virtual ICollection<tblEntidad> tblEntidad { get; set; }
        [InverseProperty("idMonedaNavigation")]
        public virtual ICollection<tblHistoricoPartidaContable> tblHistoricoPartidaContable { get; set; }
        [InverseProperty("idMonedaLocalNavigation")]
        public virtual ICollection<tblLavanderia> tblLavanderiaidMonedaLocalNavigation { get; set; }
        [InverseProperty("idMonedaNavigation")]
        public virtual ICollection<tblLavanderia> tblLavanderiaidMonedaNavigation { get; set; }
        [InverseProperty("idMonedaNavigation")]
        public virtual ICollection<tblPais> tblPais { get; set; }
        [InverseProperty("idMonedaNavigation")]
        public virtual ICollection<tblPartidaNCierrePresupuestario> tblPartidaNCierrePresupuestario { get; set; }
        [InverseProperty("idMonedaNavigation")]
        public virtual ICollection<tblPlanificacionNCierrePresupuestario> tblPlanificacionNCierrePresupuestario { get; set; }
        [InverseProperty("idMonedaDestinoNavigation")]
        public virtual ICollection<tblTasaCambioPresupuesto> tblTasaCambioPresupuesto { get; set; }
        [InverseProperty("idMonedaDestinoNavigation")]
        public virtual ICollection<tblTasaCambio> tblTasaCambioidMonedaDestinoNavigation { get; set; }
        [InverseProperty("idMonedaOrigenNavigation")]
        public virtual ICollection<tblTasaCambio> tblTasaCambioidMonedaOrigenNavigation { get; set; }
    }
}
