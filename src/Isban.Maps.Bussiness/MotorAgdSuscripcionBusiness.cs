using Isban.MapsMB.Business.Attributes;
using Isban.MapsMB.Bussiness.WSERIService;
using Isban.MapsMB.Common.Entity;
using Isban.MapsMB.Common.Entity.Extensions;
using Isban.MapsMB.Common.Entity.Request;
using Isban.MapsMB.Common.Entity.Response;
using Isban.MapsMB.Entity.Constantes.Estructuras;
using Isban.MapsMB.Entity.Request;
using Isban.MapsMB.Entity.Response;
using Isban.MapsMB.IBusiness;
using Isban.MapsMB.IBussiness;
using Isban.MapsMB.IDataAccess;
using Isban.MapsMB.IDataAccesss;
using Isban.Mercados;
using Isban.Mercados.LogTrace;
using Isban.Mercados.Service.InOut;
using Isban.Mercados.UnityInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Isban.MapsMB.Business
{
    public class MotorAgdSuscripcionBusiness : IMotorAgdSuscripcionBusiness
    {
        #region Métodos Públicos
        [NonTransactionInterceptor]
        public virtual string ProcesosComunes(RequestSecurity<SolicitudOrden> entity)
        {
            var businessMB = this;

            //2- llamar a la consulta de cuentas bloqueadas: OK
            var estadoDeCuentas = businessMB.ConsultaCuentasBloqueadas(entity);

            //3- Ejecutar la Baja de las Bloqueadas: OK
            var solCtasBloqueadas = entity.MapperClass<RequestSecurity<SolicitudOrden>>(TypeMapper.IgnoreCaseSensitive);
            solCtasBloqueadas.Datos.Solicitudes = estadoDeCuentas.Bloqueadas;
            businessMB.RealizarBajaDeCuentasBloqueadasMAPS(solCtasBloqueadas);

            //7 - realizar Evaluación de riesgo ERI OK
            var secEvaRiesto = entity.MapperClass<RequestSecurity<SolicitudOrden>>(TypeMapper.IgnoreCaseSensitive);
            secEvaRiesto.Datos.Cabecera = entity.Datos.Cabecera;
            secEvaRiesto.Datos.Solicitudes = estadoDeCuentas.Activas;
            EvaluaRiesgoResponse evaRiesgo = businessMB.RealizarEncuestaDeRiesgoERI(secEvaRiesto);


            //8- alta de solicitud de orden fondos ok
            var solAltaFondo = entity.MapperClass<RequestSecurity<SolicitudOrden>>(TypeMapper.IgnoreCaseSensitive);
            solAltaFondo.Datos.Solicitudes = estadoDeCuentas.Activas;
            var altaFondoResult = businessMB.RealizarSuscripcionFondos(solAltaFondo);

            //9 - confirmo en ERI la encuesta.
            //RealizarConfirmacionOrdenERI() falta convertir el join al tipo de datos CargaSolicitudOrden.
            var secConfOrdenERI = entity.MapperClass<RequestSecurity<ReqConfirmacionEri>>(TypeMapper.IgnoreCaseSensitive);
            var reqConfirmacion = new ReqConfirmacionEri();
            reqConfirmacion.ConfirmacionERI = evaRiesgo.NoRestrictivo;
            reqConfirmacion.AltaSuscripcion = altaFondoResult.Ok;
            secConfOrdenERI.Datos = reqConfirmacion;

            ConfirmaRiesgoResponse confRiesgoERI = businessMB.RealizarConfirmacionOrdenERI(secConfOrdenERI);

            //10 - Baja de solicitudes que se procesaron correctamente
            if (altaFondoResult.Ok.Count() > 0)
            BajaAdhesionesProcesadasYFallidas(entity,altaFondoResult.Ok);

            //11 - Baja de solicitudes que fallaron al tercer intento
            if (altaFondoResult.NoOk.Count() > 0)
            BajaAdhesionesProcesadasYFallidas(entity, altaFondoResult.NoOk?.Where(a=> a.BajaTercerIntento == true)?.ToList());

            //12 - confirmación por correo MYA
            var realizarConfirmacionMYA = entity.MapperClass<RequestSecurity<SolicitudOrden>>(TypeMapper.IgnoreCaseSensitive);
            businessMB.RealizarConfirmacionMYA(realizarConfirmacionMYA);

            return "Último servicio llamado RealizarConfirmacionOrdenERI";
        }

        [NonTransactionInterceptor]
        public virtual MotorResponse RealizarConfirmacionMYA(RequestSecurity<SolicitudOrden> realizarConfirmacionMYA)

        {
            var modoEjecucion = DependencyFactory.Resolve<IMotorServicioDataAccess>().ObtenerModoEjecucionMyA();

            if (modoEjecucion == ModoEjecucion.NET)
                return RealizarConfirmacionMYANET(realizarConfirmacionMYA);
            else
                return RealizarConfirmacionMYADB(realizarConfirmacionMYA);
        }

        public virtual MotorResponse EjecutarEnvioMailAltaCuentaTitulos(RequestSecurity<WorkflowCTRReq> entity)
        {

            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();

            var ctaOperativasAdheridos = da.ObtenerCuentasRepatriacion(new ObtenerCuentasRepaReq { IdServicio = entity.Datos.IdServicio });

            foreach (var cuentaOperativa in ctaOperativasAdheridos)
            {
                if (entity.Datos.CargaMensajes == "S")
                {
                    CargarMailAltaCuentasMyA(entity, cuentaOperativa.Nup, cuentaOperativa);
                }
            }


            EjecutarProcesoEnvioMails(entity);


            return new MotorResponse();
        }

        private void CargarMailAltaCuentasMyA(RequestSecurity<WorkflowCTRReq> entity, string nup, ObtenerCuentasRepaResp cuentaOperativa)
        {


            ClienteDDC informacionCliente = null;
            string informacionPorNup = string.Empty;

            try
            {
                informacionPorNup = string.Empty;
                informacionCliente = null;
                #region Obtener Cuentas    
                var rs = entity.MapperClass<RequestSecurity<WorkflowCTRReq>>(TypeMapper.IgnoreCaseSensitive);
                rs.Datos.Nup = nup;
                rs.Datos.Segmento = entity.Datos.Segmento;
                rs.Datos.Canal = entity.Datos.Canal;
                rs.Datos.SubCanal = entity.Datos.SubCanal;
                var cuentasDDC = ObtenerCuentas(rs, entity.Datos.Cabecera);
                #endregion

                if (cuentasDDC != null && cuentasDDC.FirstOrDefault() != null)
                    informacionCliente = cuentasDDC.FirstOrDefault();

                if (informacionCliente != null && !string.IsNullOrWhiteSpace(informacionCliente.Apellido))
                    informacionCliente.Apellido = informacionCliente.Apellido.Trim();

                if (informacionCliente != null && !string.IsNullOrWhiteSpace(informacionCliente.Nombre))
                    informacionCliente.Nombre = informacionCliente.Nombre.Trim();

                informacionPorNup = nup + "|" + informacionCliente?.NumeroDocumento + "|" + informacionCliente?.Nombre + "|" + informacionCliente?.Apellido;

                CargarMensajesDelclienteNET(informacionPorNup, entity);
            }
            catch (Exception ex)
            {
                LoggingHelper.Instance.Error(ex, ex.Message);
            }

            EjecutarProcesoEnvioMails(entity);
        }

        private void CargarMensajesDelclienteNET(string informacionPorNup, RequestSecurity<WorkflowCTRReq> entity)
        {
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();

            var request = entity.MapperClass<EnviarMensajesNetReq>(TypeMapper.IgnoreCaseSensitive);
            request.DatosCliente = informacionPorNup;
            da.CargarMensajesDelclienteNET(request);
        }

   

        private static CabeceraConsulta GenerarCabecera(RequestSecurity<WorkflowCTRReq> entity)
        {
            //TODO: se tiene que reemplazar por la forma correcta según dijo lucas.
            return new CabeceraConsulta()
            {
                H_CanalTipo = entity.Canal,
                H_SubCanalTipo = entity.SubCanal,
                H_UsuarioAttr = "  "
            };

        }


        private void EjecutarProcesoEnvioMails(RequestSecurity<WorkflowCTRReq> entity)
        {
            var mails = ObtenerMailsAEnviarACT(new ObtenerMensajes { Fecha = DateTime.Now.Date });

            foreach (var mail in mails)
            {
                Texto texto = null;
                try
                {
                        texto = ObtenerTextoAGD(mail);
   
                }
                catch (Exception ex)
                {
                    LoggingHelper.Instance.Error(ex, ex.Message);
                    ActualizarMensaje(mail.IdMensajes, mail.CodEstadoProceso, false);
                }


                if (texto != null)
                {
                    try
                    {
                        var mensaje = new MensajeMyA();
                        mensaje.HEADER = new Header()
                        {
                            RIONUP = (mail.Segmento == TipoSegmento.BancaPrivada ? String.Empty : mail.IdNup),
                            RIODNICUIT = mail.NumeroDocumento.ToString(),
                            ENTITLEMENT = ObtenerEntitlement(mail.CodEstadoProceso, mail.IdServicio),
                            LABEL = texto.Asunto,
                            TIMESTAMP = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                            TTL = "086400",
                            LEGALTYPE = "Individuo",
                            DESTINATION = (mail.Segmento == TipoSegmento.BancaPrivada ? mail.Destination : String.Empty)
                        };

                        mensaje.BODY = new Body()
                        {
                            APELLIDO = mail.ApellidoCliente,
                            FECHA_ACTUAL = new Fecha()
                            {
                                ANO = DateTime.Now.Year.ToString(),
                                DIA = DateTime.Now.Day.ToString(),
                                HORA = DateTime.Now.ToShortTimeString(),
                                MES = DateTime.Now.Month.ToString()
                            },
                            NOMBRE = mail.NombreCliente,
                            TIPOMENSAJE = texto.TipoMensaje,
                            CANAL_BAJA = mail.Canal,
                            CUENTA_DEBITO = mail.CuentaOperativa.ToString(),
                            CUENTA_TITULOS = mail.CuentaTitulo.ToString(),
                            FECHA = DateTime.Now.ToString(),
                            FONDO = mail.DescripcionFondo,
                            NRO_COMPROBANTE = mail.NumeroComprobante.ToString(),
                            TEXTO = RemplazarTexto(texto.TextoMensaje, mail),
                            TITULO = texto.TituloSolapa,
                            DESCRIPCION = texto.Descripcion
                        };

                        LoggingHelper.Instance.Information(mensaje.BODY.TITULO, "ENVIO MAIL");
                        var colaMq = new ColaMQ.ColaMQ();
                        colaMq.DejarNovedad(mensaje);

                        Thread.Sleep(BusinessHelper.ObtenerTiempoDeEsperaMilSec());
                        ActualizarMensaje(mail.IdMensajes, mail.CodEstadoProceso, true);
                    }
                    catch (Exception ex)
                    {
                        LoggingHelper.Instance.Error(ex, ex.Message);
                        ActualizarMensaje(mail.IdMensajes, mail.CodEstadoProceso, false);
                    }
                }
            }
        }

        [NonTransactionInterceptor]
        public virtual MotorResponse BajaAdhesionesProcesadasYFallidas(RequestSecurity<SolicitudOrden> entity, List<CargaSolicitudOrden> solicitudesProcesadas)
        {
            MotorResponse result = new MotorResponse();
            long cant = 0;
            long bajaError = 0;
            string listaAdhesiones = string.Empty;
            var daMotor = DependencyFactory.Resolve<IExternalWebApiService>();

            if (solicitudesProcesadas != null && solicitudesProcesadas.Count > 0)
            {
                solicitudesProcesadas.ForEach(item =>
                {
                    try
                    {
                        var formReq = entity.MapperClass<RequestSecurity<FormularioResponse>>(TypeMapper.IgnoreCaseSensitive);

                        formReq.Datos = item.MapperClass<FormularioResponse>(TypeMapper.IgnoreCaseSensitive);
                        formReq.Datos.Estado = EstadoFormularioMaps.Carga;
                        formReq.Datos.IdAdhesion = item.CodAltaAdhesion;
                        formReq.Datos.Nup = item.Nup;
                        formReq.Datos.Segmento = item.Segmento;
                        //formReq.Datos.Proceso = "AUTOMATICO";

                        FormularioResponse frm = RealizarBajaPorVigencia(daMotor, formReq);

                        #region Contabilizar Adhesiones 
                        listaAdhesiones = string.IsNullOrEmpty(listaAdhesiones) ? frm.Comprobante : listaAdhesiones += "|" + frm.Comprobante; //Adhesiones para actualizar informacion.
                        cant += 1; //Cantidad de bajas efectivas
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        result.Exitoso = false;
                        bajaError += 1;
                    }
                });
            }
            try
            {
                #region Modificar Solicitud de Orden por Comprobante Baja 
                if (!string.IsNullOrEmpty(listaAdhesiones))
                    BusinessHelper.ActualizaObservacionBajaSolicitudes(listaAdhesiones, EstadoSolicitudDeOrden.BajaDeAdhesion.ToString());
                #endregion
            }
            catch (Exception)
            {

            }

            return new MotorResponse { Exitoso = true, Descripcion = $"Se dieron de baja {cant} Adhesiones el {DateTime.Now}. Bajas con error: {bajaError}" };
        }

        [NonTransactionInterceptor]
        public virtual MotorResponse BajaAdhesionesPorVigenciaVencida(RequestSecurity<WorkflowSAFReq> entity)
        {
            MotorResponse result = new MotorResponse();
            long cant = 0;
            long bajaError = 0;
            string listaAdhesiones = string.Empty;
            List<AdheVigenciaVencida> adhesiones = new List<AdheVigenciaVencida>();
            var daMotor = DependencyFactory.Resolve<IExternalWebApiService>();

            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();
            adhesiones = da.ConsultaAdhesionesPorVigenciaVencida(entity.Datos);

            if (adhesiones != null && adhesiones.Count > 0)
            {
                adhesiones.ForEach(item =>
                {
                    try
                    {
                        var formReq = entity.MapperClass<RequestSecurity<FormularioResponse>>(TypeMapper.IgnoreCaseSensitive);

                        formReq.Datos = item.MapperClass<FormularioResponse>(TypeMapper.IgnoreCaseSensitive);
                        formReq.Datos.Estado = EstadoFormularioMaps.Carga;
                        formReq.Datos.IdAdhesion = item.CodAltaAdhesion;
                        formReq.Datos.Nup = item.Nup;
                        formReq.Datos.Segmento = item.Segmento;
                        //formReq.Datos.Proceso = "AUTOMATICO";

                        FormularioResponse frm = RealizarBajaPorVigencia(daMotor, formReq);

                        #region Contabilizar Adhesiones 
                        listaAdhesiones = string.IsNullOrEmpty(listaAdhesiones) ? frm.Comprobante : listaAdhesiones += "|" + frm.Comprobante; //Adhesiones para actualizar informacion.
                        cant += 1; //Cantidad de bajas efectivas
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        result.Exitoso = false;
                        bajaError += 1;
                    }
                });
            }
            try
            {
                #region Modificar Solicitud de Orden por Comprobante Baja 
                if (!string.IsNullOrEmpty(listaAdhesiones))
                    BusinessHelper.ActualizaObservacionSolicitudesVencida(listaAdhesiones, EstadoSolicitudDeOrden.BajaPorVigencia.ToString());
                #endregion
            }
            catch (Exception)
            {

            }

            return new MotorResponse { Exitoso = true, Descripcion = $"Se dieron de baja {cant} Adhesiones el {DateTime.Now}. Bajas con error: {bajaError}" };
        }

        public virtual FormularioResponse RealizarBajaPorVigencia(IExternalWebApiService daMotor, RequestSecurity<FormularioResponse> formReq)
        {
            formReq.Datos = daMotor.BajaAdhesion(formReq);
            formReq.Datos.Items.Clear();
            formReq.Datos.Estado = EstadoFormularioMaps.Carga;
            var frm = daMotor.BajaAdhesion(formReq);
            return frm;
        }

        /// <summary>
        /// Método que realiza el alta en fonodos.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [NonTransactionInterceptor]
        public virtual ResultadoFondos RealizarSuscripcionFondos(RequestSecurity<SolicitudOrden> entity)
        {
            var datos = entity.Datos;
            var result = new ResultadoFondos();
            ICanalesIatxDA daIATX = DependencyFactory.Resolve<ICanalesIatxDA>();
            var usuarioRacf = DependencyFactory.Resolve<IMotorServicioDataAccess>().ObtenerUsuarioRacf();
            var usuarioAsesor = DependencyFactory.Resolve<IMotorServicioDataAccess>().ObtenerUsuarioAsesor();

            datos.Solicitudes.ForEach(solicitud =>
            {
                try
                {
                    #region Banca Privada
                    if (solicitud.Segmento == TipoSegmento.BancaPrivada)
                    {
                        var requestSimulacion = new SimularAltaFondos
                        {
                            Canal = solicitud.Canal,
                            Tipo = TipoSolicitud.Suscripcion,
                            Cuenta = solicitud.NroCtaOperativaFormateada,
                            Especie = solicitud.CodEspecie,
                            Moneda = solicitud.CodMoneda == TipoMoneda.PesosAR ? TipoMoneda.PesosARG : TipoMoneda.DolaresUSA,
                            Cuotapartes = null,
                            Capital = solicitud.SaldoEnviado,
                            EspecieDestino = null,
                            UsuarioRacf = usuarioRacf.Usuario,
                            UsuarioAsesor = usuarioAsesor,
                            PasswordRacf = usuarioRacf.Password

                        };
                        LoggingHelper.Instance.Information($"Simulacion request: {JsonSerialization.SerializarToJson(requestSimulacion)}", "MotorAgdSuscripcionBusiness", "SuscripcionFondo");
                        string dentroDePerfil = DependencyFactory.Resolve<ISmcDA>().SimulacionAltaFondos(requestSimulacion);

                        var requestConfirmacion = new EjecutarAltaFondos
                        {
                            Canal = solicitud.Canal,
                            Tipo = TipoSolicitud.Suscripcion,
                            Cuenta = solicitud.NroCtaOperativaFormateada,
                            Especie = solicitud.CodEspecie,
                            Moneda = solicitud.CodMoneda == TipoMoneda.PesosAR ? TipoMoneda.PesosARG : TipoMoneda.DolaresUSA,
                            Cuotapartes = null,
                            Capital = solicitud.SaldoEnviado,
                            EspecieDestino = null,
                            UsuarioRacf = usuarioRacf.Usuario,
                            UsuarioAsesor = usuarioAsesor,
                            PasswordRacf = usuarioRacf.Password,
                            DentroDelPerfil = dentroDePerfil

                        };
                        LoggingHelper.Instance.Information($"Confirmacion request: {JsonSerialization.SerializarToJson(requestConfirmacion)}", "MotorAgdSuscripcionBusiness", "SuscripcionFondo");
                        var ejecutarAltaFondos = DependencyFactory.Resolve<ISmcDA>().EjecutarAltaFondos(requestConfirmacion);
                        LoggingHelper.Instance.Information($"Confirmacion response: {JsonSerialization.SerializarToJson(ejecutarAltaFondos)}", "MotorAgdSuscripcionBusiness", "SuscripcionFondo");

                        solicitud.NumOrdenOrigen = ejecutarAltaFondos.NumOrden.ParseGeneric<long>();
                        solicitud.TextoDisclaimer = ejecutarAltaFondos.Disclaimer;
                        BusinessHelper.GuardarProcesado(solicitud, dentroDePerfil);
                    }
                    #endregion
                    #region RTL
                    else
                    {
                        SuscripcionFCIIATXResponse suscripcionFCIIATX = daIATX.SuscripcionFCI(GenerarCabecera(solicitud), solicitud.Nup,
                                     Convert.ToInt16(solicitud.CodigoFondo),
                                     solicitud.CuentaTitulos.ToString(),
                                     solicitud.SaldoEnviado.Value,
                                     Convert.ToInt16(solicitud.TipoCtaOper),
                                     Convert.ToInt16(solicitud.SucuCtaOper),
                                     solicitud.NroCtaOper.ToString(),
                                     solicitud.CodMoneda == TipoMoneda.PesosAR ? 0 : 2);
                        BusinessHelper.GuardarProcesado(solicitud, suscripcionFCIIATX.Numero_Certificado.ToString());
                        solicitud.NumOrdenOrigen = suscripcionFCIIATX.Numero_Certificado.ParseGeneric<long>();
                    }
                    #endregion

                    result.Ok.Add(solicitud);
                }
                catch (Exception ex)
                {
                 
                    LoggingHelper.Instance.Error($"Message: {ex.Message}", "MotorAgdSuscripcionBusiness", "RealizarSuscripcionFondos");
                    LoggingHelper.Instance.Error($"InnerException: {(ex.InnerException == null ? ex.Message : ex.InnerException.Message)}", "MotorAgdSuscripcionBusiness", "RealizarSuscripcionFondos");
                    if (ex is IatxCodeException)
                    {
                        string DescError = $"IatxCodeException, Mensaje: { ex.InnerException.Message.ToString()}";
                        BusinessHelper.GuardarFallaTecnica(ex, solicitud, DescError);
                    }
                    else
                    {
                        BusinessHelper.GuardarFallaTecnica(ex, solicitud, "RealizarSuscripcionFondos");
                    }
                    result.NoOk.Add(solicitud);
                }
            });

            return result;
        }

        public virtual EstadoDecuentaResp ConsultaCuentasBloqueadas(RequestSecurity<SolicitudOrden> entity)
        {
            EstadoDecuentaResp result = new EstadoDecuentaResp();

            var datos = entity.Datos;

            //1- por cada carga solicitud de orden verificar  si esta bloqueada contra el estado en DDC
            //2- si esta bloqueada en ddc modifOrd la lista de bloqueadas
            //3- si esta activa en ddc modifOrd la lista de activas
            //4- si no esta en la lista de ddc modifOrd la lista de inexistentes. OPCIONAL

            datos.Solicitudes.ForEach(solOrd =>
            {
                try
                {
                    #region Obtener Cuentas    
                    var rs = entity.MapperClass<RequestSecurity<CargaSolicitudOrden>>(TypeMapper.IgnoreCaseSensitive);
                    rs.Datos = solOrd;
                    //validar si no conviene hacer como se hizo en MAPS con DatoFirmado para evitar el requestSecurity
                    var cuentasDDC = ObtenerCuentas(rs, entity.Datos.Cabecera);
                    #endregion
                    if (cuentasDDC != null && cuentasDDC.Length > 0)
                    {
                        var ctasDdc = cuentasDDC.SelectMany(y => y.Cuentas)
                                        .Where(f => f.SegmentoCuenta.Equals(solOrd.Segmento)
                                         ).ToArray();

                        var ctasTit = ctasDdc.Where(x => x.CodProducto.Trim() == TipoCuenta.CuentaTitulo
                                                    && long.Parse(x.NroCta) == solOrd.CuentaTitulos
                                                    ).ToArray();
                        var ctasOpers = ctasDdc.Where(x => x.CodProducto.Trim() != TipoCuenta.CuentaTitulo
                                        && long.Parse(x.NroCta) == solOrd.NroCtaOper
                                        && long.Parse(x.SucursalCta) == solOrd.SucuCtaOper
                                        && x.CodigoMoneda.Trim().ToLower().Equals(solOrd.CodMoneda.ToLower().Trim())).ToArray();

                        if (ctasTit.Length > 0 && ctasOpers.Length > 0)
                        {
                            if (ctasTit.Any(x => x.CuentaBloqueada.Trim().ToLower() == Constantes.CtaNoBloqueada)
                            && ctasOpers.Any(x => x.CuentaBloqueada.Trim().ToLower() == Constantes.CtaNoBloqueada))
                            {
                                result.Activas.Add(solOrd);
                            }
                            else if (ctasTit.Any(x => x.CuentaBloqueada.Trim().ToLower() == Constantes.CtaBloqueada)
                            || ctasOpers.Any(x => x.CuentaBloqueada.Trim().ToLower() == Constantes.CtaBloqueada))
                            {
                                result.Bloqueadas.Add(solOrd);
                                solOrd.CodEstadoProceso = EstadoSolicitudDeOrden.Bloqueado;
                            }
                            else
                            {
                                result.NoDDC.Add(solOrd);//Por estado distinto.
                                solOrd.Observacion = $"Posee otro estado la cuenta en DDC";
                                solOrd.CodEstadoProceso = EstadoSolicitudDeOrden.Otro;
                                result.NoDDC.Add(solOrd);//Falta alguna de las cuentas
                                BusinessHelper.GuardarOtros(solOrd, solOrd.Observacion);
                            }
                        }
                        else
                        {
                            solOrd.Observacion = $"Cantidad de Ctas Titulo DDC: {ctasTit.Length} y Ctas Operativas DDC: {ctasOpers.Length}. Cta Tit MAPS: {solOrd.CuentaTitulos} Cta Oper MAPS: {solOrd.NroCtaOper}";
                            solOrd.CodEstadoProceso = EstadoSolicitudDeOrden.Otro;
                            result.NoDDC.Add(solOrd);//Falta alguna de las cuentas
                            BusinessHelper.GuardarOtros(solOrd, solOrd.Observacion);
                        }
                    }
                    else
                    {
                        solOrd.Observacion = $"DDC no devolvio cuentas activas.";
                        solOrd.CodEstadoProceso = EstadoSolicitudDeOrden.Otro;
                        result.NoDDC.Add(solOrd);//Falta alguna de las cuentas
                        BusinessHelper.GuardarOtros(solOrd, solOrd.Observacion);
                    }
                }
                catch (Exception ex)
                {
                    BusinessHelper.GuardarFallaTecnica(ex, solOrd, "ConsultaCuentasBloqueadas");
                }
            });

            return result;

        }

        //[NonTransactionInterceptorAttribute]
        //public virtual List<CargaSolicitudOrden> ConsultaSaldoCuenta(RequestSecurity<SolicitudOrden> entity)
        //{
        //    ICanalesIatxDA daIATX = DependencyFactory.Resolve<ICanalesIatxDA>();
        //    var item = entity.Datos.Solicitudes.ToList();
        //    DatosCuentaIATXResponse datosCuentaIATXResponse = null;
        //    var usuarioRacf = DependencyFactory.Resolve<IMotorServicioDataAccess>().ObtenerUsuarioRacf();

        //    item.ForEach(soli =>
        //    {
        //        int tipoCta = 0;
        //        int sucCtaOper = 0;


        //        int.TryParse(soli.TipoCtaOper.ToString(), out tipoCta);
        //        int.TryParse(soli.SucuCtaOper.ToString(), out sucCtaOper);

        //        try
        //        {
        //            #region BancaPrivada
        //            if (soli.Segmento.ToUpper() == TipoSegmento.BancaPrivada)
        //            {
        //                decimal sumSaldo = 0;
        //                //soli.Canal = "79";   
        //                var atisResp = DependencyFactory.Resolve<IOpicsDA>().ObtenerAtis(new ConsultaLoadAtisRequest
        //                {
        //                    Nup = soli.Nup.ParseGeneric<long?>(),
        //                    CuentaBp = 0
        //                });

        //                var cuentaBp = ValidarCuentas(atisResp, soli.NroCtaOper, soli.CuentaTitulos);

        //                var resLoadSaldos = DependencyFactory.Resolve<IOpicsDA>().EjecutarLoadSaldos(new LoadSaldosRequest
        //                {
        //                    Canal = soli.Canal,// entity.Canal,
        //                    Cuenta = cuentaBp.Value.ParseGeneric<string>(),
        //                    FechaDesde = DateTime.Now.Date,
        //                    FechaHasta = DateTime.Now.Date,
        //                    Usuario = usuarioRacf.Usuario,
        //                    Password = usuarioRacf.Password
        //                });

        //                var resSaldoConcertado = DependencyFactory.Resolve<ISmcDA>().EjecutarSaldoConcertadoNoLiquidado(new SaldoConcertadoNoLiquidadoRequest
        //                {
        //                    Fecha = DateTime.Now.Date,
        //                    Ip = soli.Ip,
        //                    Moneda = TraducirUsdAUsb(soli.CodMoneda),
        //                    NroCtaOper = soli.NroCtaOper.ParseGeneric<string>(),
        //                    SucCtaOper = soli.SucuCtaOper.ParseGeneric<string>(),
        //                    TipoCtaOper = soli.TipoCtaOper.ParseGeneric<decimal>(),
        //                    Usuario = soli.Usuario
        //                });


        //                switch (soli.CodMoneda.ToLower())
        //                {
        //                    case "ars":
        //                        foreach (var saldo in resLoadSaldos.ListaSaldos)
        //                        {

        //                            sumSaldo += saldo.CAhorroPesos;
        //                        }

        //                        break;

        //                    case "usd":
        //                        foreach (var saldo in resLoadSaldos.ListaSaldos)
        //                        {
        //                            sumSaldo += saldo.CAhorroDolares;
        //                        }

        //                        break;
        //                }

        //                var totalSaldo = sumSaldo - (resSaldoConcertado.Saldo.HasValue ? resSaldoConcertado.Saldo.Value : 0m);
        //                soli.SaldoCuentaAntes = totalSaldo;
        //            }
        //            #endregion
        //            #region RTL
        //            else
        //            {
        //                var cabecera = GenerarCabecera(soli);
        //                cabecera.H_UsuarioID = usuarioRacf.Usuario;
        //                cabecera.H_UsuarioPwd = usuarioRacf.Password;

        //                datosCuentaIATXResponse = daIATX.ConsultaDatosCuenta(cabecera, soli);
        //                var entero = datosCuentaIATXResponse.Dispuesto_USD_cuenta.Substring(0, 12);
        //                var dec = datosCuentaIATXResponse.Dispuesto_USD_cuenta.Substring(12, 2);
        //                var signo = datosCuentaIATXResponse.Dispuesto_USD_cuenta.Substring(14, 1);
        //                decimal numDec = 0;

        //                if (decimal.TryParse($"{signo}{entero}.{dec}", out numDec))
        //                {
        //                    soli.SaldoCuentaAntes = numDec;
        //                }
        //            }
        //            #endregion

        //            BusinessHelper.GuardarSaldoCuentaAntes(soli);
        //        }
        //        catch (Exception ex)
        //        {
        //            BusinessHelper.GuardarFallaTecnica(ex, soli, usuarioRacf);

        //        }

        //    });

        //    return item;
        //}

        public virtual EvaluaRiesgoResponse RealizarEncuestaDeRiesgoERI(RequestSecurity<SolicitudOrden> entity)
        {
            var result = new EvaluaRiesgoResponse();
            var datos = entity.Datos.Solicitudes.Where(x => x.Segmento.Equals(TipoSegmento.Retail)).ToList();

            var ext = DependencyFactory.Resolve<IExternalWebApiService>();
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();

            if (datos != null && datos.Count > 0)
            {
                datos.ForEach(item =>
                {
                    try
                    {
                        var formReq2 = entity.MapperClass<RequestSecurity<CargaSolicitudOrden>>(TypeMapper.IgnoreCaseSensitive);
                        formReq2.Datos = item.MapperClass<CargaSolicitudOrden>(TypeMapper.IgnoreCaseSensitive);
                        var evaluacionRiesgoResult = ext.EvaluacionRiesgo(formReq2);

                        item.IdEvaluacion = evaluacionRiesgoResult.IdEvaluacion;
                        item.TipoDisclaimer = evaluacionRiesgoResult.TipoDisclaimer;
                        item.TextoDisclaimer = evaluacionRiesgoResult.TextoDisclaimer;

                        if (evaluacionRiesgoResult.TipoDisclaimer.Equals(2))
                        {
                            result.Restrictivo.Add(item);
                        }
                        else
                        {
                            result.NoRestrictivo.Add(item);

                        }
                        BusinessHelper.GuardarEvaluacionERI(evaluacionRiesgoResult, item);

                    }
                    catch (Exception ex)
                    {
                        BusinessHelper.GuardarFallaTecnica(ex, item, "RealizarEncuestaDeRiesgoERI");
                    }
                });

            }

            return result;

        }

        public virtual ConfirmacionOrdenResponse ConfirmacionOrden(RequestSecurity<ConfirmacionOrdenReq> request)
        {
            var ext = DependencyFactory.Resolve<IExternalWebApiService>();
            RespuestaWS respuesta = ext.ConfirmacionOrden(request) as RespuestaWS; //TODO: convertir modifOrd clase y sacar el cast
            return respuesta.MapperClass<ConfirmacionOrdenResponse>();
        }

        [NonTransactionInterceptor]
        public virtual MotorResponse RealizarBajaDeCuentasBloqueadasMAPS(RequestSecurity<SolicitudOrden> entity)
        {
            MotorResponse result = new MotorResponse();
            long cant = 0;
            long bajaError = 0;

            if (entity.Datos != null && entity.Datos.Solicitudes != null && entity.Datos.Solicitudes.Count > 0)
            {
                entity.Datos.Solicitudes.ForEach(item =>
                {
                    try
                    {
                        //TODO: ver de devolver 2 listas: baja exitosa y baja erronea.
                        var daMotor = DependencyFactory.Resolve<IExternalWebApiService>();
                        var formReq = entity.MapperClass<RequestSecurity<FormularioResponse>>(TypeMapper.IgnoreCaseSensitive);

                        formReq.Datos = item.MapperClass<FormularioResponse>(TypeMapper.IgnoreCaseSensitive);
                        formReq.Datos.Estado = EstadoFormularioMaps.Carga;
                        formReq.Datos.IdAdhesion = item.CodAltaAdhesion;
                        BajaDeCuentas(item, daMotor, formReq);
                        cant += 1;
                    }
                    catch (Exception ex)
                    {
                        result.Exitoso = false;
                        bajaError += 1;
                        BusinessHelper.GuardarFallaTecnica(ex, item, "RealizarBajaDeCuentasBloqueadasMAPS");
                    }
                });
            }

            return new MotorResponse { Exitoso = true, Descripcion = $"Se dieron de baja {cant} Adhesiones el {DateTime.Now}. Bajas con error: {bajaError}" };
        }

        public virtual void BajaDeCuentas(CargaSolicitudOrden item, IExternalWebApiService daMotor, RequestSecurity<FormularioResponse> formReq)
        {
            formReq.Datos = daMotor.BajaAdhesion(formReq);
            formReq.Datos.Items.Clear();
            formReq.Datos.Estado = EstadoFormularioMaps.Carga;
            var frm = daMotor.BajaAdhesion(formReq);
            BusinessHelper.GuardarBajaPorCuentaBloqueada(item, long.Parse(frm.Comprobante));
        }

        /// <summary>
        /// Valida que el saldo de las adhesiones enviadas sea suficiente para continuar la suscripción.
        /// </summary>
        /// <param name="datos"></param>
        /// <returns>Lista de adhesiones con saldo suficiente y lista con adhesiones con saldo insuficiente.</returns>
        //public virtual CondicionAdhesion FiltrarAdhesionesPorCondicion(RequestSecurity<SolicitudOrden> entity)
        //{
        //    var result = new CondicionAdhesion();
        //    var datos = entity.Datos.Solicitudes.ToList();

        //    datos.ForEach(soli =>
        //    {
        //        try
        //        {
        //            decimal saldoActual = soli.SaldoCuentaAntes.HasValue ? soli.SaldoCuentaAntes.Value : 0;
        //            decimal saldoFinal = soli.SaldoRescatePorFondo.HasValue ? saldoActual - soli.SaldoRescatePorFondo.Value : saldoActual;
        //            saldoFinal = saldoFinal - soli.SaldoMinDejarCta;

        //            //Según PELI:
        //            //1- si el saldo final es el maximo o superior, suscribo por salgo máximo.
        //            //2- si saldo final es menor al maximo, suscribo por el mínimo.

        //            if (saldoFinal >= soli.SaldoMaxOperacion && saldoFinal >= soli.SaldoMinPorFondo)
        //            {
        //                soli.SaldoEnviado = soli.SaldoMaxOperacion;
        //                result.SaldoSuficiente.Add(soli);//Puede seguir operando
        //            }
        //            else if (((saldoFinal >= soli.SaldoMinOperacion && saldoFinal <= soli.SaldoMaxOperacion) && saldoFinal < soli.SaldoMaxOperacion) && saldoFinal >= soli.SaldoMinPorFondo)
        //            {
        //                soli.SaldoEnviado = saldoFinal;
        //                result.SaldoSuficiente.Add(soli);//Puede seguir operando
        //            }
        //            else
        //            {
        //                soli.SaldoEnviado = saldoFinal;
        //                result.SaldoInsuficiente.Add(soli);//No posee saldo suficiente para operar

        //                BusinessHelper.GuardarSinSaldo(soli, saldoFinal);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            BusinessHelper.GuardarFallaTecnica(ex, soli, "FiltrarAdhesionesPorCondicion");
        //        }
        //    });

        //    return result;
        //}

        [NonTransactionInterceptor]
        private MotorResponse RealizarConfirmacionMYADB(RequestSecurity<SolicitudOrden> entity)
        {
            MotorResponse result = new MotorResponse();
            result.Exitoso = true;
            result.Descripcion = $"Se corrio los envios de mails al {DateTime.Now}.";

            ClienteDDC informacionCliente = new ClienteDDC();
            string informacionPorNup = string.Empty;
            List<NupSolicitudes> adhesiones = new List<NupSolicitudes>();

            var dato = entity.MapperClass<WorkflowSAFReq>(TypeMapper.IgnoreCaseSensitive);
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();
            adhesiones = da.InformacionClientePorNup(dato);

            if (adhesiones != null && adhesiones.Count > 0)
            {
                adhesiones.ForEach(item =>
                {
                    try
                    {
                        informacionPorNup = string.Empty;

                        #region Obtener Cuentas    
                        var rs = entity.MapperClass<RequestSecurity<CargaSolicitudOrden>>(TypeMapper.IgnoreCaseSensitive);
                        rs.Datos = item.MapperClass<CargaSolicitudOrden>(TypeMapper.IgnoreCaseSensitive);
                        rs.Datos.Nup = item.Nup;
                        rs.Datos.Segmento = item.Segmento;
                        rs.Datos.Canal = item.Canal;
                        rs.Datos.SubCanal = item.SubCanal;
                        var cuentasDDC = ObtenerCuentas(rs, entity.Datos.Cabecera);
                        #endregion

                        if (cuentasDDC != null && cuentasDDC.FirstOrDefault() != null)
                            informacionCliente = cuentasDDC.FirstOrDefault();

                        if (!string.IsNullOrWhiteSpace(informacionCliente.Apellido))
                            informacionCliente.Apellido = informacionCliente.Apellido.Trim();

                        if (!string.IsNullOrWhiteSpace(informacionCliente.Nombre))
                            informacionCliente.Nombre = informacionCliente.Nombre.Trim();

                        informacionPorNup = item.Nup + "|" + informacionCliente.NumeroDocumento + "|" + informacionCliente.Nombre + "|" + informacionCliente.Apellido;
                    }
                    catch (Exception ex)
                    {
                        result.Exitoso = false;
                        result.Descripcion += ex.Message;
                        result.ResultadoServicio = "Existen algunos mails con inconvenientes.";
                    }
                    try
                    {
                        #region DatosCliente
                        if (!string.IsNullOrWhiteSpace(informacionPorNup))
                        {
                            EnviarMail(informacionPorNup, entity);

                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        result.Exitoso = false;
                        result.Descripcion += ex.Message;
                        result.ResultadoServicio = "Inconvenientes con MQ. Revisar mails.";
                    }
                });
            }
            return result;
        }

        private MotorResponse RealizarConfirmacionMYANET(RequestSecurity<SolicitudOrden> entity)
        {
            MotorResponse result = new MotorResponse();
            ClienteDDC informacionCliente = null;
            string informacionPorNup = string.Empty;
            List<NupSolicitudes> adhesiones = new List<NupSolicitudes>();

            var dato = entity.MapperClass<WorkflowSAFReq>(TypeMapper.IgnoreCaseSensitive);
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();
            adhesiones = da.InformacionClientePorNup(dato);

            if (adhesiones != null && adhesiones.Count > 0)
            {
                adhesiones.ForEach(item =>
                {
                    try
                    {
                        informacionPorNup = string.Empty;
                        informacionCliente = null;
                        #region Obtener Cuentas    
                        var rs = entity.MapperClass<RequestSecurity<CargaSolicitudOrden>>(TypeMapper.IgnoreCaseSensitive);
                        rs.Datos = item.MapperClass<CargaSolicitudOrden>(TypeMapper.IgnoreCaseSensitive);
                        rs.Datos.Nup = item.Nup;
                        rs.Datos.Segmento = item.Segmento;
                        rs.Datos.Canal = item.Canal;
                        rs.Datos.SubCanal = item.SubCanal;
                        var cuentasDDC = ObtenerCuentas(rs, entity.Datos.Cabecera);
                        #endregion

                        if (cuentasDDC != null && cuentasDDC.FirstOrDefault() != null)
                            informacionCliente = cuentasDDC.FirstOrDefault();

                        if (informacionCliente != null && !string.IsNullOrWhiteSpace(informacionCliente.Apellido))
                            informacionCliente.Apellido = informacionCliente.Apellido.Trim();

                        if (informacionCliente != null && !string.IsNullOrWhiteSpace(informacionCliente.Nombre))
                            informacionCliente.Nombre = informacionCliente.Nombre.Trim();

                        informacionPorNup = item.Nup + "|" + informacionCliente?.NumeroDocumento + "|" + informacionCliente?.Nombre + "|" + informacionCliente?.Apellido;

                        CargarMensajesDelclienteNET(informacionPorNup, entity);
                    }
                    catch (Exception ex)
                    {
                        LoggingHelper.Instance.Error(ex, ex.Message);
                    }
                });

                var mails = ObtenerMailsAEnviar(new ObtenerMensajes { Fecha = DateTime.Now.Date });

                foreach (var mail in mails)
                {
                    Texto texto = null;
                    try
                    {
                        if (mail.IdServicio == Servicio.Agendamiento || mail.IdServicio == Servicio.AgendamientoFH)
                        {
                            texto = ObtenerTextoAGD(mail);
                        }
                        else
                        {
                            texto = ObtenerTexto(mail);
                        }
                    }
                    catch (Exception ex)
                    {
                        LoggingHelper.Instance.Error(ex, ex.Message);
                        ActualizarMensaje(mail.IdMensajes, mail.CodEstadoProceso, false);
                    }


                    if (texto != null)
                    {
                        try
                        {
                            var mensaje = new MensajeMyA();
                            mensaje.HEADER = new Header()
                            {
                                RIONUP = (mail.Segmento == TipoSegmento.BancaPrivada ? String.Empty : mail.IdNup),
                                RIODNICUIT = mail.NumeroDocumento.ToString(),
                                ENTITLEMENT = ObtenerEntitlement(mail.CodEstadoProceso, mail.IdServicio),
                                LABEL = texto.Asunto,
                                TIMESTAMP = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                                TTL = "086400",
                                LEGALTYPE = "Individuo",
                                DESTINATION = (mail.Segmento == TipoSegmento.BancaPrivada ? mail.Destination : String.Empty)
                            };

                            mensaje.BODY = new Body()
                            {
                                APELLIDO = mail.ApellidoCliente,
                                FECHA_ACTUAL = new Fecha()
                                {
                                    ANO = DateTime.Now.Year.ToString(),
                                    DIA = DateTime.Now.Day.ToString(),
                                    HORA = DateTime.Now.ToShortTimeString(),
                                    MES = DateTime.Now.Month.ToString()
                                },
                                NOMBRE = mail.NombreCliente,
                                TIPOMENSAJE = texto.TipoMensaje,
                                CANAL_BAJA = mail.Canal,
                                CUENTA_DEBITO = mail.CuentaOperativa.ToString(),
                                CUENTA_TITULOS = mail.CuentaTitulo.ToString(),
                                FECHA = DateTime.Now.ToString(),
                                FONDO = mail.DescripcionFondo,
                                NRO_COMPROBANTE = mail.NumeroComprobante.ToString(),
                                TEXTO = RemplazarTexto(texto.TextoMensaje, mail),
                                TITULO = texto.TituloSolapa,
                                DESCRIPCION = texto.Descripcion
                            };

                            var colaMq = new ColaMQ.ColaMQ();
                            colaMq.DejarNovedad(mensaje);

                            Thread.Sleep(BusinessHelper.ObtenerTiempoDeEsperaMilSec());
                            ActualizarMensaje(mail.IdMensajes, mail.CodEstadoProceso, true);
                        }
                        catch (Exception ex)
                        {
                            LoggingHelper.Instance.Error(ex, ex.Message);
                            ActualizarMensaje(mail.IdMensajes, mail.CodEstadoProceso, false);
                        }
                    }
                }


            }

            result.Exitoso = true;
            result.Descripcion = $"Se corrio los envios de mails al {DateTime.Now}.";
            return result;
        }

        private Texto ObtenerTextoAGD(Mensaje mail)
        {
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();
            string entitlement = "Rio/Fondos_Servicio";

            return da.ObtenerTexto(new ObtenerTexto
            {
                Entitlement = entitlement,
                CodEstadoProceso = mail.CodEstadoProceso,
                CodAlta = mail.NumeroComprobante,
                Nup = mail.IdNup,
                IdServicio = mail.IdServicio
            });
        }

        private string RemplazarTexto(string texto, Mensaje mails)
        {
            StringBuilder s = new StringBuilder(texto);

            s.Replace("@fondo", mails.DescripcionFondo);
            s.Replace("@cuentaTitulo", mails.CuentaTitulo.ToString());
            s.Replace("@cuentaOperativa", mails.CuentaOperativa.ToString());
            s.Replace("@comprobante", mails.NumeroComprobante.ToString());
            s.Replace("@fecha", DateTime.Now.ToString());
            s.Replace("@solicitante", $"{mails.ApellidoCliente}, {mails.NombreCliente}");
            s.Replace("@canalBaja", mails.Canal);

            return s.ToString();
        }

        private void CargarMensajesDelclienteNET(string informacionPorNup, RequestSecurity<SolicitudOrden> entity)
        {
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();

            var request = entity.MapperClass<EnviarMensajesNetReq>(TypeMapper.IgnoreCaseSensitive);
            request.DatosCliente = informacionPorNup;
            da.CargarMensajesDelclienteNET(request);
        }

        private void ActualizarMensaje(long idMensaje, string codEstadoProceso, bool envioOk)
        {
            var estado = string.Empty;
            if (envioOk)
            {
                switch (codEstadoProceso)
                {                   
                    case "BA":
                    case "BL":
                    case "PR":
                    case "FT":
                    case "BV":
                    case "A":
                    case "C":
                        estado = "EN";
                        break;
                    case "ER1":
                    case "ER2":
                    case "ER3":
                        estado = "DE";
                        break;
                }
            }
            else
            {
                switch (codEstadoProceso)
                {
                    case "BA":
                    case "BL":
                    case "PR":
                    case "FT":
                    case "BV":
                    case "A":
                    case "C":
                        estado = "PE";
                        break;
                    case "ER1":
                    case "ER2":
                    case "ER3":
                        estado = "PD";
                        break;
                }
            }

            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();
            da.ActualizarMensaje(new ActualizarMensaje { IdMensaje = idMensaje, Estado = estado });
        }

        private Texto ObtenerTexto(Mensaje mail)
        {
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();
            string entitlement = ObtenerEntitlement(mail.CodEstadoProceso);

            return da.ObtenerTexto(new ObtenerTexto
            {
                Entitlement = entitlement,
                CodEstadoProceso = mail.CodEstadoProceso,
                CodAlta = mail.NumeroComprobante,
                Nup = mail.IdNup,
                IdServicio = string.IsNullOrWhiteSpace(mail.IdServicio) ? "SAF" : mail.IdServicio
                //Operacion = mail.Operacion
            });
        }

        private string ObtenerEntitlement(string codEstadoProceso, string idServicio = null)
        {
            var entitlement = string.Empty;
            switch (codEstadoProceso)
            {
                case "BA":
                case "BL":
                case "ER1":
                case "ER2":
                case "PR":
                case "BV":
                case "A":
                    entitlement = "Rio/Fondos_Servicio";
                    break;
                case "ER3":
                case "FT":
                    if (string.Compare(idServicio, "AGD", true) == 0 || string.Compare(idServicio, "AGDFH", true) == 0)
                        entitlement = "Rio/Fondos_Servicio";
                    else
                        entitlement = "Rio/Fondos_Error";
                    break;
                default:
                    entitlement = "Rio/Fondos_Servicio";
                    break;
            }

            return entitlement;
        }

        private List<Mensaje> ObtenerMailsAEnviar(ObtenerMensajes request)
        {
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();
            var mails = da.ObtenerMailsAEnviar(request);

            return mails.Where(x => string.Compare(x.IdServicio, Servicio.Agendamiento, true) == 0 || string.Compare(x.IdServicio, Servicio.AgendamientoFH, true) == 0).ToList();
        }

        private List<Mensaje> ObtenerMailsAEnviarACT(ObtenerMensajes request)
        {
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();
            var mails = da.ObtenerMailsAEnviar(request);

            return mails.Where(x => string.Compare(x.IdServicio, Servicio.ACT, true) == 0).ToList();
        }

        public virtual void EnviarMail(string informacionPorNup, RequestSecurity<SolicitudOrden> entity)
        {
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();

            var request = entity.MapperClass<EnviarMensajesReq>(TypeMapper.IgnoreCaseSensitive);
            request.DatosCliente = informacionPorNup;
            da.EnviarMensaje(request);
        }

        public virtual void CargarMensajesDelcliente(string informacionPorNup, RequestSecurity<SolicitudOrden> entity)
        {
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();

            var request = entity.MapperClass<EnviarMensajesNetReq>(TypeMapper.IgnoreCaseSensitive);
            request.DatosCliente = informacionPorNup;
            da.CargarMensajesDelcliente(request);
        }

        public virtual ConfirmaRiesgoResponse RealizarConfirmacionOrdenERI(RequestSecurity<ReqConfirmacionEri> entity)
        {
            var result = new ConfirmaRiesgoResponse();
            var datos = entity.Datos;

            var ext = DependencyFactory.Resolve<IExternalWebApiService>();
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();

            if (datos != null)
            {
                var ListaAltaSuscripcion = datos.AltaSuscripcion.ToList();
                var ListaConfirmacionEri = datos.ConfirmacionERI.ToList();
                var qIntersect = ListaAltaSuscripcion.Where(x => ListaConfirmacionEri.Select(y => y.IdSolicitudOrdenes).Contains(x.IdSolicitudOrdenes)).ToList();

                qIntersect.ForEach(item =>
                {
                    try
                    {
                        if (item.TipoDisclaimer != 2 && item.IdEvaluacion > 0 && item.NumOrdenOrigen > 0 && item.Segmento.ToUpper() == TipoSegmento.Retail)
                        {

                            RespuestaWS confirmacionOrden = null;
                            var formReq2 = entity.MapperClass<RequestSecurity<ConfirmacionOrdenReq>>(TypeMapper.IgnoreCaseSensitive);
                            formReq2.Datos = item.MapperClass<ConfirmacionOrdenReq>(TypeMapper.IgnoreCaseSensitive);
                            formReq2.Datos.IdOrden = item.NumOrdenOrigen.ToString();
                            formReq2.Datos.Canal = item.CodCanal;

                            confirmacionOrden = ext.ConfirmacionOrden(formReq2).MapperClass<RespuestaWS>(TypeMapper.IgnoreCaseSensitive);

                            if (confirmacionOrden.CodigoResp.Equals(0))
                            {
                                result.Confirmadas.Add(item);
                                BusinessHelper.GuardarConfirmacionERI(item);
                            }
                            else
                            {
                                result.NoConfirmadas.Add(item);
                                string DescError = "Código de error: " + confirmacionOrden.CodigoResp + " , Mensaje: " + confirmacionOrden.MensajeError;
                                BusinessHelper.GuardarOtros(item, DescError);
                            }
                        }
                        else
                        {
                            string DescError = string.Empty;
                            if (item.TipoDisclaimer == 2)
                                DescError = "No se puede continuar por Disclaimer Restrictivo";
                            else if (item.IdEvaluacion == 0)
                                DescError = "No se puede continuar por no contener IdEvaluacion";
                            else if (item.NumOrdenOrigen == 0)
                                DescError = "No se puede continuar por no contener NumOrdenOrigen";
                            else if (item.Segmento != "RTL")
                                DescError = "No se puede continuar porque el Segmento no es RTL";

                            BusinessHelper.GuardarOtros(item, DescError);
                        }
                    }
                    catch (Exception ex)
                    {
                        BusinessHelper.GuardarFallaTecnica(ex, item, "RealizarConfirmacionOrdenERI");
                    }

                });


            }
            return result;
        }

        //public virtual List<CargaSolicitudOrden> ConsultarSaldoRescatado(RequestSecurity<SolicitudOrden> entity)
        //{
        //    var firma = entity.MapperClass<DatoFirmaMaps>();
        //    var result = entity.Datos.Solicitudes.ToList();

        //    result.ForEach(sol =>
        //    {
        //        try
        //        {
        //            var soliSec = entity.MapperClass<RequestSecurity<CargaSolicitudOrden>>(TypeMapper.IgnoreCaseSensitive);
        //            soliSec.Datos = sol;
        //            var saldoRescatadoResult = DependencyFactory.Resolve<IExternalWebApiService>().ObtenerSaldoRescatado48hs(soliSec);
        //            var suma = saldoRescatadoResult?.ListaSaldoRescatado.Where(y => y.MonedaCta.ToUpper().Equals(TipoMoneda.DolaresUSA)).Sum(x => x.SaldoRescatadoFondo);

        //            sol.SaldoRescatePorFondo = suma;
        //            BusinessHelper.GuardarSaldoRescatado(sol);
        //        }
        //        catch (Exception ex)
        //        {
        //            BusinessHelper.GuardarFallaTecnica(ex, sol, "ConsultarSaldoRescatado");
        //        }
        //    });

        //    return result;
        //}
        #endregion

        #region Métodos Privados
        private long? ValidarCuentas(List<ConsultaLoadAtisResponse> atisResp, long nroCtaOperativa, long nroCtaTitulos)
        {
            bool cuentasValidas = false;

            foreach (var item in atisResp)
            {
                cuentasValidas = item.Validar(item.CuentaBp.Value.ToString().Substring(3, item.CuentaBp.Value.ToString().Length - 3), nroCtaOperativa.ToString());//sacar los 3 primeros.
                cuentasValidas = item.Validar(item.CuentaAtit.Value.ToString(), nroCtaTitulos.ToString());

                if (cuentasValidas)
                {
                    return item.CuentaBp;
                }
            }

            if (!cuentasValidas)
            {
                throw new BusinessException("La Cuenta Título y Número de Cuenta, no corresponden al cliente");
            }

            return -1;
        }

        private string TraducirUsdAUsb(string monedaOperacion)
        {
            switch (monedaOperacion.ToUpper())
            {
                case "USD": return "USB";
                default: return monedaOperacion;
            }
        }

        private ClienteDDC[] ObtenerCuentas(RequestSecurity<CargaSolicitudOrden> request, CabeceraConsulta cabecera)
        {
            var result = new List<ClienteDDC>();
            var usuarioRacf = DependencyFactory.Resolve<IMotorServicioDataAccess>().ObtenerUsuarioRacf();
            var header = GenerarCabecera(request);
            header.H_UsuarioID = usuarioRacf.Usuario;
            header.H_UsuarioPwd = usuarioRacf.Password;

            //TODO: validar si es necesario mapeara datos.
            var reqCuentas = request.MapperClass<RequestSecurity<GetCuentas>>(TypeMapper.IgnoreCaseSensitive);
            reqCuentas.Datos = request.Datos.MapperClass<GetCuentas>(TypeMapper.IgnoreCaseSensitive);
            reqCuentas.Datos.Cabecera = header;
            reqCuentas.Datos.DatoConsulta = reqCuentas.Datos.Cabecera.H_Nup = request.Datos.Nup;
            reqCuentas.Datos.TipoBusqueda = "N";
            reqCuentas.Datos.Titulares = "N";//No trae los participantes de la cuenta.
            reqCuentas.Datos.CuentasRespuesta = "TO";

            var ddcCuentasResult = DependencyFactory.Resolve<IExternalWebApiService>().GetCuentas(reqCuentas);
            if (ddcCuentasResult != null)
                result.AddRange(ddcCuentasResult);
            //TODO: validar que devuelva lo mismo que el IF
            return ddcCuentasResult?.MapperEnumerable<ClienteDDC>(TypeMapper.IgnoreCaseSensitive).ToArray();

        }

        private ClienteDDC[] ObtenerCuentas(RequestSecurity<WorkflowCTRReq> request, CabeceraConsulta cabecera)
        {
            var result = new List<ClienteDDC>();
            var usuarioRacf = DependencyFactory.Resolve<IMotorServicioDataAccess>().ObtenerUsuarioRacf();
            var header = GenerarCabecera(request);
            header.H_UsuarioID = usuarioRacf.Usuario;
            header.H_UsuarioPwd = usuarioRacf.Password;

            //TODO: validar si es necesario mapeara datos.
            var reqCuentas = request.MapperClass<RequestSecurity<GetCuentas>>(TypeMapper.IgnoreCaseSensitive);
            reqCuentas.Datos = request.Datos.MapperClass<GetCuentas>(TypeMapper.IgnoreCaseSensitive);
            reqCuentas.Datos.Cabecera = header;
            reqCuentas.Datos.DatoConsulta = reqCuentas.Datos.Cabecera.H_Nup = request.Datos.Nup;
            reqCuentas.Datos.TipoBusqueda = "N";
            reqCuentas.Datos.Titulares = "N";//No trae los participantes de la cuenta.
            reqCuentas.Datos.CuentasRespuesta = "TO";

            var ddcCuentasResult = DependencyFactory.Resolve<IExternalWebApiService>().GetCuentas(reqCuentas);
            if (ddcCuentasResult != null)
                result.AddRange(ddcCuentasResult);
            //TODO: validar que devuelva lo mismo que el IF
            return ddcCuentasResult?.MapperEnumerable<ClienteDDC>(TypeMapper.IgnoreCaseSensitive).ToArray();

        }

        private static CabeceraConsulta GenerarCabecera(CargaSolicitudOrden entity)
        {
            //TODO: se tiene que reemplazar por la forma correcta según dijo lucas.
            return new CabeceraConsulta()
            {
                H_CanalTipo = entity.Canal, // "22",
                H_SubCanalId = "HTML",
                H_CanalVer = "000",
                H_SubCanalTipo = "99",
                H_CanalId = entity.SubCanal,
                H_UsuarioTipo = "03",
                H_UsuarioID = "ONLINEBP",
                H_UsuarioAttr = " ",
                H_UsuarioPwd = "DV09SA10",
                H_IdusConc = "00000000",
                H_NumSec = "00000002",
                H_IndSincro = "0",
                H_TipoCliente = "I",
                H_TipoIDCliente = "N",
                H_NroIDCliente = "13488020",
                H_FechaNac = "19570812"
            };

        }

        private static CabeceraConsulta GenerarCabecera(RequestSecurity<CargaSolicitudOrden> entity)
        {
            //TODO: se tiene que reemplazar por la forma correcta según dijo lucas.
            return new CabeceraConsulta()
            {
                H_CanalTipo = entity.Canal, // "22",
                //H_SubCanalId = "HTML",
                //H_CanalVer = "000",
                H_SubCanalTipo = entity.SubCanal, //"0000",
                //H_CanalId = "0001",
                //H_UsuarioTipo = "03",
                //H_UsuarioID = "01FRQF31",
                H_UsuarioAttr = "  ",
                //H_UsuarioPwd = "@DP33YTO",
                //H_IdusConc = "788646",
                //H_NumSec = "14",
                //H_IndSincro = "1",
                //H_TipoCliente = "I",
                //H_TipoIDCliente = "N",
                //H_NroIDCliente = "00020956698",
                //H_FechaNac = "19690922"
            };

        }

        public List<CargaSolicitudOrden> ConsultaAdhesionesPorFallaTecnica(RequestSecurity<WorkflowSAFReq> entity)
        {
            try
            {
                IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();
                var result = da.ConsultaAdhesionesPorFallaTecnica(entity.Datos);
                //TODO: traigo todoo tengo que filtrarlas por algo.

                return result;
            }
            catch (Exception ex)
            {
                //TODO: ver que se hará acá porque ni arranco aún.              
                throw ex;
            }
        }

        public ResultadoFondos RealizarRescateFondo(RequestSecurity<SolicitudOrden> entity)
        {
            var datos = entity.Datos;
            var result = new ResultadoFondos();
            ICanalesIatxDA daIATX = DependencyFactory.Resolve<ICanalesIatxDA>();
            var usuarioRacf = DependencyFactory.Resolve<IMotorServicioDataAccess>().ObtenerUsuarioRacf();
            var usuarioAsesor = DependencyFactory.Resolve<IMotorServicioDataAccess>().ObtenerUsuarioAsesor();
            var firma = entity.MapperClass<DatoFirmaMaps>();

            datos.Solicitudes.ForEach(solicitud =>
            {
                try
                {
                    #region Banca Privada
                    if (solicitud.Segmento == TipoSegmento.BancaPrivada)
                    {

                        string dentroDePerfil = DependencyFactory.Resolve<ISmcDA>().SimulacionAltaFondos(new SimularAltaFondos
                        {
                            Canal = solicitud.Canal,
                            Tipo = TipoSolicitud.Suscripcion,
                            Cuenta = solicitud.NroCtaOperativaFormateada,
                            Especie = solicitud.CodEspecie,
                            Moneda = solicitud.CodMoneda,
                            Cuotapartes = null,
                            Capital = solicitud.SaldoEnviado,
                            EspecieDestino = null,
                            UsuarioRacf = usuarioRacf.Usuario,
                            UsuarioAsesor = usuarioAsesor,
                            PasswordRacf = usuarioRacf.Password

                        });

                        var ejecutarAltaFondos = DependencyFactory.Resolve<ISmcDA>().EjecutarAltaFondos(new EjecutarAltaFondos
                        {
                            Canal = solicitud.Canal,
                            Tipo = TipoSolicitud.Suscripcion,
                            Cuenta = solicitud.NroCtaOperativaFormateada,
                            Especie = solicitud.CodEspecie,
                            Moneda = solicitud.CodMoneda,
                            Cuotapartes = null,
                            Capital = solicitud.SaldoEnviado,
                            EspecieDestino = null,
                            UsuarioRacf = usuarioRacf.Usuario,
                            UsuarioAsesor = usuarioAsesor,
                            PasswordRacf = usuarioRacf.Password,
                            DentroDelPerfil = dentroDePerfil

                        });

                        solicitud.NumOrdenOrigen = ejecutarAltaFondos.NumOrden.ParseGeneric<long>();
                        solicitud.TextoDisclaimer = ejecutarAltaFondos.Disclaimer;
                        BusinessHelper.GuardarProcesado(solicitud, dentroDePerfil);
                    }
                    #endregion
                    #region RTL
                    else
                    {
                        SuscripcionFCIIATXResponse suscripcionFCIIATX = daIATX.SuscripcionFCI(GenerarCabecera(solicitud), solicitud.Nup,
                                     Convert.ToInt16(solicitud.CodigoFondo),
                                     solicitud.CuentaTitulos.ToString(),
                                     solicitud.SaldoEnviado.Value,
                                     Convert.ToInt16(solicitud.TipoCtaOper),
                                     Convert.ToInt16(solicitud.SucuCtaOper),
                                     solicitud.NroCtaOper.ToString(),
                                     solicitud.CodMoneda == TipoMoneda.PesosAR ? 0 : 2);
                        BusinessHelper.GuardarProcesado(solicitud, suscripcionFCIIATX.Numero_Certificado.ToString());
                        solicitud.NumOrdenOrigen = suscripcionFCIIATX.Numero_Certificado.ParseGeneric<long>();
                    }
                    #endregion

                    result.Ok.Add(solicitud);
                }
                catch (Exception ex)
                {
                    result.NoOk.Add(solicitud);
                    if (ex is IatxCodeException)
                    {
                        string DescError = "Tipo de error: IatxCodeException, Mensaje: " + ex.InnerException.Message.ToString();
                        BusinessHelper.GuardarFallaTecnica(ex, solicitud, DescError);
                    }
                    else
                        BusinessHelper.GuardarFallaTecnica(ex, solicitud, "RealizarSuscripcionFondos");
                }
            });

            return result;
        }

        /// <summary>
        /// Servicio para poder actualizar los parametros de la Tabla Parametros de MAPS de forma dinámica
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public dynamic ActualizarMapsParametros(RequestSecurity<ActualizarMapsParametro> entity)
        {
            var result = DependencyFactory.Resolve<IMotorServicioDataAccess>().ActualizarMapsParametros(entity.Datos);

            return result;
        }

        public List<CargaSolicitudOrden> ConsultaAdhesionesActivas(RequestSecurity<WorkflowSAFReq> entity)
        {

            List<CargaSolicitudOrden> solis = new List<CargaSolicitudOrden>();

            try
            {
                IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();
                entity.Datos.IdServicio = Servicio.Agendamiento;
                var result = da.ConsultaAdhesionesActivas(entity.Datos);
                solis = result.Where(x => DateTime.Now.Date >= x.FecVigenciaDesde.Date
                                           && DateTime.Now.Date <= x.FecVigenciaHasta.Date
                                     ).ToList();


                entity.Datos.IdServicio = Servicio.AgendamientoFH;
                var resultFH = da.ConsultaAdhesionesActivas(entity.Datos);

                var fh = resultFH.Where(x => DateTime.Now.Date >= x.FecVigenciaDesde.Date
                                           && DateTime.Now.Date <= x.FecVigenciaHasta.Date
                                     ).ToList();

                if(fh != null && fh.Count() > 0)
                solis.AddRange(fh);

                BusinessHelper.AsignarUsuarioIP(solis);

                return solis;
            }
            catch (Exception ex)
            {
                //TODO: ver que se hará acá porque ni arranco aún.              
                throw ex;
            }

        }

        public virtual List<CargaSolicitudOrden> ConsultaAdhesionesEjecutar(RequestSecurity<WorkflowSAFReq> entity, string operacion)
        {
            try
            {
                var solicitudes = ConsultaAdhesionesActivas(entity);

                var result = solicitudes.Where(p => p.Operacion == operacion || p.Operacion == "Suscripcion")
                .Where(p => p.FechaDeEjecucion.Date == DateTime.Now.Date);

                return result.ToList();
            }
            catch (Exception ex)
            {
                //TODO: ver que se hará acá porque ni arranco aún.              
                throw ex;
            }
        }

        public virtual ResultadoTransferencia RealizarTransferencia(RequestSecurity<OrdernBase> entity)
        {

            var datos = new List<DatosTransferencia>();
            var result = new ResultadoTransferencia();
            var usuarioRacf = DependencyFactory.Resolve<IMotorServicioDataAccess>().ObtenerUsuarioRacf();
            var usuarioAsesor = DependencyFactory.Resolve<IMotorServicioDataAccess>().ObtenerUsuarioAsesor();
            var firma = entity.MapperClass<DatoFirmaMaps>();
            ICanalesIatxDA daIATX = DependencyFactory.Resolve<ICanalesIatxDA>();


            datos.ForEach(x =>
            {


                //result.Ok(x);

                //result.NoOkOk(x);

            });


            return result;
        }
        #endregion

    }
}
