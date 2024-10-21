

namespace Isban.MapsMB.Entity.Request
{
    using Common.Entity;
    using System.Runtime.Serialization;

    public class ActualizaEvaRiesgoReq : EntityBase
    {

        [DataMember]
        public decimal P_ID_SOLICITUD { get; set; }
        [DataMember]
        public decimal P_NU_EVA_RIESGO { get; set; }
        [DataMember]
        public string P_DE_DISCLAIMER { get; set; }   

    }
}
