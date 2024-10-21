using System;
using System.Runtime.Serialization;

namespace Isban.PDC.Middleware.Entity
{
    [DataContract]
    public class CompraVtaAccionesBonosRequest : BaseIatx
    {
        [DataMember] public string CanalCodigo { get; set; }

        [DataMember] public string TipoAccion { get; set; }

        [DataMember] public string TipoEspecie { get; set; }

        [DataMember] public long SucursalCuenta { get; set; }

        [DataMember] public long CuentaTitulo { get; set; }

        [DataMember] public long TipoCtaOper { get; set; }

        [DataMember] public string MonedaOperacion { get; set; }

        [DataMember] public string NumeroCuenta { get; set; }

        [DataMember] public long TipoOperacion { get; set; }

        [DataMember] public decimal CantidadTitulo { get; set; }

        [DataMember] public long EspecieCodigo { get; set; }

        [DataMember] public decimal ImporteDebitoCredito { get; set; }

        [DataMember] public decimal CotizacionLimite { get; set; }

        [DataMember] public decimal Cotizacion { get; set; }

        [DataMember] public string Plazo { get; set; }

        [DataMember] public string OperadorAlta { get; set; }

        [DataMember] public string Instancia { get { return TipoAccion; } }

        [DataMember] public decimal PrecioClean { get; set; }
        [DataMember]
        public string Usuario { get; set; }
        [DataMember]
        public string Ip { get; set; }

        protected override void ValidarDatos()
        {
            ValidarLargo("TipoAccion", TipoAccion, 1);
            ValidarLargo("TipoEspecie", TipoEspecie, 3);
            ValidarLargo("SucursalCuenta", SucursalCuenta, 8);
            ValidarLargo("CuentaTitulo", CuentaTitulo, 8);
            ValidarLargo("TipoCtaOper", TipoCtaOper, 2);
            ValidarLargo("MonedaOperacion", MonedaOperacion, 3);
            ValidarLargo("NumeroCuenta", NumeroCuenta, 7);
            ValidarLargo("CantidadTitulo", CantidadTitulo, 14);
            ValidarLargo("EspecieCodigo", EspecieCodigo, 5);
            ValidarLargo("ImporteDebitoCredito", ImporteDebitoCredito, 16);
            ValidarLargo("CotizacionLimite", CotizacionLimite, 14);
            ValidarLargo("Cotizacion", Cotizacion, 14);
            ValidarLargo("Plazo", Plazo, 3);
            ValidarLargo("OperadorAlta", OperadorAlta, 8);
            ValidarLargoRango("Instancia", Instancia, 0, 1);
            ValidarLargo("PrecioClean", PrecioClean, 14);

            ValidarContenido("TipoAccion", TipoAccion, "I", "D");
            ValidarContenido("TipoEspecie", TipoEspecie, "BON", "SHS");
            ValidarContenido("TipoCtaOper", TipoCtaOper, 0, 1, 3, 4, 9, 10);
            ValidarContenido("TipoOperacion", TipoOperacion, 1, 2);

            if (!string.IsNullOrEmpty(MonedaOperacion))
            {
                ValidarContenido("MonedaOperacion", MonedaOperacion, "ARS", "USB", "USD");
            }
        }
    }
}
