using Isban.Maps.DataAccess.Base;
using Isban.MapsMB.DataAccess.Constantes;
using Isban.Mercados.DataAccess.OracleClient;

namespace Isban.MapsMB.DataAccess.DBRequest
{
    [ProcedureRequest("SP_CONSULTA_ORDENES_FT", Package = "PKG_MAPS_SOLICITUD_ORDENES", Owner = Owner.Maps)]
    internal class ConsultaOrdenesFallaTecnicaDbReq : RequestBase, IProcedureRequest
    {
    }
}
