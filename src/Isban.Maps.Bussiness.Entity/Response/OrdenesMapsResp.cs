using System.Runtime.Serialization;

namespace Isban.MapsMB.Common.Entity.Response
{
    [DataContract]
    public class OrdenesMapsResp
    {
        [DataMember]
        public string Nup { get; set; }

        [DataMember]
        public long CuentaTitulos { get; set; }

        [DataMember]
        public string ComprobanteCompra { get; set; }
    }
}
