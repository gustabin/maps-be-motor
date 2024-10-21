
using Isban.Common.Data;
using Isban.Maps.DataAccess.Base;
using Isban.MapsMB.DataAccess.Constantes;
using Isban.Mercados;
using Isban.Mercados.DataAccess.ConverterDBType;
using Isban.Mercados.DataAccess.OracleClient;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Isban.MapsMB.DataAccess.DBRequest
{
    [ProcedureRequest("sp_insert", Package = Package.MapsRtf, Owner = Owner.Maps)]
    internal class InsertarArchivoRTFDbReq : IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "p_nombre_ar", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Nombre { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "p_desc", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Descripcion { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "p_cuenta_tit", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Cuenta { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "p_path", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Path { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "p_anio", BindOnNull = true, Type = OracleDbType.Int32)]
        public int? Anio { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "p_nup", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Nup { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "p_batch", Size = 1, BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Batch { get; set; }

        /// <summary>
        /// Gets or sets the codigo error.
        /// </summary>
        /// <value>
        /// The codigo error.
        /// </value>
        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "p_cod_error", Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long?>))]
        public long? CodigoError { get; set; }
        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "p_desc_error", Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 1000)]
        public string Error { get; set; }
        /// <summary>
        /// Gets or sets the ip.
        /// </summary>
        /// <value>
        /// The ip.
        /// </value>
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

        /// <summary>
        /// Checks the error.
        /// </summary>
        /// <exception cref="DBCodeException"></exception>
        public void CheckError()
        {
            if (this.CodigoError.GetValueOrDefault() != 0)
            {
                throw new DBCodeException(this.CodigoError.GetValueOrDefault(), this.Error);
            }
        }


    }
}

