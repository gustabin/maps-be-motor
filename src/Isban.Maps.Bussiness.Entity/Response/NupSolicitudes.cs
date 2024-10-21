namespace Isban.MapsMB.Entity.Response
{
    using Common.Entity;
    using Isban.MapsMB.Entity.Constantes.Estructuras;
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class NupSolicitudes 
    {
        [DataMember]
        public string Nup { get; set; }

        [DataMember]
        public string Segmento { get; set; }

        [DataMember]
        public string Canal { get; set; }

        [DataMember]
        public string SubCanal { get; set; }
    }
}
