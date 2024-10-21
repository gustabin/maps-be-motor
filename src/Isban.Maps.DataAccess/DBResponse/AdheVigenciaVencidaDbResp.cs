using Isban.Common.Data;
using Isban.Mercados.DataAccess.OracleClient;
using Isban.Mercados.DataAccess.ConverterDBType;
using System;

namespace Isban.MapsMB.DataAccess.DBResponse
{
    internal class AdheVigenciaVencidaDbResp : BaseResponse
    {
        [DBFieldDefinition(Name = "COD_ALTA_ADHESION", ValueConverter = typeof(ResponseConvert<long>))]
        public long CodAltaAdhesion { get; set; }

        [DBFieldDefinition(Name = "ID_NUP", ValueConverter = typeof(ResponseConvert<string>))]
        public string Nup { get; set; }

        [DBFieldDefinition(Name = "ID_SEGMENTO", ValueConverter = typeof(ResponseConvert<string>))]
        public string Segmento { get; set; }
    }
}
