using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.Common.Entity
{
    public class EnviarMensajesNetReq : EntityBase
    {
        [DataMember]
        public string DatosCliente { get; set; }
    }
}
