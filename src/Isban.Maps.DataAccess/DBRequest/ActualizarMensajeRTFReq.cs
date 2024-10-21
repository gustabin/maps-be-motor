using Isban.Common.Data;
using Isban.Maps.DataAccess.Base;
using Isban.MapsMB.DataAccess.Constantes;
using Isban.Mercados.DataAccess.OracleClient;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.DataAccess.DBRequest
{
    [ProcedureRequest("SP_GRABA_MENSAJE_RESUMEN ", Package = Package.MapsTools, Owner = Owner.Maps)]
    internal class ActualizarMensajeRTFReq : RequestBase, IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CUENTA_TITULO", BindOnNull = true, Type = OracleDbType.Long)]
        public long Cuenta { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ESTADO_ENVIO", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Estado { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_OBSERVACION", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Observacion { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_NUP", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Nup { get; set; }

    }
}
