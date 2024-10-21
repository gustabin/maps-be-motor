using Isban.MapsMB.Entity.Request;
using Isban.MapsMB.IDataAccess;
using Isban.Mercados;
using Isban.Mercados.DataAccess;
using Isban.Mercados.DataAccess.OracleClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Isban.MapsMB.Common.Entity.Response;
using Isban.MapsMB.DataAccess.DBRequest;
using Isban.MapsMB.DataAccess.DBResponse;
using Isban.MapsMB.Common.Entity.Request;
using Isban.MapsMB.Common.Entity;
using System.Diagnostics.CodeAnalysis;

namespace Isban.MapsMB.DataAccess
{
    [ExcludeFromCodeCoverage]
    [ProxyProvider("DBOPICS", Owner = "OPICS")]
    public class OpicsDA : BaseGsaDa, IOpicsDA
    {
        /// <summary>
        /// Consulta los saldos de la Banca Privada
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Busco Las Atits Para el Nup y/o cuenta_bp ingresada
        /// </summary>
        /// <param name="entity">Request del servicio</param>
        /// <returns>Atits Para el Nup y/o cuenta_bp ingresada</returns>
        public virtual List<ConsultaLoadAtisResponse> ObtenerAtis(ConsultaLoadAtisRequest entity)
        {
            var request = entity.MapperClass<ConsultaLoadAtisDataAccessRequest>(TypeMapper.IgnoreCaseSensitive);
            var atisData = this.Provider.GetCollection<ConsultaLoadAtisDataAccessResponse>(CommandType.StoredProcedure, request);

            request.CheckError();

            var a = from b in atisData
                    select new ConsultaLoadAtisResponse
                    {
                        CuentaBp = long.Parse(b.CuentaBp.ToString().Substring(2, 2)),
                        CuentaAtit = b.CuentaAtit
                    };

            var loadAtis = atisData.MapperEnumerable<ConsultaLoadAtisResponse>(TypeMapper.IgnoreCaseSensitive);

            return loadAtis.ToList();
        }

        public virtual void ActualizarTransferencia(List<DatosTransferencia> entity)
        {

            var request = entity.MapperClass<ActualizaTransferenciaReq>(TypeMapper.IgnoreCaseSensitive);

            Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);
           
        }

        public virtual List<SaldoCuentaResp> ObtenerCuentasOperativas(ConsultaCuentaReq entity)
        {
            var request = entity.MapperClass<ConsultaCuentaOpicsDbReq>(TypeMapper.IgnoreCaseSensitive);
            var dbResul = Provider.GetCollection<SaldoCuentaDbResp>(CommandType.StoredProcedure, request);
            request.CheckError();

            return dbResul.MapperEnumerable<SaldoCuentaResp>(TypeMapper.IgnoreCaseSensitive).ToList();
        }

        public virtual List<SaldoCuentaResp> GuardarEstadoYSaldoCuentaOperativa(ConsultaCuentaReq entity)
        {
            var request = entity.MapperClass<ConsultaCuentaOpicsDbReq>(TypeMapper.IgnoreCaseSensitive);
            var dbResul = Provider.GetCollection<SaldoCuentaDbResp>(CommandType.StoredProcedure, request);
            request.CheckError();

            return dbResul.MapperEnumerable<SaldoCuentaResp>(TypeMapper.IgnoreCaseSensitive).ToList();
        }

        public ObtenerDireccionResponse ObtenerDireccion(string nup)
        {
            var request = new ObtenerDireccionRequest() { Nup = nup };
            var result = this.Provider.GetCollection<ObtenerDireccionDbResponse>(CommandType.StoredProcedure, request);
            if (result != null)
            {
                var dire = result.MapperEnumerable<ObtenerDireccionResponse>(TypeMapper.IgnoreCaseSensitive).ToList();
                if (dire.Count() > 0)
                return dire.LastOrDefault();
            }
            return new ObtenerDireccionResponse();
        }
        
        public virtual long? AltaCuentaTiluloOpics(AltaCuentaOpicsReq entity)
        {
            var request = entity.MapperClass<AltaCuentaOpicsDBReq>(TypeMapper.IgnoreCaseSensitive);



            var asdas = Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);

            //request.CheckError();

            return request.CuentaTitulo;
        }

    }
}
