
namespace Isban.MapsMB.Host.Controller
{
    using IBusiness;
    using IBussiness;
    using Isban.MapsMB.Common.Entity.Response;
    using Isban.MapsMB.Entity.Request;
    using Isban.MapsMB.Entity.Response;
    using Isban.Mercados.Service;
    using Isban.Mercados.Service.InOut;
    using Isban.Mercados.UnityInject;
    using Mercados;
    using System;
    using System.Linq;
    using System.Configuration;
    using System.Web.Http;
    using Entity.Constantes.Estructuras;
    using Isban.Mercados.Security.Adsec.Entity;

    /// <summary>
    /// Clase responsable de poseer el flujo  de ejecución de MAPS para Servicio SAF
    /// </summary>
    public class ServiceWorkflowController : ServiceWebApiMotor
    {
        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<dynamic> EjecutarReprocesoSAF(RequestSecurity<WorkflowSAFReq> entity)
        {
            var result = ResponseDefault(entity, "Reproceso Alta SAF");

            try
            {
                var businessMB = DependencyFactory.Resolve<IMotorBusiness>();

                //0- Dar de baja todas las adhesiones que tienen vencida sus vigencias
                entity.Dato = ConfigurationManager.AppSettings["DATO"];
                entity.Firma = ConfigurationManager.AppSettings["FIRMA"];
                var response = businessMB.BajaAdhesionesPorVigenciaVencida(entity);
                result.Datos.Descripcion += " " + response.Descripcion;

                var solicitudOrden = entity.MapperClass<RequestSecurity<SolicitudOrden>>(TypeMapper.IgnoreCaseSensitive);
                solicitudOrden.Datos = entity.MapperClass<SolicitudOrden>(TypeMapper.IgnoreCaseSensitive);
                solicitudOrden.Datos.Solicitudes = businessMB.ConsultaAdhesionesPorFallaTecnica(entity);

                solicitudOrden.Dato = ConfigurationManager.AppSettings["DATO"];
                solicitudOrden.Firma = ConfigurationManager.AppSettings["FIRMA"];

                result.Mensaje = businessMB.ProcesosComunes(solicitudOrden);


            }
            catch (Exception ex)
            {
                result.Datos.Exitoso = false;
                result.Codigo = 2;
                result.Mensaje = ex.InnerException.Message;
                result.MensajeTecnico = ex.InnerException.ToString();
            }
            finally
            {
                result.Datos.FinEjecución = DateTime.Now;
                result.Datos.EjecucionTotal = result.Datos.FinEjecución.Subtract(result.Datos.InicioEjecución).Duration();
            }
            return result;
        }

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<dynamic> EjecutarWorkflowSAF(RequestSecurity<WorkflowSAFReq> entity)
        {
            //TODO: agregar la seguridad necesaria para ejecución manual. Alguna especie de toquen
            //TODO: pensarlo en abrir en hilos de packs de nups

            Response<dynamic> result = ResponseDefault(entity, "Flujo Alta SAF");

            try
            {
                var businessMB = DependencyFactory.Resolve<IMotorBusiness>();
                //0- Dar de baja todas las adhesiones que tienen vencida sus vigencias
                entity.Dato = ConfigurationManager.AppSettings["DATO"];
                entity.Firma = ConfigurationManager.AppSettings["FIRMA"];
                var response = businessMB.BajaAdhesionesPorVigenciaVencida(entity);
                result.Datos.Descripcion += " " + response.Descripcion;

                //1- Traigo todas las adhesiones activas en MAPS: OK
                var solicitudOrden = entity.MapperClass<RequestSecurity<SolicitudOrden>>(TypeMapper.IgnoreCaseSensitive);
                solicitudOrden.Datos = entity.Datos.MapperClass<SolicitudOrden>(TypeMapper.IgnoreCaseSensitive);
                solicitudOrden.Datos.Solicitudes = businessMB.ConsultaAdhesionesActivas(entity);
                solicitudOrden.Dato = ConfigurationManager.AppSettings["DATO"];
                solicitudOrden.Firma = ConfigurationManager.AppSettings["FIRMA"];

                result.Mensaje = businessMB.ProcesosComunes(solicitudOrden);

            }
            catch (Exception ex)
            {
                result.Datos.Exitoso = false;
                result.Codigo = 2;
                result.Mensaje = ex.InnerException.Message;
                result.MensajeTecnico = ex.InnerException.ToString();
            }
            finally
            {
                result.Datos.FinEjecución = DateTime.Now;
                result.Datos.EjecucionTotal = result.Datos.FinEjecución.Subtract(result.Datos.InicioEjecución).Duration();
            }

            return result;
        }


        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<dynamic> EjecutarWorkflowAGD(RequestSecurity<WorkflowSAFReq> entity)
        {
            //TODO: agregar la seguridad necesaria para ejecución manual. Alguna especie de toquen
            //TODO: pensarlo en abrir en hilos de packs de nups
            Response<dynamic> result = ResponseDefault(entity, $"{CodigosOperacion.Suscripcion}");

            try
            {
                var businessMB = DependencyFactory.Resolve<IMotorBusiness>();
                //0- Dar de baja todas las adhesiones que tienen vencida sus vigencias
                entity.Dato = ConfigurationManager.AppSettings["DATO"];
                entity.Firma = ConfigurationManager.AppSettings["FIRMA"];
                var response = businessMB.BajaAdhesionesPorVigenciaVencida(entity);
                result.Datos.Descripcion += " " + response.Descripcion;


                var motorAGD = DependencyFactory.Resolve<IMotorAgdSuscripcionBusiness>();                
                string operacionDescipcion = businessMB.ObtenerOperacion(CodigosOperacion.Suscripcion); //TODO: tomar de tabla
               

                //1- Traigo todas las adhesiones activas en MAPS: OK
                var solicitudOrden = entity.MapperClass<RequestSecurity<SolicitudOrden>>(TypeMapper.IgnoreCaseSensitive);
                solicitudOrden.Datos = entity.Datos.MapperClass<SolicitudOrden>(TypeMapper.IgnoreCaseSensitive);
                solicitudOrden.Datos.Solicitudes = motorAGD.ConsultaAdhesionesEjecutar(entity, operacionDescipcion);
                solicitudOrden.Dato = ConfigurationManager.AppSettings["DATO"];
                solicitudOrden.Firma = ConfigurationManager.AppSettings["FIRMA"];

                result.Mensaje = motorAGD.ProcesosComunes(solicitudOrden);

            }
            catch (Exception ex)
            {
                result.Datos.Exitoso = false;
                result.Codigo = 2;
                result.Mensaje = ex.InnerException.Message;
                result.MensajeTecnico = ex.InnerException.ToString();
            }
            finally
            {
                result.Datos.FinEjecución = DateTime.Now;
                result.Datos.EjecucionTotal = result.Datos.FinEjecución.Subtract(result.Datos.InicioEjecución).Duration();
            }

            return result;
        }

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<MotorResponse> EjecutarWorkflowRescate(RequestSecurity<WorkflowSAFReq> entity)
        {
            string operacionDescripcion = DependencyFactory.Resolve<IMotorBusiness>()
                .ObtenerOperacion(CodigosOperacion.Rescate); 
            var result = ResponseMotor(entity, $"{operacionDescripcion}");
            try
            {
                var businessMB = DependencyFactory.Resolve<IMotorBusiness>();
                
                //0- Dar de baja todas las adhesiones que tienen vencida sus vigencias
                entity.Dato = ConfigurationManager.AppSettings["DATO"];
                entity.Firma = ConfigurationManager.AppSettings["FIRMA"];
                var response = businessMB.BajaAdhesionesPorVigenciaVencida(entity);
                result.Datos.Descripcion += " " + response.Descripcion;

                var motorAGD = DependencyFactory.Resolve<IMotorAgdSuscripcionBusiness>();
                //1- Traigo todas las adhesiones activas de Rescate en MAPS
                var solicitudOrden = entity.MapperClass<RequestSecurity<SolicitudOrden>>(TypeMapper.IgnoreCaseSensitive);
                solicitudOrden.Datos = entity.Datos.MapperClass<SolicitudOrden>(TypeMapper.IgnoreCaseSensitive);
                solicitudOrden.Datos.Solicitudes = motorAGD.ConsultaAdhesionesEjecutar(entity, operacionDescripcion);
                solicitudOrden.Dato = ConfigurationManager.AppSettings["DATO"];
                solicitudOrden.Firma = ConfigurationManager.AppSettings["FIRMA"];
                //2- Rescate
               var solicitudes = businessMB.RealizarRescateFondo(solicitudOrden);

                //3 - Baja de solicitudes que se procesaron correctamente
                if(solicitudes.Ok.Count() > 0)
                motorAGD.BajaAdhesionesProcesadasYFallidas(solicitudOrden, solicitudes.Ok);

                //4 - Baja de solicitudes que fallaron al tercer intento
                if (solicitudes.NoOk.Count() > 0)
                motorAGD.BajaAdhesionesProcesadasYFallidas(solicitudOrden, solicitudes.NoOk?.Where(a => a.BajaTercerIntento == true)?.ToList());

                //5 - confirmación por correo MYA
                var businessMBAgd = DependencyFactory.Resolve<IMotorAgdSuscripcionBusiness>();
                var realizarConfirmacionMYA = solicitudOrden.MapperClass<RequestSecurity<SolicitudOrden>>(TypeMapper.IgnoreCaseSensitive);
                businessMBAgd.RealizarConfirmacionMYA(realizarConfirmacionMYA);

                result.Mensaje = "Último servicio llamado RealizarRescateFondo";
            }
            catch (Exception ex)
            {
                result.Datos.Exitoso = false;
                result.Codigo = 2;
                result.Mensaje = ex.InnerException.Message;
                result.MensajeTecnico = ex.InnerException.ToString();
            }
            finally
            {
                result.Datos.FinEjecución = DateTime.Now;
                result.Datos.EjecucionTotal = result.Datos.FinEjecución.Subtract(result.Datos.InicioEjecución).Duration();
            }

            return result;
        }

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<dynamic> EjecutarAltaCuentasCTRepatriacion(RequestSecurity<WorkflowCTRReq> entity)
        {
            var result = ResponseDefault(entity, "Alta cuentas Repatriacion");

            try
            {
                var businessMB = DependencyFactory.Resolve<IMotorBusiness>();
                entity.Dato = ConfigurationManager.AppSettings["DATO"];
                entity.Firma = ConfigurationManager.AppSettings["FIRMA"];

                var response = businessMB.RealizarAltaCuentasCTRepatriacion(entity);

                result.Datos.Descripcion += " " + response.Descripcion;

            }
            catch (Exception ex)
            {
                result.Datos.Exitoso = false;
                result.Codigo = 2;
                result.Mensaje = ex.InnerException.Message;
                result.MensajeTecnico = ex.InnerException.ToString();
            }
            finally
            {
                result.Datos.FinEjecución = DateTime.Now;
                result.Datos.EjecucionTotal = result.Datos.FinEjecución.Subtract(result.Datos.InicioEjecución).Duration();
            }
            return result;
        }

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<dynamic> ModificarRelacionClienteContrato(RequestSecurity<WorkflowCTRReq> entity)
        {
            var result = ResponseDefault(entity, "Modificar relacion cliente contrato");

            try
            {
                var businessMB = DependencyFactory.Resolve<IMotorBusiness>();
                entity.Dato = ConfigurationManager.AppSettings["DATO"];
                entity.Firma = ConfigurationManager.AppSettings["FIRMA"];

                var response = businessMB.ModificarRelacionClienteContrato(entity);

                result.Datos.Descripcion += " " + response.Descripcion;

            }
            catch (Exception ex)
            {
                result.Datos.Exitoso = false;
                result.Codigo = 2;
                result.Mensaje = ex.InnerException.Message;
                result.MensajeTecnico = ex.InnerException.ToString();
            }
            finally
            {
                result.Datos.FinEjecución = DateTime.Now;
                result.Datos.EjecucionTotal = result.Datos.FinEjecución.Subtract(result.Datos.InicioEjecución).Duration();
            }
            return result;
        }
        


        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<dynamic> EjecutarWorkflowRepatriacion(RequestSecurity<WorkflowCTRReq> entity)
        {
            var result = ResponseDefault(entity, "Ejecutar Workflow Repatriacion");

            try
            {
                var businessMB = DependencyFactory.Resolve<IMotorBusiness>();
                entity.Dato = ConfigurationManager.AppSettings["DATO"];
                entity.Firma = ConfigurationManager.AppSettings["FIRMA"];

                var response = businessMB.EjecutarWorkflowRepatriacion(entity);

                businessMB.EjecutarRelacionarCuentasOperativas(entity);

                businessMB.EjecutarWorkflowAltaCuentaTitulos(entity);

                WorkflowSAFReq workflow = new WorkflowSAFReq
                {
                    Canal = "04",
                    SubCanal = "0999",
                    Usuario = "SRVCA4PROD",
                    Ip = "1.1.1.1"
                };
                RequestSecurity<WorkflowSAFReq> entityMail = new RequestSecurity<WorkflowSAFReq>
                {
                    Canal = "04",
                    SubCanal = "0999",
                    DatosFirmado = DatosFirmado.No,
                    TipoHash = TipoHash.B64,
                    Dato = "SFA",
                    Firma = "kjdnskj"
                };
                entityMail.Datos = workflow;


                var solicitudOrden = entity.MapperClass<RequestSecurity<SolicitudOrden>>(TypeMapper.IgnoreCaseSensitive);
                solicitudOrden.Datos = entity.Datos.MapperClass<SolicitudOrden>(TypeMapper.IgnoreCaseSensitive);
                solicitudOrden.Datos.Solicitudes = businessMB.ConsultaAdhesionesRTFActivas(entityMail);
                solicitudOrden.Dato = ConfigurationManager.AppSettings["DATO"];
                solicitudOrden.Firma = ConfigurationManager.AppSettings["FIRMA"];

                businessMB.EnvioMailAdhesionesPendientes(solicitudOrden);

                result.Datos.Descripcion += " " + response.Descripcion;

            }
            catch (Exception ex)
            {
                result.Datos.Exitoso = false;
                result.Codigo = 2;
                result.Mensaje = ex.InnerException.Message;
                result.MensajeTecnico = ex.InnerException.ToString();
            }
            finally
            {
                result.Datos.FinEjecución = DateTime.Now;
                result.Datos.EjecucionTotal = result.Datos.FinEjecución.Subtract(result.Datos.InicioEjecución).Duration();
            }
            return result;
        }



        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<dynamic> EjecutarWorkflowAltaCuentaTitulos(RequestSecurity<WorkflowCTRReq> entity)
        {
            var result = ResponseDefault(entity, "Ejecutar Workflow Alta CuentaTitulos");

            try
            {
                var businessMB = DependencyFactory.Resolve<IMotorBusiness>();
                entity.Dato = ConfigurationManager.AppSettings["DATO"];
                entity.Firma = ConfigurationManager.AppSettings["FIRMA"];

                var response = businessMB.EjecutarWorkflowAltaCuentaTitulos(entity);

                result.Datos.Descripcion += " " + response.Descripcion;

            }
            catch (Exception ex)
            {
                result.Datos.Exitoso = false;
                result.Codigo = 2;
                result.Mensaje = ex.InnerException.Message;
                result.MensajeTecnico = ex.InnerException.ToString();
            }
            finally
            {
                result.Datos.FinEjecución = DateTime.Now;
                result.Datos.EjecucionTotal = result.Datos.FinEjecución.Subtract(result.Datos.InicioEjecución).Duration();
            }
            return result;
        }


        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<dynamic> TestServicioIN(RequestSecurity<WorkflowCTRReq> entity)
        {
            var result = ResponseDefault(entity, "Ejecutar Relacionar CuentasOperativas");

            try
            {
                var businessMB = DependencyFactory.Resolve<IMotorBusiness>();
                entity.Dato = ConfigurationManager.AppSettings["DATO"];
                entity.Firma = ConfigurationManager.AppSettings["FIRMA"];

                var response = businessMB.TestServicioIN(entity);

                result.Datos.Descripcion += " " + response.Descripcion;

            }
            catch (Exception ex)
            {
                result.Datos.Exitoso = false;
                result.Codigo = 2;
                result.Mensaje = ex.InnerException.Message;
                result.MensajeTecnico = ex.InnerException.ToString();
            }
            finally
            {
                result.Datos.FinEjecución = DateTime.Now;
                result.Datos.EjecucionTotal = result.Datos.FinEjecución.Subtract(result.Datos.InicioEjecución).Duration();
            }
            return result;
        }


        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<dynamic> EjecutarEnvioMailAltaCuentaTitulos(RequestSecurity<WorkflowSAFReq> entity)
        {
            var result = ResponseDefault(entity, "Ejecutar Envio Mail Alta CuentaTitulos");

            try
            {

                var businessMB = DependencyFactory.Resolve<IMotorBusiness>();
                entity.Dato = ConfigurationManager.AppSettings["DATO"];
                entity.Firma = ConfigurationManager.AppSettings["FIRMA"];


                var solicitudOrden = entity.MapperClass<RequestSecurity<SolicitudOrden>>(TypeMapper.IgnoreCaseSensitive);
                solicitudOrden.Datos = entity.Datos.MapperClass<SolicitudOrden>(TypeMapper.IgnoreCaseSensitive);
                solicitudOrden.Datos.Solicitudes = businessMB.ConsultaAdhesionesRTFActivas(entity);
                solicitudOrden.Dato = ConfigurationManager.AppSettings["DATO"];
                solicitudOrden.Firma = ConfigurationManager.AppSettings["FIRMA"];

                var response = businessMB.RealizarEnvioRTF(solicitudOrden);

                result.Datos.Descripcion += " " + response;

            }
            catch (Exception ex)
            {
                result.Datos.Exitoso = false;
                result.Codigo = 2;
                result.Mensaje = ex.InnerException.Message;
                result.MensajeTecnico = ex.InnerException.ToString();
            }
            finally
            {
                result.Datos.FinEjecución = DateTime.Now;
                result.Datos.EjecucionTotal = result.Datos.FinEjecución.Subtract(result.Datos.InicioEjecución).Duration();
            }
            return result;
        }



        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<dynamic> EjecutarRelacionarCuentasOperativas(RequestSecurity<WorkflowCTRReq> entity)
        {
            var result = ResponseDefault(entity, "Ejecutar Relacionar CuentasOperativas");

            try
            {
                var businessMB = DependencyFactory.Resolve<IMotorBusiness>();
                entity.Dato = ConfigurationManager.AppSettings["DATO"];
                entity.Firma = ConfigurationManager.AppSettings["FIRMA"];

                var response = businessMB.EjecutarRelacionarCuentasOperativas(entity);

                result.Datos.Descripcion += " " + response.Descripcion;

            }
            catch (Exception ex)
            {
                result.Datos.Exitoso = false;
                result.Codigo = 2;
                result.Mensaje = ex.InnerException.Message;
                result.MensajeTecnico = ex.InnerException.ToString();
            }
            finally
            {
                result.Datos.FinEjecución = DateTime.Now;
                result.Datos.EjecucionTotal = result.Datos.FinEjecución.Subtract(result.Datos.InicioEjecución).Duration();
            }
            return result;
        }


