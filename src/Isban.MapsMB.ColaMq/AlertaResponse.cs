
namespace Isban.MapsMB.ColaMq
{
    using System.Xml.Serialization;

    [XmlRoot("AlertaRequest")]
    public class AlertaResponse
    {
        [XmlElement("ALERTADETALLE")]
        public string RESPUESTA { get; set; }
    }
}
