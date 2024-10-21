
namespace Isban.MapsMB.Host.Controller
{
    using Business.Attributes;
    using Common.Entity.Request;
    using Isban.MapsMB.Common.Entity.Response;
    using Isban.MapsMB.Entity.Request;
    using Isban.MapsMB.Entity.Response;
    using Isban.MapsMB.IBussiness;
    using Isban.Mercados.Service;
    using Isban.Mercados.Service.InOut;
    using Isban.Mercados.UnityInject;
    using Mercados;
    using System.Collections.Generic;
    using System.Web.Http;

    public class ServiciosController : ServiceWebApiMotor
    {
        #region Revisados
        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<MotorResponse> RealizarBajaDeCuentasBloqueadasMAPS(RequestSecurity<SolicitudOrden> entity)
        {
            var bussiness = Mercados.UnityInject.DependencyFactory.Resolve<IMotorBusiness>();

            return CheckSecurityAndTrace(() => bussiness.RealizarBajaDeCuentasBloqueadasMAPS(entity), entity);
        }

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<CondicionAdhesion> FiltrarAdhesionesPorCondicion(RequestSecurity<SolicitudOrden> entity)
        {
            var bussiness = DependencyFactory.Resolve<IMotorBusiness>();

            return CheckSecurityAndTrace(() => bussiness.FiltrarAdhesionesPorCondicion(entity), entity);
        }

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<EvaluaRiesgoResponse> RealizarEvaluacionDeRiesgoERI(RequestSecurity<SolicitudOrden> entity)
        {

            var bussiness = DependencyFactory.Resolve<IMotorBusiness>();
            return CheckSecurityAndTrace(() => bussiness.RealizarEncuestaDeRiesgoERI(entity), entity);

        }

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<ConfirmaRiesgoResponse> ConfirmacionOrdenERI(RequestSecurity<ReqConfirmacionEri> entity)
        {
            var bussiness = Mercados.UnityInject.DependencyFactory.Resolve<IMotorBusiness>();

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
            var bussiness = DependencyFactory.Resolve<IMotorBusiness>();
            entity.Datos.Canal = entity.Canal;
            entity.Datos.SubCanal = entity.SubCanal;

            return CheckSecurityAndTrace(() => bussiness.ConsultaAdhesionesActivas(entity), entity);
        }

        /// <summary>
        /// Método que obtiene todas las adhesiones que tienen su vigencia vencida.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<MotorResponse> BajaAdhesionesPorVigenciaVencida(RequestSecurity<WorkflowSAFReq> entity)
        {
            var bussiness = DependencyFactory.Resolve<IMotorBusiness>();
            entity.Datos.Canal = entity.Canal;
            entity.Datos.SubCanal = entity.SubCanal;

            return CheckSecurityAndTrace(() => bussiness.BajaAdhesionesPorVigenciaVencida(entity), entity);
        }

