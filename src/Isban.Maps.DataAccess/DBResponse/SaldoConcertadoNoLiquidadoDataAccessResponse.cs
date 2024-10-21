using Isban.Common.Data;
using Isban.Mercados.DataAccess.ConverterDBType;
using Isban.Mercados.DataAccess.OracleClient;
using System;

namespace Isban.MapsMB.DataAccess.DBResponse
{
    internal class SaldoConcertadoNoLiquidadoDataAccessResponse : BaseResponse
    {
        [DBFieldDefinition(Name = "SALDO", ValueConverter = typeof(ResponseConvert<decimal?>))]
        public decimal? Saldo { get; set; }
    }
}

