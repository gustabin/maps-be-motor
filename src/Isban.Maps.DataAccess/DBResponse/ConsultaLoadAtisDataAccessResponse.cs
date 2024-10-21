using Isban.Common.Data;
using Isban.Mercados.DataAccess.ConverterDBType;
using Isban.Mercados.DataAccess.OracleClient;
using System;

namespace Isban.MapsMB.DataAccess.DBResponse
{
    internal class ConsultaLoadAtisDataAccessResponse : BaseResponse
    {
        [DBFieldDefinition(Name = "CUENTA_BP", ValueConverter = typeof(ResponseConvert<long?>))]
        public long? CuentaBp { get; set; }

        [DBFieldDefinition(Name = "CUENTA_ATIT", ValueConverter = typeof(ResponseConvert<long?>))]
        public long? CuentaAtit { get; set; }

        [DBFieldDefinition(Name = "CNO", ValueConverter = typeof(ResponseConvert<long>))]
        public long Cno { get; set; }
    }
}

