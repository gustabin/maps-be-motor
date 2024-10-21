using Isban.Common.Data;
using Isban.Common.Data.Providers.IATX;

namespace Isban.PDC.Middleware.DataAccess.Response
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [IATXCollectionDefinition(TableName = "Output")]
    public class ConsultaFCIResponse : Isban.Common.Data.IContract
    {
        [DBFieldDefinition(Name = "Status_Resultado_Extendido")]
        public string Status_Resultado_Extendido { get; set; }
        [DBFieldDefinition(Name = "Descripcion_Moneda")]
        public string Descripcion_Moneda { get; set; }
        public string Numero_Certificado { get; set; }
        public string Importe_Neto { get; set; }
        public string Porcentaje_Comis { get; set; }
        public string Valor_Comision { get; set; }
        public string Estado_Actual { get; set; }
        public string Motivo_Actual { get; set; }
        public string Cotizac_Cambio { get; set; }
        public string Dias_Carencia { get; set; }
        public string Nombre_fondo { get; set; }

    }
}