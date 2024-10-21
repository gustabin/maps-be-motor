using Isban.MapsMB.Common.Entity.Request;
using Isban.MapsMB.Common.Entity.Response;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Xml;

namespace Isban.MapsMB.FaxMailRTF
{
    public class EnvioFaxMailService 
    {
        public FaxMailResponse EnvioMailServidor(FaxMailParameter faxMailParameter)
        {
            FaxMailResponse result = new FaxMailResponse();
            result.Exitoso = true;

            try
            {
                var listAdjuntos = new List<string>();

                foreach (var item in faxMailParameter.Adjuntos)
                {
                    listAdjuntos.Add(DireccionBase64String(item));
                }

                var archivoAdjunto = string.Join(",", listAdjuntos);

                #region Creacion de xml
                string cadena = @"<?xml version='1.0' encoding='utf-8'?>
                        <soap12:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap12='http://www.w3.org/2003/05/soap-envelope'>
                          <soap12:Body>
                            <EnviarMensaje xmlns='http://intranet.ar.bsch/'>
                              <tipo>" + faxMailParameter.Tipo + @"</tipo>
                              <usuario>" + faxMailParameter.Usuario + @"</usuario>
                              <emisor>" + faxMailParameter.Emisor + @"</emisor>
                              <receptores>" + XML_String(faxMailParameter.Receptores) + @"</receptores>
                              <copia>" + XML_String(faxMailParameter.Copia) + @"</copia>
                              <copiaOculta>" + XML_String(faxMailParameter.CopiaOculta) + @"</copiaOculta>
                              <responderA>" + faxMailParameter.ResponderA + @"</responderA>
                              <asunto>" + faxMailParameter.Asunto + @"</asunto>
                              <isHtml>" + faxMailParameter.EsHtml.ToString().ToLower() + @"</isHtml>
                              <mensaje>" + faxMailParameter.Mensaje + @"</mensaje>
                              <tipoAdjunto>" + XML_String(faxMailParameter.TipoAdjunto) + @"</tipoAdjunto>
                              <nombreAdjunto>" + XML_String(faxMailParameter.NombreAdjunto) + @"</nombreAdjunto>
                              <adjunto>" + XML_String(archivoAdjunto) + @"</adjunto>
                              <acuseRecibo>" + faxMailParameter.AcuseRecibo.ToString().ToLower() + @"</acuseRecibo>
                              <acuseLectura>" + faxMailParameter.AcuseLectura.ToString().ToLower() + @"</acuseLectura>
                              <prioridad>" + faxMailParameter.Prioridad.ToLower() + @"</prioridad>
                            </EnviarMensaje>
                          </soap12:Body>
                        </soap12:Envelope>";

                #endregion

                #region Llamada al WS  

                Byte[] bdata = System.Text.Encoding.ASCII.GetBytes(cadena);
                // instantiate a web client
                System.Net.WebClient wc = new System.Net.WebClient();
                Byte[] bresp;
                // add appropriate headers
                wc.Headers.Add("Content-Type", "text/xml");
                wc.Credentials = CredentialCache.DefaultCredentials;

                bresp = wc.UploadData(ConfigurationManager.AppSettings["FAXMAIL_URL"], bdata);
                string resp = System.Text.Encoding.ASCII.GetString(bresp);
                XmlDocument xresp = new XmlDocument();
                xresp.LoadXml(resp);

                result.Descripcion = "Mensaje Enviado por FaxMail";

                return result;
            }
            catch (Exception ex)
            {
                result.Exitoso = false;
                result.Descripcion = ex.Message;

                return result;
            }

             #endregion
        }

        public string XML_String(string entrada)
        {
            string[] arrValor = entrada.Split(',').ToArray();
            string valorFinal = string.Empty;
            string ADELANTE = "<string>";
            string ATRAS = "</string>";

            foreach (string valor in arrValor)
            {
                valorFinal = valorFinal + ADELANTE + valor + ATRAS;
            }
            return valorFinal;

        }

        public string DireccionBase64String(string direccionString)
        {
            var BinaryFile = File.ReadAllBytes(direccionString);
            return Convert.ToBase64String(BinaryFile);
        }
    }
}
