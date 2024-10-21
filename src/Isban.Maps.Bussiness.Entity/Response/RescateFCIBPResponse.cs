using System.Runtime.Serialization;

namespace Isban.MapsMB.Entity.Response
{
    [DataContract]
    public class RescateFCIBPResponse
    {
        [DataMember]
        public string Descripcion_Moneda { get; set; }

        [DataMember]
        public string Estado_Actual { get; set; }

        [DataMember]
        public string Motivo_Actual { get; set; }

        [DataMember]
        public string Nombre_fondo { get; set; }

        [DataMember]
        public decimal Importe_rescate { get; set; }

        [DataMember]
        public decimal Total_cuotas_Partes { get; set; }

        [DataMember]
        public decimal Monto_Cambio { get; set; }

        [DataMember]
        public decimal Cotacao_pact { get; set; }

        [DataMember]
        public string Numero_de_Certificado { get; set; }

    }
}