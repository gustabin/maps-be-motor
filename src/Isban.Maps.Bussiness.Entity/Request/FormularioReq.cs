using System;
using System.Runtime.Serialization;

namespace Isban.MapsMB.Common.Entity.Request
{
    [DataContract]
    public class FormularioReq : EntityBase
    {
        [DataMember]
        public string Estado { get; set; }

        [DataMember]
        public long? IdAdhesion { get; set; }

        [DataMember]
        public string IdServicio { get; set; }

        [DataMember]
        public long? IdSimulacion { get; set; }

        public override bool Validar()
        {
            throw new NotImplementedException();
        }
    }
}
