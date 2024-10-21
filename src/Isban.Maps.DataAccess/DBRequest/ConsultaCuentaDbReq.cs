using Isban.Common.Data;
using Isban.Maps.DataAccess.Base;
using Isban.MapsMB.DataAccess.Constantes;
using Isban.Mercados.DataAccess.OracleClient;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Isban.MapsMB.DataAccess.DBRequest
{
    [ProcedureRequest("SP_OBTENER_CUENTAS", Package = Package.MapsTools, Owner = Owner.Opics)]
    internal class ConsultaCuentaDbReq : RequestBase
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ID_CUENTAS_OPERATIVAS", BindOnNull = true, Type = OracleDbType.Long)]
        public long? Id { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SALDO_ACTUAL", BindOnNull = true, Type = OracleDbType.Decimal)]
        public decimal? SaldoActual { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Size = 20, Name = "P_OBSERVACION", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Observacion { get; set; }

    }
}
