
namespace Isban.MapsMB.DataAccess.DBRequest
{
    using Isban.Common.Data;
    using Isban.Maps.DataAccess;
    using Isban.MapsMB.DataAccess.Constantes;
    using Isban.MapsMB.IDataAccess;
    using Isban.Mercados.DataAccess.ConverterDBType;
    using Isban.Mercados.DataAccess.OracleClient;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    [ProcedureRequest("EJECUTAR_FONDOS", Package = Package.BpServiciosA3, Owner = Owner.BCAINV)]
    internal class EjecutarAltaFondosDbReq : BcainvRequestBase
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_TIPO", BindOnNull = true, Size = 2, Type = OracleDbType.Varchar2)]
        public string Tipo { get; set; }
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CUENTA", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Cuenta { get; set; }
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ESPECIE", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Especie { get; set; }
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_MONEDA", BindOnNull = true, Size = 3, Type = OracleDbType.Varchar2)]
        public string Moneda { get; set; }
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CUOTAS_PARTES", BindOnNull = true, Type = OracleDbType.Decimal)]
        public decimal? Cuotapartes { get; set; }
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CAPITAL", BindOnNull = true, Type = OracleDbType.Decimal)]
        public decimal? Capital { get; set; }
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ESPECIE_DESTINO", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string EspecieDestino { get; set; }
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_USU_4", BindOnNull = true, Size = 4, Type = OracleDbType.Varchar2)]
        public string UsuarioAsesor { get; set; }
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_USUARIO", BindOnNull = true, Size = 8, Type = OracleDbType.Varchar2)]
        public string UsuarioRacf { get; set; }
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_PASSWORD", BindOnNull = true, Size = 8, Type = OracleDbType.Varchar2)]
        public string PasswordRacf { get; set; }
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CANAL", BindOnNull = true, Type = OracleDbType.Int32)]
        public int Canal { get; set; }
        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_NUM_ORDEN", Type = OracleDbType.Varchar2, Size = 7, ValueConverter = typeof(RequestConvert<string>))]
        public string NumOrden { get; set; }
        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_NUM_CERTIFICADO_FM", Type = OracleDbType.Int32, ValueConverter = typeof(RequestConvert<int>))]
        public int NumCertificadoFm { get; set; }
        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_CUOTAS_PARTES_FM", Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<decimal>))]
        public decimal CuotapartesFm { get; set; }
        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_CAPITAL_FM", Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<decimal>))]
        public decimal CapitalFm { get; set; }
        [DBParameterDefinition(Direction = ParameterDirection.InputOutput, Name = "P_DENTRO_DEL_PERFIL", Size = 10, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>))]
        public string DentroDelPerfil { get; set; }
        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_DISCLAIMER", Type = OracleDbType.Varchar2, Size = 2000, ValueConverter = typeof(RequestConvert<string>))]
        public string Disclaimer { get; set; }
        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_COTIZACION", Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<decimal>))]
        public decimal Cotizacion { get; set; }
    }
}
