

namespace Isban.MapsMB.IDataAccess
{
    using Isban.MapsMB.Entity.Request;
    using Isban.MapsMB.Entity.Response;
    using Isban.MapsMB.Common.Entity.Response;
    using Common.Entity;

    public interface ISmcDA
    {
        ChequeoAcceso Chequeo(EntityBase entity);
        string ConnectionString { get; }
        string GetInfoDB(long id);

        string SimulacionAltaFondos(SimularAltaFondos req);

        EjecutarAltaFondosResponse EjecutarAltaFondos(EjecutarAltaFondos req);

        LoadSaldosResponse EjecutarLoadSaldos(LoadSaldosRequest loadSaldosRequest);

        SaldoConcertadoNoLiquidadoResponse EjecutarSaldoConcertadoNoLiquidado(SaldoConcertadoNoLiquidadoRequest entity);

        SimularFondosResponse SimulacionFondos(SimularAltaFondos req);

        EjecutarAltaFondosResponse EjecutarFondos(EjecutarAltaFondos req);
    }
}
