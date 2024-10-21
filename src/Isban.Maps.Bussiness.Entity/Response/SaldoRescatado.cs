using Isban.MapsMB.Common.Entity.Interfaces;
using System;

namespace Isban.MapsMB.Common.Entity.Response
{
    public class SaldoRescatado : IGuidBase
    {
        /// <summary>
        /// Guid de Error
        /// </summary>
        public Guid? GUIDError { get; set; }

        /// <summary>
        /// Sucursal de la Cuenta
        /// </summary>
        public long SucursalCuenta { get; set; }

        /// <summary>
        /// Numero de Cuenta
        /// </summary>
        public long NumeroCuenta { get; set; }

        /// <summary>
        /// Codigo de Fondo
        /// </summary>
        public string CodigoFondo { get; set; }

        /// <summary>
        /// Saldo Rescatado
        /// </summary>
        public decimal? SaldoRescatadoFondo { get; set; }

        /// <summary>
        /// Moneda de la cuenta
        /// </summary>
        public string MonedaCta { get; set; }
    }
}
