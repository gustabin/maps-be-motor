namespace Isban.MapsMB.DataAccess.DBRequest
{
    using Constantes;
    using Isban.Common.Data;
    using Isban.Mercados.DataAccess.OracleClient;
    using Maps.DataAccess.Base;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    [ProcedureRequest("SP_ACT_OP_DMEP_ERROR", Package = "PKG_MAPS_TOOLS", Owner = Owner.Maps)]
    internal class AdhesionMEPErrorDbReq : RequestBase, IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ID_ADHESIONES", BindOnNull = true, Type = OracleDbType.Long)]
        public long IdAdhesion { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ESTADO", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Estado { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_OBSERVACION", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Observacion { get; set; }
    }
}
