using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Isban.MapsMB.Entity.Response
{
    [DataContract]
    public class ConsultaAdhesionesActivasResponse
    {
        public ConsultaAdhesionesActivasResponse() {
            AdhesionesActivas = new List<ConsultaAdhesionesActivasItemResponse>();
        }

        [DataMember]
        public List<ConsultaAdhesionesActivasItemResponse> AdhesionesActivas { get; set; }
    }

}