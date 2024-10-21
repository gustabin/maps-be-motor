
namespace Isban.MapsMB.Host.Controller
{
    using Common.Entity;
    using IBussiness;
    using Mercados.Service;
    using Mercados.Service.InOut;
    using System.Collections.Generic;
    using System.Web.Http;

    public class ChequeoServiceController : ServiceWebApiBase
    {
        [MetodoInfo(ModoObtencion.Metodo)]
        [HttpPost]
        public Response<List<ChequeoAcceso>> Chequeo(Request<EntityBase> entity)
        {
            var business = Isban.Mercados.UnityInject.DependencyFactory.Resolve<IChequeoBusiness>();
            return NonCheckSecurityAndTrace(() => business.Chequeo(entity.Datos), entity);

        }
    }
}