namespace Isban.MapsMB.DataAccess.DBRequest
{
    using Constantes;
    using Isban.Common.Data;
    using Isban.Mercados.DataAccess.OracleClient;
    using Maps.DataAccess.Base;
    using Oracle.ManagedDataAccess.Client;
    using System;
    using System.Data;

    [ProcedureRequest("SP_OBTENER_MENSAJES", Package = Package.MapsMyA, Owner = Owner.Maps)]
    internal class ObtenerMensajesDbReq : RequestBase, IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_FECHA", BindOnNull = true, Type = OracleDbType.Date)]
        public DateTime Fecha { get; set; }

        //[DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SERVICIO", Size =20, BindOnNull = true, Type = OracleDbType.Varchar2)]
        //public string IdServicio { get; set; }
    }
}
