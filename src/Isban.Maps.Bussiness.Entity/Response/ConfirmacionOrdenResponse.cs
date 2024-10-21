using System.Runtime.Serialization;

namespace Isban.MapsMB.Entity.Response
{
    [DataContract]
    public class ConfirmacionOrdenResponse
    {
        [DataMember]
        public int CodigoResp { get; set; }
        [DataMember]
        public string MensajeError { get; set; }

    }

}