namespace Isban.MapsMB.DataAccess.DBRequest
{
    using Constantes;
    using Isban.Common.Data;
    using Isban.MapsMB.IDataAccess;
    using Isban.Mercados.DataAccess.ConverterDBType;
    using Isban.Mercados.DataAccess.OracleClient;
    using Maps.DataAccess.Base;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    [ProcedureRequest("SP_OBTENER_OPERACION", Package = "PKG_MAPS_TOOLS", Owner = Owner.Maps)]
    internal class OperacionesDbReq : RequestBase, IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_COD_OPERACION", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string CodOperacion { get; set; }

    }
}
