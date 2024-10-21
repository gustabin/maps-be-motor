using Isban.Common.Data;
using Isban.MapsMB.DataAccess.Constantes;
using Isban.Mercados.DataAccess.OracleClient;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Isban.MapsMB.DataAccess.DBRequest
{
    [ProcedureRequest("SP_ACTUALIZA_TRANSFERENCIA", Package = Package.Transferencia, Owner = "OPICS")]
    public class ActualizaTransferenciaReq : IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ESTADO", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Estado { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_LISTA_TRANSFERENCIAS", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string ListaTransferencia { get; set; }
    }
}
