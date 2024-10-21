
namespace Isban.MapsMB.Business
{
    using Attributes;
    using Bussiness.WSERIService;
    using ColaMQ;
    using Common.Entity.Request;
    using Entity.Constantes.Estructuras;
    using Isban.MapsMB.Common.Entity;
    using Isban.MapsMB.Common.Entity.Extensions;
    using Isban.MapsMB.Common.Entity.Response;
    using Isban.MapsMB.Entity.Request;
    using Isban.MapsMB.Entity.Response;
    using Isban.MapsMB.FaxMailRTF;
    using Isban.MapsMB.IBussiness;
    using Isban.MapsMB.IDataAccess;
    using Isban.MapsMB.IDataAccesss;
    using Isban.Mercados;
    using Isban.Mercados.LogTrace;
    using Isban.Mercados.Service.InOut;
    using Isban.Mercados.UnityInject;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;

    public class MotorBusiness : IMotorBusiness
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
            //businessMB.RealizarRescateDeCuentasBloqueadas(ctasBloqueadas);//TODO: falta ver como se hace.

            //4- consulta saldo de las cuentas OK
            var solConsultaSaldoCuenta = entity.MapperClass<RequestSecurity<SolicitudOrden>>(TypeMapper.IgnoreCaseSensitive);
            solConsultaSaldoCuenta.Datos.Solicitudes = estadoDeCuentas.Activas;
            var consultaSaldoCuenta = businessMB.ConsultaSaldoCuenta(solConsultaSaldoCuenta);

            //5 - Consulto el Saldo rescatado en P&L OK
            var solConsultaSaldoRescatado = entity.MapperClass<RequestSecurity<SolicitudOrden>>(TypeMapper.IgnoreCaseSensitive);
            solConsultaSaldoRescatado.Datos.Solicitudes = consultaSaldoCuenta;
            var resultConsultaSaldoRescatado = businessMB.ConsultarSaldoRescatado(solConsultaSaldoRescatado);

            //6 - filtro de las adhesiones por condiciones OK
            var solFiltrarAdhesionesPorCondicion = entity.MapperClass<RequestSecurity<SolicitudOrden>>(TypeMapper.IgnoreCaseSensitive);
            solFiltrarAdhesionesPorCondicion.Datos.Solicitudes = resultConsultaSaldoRescatado;
            var resultFiltrarAdhesionesPorCondicion = businessMB.FiltrarAdhesionesPorCondicion(solFiltrarAdhesionesPorCondicion);

            //7 - realizar Evaluación de riesgo ERI OK
            var secEvaRiesto = entity.MapperClass<RequestSecurity<SolicitudOrden>>(TypeMapper.IgnoreCaseSensitive);
            secEvaRiesto.Datos.Cabecera = entity.Datos.Cabecera;
            secEvaRiesto.Datos.Solicitudes = resultFiltrarAdhesionesPorCondicion.SaldoSuficiente;
            EvaluaRiesgoResponse evaRiesgo = businessMB.RealizarEncuestaDeRiesgoERI(secEvaRiesto);


            //8- alta de solicitud de orden fondos ok
            var solAltaFondo = entity.MapperClass<RequestSecurity<SolicitudOrden>>(TypeMapper.IgnoreCaseSensitive);
            solAltaFondo.Datos.Solicitudes = resultFiltrarAdhesionesPorCondicion.SaldoSuficiente;
            var altaFondoResult = businessMB.RealizarAltaAdhesionFondos(solAltaFondo);

            //9 - confirmo en ERI la encuesta.
            //RealizarConfirmacionOrdenERI() falta convertir el join al tipo de datos CargaSolicitudOrden.
            var secConfOrdenERI = entity.MapperClass<RequestSecurity<ReqConfirmacionEri>>(TypeMapper.IgnoreCaseSensitive);
            var reqConfirmacion = new ReqConfirmacionEri();
            reqConfirmacion.ConfirmacionERI = evaRiesgo.NoRestrictivo;
            reqConfirmacion.AltaSuscripcion = altaFondoResult.Ok;
            secConfOrdenERI.Datos = reqConfirmacion;

            ConfirmaRiesgoResponse confRiesgoERI = businessMB.RealizarConfirmacionOrdenERI(secConfOrdenERI);

            //10- confirmación por correo MYA
            var realizarConfirmacionMYA = entity.MapperClass<RequestSecurity<SolicitudOrden>>(TypeMapper.IgnoreCaseSensitive);
            businessMB.RealizarConfirmacionMYA(realizarConfirmacionMYA);

            return "Último servicio llamado RealizarConfirmacionMYA";
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

        public virtual List<CargaSolicitudOrden> ConsultaAdhesionesActivas(RequestSecurity<WorkflowSAFReq> entity)
        {
            List<CargaSolicitudOrden> solis = new List<CargaSolicitudOrden>();

            try
            {
                IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();
                entity.Datos.IdServicio = Servicio.SAF;
                var result = da.ConsultaAdhesionesActivas(entity.Datos);
                solis = result.Where(x => DateTime.Now.Date >= x.FecVigenciaDesde.Date
                                           && DateTime.Now.Date <= x.FecVigenciaHasta.Date
                                     ).ToList();

                BusinessHelper.AsignarUsuarioIP(solis);

                return solis;
            }
            catch (Exception ex)
            {
                //TODO: ver que se hará acá porque ni arranco aún.              
                throw ex;
            }
        }

        public virtual List<CargaSolicitudOrden> ConsultaAdhesionesRTFActivas(RequestSecurity<WorkflowSAFReq> entity)
        {
            List<CargaSolicitudOrden> solis = new List<CargaSolicitudOrden>();

            try
            {
                IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();
                entity.Datos.IdServicio = Servicio.RTF;
                var result = da.ConsultaAdhesionesActivas(entity.Datos);
                solis = result?.Where(x => DateTime.Now.Date >= x.FecVigenciaDesde.Date
                                           && DateTime.Now.Date <= x.FecVigenciaHasta.Date
                                     )?.ToList();
                if (solis == null)
                    solis = new List<CargaSolicitudOrden>();

                BusinessHelper.AsignarUsuarioIP(solis);

                return solis;
            }
            catch (Exception ex)
            {
                LoggingHelper.Instance.Error(ex, $"Error consulta adhesiones Mail:{ex.Message}", "MotorBusiness", "ConsultaAdhesionesRTFActivas");
                return new List<CargaSolicitudOrden>();

            }
        }

