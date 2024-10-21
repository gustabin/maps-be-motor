using Isban.MapsMB.Common.Entity;
using Isban.MapsMB.Common.Entity.Response;
using Isban.MapsMB.DataAccess.DBRequest;
using Isban.MapsMB.DataAccess.DBResponse;
using Isban.MapsMB.Entity.Request;
using Isban.MapsMB.Entity.Response;
using Isban.MapsMB.IDataAccess;
using Isban.Mercados;
using Isban.Mercados.DataAccess;
using Isban.Mercados.DataAccess.OracleClient;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Isban.MapsMB.DataAccess
{
    [ExcludeFromCodeCoverage]
    [ProxyProvider("DBSMC", Owner = "SMC")]
    public class SmcDA : BaseGsaDa, ISmcDA
    {
        public virtual string SimulacionAltaFondos(SimularAltaFondos req)
        {
            var response = SimulacionFondos(req);

            return response.DentroDelPerfil;
        }

        public virtual EjecutarAltaFondosResponse EjecutarAltaFondos(EjecutarAltaFondos req)
        {
            return EjecutarFondos(req);
        }

        public virtual LoadSaldosResponse EjecutarLoadSaldos(LoadSaldosRequest entity)
        {
            var request = entity.MapperClass<LoadSaldosDataAccessRequest>(TypeMapper.IgnoreCaseSensitive);
            var resp = this.Provider.GetCollection<LoadSaldosDataAccessResponse>(CommandType.StoredProcedure, request);

            request.CheckError();
            var result = new LoadSaldosResponse();

            result.ListaSaldos = resp.MapperEnumerable<Saldos>(TypeMapper.IgnoreCaseSensitive).ToList();

            //validar si devuelve un valor o varios.
            return result;
        }

        public virtual SaldoConcertadoNoLiquidadoResponse EjecutarSaldoConcertadoNoLiquidado(SaldoConcertadoNoLiquidadoRequest entity)
        {
            var request = entity.MapperClass<SaldoConcertadoNoLiquidadoDataAccessRequest>(TypeMapper.IgnoreCaseSensitive);
            var resp = this.Provider.GetFirst<SaldoConcertadoNoLiquidadoDataAccessResponse>(CommandType.StoredProcedure, request);

            request.CheckError();

            return resp.MapperClass<SaldoConcertadoNoLiquidadoResponse>(TypeMapper.IgnoreCaseSensitive);
        }

        public virtual SimularFondosResponse SimulacionFondos(SimularAltaFondos req)
        {
            var request = req.MapperClass<SimularAltaFondosDbReq>(TypeMapper.IgnoreCaseSensitive);

            Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);

            request.CheckError();

            return request.MapperClass<SimularFondosResponse>(TypeMapper.IgnoreCaseSensitive);
        }

        public virtual EjecutarAltaFondosResponse EjecutarFondos(EjecutarAltaFondos req)
        {
            var request = req.MapperClass<EjecutarAltaFondosDbReq>(TypeMapper.IgnoreCaseSensitive);

            Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);

            request.CheckError();

            return request.MapperClass<EjecutarAltaFondosResponse>();
        }
    }
}
