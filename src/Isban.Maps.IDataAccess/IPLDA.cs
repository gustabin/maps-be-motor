using Isban.MapsMB.Common.Entity.Request;
using Isban.MapsMB.Common.Entity.Response;
using System.Collections.Generic;

namespace Isban.MapsMB.IDataAccess
{
    public interface IPLDA
    {
        List<ListaDeFondosResponse> ObtenerInfoFondos(ObtenerInfoFondos entity);
    }
}
