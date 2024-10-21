using Isban.Common.Data;
using Isban.Mercados.DataAccess.ConverterDBType;
using Isban.Mercados.DataAccess.OracleClient;

namespace Isban.MapsMB.DataAccess.DBResponse
{
    internal class SaldoCuentaDbResp : BaseResponse
    {

        [DBFieldDefinition(Name = "COD_MONEDA", ValueConverter = typeof(ResponseConvert<string>))]
        public string CodMoneda { get; set; }

        [DBFieldDefinition(Name = "CUENTA_TITULOS", ValueConverter = typeof(ResponseConvert<long>))]
        public long CuentaTitulos { get; set; }

        [DBFieldDefinition(Name = "NRO_CTA_OPER", ValueConverter = typeof(ResponseConvert<long>))]
        public long NumeroCuentaOperativa { get; set; }

        [DBFieldDefinition(Name = "NUP", ValueConverter = typeof(ResponseConvert<string>))]
        public string Nup { get; set; }

        [DBFieldDefinition(Name = "SALDO_ACTUAL", ValueConverter = typeof(ResponseConvert<decimal?>))]
        public decimal? SaldoEnCuenta { get; set; }

        [DBFieldDefinition(Name = "SEGMENTO", ValueConverter = typeof(ResponseConvert<string>))]
        public string Segmento { get; set; }

        [DBFieldDefinition(Name = "SUC_CTA_OPER", ValueConverter = typeof(ResponseConvert<long>))]
        public long SucuCtaOper { get; set; }

        [DBFieldDefinition(Name = "TIPO_CTA_OPER", ValueConverter = typeof(ResponseConvert<long>))]
        public long TipoCtaOper { get; set; }

        [DBFieldDefinition(Name = "OBSERVACION", ValueConverter = typeof(ResponseConvert<string>))]
        public string Observacion { get; set; }

        [DBFieldDefinition(Name = "ID_CUENTAS_OPERATIVAS", ValueConverter = typeof(ResponseConvert<long>))]
        public long Id { get; set; }
    }
}
