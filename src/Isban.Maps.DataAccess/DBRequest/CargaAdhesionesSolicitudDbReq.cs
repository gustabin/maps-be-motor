namespace Isban.MapsMB.DataAccess.DBRequest
{
    using Constantes;
    using Isban.Common.Data;
    using Isban.Mercados.DataAccess.OracleClient;
    using Maps.DataAccess.Base;
    using Mercados.DataAccess.ConverterDBType;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    [ProcedureRequest("SP_CARGA_ADHESIONES_SOLICITUD", Package = "PKG_MAPS_SOLICITUD_ORDENES", Owner = Owner.Maps)]
    internal class CargaAdhesionesSolicitudDbReq : RequestBase, IProcedureRequest
    {    
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SERVICIO", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 10)]
        public string IdServicio { get; set; }
    }
}
