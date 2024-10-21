using Isban.MapsMB.Common.Entity.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.IBusiness
{
    public interface IEnvioFaxMailService
    {
        string EnvioMailServidor(FaxMailParameter faxMailParameter);
    }
}
