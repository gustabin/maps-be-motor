namespace Isban.PDC.Middleware.DataAccess.Requests
{
    public class CompraVtaIatxRequestBase : BaseIaxtRequest
    {
        public string TipoCtaOper { get; set; }

        public string SucursalCuenta { get; set; }

        public string NumeroCuenta { get; set; }

        public string TipoOperacion { get; set; }

        public string TipoEspecie { get; set; }

        public string CantidadTitulo { get; set; }

        public string CuentaTitulo { get; set; }

        public string EspecieCodigo { get; set; }

        public string ImporteDebitoCredito { get; set; }

        public string CotizacionLimite { get; set; }

        public string Cotizacion { get; set; }

        public virtual void Format()
        {
            TipoCtaOper = TipoCtaOper.PadLeft(2, '0');
        }
    }
}
