using WebApiCore.Context;

namespace WebApiCore.Controllers.Proyectos.Administracion.Context
{
    public class FacturaVenta
    {
        public int? idAdmFacturaVenta { get; set; }
        public int? idReferenciaFacturaVenta { get; set; }
        public byte? idAdmFactura_Estado { get; set; }
        public byte? idMoneda { get; set; }
        public byte? idAdmTipoCambio { get; set; }
        public byte? idTipoAlbaran { get; set; }
        public int? idTipoFactura { get; set; }
        public int? idIncoterm { get; set; }
        public string? codigo { get; set; }
        public DateTime? fecha { get; set; }
        public DateTime? fechaVencimiento { get; set; }
        public byte? idAdmTipoNCF { get; set; }
        public string? NCF { get; set; }
        public string? comentario { get; set; }
        public int? idAdmCliente { get; set; }
        public short? idCuentaBancaria { get; set; }
        public int? idAdmFormaCobro { get; set; }
        public decimal? tasaCambio { get; set; }
        public byte? idAdmTipoDescuento { get; set; }
        public decimal? descuento { get; set; }
        public string? observaciones { get; set; }
        public string? numPedido { get; set; }
        public short idEmpresaPolarier { get; set; }
        public int? idAdmCentroCoste { get; set; }
        public int? idAdmElementoPEP { get; set; }
        public int? idAdmCondicionPago { get; set; }
        public string? tipoRetencion { get; set; }
        public decimal? codigoRetencion { get; set; }
        public bool? aplicaRetencion { get; set; }
        public List<int> idAdmAlbaranVenta { get; set; }
        public byte? idIvaNPais { get; set; }
        public List<tblAdmConceptoVenta> tblAdmConceptoVenta { get; set; }
        public string? sociedadGL { get; set; }
        public string? formaPagoMXN { get; set; }
        public string? usoCFDI { get; set; }
        public string? producto { get; set; }
        public string? cantidad { get; set; }
        public string? unidadMedida { get; set; }
        public string? clvRef1 { get; set; }
        public string? clvRef2 { get; set; }
        public string? clvRef3 { get; set; }
    }
}
