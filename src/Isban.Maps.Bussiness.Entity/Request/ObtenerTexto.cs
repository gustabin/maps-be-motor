

namespace Isban.MapsMB.Entity.Request
{
    using Common.Entity;
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class ObtenerTexto : EntityBase
    {
        [DataMember]
        public string Entitlement { get; set; }
        [DataMember]
        public string CodEstadoProceso { get; set; }

        [DataMember]
        public long CodAlta { get; set; }

        [DataMember]
        public string IdServicio { get; set; }

        [DataMember]
        public string Operacion { get; set; }


    }
}
