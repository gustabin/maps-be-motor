namespace Isban.MapsMB.DataAccess.DBRequest
{
    using Constantes;
    using Isban.Mercados.DataAccess.OracleClient;
    using Maps.DataAccess.Base;

    [ProcedureRequest("SP_OBTENER_OP_VMEP_FIFO", Package = "PKG_MAPS_TOOLS", Owner = Owner.Maps)]
    internal class ObtenerAdhesionesMEPInversaDbReq : RequestBase, IProcedureRequest
    {

      
    }
}
