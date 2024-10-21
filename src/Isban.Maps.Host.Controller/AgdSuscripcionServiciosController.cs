using Isban.MapsMB.Business.Attributes;
using Isban.MapsMB.Common.Entity;
using Isban.MapsMB.Common.Entity.Response;
using Isban.MapsMB.Entity.Constantes.Estructuras;
using Isban.MapsMB.Entity.Request;
using Isban.MapsMB.Entity.Response;
using Isban.MapsMB.IBusiness;
using Isban.MapsMB.IBussiness;
using Isban.Mercados.Service;
using Isban.Mercados.Service.InOut;
using Isban.Mercados.UnityInject;
using System.Collections.Generic;
using System.Web.Http;

namespace Isban.MapsMB.Host.Controller
{
    public class AgdSuscripcionServiciosController : ServiceWebApiMotor
    {
        #region Revisados
        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<MotorResponse> RealizarBajaDeCuentasBloqueadasMAPS(RequestSecurity<SolicitudOrden> entity)
        {
            var bussiness = Mercados.UnityInject.DependencyFactory.Resolve<IMotorAgdSuscripcionBusiness>();

            return CheckSecurityAndTrace(() => bussiness.RealizarBajaDeCuentasBloqueadasMAPS(entity), entity);
        }

        //[HttpPost]
        //[MetodoInfo(ModoObtencion.Metodo)]
        //public virtual Response<CondicionAdhesion> FiltrarAdhesionesPorCondicion(RequestSecurity<SolicitudOrden> entity)
        //{
        //    var bussiness = DependencyFactory.Resolve<IMotorAgdSuscripcionBusiness>();

        //    return CheckSecurityAndTrace(() => bussiness.FiltrarAdhesionesPorCondicion(entity), entity);
        //}

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<EvaluaRiesgoResponse> RealizarEvaluacionDeRiesgoERI(RequestSecurity<SolicitudOrden> entity)
        {

            var bussiness = DependencyFactory.Resolve<IMotorAgdSuscripcionBusiness>();
            return CheckSecurityAndTrace(() => bussiness.RealizarEncuestaDeRiesgoERI(entity), entity);

        }

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<ConfirmaRiesgoResponse> ConfirmacionOrdenERI(RequestSecurity<ReqConfirmacionEri> entity)
        {
            var bussiness = Mercados.UnityInject.DependencyFactory.Resolve<IMotorAgdSuscripcionBusiness>();

            return CheckSecurityAndTrace(() => bussiness.RealizarConfirmacionOrdenERI(entity), entity);

        }

        /// <summary>
        /// Método que obtiene todas las adhesiones activas y listas para ser procesadas en el día de hoy.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<List<CargaSolicitudOrden>> ConsultaAdhesionesActivas(RequestSecurity<WorkflowSAFReq> entity)
        {
            var bussiness = DependencyFactory.Resolve<IMotorAgdSuscripcionBusiness>();
            var motorBss = DependencyFactory.Resolve<IMotorBusiness>();
            entity.Datos.Canal = entity.Canal;
            entity.Datos.SubCanal = entity.SubCanal;
            string operacionDescipcion = motorBss.ObtenerOperacion(CodigosOperacion.Suscripcion); //TODO: tomar de tabla
            return CheckSecurityAndTrace(() => bussiness.ConsultaAdhesionesEjecutar(entity, operacionDescipcion), entity);
        }

        /// <summary>
        /// Método que obtiene todas las adhesiones que tienen su vigencia vencida.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //[HttpPost]
        //[MetodoInfo(ModoObtencion.Metodo)]
        //public virtual Response<MotorResponse> BajaAdhesionesPorVigenciaVencida(RequestSecurity<WorkflowSAFReq> entity)
        //{
        //    var bussiness = DependencyFactory.Resolve<IMotorAgdSuscripcionBusiness>();
        //    entity.Datos.Canal = entity.Canal;
        //    entity.Datos.SubCanal = entity.SubCanal;

        //    return CheckSecurityAndTrace(() => bussiness.BajaAdhesionesPorVigenciaVencida(entity), entity);
        //}

        /// <summary>
        /// Método que obtiene todas las adhesiones activas que no se procesaron por falla técnica.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<List<CargaSolicitudOrden>> ConsultaAdhesionesPorFallaTecnica(RequestSecurity<WorkflowSAFReq> entity)
        {
            var bussiness = DependencyFactory.Resolve<IMotorAgdSuscripcionBusiness>();
            entity.Datos.Canal = entity.Canal;
            entity.Datos.SubCanal = entity.SubCanal;

            return CheckSecurityAndTrace(() => bussiness.ConsultaAdhesionesPorFallaTecnica(entity), entity);
        }

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<EstadoDecuentaResp> ConsultaCuentasBloqueadas(RequestSecurity<SolicitudOrden> entity)
        {
            var bussiness = DependencyFactory.Resolve<IMotorAgdSuscripcionBusiness>();

            return CheckSecurityAndTrace(() => bussiness.ConsultaCuentasBloqueadas(entity), entity);
        }       

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        [NonTransactionInterceptor]
        public virtual Response<MotorResponse> RealizarConfirmacionMYA(RequestSecurity<SolicitudOrden> entity)
        {
            var bussiness = DependencyFactory.Resolve<IMotorAgdSuscripcionBusiness>();
            entity.Datos.Canal = entity.Canal;
            entity.Datos.SubCanal = entity.SubCanal;

            return CheckSecurityAndTrace(() => bussiness.RealizarConfirmacionMYA(entity), entity);
        }

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<ResultadoFondos> RealizarSuscripcionFondos(RequestSecurity<SolicitudOrden> entity)
        {
            var bussiness = Mercados.UnityInject.DependencyFactory.Resolve<IMotorAgdSuscripcionBusiness>();

            return CheckSecurityAndTrace(() => bussiness.RealizarSuscripcionFondos(entity), entity);
        }

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<ResultadoTransferencia> RealizarTransferencia(RequestSecurity<OrdernBase> entity)
        {
            var bussiness = DependencyFactory.Resolve<IMotorAgdSuscripcionBusiness>();

            return CheckSecurityAndTrace(() => bussiness.RealizarTransferencia(entity), entity);
        }
        #endregion

    }
}
