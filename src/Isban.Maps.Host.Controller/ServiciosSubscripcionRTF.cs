using Isban.MapsMB.Common.Entity.Response;
using Isban.MapsMB.Entity.Request;
using Isban.MapsMB.Entity.Response;
using Isban.MapsMB.IBussiness;
using Isban.Mercados.Service;
using Isban.Mercados.Service.InOut;
using Isban.Mercados.UnityInject;
using System.Web.Http;

namespace Isban.MapsMB.Host.Controller
{
    public class ServiciosSubscripcionRTF : ServiceWebApiMotor
    {
        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<MotorResponse> RealizarAltaMailsRTF(RequestSecurity<SolicitudOrden> entity)
        {
            var bussiness = DependencyFactory.Resolve<IMotorBusiness>();

            return CheckSecurityAndTrace(() => bussiness.EnvioMailAdhesionesPendientes(entity),entity);
        }

        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<MotorResponse> RealizarEnvioAdjuntoRTF(RequestSecurity<WorkflowSAFReq> entity)
        {
            var bussiness = DependencyFactory.Resolve<IMotorBusiness>();

            return CheckSecurityAndTrace(() => bussiness.EnvioResumenPorFaxMail(entity), entity);
        }
    }
}
