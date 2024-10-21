

namespace Isban.MapsMB.DataAccess.DBRequest
{
    using Constantes;
    using Isban.Common.Data;
    using Isban.Maps.DataAccess.Base;
    using Isban.Mercados.DataAccess.OracleClient;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    /// <summary>
    /// class ObtenerParametroRequest 
    /// </summary>
    /// <seealso cref="Isban.PDC.DataAccess.Requests.ResquestBase" />
    [ProcedureRequest("SP_CONSULTA_PARAMETROS", Package = Package.MapsParametros, Owner = Owner.Maps)]
    internal class ObtenerParametroRequest : RequestBase
    {
        /// <summary>
        /// Atributo: Parametro
        /// Tipo de Dato: string
        /// </summary>
        /// <value>
        /// The parametro.
        /// </value>
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_NOM_PARAMETRO", Size = 60, BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Parametro { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_COD_SISTEMA", Size = 4, BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Sistema { get; set; }
    }

}
