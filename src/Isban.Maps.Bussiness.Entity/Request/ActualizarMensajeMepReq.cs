using System;
using System.Runtime.Serialization;

namespace Isban.MapsMB.Common.Entity.Request
{
    public class ActualizarMensajeMepReq
    {
        [DataMember]
        public long IdAdhesiones { get; set; }

        [DataMember]
        public string Estado { get; set; }

        [DataMember]
        public string Usuario { get; set; }

        [DataMember]
        public string Ip { get; set; }
    }
}
