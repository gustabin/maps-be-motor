
namespace Isban.MapsMB.DataAccess.DBRequest
{
    using Isban.Common.Data;
    using Isban.MapsMB.DataAccess.Constantes;
    using Isban.Mercados.DataAccess.ConverterDBType;
    using Isban.Mercados.DataAccess.OracleClient;
    using Maps.DataAccess.Base;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    [ProcedureRequest("SP_ACTUALIZA_SOLICITUD", Package = "PKG_MAPS_SOLICITUD_ORDENES", Owner = Owner.Maps)]
    internal class ModificarSolicitudOrdenDbReq : RequestBase, IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ID_SOLICITUD", BindOnNull = true, Type = OracleDbType.Long)]
        public long IdSolicitudOrdenes { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ESTADO", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Estado { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_NU_CERTIFICADO", BindOnNull = true, Type = OracleDbType.Long)]
        public long NumOrdenOrigen { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_OBSERVACION", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Observacion { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_DE_DISCLAIMER", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string TextoDisclaimer { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_TP_DISCLAIMER", BindOnNull = true, Type = OracleDbType.Long)]
        public long? TipoDisclaimer { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_NU_EVA_RIESGO", BindOnNull = true, Type = OracleDbType.Long)]
        public long IdEvaluacion { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SALDO_ENVIADO", BindOnNull = true, Type = OracleDbType.Decimal)]
        public decimal? SaldoEnviado { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_COD_BAJA_ADHESION", BindOnNull = true, Type = OracleDbType.Long)]
        public long? CodBajaAdhesion { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SALDO_CUENTA_ANTES", BindOnNull = true, Type = OracleDbType.Decimal)]
        public decimal? SaldoCuentaAntes { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SALDO_MIN_POR_FONDO", BindOnNull = true, Type = OracleDbType.Decimal)]
        public decimal? SaldoMinPorFondo { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SALDO_RESCATE_POR_FONDO", BindOnNull = true, Type = OracleDbType.Decimal)]
        public decimal? SaldoRescatePorFondo { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_CANT_INTENTOS", Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long?>))]
        public long? CantidadIntentos { get; set; }
    }
}
