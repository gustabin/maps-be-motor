

namespace Isban.MapsMB.Common.Entity.Response
{
    using Isban.MapsMB.Entity.Response;
    using System.Collections.Generic;
    public class CondicionAdhesion
    {
        public CondicionAdhesion()
        {
            SaldoInsuficiente = new List<CargaSolicitudOrden>();
            SaldoSuficiente = new List<CargaSolicitudOrden>();            
        }
        /// <summary>
        /// Lista de adhesiones con saldo suficiente para operar
        /// </summary>
        public List<CargaSolicitudOrden> SaldoSuficiente { get; set; }

        /// <summary>
        /// Lista de adhesiones con saldo insuficiente para operar
        /// </summary>
        public List<CargaSolicitudOrden> SaldoInsuficiente { get; set; }
               
    }
}
