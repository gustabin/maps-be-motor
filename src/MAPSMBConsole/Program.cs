using Isban.MapsMB.Entity.Request;
using Isban.Mercados.LogTrace;
using Isban.Mercados.Security.Adsec.Entity;
using Isban.Mercados.Service.InOut;
using Isban.Mercados.WebApiClient;
using System.Configuration;
using System.Linq;

namespace MotorConsole
{
    class MMAPSSAF
    {
        static void Main(string[] args)
        {
            using (
                var token = PerformanceHelper.Instance.BeginTrace("MotorConsole",
                    (typeof(MMAPSSAF).GetType().ToString()), "Main"))
            {
                switch (args.FirstOrDefault())
                {
                    case "SUS":
                        EjecutarWfSuscripcion();
                        break;
                    case "RES":
                        EjecutarWfRescate();
                        break;
                    case "RTF":
                        EjecutarWfRTF();
                        break;
                    case "RTFPDF":
                        EjecutarWfRTFPDF();
                        break;
                    case "CTR":
                        EjecutarWfRepatriacion();
                        break;
                    case "ACT":
                        EjecutarWfAperturaCuentas();
                        break;
                    default:
                        EjecutarWfSAF();
                        break;
                }


            }
        }

        //[WebApiService("WorkflowSAF")]
        private static void EjecutarWfSAF()
        {
            using (
                var token = PerformanceHelper.Instance.BeginTrace("MotorConsole",
                    (typeof(MMAPSSAF).GetType().ToString()), "EjecutarWfSAF"))
            {
                WorkflowSAFReq workflow = new WorkflowSAFReq
                {
                    Canal = "89",
                    SubCanal = "0000",
                    Usuario = "SRVCA4PROD",
                    Ip = "1.1.1.1"
                };
                RequestSecurity<WorkflowSAFReq> entity = new RequestSecurity<WorkflowSAFReq>
                {
                    Canal = "89",
                    SubCanal = "0000",
                    DatosFirmado = DatosFirmado.No,
                    TipoHash = TipoHash.B64,
                    Dato = "SFA",
                    Firma = "kjdnskj"
                };
                entity.Datos = workflow;

                var a = CallWebApiSingletone.Instance.CallWebApiMethod<Response<dynamic>, RequestSecurity<WorkflowSAFReq>>(
                        entity,
                        ConfigurationManager.AppSettings["UriService"], TipoJson.Newtonsoft);
            }

        }

        private static void EjecutarWfRescate()
        {
            using (
                var token = PerformanceHelper.Instance.BeginTrace("MotorConsole",
                    (typeof(MMAPSSAF).GetType().ToString()), "EjecutarWfSAF"))
            {
                WorkflowSAFReq workflow = new WorkflowSAFReq
                {
                    Canal = "89",
                    SubCanal = "0000",
                    Usuario = "SRVCA4PROD",
                    Ip = "1.1.1.1"
                };
                RequestSecurity<WorkflowSAFReq> entity = new RequestSecurity<WorkflowSAFReq>
                {
                    Canal = "89",
                    SubCanal = "0000",
                    DatosFirmado = DatosFirmado.No,
                    TipoHash = TipoHash.B64,
                    Dato = "SFA",
                    Firma = "kjdnskj"
                };
                entity.Datos = workflow;

                var a = CallWebApiSingletone.Instance.CallWebApiMethod<Response<dynamic>, RequestSecurity<WorkflowSAFReq>>(
                        entity,
                        ConfigurationManager.AppSettings["UriRescate"], TipoJson.Newtonsoft);
            }

        }

        private static void EjecutarWfSuscripcion()
        {
            using (
                var token = PerformanceHelper.Instance.BeginTrace("MotorConsole",
                    (typeof(MMAPSSAF).GetType().ToString()), "EjecutarWfSuscripciones"))
            {
                WorkflowSAFReq workflow = new WorkflowSAFReq
                {
                    Canal = "89",
                    SubCanal = "0000",
                    Usuario = "SRVCA4PROD",
                    Ip = "1.1.1.1"
                };
                RequestSecurity<WorkflowSAFReq> entity = new RequestSecurity<WorkflowSAFReq>
                {
                    Canal = "89",
                    SubCanal = "0000",
                    DatosFirmado = DatosFirmado.No,
                    TipoHash = TipoHash.B64,
                    Dato = "SFA",
                    Firma = "kjdnskj"
                };
                entity.Datos = workflow;

                var a = CallWebApiSingletone.Instance.CallWebApiMethod<Response<dynamic>, RequestSecurity<WorkflowSAFReq>>(
                        entity,
                        ConfigurationManager.AppSettings["UriSuscripciones"], TipoJson.Newtonsoft);
            }

        }