        /// <summary>
        /// Método que obtiene todas las adhesiones activas que no se procesaron por falla técnica.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<List<CargaSolicitudOrden>> ConsultaAdhesionesPorFallaTecnica(RequestSecurity<WorkflowSAFReq> entity)
        {
            var bussiness = DependencyFactory.Resolve<IMotorBusiness>();
            entity.Datos.Canal = entity.Canal;
            entity.Datos.SubCanal = entity.SubCanal;

            return CheckSecurityAndTrace(() => bussiness.ConsultaAdhesionesPorFallaTecnica(entity), entity);
        }

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<EstadoDecuentaResp> ConsultaCuentasBloqueadas(RequestSecurity<SolicitudOrden> entity)
        {
            var bussiness = DependencyFactory.Resolve<IMotorBusiness>();

            return CheckSecurityAndTrace(() => bussiness.ConsultaCuentasBloqueadas(entity), entity);
        }

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<List<CargaSolicitudOrden>> ConsultaSaldoCuenta(RequestSecurity<SolicitudOrden> entity)
        {
            
            var bussiness = DependencyFactory.Resolve<IMotorBusiness>();

            return CheckSecurityAndTrace(() => bussiness.ConsultaSaldoCuenta(entity), entity);
        }

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<List<CargaSolicitudOrden>> ConsultarSaldoRescatado(RequestSecurity<SolicitudOrden> entity)
        {
            var bussiness = DependencyFactory.Resolve<IMotorBusiness>();

            return CheckSecurityAndTrace(() => bussiness.ConsultarSaldoRescatado(entity), entity);
        }

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        [NonTransactionInterceptor]
        public virtual Response<MotorResponse> RealizarConfirmacionMYA(RequestSecurity<SolicitudOrden> entity)
        {
            var bussiness = DependencyFactory.Resolve<IMotorBusiness>();
            entity.Datos.Canal = entity.Canal;
            entity.Datos.SubCanal = entity.SubCanal;

            return CheckSecurityAndTrace(() => bussiness.RealizarConfirmacionMYA(entity), entity);
        }

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<ResultadoFondos> RealizarAltaAdhesionFondos(RequestSecurity<SolicitudOrden> entity)
        {
            var bussiness = Mercados.UnityInject.DependencyFactory.Resolve<IMotorBusiness>();

            return CheckSecurityAndTrace(() => bussiness.RealizarAltaAdhesionFondos(entity), entity);
        }

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<dynamic> ActualizarMapsParametros(RequestSecurity<ActualizarMapsParametro> entity)
        {
            var bussiness = DependencyFactory.Resolve<IMotorBusiness>();

            return CheckSecurityAndTrace(() => bussiness.ActualizarMapsParametros(entity), entity);
        }

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<ResultadoFondos> RealizarRescateFondo(RequestSecurity<SolicitudOrden> entity)
        {
            var bussiness = DependencyFactory.Resolve<IMotorBusiness>();

            return CheckSecurityAndTrace(() => bussiness.RealizarRescateFondo(entity), entity);
        }

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<ResultadoFondosOBE> SubscribirFondos(RequestSecurity<SolicitudOrden> entity)
        {
            var bussiness = DependencyFactory.Resolve<IMotorBusiness>();

            return NonCheckSecurityAndTrace(() => bussiness.SubscribirFondos(entity), entity);
        }

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<ObtenerRTFDisponiblesResponse> ObtenerRTFDisponiblesPorCliente(RequestSecurity<RTFWorkflowOnDemandReq> entity)
        {
            var bussiness = DependencyFactory.Resolve<IMotorBusiness>();

            return CheckSecurityAndTrace(() => bussiness.ObtenerRTFDisponiblesPorCliente(entity), entity);
        }
        
        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<List<ArchivoRTF>> ObtenerPdfPorCuentaRTF(RequestSecurity<RTFWorkflowOnDemandReq> entity)
        {
            var bussiness = DependencyFactory.Resolve<IMotorBusiness>();

            return CheckSecurityAndTrace(() => bussiness.ObtenerPdfPorCuentaRTF(entity), entity);
        }

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<List<AdhesionesMEPResp>> ObtenerAdhesionesMEP(RequestSecurity<AdhesionesMEPReq> entity)
        {
            var bussiness = DependencyFactory.Resolve<IMotorBusiness>();

            return CheckSecurityAndTrace(() => bussiness.ObtenerAdhesionesMEP(entity), entity);
        }

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<AdhesionMEPResp> ObtenerAdhesionMEP(RequestSecurity<AdhesionesMEPReq> entity)
        {
            var bussiness = DependencyFactory.Resolve<IMotorBusiness>();

            return CheckSecurityAndTrace(() => bussiness.ObtenerAdhesionMEP(entity), entity);
        }

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<string> ActualizarAdhesionMEPCompra(RequestSecurity<AdhesionMEPCompraReq> entity)
        {
            var bussiness = DependencyFactory.Resolve<IMotorBusiness>();

            return CheckSecurityAndTrace(() => bussiness.ActualizarAdhesionMEPCompra(entity), entity);
        }

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<string> ActualizarAdhesionMEPError(RequestSecurity<AdhesionMEPErrorReq> entity)
        {
            var bussiness = DependencyFactory.Resolve<IMotorBusiness>();

            return CheckSecurityAndTrace(() => bussiness.ActualizarAdhesionMEPError(entity), entity);
        }

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<string> ActualizarAdhesionMEPVenta(RequestSecurity<AdhesionMEPVentaReq> entity)
        {
            var bussiness = DependencyFactory.Resolve<IMotorBusiness>();

            return CheckSecurityAndTrace(() => bussiness.ActualizarAdhesionMEPVenta(entity), entity);
        }

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<List<AdhesionesMEPCompraResp>> ObtenerAdhesionesMEPCompra(RequestSecurity<AdhesionesMEPReq> entity)
        {
            var bussiness = DependencyFactory.Resolve<IMotorBusiness>();

            return CheckSecurityAndTrace(() => bussiness.ObtenerAdhesionesMEPCompra(entity), entity);
        }

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<AdhesionesMEPCompraResp> ObtenerAdhesionMEPCompra(RequestSecurity<AdhesionesMEPReq> entity)
        {
            var bussiness = DependencyFactory.Resolve<IMotorBusiness>();

            return CheckSecurityAndTrace(() => bussiness.ObtenerAdhesionMEPCompra(entity), entity);
        }

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<List<OrdenesMapsResp>> ObtenerOrdenesMaps(RequestSecurity<OrdenesMapsReq> entity)
        {
            var bussiness = DependencyFactory.Resolve<IMotorBusiness>();

            return CheckSecurityAndTrace(() => bussiness.ObtenerOrdenesMaps(entity), entity);
        }

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<string> ActualizarOrden(RequestSecurity<ActualizarOrdenReq> entity)
        {
            var bussiness = DependencyFactory.Resolve<IMotorBusiness>();

            return CheckSecurityAndTrace(() => bussiness.ActualizarOrden(entity), entity);
        }

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<List<AdhesionesMEPResp>> ObtenerAdhesionesMEPInversa(RequestSecurity<AdhesionesMEPReq> entity)
        {
            var bussiness = DependencyFactory.Resolve<IMotorBusiness>();

            return CheckSecurityAndTrace(() => bussiness.ObtenerAdhesionesMEPInversa(entity), entity);
        }

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<List<AdhesionesMEPCompraResp>> ObtenerAdhesionesMEPInversaCompra(RequestSecurity<AdhesionesMEPReq> entity)
        {
            var bussiness = DependencyFactory.Resolve<IMotorBusiness>();

            return CheckSecurityAndTrace(() => bussiness.ObtenerAdhesionesMEPInversaCompra(entity), entity);
        }

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<string> ActualizarVisualizacionTarjeta(RequestSecurity<ActualizarTarjetaReq> entity)
        {
            var bussiness = DependencyFactory.Resolve<IMotorBusiness>();

            return CheckSecurityAndTrace(() => bussiness.ActualizarTarjeta(entity), entity);
        }

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<string> InsertarDisclaimerEriMEPCompra(RequestSecurity<InsertarDisclaimerEriMEPCompraReq> entity)
        {
            var bussiness = DependencyFactory.Resolve<IMotorBusiness>();

            return CheckSecurityAndTrace(() => bussiness.InsertarDisclaimerEriMEPCompra(entity), entity);
        }

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        [NonTransactionInterceptor]
        public virtual Response<MotorResponse> RealizarEnvioMepMya(RequestSecurity<EnvioMepMYAReq> entity)
        {
            var bussiness = DependencyFactory.Resolve<IMotorBusiness>();
            
            return CheckSecurityAndTrace(() => bussiness.RealizarEnvioMepMya(entity), entity);
        }
        #endregion
    }
}
