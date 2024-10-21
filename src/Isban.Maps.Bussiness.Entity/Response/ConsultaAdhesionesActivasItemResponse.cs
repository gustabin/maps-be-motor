using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Isban.MapsMB.Entity.Response
{
    [DataContract]
    public class ConsultaAdhesionesActivasItemResponse
    {
        [DataMember]
        public decimal ID_SOLICITUD_ORDENES { get; set; }
        [DataMember]
        public decimal ID_ADHESIONES { get; set; }
        [DataMember]
        public decimal COD_ALTA_ADHESION { get; set; }
        [DataMember]
        public string ID_NUP { get; set; }
        [DataMember]
        public string ID_SEGMENTO { get; set; }
        [DataMember]
        public string ID_SERVICIO { get; set; }
        [DataMember]
        public string CANAL { get; set; }
        [DataMember]
        public string SUBCANAL { get; set; }
        [DataMember]
        public decimal CUENTA_TITULOS { get; set; }
        [DataMember]
        public decimal SUC_CTA_OPER { get; set; }
        [DataMember]
        public decimal NRO_CTA_OPER { get; set; }
        [DataMember]
        public decimal TIPO_CTA_OPER { get; set; }
        [DataMember]
        public decimal SALDO_CUENTA_ANTES { get; set; }
        [DataMember]
        public string COD_MONEDA { get; set; }
        [DataMember]
        public System.DateTime FECHA_PROCESO { get; set; }
        [DataMember]
        public decimal IN_REPROCESO { get; set; }
        [DataMember]
        public string COD_ESTADO_PROCESO { get; set; }
        [DataMember]
        public string OBSERVACION { get; set; }
        [DataMember]
        public string DE_DISCLAIMER { get; set; }
        [DataMember]
        public System.DateTime FE_VIGENCIA_DESDE { get; set; }
        [DataMember]
        public System.DateTime FE_VIGENCIA_HASTA { get; set; }
        [DataMember]
        public decimal SALDO_MIN_OPERACION { get; set; }
        [DataMember]
        public decimal SALDO_MAX_OPERACION { get; set; }
        [DataMember]
        public decimal SALDO_ENVIADO { get; set; }
        [DataMember]
        public string COD_FONDO { get; set; }
        [DataMember]
        public string TIPO_ESPECIE { get; set; }
        [DataMember]
        public decimal SALDO_MIN_POR_FONDO { get; set; }
        [DataMember]
        public decimal SALDO_RESCATE_POR_FONDO { get; set; }
        [DataMember]
        public decimal NU_ORDEN_ORIGEN { get; set; }


    }

}