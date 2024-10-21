namespace Isban.MapsMB.DataAccess.DBRequest
{
    using Constantes;
    using Isban.Common.Data;
    using Isban.Mercados.DataAccess.OracleClient;
    using Maps.DataAccess.Base;
    using Oracle.ManagedDataAccess.Client;
    using System;
    using System.Data;

    [ProcedureRequest("SP_ACTUALIZA_ORDENES", Package = "PKG_MAPS_TOOLS", Owner = Owner.Maps)]
    internal class ActualizarOrdenDbReq : RequestBase, IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_FECHA", BindOnNull = true, Type = OracleDbType.Date)]
        public DateTime Fecha { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_NUP", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Nup { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CUENTA", BindOnNull = true, Type = OracleDbType.Decimal)]
        public decimal Cuenta { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_NUM_ORDEN", BindOnNull = true, Type = OracleDbType.Decimal)]
        public decimal NumOrden { get; set; }
    }
}
