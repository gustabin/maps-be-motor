
namespace Isban.MapsMB.Common.Entity.Response
{
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class BajaOperacionResp
    {
        [DataMember]
        public string Nup { get; set; }

        [DataMember]
        public string IdServicio { get; set; }

        [DataMember]
        public long? IdAdhesion { get; set; }

        [DataMember]
        public string Segmento { get; set; }

        [DataMember]
        public string Canal { get; set; }

        [DataMember]
        public string SubCanal { get; set; }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Dato { get; set; }

        [DataMember]
        public string Firma { get; set; }

        [DataMember]
        public long? IdSimulacion { get; set; }

        [DataMember]
        public string Estado { get; set; }

        [DataMember]
        public virtual List<JObject> Items { get; set; }
    }
}
