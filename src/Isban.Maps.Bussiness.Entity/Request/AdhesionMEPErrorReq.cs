using System.Runtime.Serialization;

namespace Isban.MapsMB.Common.Entity.Request
{
    public class AdhesionMEPErrorReq
    {
        [DataMember]
        public long IdAdhesion { get; set; }

        [DataMember]
        public string Estado { get; set; }

        [DataMember]
        public string Observacion { get; set; }

        [DataMember]
        public string Usuario { get; set; }

        [DataMember]
        public string Ip { get; set; }
    }
}
