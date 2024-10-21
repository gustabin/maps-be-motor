using Isban.Common.Data;
using Isban.Mercados.DataAccess.OracleClient;
using Isban.Mercados.DataAccess.ConverterDBType;

namespace Isban.MapsMB.DataAccess.DBResponse
{
    internal class OrdenesMapsDbResp : BaseResponse
    {
        [DBFieldDefinition(Name = "ID_NUP", ValueConverter = typeof(ResponseConvert<string>))]
        public string Nup { get; set; }

        [DBFieldDefinition(Name = "CUENTA_TITULOS", ValueConverter = typeof(ResponseConvert<long>))]
        public long CuentaTitulos { get; set; }

        [DBFieldDefinition(Name = "COMPROBANTE_COMPRA", ValueConverter = typeof(ResponseConvert<string>))]
        public string ComprobanteCompra { get; set; }
    }
}
