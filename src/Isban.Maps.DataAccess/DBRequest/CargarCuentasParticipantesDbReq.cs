
using Isban.Common.Data;
using Isban.Maps.DataAccess.Base;
using Isban.MapsMB.DataAccess.Constantes;
using Isban.MapsMB.IDataAccess;
using Isban.Mercados;
using Isban.Mercados.DataAccess.ConverterDBType;
using Isban.Mercados.DataAccess.OracleClient;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Isban.MapsMB.DataAccess.DBRequest
{
    [ProcedureRequest("SP_INSERT_CUENTAS_PROC", Package = Package.MapsTools, Owner = Owner.Maps)]
    internal class CargarCuentasParticipantesDbReq : IProcedureRequest,IRequestBase
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CUENTA_TITULO", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long?>))]
        public long? CuentaTitulo { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_NRO_CTA_OPER", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long?>))]
        public long? CtaOperativa { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SUC_CTA_OPER", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long?>))]
        public long? SucCtaOperativa { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_TIPO_CTA_OPER", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long?>))]
        public long? TipoCtaOperativa { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_PRODUCTO_OPER", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 2)]
        public string ProductoOperativa { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SUBPRODUCTO_OPER", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 4)]
        public string SubproductoOperativa { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SUBPRODUCTO_ATIT", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 4)]
        public string SubproductoTitular { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_PRODUCTO_ATIT", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 2)]
        public string ProductoTit { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_PROCESADO", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 1)]
        public string Procesado { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_RELACION", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 1)]
        public string RelacionCtasOperativas { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CANT_PARTICIPANTES", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<int?>))]
        public int? CantParticipantes { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_COD_ALTA_ADHESION", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long>))]
        public long CodAltAdAhesion { get; set; }

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
        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_DESCR_ERR", Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 1000)]
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
