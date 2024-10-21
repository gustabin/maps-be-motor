
namespace Isban.MapsMB.DataAccess.DBRequest
{
    using Isban.Common.Data;
    using Isban.MapsMB.DataAccess.Constantes;
    using Isban.MapsMB.IDataAccess;
    using Isban.Mercados.DataAccess.ConverterDBType;
    using Isban.Mercados.DataAccess.OracleClient;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    [ProcedureRequest("SP_ACTUALIZA_EVA_RIESGO", Package = "PKG_MAPS_SOLICITUD_ORDENES", Owner = Owner.Maps)]
    internal class ActualizaEvaRiesgoDbReq : IProcedureRequest, IRequestBase// : BaseMapsRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ID_SOLICITUD", BindOnNull = true, Type = OracleDbType.Decimal)]
        public decimal IdSolicitud { get; set; }
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_NU_EVA_RIESGO", BindOnNull = true, Type = OracleDbType.Decimal)]
        public decimal NumEvaRiesgo { get; set; }
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_DE_DISCLAIMER", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string DescDisclaimer { get; set; }
        /// <summary>
        /// Gets or sets the codigo error.
        /// </summary>
        /// <value>
        /// The codigo error.
        /// </value>
        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_COD_RES", Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long?>))]
        public long? CodigoError { get; set; }
        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_DESCR_ERR", Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 4000)]
        public string Error { get; set; }
        /// <summary>
        /// Gets or sets the ip.
        /// </summary>
        /// <value>
        /// The ip.
        /// </value>
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_IP", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 20)]
        public string Ip { get; set; }
        /// <summary>
        /// Gets or sets the usuario.
        /// </summary>
        /// <value>
        /// The usuario.
        /// </value>
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_USUARIO", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 30)]
        public string Usuario { get; set; }


        public virtual void CheckError()
        {

        }
    }
}
