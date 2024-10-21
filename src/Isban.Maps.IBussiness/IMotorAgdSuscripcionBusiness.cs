using Isban.MapsMB.Common.Entity;
using Isban.MapsMB.Common.Entity.Request;
using Isban.MapsMB.Common.Entity.Response;
using Isban.MapsMB.Entity.Request;
using Isban.MapsMB.Entity.Response;
using Isban.Mercados.Service.InOut;
using System.Collections.Generic;

namespace Isban.MapsMB.IBusiness
{
    public interface IMotorAgdSuscripcionBusiness
    {
        string ProcesosComunes(RequestSecurity<SolicitudOrden> entity);        
        MotorResponse BajaAdhesionesPorVigenciaVencida(RequestSecurity<WorkflowSAFReq> entity);

        MotorResponse BajaAdhesionesProcesadasYFallidas(RequestSecurity<SolicitudOrden> entity, List<CargaSolicitudOrden> solicitudesProcesadas);
        //List<CargaSolicitudOrden> ConsultaSaldoCuenta(RequestSecurity<SolicitudOrden> entity);
        EvaluaRiesgoResponse RealizarEncuestaDeRiesgoERI(RequestSecurity<SolicitudOrden> entity);
        ConfirmacionOrdenResponse ConfirmacionOrden(RequestSecurity<ConfirmacionOrdenReq> entity);

        ResultadoFondos RealizarSuscripcionFondos(RequestSecurity<SolicitudOrden> entity);
        MotorResponse RealizarConfirmacionMYA(RequestSecurity<SolicitudOrden> realizarConfirmacionMYA);

        MotorResponse RealizarBajaDeCuentasBloqueadasMAPS(RequestSecurity<SolicitudOrden> datos);
        //CondicionAdhesion FiltrarAdhesionesPorCondicion(RequestSecurity<SolicitudOrden> datos);
        List<CargaSolicitudOrden> ConsultaAdhesionesPorFallaTecnica(RequestSecurity<WorkflowSAFReq> entity);
        EstadoDecuentaResp ConsultaCuentasBloqueadas(RequestSecurity<SolicitudOrden> entity);
        //List<CargaSolicitudOrden> ConsultarSaldoRescatado(RequestSecurity<SolicitudOrden> entity);
        ConfirmaRiesgoResponse RealizarConfirmacionOrdenERI(RequestSecurity<ReqConfirmacionEri> entity);
        ResultadoFondos RealizarRescateFondo(RequestSecurity<SolicitudOrden> entity);
        List<CargaSolicitudOrden> ConsultaAdhesionesActivas(RequestSecurity<WorkflowSAFReq> entity);
        List<CargaSolicitudOrden> ConsultaAdhesionesEjecutar(RequestSecurity<WorkflowSAFReq> entity, string operacionDescipcion);
        ResultadoTransferencia RealizarTransferencia(RequestSecurity<OrdernBase> entity);

        MotorResponse EjecutarEnvioMailAltaCuentaTitulos(RequestSecurity<WorkflowCTRReq> entity);
    }
}
