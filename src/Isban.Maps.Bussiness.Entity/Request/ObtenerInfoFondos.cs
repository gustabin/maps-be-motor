using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.Common.Entity.Request
{
    [DataContract]
    public class ObtenerInfoFondos : EntityBase
    {
        [DataMember]
        public long CuentaTitulo { get; set; }
    }
}
