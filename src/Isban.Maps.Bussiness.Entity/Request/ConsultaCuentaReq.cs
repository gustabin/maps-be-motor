using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.Common.Entity.Request
{
    [DataContract]
    public class ConsultaCuentaReq
    {
        [DataMember]
        public long? Id { get; set; }

        [DataMember]
        public decimal? SaldoActual { get; set; }

        [DataMember]
        public string Observacion { get; set; }

        [DataMember]
        public string Usuario { get; set; }

        [DataMember]
        public string Ip { get; set; }
    }
}
