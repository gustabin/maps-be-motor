
namespace Isban.MapsMB.Common.Entity.Request
{
    using System.Xml.Serialization;

    [XmlRoot("MENSAJE")]
    public class MensajeMyA
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
        public string TITULO { get; set; }

        public string CHANNEL_BANK { get; set; }
        public string SUBCHANNEL_BANK { get; set; }
        public Fecha FECHA_ACTUAL { get; set; }
        //public Notificacion TRANSFERENCIA { get; set; }
        //[XmlElement("TIPO-MENSAJE")]
        //public string TIPO_MENSAJE { get; set; }
        [XmlElement("NRO-COMPROBANTE")]
        public string NRO_COMPROBANTE { get; set; }
        [XmlElement("FONDO")]
        public string FONDO { get; set; }
        [XmlElement("CUENTA-TITULOS")]
        public string CUENTA_TITULOS { get; set; }
        [XmlElement("CUENTA-DEBITO")]
        public string CUENTA_DEBITO { get; set; }
        [XmlElement("FECHA")]
        public string FECHA { get; set; }
        [XmlElement("CANAL-BAJA")]
        public string CANAL_BAJA { get; set; }
        public string DESCRIPCION { get; set; }
    }

    public class Fecha
    {
        public string DIA { get; set; }
        public string MES { get; set; }
        public string ANO { get; set; }
        public string HORA { get; set; }
    }

    public class Notificacion
    {
        [XmlElement("NOMBRE")]
        public string NOMBRE { get; set; }
        [XmlElement("APELLIDO")]
        public string APELLIDO { get; set; }
        [XmlElement("TIPO-MENSAJE")]
        public string TIPO_MENSAJE { get; set; }
        [XmlElement("NRO-COMPROBANTE")]
        public string NRO_COMPROBANTE { get; set; }
        [XmlElement("FONDO")]
        public string FONDO { get; set; }
        [XmlElement("CUENTA-TITULOS")]
        public string CUENTA_TITULOS { get; set; }
        [XmlElement("CUENTA-DEBITO")]
        public string CUENTA_DEBITO { get; set; }
        [XmlElement("FECHA")]
        public string FECHA { get; set; }
        [XmlElement("CANAL-BAJA")]
        public string CANAL_BAJA { get; set; }
        [XmlElement("TEXTO")]
        public string TEXTO { get; set; }
        //public CuentaDebito CUENTA_DEBITO { get; set; }
        //public CuentaCredito CUENTA_CREDITO { get; set; }
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
