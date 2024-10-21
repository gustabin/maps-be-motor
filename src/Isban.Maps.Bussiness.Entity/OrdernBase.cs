using Isban.MapsMB.Entity.Request;
using System.Runtime.Serialization;

namespace Isban.MapsMB.Common.Entity
{
    [DataContract]
    public class OrdernBase
    {
        [DataMember]
        public CabeceraConsulta Cabecera { get; set; }
    }
}
