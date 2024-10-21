namespace Isban.MapsMB.DataAccess.DBRequest
{
    using Constantes;
    using Isban.Common.Data;
    using Isban.Mercados.DataAccess.OracleClient;
    using Maps.DataAccess.Base;
    using Oracle.ManagedDataAccess.Client;
    using System;
    using System.Data;

    [ProcedureRequest("SP_OBTENER_MENSAJES", Package = Package.MapsMyAMEP, Owner = Owner.Maps)]
    internal class ObtenerMensajesMEPDbReq : RequestBase, IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_FECHA", BindOnNull = true, Type = OracleDbType.Date)]
        public DateTime Fecha { get; set; }
    }
}
