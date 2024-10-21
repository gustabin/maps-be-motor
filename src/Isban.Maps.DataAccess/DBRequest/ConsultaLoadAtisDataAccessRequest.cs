

namespace Isban.MapsMB.DataAccess.DBRequest
{
    using Constantes;
    using Isban.Common.Data;
    using Isban.Maps.DataAccess;
    using Isban.Mercados.DataAccess.OracleClient;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    [ProcedureRequest("LOAD_ATITS", Package = Package.BpServiciosA3, Owner = Owner.BCAINV)]
    public class ConsultaLoadAtisDataAccessRequest : BcainvRequestBase, IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CUENTA_BP", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Long)]
        public long? CuentaBp { get; set; }//ok

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_NUP", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Long)]
        public long? Nup { get; set; }//ok
    }

}
