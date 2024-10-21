namespace Isban.MapsMB.Entity.Response
{
    using Common.Entity;
    using Isban.MapsMB.Entity.Constantes.Estructuras;
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class AdheVigenciaVencida //: EntityBase
    {
        [DataMember]
        public long CodAltaAdhesion { get; set; }

        [DataMember]
        public string Nup { get; set; }

        [DataMember]
        public string Segmento { get; set; }
    }
}
