namespace Isban.MapsMB.Entity.Request
{
    using Common.Entity;
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class ActualizarMensajeRTF : EntityBase
    {
        [DataMember]
        public string Nup { get; set; }
        [DataMember]
        public string Estado { get; set; }
        [DataMember]
        public long Cuenta { get; set; }
        [DataMember]
        public string Observacion { get; set; }
    }
}
