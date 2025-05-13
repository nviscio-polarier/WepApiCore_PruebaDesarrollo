using WebApiCore.Context;

namespace WebApiCore.Controllers.Proyectos.Administracion.Context
{
    public class PedidoProveedorConArticulos
    {
        public int idAdmPedidoProveedor { get; set; }
        public byte? idTipoPedido { get; set; }
        public string codigo { get; set; } = null!;
        public DateTime? fechaCreacion { get; set; }
        public byte? idAdmPedido_Estado { get; set; }
        public byte? idMoneda { get; set; }
        public int? idAdmProveedor { get; set; }
        public byte? idAdmTipoCambio { get; set; }
        public decimal? tasaCambio { get; set; }
        public int? idAdmFormaPago { get; set; }
        public int? idAdmCentroCoste { get; set; }
        public int? idAdmElementoPEP { get; set; }
        public string? direccionEntrega { get; set; }
        public string? idCentro { get; set; }
        public decimal? descuento { get; set; }
        public byte? idAdmTipoDescuento { get; set; }
        public int? idIncoterm { get; set; }
        public DateTime? fechaEstimadaRecepcion { get; set; }
        public string? numPresupuestoProveedor { get; set; }
        public string? observaciones { get; set; }
        public short idEmpresaPolarier { get; set; }
        public List<tblArticuloNAdmPedidoProveedor>? tblArticuloNAdmPedidoProveedor { get; set; }
    }
}
