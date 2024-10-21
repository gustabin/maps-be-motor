

namespace Isban.MapsMB.Common.Entity.Response
{
    using Isban.MapsMB.Entity.Response;
    using System.Collections.Generic;
    public class ConfirmaRiesgoResponse
    {
        public ConfirmaRiesgoResponse()
        {
            Confirmadas = new List<CargaSolicitudOrden>();
            NoConfirmadas = new List<CargaSolicitudOrden>();
        }
        
        public List<CargaSolicitudOrden> Confirmadas { get; set; }
        public List<CargaSolicitudOrden> NoConfirmadas { get; set; }
    }
}
