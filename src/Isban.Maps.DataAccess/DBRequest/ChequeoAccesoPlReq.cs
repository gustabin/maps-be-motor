﻿

namespace Isban.MapsMB.DataAccess.DBRequest
{
    using Constantes;
    using Isban.Common.Data;
    using Isban.Mercados.DataAccess.OracleClient;
    using Mercados.DataAccess.ConverterDBType;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    [ProcedureRequest("SP_GET_VARIABLES_ENTORNO", Package = Package.MapsHelp, Owner = Owner.Maps)]
        internal class ChequeoAccesoPlReq : IProcedureRequest
        {
            [DBParameterDefinition(Direction = ParameterDirection.Output, Size = 8, Name = "P_COD_ERROR", Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long?>))]
            public long? CodigoError { get; set; }
            [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_DESC_ERROR", Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 4000)]
            public string Error { get; set; }
            [DBParameterDefinition(Direction = ParameterDirection.Input, Size = 50, Name = "P_IP", BindOnNull = true, Type = OracleDbType.Varchar2)]
            public string Ip { get; set; }
            [DBParameterDefinition(Direction = ParameterDirection.Input, Size = 50, Name = "P_USUARIO", BindOnNull = true, Type = OracleDbType.Varchar2)]
            public string Usuario { get; set; }
        }
   
}
