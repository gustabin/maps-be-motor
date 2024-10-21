using System.Runtime.Serialization;

namespace Isban.MapsMB.Entity.Response
{
    [DataContract]
    public class RescateFCIIATXResponse
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
        public long Nro_Rescate { get; set; }

        [DataMember]
        public decimal Importe_Rescate_Neto { get; set; }

        [DataMember]
        public decimal Importe_Rescate_Comision { get; set; }

        [DataMember]
        public decimal Importe_Rescate_Bruto { get; set; }

        [DataMember]
        public decimal Total_cuotas_partes { get; set; }

        [DataMember]
        public decimal Monto_Cambio { get; set; }

        [DataMember]
        public decimal Cotacao_pact { get; set; }

        [DataMember]
        public string Marca_KU { get; set; }

    }
}