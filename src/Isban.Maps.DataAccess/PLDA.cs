using Isban.MapsMB.Common.Entity.Request;
using Isban.MapsMB.Common.Entity.Response;
using Isban.MapsMB.DataAccess.DBRequest;
using Isban.MapsMB.DataAccess.DBResponse;
using Isban.MapsMB.IDataAccess;
using Isban.Mercados;
using Isban.Mercados.DataAccess;
using Isban.Mercados.DataAccess.OracleClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.DataAccess
{
    [ProxyProvider("DBPL", Owner = "PL")]
    public class PLDA : BaseProxy, IPLDA
    {
        public virtual List<ListaDeFondosResponse> ObtenerInfoFondos(ObtenerInfoFondos entity)
        {
            var request = entity.MapperClass<ObtenerInfoFondosDbReq>(TypeMapper.IgnoreCaseSensitive);

            var resp = this.Provider.GetCollection<ListaDeFondosDbResp>(CommandType.StoredProcedure, request);
            //request.CheckError();

            return resp.MapperEnumerable<ListaDeFondosResponse>(TypeMapper.IgnoreCaseSensitive).ToList();
        }
    }
 }
