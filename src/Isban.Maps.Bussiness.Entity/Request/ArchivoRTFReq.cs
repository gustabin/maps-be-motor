using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.Common.Entity.Request
{
    public class ArchivoRTFReq : EntityBase
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string Cuenta { get; set; }
        [DataMember]
        public string Nup { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public string Nombre { get; set; }
        [DataMember]
        public bool Visto { get; set; }
        [DataMember]
        public string Path { get; set; }
        [DataMember]
        public int? Anio { get; set; }
        [DataMember]
        public string Batch { get; set; }
    }
}
