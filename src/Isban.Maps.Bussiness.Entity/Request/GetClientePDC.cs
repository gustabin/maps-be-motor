
namespace Isban.MapsMB.Entity.Request
{
    using Isban.MapsMB.Common.Entity;
    using Isban.MapsMB.Entity.Response;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System;

    [DataContract]
    public class GetClientePDC : EntityBase
    {
        [DataMember]
        public List<ConsultaPdcResponse> Clientes { get; set; }

        public override bool Validar()
        {
            throw new NotImplementedException();
        }
    }
}
