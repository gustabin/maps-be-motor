using Isban.Common.Data;
using Isban.Common.Data.Providers.IATX;

namespace Isban.PDC.Middleware.DataAccess.Response
{
    [IATXCollectionDefinition(TableName = "Output")]
    public class RescateFCIBPIATXResponse : Isban.Common.Data.IContract
    {
        [DBFieldDefinition(Name = "Descripcion_Moneda")]
        public string Descripcion_Moneda { get; set; }

        [DBFieldDefinition(Name = "Estado_Actual")]
        public string Estado_Actual { get; set; }

        [DBFieldDefinition(Name = "Motivo_Actual")]
        public string Motivo_Actual { get; set; }

        [DBFieldDefinition(Name = "Nombre_fondo")]
        public string Nombre_fondo { get; set; }

        [DBFieldDefinition(Name = "Importe_rescate")]
        public string Importe_rescate { get; set; }

        [DBFieldDefinition(Name = "Total_cuotas_Partes")]
        public string Total_cuotas_Partes { get; set; }

        [DBFieldDefinition(Name = "Monto_Cambio")]
        public string Monto_Cambio { get; set; }

        [DBFieldDefinition(Name = "Cotacao_pact")]
        public string Cotacao_pact { get; set; }

        [DBFieldDefinition(Name = "Numero_de_Certificado")]
        public string Numero_de_Certificado { get; set; }

    }
}