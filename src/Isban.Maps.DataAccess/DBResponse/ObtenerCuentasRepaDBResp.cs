using Isban.Common.Data;
using Isban.Mercados.DataAccess.ConverterDBType;
using Isban.Mercados.DataAccess.OracleClient;


namespace Isban.MapsMB.DataAccess.DBResponse
{
   internal class ObtenerCuentasRepaDBResp : BaseResponse
    {
        [DBFieldDefinition(Name = "ID_NUP", ValueConverter = typeof(ResponseConvert<string>))]
        public string Nup { get; set; }

        [DBFieldDefinition(Name = "COD_ALTA_ADHESION", ValueConverter = typeof(ResponseConvert<long?>))]
        public long? CodAltaAdhesion { get; set; }


        [DBFieldDefinition(Name = "CUENTA_TITULOS", ValueConverter = typeof(ResponseConvert<long?>))]
        public long? CuentaTitulo { get; set; }

        [DBFieldDefinition(Name = "NRO_CTA_OPER", ValueConverter = typeof(ResponseConvert<long?>))]
        public long? CtaOperativa { get; set; }

        [DBFieldDefinition(Name = "SUC_CTA_OPER", ValueConverter = typeof(ResponseConvert<long?>))]
        public long? SucursalOperativa { get; set; }

        [DBFieldDefinition(Name = "TIPO_CTA_OPER", ValueConverter = typeof(ResponseConvert<long?>))]
        public long? TipoOperativa { get; set; }


        [DBFieldDefinition(Name = "NRO_CTA_OPER_ACT", ValueConverter = typeof(ResponseConvert<long?>))]
        public long? CtaOperativaDolares { get; set; }

        [DBFieldDefinition(Name = "SUC_CTA_OPER_ACT", ValueConverter = typeof(ResponseConvert<long?>))]
        public long? SucursalOperativaDolares { get; set; }

        [DBFieldDefinition(Name = "TIPO_CTA_OPER_ACT", ValueConverter = typeof(ResponseConvert<long?>))]
        public long? TipoOperativaDolares { get; set; }
    }
}
