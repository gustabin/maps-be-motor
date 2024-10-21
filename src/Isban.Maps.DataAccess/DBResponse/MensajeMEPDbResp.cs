using Isban.Common.Data;
using Isban.Mercados.DataAccess.OracleClient;
using Isban.Mercados.DataAccess.ConverterDBType;
using System;

namespace Isban.MapsMB.DataAccess.DBResponse
{
    internal class MensajeMEPDbResp : BaseResponse
    {           
        [DBFieldDefinition(Name = "ID_ADHESIONES", ValueConverter = typeof(ResponseConvert<long>))]
        public long IdAhesion { get; set; }

        [DBFieldDefinition(Name = "NOMBRE", ValueConverter = typeof(ResponseConvert<string>))]
        public string Nombre { get; set; }

        [DBFieldDefinition(Name = "APELLIDO", ValueConverter = typeof(ResponseConvert<string>))]
        public string Apellido { get; set; }

        [DBFieldDefinition(Name = "NU_DNI", ValueConverter = typeof(ResponseConvert<string>))]
        public string IdDni { get; set; }

        [DBFieldDefinition(Name = "ID_SEGMENTO", ValueConverter = typeof(ResponseConvert<string>))]
        public string Segmento { get; set; }

        [DBFieldDefinition(Name = "ID_NUP", ValueConverter = typeof(ResponseConvert<string>))]
        public string Nup { get; set; }

        [DBFieldDefinition(Name = "CANAL", ValueConverter = typeof(ResponseConvert<string>))]
        public string Canal { get; set; }

        [DBFieldDefinition(Name = "NRO_CTA_OPER", ValueConverter = typeof(ResponseConvert<long>))]
        public long CuentaOperativa { get; set; }

        [DBFieldDefinition(Name = "CUENTA_TITULOS", ValueConverter = typeof(ResponseConvert<long>))]
        public long CuentaTitulo { get; set; }
    }
}
