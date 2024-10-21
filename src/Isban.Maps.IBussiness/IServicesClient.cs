using Isban.MapsMB.Common.Entity;
using Isban.MapsMB.Entity.Controles.Independientes;
using Isban.Mercados.Service.InOut;
using System.Collections.Generic;

namespace Isban.MapsMB.IBussiness
{
    public interface IServicesClient
    { 
        List<string> GetMailXNup(string canal, string subcanal, string nup);
    }
}