        private static void EjecutarWfRTF()
        {
            using (
                var token = PerformanceHelper.Instance.BeginTrace("MotorConsole",
                    (typeof(MMAPSSAF).GetType().ToString()), "EjecutarWfAltaRTF"))
            {
                WorkflowSAFReq workflow = new WorkflowSAFReq
                {
                    Canal = "04",
                    SubCanal = "0999",
                    Usuario = "SRVCA4PROD",
                    Ip = "1.1.1.1"
                };
                RequestSecurity<WorkflowSAFReq> entity = new RequestSecurity<WorkflowSAFReq>
                {
                    Canal = "04",
                    SubCanal = "0999",
                    DatosFirmado = DatosFirmado.No,
                    TipoHash = TipoHash.B64,
                    Dato = "SFA",
                    Firma = "kjdnskj"
                };
                entity.Datos = workflow;

                var a = CallWebApiSingletone.Instance.CallWebApiMethod<Response<dynamic>, RequestSecurity<WorkflowSAFReq>>(
                        entity,
                        ConfigurationManager.AppSettings["UriRTF"], TipoJson.Newtonsoft);
            }

        }

        private static void EjecutarWfRTFPDF()
        {
            using (
                var token = PerformanceHelper.Instance.BeginTrace("MotorConsole",
                    (typeof(MMAPSSAF).GetType().ToString()), "EjecutarWfAltaRTF"))
            {
                WorkflowSAFReq workflow = new WorkflowSAFReq
                {
                    Canal = "04",
                    SubCanal = "0999",
                    Usuario = "SRVCA4PROD",
                    Ip = "1.1.1.1"
                };
                RequestSecurity<WorkflowSAFReq> entity = new RequestSecurity<WorkflowSAFReq>
                {
                    Canal = "04",
                    SubCanal = "0999",
                    DatosFirmado = DatosFirmado.No,
                    TipoHash = TipoHash.B64,
                    Dato = "SFA",
                    Firma = "kjdnskj"
                };
                entity.Datos = workflow;

                var a = CallWebApiSingletone.Instance.CallWebApiMethod<Response<dynamic>, RequestSecurity<WorkflowSAFReq>>(
                        entity,
                        ConfigurationManager.AppSettings["UriRTFPDF"], TipoJson.Newtonsoft);
            }

        }

        private static void EjecutarWfRepatriacion()
        {
            using (
                var token = PerformanceHelper.Instance.BeginTrace("MotorConsole",
                    (typeof(MMAPSSAF).GetType().ToString()), "EjecutarWfRepatriacion"))
            {
                WorkflowCTRReq workflow = new WorkflowCTRReq
                {
                    Canal = "04",
                    SubCanal = "0999",
                    Usuario = "SRVCA4PROD",
                    Ip = "1.1.1.1",
                    IdServicio = "CTR"
                };
                RequestSecurity<WorkflowCTRReq> entity = new RequestSecurity<WorkflowCTRReq>
                {
                    Canal = "04",
                    SubCanal = "0999",
                    DatosFirmado = DatosFirmado.No,
                    TipoHash = TipoHash.B64,
                    Dato = "SFA",
                    Firma = "kjdnskj"
                };
                entity.Datos = workflow;

                var a = CallWebApiSingletone.Instance.CallWebApiMethod<Response<dynamic>, RequestSecurity<WorkflowCTRReq>>(
                        entity,
                        ConfigurationManager.AppSettings["UriRepatriacion"], TipoJson.Newtonsoft);
            }

        }

        private static void EjecutarWfAperturaCuentas()
        {
            using (
                var token = PerformanceHelper.Instance.BeginTrace("MotorConsole",
                    (typeof(MMAPSSAF).GetType().ToString()), "EjecutarWfRepatriacion"))
            {
                WorkflowCTRReq workflow = new WorkflowCTRReq
                {
                    Canal = "04",
                    SubCanal = "0999",
                    Usuario = "SRVCA4PROD",
                    Ip = "1.1.1.1",
                    IdServicio = "ACT"
                };
                RequestSecurity<WorkflowCTRReq> entity = new RequestSecurity<WorkflowCTRReq>
                {
                    Canal = "04",
                    SubCanal = "0999",
                    DatosFirmado = DatosFirmado.No,
                    TipoHash = TipoHash.B64,
                    Dato = "SFA",
                    Firma = "kjdnskj"
                };
                entity.Datos = workflow;

                var a = CallWebApiSingletone.Instance.CallWebApiMethod<Response<dynamic>, RequestSecurity<WorkflowCTRReq>>(
                        entity,
                        ConfigurationManager.AppSettings["UriAltaCuentas"], TipoJson.Newtonsoft);
            }

        }
    }
}
