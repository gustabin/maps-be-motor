

namespace Isban.MapsMB.Entity.Request
{
    using Common.Entity;
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class ObtenerMensajes : EntityBase
    {
        [DataMember]
        public DateTime Fecha { get; set; }

        //[DataMember]
        //public string IdServicio { get; set; }

    }
}
