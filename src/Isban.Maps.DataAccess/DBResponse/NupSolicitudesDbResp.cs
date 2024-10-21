using Isban.Common.Data;
using Isban.Mercados.DataAccess.OracleClient;
using Isban.Mercados.DataAccess.ConverterDBType;
using System;

namespace Isban.MapsMB.DataAccess.DBResponse
{
    internal class NupSolicitudesDbResp : BaseResponse
    {
        [DBFieldDefinition(Name = "ID_NUP", ValueConverter = typeof(ResponseConvert<string>))]
        public string Nup { get; set; }

        [DBFieldDefinition(Name = "ID_SEGMENTO", ValueConverter = typeof(ResponseConvert<string>))]
        public string Segmento { get; set; }

        [DBFieldDefinition(Name = "CANAL", ValueConverter = typeof(ResponseConvert<string>))]
        public string Canal { get; set; }

        [DBFieldDefinition(Name = "SUBCANAL", ValueConverter = typeof(ResponseConvert<string>))]
        public string SubCanal { get; set; }
    }
}
