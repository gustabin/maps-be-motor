namespace Isban.MapsMB.IDataAccess
{
    using Isban.MapsMB.Entity.Request;
    using Isban.MapsMB.Common.Entity;
    using Isban.MapsMB.Entity.Response;
    using System.Collections.Generic;
    using Common.Entity.Request;
    using Isban.MapsMB.Common.Entity.Response;

    public interface IMotorServicioDataAccess
    {
        //void ActualizaEvaluacionRiesgo(ActualizaEvaRiesgoReq entity);
        long? ActualizaSolicitud(ModificarSolicitudOrdenReq entity);
        string ConnectionString { get; }
        ChequeoAcceso Chequeo(EntityBase entity);
        string GetInfoDB(long id);
        List<CargaSolicitudOrden> ConsultaAdhesionesActivas(WorkflowSAFReq entity);
        List<AdheVigenciaVencida> ConsultaAdhesionesPorVigenciaVencida(WorkflowSAFReq entity);
        UsuarioRacf ObtenerUsuarioRacf();
        List<CargaSolicitudOrden> ConsultaAdhesionesPorFallaTecnica(WorkflowSAFReq entity);
        string ObtenerUsuarioAsesor();
        void ActualizaObservacionSolicitudesVencida(ModificarSolicitudOrdenReq datos);
        void EnviarMensaje(EnviarMensajesReq entity);
        List<NupSolicitudes> InformacionClientePorNup(WorkflowSAFReq entity);
        List<Mensaje> ObtenerMailsAEnviar(ObtenerMensajes request);
        void CargarMensajesDelcliente(EnviarMensajesNetReq request);
        string ObtenerModoEjecucionMyA();
        Texto ObtenerTexto(ObtenerTexto obtenerTextos);
        void ActualizarMensaje(ActualizarMensaje actualizarMensaje);
        void ActualizarMensajeRTF(ActualizarMensajeRTF actualizarMensaje);

        string ObtenerParametro(string parametro);
        
        dynamic ActualizarMapsParametros(ActualizarMapsParametro datos);
        void CargarMensajesDelclienteNET(EnviarMensajesNetReq request);
        List<OperacionesResp> ObtenerOperaciones();
        List<SaldoCuentaResp> ObtenerCuentasOperativas(ConsultaCuentaReq entity);
        List<SaldoCuentaResp> GuardarEstadoYSaldoCuentaOperativa(ConsultaCuentaReq entity);
        List<CuentasActivasRTF> ConsultaAdhesionesResumenRTF(WorkflowSAFReq entity);
        List<CuentaAdheridaRTF> ConsultaArchivosRTF(RTFWorkflowOnDemandReq entity);
        List<ArchivoRTF> ObtenerArchivoRTF(RTFWorkflowOnDemandReq entity);
        void CargarArchivoRTF(ArchivoRTFReq entity);
        List<ObtenerCuentasRepaResp> ObtenerCuentasRepatriacion(ObtenerCuentasRepaReq entity);
        void CargarCuentasParticipantes(CargarCuentasParticipantesReq entity);
        void ActualizarAdhesionRepatriacion(ActualizarAdhesionRepatriacionReq entity);
        List<AdhesionesMEPResp> ObtenerAdhesionesMEP(AdhesionesMEPReq entity);
        AdhesionMEPResp ObtenerAdhesionMEP(AdhesionesMEPReq entity);
        void ActualizarAdhesionMEPCompra(AdhesionMEPCompraReq entity);
        void ActualizarAdhesionMEPError(AdhesionMEPErrorReq entity);
        void ActualizarAdhesionMEPVenta(AdhesionMEPVentaReq entity);
        List<AdhesionesMEPCompraResp> ObtenerAdhesionesMEPCompra(AdhesionesMEPReq entity);
        AdhesionesMEPCompraResp ObtenerAdhesionMEPCompra(AdhesionesMEPReq entity);
        List<OrdenesMapsResp> ObtenerOrdenesMaps(OrdenesMapsReq entity);
        void ActualizarOrden(ActualizarOrdenReq entity);
        List<AdhesionesMEPResp> ObtenerAdhesionesMEPInversa(AdhesionesMEPReq entity);
        List<AdhesionesMEPCompraResp> ObtenerAdhesionesMEPInversaCompra(AdhesionesMEPReq entity);
        void ActualizarTarjeta(ActualizarTarjetaReq entity);
        void InsertarDisclaimerEriMEPCompra(InsertarDisclaimerEriMEPCompraReq entity);
        List<MensajeMEP> ObtenerMailsMepAEnviar(ObtenerMensajesMepReq entity);
        void ActualizarMensajeMep(ActualizarMensajeMepReq entity);
        Texto ObtenerTextoMep(ObtenerTextoMepReq entity);
    }
}
