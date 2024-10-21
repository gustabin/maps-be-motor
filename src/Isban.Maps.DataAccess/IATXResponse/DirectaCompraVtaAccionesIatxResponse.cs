namespace Isban.PDC.Middleware.DataAccess.Response
{
    using Common.Data;
    using Isban.Common.Data.Providers.IATX;
    [IATXCollectionDefinition(TableName = "Output")]
    public class DirectaCompraVtaAccionesIatxResponse : CompraVtaIatxResponseBase
    {
        [DBFieldDefinition(Name = "CotizacionLimite")]
        public string CotizacionLimite { get; set; }

        [DBFieldDefinition(Name = "FechaLiquidacion")]
        public string FechaLiquidacion { get; set; }

        [DBFieldDefinition(Name = "NumeroOrden")]
        public string NumeroOrden { get; set; }

        [DBFieldDefinition(Name = "PrecioClean")]
        public string PrecioClean { get; set; }
    }
}
