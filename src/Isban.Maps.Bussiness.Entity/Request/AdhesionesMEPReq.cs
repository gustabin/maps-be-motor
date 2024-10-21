using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Isban.MapsMB.Common.Entity.Request
{
    public class AdhesionesMEPReq 
    {
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Fecha { get; set; }

        [DataMember]
        public string Usuario { get; set; }

        [DataMember]
        public string Ip { get; set; }
    }
}
