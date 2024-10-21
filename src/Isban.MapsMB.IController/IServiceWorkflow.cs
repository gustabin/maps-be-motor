
namespace Isban.MapsMB.Host.Controller.Interfaces
{
    using Isban.MapsMB.Entity.Request;
    using Isban.Mercados.Service.InOut;

    public interface IServiceWorkflow
    {
        Response<dynamic> EjecutarReprocesoSAF(RequestSecurity<WorkflowSAFReq> entity);
        Response<dynamic> EjecutarWorkflowSAF(RequestSecurity<WorkflowSAFReq> entity);
    }
}
