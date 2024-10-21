using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.Common.Entity.Response
{
    [DataContract]
    public class CuentasActivasRTF
    {
        [DataMember]
        public string Nup { get; set; }

        [DataMember]
        public long CuentaTitulo { get; set; }
    }
}
