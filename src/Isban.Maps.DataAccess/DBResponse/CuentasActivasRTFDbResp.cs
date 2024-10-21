using Isban.Common.Data;
using Isban.Mercados.DataAccess.ConverterDBType;
using Isban.Mercados.DataAccess.OracleClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.DataAccess.DBResponse
{
    internal class CuentasActivasRTFDbResp : BaseResponse
    {
        [DBFieldDefinition(Name = "NUP", ValueConverter = typeof(ResponseConvert<string>))]
        public string Nup { get; set; }

        [DBFieldDefinition(Name = "CUENTA_TITULOS", ValueConverter = typeof(ResponseConvert<long>))]
        public long CuentaTitulo { get; set; }
    }
}
