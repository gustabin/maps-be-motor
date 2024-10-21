using Isban.Common.Data;
using Isban.Mercados.DataAccess.ConverterDBType;

namespace Isban.MapsMB.DataAccess.DBResponse
{
    public class AdhesionMepDbResp : AdhesionMepBaseDbResp
    {
        [DBFieldDefinition(Name = "COD_ALTA_ADHESION", ValueConverter = typeof(ResponseConvert<long>))]
        public long IdMaps { get; set; }
    }
}
