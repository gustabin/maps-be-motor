using System.Xml.Serialization;

namespace Isban.MapsMB.ColaMQ
{
    [XmlRoot("MENSAJE")]
    public class Mensaje
    {
        public Header HEADER;
        public Body BODY;
    }

    public class Header
    {
        [XmlElement("RIO-NUP")]
        public string RIONUP { get; set; }

        [XmlElement("RIO-DNI-CUIT")]
        public string RIODNICUIT { get; set; }

        [XmlElement("RIO-CLIENTDATE")]
        public string RIOCLIENTDATE { get; set; }

        public string ENTITLEMENT { get; set; }
        public string TIMESTAMP { get; set; }
        public string TTL { get; set; }
        public string CHANNEL { get; set; }
        public string DESTINATION { get; set; }
        public string LABEL { get; set; }

        [XmlElement("LEGAL-TYPE")]
        public string LEGALTYPE { get; set; }

        [XmlElement("ERROR-CODE")]
        public string ERRORCODE { get; set; }
    }
    public class Body
    {
        public string NOMBRE { get; set; }
        public string APELLIDO { get; set; }

        [XmlElement("TIPO-MENSAJE")]
        public string TIPOMENSAJE { get; set; }
        public string TEXTO { get; set; }

        public string CHANNEL_BANK { get; set; }
        public string SUBCHANNEL_BANK { get; set; }
        public Fecha FECHA_ACTUAL { get; set; }
        public Transferencia TRANSFERENCIA { get; set; }
    }

    public class Fecha
    {
        public string DIA { get; set; }
        public string MES { get; set; }
        public string ANO { get; set; }
        public string HORA { get; set; }
    }

    public class Transferencia
    {
        public string TIPO_MENSAJE { get; set; }
        public string DESTINO_TRANSFERENCIA { get; set; }
        public string PLAZO_ACREDITACION { get; set; }
        public string FECHAINGRESO { get; set; }
        public string MOTIVO_RECHAZO { get; set; }
        public string NRO_COMPROBANTE { get; set; }
        public string TITULAR_CUENTA_CREDITO { get; set; }
        public string MONEDA { get; set; }
        public string IMPORTE { get; set; }
        public string CANAL { get; set; }
        public CuentaDebito CUENTA_DEBITO { get; set; }
        public CuentaCredito CUENTA_CREDITO { get; set; }
    }

    public class CuentaDebito
    {
        public string TIPO_CUENTA_DEBITO { get; set; }
        public string NUMERO_CUENTA_DEBITO { get; set; }
        public string NUMERO_SUCURSAL_DEBITO { get; set; }
    }

    public class CuentaCredito
    {
        public string TIPO_CUENTA_CREDITO { get; set; }
        public string NUMERO_CUENTA_CREDITO { get; set; }
        public string NUMERO_SUCURSAL_CREDITO { get; set; }
        public string CBU_CUENTA_CREDITO { get; set; }
    }


}
