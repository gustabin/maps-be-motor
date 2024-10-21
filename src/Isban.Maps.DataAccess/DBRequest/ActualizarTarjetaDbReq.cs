namespace Isban.MapsMB.DataAccess.DBRequest
{
    using Constantes;
    using Isban.Common.Data;
    using Isban.Mercados.DataAccess.OracleClient;
    using Maps.DataAccess.Base;
    using Oracle.ManagedDataAccess.Client;
    using System;
    using System.Data;

    [ProcedureRequest("SP_ACTUALIZA_PARAMETRO_VMEP", Package = "PKG_MAPS_TOOLS", Owner = Owner.Maps)]
    internal class ActualizarTarjetaDbReq : RequestBase, IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_PARAMETRO", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Parametro { get; set; }
    }
}
