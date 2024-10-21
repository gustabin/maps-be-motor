namespace Isban.MapsMB.DataAccess.DBRequest
{
    using Constantes;
    using Isban.Common.Data;
    using Isban.Mercados.DataAccess.OracleClient;
    using Maps.DataAccess.Base;
    using Oracle.ManagedDataAccess.Client;
    using System;
    using System.Data;

    [ProcedureRequest("SP_OBTENER_TEXTOS", Package = Package.MapsMyA, Owner = Owner.Maps)]
    internal class ObtenerTextoDbReq : RequestBase, IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ENTITLEMENT", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Entitlement { get; set; }
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_COD_ESTADO", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string CodEstadoProceso { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_NUP", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Nup { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_COD_ALTA", BindOnNull = true, Type = OracleDbType.Long)]
        public long CodAlta { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SERVICIO", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string IdServicio { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_OPERACION", BindOnNull = true, Type = OracleDbType.Varchar2)]        
        public string Operacion { get; set; }
    }
}
