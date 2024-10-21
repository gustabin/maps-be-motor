namespace Isban.MapsMB.Host.Controller
{
    using Isban.MapsMB.Common.Entity.Request;
    using Isban.MapsMB.Common.Entity.Response;
    using Isban.MapsMB.Entity.Request;
    using Isban.MapsMB.Entity.Response;
    using Isban.MapsMB.IBussiness;
    using Isban.Mercados;
    using Isban.Mercados.Service;
    using Isban.Mercados.Service.InOut;
    using Isban.Mercados.UnityInject;
    using System;
    using System.Configuration;
    using System.Web.Http;

    /// <summary>
    /// Clase responsable de poseer el flujo  de ejecución de MAPS para Servicio SAF
    /// </summary>
    public class WorkflowRTFController : ServiceWebApiMotor
    {

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<dynamic> EjecutarWorkflowRTF(RequestSecurity<WorkflowSAFReq> entity)
        {
            var result = ResponseDefault(entity, "Workflow Mail RTF");

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

                //var response = businessMB.RealizarEnvioRTF(solicitudOrden);

                result.Datos.Descripcion += " " ;

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
        public virtual Response<dynamic> EjecutarWorkflowEnvioPdfRTF(RequestSecurity<WorkflowSAFReq> entity)
        {
            var result = ResponseDefault(entity, "Workflow Pdf RTF");

            try
            {
                var businessMB = DependencyFactory.Resolve<IMotorBusiness>();
                entity.Dato = ConfigurationManager.AppSettings["DATO"];
                entity.Firma = ConfigurationManager.AppSettings["FIRMA"];

                var response = businessMB.EnvioResumenPorFaxMail(entity);

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
        public virtual Response<dynamic> EjecutarEnvioRTFPdfPorCuentas(RequestSecurity<WorkflowRTFReq> entity)
        {
            var result = ResponseDefault(entity, "Workflow Pdf RTF");

            try
            {
                var businessMB = DependencyFactory.Resolve<IMotorBusiness>();
                entity.Dato = ConfigurationManager.AppSettings["DATO"];
                entity.Firma = ConfigurationManager.AppSettings["FIRMA"];

                var response = businessMB.EnvioRTFPorCuentasFaxMail(entity);

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

        private Response<dynamic> ResponseDefault(RequestSecurity<WorkflowRTFReq> entity, string nombreFlujo)
        {
            var result = entity.MapperClass<Response<dynamic>>(TypeMapper.IgnoreCaseSensitive);
            var motorResponse = entity.MapperClass<MotorResponse>(TypeMapper.IgnoreCaseSensitive);
            motorResponse.Exitoso = true;
            motorResponse.Descripcion = $"El flujo {nombreFlujo} finalizó.";
            motorResponse.InicioEjecución = DateTime.Now;
            result.Datos = motorResponse;

            return result;

        }
    }
}
