namespace Isban.MapsMB.Entity.Request
{
    using Common.Entity;
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class LoadSaldosRequest
    {

        [DataMember]
        public string Cuenta { get; set; }

        [DataMember]
        public DateTime FechaDesde { get; set; }

        [DataMember]
        public DateTime FechaHasta { get; set; }

        [DataMember]
        public string Canal { get; set; }

        [DataMember]
        public string Usuario { get; set; }

        [DataMember]
        public string Password { get; set; }
    }
}
