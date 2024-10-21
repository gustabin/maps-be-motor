using Isban.MapsMB.Common.Entity;
using Isban.MapsMB.Common.Entity.Response;
using Isban.Mercados.Service.InOut;
using System.Collections.Generic;

namespace Isban.MapsMB.IBusiness
{
    public interface ICuentasBusiness
    {
        List<SaldoCuentaResp> ConsultarCuentasOperativas(RequestSecurity<EntityBase> entity);
    }
}
