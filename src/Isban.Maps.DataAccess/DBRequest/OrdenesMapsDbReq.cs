namespace Isban.MapsMB.DataAccess.DBRequest
{
    using Constantes;
    using Isban.Common.Data;
    using Isban.Mercados.DataAccess.OracleClient;
    using Maps.DataAccess.Base;
    using Oracle.ManagedDataAccess.Client;
    using System;
    using System.Data;

    [ProcedureRequest("SP_CONSULTA_ORDENES_MAPS", Package = "PKG_MAPS_TOOLS", Owner = Owner.Maps)]
    internal class OrdenesMapsDbReq : RequestBase, IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_FECHA", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Date)]
        public DateTime Fecha { get; set; }
    }
}
