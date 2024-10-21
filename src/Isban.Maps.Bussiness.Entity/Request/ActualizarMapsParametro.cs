using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.Common.Entity.Request
{
    [DataContract]
    public class ActualizarMapsParametro : EntityBase
    {
        [DataMember]
        public string Parametro { get; set; }

        [DataMember]
        public string Sistema { get; set; }

        [DataMember]
        public string Valor { get; set; }
    }
}
