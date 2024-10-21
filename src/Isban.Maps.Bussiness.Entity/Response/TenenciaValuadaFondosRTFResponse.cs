using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.Common.Entity.Response
{
    [DataContract]
    public class TenenciaValuadaFondosRTFResponse
    {
        [DataMember]
        public List<CuentaFondoRTF> AgrupadoPorCuenta { get; set; }

        [DataMember]
        public List<string> Legales { get; set; }


    }
}
