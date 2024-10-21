
namespace Isban.MapsMB.Bussiness
{
    using Common.Entity;
    using Common.Entity.Response;
    using Entity.Constantes.Estructuras;
    using Entity.Controles.Independientes;
    using Entity.Response;
    using IBussiness;
    using IDataAccess;
    using Isban.MapsMB.Common.Entity.Request;
    using Isban.MapsMB.Entity.Request;
    using Isban.Mercados.LogTrace;
    using Isban.Mercados.Service.InOut;
    using Isban.Mercados.WebApiClient;
    using Mercados;
    using Mercados.UnityInject;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.ServiceModel;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using WSERIService;

    public class ExternalWebApiService : IExternalWebApiService
    {
        [WebApiService("GetClienteByText")]
        public ClienteDDC[] GetCuentas(RequestSecurity<GetCuentas> entity)
        {
            var response = DependencyFactory.Resolve<ICallWebApi>().CallWebApiMethod<Response<ClienteDDC[]>, RequestSecurity<GetCuentas>>(entity);

            if (response.Codigo != 0)
            {
                //Manejo de Excepciones
            }

            return response.Datos;
        }

        [WebApiService("GetClienteByText")]
        public ClienteDDC[] GetDatosCliente(RequestSecurity<GetDatosCliente> entity)
        {
            var response = DependencyFactory.Resolve<ICallWebApi>().CallWebApiMethod<Response<ClienteDDC[]>, RequestSecurity<GetDatosCliente>>(entity);

            if (response.Codigo != 0)
            {
                //Manejo de Excepciones
            }

            return response.Datos;
        }

        [WebApiService("GetClienteByDoc")]
        public ClienteDDC[] GetDatosClientePorDocumento(RequestSecurity<GetDatosCliente> entity)
        {
            var response = DependencyFactory.Resolve<ICallWebApi>().CallWebApiMethod<Response<ClienteDDC[]>, RequestSecurity<GetDatosCliente>>(entity);

            if (response.Codigo != 0)
            {
                //Manejo de Excepciones
            }

            return response.Datos;
        }

        [WebApiService("GetTitulares")]
        public GetTitularesResponse[] GetTitulares(RequestSecurity<GetTitulares> entity)
        {
            var response = DependencyFactory.Resolve<ICallWebApi>().CallWebApiMethod<Response<GetTitularesResponse[]>, RequestSecurity<GetTitulares>>(entity);

            if (response.Codigo != 0)
            {
                //Manejo de Excepciones
            }

            return response.Datos;
        }

        [WebApiService("BajaAdhesionMaps")]
        public virtual FormularioResponse BajaAdhesion(RequestSecurity<FormularioResponse> entity)
        {
            var formulario = CallWebApiSingletone.Instance.CallWebApiMethod<Response<FormularioResponse>, RequestSecurity<FormularioResponse>>(entity);
            if (formulario.Datos.Error != 0)
                throw new Exception(formulario.Datos.Error_Desc);

            return formulario.Datos;
        }

        [WebApiService("SaldoRescatado48hs")]
        public SaldoRescatadoResponse ObtenerSaldoRescatado48hs(RequestSecurity<CargaSolicitudOrden> sol)
        {
            var requestSec = sol.MapperClass<Request<SaldoRescatadoRequest>>(TypeMapper.IgnoreCaseSensitive);
            var saldoRescatado = sol.Datos.MapperClass<SaldoRescatadoRequest>(TypeMapper.IgnoreCaseSensitive);
            //Ninguno de los dos deberia enviarse cod de fondo.

            saldoRescatado.ListaCuentas.Add(new SucNroCta
            {
                //BP cuenta titulo, si hay otro segmento hay que cambiar esto.
                NumeroCuenta = sol.Datos.Segmento.Equals(TipoSegmento.Retail) ? sol.Datos.NroCtaOper : sol.Datos.CuentaTitulos,
                Sucursal = sol.Datos.SucuCtaOper
            });

            saldoRescatado.FechaLiquidacionHasta = sol.Datos.FechaProceso;
            requestSec.Datos = saldoRescatado;
            requestSec.Datos.CodigoFondo = null;

            var srvResult = CallWebApiSingletone.Instance.CallWebApiMethod<Response<SaldoRescatadoResponse>, Request<SaldoRescatadoRequest>>(requestSec);

            return srvResult.Datos;
        }

