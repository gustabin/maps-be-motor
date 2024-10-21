
namespace Isban.MapsMB.IBussiness
{
    using Isban.MapsMB.Common.Entity.Request;
    using Isban.MapsMB.Common.Entity.Response;

    public interface IMiddlewareBusiness
    {
        MiddlwareResponse ConsultaCuentasBloqueadas(MiddlwareReq req);
    }
}
