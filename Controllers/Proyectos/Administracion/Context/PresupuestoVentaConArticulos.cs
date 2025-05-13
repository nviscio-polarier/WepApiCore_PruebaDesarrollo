using WebApiCore.Context;

namespace WebApiCore.Controllers.Proyectos.Administracion.Context
{
    public class PresupuestoVentaConArticulos
    {
        public int idAdmPresupuestoVenta { get; set; }
        public string codigo { get; set; } = null!;
        public int? idAdmCliente { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public byte? idMoneda { get; set; }
        public decimal? tasaCambio { get; set; }
        public int? idAdmFormaPago { get; set; }
        public decimal? descuento { get; set; }
        public byte? idAdmTipoDescuento { get; set; }
        public byte? idTipoPresupuesto { get; set; }
        public string? observaciones { get; set; }
        public byte? idAdmPresupuestoVenta_Estado { get; set; }
        public byte? idIvaNPais { get; set; }
        public byte? idAdmTipoCambio { get; set; }
        public short idEmpresaPolarier { get; set; }
        public List<tblArticuloNAdmPresupuestoVenta>? tblArticuloNAdmPresupuestoVenta { get; set; }
    }
}