        public virtual List<CargaSolicitudOrden> ConsultaAdhesionesActivasACT(RequestSecurity<WorkflowSAFReq> entity)
        {
            List<CargaSolicitudOrden> solis = new List<CargaSolicitudOrden>();

            try
            {
                IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();
                entity.Datos.IdServicio = Servicio.ACT;
                var result = da.ConsultaAdhesionesActivas(entity.Datos);

                BusinessHelper.AsignarUsuarioIP(solis);

                return result;
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
                var solicitudes = ConsultaAdhesionesActivas(entity)
                    .Where(p => p.Operacion == operacion)
                    .Where(p => p.FechaDeEjecucion.Date == DateTime.Now.Date)
                    ;

                return solicitudes.ToList();
            }
            catch (Exception ex)
            {
                //TODO: ver que se hará acá porque ni arranco aún.              
                throw ex;
            }
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
        public virtual ResultadoFondos RealizarAltaAdhesionFondos(RequestSecurity<SolicitudOrden> entity)
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

        public virtual ResultadoFondosOBE SubscribirFondos(RequestSecurity<SolicitudOrden> entity)
        {
            var datos = entity.Datos;
            var result = new ResultadoFondosOBE();
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

                    }
                    #endregion
                    #region RTL
                    else
                    {
                        SuscripcionFCIIATXResponse suscripcionFCIIATX = daIATX.SuscripcionFCIOBE(GenerarCabeceraFondos(solicitud, datos.Cabecera.H_Func_Autor,
                            datos.Cabecera.CodigoCanal, solicitud.Documento, solicitud.TipoDocumento), solicitud.Nup,
                                     Convert.ToInt16(solicitud.CodigoFondo),
                                     solicitud.CuentaTitulos.ToString(),
                                     solicitud.SaldoEnviado.Value,
                                     Convert.ToInt16(solicitud.TipoCtaOper),
                                     Convert.ToInt16(solicitud.SucuCtaOper),
                                     solicitud.NroCtaOper.ToString(),
                                     solicitud.CodMoneda == TipoMoneda.PesosAR ? 0 : 2);

                        //solicitud = suscripcionFCIIATX.Numero_Certificado.ParseGeneric<long>();
                    }
                    #endregion

                    var response = solicitud.MapperClass<SubscripcionFondoResponse>(TypeMapper.IgnoreCaseSensitive);

                    response.Estado = "PENDIENTE";

                    result.Ok.Add(response);
                }
                catch (Exception ex)
                {

                    var response = solicitud.MapperClass<SubscripcionFondoResponse>(TypeMapper.IgnoreCaseSensitive);

                    response.Estado = "ERROR";

                    LoggingHelper.Instance.Error($"Message: {ex.Message}", "MotorBusiness", "RealizarSuscripcionFondos");
                    LoggingHelper.Instance.Error($"InnerException: {(ex.InnerException == null ? ex.Message : ex.InnerException.Message)}", "MotorBusiness", "RealizarSuscripcionFondos");
                    result.NoOk.Add(response);
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

        [NonTransactionInterceptor]
        public virtual List<CargaSolicitudOrden> ConsultaSaldoCuenta(RequestSecurity<SolicitudOrden> entity)
        {
            ICanalesIatxDA daIATX = DependencyFactory.Resolve<ICanalesIatxDA>();
            var item = entity.Datos.Solicitudes.ToList();
            DatosCuentaIATXResponse datosCuentaIATXResponse = null;
            var usuarioRacf = DependencyFactory.Resolve<IMotorServicioDataAccess>().ObtenerUsuarioRacf();

            item.ForEach(soli =>
            {
                int tipoCta = 0;
                int sucCtaOper = 0;


                int.TryParse(soli.TipoCtaOper.ToString(), out tipoCta);
                int.TryParse(soli.SucuCtaOper.ToString(), out sucCtaOper);

                try
                {
                    #region BancaPrivada
                    if (soli.Segmento.ToUpper() == TipoSegmento.BancaPrivada)
                    {
                        decimal sumSaldo = 0;
                        //soli.Canal = "79";   
                        var atisResp = DependencyFactory.Resolve<IOpicsDA>().ObtenerAtis(new ConsultaLoadAtisRequest
                        {
                            Nup = soli.Nup.ParseGeneric<long?>(),
                            CuentaBp = 0
                        });

                        var cuentaBp = ValidarCuentas(atisResp, soli.NroCtaOper, soli.CuentaTitulos);

                        var resLoadSaldos = DependencyFactory.Resolve<IOpicsDA>().EjecutarLoadSaldos(new LoadSaldosRequest
                        {
                            Canal = soli.Canal,// entity.Canal,
                            Cuenta = cuentaBp.Value.ParseGeneric<string>(),
                            FechaDesde = DateTime.Now.Date,
                            FechaHasta = DateTime.Now.Date,
                            Usuario = usuarioRacf.Usuario,
                            Password = usuarioRacf.Password
                        });

                        var resSaldoConcertado = DependencyFactory.Resolve<ISmcDA>().EjecutarSaldoConcertadoNoLiquidado(new SaldoConcertadoNoLiquidadoRequest
                        {
                            Fecha = DateTime.Now.Date,
                            Ip = soli.Ip,
                            Moneda = TraducirUsdAUsb(soli.CodMoneda),
                            NroCtaOper = soli.NroCtaOper.ParseGeneric<string>(),
                            SucCtaOper = soli.SucuCtaOper.ParseGeneric<string>(),
                            TipoCtaOper = soli.TipoCtaOper.ParseGeneric<decimal>(),
                            Usuario = soli.Usuario
                        });


                        switch (soli.CodMoneda.ToLower())
                        {
                            case "ars":
                                foreach (var saldo in resLoadSaldos.ListaSaldos)
                                {

                                    sumSaldo += saldo.CAhorroPesos;
                                }

                                break;

                            case "usd":
                                foreach (var saldo in resLoadSaldos.ListaSaldos)
                                {
                                    sumSaldo += saldo.CAhorroDolares;
                                }

                                break;
                        }

                        var totalSaldo = sumSaldo - (resSaldoConcertado.Saldo.HasValue ? resSaldoConcertado.Saldo.Value : 0m);
                        soli.SaldoCuentaAntes = totalSaldo;
                    }
                    #endregion
                    #region RTL
                    else
                    {
                        var cabecera = GenerarCabecera(soli);
                        cabecera.H_UsuarioID = usuarioRacf.Usuario;
                        cabecera.H_UsuarioPwd = usuarioRacf.Password;

                        datosCuentaIATXResponse = daIATX.ConsultaDatosCuenta(cabecera, soli);
                        var entero = datosCuentaIATXResponse.Dispuesto_USD_cuenta.Substring(0, 12);
                        var dec = datosCuentaIATXResponse.Dispuesto_USD_cuenta.Substring(12, 2);
                        var signo = datosCuentaIATXResponse.Dispuesto_USD_cuenta.Substring(14, 1);
                        decimal numDec = 0;

                        if (decimal.TryParse($"{signo}{entero}.{dec}", out numDec))
                        {
                            soli.SaldoCuentaAntes = numDec;
                        }
                    }
                    #endregion

                    BusinessHelper.GuardarSaldoCuentaAntes(soli);
                }
                catch (Exception ex)
                {
                    BusinessHelper.GuardarFallaTecnica(ex, soli, usuarioRacf);

                }

            });

            return item;
        }

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
        public virtual CondicionAdhesion FiltrarAdhesionesPorCondicion(RequestSecurity<SolicitudOrden> entity)
        {
            var result = new CondicionAdhesion();
            var datos = entity.Datos.Solicitudes.ToList();

            datos.ForEach(soli =>
            {
                try
                {
                    decimal saldoActual = soli.SaldoCuentaAntes.HasValue ? soli.SaldoCuentaAntes.Value : 0;
                    decimal saldoFinal = soli.SaldoRescatePorFondo.HasValue ? saldoActual - soli.SaldoRescatePorFondo.Value : saldoActual;
                    saldoFinal = saldoFinal - soli.SaldoMinDejarCta;

                    //Según PELI:
                    //1- si el saldo final es el maximo o superior, suscribo por salgo máximo.
                    //2- si saldo final es menor al maximo, suscribo por el mínimo.

                    if (saldoFinal >= soli.SaldoMaxOperacion && saldoFinal >= soli.SaldoMinPorFondo)
                    {
                        soli.SaldoEnviado = soli.SaldoMaxOperacion;
                        result.SaldoSuficiente.Add(soli);//Puede seguir operando
                    }
                    else if (((saldoFinal >= soli.SaldoMinOperacion && saldoFinal <= soli.SaldoMaxOperacion) && saldoFinal < soli.SaldoMaxOperacion) && saldoFinal >= soli.SaldoMinPorFondo)
                    {
                        soli.SaldoEnviado = saldoFinal;
                        result.SaldoSuficiente.Add(soli);//Puede seguir operando
                    }
                    else
                    {
                        soli.SaldoEnviado = saldoFinal;
                        result.SaldoInsuficiente.Add(soli);//No posee saldo suficiente para operar

                        BusinessHelper.GuardarSinSaldo(soli, saldoFinal);
                    }
                }
                catch (Exception ex)
                {
                    BusinessHelper.GuardarFallaTecnica(ex, soli, "FiltrarAdhesionesPorCondicion");
                }
            });

            return result;
        }

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
            result.Exitoso = true;
            result.Descripcion = $"Se corrio los envios de mails al {DateTime.Now}.";

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
                    var texto = ObtenerTexto(mail);

                    try
                    {
                        var mensaje = new MensajeMyA();
                        mensaje.HEADER = new Header()
                        {
                            RIONUP = (mail.Segmento == TipoSegmento.BancaPrivada ? String.Empty : mail.IdNup),
                            RIODNICUIT = mail.NumeroDocumento.ToString(),
                            ENTITLEMENT = ObtenerEntitlement(mail.CodEstadoProceso),
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
                            TEXTO = texto.TextoMensaje,
                            TITULO = texto.TituloSolapa,
                            DESCRIPCION = texto.Descripcion
                        };

                        var colaMq = new ColaMQ();
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
            return result;
        }

        private void CargarMensajesDelclienteNET(string informacionPorNup, RequestSecurity<SolicitudOrden> entity)
        {
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();

            var request = entity.MapperClass<EnviarMensajesNetReq>(TypeMapper.IgnoreCaseSensitive);
            request.DatosCliente = informacionPorNup;
            da.CargarMensajesDelclienteNET(request);
        }

        private void CargarMensajesDelclienteNET(string informacionPorNup, RequestSecurity<WorkflowCTRReq> entity)
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
                    case "A":
                    case "BA":
                    case "BL":
                    case "PR":
                    case "FT":
                    case "BV":
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
                    case "A":
                    case "BA":
                    case "BL":
                    case "PR":
                    case "FT":
                    case "BV":
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
                IdServicio = mail.IdServicio

            });
        }

        private Texto ObtenerTextoRTF(Mensaje mail)
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

        private string ObtenerEntitlement(string codEstadoProceso)
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

                    entitlement = "Rio/Fondos_Error";
                    break;
                default:
                    entitlement = "Rio/Fondos_Servicio";
                    break;
            }

            return entitlement;
        }

        private List<Entity.Response.Mensaje> ObtenerMailsAEnviar(ObtenerMensajes request)
        {
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();
            var mails = da.ObtenerMailsAEnviar(request);

            return mails.Where(x => string.Compare(x.IdServicio, Servicio.SAF, true) == 0).ToList();
        }

        private List<Entity.Response.Mensaje> ObtenerMailsAEnviarRTF(ObtenerMensajes request)
        {
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();
            var mails = da.ObtenerMailsAEnviar(request);

            return mails.Where(x => string.Compare(x.IdServicio, Servicio.ACT, true) == 0).ToList();
        }

        private List<Entity.Response.Mensaje> ObtenerMailsAEnviarACT(ObtenerMensajes request)
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

        public virtual List<CargaSolicitudOrden> ConsultarSaldoRescatado(RequestSecurity<SolicitudOrden> entity)
        {
            var firma = entity.MapperClass<DatoFirmaMaps>();
            var result = entity.Datos.Solicitudes.ToList();

            result.ForEach(sol =>
            {
                try
                {
                    var soliSec = entity.MapperClass<RequestSecurity<CargaSolicitudOrden>>(TypeMapper.IgnoreCaseSensitive);
                    soliSec.Datos = sol;
                    var saldoRescatadoResult = DependencyFactory.Resolve<IExternalWebApiService>().ObtenerSaldoRescatado48hs(soliSec);
                    var suma = saldoRescatadoResult?.ListaSaldoRescatado.Where(y => y.MonedaCta.ToUpper().Equals(TipoMoneda.DolaresUSA)).Sum(x => x.SaldoRescatadoFondo);

                    sol.SaldoRescatePorFondo = suma;
                    BusinessHelper.GuardarSaldoRescatado(sol);
                }
                catch (Exception ex)
                {
                    BusinessHelper.GuardarFallaTecnica(ex, sol, "ConsultarSaldoRescatado");
                }
            });

            return result;
        }

        public ResultadoFondos RealizarRescateFondo(RequestSecurity<SolicitudOrden> entity)
        {
            var result = new ResultadoFondos();
            var daIATX = DependencyFactory.Resolve<ICanalesIatxDA>();
            var usuarioRacf = DependencyFactory.Resolve<IMotorServicioDataAccess>().ObtenerUsuarioRacf();
            var usuarioAsesor = DependencyFactory.Resolve<IMotorServicioDataAccess>().ObtenerUsuarioAsesor();
            var firma = entity.MapperClass<DatoFirmaMaps>();

            entity.Datos.Solicitudes.ForEach(solicitud =>
            {
                try
                {
                    #region Banca Privada
                    if (solicitud.Segmento == TipoSegmento.BancaPrivada)
                    {
                        var reqSimulacion = new SimularAltaFondos
                        {
                            Canal = solicitud.Canal,
                            Tipo = TipoSolicitud.Rescate,
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
                        LoggingHelper.Instance.Information($"Simulacion request: {JsonSerialization.SerializarToJson(reqSimulacion)}", "MotorBusiness", "RescateFondo");
                        var responseSimulacion = DependencyFactory.Resolve<ISmcDA>().SimulacionFondos(reqSimulacion);

                        var reqRescate = reqSimulacion.MapperClass<EjecutarAltaFondos>();
                        reqRescate.DentroDelPerfil = responseSimulacion.DentroDelPerfil;
                        LoggingHelper.Instance.Information($"Confirmacion request: {JsonSerialization.SerializarToJson(reqRescate)}", "MotorBusiness", "RescateFondo");
                        var responseFondos = DependencyFactory.Resolve<ISmcDA>().EjecutarFondos(reqRescate);

                        solicitud.NumOrdenOrigen = responseFondos.NumOrden.ParseGeneric<long>();
                        solicitud.TextoDisclaimer = responseFondos.Disclaimer;
                        BusinessHelper.GuardarProcesado(solicitud, responseSimulacion.DentroDelPerfil);

                    }
                    #endregion
                    #region RTL
                    else
                    {
                        var rescate = daIATX.RescateFCIRTL(GenerarCabecera(solicitud), solicitud);
                        BusinessHelper.GuardarProcesado(solicitud, rescate.Nro_Rescate.ToString());
                        solicitud.NumOrdenOrigen = rescate.Nro_Rescate.ParseGeneric<long>();
                    }
                    #endregion

                    result.Ok.Add(solicitud);
                }
                catch (Exception ex)
                {
                    if (ex is IatxCodeException)
                    {
                        //TODO: Aca si es falla por falta de saldo es otro codigo? Esto falta, no estaba en el Caso de uso
                        string DescError = "RealizarRescateFondo: Tipo de error: IatxCodeException, Mensaje: " + ex.InnerException.Message.ToString();
                        BusinessHelper.GuardarFallaTecnica(ex, solicitud, DescError);
                    }
                    else
                    {
                        BusinessHelper.GuardarFallaTecnica(ex, solicitud, "RealizarRescateFondo");
                    }
                    result.NoOk.Add(solicitud);
                }
            });

            return result;
        }

        public ObtenerRTFDisponiblesResponse ObtenerRTFDisponiblesPorCliente(RequestSecurity<RTFWorkflowOnDemandReq> entity)
        {
            var listaRtf = DependencyFactory.Resolve<IMotorServicioDataAccess>().ConsultaArchivosRTF(entity.Datos);

            var listaCuentas = new List<Cuenta>();

            var reqCuentasD = new RequestSecurity<GetDatosCliente>();

            var datosClienteD = new GetDatosCliente
            {
                DatoConsulta = entity.Datos.Nup,
                //Segmento = "RTL",
                TipoBusqueda = "A",
                CuentasRespuesta = "TI",
                Ip = "1.1.1.1",
                Usuario = "B049684",
                Canal = entity.Canal,
                SubCanal = entity.SubCanal
            };

            reqCuentasD.Datos = datosClienteD;

            var cuentas = DependencyFactory.Resolve<IExternalWebApiService>().GetDatosCliente(reqCuentasD);

            if (cuentas != null)
            {
                foreach (var cuenta in cuentas)
                {
                    cuenta.Cuentas?.ForEach(c => listaCuentas.Add(new Cuenta { NumeroCuenta = c.NroCta, Segmento = c.SegmentoCuenta }));
                }
            }

            var response = new ObtenerRTFDisponiblesResponse();
            response.ListaRTF = listaRtf;
            response.ListaCuentas = listaCuentas;

            return response;
        }

        public List<ArchivoRTF> ObtenerPdfPorCuentaRTF(RequestSecurity<RTFWorkflowOnDemandReq> entity)
        {
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();

            var cuentas = da.ObtenerArchivoRTF(entity.Datos);

            var pathParameter = da.ObtenerParametro("RTF_PATH");

            foreach (var cuenta in cuentas)
            {
                cuenta.Archivo = cuenta.Archivo = DireccionBase64String(Path.Combine(pathParameter, cuenta.Path));
            }

            return cuentas;
        }

        [NonTransactionInterceptor]
        public virtual string RealizarEnvioRTF(RequestSecurity<SolicitudOrden> entity)
        {
            EnvioMailAdhesionesPendientes(entity);

            return "Realizado con exito envio de mails para RTF";
        }

        [NonTransactionInterceptor]
        public virtual MotorResponse EnvioMailAdhesionesPendientes(RequestSecurity<SolicitudOrden> entity)
        {

            MotorResponse result = new MotorResponse();
            result.Exitoso = true;
            result.Descripcion = $"Se corrio los envios de mails al {DateTime.Now}.";

            try
            {
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

                    var mails = ObtenerMailsAEnviarACT(new ObtenerMensajes { Fecha = DateTime.Now.Date });

                    foreach (var mail in mails)
                    {
                        if (mail.CuentaTitulo == 0 || mail.CuentaTitulo == 1) continue;

                        var texto = ObtenerTextoRTF(mail);

                        try
                        {
                            var mensaje = new MensajeMyA();
                            mensaje.HEADER = new Header()
                            {
                                RIONUP = (mail.Segmento == TipoSegmento.BancaPrivada ? String.Empty : mail.IdNup),
                                RIODNICUIT = mail.NumeroDocumento.ToString(),
                                ENTITLEMENT = ObtenerEntitlement(mail.CodEstadoProceso),
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
                                TEXTO = RemplazarTextoACT(texto.TextoMensaje, mail),
                                TITULO = texto.TituloSolapa,
                                DESCRIPCION = RemplazarTextoACT(texto.Descripcion, mail)
                            };

                            var colaMq = new ColaMQ();
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
            catch (Exception ex)
            {
                LoggingHelper.Instance.Error(ex, $"Error en el envio mails ACT:{ex.Message}", "MotorBusiness", "EnvioMailAdhesionesPendientes");
            }

            return result;

        }

        private void CargarMailAltaCuentasMyA(RequestSecurity<SolicitudOrden> entity)
        {
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

                var mails = ObtenerMailsAEnviarACT(new ObtenerMensajes { Fecha = DateTime.Now.Date });

                foreach (var mail in mails)
                {
                    var texto = ObtenerTextoRTF(mail);

                    try
                    {
                        var mensaje = new MensajeMyA();
                        mensaje.HEADER = new Header()
                        {
                            RIONUP = (mail.Segmento == TipoSegmento.BancaPrivada ? String.Empty : mail.IdNup),
                            RIODNICUIT = mail.NumeroDocumento.ToString(),
                            ENTITLEMENT = ObtenerEntitlement(mail.CodEstadoProceso),
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
                            TEXTO = RemplazarTextoACT(texto.TextoMensaje, mail),
                            TITULO = texto.TituloSolapa,
                            DESCRIPCION = texto.Descripcion
                        };

                        var colaMq = new ColaMQ();
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

        private void EjecutarProcesoEnvioMails(RequestSecurity<WorkflowCTRReq> entity)
        {
            var mails = ObtenerMailsAEnviarACT(new ObtenerMensajes { Fecha = DateTime.Now.Date });

            foreach (var mail in mails)
            {
                var texto = ObtenerTexto(mail);

                try
                {
                    var mensaje = new MensajeMyA();
                    mensaje.HEADER = new Header()
                    {
                        RIONUP = (mail.Segmento == TipoSegmento.BancaPrivada ? String.Empty : mail.IdNup),
                        RIODNICUIT = mail.NumeroDocumento.ToString(),
                        ENTITLEMENT = ObtenerEntitlement(mail.CodEstadoProceso),
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
                        TEXTO = RemplazarTextoACT(texto.TextoMensaje, mail),
                        TITULO = texto.TituloSolapa,
                        DESCRIPCION = texto.Descripcion
                    };

                    var colaMq = new ColaMQ();
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

        [NonTransactionInterceptor]
        public virtual MotorResponse EnvioResumenPorFaxMail(RequestSecurity<WorkflowSAFReq> entity)
        {
            MotorResponse result = new MotorResponse();
            result.Exitoso = true;
            result.Descripcion = $"Se corrio correctamente la generacion de RTF al {DateTime.Now}.";

            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();

            var pathParameter = da.ObtenerParametro("RTF_PATH");

            var cuentasPorNup = ObtenerCuentasActivasRTFPorNuP(entity);

            foreach (var item in cuentasPorNup)
            {
                var pathPdfGenerados = new List<string>();

                try
                {
                    var listaFondos = ObtenerTenenciaFondos(entity, item);

                    if (listaFondos == null) continue;

                    var direccion = GetDireccionPorNup(item.Key);
                    var nombreCliente = GetNombreCliente(item.Key, entity.Canal, entity.SubCanal);

                    foreach (var cuentaFondo in listaFondos.AgrupadoPorCuenta)
                    {
                        if (ValidarSiTieneCuentas(cuentaFondo))
                        {
                            var pdf = new RTFGeneracionPdF();
                            var pathArchivo = pdf.CreacionPdf(cuentaFondo, item.Key, nombreCliente, pathParameter, direccion, listaFondos.Legales, GetNombresClientes(cuentaFondo.CtaTitulo.ToString(), entity.Canal, entity.SubCanal));

                            VolcadoDatosRTF(pathArchivo, cuentaFondo, item.Key, cuentaFondo.Periodo, "S");

                        }

                    }
                }
                catch (Exception ex)
                {
                    foreach (var cuenta in item.Value)
                    {
                        try
                        {
                            var message = ex.Message;
                            message = ex.Message.Length > 300 ? ex.Message.Substring(0, 300) : ex.Message;

                            LoggingHelper.Instance.Error(message);
                            ActulizarEstadoEnviosRTF(da, "PE", item.Key, message, cuenta);

                        }
                        catch (Exception exx)
                        {
                            LoggingHelper.Instance.Error(exx.Message);
                            continue;
                        }

                    }
                    continue;
                }

            }


            return result;
        }

        //Metodo provisorio para probar envio RTF por cuentas especificas
        [NonTransactionInterceptor]
        public virtual MotorResponse EnvioRTFPorCuentasFaxMail(RequestSecurity<WorkflowRTFReq> entity)
        {

            MotorResponse result = new MotorResponse();
            result.Exitoso = true;
            result.Descripcion = $"Se corrio correctamente la generacion de RTF al {DateTime.Now}.";

            var cuentasActivas = entity.Datos.CuentasActivas;

            var cuentasPorNup = new Dictionary<string, List<long>>();

            var da = DependencyFactory.Resolve<IMotorServicioDataAccess>();

            var pathParameter = da.ObtenerParametro("RTF_PATH");

            if (entity.Datos.RequiereNup == "S")
            {
                foreach (var cliente in cuentasActivas)
                {
                    if (cuentasPorNup.ContainsKey(cliente.Nup))
                    {
                        cuentasPorNup[cliente.Nup].Add(cliente.CuentaTitulo);

                    }
                    else
                    {
                        var listaCuentas = new List<long>();
                        listaCuentas.Add(cliente.CuentaTitulo);
                        cuentasPorNup.Add(cliente.Nup, listaCuentas);
                    }
                }

            }
            else
            {
                CompletarCuentasPorNupRTF(cuentasPorNup, entity.Canal, entity.SubCanal, cuentasActivas);
            }

            foreach (var item in cuentasPorNup)
            {
                var pathPdfGenerados = new List<string>();

                try
                {
                    var listaFondos = ObtenerTenenciaFondos(entity, item);

                    if (listaFondos == null) continue;

                    var nombreCliente = GetNombreCliente(item.Key, entity.Canal, entity.SubCanal);

                    var direccion = GetDireccionPorNup(item.Key);

                    foreach (var cuentaFondo in listaFondos.AgrupadoPorCuenta)
                    {
                        if (ValidarSiTieneCuentas(cuentaFondo))
                        {
                            var pdf = new RTFGeneracionPdF();
                            var pathArchivo = pdf.CreacionPdf(cuentaFondo, item.Key, nombreCliente, pathParameter, direccion, listaFondos.Legales, GetNombresClientes(cuentaFondo.CtaTitulo.ToString(), entity.Canal, entity.SubCanal));

                            VolcadoDatosRTF(pathArchivo, cuentaFondo, item.Key, cuentaFondo.Periodo, entity.Datos.Batch);
                        }

                    }
                }
                catch (Exception ex)
                {
                    foreach (var cuenta in item.Value)
                    {
                        try
                        {
                            var message = ex.Message;
                            message = ex.Message.Length > 300 ? ex.Message.Substring(0, 300) : ex.Message;

                            LoggingHelper.Instance.Error(message);
                            ActulizarEstadoEnviosRTF(da, "PE", item.Key, message, cuenta);

                        }
                        catch (Exception exx)
                        {
                            LoggingHelper.Instance.Error(exx.Message);
                            continue;
                        }

                    }
                    continue;
                }


            }

            return result;
        }

        public virtual List<AdhesionesMEPResp> ObtenerAdhesionesMEP(RequestSecurity<AdhesionesMEPReq> entity)
        {
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();

            var adhesiones = da.ObtenerAdhesionesMEP(entity.Datos);

            return adhesiones;
        }

        public virtual AdhesionMEPResp ObtenerAdhesionMEP(RequestSecurity<AdhesionesMEPReq> entity)
        {
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();

            var adhesion = da.ObtenerAdhesionMEP(entity.Datos);

            CompletarDatosClienteEnAdhesionMEP(adhesion, entity.Canal, entity.SubCanal);

            return adhesion;
        }

        private void CompletarDatosClienteEnAdhesionMEP(AdhesionMEPResp adhesion, string canal, string subCanal)
        {
            if (adhesion == null)
                return;

            var cliente = GetCliente(adhesion.Nup, canal, subCanal);

            if (cliente != null)
            {
                adhesion.FechaNacimiento = cliente.FechaNacimiento.ToString("yyyyMMdd");
                adhesion.NumeroDocumento = cliente.NumeroDocumento;

                if (!string.IsNullOrWhiteSpace(cliente.Apellido))
                    adhesion.Apellido = cliente.Apellido.Trim();

                if (!string.IsNullOrWhiteSpace(cliente.Nombre))
                    adhesion.Nombre = cliente.Nombre.Trim();
            }
        }

        private void CompletarDatosClienteEnAdhesionMEP(AdhesionesMEPCompraResp adhesion, string canal, string subCanal)
        {
            if (adhesion == null)
                return;

            var cliente = GetCliente(adhesion.Nup, canal, subCanal);

            if (cliente != null)
            {
                adhesion.FechaNacimiento = cliente.FechaNacimiento.ToString("yyyyMMdd");
                adhesion.NumeroDocumento = cliente.NumeroDocumento;
            }
        }

        private ClienteDDC GetCliente(string nup, string canal, string subCanal)
        {
            var reqCuentasD = new RequestSecurity<GetDatosCliente>();

            var datosClienteD = new GetDatosCliente
            {
                DatoConsulta = nup,
                TipoBusqueda = "N",
                CuentasRespuesta = "TO",
                Ip = "1.1.1.1",
                Usuario = "A999999",
                Canal = canal,
                SubCanal = subCanal
            };

            reqCuentasD.Datos = datosClienteD;

            var infoCliente = DependencyFactory.Resolve<IExternalWebApiService>().GetDatosCliente(reqCuentasD)?.FirstOrDefault();

            return infoCliente;
        }

        public virtual string ActualizarAdhesionMEPCompra(RequestSecurity<AdhesionMEPCompraReq> entity)
        {
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();

            da.ActualizarAdhesionMEPCompra(entity.Datos);

            return "OK";
        }

        public virtual string ActualizarAdhesionMEPError(RequestSecurity<AdhesionMEPErrorReq> entity)
        {
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();

            da.ActualizarAdhesionMEPError(entity.Datos);

            return "OK";
        }

        public virtual string ActualizarAdhesionMEPVenta(RequestSecurity<AdhesionMEPVentaReq> entity)
        {
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();

            da.ActualizarAdhesionMEPVenta(entity.Datos);

            return "OK";
        }

        public virtual List<AdhesionesMEPCompraResp> ObtenerAdhesionesMEPCompra(RequestSecurity<AdhesionesMEPReq> entity)
        {
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();

            var adhesiones = da.ObtenerAdhesionesMEPCompra(entity.Datos);

            return adhesiones;
        }

        public virtual AdhesionesMEPCompraResp ObtenerAdhesionMEPCompra(RequestSecurity<AdhesionesMEPReq> entity)
        {
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();

            var adhesion = da.ObtenerAdhesionMEPCompra(entity.Datos);


            CompletarDatosClienteEnAdhesionMEP(adhesion, entity.Canal, entity.SubCanal);

            return adhesion;
        }

        public virtual List<OrdenesMapsResp> ObtenerOrdenesMaps(RequestSecurity<OrdenesMapsReq> entity)
        {
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();

            var ordenes = da.ObtenerOrdenesMaps(entity.Datos);

            return ordenes;
        }

        public virtual string ActualizarOrden(RequestSecurity<ActualizarOrdenReq> entity)
        {
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();

            da.ActualizarOrden(entity.Datos);

            return "OK";
        }

        public virtual List<AdhesionesMEPResp> ObtenerAdhesionesMEPInversa(RequestSecurity<AdhesionesMEPReq> entity)
        {
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();

            var adhesiones = da.ObtenerAdhesionesMEPInversa(entity.Datos);

            //Cargar fechaNac y nroDoc
            if (adhesiones != null && adhesiones.Count > 0)
            {
                for (int i = 0; i < adhesiones.Count; i++)
                {
                    var listaCuentas = new List<Cuenta>();

                    var reqCuentasD = new RequestSecurity<GetDatosCliente>();

                    var datosClienteD = new GetDatosCliente
                    {
                        DatoConsulta = adhesiones[i].Nup,
                        TipoBusqueda = "N",
                        CuentasRespuesta = "TO",
                        Ip = "1.1.1.1",
                        Usuario = "A999999",
                        Canal = entity.Canal,
                        SubCanal = entity.SubCanal
                    };

                    reqCuentasD.Datos = datosClienteD;

                    var infoCliente = DependencyFactory.Resolve<IExternalWebApiService>().GetDatosCliente(reqCuentasD)?.FirstOrDefault();

                    if (infoCliente != null)
                    {
                        adhesiones[i].FechaNacimiento = infoCliente.FechaNacimiento.ToString("yyyyMMdd");
                        adhesiones[i].NumeroDocumento = infoCliente.NumeroDocumento;

                        if (!string.IsNullOrWhiteSpace(infoCliente.Apellido))
                            adhesiones[i].Apellido = infoCliente.Apellido.Trim();

                        if (!string.IsNullOrWhiteSpace(infoCliente.Nombre))
                            adhesiones[i].Nombre = infoCliente.Nombre.Trim();
                    }
                }
            }
            return adhesiones;
        }

        public virtual List<AdhesionesMEPCompraResp> ObtenerAdhesionesMEPInversaCompra(RequestSecurity<AdhesionesMEPReq> entity)
        {
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();

            var adhesiones = da.ObtenerAdhesionesMEPInversaCompra(entity.Datos);

            return adhesiones;
        }

        public virtual string ActualizarTarjeta(RequestSecurity<ActualizarTarjetaReq> entity)
        {
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();

            da.ActualizarTarjeta(entity.Datos);

            return "OK";
        }

        public virtual string InsertarDisclaimerEriMEPCompra(RequestSecurity<InsertarDisclaimerEriMEPCompraReq> entity)
        {
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();

            da.InsertarDisclaimerEriMEPCompra(entity.Datos);

            return "OK";
        }

        [NonTransactionInterceptor]
        public virtual MotorResponse RealizarEnvioMepMya(RequestSecurity<EnvioMepMYAReq> entity)
        {
            return RealizarEnvioMepMyaNet(entity);
        }
        #endregion

        private MotorResponse RealizarEnvioMepMyaNet(RequestSecurity<EnvioMepMYAReq> entity)
        {
            MotorResponse result = new MotorResponse
            {
                Exitoso = true,
                Descripcion = $"Se corrio los envios de mails al {entity.Datos.Fecha}."
            };

            var mails = ObtenerMailsMepAEnviar(new ObtenerMensajesMepReq { Fecha = entity.Datos.Fecha.Date });

            foreach (var mail in mails)
            {
                var texto = ObtenerTextoMEP(mail);

                try
                {
                    var mensaje = new MensajeMyA
                    {
                        HEADER = new Header()
                        {
                            RIONUP = mail.Nup, //(mail.Segmento == TipoSegmento.BancaPrivada ? string.Empty : mail.Nup),
                            RIODNICUIT = mail.Dni,
                            ENTITLEMENT = "Rio/Fondos_Servicio", //ObtenerEntitlement(mail.CodEstadoProceso),
                            LABEL = texto.Asunto,
                            TIMESTAMP = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                            TTL = "086400",
                            LEGALTYPE = "Individuo",
                            DESTINATION = string.Empty //(mail.Segmento == TipoSegmento.BancaPrivada ? mail.Destination : string.Empty)
                        },

                        BODY = new Body()
                        {
                            APELLIDO = mail.Apellido,
                            FECHA_ACTUAL = new Fecha()
                            {
                                ANO = DateTime.Now.Year.ToString(),
                                DIA = DateTime.Now.Day.ToString(),
                                HORA = DateTime.Now.ToShortTimeString(),
                                MES = DateTime.Now.Month.ToString()
                            },
                            NOMBRE = mail.Nombre,
                            TIPOMENSAJE = texto.TipoMensaje,
                            CANAL_BAJA = mail.Canal,
                            CUENTA_DEBITO = mail.CuentaOperativa.ToString(),
                            CUENTA_TITULOS = mail.CuentaTitulo.ToString(),
                            FECHA = DateTime.Now.ToString(),
                            //FONDO = "GD30",
                            NRO_COMPROBANTE = mail.ComprobanteCompra,
                            TEXTO = texto.TextoMensaje,
                            TITULO = texto.TituloSolapa,
                            DESCRIPCION = texto.Descripcion
                        }
                    };

                    var colaMq = new ColaMQ();
                    colaMq.DejarNovedad(mensaje);

                    Thread.Sleep(BusinessHelper.ObtenerTiempoDeEsperaMilSec());
                    ActualizarMensajeMep(mail.IdAhesion, "EN");
                }
                catch (Exception ex)
                {
                    LoggingHelper.Instance.Error(ex, ex.Message);
                    ActualizarMensajeMep(mail.IdAhesion, "PD");
                }
            }

            return result;
        }

        private Texto ObtenerTextoMEP(MensajeMEP mail)
        {
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();

            return da.ObtenerTextoMep(new ObtenerTextoMepReq
            {
                IdAdhesion = mail.IdAhesion
            });
        }

        private void ActualizarMensajeMep(long idAdhesion, string estado)
        {
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();
            da.ActualizarMensajeMep(new ActualizarMensajeMepReq { IdAdhesiones = idAdhesion, Estado = estado });
        }

        [NonTransactionInterceptor]
        private MotorResponse RealizarEnvioMepMyaDB(RequestSecurity<EnvioMepMYAReq> entity)
        {
            MotorResponse result = new MotorResponse();
            result.Exitoso = true;
            result.Descripcion = $"Se corrio los envios de mails al {DateTime.Now}.";

            try
            {
                #region DatosCliente
                /*if (!string.IsNullOrWhiteSpace(informacionPorNup))
                {
                    EnviarMail(informacionPorNup, entity);

                }*/
                #endregion
            }
            catch (Exception ex)
            {
                result.Exitoso = false;
                result.Descripcion += ex.Message;
                result.ResultadoServicio = "Inconvenientes con MQ. Revisar mails.";
            }

            return result;
        }

        private List<MensajeMEP> ObtenerMailsMepAEnviar(ObtenerMensajesMepReq entity)
        {
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();
            return da.ObtenerMailsMepAEnviar(entity);
        }

        private void CompletarCuentasPorNupRTF(Dictionary<string, List<long>> cuentasPorNup, string canal, string subcanal, List<CuentasActivasRTF> cuentasActivas)
        {
            foreach (var item in cuentasActivas)
            {
                var reqCuentas = new RequestSecurity<GetTitulares>();

                var datosCliente = new GetTitulares
                {
                    CuentaTitulos = item.CuentaTitulo.ToString()
                };

                reqCuentas.Datos = datosCliente;

                var documentos = DependencyFactory.Resolve<IExternalWebApiService>().GetTitulares(reqCuentas);

                if (documentos != null && documentos.Count() > 0)
                {
                    var documentosList = documentos.ToList();

                    foreach (var doc in documentosList)
                    {
                        var reqCuentasD = new RequestSecurity<GetDatosCliente>();

                        var datosClienteD = new GetDatosCliente
                        {
                            DatoConsulta = doc.NroDocumento,
                            Segmento = "RTL",
                            CuentasRespuesta = "TI",
                            Ip = "1.1.1.1",
                            Usuario = "B049684",
                            Canal = canal,
                            SubCanal = subcanal
                        };

                        reqCuentasD.Datos = datosClienteD;

                        var clientes = DependencyFactory.Resolve<IExternalWebApiService>().GetDatosClientePorDocumento(reqCuentasD);

                        if (clientes != null && clientes.Count() > 0)
                        {
                            var cliente = clientes.FirstOrDefault();

                            foreach (var cli in clientes)
                            {
                                if (cli.Cuentas.Count() > 0)
                                {
                                    var cuentaAsignar = cli.Cuentas.FirstOrDefault(c => c.NroCta.PadLeft(12, '0') == item.CuentaTitulo.ToString().PadLeft(12, '0'));

                                    if (cuentaAsignar != null)
                                        cliente = cli;
                                }
                            }

                            if (cuentasPorNup.ContainsKey(cliente.Nup))
                            {
                                cuentasPorNup[cliente.Nup].Add(item.CuentaTitulo);
                            }
                            else
                            {
                                var listaCuentas = new List<long>();
                                listaCuentas.Add(item.CuentaTitulo);
                                cuentasPorNup.Add(cliente.Nup, listaCuentas);
                            }

                        }

                    }
                }

            }

        }

        private bool ValidarSiTieneCuentas(CuentaFondoRTF cuentaFondo)
        {
            return cuentaFondo.ListaEspeciesDolares != null && cuentaFondo.ListaEspeciesDolares.Count() > 0
               || cuentaFondo.ListaEspeciesPesos != null && cuentaFondo.ListaEspeciesPesos.Count() > 0
               || cuentaFondo.ListaEspeciesMovimientosDolares != null && cuentaFondo.ListaEspeciesMovimientosDolares.Count() > 0
               || cuentaFondo.ListaEspeciesMovimientosPesos != null && cuentaFondo.ListaEspeciesMovimientosPesos.Count() > 0;



        }

        private void ActulizarEstadoEnviosRTF(IMotorServicioDataAccess da, string estado, string nup, string message, long cuenta)
        {
            da.ActualizarMensajeRTF(new ActualizarMensajeRTF { Nup = nup, Cuenta = cuenta, Estado = estado, Observacion = message });
        }

        private void VolcadoDatosRTF(string pathPdf, CuentaFondoRTF cuenta, string Nup, Periodo periodo, string Batch)
        {
            if (string.IsNullOrEmpty(Batch))
                Batch = "N";

            var archivo = new ArchivoRTFReq
            {
                Path = pathPdf,
                Nombre = ObtenerNombreResumen(periodo),
                Cuenta = cuenta.CtaTitulo.ToString(),
                Descripcion = ObtenerDescripcionResumen(periodo),
                Nup = Nup,
                Anio = periodo.FechaInicio.Year,
                Batch = Batch
            };

            DependencyFactory.Resolve<IMotorServicioDataAccess>().CargarArchivoRTF(archivo);
        }

        private string ObtenerNombreResumen(Periodo periodo)
        {
            var nombre = "";

            switch (periodo.FechaInicio.Date.Month)
            {
                case 1:
                    nombre = "ENE-MAR";
                    break;
                case 4:
                    nombre = "ABR-JUN";
                    break;
                case 7:
                    nombre = "JUL-SEPT";
                    break;
                case 10:
                    nombre = "OCT-DIC";
                    break;
            }

            return string.Format("ResumenFondos.{0}-{1}.pdf", nombre, periodo.FechaInicio.Year);
        }

        private string ObtenerDescripcionResumen(Periodo periodo)
        {
            var nombre = "";

            switch (periodo.FechaInicio.Date.Month)
            {
                case 1:
                    nombre = "ENERO MARZO";
                    break;
                case 4:
                    nombre = "ABRIL JUNIO";
                    break;
                case 7:
                    nombre = "JULIO SEPTIEMBRE";
                    break;
                case 10:
                    nombre = "OCTUBRE DICIEMBRE";
                    break;
            }

            return nombre;
        }


        private TenenciaValuadaFondosRTFResponse ObtenerTenenciaFondos(RequestSecurity<WorkflowRTFReq> entity, KeyValuePair<string, List<long>> item)
        {
            var request = new RequestSecurity<TenenciaValuadaFondosRTFRequest>();

            var subcanal = entity.SubCanal.Substring(2, 2);

            var datos = new TenenciaValuadaFondosRTFRequest
            {
                Nup = item.Key,
                Segmento = "RTL",
                ListaCuentas = item.Value,
                DatosServicios = new DatosServicios()
                {
                    CanalTipo = entity.Canal,
                    CanalId = "HTML",
                    CanalVersion = "000",
                    SubcanalTipo = subcanal,
                    SubcanalId = "0001",
                    UsuarioTipo = "I",
                    UsuarioId = "FREEUSER",
                    UsuarioAtrib = "AA",
                    UsuarioPwd = "FREEUSER",
                    TipoPersona = "I",
                    TipoId = "N",
                    IdCliente = "00010271006",
                    FechaNac = "19520527"
                },
                Ip = "1.1.1.1",
                Usuario = "B049684"
            };

            request.Datos = datos;

            return DependencyFactory.Resolve<IExternalWebApiService>().CallObtenerTenenciaValuadaFondos(request);
        }

        private List<CuentasActivasRTF> ObtenerCuentasActivasPorPeriodoRTF(RequestSecurity<WorkflowSAFReq> entity)
        {
            var request = new RequestSecurity<TenenciaValuadaFondosRTFRequest>();

            var subcanal = entity.SubCanal.Substring(2, 2);

            var datos = new TenenciaValuadaFondosRTFRequest
            {
                Segmento = "RTL",
                DatosServicios = new DatosServicios()
                {
                    CanalTipo = entity.Canal,
                    CanalId = "HTML",
                    CanalVersion = "000",
                    SubcanalTipo = subcanal,
                    SubcanalId = "0001",
                    UsuarioTipo = "I",
                    UsuarioId = "FREEUSER",
                    UsuarioAtrib = "AA",
                    UsuarioPwd = "FREEUSER",
                    TipoPersona = "I",
                    TipoId = "N",
                    IdCliente = "00010271006",
                    FechaNac = "19520527"
                },
                Ip = "1.1.1.1",
                Usuario = "B049684"
            };

            request.Datos = datos;

            return DependencyFactory.Resolve<IExternalWebApiService>().ObtenerCuentasActivasPorPeriodoRTF(request);
        }

        private TenenciaValuadaFondosRTFResponse ObtenerTenenciaFondos(RequestSecurity<WorkflowSAFReq> entity, KeyValuePair<string, List<long>> item)
        {
            var request = new RequestSecurity<TenenciaValuadaFondosRTFRequest>();

            var subcanal = entity.SubCanal.Substring(2, 2);

            var datos = new TenenciaValuadaFondosRTFRequest
            {
                Nup = item.Key,
                Segmento = "RTL",
                ListaCuentas = item.Value,
                DatosServicios = new DatosServicios()
                {
                    CanalTipo = entity.Canal,
                    CanalId = "HTML",
                    CanalVersion = "000",
                    SubcanalTipo = subcanal,
                    SubcanalId = "0001",
                    UsuarioTipo = "I",
                    UsuarioId = "FREEUSER",
                    UsuarioAtrib = "AA",
                    UsuarioPwd = "FREEUSER",
                    TipoPersona = "I",
                    TipoId = "N",
                    IdCliente = "00010271006",
                    FechaNac = "19520527"
                },
                Ip = "1.1.1.1",
                Usuario = "B049684"
            };

            request.Datos = datos;

            return DependencyFactory.Resolve<IExternalWebApiService>().CallObtenerTenenciaValuadaFondos(request);
        }

        private string GetNombreCliente(string nup, string canal, string subcanal)
        {
            var reqCuentas = new RequestSecurity<GetDatosCliente>();

            var datosCliente = new GetDatosCliente
            {
                DatoConsulta = nup,
                Segmento = "RTL",
                TipoBusqueda = "N",
                CuentasRespuesta = "TI",
                Ip = "1.1.1.1",
                Usuario = "B049684",
                Canal = canal,
                SubCanal = subcanal
            };

            reqCuentas.Datos = datosCliente;

            var ddcCuentasResult = DependencyFactory.Resolve<IExternalWebApiService>().GetDatosCliente(reqCuentas);

            if (ddcCuentasResult == null || ddcCuentasResult != null && ddcCuentasResult.Count() == 0) return string.Empty;

            return string.Format("{0} {1}", ddcCuentasResult.FirstOrDefault()?.Nombre.Trim(), ddcCuentasResult.FirstOrDefault()?.Apellido.Trim());
        }

        private string GetNombresClientes(string cuenta, string canal, string subcanal)
        {
            var reqCuentas = new RequestSecurity<GetTitulares>();

            var datosCliente = new GetTitulares
            {
                CuentaTitulos = cuenta
            };

            reqCuentas.Datos = datosCliente;

            var cuentas = DependencyFactory.Resolve<IExternalWebApiService>().GetTitulares(reqCuentas);

            if (cuentas == null || cuentas != null && cuentas.Count() == 0) return string.Empty;

            var nombres = new List<string>();

            foreach (var item in cuentas.ToList())
            {
                nombres.Add(string.Format("{0} {1} {2}", item.Nombre?.Trim(), item.PrimerApellido?.Trim(), item.SegundoApellido?.Trim()));
            }
            return string.Join(", ", nombres);
        }

        private string GetDireccionPorNup(string nup)
        {
            var direccionResponse = DependencyFactory.Resolve<IOpicsDA>().ObtenerDireccion(nup);

            if (direccionResponse == null) return string.Empty;

            var builder = new StringBuilder();

            builder.Append(string.Format("{0} {1}", !string.IsNullOrEmpty(direccionResponse.Calle) ? direccionResponse.Calle.Trim() : direccionResponse.Calle,
               !string.IsNullOrEmpty(direccionResponse.Numero) ? direccionResponse.Numero.Trim() : direccionResponse.Numero));

            if (!string.IsNullOrEmpty(direccionResponse.Piso) && !string.IsNullOrWhiteSpace(direccionResponse.Piso))
                builder.Append(string.Format(" - Piso {0}", direccionResponse.Piso.Trim()));

            if (!string.IsNullOrEmpty(direccionResponse.Depto) && !string.IsNullOrWhiteSpace(direccionResponse.Depto))
                builder.Append(string.Format(" DPTO {0}", direccionResponse.Depto.Trim()));

            builder.Append(string.Format(Environment.NewLine + "{0} - {1}", !string.IsNullOrEmpty(direccionResponse.CodigoPostal) ? direccionResponse.CodigoPostal.Trim() : direccionResponse.CodigoPostal,
              !string.IsNullOrEmpty(direccionResponse.Localidad) ? direccionResponse.Localidad.Trim() : direccionResponse.Localidad));


            return builder.ToString();
        }

        private FaxMailResponse GeneracionEnvioPdfPorFaxMail(string mail, List<string> pathPdfGenerados, string imgBodyParameter)
        {

            FaxMailResponse resultFaxMail = new FaxMailResponse();
            resultFaxMail.Exitoso = true;

            try
            {
                var tiposAdjuntos = new List<string>();
                var nombresAdjuntos = new List<string>();

                for (int i = 0; i < pathPdfGenerados.Count; i++)
                {
                    tiposAdjuntos.Add("pdf");
                    nombresAdjuntos.Add(string.Format("ResumenTrimestralFondos-{0}.pdf", DateTime.Now.ToString("d-M-yyyy")));
                }

                var faxMailParameter = new FaxMailParameter()
                {
                    Mensaje = string.Format("<![CDATA[<table><tbody><tr><td><img src=\"{0}\"></td></tr><tr><td></td></tr></tbody></table>]]> ", imgBodyParameter),
                    Tipo = "MAIL",
                    Usuario = "EXTR",
                    Emisor = "informe@santander.com.ar",
                    Receptores = mail,
                    Copia = string.Empty,
                    CopiaOculta = string.Empty,
                    ResponderA = string.Empty,
                    Asunto = "Resumen trimestral de Fondos Comunes de Inversion",
                    EsHtml = true,
                    NombreAdjunto = string.Join(",", nombresAdjuntos),
                    TipoAdjunto = string.Join(",", tiposAdjuntos),
                    Adjuntos = pathPdfGenerados,
                    AcuseRecibo = true,
                    AcuseLectura = true,
                    Prioridad = "normal"
                };

                var faxMail = new EnvioFaxMailService();

                return faxMail.EnvioMailServidor(faxMailParameter);
            }
            catch (Exception ex)
            {
                resultFaxMail.Exitoso = false;
                resultFaxMail.Descripcion = ex.Message;

                return resultFaxMail;
            }

        }

        public string DireccionBase64String(string direccionString)
        {
            var BinaryFile = File.ReadAllBytes(direccionString);
            return Convert.ToBase64String(BinaryFile);
        }

        private Dictionary<string, List<long>> ObtenerCuentasActivasRTFPorNuP(RequestSecurity<WorkflowSAFReq> entity)
        {
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();

            var cuentasActivas = ObtenerCuentasActivasPorPeriodoRTF(entity);

            var cuentasPorNup = new Dictionary<string, List<long>>();

            CompletarCuentasPorNupRTF(cuentasPorNup, entity.Canal, entity.SubCanal, cuentasActivas);

            return cuentasPorNup;
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

        private string RemplazarTextoACT(string texto, Mensaje mails)
        {
            StringBuilder s = new StringBuilder(texto);
            var cuentaTitulo = mails.CuentaTitulo.ToString();


            if (cuentaTitulo.Length > 1)
                cuentaTitulo = cuentaTitulo.Insert(cuentaTitulo.Length - 1, "/");

            s.Replace("@fondo", mails.DescripcionFondo);
            s.Replace("@cuentaTitulo", cuentaTitulo);
            s.Replace("@cuentaOperativa", mails.CuentaOperativa.ToString());
            s.Replace("@comprobante", mails.NumeroComprobante.ToString());
            s.Replace("@fecha", DateTime.Now.ToString());
            s.Replace("@solicitante", $"{mails.ApellidoCliente}, {mails.NombreCliente}");
            s.Replace("@canalBaja", mails.Canal);

            return s.ToString();
        }

        public virtual MotorResponse EjecutarWorkflowRepatriacion(RequestSecurity<WorkflowCTRReq> entity)
        {
            try
            {

                IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();

                var ctaOperativasAdheridos = da.ObtenerCuentasRepatriacion(new ObtenerCuentasRepaReq { IdServicio = entity.Datos.IdServicio });

                foreach (var cuentaOperativa in ctaOperativasAdheridos)
                {

                    if (cuentaOperativa.CuentaTitulo == null || cuentaOperativa.CuentaTitulo == 1)
                    {
                        var cuentaGenerada = CreacionCuentaTitulosRepatriacion(entity, cuentaOperativa.Nup, "USD");

                        if (!string.IsNullOrEmpty(cuentaGenerada))
                        {
                            cuentaOperativa.CuentaTitulo = long.Parse(cuentaGenerada);

                            da.ActualizarAdhesionRepatriacion(new ActualizarAdhesionRepatriacionReq { CuentaTitulo = cuentaOperativa.CuentaTitulo, CodAltAdAhesion = cuentaOperativa.CodAltaAdhesion ?? 0 });

                            AsociarCotitularesRepatriacion(cuentaOperativa, entity);

                            CrearRelacionCuentas(cuentaGenerada, entity, cuentaOperativa);
                        }
                        else
                        {

                            da.CargarCuentasParticipantes(new CargarCuentasParticipantesReq
                            {
                                CuentaTitulo = cuentaOperativa.CuentaTitulo,
                                CtaOperativa = cuentaOperativa.CtaOperativa,
                                SucCtaOperativa = cuentaOperativa.SucursalOperativa,
                                TipoCtaOperativa = cuentaOperativa.TipoOperativa,
                                CodAltAdAhesion = cuentaOperativa.CodAltaAdhesion ?? 0,
                                CantParticipantes = 0,
                                Procesado = "N"
                            });
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                LoggingHelper.Instance.Error(ex, $"Error flujo Repatriacion:{ex.Message}", "MotorBusiness", "EjecutarWorkflowRepatriacion");

            }

            return new MotorResponse();
        }

        public virtual MotorResponse EjecutarWorkflowAltaCuentaTitulos(RequestSecurity<WorkflowCTRReq> entity)
        {
            try
            {

                entity.Datos.IdServicio = "ACT";

                IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();

                var ctaOperativasAdheridos = da.ObtenerCuentasRepatriacion(new ObtenerCuentasRepaReq { IdServicio = entity.Datos.IdServicio });

                foreach (var cuentaOperativa in ctaOperativasAdheridos)
                {

                    if (cuentaOperativa.CuentaTitulo == null || cuentaOperativa.CuentaTitulo == 1)
                    {
                        var cuentaGenerada = CreacionCuentaTitulosRepatriacion(entity, cuentaOperativa.Nup, "ARS");

                        if (!string.IsNullOrEmpty(cuentaGenerada))
                        {
                            cuentaOperativa.CuentaTitulo = long.Parse(cuentaGenerada);

                            da.ActualizarAdhesionRepatriacion(new ActualizarAdhesionRepatriacionReq { CuentaTitulo = cuentaOperativa.CuentaTitulo, CodAltAdAhesion = cuentaOperativa.CodAltaAdhesion ?? 0 });

                            ActualizarCuentasDDC(cuentaGenerada, entity, cuentaOperativa);

                            da.CargarCuentasParticipantes(new CargarCuentasParticipantesReq
                            {
                                CuentaTitulo = cuentaOperativa.CuentaTitulo,
                                CtaOperativa = cuentaOperativa.CtaOperativa,
                                SucCtaOperativa = cuentaOperativa.SucursalOperativa,
                                TipoCtaOperativa = cuentaOperativa.TipoOperativa,
                                CodAltAdAhesion = cuentaOperativa.CodAltaAdhesion ?? 0,
                                CantParticipantes = 1,
                                Procesado = "S",
                                RelacionCtasOperativas = "N"
                            });

                        }
                        else
                        {

                            da.CargarCuentasParticipantes(new CargarCuentasParticipantesReq
                            {
                                CuentaTitulo = cuentaOperativa.CuentaTitulo,
                                CtaOperativa = cuentaOperativa.CtaOperativa,
                                SucCtaOperativa = cuentaOperativa.SucursalOperativa,
                                TipoCtaOperativa = cuentaOperativa.TipoOperativa,
                                CodAltAdAhesion = cuentaOperativa.CodAltaAdhesion ?? 0,
                                CantParticipantes = 0,
                                Procesado = "N",
                                RelacionCtasOperativas = "N"
                            });
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LoggingHelper.Instance.Error(ex, $"Error flujo Alta Cuenta Titulos:{ex.Message}", "MotorBusiness", "EjecutarWorkflowAltaCuentaTitulos");
            }

            return new MotorResponse();
        }

        public virtual MotorResponse TestServicioIN(RequestSecurity<WorkflowCTRReq> entity)
        {
            ICanalesIatxDA daIATX = DependencyFactory.Resolve<ICanalesIatxDA>();
            //GeneracionCuentaResponse datosCuentaIATXResponse = null;
            var usuarioRacf = DependencyFactory.Resolve<IMotorServicioDataAccess>().ObtenerUsuarioRacf();

            try
            {
                LoggingHelper.Instance.Information(usuarioRacf.Usuario);
                LoggingHelper.Instance.Information(usuarioRacf.Password);

                var cabecera = GenerarCabeceraACT(entity.Canal, entity.SubCanal);
                cabecera.H_UsuarioID = entity.Datos.DatosCliente;
                cabecera.H_UsuarioPwd = entity.Datos.OrdenCliente;

                var datosCuentaIATXResponse = daIATX.ServicioINIATX(cabecera, entity.Datos.Nup);

                //if (!string.IsNullOrEmpty(datosCuentaIATXResponse.Código_retorno_extendido))
                //    daIATX.AltaCuentaIATX(cabecera, nup, datosCuentaIATXResponse.Código_retorno_extendido, codigoMoneda);

                return new MotorResponse();

            }
            catch (Exception ex)
            {
                LoggingHelper.Instance.Error(ex, $"Error al generar la cuenta titulos:{ex.Message}", "MotorBusiness", "CreacionCuentaTitulosRepatriacion");

                return new MotorResponse() { Descripcion = ex.Message };
            }
        }

        public virtual MotorResponse EjecutarEnvioMailAltaCuentaTitulos(RequestSecurity<WorkflowSAFReq> entity)
        {

            var solicitudOrden = entity.MapperClass<RequestSecurity<SolicitudOrden>>(TypeMapper.IgnoreCaseSensitive);
            solicitudOrden.Datos = entity.Datos.MapperClass<SolicitudOrden>(TypeMapper.IgnoreCaseSensitive);
            solicitudOrden.Datos.Solicitudes = ConsultaAdhesionesRTFActivas(entity);
            solicitudOrden.Dato = ConfigurationManager.AppSettings["DATO"];
            solicitudOrden.Firma = ConfigurationManager.AppSettings["FIRMA"];

            EnvioMailAdhesionesPendientes(solicitudOrden);

            return new MotorResponse();
        }

        public virtual MotorResponse EjecutarRelacionarCuentasOperativas(RequestSecurity<WorkflowCTRReq> entity)
        {
            try
            {
                IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();

                var ctaOperativasAdheridos = da.ObtenerCuentasRepatriacion(new ObtenerCuentasRepaReq { IdServicio = "ACT" });

                foreach (var cuentaOperativa in ctaOperativasAdheridos)
                {
                    if (cuentaOperativa.CuentaTitulo != null && cuentaOperativa.CuentaTitulo != 1)
                    {
                        var relacionIATX = AsociarCuentasOperativas(entity, cuentaOperativa.Nup, cuentaOperativa.CuentaTitulo.ToString().PadLeft(12, '0'), cuentaOperativa);

                        if (!string.IsNullOrEmpty(relacionIATX))
                        {
                            da.CargarCuentasParticipantes(new CargarCuentasParticipantesReq
                            {
                                CuentaTitulo = cuentaOperativa.CuentaTitulo,
                                CtaOperativa = cuentaOperativa.CtaOperativa,
                                SucCtaOperativa = cuentaOperativa.SucursalOperativa,
                                TipoCtaOperativa = cuentaOperativa.TipoOperativa,
                                CodAltAdAhesion = cuentaOperativa.CodAltaAdhesion ?? 0,
                                Procesado = "S",
                                RelacionCtasOperativas = "S"
                            });
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                LoggingHelper.Instance.Error(ex, $"Error al asociar cuentas operativas:{ex.Message}", "MotorBusiness", "EjecutarRelacionarCuentasOperativas");
            }

            return new MotorResponse();
        }

        public virtual MotorResponse RealizarAltaCuentasCTRepatriacion(RequestSecurity<WorkflowCTRReq> entity)
        {
            ICanalesIatxDA daIATX = DependencyFactory.Resolve<ICanalesIatxDA>();
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();
            var usuarioRacf = DependencyFactory.Resolve<IMotorServicioDataAccess>().ObtenerUsuarioRacf();

            var cabecera = GenerarCabecera(entity.Canal, entity.SubCanal);
            cabecera.H_UsuarioID = usuarioRacf.Usuario;
            cabecera.H_UsuarioPwd = usuarioRacf.Password;

            var cuentasRepa = da.ObtenerCuentasRepatriacion(new ObtenerCuentasRepaReq { IdServicio = entity.Datos.IdServicio });

            foreach (var cuentaRepatriacion in cuentasRepa)
            {

                if (cuentaRepatriacion.CuentaTitulo == null) continue;

                var procesado = false;

                var reqCuentas = new RequestSecurity<GetTitulares>();

                var datosCliente = new GetTitulares
                {
                    NroCtaOperativa = cuentaRepatriacion.CtaOperativa?.ToString(),
                    SucursalCtaOperativa = cuentaRepatriacion.SucursalOperativa?.ToString(),
                    TipoCtaOperativa = cuentaRepatriacion.TipoOperativa?.ToString()
                };

                reqCuentas.Datos = datosCliente;

                var documentos = DependencyFactory.Resolve<IExternalWebApiService>().GetTitulares(reqCuentas).ToList();

                var cantidadParticipantes = 0;
                var ordenParticipacion = 2;

                foreach (var doc in documentos)
                {
                    var reqCuentasD = new RequestSecurity<GetDatosCliente>();

                    var datosClienteD = new GetDatosCliente
                    {
                        DatoConsulta = doc.NroDocumento,
                        //Segmento = "RTL",
                        CuentasRespuesta = "TI",
                        Ip = "1.1.1.1",
                        Usuario = "B049684",
                        Canal = entity.Canal,
                        SubCanal = entity.SubCanal
                    };

                    reqCuentasD.Datos = datosClienteD;

                    var clientes = DependencyFactory.Resolve<IExternalWebApiService>().GetDatosClientePorDocumento(reqCuentasD);

                    if (clientes != null && clientes.Count() > 0)
                    {
                        for (int i = 0; i < clientes.Count(); i++)
                        {
                            cantidadParticipantes++;

                            if (clientes[i].Nup != cuentaRepatriacion.Nup)
                            {
                                try
                                {
                                    daIATX.CMBRELCLICIATX(cabecera, clientes[i].Nup, cuentaRepatriacion.CuentaTitulo.ToString().PadLeft(12, '0'), ordenParticipacion.ToString());

                                    procesado = true;
                                    ordenParticipacion++;
                                }
                                catch (Exception ex)
                                {
                                    procesado = false;
                                    LoggingHelper.Instance.Error($"Error en el servicio CMBRELCLICIATX , Response: {ex.Message}", "MotorBusiness", "RealizarAltaCuentasCTRepatriacion");
                                    continue;

                                }

                            }
                        }
                    }
                }

                da.CargarCuentasParticipantes(new CargarCuentasParticipantesReq
                {
                    CuentaTitulo = cuentaRepatriacion.CuentaTitulo,
                    CtaOperativa = cuentaRepatriacion.CtaOperativa,
                    SucCtaOperativa = cuentaRepatriacion.SucursalOperativa,
                    TipoCtaOperativa = cuentaRepatriacion.TipoOperativa,
                    CodAltAdAhesion = cuentaRepatriacion.CodAltaAdhesion ?? 0,
                    CantParticipantes = cantidadParticipantes,
                    Procesado = procesado ? "S" : "N"
                });
            }

            return new MotorResponse();
        }

        public virtual MotorResponse ModificarRelacionClienteContrato(RequestSecurity<WorkflowCTRReq> entity)
        {
            ICanalesIatxDA daIATX = DependencyFactory.Resolve<ICanalesIatxDA>();
            var usuarioRacf = DependencyFactory.Resolve<IMotorServicioDataAccess>().ObtenerUsuarioRacf();

            var relacionCliente = entity.MapperClass<RelacionClienteContrato>(TypeMapper.IgnoreCaseSensitive);

            var cabecera = GenerarCabecera(entity.Canal, entity.SubCanal);
            cabecera.H_UsuarioID = usuarioRacf.Usuario;
            cabecera.H_UsuarioPwd = usuarioRacf.Password;


            daIATX.ModificarRelacionClienteContratoIATX(cabecera, relacionCliente);

            return new MotorResponse();
        }

        #region Métodos Privados
        private string CreacionCuentaTitulosRepatriacion(RequestSecurity<WorkflowCTRReq> entity, string nup, string codigoMoneda)
        {
            ICanalesIatxDA daIATX = DependencyFactory.Resolve<ICanalesIatxDA>();
            //GeneracionCuentaResponse datosCuentaIATXResponse = null;
            var usuarioRacf = DependencyFactory.Resolve<IMotorServicioDataAccess>().ObtenerUsuarioRacf();

            try
            {
                var cabecera = GenerarCabeceraACT(entity.Canal, entity.SubCanal);
                cabecera.H_UsuarioID = usuarioRacf.Usuario;
                cabecera.H_UsuarioPwd = usuarioRacf.Password;

                var datosCuentaIATXResponse = daIATX.GeneracionCuentaIATX(cabecera, nup);

                if (!string.IsNullOrEmpty(datosCuentaIATXResponse.Código_retorno_extendido))
                    daIATX.AltaCuentaIATX(cabecera, nup, datosCuentaIATXResponse.Código_retorno_extendido, codigoMoneda);

                return datosCuentaIATXResponse.Código_retorno_extendido;

            }
            catch (Exception ex)
            {
                LoggingHelper.Instance.Error(ex, $"Error al generar la cuenta titulos:{ex.Message}", "MotorBusiness", "CreacionCuentaTitulosRepatriacion");
                return null;
            }
        }

        private string AsociarCuentasOperativas(RequestSecurity<WorkflowCTRReq> entity, string nup, string numeroCuenta, ObtenerCuentasRepaResp cuentaOperativa)
        {
            ICanalesIatxDA daIATX = DependencyFactory.Resolve<ICanalesIatxDA>();
            //GeneracionCuentaResponse datosCuentaIATXResponse = null;
            var usuarioRacf = DependencyFactory.Resolve<IMotorServicioDataAccess>().ObtenerUsuarioRacf();

            try
            {

                var cabecera = GenerarCabecera(entity.Canal, entity.SubCanal);
                cabecera.H_UsuarioID = usuarioRacf.Usuario;
                cabecera.H_UsuarioPwd = usuarioRacf.Password;
                cabecera.H_Nup = nup;

                var datosCuentaIATXResponse = daIATX.AsociarCuentasOperativasIATX(cabecera, entity.Datos, ArmadoRequestRelacionCtaOperativa(cuentaOperativa, usuarioRacf.Usuario, numeroCuenta));

                IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();

                return string.IsNullOrEmpty(datosCuentaIATXResponse) ? "OK" : datosCuentaIATXResponse;

            }
            catch (Exception ex)
            {
                LoggingHelper.Instance.Error(ex, $"Error al asociar cuentas operativas:{ex.Message}", "MotorBusiness", "AsociarCuentasOperativas");
                return null;
            }
        }

        private AsociarCtaOperativaReq ArmadoRequestRelacionCtaOperativa(ObtenerCuentasRepaResp cuentaOperativa, string usuarioVerificacion, string cuentaTitulo)
        {
            var cuentaCorriente = "ACTE0000000000000000";
            var cajaAhorroPesos = "ACAH0000000000000000";
            var cuentaDolares = "ACCD0000000000000000";

            var sucursalPesos = cuentaOperativa.SucursalOperativa?.ToString().PadLeft(4, '0');
            var cuentaOpPesos = cuentaOperativa.CtaOperativa?.ToString().PadLeft(12, '0');

            var sucursalDolares = cuentaOperativa.SucursalOperativaDolares?.ToString().PadLeft(4, '0');
            var cuentaOpDolares = cuentaOperativa.CtaOperativaDolares?.ToString().PadLeft(12, '0');

            switch (cuentaOperativa.TipoOperativa)
            {
                case 0:
                    cuentaCorriente = string.Format("ACTE" + sucursalPesos + cuentaOpPesos);
                    break;

                case 9:
                    cajaAhorroPesos = string.Format("ACAH" + sucursalPesos + cuentaOpPesos);
                    break;

                case 10:
                    cuentaDolares = string.Format("ACCD" + sucursalDolares + cuentaOpDolares);
                    break;

                default:
                    cajaAhorroPesos = string.Format("ACAH" + sucursalPesos + cuentaOpPesos);
                    break;
            }

            if (cuentaOperativa.CtaOperativaDolares != null)
            {
                switch (cuentaOperativa.TipoOperativaDolares)
                {
                    case 0:
                        cuentaCorriente = string.Format("ACTE" + sucursalDolares + cuentaOpDolares);
                        break;

                    case 9:
                        cajaAhorroPesos = string.Format("ACAH" + sucursalPesos + cuentaOpPesos);
                        break;

                    case 10:
                        cuentaDolares = string.Format("ACCD" + sucursalDolares + cuentaOpDolares);
                        break;

                    default:
                        cajaAhorroPesos = string.Format("ACAH" + sucursalPesos + cuentaOpPesos);
                        break;
                }
            }

            return new AsociarCtaOperativaReq
            {
                UsuarioVerificacion = usuarioVerificacion,
                NumeroCuenta = cuentaTitulo,
                CuentaCorriente = cuentaCorriente,
                CajaAhorro = cajaAhorroPesos,
                CuentaDolares = cuentaDolares,
                Sucursal = "000"
            };
        }

        private void AsociarCotitularesRepatriacion(ObtenerCuentasRepaResp cuentaRepatriacion, RequestSecurity<WorkflowCTRReq> entity)
        {
            ICanalesIatxDA daIATX = DependencyFactory.Resolve<ICanalesIatxDA>();
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();
            var usuarioRacf = DependencyFactory.Resolve<IMotorServicioDataAccess>().ObtenerUsuarioRacf();

            var cabecera = GenerarCabecera(entity.Canal, entity.SubCanal);
            cabecera.H_UsuarioID = usuarioRacf.Usuario;
            cabecera.H_UsuarioPwd = usuarioRacf.Password;

            var reqCuentas = new RequestSecurity<GetTitulares>();

            var datosCliente = new GetTitulares
            {
                NroCtaOperativa = cuentaRepatriacion.CtaOperativa?.ToString(),
                SucursalCtaOperativa = cuentaRepatriacion.SucursalOperativa?.ToString(),
                TipoCtaOperativa = cuentaRepatriacion.TipoOperativa?.ToString()
            };

            reqCuentas.Datos = datosCliente;

            var documentos = DependencyFactory.Resolve<IExternalWebApiService>().GetTitulares(reqCuentas).ToList();

            var cantidadParticipantes = 0;
            var ordenParticipacion = 2;
            var procesado = true;

            foreach (var doc in documentos)
            {
                var reqCuentasD = new RequestSecurity<GetDatosCliente>();

                var datosClienteD = new GetDatosCliente
                {
                    DatoConsulta = doc.NroDocumento,
                    //Segmento = "RTL",
                    CuentasRespuesta = "TI",
                    Ip = "1.1.1.1",
                    Usuario = "B049684",
                    Canal = entity.Canal,
                    SubCanal = entity.SubCanal
                };

                reqCuentasD.Datos = datosClienteD;

                var clientes = DependencyFactory.Resolve<IExternalWebApiService>().GetDatosClientePorDocumento(reqCuentasD);

                if (clientes != null && clientes.Count() > 0)
                {
                    for (int i = 0; i < clientes.Count(); i++)
                    {
                        cantidadParticipantes++;

                        if (clientes[i].Nup != cuentaRepatriacion.Nup)
                        {
                            try
                            {
                                daIATX.CMBRELCLICIATX(cabecera, clientes[i].Nup, cuentaRepatriacion.CuentaTitulo.ToString().PadLeft(12, '0'), ordenParticipacion.ToString());

                                procesado = true;
                                ordenParticipacion++;
                            }
                            catch (Exception ex)
                            {
                                procesado = false;
                                LoggingHelper.Instance.Error($"Error en el servicio CMBRELCLICIATX , Response: {ex.Message}", "MotorBusiness", "RealizarAltaCuentasCTRepatriacion");
                                continue;

                            }

                        }
                    }
                }
            }

            da.CargarCuentasParticipantes(new CargarCuentasParticipantesReq
            {
                CuentaTitulo = cuentaRepatriacion.CuentaTitulo,
                CtaOperativa = cuentaRepatriacion.CtaOperativa,
                SucCtaOperativa = cuentaRepatriacion.SucursalOperativa,
                TipoCtaOperativa = cuentaRepatriacion.TipoOperativa,
                CodAltAdAhesion = cuentaRepatriacion.CodAltaAdhesion ?? 0,
                CantParticipantes = cantidadParticipantes,
                Procesado = procesado ? "S" : "N"
            });
        }

        private void CrearRelacionCuentas(string cuentaTitulos, RequestSecurity<WorkflowCTRReq> entity, ObtenerCuentasRepaResp cuentaRepatriacion)
        {
            IOpicsDA opic = DependencyFactory.Resolve<IOpicsDA>();

            var vinculacion = new VincularCuentasActivasReq
            {
                Nup = cuentaRepatriacion.Nup,
                CuentaTitulos = long.Parse(cuentaTitulos),
                Producto = "60",
                SubProducto = "0000",
                //Alias = alias?.Valor,
                Segmento = "RTL",
                //Descripcion = descripcion?.Valor,
                CuentaOperativa = cuentaRepatriacion.CtaOperativa ?? 0,
                Sucursal = cuentaRepatriacion.SucursalOperativa ?? 0,
                TipoCuenta = cuentaRepatriacion.TipoOperativa ?? 0,
                CodMoneda = "USD",
                Usuario = entity.Datos.Usuario,
                Ip = entity.Datos.Ip
            };

            //daDDC.VincularCuentasActivas(vinculacion);

            var reqSecurity = entity.MapperClass<RequestSecurity<VincularCuentasActivasReq>>(TypeMapper.IgnoreCaseSensitive);

            reqSecurity.Datos = vinculacion;
            reqSecurity.Canal = entity.Canal;
            reqSecurity.SubCanal = entity.SubCanal;

            DependencyFactory.Resolve<IExternalWebApiService>().VincularCuentasActivas(reqSecurity);

            opic.AltaCuentaTiluloOpics(new AltaCuentaOpicsReq { CuentaTitulo = long.Parse(cuentaTitulos), Usuario = entity.Datos.Usuario, Ip = entity.Datos.Ip });

        }

        private void ActualizarCuentasDDC(string cuentaTitulos, RequestSecurity<WorkflowCTRReq> entity, ObtenerCuentasRepaResp cuenta)
        {
            IOpicsDA opic = DependencyFactory.Resolve<IOpicsDA>();

            var vinculacion = new VincularCuentasActivasReq
            {
                Nup = cuenta.Nup,
                CuentaTitulos = long.Parse(cuentaTitulos),
                Producto = "60",
                SubProducto = "0000",
                Segmento = "RTL",
                CuentaOperativa = cuenta.CtaOperativa ?? 0,
                Sucursal = cuenta.SucursalOperativa ?? 0,
                TipoCuenta = cuenta.TipoOperativa ?? 0,
                Usuario = entity.Datos.Usuario,
                Ip = entity.Datos.Ip,
                CodAltaAdhesion = cuenta.CodAltaAdhesion
            };

            var reqSecurity = entity.MapperClass<RequestSecurity<VincularCuentasActivasReq>>(TypeMapper.IgnoreCaseSensitive);

            reqSecurity.Datos = vinculacion;
            reqSecurity.Canal = entity.Canal;
            reqSecurity.SubCanal = entity.SubCanal;

            DependencyFactory.Resolve<IExternalWebApiService>().VincularCuentasActivas(reqSecurity);

        }

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

        private static CabeceraConsulta GenerarCabeceraFondos(CargaSolicitudOrden entity, string FuncAutor, string codigoCanal, string cuit, string tipoDocumento)
        {
            return new CabeceraConsulta()
            {
                H_CanalTipo = "04",
                H_SubCanalId = "HTML",
                H_CanalVer = "000",
                H_SubCanalTipo = "11",
                H_CanalId = "5001",
                H_UsuarioTipo = "04",
                H_UsuarioID = "ONLINE",
                H_UsuarioAttr = " ",
                H_UsuarioPwd = "DV09SA10",
                H_IdusConc = "00000000",
                H_NumSec = "00000002",
                H_IndSincro = "0",
                H_TipoCliente = "E",
                H_TipoIDCliente = tipoDocumento,
                H_NroIDCliente = cuit,
                H_FechaNac = "19730510",
                H_Func_Autor = FuncAutor,
                CodigoCanal = codigoCanal
            };

        }

        private static CabeceraConsulta GenerarCabecera(string canal, string subcanal)
        {
            //TODO: se tiene que reemplazar por la forma correcta según dijo lucas.
            return new CabeceraConsulta()
            {
                H_CanalTipo = canal, // "22",
                H_SubCanalId = "0000",
                H_CanalVer = "000",
                H_SubCanalTipo = "99",
                H_CanalId = subcanal,
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

        private static CabeceraConsulta GenerarCabeceraACT(string canal, string subcanal)
        {
            //TODO: se tiene que reemplazar por la forma correcta según dijo lucas.
            return new CabeceraConsulta()
            {
                H_CanalTipo = canal, // "22",
                H_SubCanalId = "0000",
                H_CanalVer = "000",
                H_SubCanalTipo = "99",
                H_CanalId = subcanal,
                H_UsuarioTipo = "03",
                H_UsuarioID = "ONLINEBP",
                H_UsuarioAttr = " ",
                H_UsuarioPwd = "DV09SA10",
                H_IdusConc = "00000000",
                H_NumSec = "00000002",
                H_IndSincro = "0"
            };

        }

        //private static CabeceraConsulta GenerarCabeceraAltaCuenta()
        //{
        //    return new CabeceraConsulta()
        //    {
        //        H_CanalTipo = "04", // "22",
        //        H_SubCanalId = "HTML",
        //        H_CanalVer = "000",
        //        H_SubCanalTipo = "99",
        //        H_CanalId = "0099",
        //        H_UsuarioTipo = "03",
        //        H_UsuarioID = "ONLINEBP",
        //        H_UsuarioAttr = " ",
        //        H_UsuarioPwd = "DV09SA10",
        //        H_IdusConc = "00000000",
        //        H_NumSec = "00000002",
        //        H_IndSincro = "0",
        //    };

        //}

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

        public string ObtenerOperacion(string codigo)
        {
            try
            {
                var result = DependencyFactory.Resolve<IMotorServicioDataAccess>().ObtenerOperaciones();

                return result.FirstOrDefault(p => p.Codigo.ToUpper() == codigo.ToUpper())
                    ?.Descripcion;
            }
            catch (Exception ex)
            {
                //TODO: ver que se hará acá porque ni arranco aún.              
                throw ex;
            }
        }
        #endregion
    }
}
