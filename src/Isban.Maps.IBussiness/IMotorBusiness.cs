
namespace Isban.MapsMB.IBussiness
{
    using Common.Entity;
    using Common.Entity.Request;
    using Common.Entity.Response;
    using Entity.Request;
    using Entity.Response;
    using Mercados.Service.InOut;
    using System.Collections.Generic;

    public interface IMotorBusiness
    {
        string ProcesosComunes(RequestSecurity<SolicitudOrden> entity);
        List<CargaSolicitudOrden> ConsultaAdhesionesActivas(RequestSecurity<WorkflowSAFReq> entity);
        List<CargaSolicitudOrden> ConsultaAdhesionesRTFActivas(RequestSecurity<WorkflowSAFReq> entity);
        MotorResponse BajaAdhesionesPorVigenciaVencida(RequestSecurity<WorkflowSAFReq> entity);
        List<CargaSolicitudOrden> ConsultaSaldoCuenta(RequestSecurity<SolicitudOrden> entity);
        EvaluaRiesgoResponse RealizarEncuestaDeRiesgoERI(RequestSecurity<SolicitudOrden> entity);
        ConfirmacionOrdenResponse ConfirmacionOrden(RequestSecurity<ConfirmacionOrdenReq> entity);
        ResultadoFondos RealizarAltaAdhesionFondos(RequestSecurity<SolicitudOrden> entity);
        MotorResponse RealizarConfirmacionMYA(RequestSecurity<SolicitudOrden> realizarConfirmacionMYA);
        MotorResponse RealizarBajaDeCuentasBloqueadasMAPS(RequestSecurity<SolicitudOrden> datos);
        CondicionAdhesion FiltrarAdhesionesPorCondicion(RequestSecurity<SolicitudOrden> datos);
        List<CargaSolicitudOrden> ConsultaAdhesionesPorFallaTecnica(RequestSecurity<WorkflowSAFReq> entity);
        EstadoDecuentaResp ConsultaCuentasBloqueadas(RequestSecurity<SolicitudOrden> entity);
        List<CargaSolicitudOrden> ConsultarSaldoRescatado(RequestSecurity<SolicitudOrden> entity);
        ConfirmaRiesgoResponse RealizarConfirmacionOrdenERI(RequestSecurity<ReqConfirmacionEri> entity);
        ResultadoFondos RealizarRescateFondo(RequestSecurity<SolicitudOrden> entity);
        dynamic ActualizarMapsParametros(RequestSecurity<ActualizarMapsParametro> entity);
        List<CargaSolicitudOrden> ConsultaAdhesionesEjecutar(RequestSecurity<WorkflowSAFReq> entity, string operacion);
        string ObtenerOperacion(string codigo);
        string RealizarEnvioRTF(RequestSecurity<SolicitudOrden> entity);
        MotorResponse EnvioMailAdhesionesPendientes(RequestSecurity<SolicitudOrden> entity);
        MotorResponse EnvioResumenPorFaxMail(RequestSecurity<WorkflowSAFReq> entity);
        MotorResponse EnvioRTFPorCuentasFaxMail(RequestSecurity<WorkflowRTFReq> entity);
        ObtenerRTFDisponiblesResponse ObtenerRTFDisponiblesPorCliente(RequestSecurity<RTFWorkflowOnDemandReq> entity);
        List<ArchivoRTF> ObtenerPdfPorCuentaRTF(RequestSecurity<RTFWorkflowOnDemandReq> entity);
        MotorResponse RealizarAltaCuentasCTRepatriacion(RequestSecurity<WorkflowCTRReq> entity);
        MotorResponse ModificarRelacionClienteContrato(RequestSecurity<WorkflowCTRReq> entity);
        MotorResponse EjecutarWorkflowRepatriacion(RequestSecurity<WorkflowCTRReq> entity);
        MotorResponse EjecutarWorkflowAltaCuentaTitulos(RequestSecurity<WorkflowCTRReq> entity);
        MotorResponse EjecutarEnvioMailAltaCuentaTitulos(RequestSecurity<WorkflowSAFReq> entity);
        MotorResponse EjecutarRelacionarCuentasOperativas(RequestSecurity<WorkflowCTRReq> entity);
        MotorResponse TestServicioIN(RequestSecurity<WorkflowCTRReq> entity);
        List<CargaSolicitudOrden> ConsultaAdhesionesActivasACT(RequestSecurity<WorkflowSAFReq> entity);
        ResultadoFondosOBE SubscribirFondos(RequestSecurity<SolicitudOrden> entity);
        List<AdhesionesMEPResp> ObtenerAdhesionesMEP(RequestSecurity<AdhesionesMEPReq> entity);
        AdhesionMEPResp ObtenerAdhesionMEP(RequestSecurity<AdhesionesMEPReq> entity);
        string ActualizarAdhesionMEPCompra(RequestSecurity<AdhesionMEPCompraReq> entity);
        string ActualizarAdhesionMEPError(RequestSecurity<AdhesionMEPErrorReq> entity);
        string ActualizarAdhesionMEPVenta(RequestSecurity<AdhesionMEPVentaReq> entity);
        List<AdhesionesMEPCompraResp> ObtenerAdhesionesMEPCompra(RequestSecurity<AdhesionesMEPReq> entity);
        AdhesionesMEPCompraResp ObtenerAdhesionMEPCompra(RequestSecurity<AdhesionesMEPReq> entity);
        List<OrdenesMapsResp> ObtenerOrdenesMaps(RequestSecurity<OrdenesMapsReq> entity);
        string ActualizarOrden(RequestSecurity<ActualizarOrdenReq> entity);
        List<AdhesionesMEPResp> ObtenerAdhesionesMEPInversa(RequestSecurity<AdhesionesMEPReq> entity);
        List<AdhesionesMEPCompraResp> ObtenerAdhesionesMEPInversaCompra(RequestSecurity<AdhesionesMEPReq> entity);
        string ActualizarTarjeta(RequestSecurity<ActualizarTarjetaReq> entity);
        string InsertarDisclaimerEriMEPCompra(RequestSecurity<InsertarDisclaimerEriMEPCompraReq> entity);
        MotorResponse RealizarEnvioMepMya(RequestSecurity<EnvioMepMYAReq> realizarConfirmacionMYA);
    }
}