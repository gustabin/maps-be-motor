

namespace Isban.MapsMB.DataAccess.DBRequest
{
    using Constantes;
    using Isban.Common.Data;
    using Isban.Maps.DataAccess;
    using Isban.Mercados.DataAccess.OracleClient;
    using Oracle.ManagedDataAccess.Client;
    using System;
    using System.Data;

    [ProcedureRequest("LOAD_SALDOS", Package = Package.BpOnlineBanking, Owner = Owner.BCAINV)]
    public class LoadSaldosDataAccessRequest : BcainvRequestBase, IProcedureRequest
    {

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CUENTA", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Varchar2, Size =20)]
        public string Cuenta { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_FECHA_DESDE", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Date)]
        public DateTime FechaDesde { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_FECHA_HASTA", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Date)]
        public DateTime FechaHasta { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CANAL", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Varchar2, Size =3)]
        public string Canal { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_USUARIO", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Varchar2, Size = 20)]
        public string Usuario { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_PASSWORD", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Varchar2, Size = 20)]
        public string Password { get; set; }

        public override void CheckError()
        {
            //base.CheckError();
            //if (!Retorno.ToLower().Equals("ok"))
            //{
            //    throw new BusinessException("Saldo insuficiente", new Exception("ADB-00001-Cuenta Operativa Sin Saldo"));
            //}

        }
    }

}
