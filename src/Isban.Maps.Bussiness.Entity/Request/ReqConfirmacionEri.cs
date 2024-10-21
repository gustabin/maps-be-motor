

namespace Isban.MapsMB.Common.Entity.Response
{
    using Isban.MapsMB.Entity.Response;
    using System.Collections.Generic;
    public class ReqConfirmacionEri
    {
        public ReqConfirmacionEri()
        {
            ConfirmacionERI = new List<CargaSolicitudOrden>();
            AltaSuscripcion = new List<CargaSolicitudOrden>();            
        }
        /// <summary>
        /// Lista de adhesiones con saldo suficiente para operar
        /// </summary>
        public List<CargaSolicitudOrden> ConfirmacionERI { get; set; }

        /// <summary>
        /// Lista de adhesiones con saldo insuficiente para operar
        /// </summary>
        public List<CargaSolicitudOrden> AltaSuscripcion { get; set; }
               
    }
}
