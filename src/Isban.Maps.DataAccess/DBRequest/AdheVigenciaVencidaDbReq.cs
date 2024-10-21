namespace Isban.MapsMB.DataAccess.DBRequest
{
    using Constantes;
    using Isban.Common.Data;
    using Isban.MapsMB.IDataAccess;
    using Isban.Mercados.DataAccess.ConverterDBType;
    using Isban.Mercados.DataAccess.OracleClient;
    using Maps.DataAccess.Base;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    [ProcedureRequest("SP_ADHE_VIGENCIA_VENCIDA", Package = "PKG_MAPS_ADHESIONES", Owner = Owner.Maps)]
    internal class AdheVigenciaVencidaDbReq : RequestBase, IProcedureRequest
    {

      
    }
}
