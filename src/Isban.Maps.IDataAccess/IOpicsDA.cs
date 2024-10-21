

namespace Isban.MapsMB.IDataAccess
{
    using Common.Entity.Request;
    using Isban.MapsMB.Common.Entity;
    using Isban.MapsMB.Common.Entity.Response;
    using Isban.MapsMB.Entity.Request;
    using System.Collections.Generic;

    public interface IOpicsDA
    {
        string ConnectionString { get; }
        ChequeoAcceso Chequeo(EntityBase entity);
        string GetInfoDB(long id);

        LoadSaldosResponse EjecutarLoadSaldos(LoadSaldosRequest entity);

        List<ConsultaLoadAtisResponse> ObtenerAtis(ConsultaLoadAtisRequest consultaLoadAtisRequest);

        List<SaldoCuentaResp> ObtenerCuentasOperativas(ConsultaCuentaReq entity);

        List<SaldoCuentaResp> GuardarEstadoYSaldoCuentaOperativa(ConsultaCuentaReq entity);

        ObtenerDireccionResponse ObtenerDireccion(string nup);

        long? AltaCuentaTiluloOpics(AltaCuentaOpicsReq entity);
    }
}
