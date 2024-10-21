using Isban.Common.Data;
using Isban.Mercados.DataAccess.ConverterDBType;
using Isban.Mercados.DataAccess.OracleClient;
using System;

namespace Isban.MapsMB.DataAccess.DBResponse
{
    public class OperacionesDbResp : BaseResponse
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        /// <value>
        /// The estado.
        /// </value>
        [DBFieldDefinition(Name = "ID_OPERACION", ValueConverter = typeof(ResponseConvert<decimal>))]
        public decimal Id { get; set; }

        /// <summary>
        /// Gets or sets the Codigo.
        /// </summary>
        /// <value>
        /// The descripcion.
        /// </value>
        [DBFieldDefinition(Name = "COD_OPERACION", ValueConverter = typeof(ResponseConvert<string>))]
        public string Codigo { get; set; }

        /// <summary>
        /// Gets or sets the Descripcion.
        /// </summary>
        /// <value>
        /// The valor.
        /// </value>
        [DBFieldDefinition(Name = "DESC_OPERACION", ValueConverter = typeof(ResponseConvert<string>))]
        public string Descripcion { get; set; }
    }
}

