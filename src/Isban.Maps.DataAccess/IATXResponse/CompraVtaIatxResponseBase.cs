namespace Isban.PDC.Middleware.DataAccess.Response
{
    using Isban.Common.Data;
    using Isban.Common.Data.Providers.IATX;
    [IATXCollectionDefinition(TableName = "Output")]
    public abstract class CompraVtaIatxResponseBase : IContract
    {
        [DBFieldDefinition(Name = "ImporteDebitoCredito")]
        public string ImporteDebitoCredito { get; set; }

        [DBFieldDefinition(Name = "Cantidad")]
        public string Cantidad { get; set; }

        [DBFieldDefinition(Name = "Cotizacion")]
        public string Cotizacion { get; set; }

    }
}