        public virtual object ConfirmacionOrden(RequestSecurity<ConfirmacionOrdenReq> request)
        {
            var firma = request;

            var binding = new BasicHttpBinding()
            {
                Name = "ConfirmacionOrden",
                MaxBufferSize = int.MaxValue,
                MaxReceivedMessageSize = int.MaxValue,
            };

            var er = new Uri(ConfigurationManager.AppSettings["ERI_URL"]);

            var add = new EndpointAddress(er);
            var service = new ERIServiceClient(binding, add);

            WSERIService.ConfirmacionOrdenRequest param = new WSERIService.ConfirmacionOrdenRequest();

            param.Firma_datos_dentro = "N";
            param.Firma_datos_firmados = firma.Dato;
            param.Firma_formato = firma.TipoHash.Equals(Isban.Mercados.Security.Adsec.Entity.TipoHash.B64)
                ? "B64"
                : "PEM";
            param.Firma_hash = firma.Firma;

            param.Datos = new WSERIService.ConfirmacionOrden();
            param.Datos.CodCanal = request.Datos.Canal;

            param.Datos.IdEvaluacion = request.Datos.IdEvaluacion;
            param.Datos.NroOperacion = request.Datos.IdOrden;

            RespuestaWS response = service.ConfirmacionOrden(param);

            //if (response.CodigoResp != 0)
            //{
            //    throw new Exception("Error confirmando la orden contra ERI " + response.MensajeError);
            //}

            return response;
        }

        public virtual DisclaimerERI EvaluacionRiesgo(RequestSecurity<CargaSolicitudOrden> entity)
        {
            var datos = entity.Datos;

            var binding = new BasicHttpBinding()
            {
                Name = "EvaluacionDeRiesgo",
                MaxBufferSize = int.MaxValue,
                MaxReceivedMessageSize = int.MaxValue,
            };

            var er = new Uri(ConfigurationManager.AppSettings["ERI_URL"]);

            var add = new EndpointAddress(er);
            var service = new ERIServiceClient(binding, add);
            var usuarioRacf = DependencyFactory.Resolve<IMotorServicioDataAccess>().ObtenerUsuarioRacf();
            EvaluacionDeRiesgoRequest param = new EvaluacionDeRiesgoRequest();

            param.Firma_datos_dentro = "N";
            param.Firma_datos_firmados = entity.Dato;
            param.Firma_formato = entity.TipoHash.Equals(Isban.Mercados.Security.Adsec.Entity.TipoHash.B64)
                ? "B64"
                : "PEM";
            param.Firma_hash = entity.Firma;

            param.Datos = new EvaluacionDeRiesgo
            {
                Nup = datos.Nup,
                CodCanal = datos.CodCanal,// "OBP",
                Producto = datos.Producto,// "FC",
                Monto = datos.SaldoEnviado,
                Moneda = datos.CodMoneda,
                NroCuenta = datos.NroCtaOper.ToString(),
                NroCuentaTitulo = datos.CuentaTitulos,
                TipoCuenta = short.Parse(datos.TipoCtaOper.ToString()),
                SucursalCuenta = datos.SucuCtaOper.ToString(),
                TipoOperacion = datos.TipoOperacion,//  datos.IdServicio.Equals("SAF") ? "SFON" : datos.IdServicio.Trim(), // por este parametro falla la llamada a ERI
                CanalId = entity.Datos.Canal.PadLeft(4, '0'),
                CompraVenta = datos.CompraVenta.ToString(),
                Especie = datos.CodigoFondo,
                ImporteDebCred = datos.SaldoEnviado.HasValue ? datos.SaldoEnviado.Value : 0m,
                UsuarioId = usuarioRacf.Usuario,
                UsuarioPwd = usuarioRacf.Password

            };

            EvaluaRiesgo response;
            IList<ItemDisclaimer<string>> result = new List<ItemDisclaimer<string>>();
            try
            {
                response = service.EvaluacionDeRiesgo(param);
                XmlSerializer mySerializer = new XmlSerializer(typeof(WSERIService.MappingXML.Mensaje));

                WSERIService.MappingXML.Mensaje oRespuesta = mySerializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(response.Disclaimer))) as WSERIService.MappingXML.Mensaje;
                var disclaimer = new StringBuilder();

