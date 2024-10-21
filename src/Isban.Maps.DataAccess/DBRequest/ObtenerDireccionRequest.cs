using Isban.Common.Data;
using Isban.Maps.DataAccess.Base;
using Isban.MapsMB.DataAccess.Constantes;
using Isban.Mercados.DataAccess.OracleClient;
using Oracle.ManagedDataAccess.Client;
using System.Data;


namespace Isban.MapsMB.DataAccess.DBRequest
{
    [ProcedureRequest("SP_CONSULTA_DIRECCION ", Package = Package.OpicsMaps, Owner = Owner.Opics)]
    internal class ObtenerDireccionRequest : RequestBase, IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "p_nup", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Nup { get; set; }
    }
}
