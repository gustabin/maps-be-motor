namespace Isban.MapsMB.Entity.Request
{
    using Common.Entity;
    using System.Runtime.Serialization;

    [DataContract]
    public class ObtenerTextoMepReq 
    {
        [DataMember]
        public long IdAdhesion { get; set; }

        [DataMember]
        public string Usuario { get; set; }

        [DataMember]
        public string Ip { get; set; }
    }
}
