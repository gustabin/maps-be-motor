using System.Runtime.Serialization;

namespace Isban.MapsMB.Entity.Response
{
    [DataContract]
    public class AltaAdhesionMAPSResponse
    {
        [DataMember]
        public string Status_Resultado_Extendido { get; set; }
        [DataMember]
        public string Descripcion_Moneda { get; set; }
        [DataMember]
        public decimal Numero_Certificado { get; set; }
        [DataMember]
        public decimal Importe_Neto { get; set; }
        [DataMember]
        public decimal Porcentaje_Comis { get; set; }
        [DataMember]
        public decimal Valor_Comision { get; set; }
        [DataMember]
        public string Estado_Actual { get; set; }
        [DataMember]
        public string Motivo_Actual { get; set; }
        [DataMember]
        public decimal Cotizac_Cambio { get; set; }
        [DataMember]
        public decimal Dias_Carencia { get; set; }
        [DataMember]
        public string Nombre_fondo { get; set; }
        [DataMember]
        public decimal Importe_moneda_fondo { get; set; }

    }

}