using Isban.Common.Data;
using Isban.Mercados.DataAccess.OracleClient;
using Isban.Mercados.DataAccess.ConverterDBType;

namespace Isban.MapsMB.DataAccess.DBResponse
{
    public class AdhesionMepBaseDbResp : BaseResponse
    {
        [DBFieldDefinition(Name = "ID_ADHESIONES", ValueConverter = typeof(ResponseConvert<long?>))]
        public long? IdAdhesion { get; set; }

        [DBFieldDefinition(Name = "ID_NUP", ValueConverter = typeof(ResponseConvert<string>))]
        public string Nup { get; set; }

        [DBFieldDefinition(Name = "ID_SEGMENTO", ValueConverter = typeof(ResponseConvert<string>))]
        public string Segmento { get; set; }

        [DBFieldDefinition(Name = "CANAL", ValueConverter = typeof(ResponseConvert<string>))]
        public string Canal { get; set; }

        [DBFieldDefinition(Name = "SUBCANAL", ValueConverter = typeof(ResponseConvert<string>))]
        public string SubCanal { get; set; }

        [DBFieldDefinition(Name = "CUENTA_TITULOS", ValueConverter = typeof(ResponseConvert<long>))]
        public long CuentaTitulos { get; set; }

        [DBFieldDefinition(Name = "SUC_CTA_OPER", ValueConverter = typeof(ResponseConvert<long>))]
        public long SucCtaOper { get; set; }

        [DBFieldDefinition(Name = "NRO_CTA_OPER", ValueConverter = typeof(ResponseConvert<long>))]
        public long NroCtaOper { get; set; }

        [DBFieldDefinition(Name = "TIPO_CTA_OPER", ValueConverter = typeof(ResponseConvert<long>))]
        public long TipoCtaOper { get; set; }

        [DBFieldDefinition(Name = "IMPORTE_ENVIADO", ValueConverter = typeof(ResponseConvert<decimal?>))]
        public decimal? ImporteEnviado { get; set; }

        [DBFieldDefinition(Name = "COD_ESPECIE", ValueConverter = typeof(ResponseConvert<string>))]
        public string CodEspecie { get; set; }

        [DBFieldDefinition(Name = "ESTADO", ValueConverter = typeof(ResponseConvert<string>))]
        public string Estado { get; set; }

        [DBFieldDefinition(Name = "NU_DNI", ValueConverter = typeof(ResponseConvert<string>))]
        public string NumeroDocumento { get; set; }

        [DBFieldDefinition(Name = "FECHA_NACIMIENTO", ValueConverter = typeof(ResponseConvert<string>))]
        public string FechaNacimiento { get; set; }

    }
}
