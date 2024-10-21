

namespace Isban.MapsMB.Entity.Request
{
    using Common.Entity;
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class ActualizarMensaje : EntityBase
    {
        [DataMember]
        public long IdMensaje { get; set; }
        [DataMember]
        public string Estado { get; set; }
    }
}
