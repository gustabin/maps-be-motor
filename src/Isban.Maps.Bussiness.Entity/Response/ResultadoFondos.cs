

namespace Isban.MapsMB.Common.Entity.Response
{
    using Isban.MapsMB.Entity.Response;
    using System.Collections.Generic;
    public class ResultadoFondos
    {
        public ResultadoFondos()
        {
            Ok = new List<CargaSolicitudOrden>();
            NoOk = new List<CargaSolicitudOrden>();
        }
        
        public List<CargaSolicitudOrden> Ok { get; set; }
        public List<CargaSolicitudOrden> NoOk { get; set; }
    }

    public class ResultadoFondosOBE
    {
        public ResultadoFondosOBE()
        {
            Ok = new List<SubscripcionFondoResponse>();
            NoOk = new List<SubscripcionFondoResponse>();
        }

        public List<SubscripcionFondoResponse> Ok { get; set; }
        public List<SubscripcionFondoResponse> NoOk { get; set; }
    }


}