                if (oRespuesta != null && oRespuesta.Disclaimers != null && oRespuesta.Disclaimers.Count > 0)
                {
                    disclaimer.Append(response.Cabecera);
                    disclaimer.Append("<br/>");
                    disclaimer.Append(string.Join(" ", oRespuesta.Disclaimers.Select(x => x.Text).ToList()));
                    disclaimer.Append("<br/>");
                    disclaimer.Append(response.Pie);
                }

                var discResult = new DisclaimerERI
                {
                    TextoDisclaimer = disclaimer.ToString(),
                    IdEvaluacion = response.IdEvaluacion,
                    CantidadDisclaimer = oRespuesta.CantidadDeDisclaimers,
                    TipoDisclaimer = response.TipoDisclaimer

                };

                return discResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebApiService("VincularCuentasActivas")]
        public void VincularCuentasActivas(RequestSecurity<VincularCuentasActivasReq> request)
        {
            var response = DependencyFactory.Resolve<ICallWebApi>().CallWebApiMethod<Response<ObtenerPasoResponse>, RequestSecurity<VincularCuentasActivasReq>>(request);
            if (response.Codigo != 0)
            {
                throw new ExceptionWebApiClient(response.MensajeTecnico);
            }
            //return response.Datos;
        }

        #region revisar
        [WebApiService("PDCCUENTASAPTAS")]
        public List<Cliente> GetCuentasAptas(RequestSecurity<CuentasAptasReq> entity)
        {
            var response = DependencyFactory.Resolve<ICallWebApi>().CallWebApiMethod<Response<List<ConsultaPdcResponse>>, RequestSecurity<CuentasAptasReq>>(entity);
            var clientes = new List<Cliente>();
            if (response.Datos != null && response.Datos.Count() > 0)
            {
                var req = new GetClientePDC
                {
                    Canal = entity.Canal,
                    SubCanal = entity.SubCanal,
                    Ip = entity.Datos.Ip,
                    Usuario = entity.Datos.Usuario,
                    Clientes = response.Datos
                };

                clientes = GetClientePDC(req);
            }

            clientes = clientes.Where(r => (!(r.CuentaApta.Equals(CuentaPdc.CuentaNoApta) && r.Estado.Equals(CuentaPdc.EstadoCuentaBaja))
            && !(r.CuentaApta.Equals(CuentaPdc.CuentaNoApta) && r.Estado.Equals(CuentaPdc.EstadoCuentaNoAdherido))
            && !string.IsNullOrEmpty(r.CuentaApta))).ToList();

            foreach (var item in clientes.Where(o => o.CuentaPdc < 0))
            {
                item.CuentaPdc = 0;
            }

            if (response.Codigo != 0)
            {
                //Manejo de Excepciones
            }

            return clientes;

        }

        [WebApiService("GetClientePDC")]
        public List<Cliente> GetClientePDC(GetClientePDC req)
        {
            var clientePDC = DependencyFactory.Resolve<ICallWebApi>().CallWebApiMethod<Response<List<Cliente>>, Request<GetClientePDC>>(new Request<GetClientePDC> { Canal = req.Canal, SubCanal = req.SubCanal, Datos = req }).Datos;

            return clientePDC;
        }

        [WebApiService("SIMULACIONALTASBAJAS")]
        public SimulaPdcResponse PDCSimulacionAltasBajas(RequestSecurity<SimulaPdcReq> entity)
        {
            var response = DependencyFactory.Resolve<ICallWebApi>().CallWebApiMethod<Response<SimulaPdcResponse>, RequestSecurity<SimulaPdcReq>>(entity);

            if (response.Codigo != 0)
            {
                //Manejo de Excepciones
            }

            return response.Datos;
        }

