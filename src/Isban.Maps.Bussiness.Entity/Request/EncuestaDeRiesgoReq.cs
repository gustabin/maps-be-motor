using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.Common.Entity
{
    public class EncuestaDeRiesgoReq : EntityBase
    {  
        [DataMember]
        public decimal IdSolicitud { get; set; }

        [DataMember]
        public string CodMoneda { get; set; }
        
        public override bool Validar()
        {
            throw new NotImplementedException();
        }
    }
}
