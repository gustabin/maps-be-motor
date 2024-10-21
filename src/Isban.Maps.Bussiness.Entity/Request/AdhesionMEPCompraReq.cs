using System.Runtime.Serialization;

namespace Isban.MapsMB.Common.Entity.Request
{
    public class AdhesionMEPCompraReq
    {
        [DataMember]
        public long IdAdhesion { get; set; }

        [DataMember]
        public string ComprobanteCompra { get; set; }

        [DataMember]
        public decimal CantidadNominales { get; set; }

        [DataMember]
        public string Estado { get; set; }

        [DataMember]
        public string Usuario { get; set; }

        [DataMember]
        public string Ip { get; set; }
    }
}
