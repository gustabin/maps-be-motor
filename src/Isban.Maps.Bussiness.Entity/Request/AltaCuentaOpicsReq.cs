using System.Runtime.Serialization;

namespace Isban.MapsMB.Common.Entity.Request
{
    [DataContract]
    public class AltaCuentaOpicsReq : EntityBase
    {
        [DataMember]
        public long? CuentaTitulo { get; set; }
    }
}
