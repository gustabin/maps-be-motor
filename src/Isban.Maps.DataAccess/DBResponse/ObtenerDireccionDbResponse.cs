using Isban.Common.Data;
using Isban.Mercados.DataAccess.ConverterDBType;
using Isban.Mercados.DataAccess.OracleClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.DataAccess.DBResponse
{
    public class ObtenerDireccionDbResponse : BaseResponse
    {
        [DBFieldDefinition(Name = "COD_POS", ValueConverter = typeof(ResponseConvert<string>))]
        public string CodigoPostal { get; set; }

        [DBFieldDefinition(Name = "CALLE", ValueConverter = typeof(ResponseConvert<string>))]
        public string Calle { get; set; }

        [DBFieldDefinition(Name = "NRO", ValueConverter = typeof(ResponseConvert<string>))]
        public string Numero { get; set; }

        [DBFieldDefinition(Name = "PISO", ValueConverter = typeof(ResponseConvert<string>))]
        public string Piso { get; set; }

        [DBFieldDefinition(Name = "DEPTO", ValueConverter = typeof(ResponseConvert<string>))]
        public string Depto { get; set; }

        [DBFieldDefinition(Name = "JLOCAL", ValueConverter = typeof(ResponseConvert<string>))]
        public string Localidad { get; set; }
    }
}
