namespace Isban.MapsMB.Entity.Controles.Independientes
{
    using System.Runtime.Serialization;

    [DataContract]
    public class DisclaimerERI
    {
        public string CantidadDisclaimer { get; set; }

        [DataMember]
        public int IdEvaluacion { get; set; }

        [DataMember]
        public string TextoDisclaimer { get; set; }

        public short TipoDisclaimer { get; set; }
    }
}
