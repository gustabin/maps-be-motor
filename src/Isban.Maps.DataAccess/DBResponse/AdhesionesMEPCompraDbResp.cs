using Isban.Common.Data;
using Isban.Mercados.DataAccess.ConverterDBType;

namespace Isban.MapsMB.DataAccess.DBResponse
{
    internal class AdhesionesMepCompraDbResp : AdhesionMepBaseDbResp
    {
        [DBFieldDefinition(Name = "CANTIDAD_NOMINALES", ValueConverter = typeof(ResponseConvert<decimal?>))]
        public decimal? CantidadNominales { get; set; }

        [DBFieldDefinition(Name = "COD_ALTA_ADHESION", ValueConverter = typeof(ResponseConvert<long>))]
        public long CodAltaAdhesion { get; set; }
    }
}
