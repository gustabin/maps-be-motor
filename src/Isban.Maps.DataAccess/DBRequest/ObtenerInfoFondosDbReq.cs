using Isban.Common.Data;
using Isban.Maps.DataAccess.Base;
using Isban.MapsMB.DataAccess.Constantes;
using Isban.Mercados.DataAccess.ConverterDBType;
using Isban.Mercados.DataAccess.OracleClient;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.DataAccess.DBRequest
{

    [ProcedureRequest("SP_OBTENER_INFO_FONDOS", Package = Package.MapsCommon, Owner = Owner.PL)]
    internal class ObtenerInfoFondosDbReq : IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CUENTA_TITULOS", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long>))]
        public long CuentaTitulo { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Size = 20, Name = "P_IP", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Ip { get; set; }
        /// <summary>
        /// Gets or sets the usuario.
        /// </summary>
        /// <value>
        /// The usuario.
        /// </value>
        [DBParameterDefinition(Direction = ParameterDirection.Input, Size = 20, Name = "P_USUARIO", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Usuario { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_COD_ERROR", Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long?>))]
        public long? CodigoError { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_DESC_ERROR", Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 4000)]
        public string Error { get; set; }
    }
}
