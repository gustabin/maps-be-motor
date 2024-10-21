using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.Common.Entity.Request
{
    [DataContract]
    public class TenenciaValuadaFondosRTFRequest : EntityBase
    {
        [DataMember]
        public string Nup { get; set; }

        [DataMember]
        public string Segmento { get; set; }

        [DataMember]
        public List<long> ListaCuentas { get; set; }

        [DataMember]
        public DateTime InicioPeriodo { get; set; }

        [DataMember]
        public DateTime FinPeriodo { get; set; }

        [DataMember]
        public DatosServicios DatosServicios { get; set; }
    }
}
