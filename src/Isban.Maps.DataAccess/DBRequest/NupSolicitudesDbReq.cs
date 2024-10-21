namespace Isban.MapsMB.DataAccess.DBRequest
{
    using Constantes;
    using Isban.Mercados.DataAccess.OracleClient;
    using Maps.DataAccess.Base;

    [ProcedureRequest("SP_NUP_SOLICITUDES", Package = "PKG_MAPS_MYA", Owner = Owner.Maps)]
    internal class NupSolicitudesDbReq : RequestBase, IProcedureRequest
    {

      
    }
}