        #region Métodos Privados                                           
        private Response<dynamic> ResponseDefault(RequestSecurity<WorkflowSAFReq> entity, string nombreFlujo)
        {
            var result = entity.MapperClass<Response<dynamic>>(TypeMapper.IgnoreCaseSensitive);
            var motorResponse = entity.MapperClass<MotorResponse>(TypeMapper.IgnoreCaseSensitive);
            motorResponse.Exitoso = true;
            motorResponse.Descripcion = $"El flujo {nombreFlujo} finalizó.";
            motorResponse.InicioEjecución = DateTime.Now;
            result.Datos = motorResponse;

            return result;

        }

        private Response<dynamic> ResponseDefault(RequestSecurity<WorkflowCTRReq> entity, string nombreFlujo)
        {
            var result = entity.MapperClass<Response<dynamic>>(TypeMapper.IgnoreCaseSensitive);
            var motorResponse = entity.MapperClass<MotorResponse>(TypeMapper.IgnoreCaseSensitive);
            motorResponse.Exitoso = true;
            motorResponse.Descripcion = $"El flujo {nombreFlujo} finalizó.";
            motorResponse.InicioEjecución = DateTime.Now;
            result.Datos = motorResponse;

            return result;

        }


        private Response<MotorResponse> ResponseMotor(RequestSecurity<WorkflowSAFReq> entity, string nombreFlujo)
        {
            var result = entity.MapperClass<Response<MotorResponse>>(TypeMapper.IgnoreCaseSensitive);
            var motorResponse = entity.MapperClass<MotorResponse>(TypeMapper.IgnoreCaseSensitive);
            motorResponse.Exitoso = true;
            motorResponse.Descripcion = $"El flujo {nombreFlujo} finalizó.";
            motorResponse.InicioEjecución = DateTime.Now;
            result.Datos = motorResponse;

            return result;

        }
        #endregion

    }
}
