using System.CodeDom.Compiler;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace WebApiCore.Class.bdERP.Administracion
{
    public class EntradaAsientoContable
    {
        public static string GenerateCSV(IEnumerable<EntradaAsientoContable> data)
        {
            return Utils.GenerateCSV(data, x =>
            {
                if (x.Name.Contains("ClvRef"))
                    return x.Name.Replace("ClvRef", "Clv.ref.");
                else
                    return x.Name;
            });
        }

        public int IDApunte { get; set; }
        [StringLength(4)]
        public string Sociedad { get; set; }
        [StringLength(2)]
        public string ClaseDocumento { get; set; }
        [StringLength(16)]
        public string? ReferenciaFactura { get; set; }
        [StringLength(25)]
        public string? TextoCabecera { get; set; }
        [StringLength(20)]
        public string? Referencia1 { get; set; }
        [StringLength(20)]
        public string? Referencia2 { get; set; }
        [StringLength(12)]
        public string? ClvRef1 { get; set; }
        [StringLength(12)]
        public string? ClvRef2 { get; set; }
        [StringLength(12)]
        public string? ClvRef3 { get; set; }
        public string? FechaFactura { get; set; }
        public string FechaContable { get; set; }
        [StringLength(10)]
        public string? Cliente { get; set; }
        [StringLength(10)]
        public string? Proveedor { get; set; }
        [StringLength(10)]
        public string? CuentaContable { get; set; }
        public string? SociedadGL { get; set; }
        public bool? CPD { get; set; }
        [StringLength(35)]
        public string? Nombre { get; set; }
        [StringLength(35)]
        public string? Poblacion { get; set; }
        [StringLength(3)]
        public string? Pais { get; set; }
        [StringLength(16)]
        public string? TaxNumber1 { get; set; }
        [StringLength(11)]
        public string? TaxNumber2 { get; set; }
        [StringLength(18)]
        public string? TaxNumber3 { get; set; }
        [StringLength(18)]
        public string? TaxNumber4 { get; set; }
        [StringLength(60)]
        public string? TaxNumber5 { get; set; }
        public bool? IsNaturalPerson { get; set; }
        [StringLength(2)]
        public string? IndicadorIVA { get; set; }
        [StringLength(3)]
        public string? ClasificacionImp { get; set; }
        [StringLength(4)]
        public string? TpCondicion { get; set; }
        public string? TipoRet { get; set; }
        public int? IndicadorRet { get; set; }
        public string? FeDeclImpt { get; set; }
        public string? FeCumpImpt { get; set; }
        public decimal? ImporteTransaccion { get; set; }
        public decimal? ImporteImpuestoTransaccion { get; set; }
        public decimal? ImporteBase { get; set; }
        public decimal? ImporteRet { get; set; }
        [StringLength(3)]
        public string? MonedaTransaccion { get; set; }
        public decimal? ImporteSociedad { get; set; }
        public decimal? ImporteImpuestoSociedad { get; set; }
        public decimal? ImporteBaseSociedad { get; set; }
        public decimal? ImporteRetSociedad { get; set; }
        [StringLength(3)]
        public string? MonedaSociedad { get; set; }
        public decimal? ImporteGrupo { get; set; }
        public decimal? ImporteImpuestoGrupo { get; set; }
        public decimal? ImporteBaseGrupo { get; set; }
        public decimal? ImporteRetGrupo { get; set; }
        [StringLength(3)]
        public string? MonedaGrupo { get; set; }
        [StringLength(10)]
        public string? CuentaAlternativa { get; set; }
        [StringLength(1)]
        public string? CME { get; set; }
        [StringLength(5)]
        public string? BancoPropio { get; set; }
        public string? IDCuenta { get; set; }
        [StringLength(1)]
        public string? Viapago { get; set; }
        [StringLength(4)]
        public string? Condicionpago { get; set; }
        public string? FechaBase { get; set; }
        public int? DiasVencimiento { get; set; }
        [StringLength(50)]
        public string? Textoposicion { get; set; }
        [StringLength(18)]
        public string? Asignacion { get; set; }
        [StringLength(10)]
        public string? Centrocoste { get; set; }
        [StringLength(10)]
        public string? Centrobeneficio { get; set; }
        [StringLength(24)]
        public string? Pep { get; set; }
        public string? COPA_Customer { get; set; }
        public string? Producto { get; set; }
        public string? Cantidad { get; set; }
        public string? Unidadmedida { get; set; }
    }
}
