namespace Isban.MapsMB.DataAccess.DBRequest
{
    using Constantes;
    using Isban.Common.Data;
    using Isban.Mercados.DataAccess.OracleClient;
    using Maps.DataAccess.Base;
    using Oracle.ManagedDataAccess.Client;
    using System;
    using System.Data;

    [ProcedureRequest("SP_OBTENER_OP_VMEP_COMPRA_FIFO", Package = "PKG_MAPS_TOOLS", Owner = Owner.Maps)]
    internal class ObtenerAdhesionesMEPInversaCompraDbReq : RequestBase, IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_FECHA", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Date)]
        public DateTime Fecha { get; set; }
    }
}
