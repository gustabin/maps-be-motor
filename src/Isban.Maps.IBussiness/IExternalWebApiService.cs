
namespace Isban.MapsMB.IBussiness
{
    using Common.Entity;
    using Common.Entity.Request;
    using Common.Entity.Response;
    using Entity.Controles.Independientes;
    using Entity.Request;
    using Isban.MapsMB.Entity.Response;
    using Mercados.Security.Adsec.Entity;
    using Mercados.Service.InOut;
    using System.Collections.Generic;

    public interface IExternalWebApiService
    {
        ClienteDDC[] GetCuentas(RequestSecurity<GetCuentas> entity);
        ClienteDDC[] GetDatosCliente(RequestSecurity<GetDatosCliente> entity);
        FormularioResponse BajaAdhesion(RequestSecurity<FormularioResponse> entity);
        object ConfirmacionOrden(RequestSecurity<ConfirmacionOrdenReq> request);
        DisclaimerERI EvaluacionRiesgo(RequestSecurity<CargaSolicitudOrden> entity);
        SaldoRescatadoResponse ObtenerSaldoRescatado48hs(RequestSecurity<CargaSolicitudOrden> saldoRescatadoRequest);
        List<Cliente> GetCuentasAptas(RequestSecurity<CuentasAptasReq> entity);
        List<Cliente> GetClientePDC(GetClientePDC req);
        SimulaPdcResponse PDCSimulacionAltasBajas(RequestSecurity<SimulaPdcReq> entity);
        GetTitularesResponse[] GetTitulares(RequestSecurity<GetTitulares> entity);
        TenenciaValuadaFondosRTFResponse ObtenerTenenciaValuadaFondos(RequestSecurity<TenenciaValuadaFondosRTFRequest> entity);

        List<CuentasActivasRTF> ObtenerCuentasActivasPorPeriodoRTF(RequestSecurity<TenenciaValuadaFondosRTFRequest> entity);

        void VincularCuentasActivas(RequestSecurity<VincularCuentasActivasReq> request);

        ClienteDDC[] GetDatosClientePorDocumento(RequestSecurity<GetDatosCliente> entity);

        TenenciaValuadaFondosRTFResponse CallObtenerTenenciaValuadaFondos(RequestSecurity<TenenciaValuadaFondosRTFRequest> request);
    }
}
