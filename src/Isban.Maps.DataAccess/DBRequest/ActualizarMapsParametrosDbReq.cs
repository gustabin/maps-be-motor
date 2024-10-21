using Isban.Common.Data;
using Isban.MapsMB.DataAccess.Constantes;
using Isban.Mercados.DataAccess.OracleClient;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Isban.MapsMB.DataAccess.DBRequest
{
    [ProcedureRequest("SP_ACTUALIZA_PARAMETROS", Package = Package.MapsParametros, Owner = Owner.Maps)]
    internal class ActualizarMapsParametrosDbReq : ObtenerParametroRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_VALOR", Size = 300, BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Valor { get; set; }

    }
}