        [WebApiService("ObtenerTenenciaValuadaFondos")]
        public TenenciaValuadaFondosRTFResponse ObtenerTenenciaValuadaFondos(RequestSecurity<TenenciaValuadaFondosRTFRequest> entity)
        {
            var response = DependencyFactory.Resolve<ICallWebApi>().CallWebApiMethod<Response<TenenciaValuadaFondosRTFResponse>, RequestSecurity<TenenciaValuadaFondosRTFRequest>>(entity);

            if (response.Codigo != 0)
            {
                //Manejo de Excepciones
            }

            return response.Datos;
        }


        [WebApiService("ObtenerCuentasActivasPorPeriodo")]
        public List<CuentasActivasRTF> ObtenerCuentasActivasPorPeriodoRTF(RequestSecurity<TenenciaValuadaFondosRTFRequest> entity)
        {
            var response = DependencyFactory.Resolve<ICallWebApi>().CallWebApiMethod<Response<List<CuentasActivasRTF>>, RequestSecurity<TenenciaValuadaFondosRTFRequest>>(entity);

            if (response.Codigo != 0)
            {
                //Manejo de Excepciones
            }

            return response.Datos;
        }

        public TenenciaValuadaFondosRTFResponse CallObtenerTenenciaValuadaFondos(RequestSecurity<TenenciaValuadaFondosRTFRequest> request)
        {
            var time = DateTime.Now;
            TenenciaValuadaFondosRTFResponse response = null;
            string httpResult;
            string statusCode;
            long timeElapsed;
            var req = JsonConvert.SerializeObject(request);
            var url = ConfigurationManager.AppSettings["URL_TENENCIA_RTF"];
            httpResult = CallHttpService(req, url, "POST", out statusCode, out timeElapsed);
            
            if (statusCode == "OK")
            {
                response = JsonConvert.DeserializeObject<Response<TenenciaValuadaFondosRTFResponse>>(httpResult).Datos;            
            } else
            {
                LoggingHelper.Instance.Error($"Error {statusCode} | URL: {url} | Request: {req}", "ExternalWebApiService", "CallHttpService");
            }
            return response;
        }

        private string CallHttpService(string request, string url, string type, out string error, out long timeElapsed, Dictionary<string, string> headers = null)
        {
            string result = String.Empty;
            error = String.Empty;
            timeElapsed = 0;

            if (url != null)
            {
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromMinutes(15);
                try
                {
                    #region Headers
                    if (headers != null)
                    {
                        headers.Keys.ToList().ForEach(key =>
                        {
                            string value;
                            switch (key)
                            {
                                case "Authorization":
                                    headers.TryGetValue("Authorization", out value);
                                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(value);
                                    break;
                                default:
                                    headers.TryGetValue(key, out value);
                                    client.DefaultRequestHeaders.Add(key, value);
                                    break;
                            }
                        });
                    }
                    #endregion

                    #region HTTP Call
                    var stopWatch = new Stopwatch();
                    stopWatch.Start();
                    if (type == "POST")
                    {
                        var json = request;
                        var data = new StringContent(json, Encoding.UTF8, "application/json");
                        var task = Task.Run(() => client.PostAsync(url, data));
                        task.Wait();
                        var response = task;
                        result = response.Result.Content.ReadAsStringAsync().Result;
                        error = response.Result.StatusCode.ToString();

                    }
                    else if (type == "GET")
                    {
                        var task = Task.Run(() => client.GetAsync(url));
                        task.Wait();
                        var response = task;
                        result = response.Result.Content.ReadAsStringAsync().Result;
                        error = response.Result.StatusCode.ToString();
                    }
                    #endregion
                    stopWatch.Stop();
                    timeElapsed = Convert.ToInt32(stopWatch.Elapsed.TotalMilliseconds);
                    LoggingHelper.Instance.Information($"Ejecución de llamada {url} | Tiempo de ejecución: {timeElapsed}ms", "ExternalWebApiService", "CallHttpService");
                    LoggingHelper.Instance.Information($"Resultado: {result}", "ExternalWebApiService", "CallHttpService");
                }
                catch (Exception e)
                {
                    result = $"ERROR | {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")} | {e.Message} | {e.InnerException.Message}";
                    LoggingHelper.Instance.Error(result, "ExternalWebApiService", "CallHttpService");
                }
            }
            else
            {
                result = $"ERROR | {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")} | URL no puede ser Nula";
            }

            return result;
        }
        #endregion
    }
}
