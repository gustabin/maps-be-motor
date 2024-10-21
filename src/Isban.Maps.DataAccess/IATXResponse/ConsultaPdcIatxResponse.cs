using Isban.Common.Data;
using Isban.Common.Data.Providers.IATX;

namespace Isban.PDC.Middleware.DataAccess.Response
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [IATXCollectionDefinition(TableName = "Output")]
    public class ConsultaPdcIatxResponse : Isban.Common.Data.IContract
    {
        [DBFieldDefinition(Name = "Cant_repeticiones")]
        public string Cant_repeticiones { get; set; }
        [IATXCollectionDefinition(TableName = "Repeticion")]
        public Repeticion[] Repeticion { get; set; }
    }

    [IATXCollectionDefinition(TableName = "Repeticion")]
    public class Repeticion : Isban.Common.Data.IContract
    {
        [DBFieldDefinition(Name = "FechaCompra")]
        public string FechaCompra { get; set; }
        [DBFieldDefinition(Name = "TipoCuenta")]
        public string TipoCuenta { get; set; }
        [DBFieldDefinition(Name = "Sucursal")]
        public string Sucursal { get; set; }
        [DBFieldDefinition(Name = "NumeroCuenta")]
        public string NumeroCuenta { get; set; }
        [DBFieldDefinition(Name = "CuentaTitulos")]
        public string CuentaTitulos { get; set; }
        [DBFieldDefinition(Name = "SaldoPdc")]
        public string SaldoPDC { get; set; }
    }
}