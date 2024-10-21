namespace Isban.MapsMB.DataAccess.DBRequest
{
    using Constantes;
    using Isban.Common.Data;
    using Isban.Mercados.DataAccess.OracleClient;
    using Maps.DataAccess.Base;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    [ProcedureRequest("SP_ACT_OP_DMEP_COMPRA", Package = "PKG_MAPS_TOOLS", Owner = Owner.Maps)]
    internal class AdhesionMEPCompraDbReq : RequestBase, IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ID_ADHESIONES", BindOnNull = true, Type = OracleDbType.Long)]
        public long IdAdhesion { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_COMPROBANTE_COMPRA", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string ComprobanteCompra { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CANTIDAD_NOMINALES", BindOnNull = true, Type = OracleDbType.Decimal)]
        public decimal CantidadNominales { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ESTADO", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Estado { get; set; }
    }
}
