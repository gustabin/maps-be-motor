

namespace Isban.MapsMB.Common.Entity.Response
{
    using Isban.MapsMB.Entity.Response;
    using System.Collections.Generic;
    public class EvaluaRiesgoResponse
    {
        public EvaluaRiesgoResponse()
        {
            Restrictivo = new List<CargaSolicitudOrden>();
            NoRestrictivo = new List<CargaSolicitudOrden>();
        }
        
        public List<CargaSolicitudOrden> Restrictivo { get; set; }
        public List<CargaSolicitudOrden> NoRestrictivo { get; set; }
    }
}
