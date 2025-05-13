using WebApiCore.Context;

namespace WebApiCore.Controllers.Proyectos.Administracion.Context
{
    public class FacturaCompra
    {
        public int? idAdmFacturaCompra { get; set; }
        public byte? idAdmFactura_Estado { get; set; }
        public byte? idMoneda { get; set; }
        public byte? idAdmTipoCambio { get; set; }
        public byte? idTipoAlbaran { get; set; }
        public int? idTipoFactura { get; set; }
        public int? idIncoterm { get; set; }
        public string? codigo { get; set; }
        public DateTime? fecha { get; set; }
        public decimal? tasaCambio { get; set; }
        public decimal? descuento { get; set; }
        public byte? idAdmTipoDescuento { get; set; }
        public int? idAdmProveedor { get; set; }
        public int? idAdmFormaPago { get; set; }
        public string? observaciones { get; set; }
        public string? numFacturaProveedor { get; set; }
        public short idEmpresaPolarier { get; set; }
        public int? idAdmCentroCoste { get; set; }
        public int? idAdmElementoPEP { get; set; }
        public int? idAdmCondicionPago { get; set; }
        public List<int>? idAdmAlbaranCompra { get; set; }
        public List<tblAdmConceptoCompra>? tblAdmConceptoCompra { get; set; }
    }

}
