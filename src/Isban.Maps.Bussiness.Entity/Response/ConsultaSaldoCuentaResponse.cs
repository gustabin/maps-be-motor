using System.Runtime.Serialization;

namespace Isban.MapsMB.Entity.Response
{
    [DataContract]
    public class ConsultaSaldoCuentaResponse
    {
        [DataMember]
        public string Codigo_retorno_extendido { get; set; }
        [DataMember]
        public string Clase_Paquete { get; set; }
        [DataMember]
        public string Tipo_Cuenta { get; set; }
        [DataMember]
        public string Sucursal_Cuenta { get; set; }
        [DataMember]
        public string Nro_Cuenta { get; set; }
        [DataMember]
        public string Nombre_Titular_Cuenta { get; set; }
        [DataMember]
        public string Tipo_Cobertura_Cuenta { get; set; }
        [DataMember]
        public string Tipo_Posicionamiento_Cuenta { get; set; }

        [DataMember]
        public string Saldo_Cuenta { get; set; }
        [DataMember]
        public string Saldo_impago { get; set; }
        [DataMember]
        public string Saldo_por_Conformar { get; set; }
        [DataMember]
        public string Depositos_Efectivo { get; set; }
        [DataMember]
        public string Depositos_24hs_CC { get; set; }
        [DataMember]
        public string Depositos_48hs_CC { get; set; }
        [DataMember]
        public string Depositos_72hs_CC { get; set; }
        [DataMember]
        public string Intereses_Ganados_CA { get; set; }
        [DataMember]
        public string Limite_Acuerdo_CC { get; set; }
        [DataMember]
        public string Cheques_Rechazados { get; set; }
        [DataMember]
        public string Fecha_Apertura { get; set; }

        [DataMember]
        public string Saldo_Cuenta_D { get; set; }
        [DataMember]
        public string Saldo_impago_D { get; set; }
        [DataMember]
        public string Saldo_por_Conformar_D { get; set; }
        [DataMember]
        public string Depositos_Efectivo_D { get; set; }
        [DataMember]
        public string Depositos_24hs_CC_D { get; set; }
        [DataMember]
        public string Depositos_48hs_CC_D { get; set; }
        [DataMember]
        public string Depositos_72hs_CC_D { get; set; }
        [DataMember]
        public string Intereses_Ganados_CA_D { get; set; }
        [DataMember]
        public string Limite_Acuerdo_CC_D { get; set; }
        [DataMember]
        public string Cheques_Rechazados_D { get; set; }
        [DataMember]
        public string Fecha_Apertura_D { get; set; }

        [DataMember]
        public string Indicador_direcciona { get; set; }
        [DataMember]
        public string Saldo_ACTE { get; set; }
        [DataMember]
        public string Saldo_ACAH { get; set; }
        [DataMember]
        public string Saldo_ACCD { get; set; }
        [DataMember]
        public string Saldo_ACAD { get; set; }
        [DataMember]
        public string Tope_USD_cuenta { get; set; }
        [DataMember]
        public string Tope_ARS_cuenta { get; set; }
        [DataMember]
        public string Acumulado_USD_cuenta { get; set; }
        [DataMember]
        public string Acumulado_ARS_cuenta { get; set; }
        [DataMember]
        public string Disponible_USD_cuenta { get; set; }
        [DataMember]
        public string Disponible_ARS_cuenta { get; set; }
        [DataMember]
        public string Disponible_USD_menor_cuenta_conjunto { get; set; }
        [DataMember]
        public string Disponible_ARS_menor_cuenta_conjunto { get; set; }
        [DataMember]
        public string Dispuesto_USD_cuenta { get; set; }
        [DataMember]
        public string Dispuesto_ARS_cuenta { get; set; }
        [DataMember]
        public string Fecha_Ultima_Operacion { get; set; }
        [DataMember]
        public string Ind_Sobregiro { get; set; }
        [DataMember]
        public string Estado { get; set; }

    }

}