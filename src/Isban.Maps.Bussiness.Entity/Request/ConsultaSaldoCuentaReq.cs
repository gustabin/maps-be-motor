using Isban.MapsMB.Common.Entity;
using System.Runtime.Serialization;
using System;

namespace Isban.MapsMB.Entity.Request
{
    public class ConsultaSaldoCuentaReq : EntityBase
    {   
        [DataMember]
        public string TipoCta { get; set; }

        [DataMember]
        public string SucCta { get; set; }

        [DataMember]
        public string NroCta { get; set; }

        public override bool Validar()
        {
            throw new NotImplementedException();
        }
    }
}
