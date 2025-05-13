namespace WebApiCore.Class.externos.SAP.Controllers
{
    /// <summary>
    /// Wrapper de los distintos controllers de SAP disponibles.
    /// </summary>
    public class SAPWrap
    {
        public readonly CentrosBeneficioController centrosBeneficioController = new();
        public readonly ClientesController clientesController = new();
        public readonly CondicionesCobroController condicionesCobroController = new();
        public readonly ProveedoresController proveedoresController = new();
        public readonly ViasPagoCobroController viasPagoCobroController = new();
        public readonly GruposDeArticulosController gruposDeArticulosController = new();
        public readonly CentrosdeCosteController centrosdeCosteController = new();
        public readonly PlanificacionController planificacionController = new();
        public readonly PartidasContablesController partidasContablesController = new();
        public readonly GrupoArticulosController grupoArticulosController = new();
        public readonly CondicionPagoController condicionPagoController = new();
        public readonly ElementosPEPController elementosPEPController = new();
        public readonly CuentaContableController cuentaContableController = new();
        public readonly AllOriginalsController allOriginalsController = new();
        public readonly AttachmentContentController attachmentContentController = new();
        public readonly FacturasProveedorController facturasProveedorController = new();
        public readonly ElectronicDocFileController electronicDocFileController = new();
        public readonly EInvoiceMexicoController eInvoiceMexicoController = new();
    }
}
