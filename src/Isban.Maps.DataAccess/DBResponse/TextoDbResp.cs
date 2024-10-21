using Isban.Common.Data;
using Isban.Mercados.DataAccess.OracleClient;
using Isban.Mercados.DataAccess.ConverterDBType;
using System;

namespace Isban.MapsMB.DataAccess.DBResponse
{
    internal class TextoDbResp : BaseResponse
    {           
        [DBFieldDefinition(Name = "DESC_TITULO_H", ValueConverter = typeof(ResponseConvert<string>))]
        public string Titulo { get; set; }

        [DBFieldDefinition(Name = "DESC_ASUNTO_H", ValueConverter = typeof(ResponseConvert<string>))]
        public string Asunto { get; set; }

        [DBFieldDefinition(Name = "DESC_TEXTO_MENSAJE_B", ValueConverter = typeof(ResponseConvert<string>))]
        public string TextoMensaje { get; set; }

        [DBFieldDefinition(Name = "COD_TP_MENSAJE_B", ValueConverter = typeof(ResponseConvert<string>))]
        public string TipoMensaje { get; set; }

        [DBFieldDefinition(Name = "TITULO", ValueConverter = typeof(ResponseConvert<string>))]
        public string TituloSolapa { get; set; }

        [DBFieldDefinition(Name = "DESCRIPCION", ValueConverter = typeof(ResponseConvert<string>))]
        public string Descripcion { get; set; }
    }
}
