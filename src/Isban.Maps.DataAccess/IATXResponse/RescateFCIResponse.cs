using Isban.Common.Data;
using Isban.Common.Data.Providers.IATX;

namespace Isban.PDC.Middleware.DataAccess.Response
{
    [IATXCollectionDefinition(TableName = "Output")]
    public class RescateFCIResponse : Isban.Common.Data.IContract
    {

        [DBFieldDefinition(Name = "Descripcion_Moneda")]
        public string Descripcion_Moneda { get; set; }

        [DBFieldDefinition(Name = "Estado_Actual")]
        public string Estado_Actual { get; set; }

        [DBFieldDefinition(Name = "Motivo_Actual")]
        public string Motivo_Actual { get; set; }

        [DBFieldDefinition(Name = "Nombre_fondo")]
        public string Nombre_fondo { get; set; }

        [DBFieldDefinition(Name = "Nro_Rescate")]
        public string Nro_Rescate { get; set; }

        [DBFieldDefinition(Name = "Importe_Rescate_Neto")]
        public string Importe_Rescate_Neto { get; set; }

        [DBFieldDefinition(Name = "Importe_Rescate_Comision")]
        public string Importe_Rescate_Comision { get; set; }

        [DBFieldDefinition(Name = "Importe_Rescate_Bruto")]
        public string Importe_Rescate_Bruto { get; set; }

        [DBFieldDefinition(Name = "Total_cuotas_partes")]
        public string Total_cuotas_partes { get; set; }

        [DBFieldDefinition(Name = "Monto_Cambio")]
        public string Monto_Cambio { get; set; }

        [DBFieldDefinition(Name = "Cotacao_pact")]
        public string Cotacao_pact { get; set; }

        [DBFieldDefinition(Name = "Marca_KU")]
        public string Marca_KU { get; set; }

    }
}