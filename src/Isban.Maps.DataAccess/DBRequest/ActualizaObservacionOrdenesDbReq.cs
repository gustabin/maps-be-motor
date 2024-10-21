
namespace Isban.MapsMB.DataAccess.DBRequest
{
    using Isban.Common.Data;
    using Isban.MapsMB.DataAccess.Constantes;
    using Isban.Mercados.DataAccess.OracleClient;
    using Maps.DataAccess.Base;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    [ProcedureRequest("SP_ACT_OBSERVA_BAJA", Package = "PKG_MAPS_SOLICITUD_ORDENES", Owner = Owner.Maps)]
    internal class ActualizaObservacionOrdenesDbReq : RequestBase, IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_COMPROBANTES", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Comprobantes { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_OBSERVACION", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Observacion { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_COD_ESTADO", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Estado { get; set; }
    }
}
