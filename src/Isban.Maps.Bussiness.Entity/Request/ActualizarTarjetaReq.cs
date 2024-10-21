using System;
using System.Runtime.Serialization;

namespace Isban.MapsMB.Common.Entity.Request
{
    public class ActualizarTarjetaReq
    {
        [DataMember]
        public string Parametro { get; set; }

        [DataMember]
        public string Usuario { get; set; }

        [DataMember]
        public string Ip { get; set; }
    }
}
