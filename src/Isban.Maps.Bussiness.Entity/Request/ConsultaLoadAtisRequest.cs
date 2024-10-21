namespace Isban.MapsMB.Entity.Request
{
    using Common.Entity;
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class ConsultaLoadAtisRequest
    {
        [DataMember]
        public long? Nup { get; set; }

        [DataMember]
        public long? CuentaBp { get; set; }

    }
}
