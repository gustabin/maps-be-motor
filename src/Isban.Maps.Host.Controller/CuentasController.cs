using Isban.MapsMB.Common.Entity;
using Isban.MapsMB.Common.Entity.Request;
using Isban.MapsMB.Common.Entity.Response;
using Isban.MapsMB.IBusiness;
using Isban.Mercados;
using Isban.Mercados.Service;
using Isban.Mercados.Service.InOut;
using System.Collections.Generic;
using System.Web.Http;

namespace Isban.MapsMB.Host.Controller
{
    public class CuentasController : ServiceWebApiMotor
    {
      
        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<List<SaldoCuentaResp>> ConsultarCuentasOperativas(RequestSecurity<EntityBase> entity)
        {
            var bussiness = Mercados.UnityInject.DependencyFactory.Resolve<ICuentasBusiness>();
         
            return CheckSecurityAndTrace(() => bussiness.ConsultarCuentasOperativas(entity), entity);
        }
    }
}