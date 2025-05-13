using WebApiCore.Context;

namespace WebApiCore.Controllers.Proyectos.Administracion.Context
{
    public class AlbaranCompraConArticulos
    {
        public int idAdmAlbaranCompra { get; set; }
        public string codigo { get; set; } = null!;
        public DateTime? fechaCreacion { get; set; }
        public byte? idTipoAlbaran { get; set; }
        public byte? idAdmAlbaran_Estado { get; set; }
        public int? idAdmProveedor { get; set; }
        public byte? idMoneda { get; set; }
        public byte? idAdmTipoCambio { get; set; }
        public decimal? tasaCambio { get; set; }
        public decimal? descuento { get; set; }
        public byte? idAdmTipoDescuento { get; set; }
        public int? idAdmFormaPago { get; set; }
        public int? idIncoterm { get; set; }
        public int? idAdmPedidoProveedor { get; set; }
        public string? observaciones { get; set; }
        public string? numAlbaranProveedor { get; set; }
        public short idEmpresaPolarier { get; set; }
        public int? idAdmCentroCoste { get; set; }
        public int? idAdmElementoPEP { get; set; }
        public List<tblArticuloNAdmAlbaranCompra>? tblArticuloNAdmAlbaranCompra { get; set; }
    }
}
