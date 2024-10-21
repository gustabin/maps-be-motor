using Isban.Common.Data;
using Isban.Mercados.DataAccess.ConverterDBType;
using Isban.Mercados.DataAccess.OracleClient;
using System;

namespace Isban.MapsMB.DataAccess.DBResponse
{
    internal class ParametroResponse : BaseResponse
    {
        /// <summary>
        /// Gets or sets the estado.
        /// </summary>
        /// <value>
        /// The estado.
        /// </value>
        [DBFieldDefinition(Name = "NOM_PARAMETRO", ValueConverter = typeof(ResponseConvert<string>))]
        public string NombreParametro { get; set; }

        /// <summary>
        /// Gets or sets the descripcion.
        /// </summary>
        /// <value>
        /// The descripcion.
        /// </value>
        [DBFieldDefinition(Name = "DES_PARAMETRO", ValueConverter = typeof(ResponseConvert<string>))]
        public string Descripcion { get; set; }

        /// <summary>
        /// Gets or sets the valor.
        /// </summary>
        /// <value>
        /// The valor.
        /// </value>
        [DBFieldDefinition(Name = "VALOR", ValueConverter = typeof(ResponseConvert<string>))]
        public string Valor { get; set; }
    }
}

