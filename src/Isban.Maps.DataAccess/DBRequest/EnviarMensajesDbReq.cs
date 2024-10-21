namespace Isban.MapsMB.DataAccess.DBRequest
{
    using Constantes;
    using Isban.Common.Data;
    using Isban.Mercados.DataAccess.OracleClient;
    using Maps.DataAccess.Base;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    [ProcedureRequest("SP_ENVIAR_MENSAJES", Package = Package.MapsMyA, Owner = Owner.Maps)]
    internal class EnviarMensajesDbReq : RequestBase, IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_DATOS_CLIENTE", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string DatosCliente { get; set; }
    }
}
