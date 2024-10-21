using System;
using System.Runtime.Serialization;

namespace Isban.MapsMB.Common.Entity.Request
{
    public class ActualizarOrdenReq
    {
        [DataMember]
        public DateTime Fecha { get; set; }
        
        [DataMember]
        public string Nup { get; set; }

        [DataMember]
        public decimal Cuenta { get; set; }

        [DataMember]
        public decimal NumOrden { get; set; }

        [DataMember]
        public string Usuario { get; set; }

        [DataMember]
        public string Ip { get; set; }
    }
}
