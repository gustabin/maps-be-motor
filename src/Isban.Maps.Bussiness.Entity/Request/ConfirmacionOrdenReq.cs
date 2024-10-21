using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.Common.Entity
{
    public class ConfirmacionOrdenReq : EntityBase
    {
        [DataMember]
        public int IdEvaluacion { get; set; }

        [DataMember]
        public string IdOrden { get; set; }

        public override bool Validar()
        {
            throw new NotImplementedException();
        }
    }
}
