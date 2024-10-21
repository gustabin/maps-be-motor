
using Isban.Common.Data;
using Isban.Mercados.DataAccess.ConverterDBType;
using Isban.Mercados.DataAccess.OracleClient;
using Oracle.ManagedDataAccess.Client;
using System.Data;
namespace Isban.MapsMB.DataAccess.DBResponse
{
    [ProcedureRequest("SP_ALTA_ATIT_REPA", Package = "pkg_repatriacion", Owner = "OPICS")]
    public class AltaCuentaOpicsDBReq : IProcedureRequest
    {

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "xnumatit", Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long?>))]
        public long? CuentaTitulo { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Output, Size = 8, Name = "P_COD_RES", Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long?>))]
        public long? CodigoError { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_DESCR_ERR", Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 4000)]
        public string Error { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Size = 50, Name = "P_IP", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Ip { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Size = 50, Name = "P_USUARIO", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Usuario { get; set; }
    }
}
