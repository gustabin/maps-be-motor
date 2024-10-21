namespace Isban.MapsMB.Entity.Request
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class ObtenerMensajesMepReq 
    {
        [DataMember]
        public DateTime Fecha { get; set; }
        
        [DataMember]
        public string Usuario { get; set; }

        [DataMember]
        public string Ip { get; set; }

    }
}
