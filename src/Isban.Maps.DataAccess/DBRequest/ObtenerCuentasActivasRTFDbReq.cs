using Isban.Maps.DataAccess.Base;
using Isban.MapsMB.DataAccess.Constantes;
using Isban.Mercados.DataAccess.OracleClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.DataAccess.DBRequest
{
    [ProcedureRequest("SP_OBTENER_CUENTAS_RTF", Package = "PKG_MAPS_TOOLS", Owner = Owner.Maps)]
    internal class ObtenerCuentasActivasRTFDbReq : RequestBase, IProcedureRequest
    {


    }
}
