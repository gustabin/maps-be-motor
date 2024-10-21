
namespace Isban.MapsMB.Bussiness
{
    using System.Configuration;
    using System.IO;
    using System.ServiceModel;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using System;
    using System.Reflection;
    using Isban.MapsMB.IBussiness;
    using System.Collections.Generic;

    public class ServicesClient : IServicesClient
    {
        public static BindingFlags bindFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;

        public List<string> GetMailXNup(string canal, string subcanal, string nup)
        {
            var binding = new BasicHttpBinding()
            {
                Name = "getEstadoCliente",
                MaxBufferSize = int.MaxValue,
                MaxReceivedMessageSize = int.MaxValue,
            };

            var emails = new List<string>();

            if (ConfigurationManager.AppSettings["MYA_URL"] != null)
            {
                var er = new Uri(ConfigurationManager.AppSettings["MYA_URL"]);

                var add = new EndpointAddress(er);
                var service = new MapsMB.Business.MYAService.ServicesClient(binding, add);

                //canal = "22";
                subcanal = subcanal.Substring(2, 2);
                string param = string.Format("<xml><header><Servicio>getEstadoCliente</Servicio><Canal>{0}</Canal><SubCanal>{1}</SubCanal><NUP>{2}</NUP></header><datosFirmados></datosFirmados></xml>", canal, subcanal, nup);

                //En caso que venga por error, maps debe continuar -----> Preguntar
                try
                {
                    string response = service.getEstadoCliente(param);

                    XmlSerializer mySerializer = new XmlSerializer(typeof(MapsMB.Business.MYAService.respuesta));

                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml(response);
                    XmlNodeList elemlist = xDoc.GetElementsByTagName("soapenv:Body");

                    MapsMB.Business.MYAService.respuesta oRespuesta = mySerializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(elemlist[0].InnerXml))) as MapsMB.Business.MYAService.respuesta;

                    if (oRespuesta.CodRet == "0")
                    {
                        foreach (MapsMB.Business.MYAService.Destino destino in oRespuesta.Destinos)
                        {
                            if (destino.DestinoTipo.ToUpper() == "MAIL")
                            {
                                ////if (email == string.Empty || destino.DestinoSecuencia == "1")
                                    emails.Add(destino.DestinoDescripcion);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    new List<string>();
                }
            }
            return emails;
        }

    }
}
