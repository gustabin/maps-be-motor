namespace Isban.MapsMB.DataAccess.DBRequest
{
    using Constantes;
    using Isban.Common.Data;
    using Isban.Mercados.DataAccess.OracleClient;
    using Maps.DataAccess.Base;
    using Oracle.ManagedDataAccess.Client;
    using System;
    using System.Data;

    [ProcedureRequest("SP_INSERT_DISCL_MEP_COMPRA", Package = "PKG_MAPS_TOOLS", Owner = Owner.Maps)]
    internal class InsertarDisclaimerEriMEPCompraDbReq : RequestBase, IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ID_ADHESIONES", BindOnNull = true, Type = OracleDbType.Long)]
        public long IdAdhesion { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_COMPROBANTE_COMPRA", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string ComprobanteCompra { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CANTIDAD_NOMINALES", BindOnNull = true, Type = OracleDbType.Decimal)]
        public decimal CantidadNominales { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ESTADO", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Estado { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CANT_DISCLAIMERS", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string CantidadDisclaimers { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ID_EVALUACION", BindOnNull = true, Type = OracleDbType.Long)]
        public long IdEvaluacion { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_TEXTO_DISCLAIMER", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string TextoDisclaimer { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_TIPO_DISCLAIMER", BindOnNull = true, Type = OracleDbType.Int16)]
        public short TipoDisclaimer { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ID_NUP", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Nup { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_FECHA_CONFIRM", BindOnNull = true, Type = OracleDbType.Date)]
        public DateTime FechaConfirmacionCompra { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_NU_DNI", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string NuDni { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_NOMBRE", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Nombre { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_APELLIDO", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Apellido { get; set; }
    }
}
