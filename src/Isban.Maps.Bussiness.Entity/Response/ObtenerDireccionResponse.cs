using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.Common.Entity.Response
{
    [DataContract]
    public class ObtenerDireccionResponse
    {
        [DataMember]
        public string CodigoPostal { get; set; }

        [DataMember]
        public string Calle { get; set; }
        [DataMember]
        public string Numero { get; set; }
        [DataMember]
        public string Piso { get; set; }
        [DataMember]
        public string Depto { get; set; }
        [DataMember]
        public string Localidad { get; set; }
    }
}
